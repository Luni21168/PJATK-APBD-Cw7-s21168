using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcWarehoseApi.DTOs;
using PcWarehouseApi.Data;
using PcWarehouseApi.DTOs;

namespace PcWarehouseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PcsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PcsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PcListDto>>> GetAll()
    {
        var pcs = await _context.PCs
            .OrderBy(p => p.Id)
            .Select(p => new PcListDto
            {
                Id = p.Id,
                Name = p.Name,
                Weight = p.Weight,
                Warranty = p.Warranty,
                CreatedAt = p.CreatedAt,
                Stock = p.Stock
            })
            .ToListAsync();

        return Ok(pcs);
    }

    [HttpGet("{id:int}/components")]
    public async Task<ActionResult<PcWithComponentsDto>> GetByIdWithComponents([FromRoute] int id)
    {
        var pc = await _context.PCs
            .Include(p => p.PCComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.ComponentManufacturer)
            .Include(p => p.PCComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.ComponentType)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pc == null)
        {
            return NotFound(new ErrorResponseDto
            {
                Message = $"PC with id {id} was not found."
            });
        }

        var result = new PcWithComponentsDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock,
            Components = pc.PCComponents
                .OrderBy(x => x.ComponentCode)
                .Select(x => new PcAssignedComponentDto
                {
                    Amount = x.Amount,
                    Component = new ComponentDetailsDto
                    {
                        Code = x.Component.Code,
                        Name = x.Component.Name,
                        Description = x.Component.Description,
                        Manufacturer = new ManufacturerDto
                        {
                            Id = x.Component.ComponentManufacturer.Id,
                            Abbreviation = x.Component.ComponentManufacturer.Abbreviation,
                            FullName = x.Component.ComponentManufacturer.FullName,
                            FoundationDate = x.Component.ComponentManufacturer.FoundationDate
                        },
                        Type = new ComponentTypeDto
                        {
                            Id = x.Component.ComponentType.Id,
                            Abbreviation = x.Component.ComponentType.Abbreviation,
                            Name = x.Component.ComponentType.Name
                        }
                    }
                })
                .ToList()
        };

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<PcListDto>> Create([FromBody] UpsertPcDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "Name is required."
            });
        }

        if (request.Weight <= 0)
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "Weight must be greater than 0."
            });
        }

        if (request.Warranty < 0)
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "Warranty cannot be negative."
            });
        }

        if (request.Stock < 0)
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "Stock cannot be negative."
            });
        }

        var pc = new Models.PC
        {
            Name = request.Name,
            Weight = request.Weight,
            Warranty = request.Warranty,
            CreatedAt = request.CreatedAt,
            Stock = request.Stock
        };

        _context.PCs.Add(pc);
        await _context.SaveChangesAsync();

        var result = new PcListDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };

        return CreatedAtAction(nameof(GetByIdWithComponents), new { id = pc.Id }, result);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpsertPcDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "Name is required."
            });
        }

        if (request.Weight <= 0)
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "Weight must be greater than 0."
            });
        }

        if (request.Warranty < 0)
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "Warranty cannot be negative."
            });
        }

        if (request.Stock < 0)
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "Stock cannot be negative."
            });
        }

        var pc = await _context.PCs.FirstOrDefaultAsync(p => p.Id == id);

        if (pc == null)
        {
            return NotFound(new ErrorResponseDto
            {
                Message = $"PC with id {id} was not found."
            });
        }

        pc.Name = request.Name;
        pc.Weight = request.Weight;
        pc.Warranty = request.Warranty;
        pc.CreatedAt = request.CreatedAt;
        pc.Stock = request.Stock;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "PC updated successfully."
        });
    }
    
}