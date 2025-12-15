using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4.Models
{
    internal class CentreSportifDbContext : DbContext
    {
        public virtual DbSet<Membre> Membres { get; set; }
        public virtual DbSet<Cours> Cours { get; set; }
        public virtual DbSet<TypeCours> TypeCours { get; set; }
        public virtual DbSet<Inscription> Inscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer("Data Source=ANICETPC\\SQLEXPRESS;Initial Catalog=DevLogTP4;Trusted_Connection=true;TrustServerCertificate=true;");

    }
}
