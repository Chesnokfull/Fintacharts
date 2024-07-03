using Fintacharts_Data_Collection.Models;
using Microsoft.EntityFrameworkCore;

namespace Fintacharts_Data_Collection.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<InstrumentValuesTimely> InstrumentsValuesTimely { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<WebSocketMessage> WebSocketMessages { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
