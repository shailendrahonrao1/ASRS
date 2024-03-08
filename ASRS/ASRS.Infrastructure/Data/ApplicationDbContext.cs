using ASRS.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ASRS.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ItemMaster> ItemMasters { get; set; }
        public DbSet<StoreReceipt> StoreReceipts { get; set; }
        public DbSet<StockRelease> StockReleases { get; set; }
        public DbSet<StoreReceiptRequest> StoreReceiptRequests { get; set; }
    }
}
