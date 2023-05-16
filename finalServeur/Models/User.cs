using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finalServeur.Models
{
    public class User : IdentityUser
    {
        public virtual List<Produit> Produits { get; set; }
    }
}
