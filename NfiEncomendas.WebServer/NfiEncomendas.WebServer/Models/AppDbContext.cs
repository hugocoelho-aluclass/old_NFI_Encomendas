using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace NfiEncomendas.WebServer.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=DefaultConnection")
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
        }

        //APP TABLES
        public virtual DbSet<Operadores> Operadores { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<TipoEncomendas> TipoEncomendas { get; set; }
        public virtual DbSet<TipoAvarias> TipoAvarias { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<Encomendas> Encomendas { get; set; }
        public virtual DbSet<Relatorios> Relatorios { get; set; }
        public virtual DbSet<Savs> Savs { get; set; }
        public virtual DbSet<EstadoSav> EstadosSav { get; set; }
        public virtual DbSet<ProdutoSav> ProdutoSav { get; set; }
        public virtual DbSet<DepartamentoSav> DepartamentoSav { get; set; }
        public virtual DbSet<Anexos> Anexos { get; set; }
        public virtual DbSet<EstadoRecolha> EstadoRecolha { get; set; }
        public virtual DbSet<Recolhas> Recolhas { get; set; }
        public virtual DbSet<Setor> Setores { get; set; }
        public virtual DbSet<SetorEncomendas> SetoresEncomendas { get; set; }


        //public virtual DbSet<EncomendasCompras> EncomendasCompras { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Operadores>()
                .HasOptional(a => a.CriadoPor).WithMany().Map(m => m.MapKey("EditadoPorId"));


        }
    }
}
