using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class DeckController : Controller
    {
        public IActionResult Random()
        {
            var deck = new Deck() { Name = "N'zoth mill rogue" };
            
            return View(deck);
        }

        public ActionResult Edit(int id)
        {
            return Content("id = " + id);
        }

        public ActionResult Index(int? pageIndex, string sortBy)
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;

            if (String.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";

            return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        }
        
        [Route("Deck/Class/{ClassId:int:regex(^\\d{{1}}$):range(1,9)}")] // Finns bara 9 classer så enforcar det här.
        public ActionResult decksByClass(int ClassId)
        {
            return Content("ClassId = " + ClassId);
        }
    }
}