using System.Collections;
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
            Success = new RepositoryResult(true, null);
        }
        
        private RepositoryResult(bool succeeded, IEnumerable<RepositoryResultError> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public static RepositoryResult Failed(params RepositoryResultError[] errors)
            => new RepositoryResult(false, errors);
    }
}