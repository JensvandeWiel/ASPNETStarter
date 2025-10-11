namespace ASPNETStarter.Server.Seeders;

[AttributeUsage(AttributeTargets.Class)]
public class SeederAttribute : Attribute
{
    public SeederAttribute(int order = 0)
    {
        Order = order;
    }

    public int Order { get; }
}