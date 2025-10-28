using Application.Abstractions;
using Asp.Versioning;
using Domain.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IAppDbContext _db;

    public CustomersController(IAppDbContext db)
    {
        _db = db;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "read")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var entity = await _db.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity == null) return NotFound();
        return Ok(new { entity.Id, entity.Email });
    }

    [HttpPost]
    [Authorize(Policy = "write")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest(new { message = "email required" });

        var entity = new Customer(Guid.NewGuid(), request.Email);
        await _db.Customers.AddAsync(entity, ct);
        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id, version = "1" },
            new { entity.Id, entity.Email });
    }

    public sealed record CreateCustomerRequest(string Email);
}
