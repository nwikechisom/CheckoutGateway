using CheckoutGateway.DataLayer.Context.Configurations;
using CheckoutGateway.DataLayer.Models;
using Microsoft.EntityFrameworkCore;


namespace CheckoutGateway.DataLayer.Context
{
    public class DatabaseContext : DbContext
    {
        #region Constructors
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        #endregion

        #region DbSets
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MerchantConfiguration());
            modelBuilder.ApplyConfiguration(new BankConfiguration());
            modelBuilder.ApplyConfiguration(new BankAcccountConfiguration());
            modelBuilder.ApplyConfiguration(new CardConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
