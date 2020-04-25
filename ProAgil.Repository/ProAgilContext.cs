using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Domain.Identity;

namespace ProAgil.Repository
{
    public class ProAgilContext : IdentityDbContext<User, Role, int,
                                                    IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                                    IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ProAgilContext(DbContextOptions<ProAgilContext> options) : base (options){ }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<RedeSocial> RedesSociais { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
                {
                    userRole.HasKey(ur => new{ur.UserId, ur.RoleId});


                    /*
                    Custei a enteder esta merda
                    o relacionamento de n-n é dado por mei odas chaves estrangeiras dentro de UseRole
                    que vem de Role e User e são obrigatorias
                    */
                    userRole.HasOne(ur => ur.Role)
                    .WithMany(r =>UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                    userRole.HasOne(ur => ur.User)
                    .WithMany(r =>UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                    /*
                    Custei a enteder esta merda

                    */
                }
            );
            /*
                sobrescrevendo o metodo
                especificando a relação (n,n) Evento - Palestrante 
            */
            modelBuilder.Entity<PalestranteEvento>()
            .HasKey(PE => new { PE.EventoId, PE.PalestranteId });
        }
    }
}