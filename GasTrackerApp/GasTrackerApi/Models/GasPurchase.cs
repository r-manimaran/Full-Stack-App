namespace GasTrackerApi.Models;

public class GasPurchase
{
    public int Id { get; set; }
    public decimal PricePerGallon { get; set; }
    public decimal GallonsPurchased { get; set; }
    public DateTime DateAndTime { get; set; }
    public decimal TotalPrice { get; set; }
    public string FuelStation { get; set; } = string.Empty;
}
