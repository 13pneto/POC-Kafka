namespace Contracts;

public class BaseOrder
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public string Description { get; set; }
    public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;

    public BaseOrder(string description)
    {
        Description = description ;
    }
}