namespace FristList.Service.PublicApi.Filters.Exceptions;

public class CategoryAccessDeniedException : Exception
{
    public Guid CategoryId { get; }

    public CategoryAccessDeniedException(Guid categoryId, string? message = null)
        : base(message ?? $"Category {categoryId} access denied")
    {
        CategoryId = categoryId;
    }
}