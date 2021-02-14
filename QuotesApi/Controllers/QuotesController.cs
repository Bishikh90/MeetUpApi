using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuotesController : Controller
    {
        private QuotesDbContext _quotesDbContext;

        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }
        // GET: api/values
        [HttpGet]
        [ResponseCache(Duration =60,Location =ResponseCacheLocation.Client)]
        public IActionResult Get(string sort)
        {
            IQueryable<Quotes> quotes;
            switch(sort)
            {
                case "desc":
                    quotes = _quotesDbContext.Quotes.
                        OrderByDescending(q => q.CreatedAt);
                        break;
                case "asc":
                    quotes = _quotesDbContext.Quotes
                        .OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = _quotesDbContext.Quotes;
                    break;
            }
            return Ok(quotes);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_quotesDbContext.Quotes.Find(id));
        }

        //Paging Logic
        //Skip and Take Algorithm
        [HttpGet("[action]")]
        public IActionResult PagingQuote(int? pagenumber, int? pagesize)
        {
            var quotes = _quotesDbContext.Quotes.OrderBy(q =>q.Id);
            var currentPagenumber = pagenumber ?? 1;
            var currentPageSize = pagesize ?? 1;
            return Ok(quotes.Skip((currentPagenumber - 1) * currentPageSize)
                .Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public IActionResult SearchQuote(string stype)
        {
            var quotes = _quotesDbContext.Quotes.Where(q => q.Type
            .StartsWith(stype));
            return Ok(quotes);
        }

        //Routing example
        [HttpGet("[action]/{id}")]
        public int Test(int id)
        {
            return id;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Quotes quotes)
        {
            _quotesDbContext.Quotes.Add(quotes);
            _quotesDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quotes quotes)
        {
            var entity = _quotesDbContext.Quotes.Find(id);
            if (entity == null)
            {
                return NotFound("No record found for this id....");
            }
            else
            {
                entity.Author = quotes.Author;
                entity.Title = quotes.Title;
                entity.Description = quotes.Description;
                entity.Type = quotes.Type;
                entity.CreatedAt = quotes.CreatedAt;
                _quotesDbContext.SaveChanges();
                return Ok("Record Updated Successfully");
            }

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var quote = _quotesDbContext.Quotes.Find(id);

            if (quote == null)
            {
                return NotFound("No id was found for this record");
            }
            else
            {
                _quotesDbContext.Remove(quote);
                _quotesDbContext.SaveChanges();
                return Ok("Quote deleted");
            }
        }
    }
}
