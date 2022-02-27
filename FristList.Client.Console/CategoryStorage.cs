using System.Collections.Generic;
using System.Linq;
using FristList.Data.Dto;

namespace FristList.Client.Console;

public class CategoryStorage
{
    private readonly IDictionary<int, Category> _categories;

    public CategoryStorage()
    { 
        _categories = new Dictionary<int, Category>();
    }

    public bool TryAdd(Category category)
    {
        var success = _categories.TryAdd(category.Id, category);

        return success;
    }

    public bool Remove(int id)
    {
        if (!_categories.ContainsKey(id))
            return false;
        
        _categories.Remove(id);
        return true;
    }

    public bool TryGet(int id, out Category? category)
        => _categories.TryGetValue(id, out category);

    public IEnumerable<Category> FindByIds(IEnumerable<int> ids)
        => ids.Select(id => _categories[id]);
}