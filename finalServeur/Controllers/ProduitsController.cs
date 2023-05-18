using finalServeur.Data;
using finalServeur.DTOs;
using finalServeur.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace finalServeur.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly finalServeurContext _context;

        public ProduitsController(finalServeurContext contextBD)
        {
            _context = contextBD;
        }

        [HttpGet("{categorie}")]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduitsByCategorie(string categorie)
        {
            var produits = await _context.Produit
                .Include(p => p.Categorie)
                .Where(p => p.Categorie.Nom.ToLower() == categorie.ToLower())
                .ToListAsync();

            return Ok(produits);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduit()
        {
            return Ok(await _context.Produit.ToListAsync());

        }

        [HttpPost]
        [Authorize(Roles = "vendor")]
        public async Task<ActionResult<Produit>> PostProduit(ProduitDTO produitDTO)
        {
            // Vérifier si la catégorie existe déjà dans la base de données
            var categorie = await _context.Categorie.FirstOrDefaultAsync(c => c.Nom == produitDTO.NomCategorie);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User? user = await _context.Users.FindAsync(userId);

            // Si la catégorie n'existe pas, la créer et l'ajouter à la base de données
            if (categorie == null)
            {
                categorie = new Categorie { Nom = produitDTO.NomCategorie };
                _context.Categorie.Add(categorie);
                await _context.SaveChangesAsync();
            }

            // Créer un nouveau produit avec les données fournies
            Produit produit = new Produit
            {
                Nom = produitDTO.Nom,
                Prix = produitDTO.Prix,
                Categorie = categorie,
                Vendor = user // Lier l'utilisateur fournisseur actuel
            };

            // Ajouter le produit à la base de données
            _context.Produit.Add(produit);
            await _context.SaveChangesAsync();

            // Retourner le nouveau produit pour terminer
            return Ok(produit);
        }



        [HttpPost("{id}")]
        [Authorize(Roles = "vendor")]
        public async Task<ActionResult<Produit>> PostImageProduit(int id)
        {
            Produit? produit = await _context.Produit.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            try
            {
                IFormCollection formCollection = await Request.ReadFormAsync();
                IFormFile? file = formCollection.Files.GetFile("imageProduit");

                if (file != null)
                {
                    if(produit.FileName != null)
                    {
                        System.IO.File.Delete(Directory.GetCurrentDirectory() + "/Images/" + produit.FileName);
                    }

                    Image image = Image.Load(file.OpenReadStream());
                    produit.FileName = produit.Nom.ToString() + Path.GetExtension(file.FileName);
                    produit.MimeType = file.ContentType;

                    image.Save(Directory.GetCurrentDirectory() + "/Images/" + produit.FileName);

                    await _context.SaveChangesAsync();
                    return Ok(produit);
                }
                else
                {
                    return NotFound(new { Message = "Aucune image fournie" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "vendor")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            Produit? produit = await _context.Produit
                .Include(p => p.Categorie)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produit == null)
            {
                return NotFound(new { Message = "Produit non trouvé." });
            }

            // Vérifier si l'utilisateur authentifié est le fournisseur (vendor) du produit
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (produit.Vendor.Id != userId)
            {
                return Forbid(); // L'utilisateur n'est pas autorisé à supprimer ce produit
            }

            // Supprimer l'image du disque si elle existe
           if (!string.IsNullOrEmpty(produit.FileName))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", produit.FileName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

            }

            // Supprimer le produit de la base de données
            _context.Produit.Remove(produit);

            // Vérifier si la catégorie du produit n'est associée à aucun autre produit
            if (produit.Categorie != null && !_context.Produit.Any(p => p.Categorie.Id == produit.Categorie.Id))
            {
                _context.Categorie.Remove(produit.Categorie); // Supprimer la catégorie de la base de données
            }

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Produit supprimé." });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetImage(int id)
        {
            // Le paramètre id correspond à l'id du produit dont on souhaite obtenir l'image
            Produit? produit = await _context.Produit.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            string path = Directory.GetCurrentDirectory() + "/Images/" + (produit.FileName == null ? "placeholder.png" : produit.FileName);
            byte[] bytes = await System.IO.File.ReadAllBytesAsync(path);

            return File(bytes, produit.MimeType == null ? "images/png" : produit.MimeType);

        }
    }
}
