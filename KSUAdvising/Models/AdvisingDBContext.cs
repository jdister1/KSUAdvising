using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace KSUAdvising.Models
{
    public class AdvisingDBContext : DbContext
    {
            public DbSet<Adviser> Advisers{ get; set; }
            public DbSet<CollegeSetting> CollegeSettings { get; set; }
            public DbSet<Admin> Admins { get; set; }
            public DbSet<AdministrativeAssistant> AdminstrativeAssistants { get; set; }
            public DbSet<WalkinQueue> WalkinQueue { get; set; }

            //protected override void OnModelCreating(DbModelBuilder modelBuilder)
            //{
            //    base.OnModelCreating(modelBuilder);
            //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //}
    }
}