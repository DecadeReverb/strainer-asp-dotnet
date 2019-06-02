using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Example.Entities;

namespace Fluorite.Strainer.Example.Controllers
{
	[Route("api/[controller]/[action]")]
    public class PostsController : Controller
    {
        private readonly IStrainerProcessor _strainerProcessor;
        private readonly ApplicationDbContext _dbContext;

        public PostsController(IStrainerProcessor strainerProcessor,
            ApplicationDbContext dbContext)
        {
            _strainerProcessor = strainerProcessor;
            _dbContext = dbContext;
        }

        [HttpGet]
        public JsonResult GetAllWithStrainer(StrainerModel strainerModel)
        {
            var result = _dbContext.Posts.AsNoTracking();

            result = _strainerProcessor.Apply(strainerModel, result);

            return Json(result.ToList());
        }

        [HttpGet]
        public JsonResult Create(int number = 10)
        {
            for (int i = 0; i < number; i++)
            {
                _dbContext.Posts.Add(new Post());
            }

            _dbContext.SaveChanges();

            return Json(_dbContext.Posts.ToList());
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            return Json(_dbContext.Posts.ToList());
        }
    }
}
