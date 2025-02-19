using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.DTOs;
using ProductsApi.Models;

namespace ProductsApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductsController(AppDbContext context) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<Product>>> GetProducts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            int skip = (pageNumber - 1) * pageSize;
            int totalCount = await context.Products.CountAsync(cancellationToken);
            var products = await context.Products
                                .AsNoTracking()
                                .OrderBy(p => p.CreatedOn)
                                .Skip(skip)
                                .Take(pageSize)
                                .ToListAsync(cancellationToken);
            return Ok(new PaginatedResponse<Product>
            {
                Items = products,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }
        catch (Exception ex)
        {
            return Ok(new PaginatedResponse<Product>
            {
                Items = [],
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 0,
                TotalPages = 0
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(
        Product product,
        CancellationToken cancellationToken)
    {
        product = product with
        {
            Id = Guid.NewGuid(),
            CreatedOn = DateTime.UtcNow,
        };
        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetProduct),
            new { id = product.Id }, product);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Product>> GetProduct(Guid id,  CancellationToken cancellationToken)
    {
        var product = await context.Products.AsNoTracking()
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if(product == null)
            return NotFound();
        return Ok(product);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProduct(
                            Guid id, 
                            Product product,
                            CancellationToken cancellationToken)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        var existingProduct = await context.Products.FindAsync([id], cancellationToken);
        if (existingProduct is null)
        {
             return NotFound();
        }
        var updateProduct = product with
        {
            CreatedOn = existingProduct.CreatedOn,
            UpdatedOn = DateTime.UtcNow
        };
        context.Entry(existingProduct).CurrentValues.SetValues(updateProduct);
        await context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        Product? product = await context.Products.FindAsync([id],cancellationToken);
        if (product is null)
        {
            return NotFound();
        }
        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

}
