using Hubtel_App.Infrastructure;
using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Hubtel_App.DataProvider.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    internal DbSet<Users> _dbSet;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<Users>();
    }
    
    private async Task<UserDto> AddUser(Users user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return user.AsDto();
    }

    public async Task<UserDto> LoginUser(LoginUserDto usersPhoneNumber)
    {
        var user = await _db.Users.FirstOrDefaultAsync
            (x => x.UserNumber == usersPhoneNumber.UserNumber);

        if (user == null)
        {
            Users newUser = new Users
            {
                UserId = Guid.NewGuid(),
                UserNumber = usersPhoneNumber.UserNumber,
                NumberOfWallets = 0
            };
            AddUser(newUser);
            user = newUser;
        }
        return user.AsDto();
    }

    public async Task<Users> GetUser(string userPhoneNumber)
    {
        return await _db.Users.FirstOrDefaultAsync(x => x.UserNumber == userPhoneNumber);
    }
}