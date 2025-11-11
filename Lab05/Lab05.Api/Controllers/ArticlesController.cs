using Lab05.Data;
using Lab05.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab05.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly NewsDbContext _db;
        public ArticlesController(NewsDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<Article>> GetAll() =>
            await _db.Articles.Include(a => a.Category).ToListAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Article>> Get(int id)
        {
            var article = await _db.Articles.Include(a => a.Category).FirstOrDefaultAsync(a => a.Id == id);
            return article == null ? NotFound() : Ok(article);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Article>> Post(Article article)
        {
            article.PublishedAt = DateTime.UtcNow;
            _db.Articles.Add(article);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = article.Id }, article);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, Article article)
        {
            if (id != article.Id) return BadRequest();
            _db.Entry(article).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _db.Articles.FindAsync(id);
            if (article == null) return NotFound();
            _db.Articles.Remove(article);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
