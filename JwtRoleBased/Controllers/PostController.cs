using Asp.Versioning;
using JwtRoleBased.Data;
using JwtRoleBased.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JwtRoleBased.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion(1.0)]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public PostController(ILogger<PostController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("new")]
        public async Task<ActionResult<Page>> CreatePage(PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = new Post
            {
                Id = postDto.Id,
                Title = postDto.Title,
                Author = postDto.Author,
                Body = postDto.Body,
            };

            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPage), new { id = post.Id }, post);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<PostDto>> GetPage(int id)
        {
            var page = await _dbContext.Posts.FindAsync(id);

            if (page is null)
            {
                return NotFound();
            }

            var pageDto = new PostDto
            {
                Id = page.Id,
                Author = page.Author,
                Body = page.Body,
                Title = page.Title
            };

            return pageDto;
        }


        [HttpGet]
        public async Task<PostsDto> ListPages()
        {
            var postsFromDb = await _dbContext.Posts.ToListAsync();

            var postsDto = new PostsDto();

            foreach (var post in postsFromDb)
            {
                var postDto = new PostDto
                {
                    Id = post.Id,
                    Author = post.Author,
                    Body = post.Body,
                    Title = post.Title
                };

                postsDto.Posts.Add(postDto);
            }

            return postsDto;
        }
    }
}
