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
        public async Task<List<User>> GetAllPagination(string? searchTerm, int pageNumber, int pageSize, string sortOrder)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Username.Contains(searchTerm) || c.Name.Contains(searchTerm) || c.Email.Contains(searchTerm) || c.Role.Contains(searchTerm));
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(c => c.Username),
                "name" => query.OrderBy(c => c.Username),
                "id_desc" => query.OrderByDescending(c => c.UserID),
                _ => query.OrderBy(c => c.UserID),
            };

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(u => u.Articles).ToListAsync();
        }

        public async Task<int> Count()
        {
            return await _context.Users.CountAsync();
        }


        public async Task<User> GetById(int id)
        {
            return await _context.Users.AsNoTracking()
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
