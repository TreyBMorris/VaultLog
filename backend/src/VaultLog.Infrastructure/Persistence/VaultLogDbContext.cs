using Microsoft.EntityFrameworkCore;

namespace VaultLog.Infrastructure.Persistence;

public class VaultLogDbContext(DbContextOptions<VaultLogDbContext> options) : DbContext(options)
{
    public const string ConnectionStringKey = "VaultLog";
}