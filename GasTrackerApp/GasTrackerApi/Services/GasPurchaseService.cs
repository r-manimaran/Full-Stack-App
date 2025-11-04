using GasTrackerApi.Models;
using GasTrackerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace GasTrackerApi.Services;

public class GasPurchaseService
{
    private readonly GasTrackerDbContext _context;

    public GasPurchaseService(GasTrackerDbContext context)
    {
        _context = context;
    }

    public decimal CalculateTotalPrice(decimal pricePerGallon, decimal gallonsPurchased)
    {
        return pricePerGallon * gallonsPurchased;
    }

    public async Task<GasPurchase> CreatePurchaseAsync(GasPurchase purchase)
    {
        purchase.TotalPrice = CalculateTotalPrice(purchase.PricePerGallon, purchase.GallonsPurchased);
        _context.GasPurchases.Add(purchase);
        await _context.SaveChangesAsync();
        return purchase;
    }

    public async Task<GasPurchase?> UpdatePurchaseAsync(int id, GasPurchase purchase)
    {
        var existingPurchase = await _context.GasPurchases.FindAsync(id);
        if (existingPurchase == null)
            return null;

        existingPurchase.PricePerGallon = purchase.PricePerGallon;
        existingPurchase.GallonsPurchased = purchase.GallonsPurchased;
        existingPurchase.DateAndTime = purchase.DateAndTime;
        existingPurchase.FuelStation = purchase.FuelStation;
        existingPurchase.TotalPrice = CalculateTotalPrice(purchase.PricePerGallon, purchase.GallonsPurchased);

        await _context.SaveChangesAsync();
        return existingPurchase;
    }

    public async Task<bool> DeletePurchaseAsync(int id)
    {
        var purchase = await _context.GasPurchases.FindAsync(id);
        if (purchase == null)
            return false;

        _context.GasPurchases.Remove(purchase);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<GasPurchase>> GetAllPurchasesAsync()
    {
        return await _context.GasPurchases
            .OrderByDescending(p => p.DateAndTime)
            .ToListAsync();
    }

    public async Task<GasPurchase?> GetPurchaseByIdAsync(int id)
    {
        return await _context.GasPurchases.FindAsync(id);
    }
}
