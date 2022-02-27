using System;
using System.Collections.Generic;

namespace FristList.Services
{
    public sealed class RepositoryResult
    {
        public bool Succeeded { get; }
        public IEnumerable<RepositoryResultError> Errors { get; }
        
        public static RepositoryResult Success { get; }

        static RepositoryResult()
        {
            Success = new RepositoryResult(true, Array.Empty<RepositoryResultError>());
        }
        
        private RepositoryResult(bool succeeded, IEnumerable<RepositoryResultError> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public static RepositoryResult Failed(params RepositoryResultError[] errors)
            => new (false, errors);
    }
}