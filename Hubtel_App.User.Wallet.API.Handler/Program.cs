using Hubtel_App.DataProvider;
using Hubtel_App.DataProvider.Repositories;
using Hubtel_App.DataProvider.Services;
using Hubtel_App.Infrastructure.Authentication;
using Hubtel_App.Infrastructure.CardSecurity;
using Hubtel_App.Infrastructure.RabbitMqBus;
using Hubtel_App.User.Wallet.Api.Handlers.WalletHandler;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddSingleton<IEncrypter, Encrypter>();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddScoped<IAuthenticationHandler, AuthenticationHandler>();

//register userHandlers
builder.Services.AddScoped<LoginUserhandler>();

//register walletHandlers
builder.Services.AddScoped<AddWalletHandler>();
builder.Services.AddScoped<DeleteWalletHandler>();
builder.Services.AddScoped<GetWalletHandler>();
builder.Services.AddScoped<GetAllWalletsHandler>();

//db
builder.Services.AddDbContext<ApplicationDbContext>(Options =>
    Options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Api Gateway
var rabbitMqOption = new RabbitMqOption();
builder.Configuration.GetSection("rabbitmq").Bind(rabbitMqOption);
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LoginUserhandler>();
    x.AddConsumer<AddWalletHandler>();
    x.AddConsumer<DeleteWalletHandler>();
    x.AddConsumer<GetAllWalletsHandler>();
    x.AddConsumer<GetWalletHandler>();
    
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        var rabbitMq = new RabbitMqOption();
        builder.Configuration.GetSection("rabbitmq").Bind(rabbitMq);
        cfg.Host(new Uri(rabbitMqOption.ConnectionString), hostconfig =>
        {
            hostconfig.Username(rabbitMqOption.Username);
            hostconfig.Password(rabbitMqOption.Password);
        });
        cfg.ReceiveEndpoint("add_wallet", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
            ep.ConfigureConsumer<AddWalletHandler>(provider);
        });
        cfg.ReceiveEndpoint("delete_wallet", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(retryConfig => { retryConfig.Interval(2, 100); });
            ep.ConfigureConsumer<DeleteWalletHandler>(provider);
        });
        cfg.ConfigureEndpoints(provider);
    }));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endPoints =>
{
    endPoints.MapControllers();
});
var busControl = app.Services.GetService<IBusControl>();
busControl.Start();
using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbInitializer.Initialize();
}

app.Run();