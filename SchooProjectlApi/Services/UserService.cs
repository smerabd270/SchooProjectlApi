using Microsoft.EntityFrameworkCore;
using SchooProjectlApi.Data;
using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services
{
    public class UserService : IUserService
    {
        private readonly SchoolContext _context;
        public UserService(SchoolContext context) => _context = context;

        public async Task<IEnumerable<User>> GetAllUsersAsync() =>
            await _context.Users.ToListAsync();

        public async Task<User?> GetUserByIdAsync(int id) =>
            await _context.Users.FindAsync(id);

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
