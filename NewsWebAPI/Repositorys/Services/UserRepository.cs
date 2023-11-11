using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;

        public UserRepository(MyDbContext context)
        {
            this._context = context;
        }

        public async Task<User> Create(User user)
        {
            var entityEntry = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task Delete(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> FindByPassWord(string passWord)
        {
            return await _context.Users.Where(u => u.Password == passWord).FirstOrDefaultAsync();
        }

        public async Task<User> FindByUserName(string userName)
        {
            return await _context.Users.Where(u => u.Username == userName).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task Update(User user)
        {
            _context.Users!.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
