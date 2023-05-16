using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace finalServeur.Models
{
    public class Produit
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nom { get; set; }
        [Required]
        public double Prix { get; set; }
        [JsonIgnore]
        public virtual Categorie Categorie { get; set; }
        [JsonIgnore]
        public string? FileName { get; set; }
        [JsonIgnore]
        public string? MimeType { get; set; }
        [JsonIgnore]
        public virtual User Vendor { get; set; } = null!;
    }
}
