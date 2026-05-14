using Microsoft.EntityFrameworkCore;

namespace PcWarehouseApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}