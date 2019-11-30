using Fluorite.Strainer.ExampleWebApi.Data;
using Fluorite.Strainer.ExampleWebApi.Entities;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fluorite.Strainer.ExampleWebApi.Controllers
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

        /// <summary>
        /// Gets all posts.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<Post>), 200)]
        public async Task<ActionResult<List<Post>>> GetAll()
        {
            var result = await _dbContext
                .Posts
                .Include(p => p.Comments)
                .AsNoTracking()
                .ToListAsync();

            return Json(result);
        }

        /// <summary>
        /// Gets all posts with Strainer processing.
        /// </summary>
        /// <param name="strainerModel">
        /// The Strainer model containing filtering, sorting and pagination
        /// information.
        /// </param>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(List<Post>), 200)]
        public async Task<ActionResult<List<Post>>> GetAllWithStrainer([FromQuery] StrainerModel strainerModel)
        {
            var source = _dbContext
                .Posts
                .Include(p => p.Comments)
                .AsNoTracking();
            var result = _strainerProcessor.Apply(strainerModel, source);

            return await result.ToListAsync();
        }
    }
}
