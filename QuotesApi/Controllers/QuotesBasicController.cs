using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesBasicController : Controller
    {
        static List<Quotes> _quotes = new List<Quotes>()
        {
            new Quotes(){Id=0,Author="Bishikh",Description="MBRDI"
                ,Title="Hello"},
            new Quotes(){Id=1,Author="Labani",Description="IBS"
                ,Title="World"}
        };

        [HttpGet]
        public IEnumerable<Quotes> GetQuotes()
        {
            return _quotes;
        }

        [HttpPost]
        public void Postquote([FromBody]Quotes quotes)
        {
            _quotes.Add(quotes);
        }

        [HttpDelete("{id}")]
        public void DeleteQuote(int id)
        {
            _quotes.RemoveAt(id);
        }

        [HttpPut("{id}")]
        public void UpdateQuote(int id, [FromBody] Quotes quotes)
        {
            _quotes[id] = quotes;
        }
    }
}
