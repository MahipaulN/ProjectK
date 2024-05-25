using Microsoft.EntityFrameworkCore;

namespace PROJECT_K.Models
{
    public class PK_DbContext : DbContext
    {
        public PK_DbContext(DbContextOptions<PK_DbContext> options) : base(options)
        {
            
        }
        public DbSet<UserStatus> UserStatus {get; set;}
        public DbSet<Roles> Roles {get;set;}
        public DbSet<Users> UserInfo {get; set;}
        public DbSet<StoringSalt> Salt {get; set;}
        public DbSet<Properties> Properties{get; set;}
        public DbSet<BookingHistory> BookingHistory{get; set;}
    }
}