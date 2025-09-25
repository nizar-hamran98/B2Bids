namespace SharedKernel;

[AttributeUsage(AttributeTargets.Class)]
public class DbConnectionAttribute : Attribute
{
    public string ConnectionName { get; }
    public DbConnectionAttribute(string connectionName) => ConnectionName = connectionName;
}