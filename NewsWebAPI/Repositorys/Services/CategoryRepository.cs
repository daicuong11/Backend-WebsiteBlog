using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;
using System.ComponentModel.Design;

namespace NewsWebAPI.Repositorys.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        public readonly MyDbContext _context;
        public CategoryRepository(MyDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<Category> Create(Category category)
        {

            var entityEntry = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task Delete(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> FindOne(int id)
        {
            return await _context.Categories.FindAsync(id);

        }

        public async Task<List<Category>> GetAll(string? searchTerm, int pageNumber, int pageSize, string sortOrder)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.CategoryName.Contains(searchTerm));
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(c => c.CategoryName),
                "name" => query.OrderBy(c => c.CategoryName),
                "id_desc" => query.OrderByDescending(c => c.CategoryID),
                _ => query.OrderBy(c => c.CategoryID),
            };

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }


        public async Task Update(Category category)
        {
            _context.Categories!.Update(category);
            await _context.SaveChangesAsync();

        }
    }
}
