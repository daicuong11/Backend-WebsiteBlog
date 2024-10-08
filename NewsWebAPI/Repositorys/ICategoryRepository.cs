﻿using NewsWebAPI.Entities;

namespace NewsWebAPI.Repositorys
{
    public interface ICategoryRepository 
    {
        Task<List<Category>> GetAll(string? searchTerm, int pageNumber, int pageSize, string sortOrder);
        Task<Category> FindOne(int id);
        Task<Category> Create(Category category);
        Task Update(Category category);
        Task Delete(Category category);
    }
}
