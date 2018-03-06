using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace DHMRice.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public override string Email { get; set; }
        public string MobileNo { get; set; }
        [RegularExpression(@"^[1-4]{1}[0-9]{4}(-)?[0-9]{7}(-)?[0-9]{1}$", ErrorMessage = "Please enter valid Cnic Number")]
        public string User_Cnic { get; set; }
        public string User_Adress { get; set; }
        public bool ShopCrendital { get; set; }
        public bool FactroryCrendital { get; set; }
        public bool Status { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()

        {
            return new ApplicationDbContext();
        }

        public DbSet<Factory> Factories { get; set; }
        public DbSet<Shop> Shopes { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Broker> Brokers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Packing> Packings { get; set; }
        public DbSet<Rice_Category> Rice_Categories { get; set; }
        public DbSet<RawRice> RarRices  { get; set; }
        public DbSet<RawRiceExpense> RawRiceExpense { get; set; }
        public DbSet<Pricing> Pricing { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Opening_ClosingDays> Opening_ClosingDays { get; set; }
        public DbSet<Rice_Production> Rice_Productions { get; set; }
        public DbSet<SaleInvoice> SaleInvoice { get; set; }
        public DbSet<RawRice_Sales_pt> RawRice_Sales_pt { get; set; }
        public DbSet<RawRice_Sales_ch> RawRice_Sales_ch { get; set; }
        public DbSet<ProducedRiceSales_pt> ProducedRiceSales_pt { get; set; }
        public DbSet<ProducedRiceSales_ch> ProducedRiceSales_ch { get; set; }
        public DbSet<ShopRiceSales_pt> ShopRiceSales_pt { get; set; }
        public DbSet<ShopRiceSales_ch> ShopRiceSales_ch { get; set; }

        public DbSet<Production_Rice> Production_Rices { get; set; }
        public DbSet<Rice_Production_ShortFall> Rice_Production_ShortFalls { get; set; }
        public DbSet<Rice_Production_Expense> Rice_Production_Expenses { get; set; }
        public DbSet<Rice_Production_ProductWorth> Rice_Production_ProductWorths { get; set; }
        public DbSet<Production_Extra_Rice> Production_Extra_Rice { get; set; }
        public DbSet<Rice_Produce_Bag> Rice_Produce_Bags { get; set; }
        public DbSet<GatePassInwared> GatePassInwareds { get; set; }
        public DbSet<Shop_Account> Shop_Accounts { get; set; }
        public DbSet<ShopStock> ShopStock { get; set; }
        public DbSet<Opening_ClosingDays_Shop> Opening_ClosingDays_Shop { get; set; }
        public DbSet<Transaction_Shop> Transaction_Shop { get; set; }

        public DbSet<BpRiceSales_pt> BpRiceSales_pts { get; set; }
        public DbSet<BpRiceSales_ch> BpRiceSales_chs { get; set; }

    }
}