using System.Linq.Expressions;
using System.Text.RegularExpressions;
using FristList.Service.Data;
using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Categories;
using FristList.Service.Data.Models.Categories.Base;
using FristList.Service.PublicApi.Services.Abstractions.Categories;
using FristList.Service.PublicApi.Services.Models;
using FristList.Service.PublicApi.Services.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace FristList.Service.PublicApi.Services.Implementations.Categories;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _dbContext;

    public CategoryService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddCategoryAsync(BaseCategory category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(BaseCategory category)
    {
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category is null)
            return;
        await DeleteCategoryAsync(category);
    }

    public async Task<BaseCategory?> GetCategoryAsync(Guid categoryId)
    {
        var category = await _dbContext.Categories.FindAsync(categoryId);
        if (category is null)
            return null;
        if (category is PersonalCategory personalCategory)
            await _dbContext.Entry(personalCategory).Navigation("Owner").LoadAsync();
        return category;
    }

    public async Task<IEnumerable<BaseCategory>> GetCategoriesAsync(IEnumerable<Guid> categoryIds)
    {
        var categoryIdsSet = categoryIds.ToHashSet();
        return await _dbContext.Categories.Where(c => categoryIdsSet.Contains(c.Id))
            .ToListAsync();
    }

    public async Task<IEnumerable<BaseCategory>> GetCategoriesAvailableToUserAsync(Guid userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user is null)
            return Enumerable.Empty<BaseCategory>();
        return await GetCategoriesAvailableToUserAsync(user);
    }

    public async Task<IEnumerable<BaseCategory>> GetCategoriesAvailableToUserAsync(User user)
    {
        var personalCategories = await _dbContext.PersonalCategories.Where(c => c.Owner.Id == user.Id)
            .ToListAsync();
        return personalCategories;
    }

    public Task<IEnumerable<BaseCategory>> GetCategoriesAvailableToUserAsync(User user, CategorySearchParams @params)
    {
        IQueryable<BaseCategory> categories = _dbContext.PersonalCategories
            .Include(c => c.Owner)
            .Where(c => c.Owner.Id == user.Id);

        if (@params.NamePattern is not null)
            categories = categories.Where(c => Regex.IsMatch(c.Name, @params.NamePattern));

        Expression<Func<BaseCategory, dynamic>> orderKeySelector = @params.SortField switch
        {
            CategorySearchSortField.Name => c => c.Name,
            _ => c => c.Id
        };

        categories = @params.SortOrder switch
        {
            SortOrder.Ascending => categories.OrderBy(orderKeySelector),
            SortOrder.Descending => categories.OrderByDescending(orderKeySelector),
            _ => throw new IndexOutOfRangeException(nameof(SortOrder))
        };

        return Task.FromResult<IEnumerable<BaseCategory>>(categories);
    }

    public async Task<bool> IsCategoryAvailableToUserAsync(Guid userId, Guid categoryId)
    {
        var category = await GetCategoryAsync(categoryId);
        if (category is not PersonalCategory personalCategory)
            return false;
        return personalCategory.Owner.Id == userId;
    }
}