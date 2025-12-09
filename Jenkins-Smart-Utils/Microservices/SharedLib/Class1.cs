namespace SharedLib;

public class Response
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}
