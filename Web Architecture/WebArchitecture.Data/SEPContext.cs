
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using WebArchitecture.Entities.Models;

namespace WebArchitecture.Data
{
    public class SEPContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties()
                .Where(x =>
                    x.PropertyType.FullName.Equals("System.String") &&
                    !x.GetCustomAttributes(false).OfType<ColumnAttribute>().Where(q => q.TypeName != null && q.TypeName.Equals("varchar", StringComparison.InvariantCultureIgnoreCase)).Any())
                .Configure(c =>
                    c.HasColumnType("varchar"));

            base.OnModelCreating(modelBuilder);
        }

    }
}
