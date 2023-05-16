using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using finalServeur.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace finalServeur.Data
{
    public class finalServeurContext : IdentityDbContext<User>
    {
        public finalServeurContext(DbContextOptions<finalServeurContext> options)
            : base(options)
        {
        }

        public DbSet<finalServeur.Models.Categorie> Categorie { get; set; } = default!;

        public DbSet<finalServeur.Models.Produit> Produit { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "vendor", NormalizedName = "VENDOR"}    
            );

            // Users
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            User u1 = new User
            {
                Id = "3581643a-1846-4d14-9d5f-bd778e2df16b",
                UserName = "JeanYvan", NormalizedUserName = "JEANYVAN",
                Email = "jean@yvan.com", NormalizedEmail = "JEAN@YVAN.COM"
            };
            u1.PasswordHash = hasher.HashPassword(u1, "Salut1!");

            User u2 = new User
            {
                Id = "11111111-1111-1111-1111-111111111111", 
                UserName = "BobVendPas", NormalizedUserName = "BOBVENDPAS",
                Email = "b@b.b", NormalizedEmail = "B@B.B"
            };
            u2.PasswordHash = hasher.HashPassword(u2, "Salut1!");

            builder.Entity<User>().HasData(u1, u2);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = u1.Id, RoleId = "1" }    
            );

            // Categories
            builder.Entity<Categorie>().HasData(
            new Categorie { Id = 1, Nom = "Ordinateurs", IsDisabled = false },
                new Categorie { Id = 2, Nom = "Périphériques", IsDisabled = false },
                new Categorie { Id = 3, Nom = "Composants", IsDisabled = false },
                new Categorie { Id = 4, Nom = "Accessoires", IsDisabled = false }
            );

            builder.Entity<Produit>().HasData(
               new
               {
                   Id = 1,
                   Nom = "PC portable ASUS",
                   Prix = 849.99,
                   CategorieId = 1,
                   FileName = "PC_Asus.jpg",
                   MimeType = "image/jpg",
                   VendorId = u1.Id
               },
               new
               {
                   Id = 2,
                   Nom = "Souris Logitech",
                   Prix = 19.99,
                   CategorieId = 2,
                   FileName = "souris_logitech_.jpg",
                   MimeType = "image/jpg",
                   VendorId = u1.Id
               },
               new
               {
                   Id = 3,
                   Nom = "Carte graphique Nvidia RTX 3070",
                   Prix = 849.99,
                   CategorieId = 3,
                   FileName = "gforce_rtx_3070.jpg",
                   MimeType = "image/jpg",
                   VendorId = u1.Id
               },
               new
               {
                   Id = 4,
                   Nom = "Clavier mécanique Corsair",
                   Prix = 129.99,
                   CategorieId = 2,
                   FileName = "clavier_corsaire.jpg",
                   MimeType = "image/jpg",
                   VendorId = u1.Id
               },
               new
               {
                   Id = 5,
                   Nom = "Moniteur LG UltraWide",
                   Prix = 699.99,
                   CategorieId = 1,
                   FileName = "moniteur_LG.jpg",
                   MimeType = "image/jpg",
                   VendorId = u1.Id
               }
           );
        }
    }
}
