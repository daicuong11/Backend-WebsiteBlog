using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NewsWebAPI.Data;
using NewsWebAPI.Entities;
using NewsWebAPI.Migrations;
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

        public async Task<List<Category>> getAll(string? s,int pageNumber, int pageSize, string sortOrder)
        {
            var query = from q in _context.Categories
                        select q;
            if (!String.IsNullOrEmpty(s))
            {
                query = query.Where(b => b.CategoryName.Contains(s));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(s => s.CategoryName);
                    break;
                case "name":
                    query = query.OrderBy(s => s.CategoryName);
                    break;
                case "id_desc":
                    query = query.OrderByDescending(s => s.CategoryID);
                    break;
                default:
                    query = query.OrderBy(s => s.CategoryID);
                    break;
            }

            return await query.Skip((pageNumber -1)* pageSize).Take(pageSize).ToListAsync();
        }

        public async Task Update(Category category)
        {
            _context.Categories!.Update(category);
            await _context.SaveChangesAsync();

        }
    }
}
