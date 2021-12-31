using System.Text;

namespace FristList.Services.PostgreSql;

public class PostgreSqlConnectionStringBuilder
{
    private string? _host;
    private int? _port;
    private string? _database;
    private string? _username;
    private string? _password;

    public PostgreSqlConnectionStringBuilder WithHost(string value)
    {
        _host = value;
        return this;
    }

    public PostgreSqlConnectionStringBuilder WithPort(int value)
    {
        _port = value;
        return this;
    }

    public PostgreSqlConnectionStringBuilder WithDatabase(string value)
    {
        _database = value;
        return this;
    }

    public PostgreSqlConnectionStringBuilder WithUsername(string value)
    {
        _username = value;
        return this;
    }

    public PostgreSqlConnectionStringBuilder WithPassword(string value)
    {
        _password = value;
        return this;
    }

    private void AddOption(StringBuilder builder, string option, string value)
    {
        if (builder.Length > 0)
            builder.Append(';');
        builder.Append($"{option}={value}");
    }
    
    public string Build()
    {
        var builder = new StringBuilder();

        if (_host is not null)
            AddOption(builder, "Host", _host);
        if (_port is not null)
            AddOption(builder, "Port", _port.ToString()!);
        if (_database is not null)
            AddOption(builder, "Database", _database);
        if (_username is not null)
            AddOption(builder, "Username", _username);
        if (_password is not null)
            AddOption(builder, "Password", _password);

        return builder.ToString();
    }
}