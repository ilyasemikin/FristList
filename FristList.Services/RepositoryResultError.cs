using System;

namespace FristList.Services
{
    public class RepositoryResultError
    {
        public string Description { get; init; }

        public RepositoryResultError()
        {
            Description = string.Empty;
        }
    }
}