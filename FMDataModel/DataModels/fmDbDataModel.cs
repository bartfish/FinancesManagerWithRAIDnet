namespace FMDataModel.DataModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    public partial class fmDbDataModel : DbContext
    {
        public static string conn = "data source=BARTRYB;initial catalog=fmWebApp;user id=sa;password=br123;MultipleActiveResultSets=True;App=EntityFramework";
        public fmDbDataModel(string connectionString = "")
            : base(((Func<string>)(
                  delegate ()
                    {
                        if (HttpContext.Current.Session["dbConnectionString"] != null)
                        {
                            conn = HttpContext.Current.Session["dbConnectionString"].ToString();
                        }
                        return conn;
                    }
                    ))()
            )
        {

        }

        public virtual DbSet<fm_Incomes> fm_Incomes { get; set; }
        public virtual DbSet<fm_Users> fm_Users { get; set; }
        public virtual DbSet<fm_Outcomes> fm_Outcomes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<fm_Incomes>()
                .Property(e => e.Amount);

            modelBuilder.Entity<fm_Users>()
                .Property(e => e.Salt)
                .IsFixedLength();

            modelBuilder.Entity<fm_Outcomes>()
                .Property(e => e.Amount);
        }
    }
}
