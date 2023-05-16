using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace finalServeur.Models
{
    public class Categorie
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nom { get; set; }
        public bool IsDisabled { get; set; }

        [JsonIgnore]
        public virtual List<Produit> Produits { get; set; }
    }
}
