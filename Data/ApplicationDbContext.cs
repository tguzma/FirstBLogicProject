using FirstBLogicProject.Models.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstBLogicProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<AdvisorItem> AdvisorItems { get; set; }
        public DbSet<ContractItem> ContractItems { get; set; }
        public DbSet<ClientItem> ClientItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
