using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluorite.Sieve.Example.Data;
using Fluorite.Strainer.Example.Entities;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fluorite.Strainer.Example.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IStrainerProcessor _strainerProcessor;
        private readonly ApplicationDbContext _dbContext;

        public PostsController(IStrainerProcessor strainerProcessor, ApplicationDbContext dbContext)
        {
            _strainerProcessor = strainerProcessor;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetAll()
        {
            return Json(await _dbContext.Posts.ToListAsync());
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<Post>>> GetAllWithStrainer(StrainerModel strainerModel)
        {
            var source = _dbContext.Posts.AsNoTracking();
            var result = _strainerProcessor.Apply(strainerModel, source);

            return await result.ToListAsync();
        }
    }
}
