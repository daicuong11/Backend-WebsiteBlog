using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;
using NewsWebAPI.Modals;

namespace NewsWebAPI.Repositorys.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(MyDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<User> Create(UserModal user)
        {
            var entityEntry = await _context.Users.AddAsync(_mapper.Map<User>(user));
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task Delete(UserModal user)
        {
            _context.Users.Remove(_mapper.Map<User>(user));
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
            return await _context.Users
                .Include(u => u.Articles)
                .ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users
                .Include(u => u.Articles)
                .SingleOrDefaultAsync(u => u.UserID == id);
        }

        public async Task Update(UserModal user)
        {
            _context.Users!.Update(_mapper.Map<User>(user));
            await _context.SaveChangesAsync();
        }
    }
}
