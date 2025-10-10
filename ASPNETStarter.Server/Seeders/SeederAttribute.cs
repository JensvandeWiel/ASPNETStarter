namespace ASPNETStarter.Server.Seeders;

[AttributeUsage(AttributeTargets.Class)]
public class SeederAttribute : Attribute
{
    public int Order { get; }

    public SeederAttribute(int order = 0)
    {
        Order = order;
    }
}
