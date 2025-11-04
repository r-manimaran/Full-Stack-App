using Microsoft.AspNetCore.Mvc;
using GasTrackerApi.Models;
using GasTrackerApi.Services;

namespace GasTrackerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GasPurchasesController : ControllerBase
{
    private readonly GasPurchaseService _service;

    public GasPurchasesController(GasPurchaseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GasPurchase>>> GetGasPurchases()
    {
        var purchases = await _service.GetAllPurchasesAsync();
        return Ok(purchases);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GasPurchase>> GetGasPurchase(int id)
    {
        var purchase = await _service.GetPurchaseByIdAsync(id);
        if (purchase == null)
        {
            return NotFound();
        }
        return Ok(purchase);
    }

    [HttpPost]
    public async Task<ActionResult<GasPurchase>> CreateGasPurchase(GasPurchase purchase)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdPurchase = await _service.CreatePurchaseAsync(purchase);
        return CreatedAtAction(nameof(GetGasPurchase), new { id = createdPurchase.Id }, createdPurchase);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGasPurchase(int id, GasPurchase purchase)
    {
        if (id != purchase.Id)
        {
            return BadRequest();
        }

        var updatedPurchase = await _service.UpdatePurchaseAsync(id, purchase);
        if (updatedPurchase == null)
        {
            return NotFound();
        }

        return Ok(updatedPurchase);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGasPurchase(int id)
    {
        var result = await _service.DeletePurchaseAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
