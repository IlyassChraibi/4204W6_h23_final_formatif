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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduit()
        {
            return Ok(await _context.Produit.ToListAsync());

        }

        [HttpPost]
        public async Task<ActionResult<Produit>> PostProduit(ProduitDTO produitDTO)
        {
            // ███ Ajouter du code ici ███

            Produit produit = new Produit() { }; // À compléter ...

            // ███ Ajouter du code ici ███

            // On retourne le nouveau produit pour terminer (Conservez cette ligne de code)
            return Ok(produit);
        }

        [HttpPost("{id}")]
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
        public async Task<IActionResult> DeleteProduit(int id)
        {
            Produit? produit = null; // ███ À modifier ███

            // ███ Beaucoup de code à ajouter ici ███

            _context.Produit.Remove(produit);
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
