using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using finalServeur.Models;
using Microsoft.AspNetCore.Authorization;
using finalServeur.Data;
using Microsoft.EntityFrameworkCore;

namespace finalServeur.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly finalServeurContext _context;

        public CategoriesController(finalServeurContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categorie>>> GetCategorie()
        {
            return Ok(await _context.Categorie.ToListAsync());
        }
    }
}
