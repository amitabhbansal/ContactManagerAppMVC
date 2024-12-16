using System.Diagnostics;
using ContactManagerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerApp.Controllers
{
    public class HomeController : Controller
    {
        ContactContext _ctx;
        public HomeController (ContactContext ctx)
            {
            this._ctx = ctx;
            }
        [HttpGet]
        [HttpGet]
        public IActionResult Index (string query)
            {
            // Check if a search query is provided and filter contacts accordingly
            var contacts = string.IsNullOrEmpty(query)
                ? _ctx.Contacts.Include(c => c.Category).OrderBy(c => c.LastName).ToList()
                : _ctx.Contacts
                    .Include(c => c.Category)
                    .Where(c => c.FirstName.Contains(query) ||
                                c.LastName.Contains(query) ||
                                c.Email.Contains(query) ||
                                c.PhoneNumber.Contains(query) ||
                                c.Category.Name.Contains(query))
                    .ToList();

            // Pass the filtered contacts to the view
            return View(contacts);
            }



        }
    }
