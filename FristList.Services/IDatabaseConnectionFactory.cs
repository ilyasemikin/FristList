using System.Data.Common;

namespace FristList.Services;

public interface IDatabaseConnectionFactory
{
    DbConnection CreateConnection();
}