using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GasTrackerApi.Data;
using GasTrackerApi.Models;

namespace GasTrackerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly GasTrackerDbContext _context;

    public AnalyticsController(GasTrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetSummary()
    {
        var purchases = await _context.GasPurchases.ToListAsync();

        if (!purchases.Any())
        {
            return Ok(new
            {
                totalSpent = 0,
                totalGallons = 0,
                averagePricePerGallon = 0,
                totalPurchases = 0,
                averageGallonsPerPurchase = 0
            });
        }

        var totalSpent = purchases.Sum(p => p.TotalPrice);
        var totalGallons = purchases.Sum(p => p.GallonsPurchased);
        var averagePricePerGallon = purchases.Average(p => p.PricePerGallon);
        var totalPurchases = purchases.Count;
        var averageGallonsPerPurchase = purchases.Average(p => p.GallonsPurchased);

        return Ok(new
        {
            totalSpent = Math.Round(totalSpent, 2),
            totalGallons = Math.Round(totalGallons, 2),
            averagePricePerGallon = Math.Round(averagePricePerGallon, 2),
            totalPurchases,
            averageGallonsPerPurchase = Math.Round(averageGallonsPerPurchase, 2)
        });
    }

    [HttpGet("pricestats")]
    public async Task<ActionResult<object>> GetPriceStats()
    {
        var purchases = await _context.GasPurchases.ToListAsync();

        if (!purchases.Any())
        {
            return Ok(new
            {
                minPrice = (decimal?)null,
                maxPrice = (decimal?)null,
                minPricePurchase = (GasPurchase?)null,
                maxPricePurchase = (GasPurchase?)null
            });
        }

        var minPurchase = purchases.OrderBy(p => p.PricePerGallon).First();
        var maxPurchase = purchases.OrderByDescending(p => p.PricePerGallon).First();

        return Ok(new
        {
            minPrice = Math.Round(minPurchase.PricePerGallon, 2),
            maxPrice = Math.Round(maxPurchase.PricePerGallon, 2),
            minPricePurchase = minPurchase,
            maxPricePurchase = maxPurchase
        });
    }

    [HttpGet("trends")]
    public async Task<ActionResult<object>> GetTrends([FromQuery] string? period = "daily")
    {
        var purchases = await _context.GasPurchases
            .OrderBy(p => p.DateAndTime)
            .ToListAsync();

        if (!purchases.Any())
        {
            return Ok(new List<object>());
        }

        object trends;

        if (period == "monthly")
        {
            trends = purchases
                .GroupBy(p => new { p.DateAndTime.Year, p.DateAndTime.Month })
                .Select(g => new
                {
                    label = $"{g.Key.Year}-{g.Key.Month:D2}",
                    averagePrice = Math.Round(g.Average(p => p.PricePerGallon), 2),
                    totalSpent = Math.Round(g.Sum(p => p.TotalPrice), 2),
                    totalGallons = Math.Round(g.Sum(p => p.GallonsPurchased), 2),
                    count = g.Count()
                })
                .OrderBy(t => t.label)
                .ToList();
        }
        else if (period == "yearly")
        {
            trends = purchases
                .GroupBy(p => p.DateAndTime.Year)
                .Select(g => new
                {
                    label = g.Key.ToString(),
                    averagePrice = Math.Round(g.Average(p => p.PricePerGallon), 2),
                    totalSpent = Math.Round(g.Sum(p => p.TotalPrice), 2),
                    totalGallons = Math.Round(g.Sum(p => p.GallonsPurchased), 2),
                    count = g.Count()
                })
                .OrderBy(t => t.label)
                .ToList();
        }
        else // daily
        {
            trends = purchases
                .Select(p => new
                {
                    label = p.DateAndTime.ToString("yyyy-MM-dd"),
                    price = Math.Round(p.PricePerGallon, 2),
                    total = Math.Round(p.TotalPrice, 2),
                    gallons = Math.Round(p.GallonsPurchased, 2),
                    station = p.FuelStation,
                    date = p.DateAndTime
                })
                .ToList();
        }

        return Ok(trends);
    }

    [HttpGet("bystation")]
    public async Task<ActionResult<object>> GetByStation()
    {
        var purchases = await _context.GasPurchases.ToListAsync();

        if (!purchases.Any())
        {
            return Ok(new object[0]);
        }

        var byStation = purchases
            .GroupBy(p => p.FuelStation)
            .Select(g => new
            {
                station = g.Key,
                totalSpent = Math.Round(g.Sum(p => p.TotalPrice), 2),
                totalGallons = Math.Round(g.Sum(p => p.GallonsPurchased), 2),
                averagePricePerGallon = Math.Round(g.Average(p => p.PricePerGallon), 2),
                purchaseCount = g.Count(),
                lastPurchaseDate = g.Max(p => p.DateAndTime)
            })
            .OrderByDescending(s => s.totalSpent)
            .ToList();

        return Ok(byStation);
    }

    [HttpGet("monthly")]
    public async Task<ActionResult<object>> GetMonthly()
    {
        var purchases = await _context.GasPurchases.ToListAsync();

        if (!purchases.Any())
        {
            return Ok(new object[0]);
        }

        var monthly = purchases
            .GroupBy(p => new { p.DateAndTime.Year, p.DateAndTime.Month })
            .Select(g => new
            {
                year = g.Key.Year,
                month = g.Key.Month,
                label = $"{g.Key.Year}-{g.Key.Month:D2}",
                totalSpent = Math.Round(g.Sum(p => p.TotalPrice), 2),
                totalGallons = Math.Round(g.Sum(p => p.GallonsPurchased), 2),
                averagePricePerGallon = Math.Round(g.Average(p => p.PricePerGallon), 2),
                purchaseCount = g.Count()
            })
            .OrderBy(m => m.year)
            .ThenBy(m => m.month)
            .ToList();

        return Ok(monthly);
    }

    [HttpGet("yearly")]
    public async Task<ActionResult<object>> GetYearly()
    {
        var purchases = await _context.GasPurchases.ToListAsync();

        if (!purchases.Any())
        {
            return Ok(new object[0]);
        }

        var yearly = purchases
            .GroupBy(p => p.DateAndTime.Year)
            .Select(g => new
            {
                year = g.Key,
                totalSpent = Math.Round(g.Sum(p => p.TotalPrice), 2),
                totalGallons = Math.Round(g.Sum(p => p.GallonsPurchased), 2),
                averagePricePerGallon = Math.Round(g.Average(p => p.PricePerGallon), 2),
                purchaseCount = g.Count()
            })
            .OrderBy(y => y.year)
            .ToList();

        return Ok(yearly);
    }
}
