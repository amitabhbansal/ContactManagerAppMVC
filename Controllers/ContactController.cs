using ContactManagerApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContactManagerApp.Controllers
    {
    public class ContactController : Controller
        {
        ContactContext _ctx;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ContactController (ContactContext ctx,IWebHostEnvironment webHostEnvironment)
            {
            this._ctx = ctx;
            _webHostEnvironment = webHostEnvironment;
            }
        public IActionResult Index ()
            {
            return View();
            }
        [HttpGet]
        public IActionResult Add ()
            {
            ViewBag.Categories = new SelectList(_ctx.Categories, "CategoryId", "Name");
            return View("Edit", new Contact());
            }
        [HttpPost]
        public async Task<IActionResult> Add (Contact contact, IFormFile? ProfilePhoto)
            {
            if (!ModelState.IsValid)
                {
                ViewBag.Categories = new SelectList(_ctx.Categories, "CategoryId", "Name");
                return View(contact);
                }

            // Handle profile photo upload
            if (ProfilePhoto != null && ProfilePhoto.Length > 0)
                {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    {
                    Directory.CreateDirectory(uploadsFolder);
                    }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfilePhoto.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                    await ProfilePhoto.CopyToAsync(stream);
                    }
                contact.ProfilePhoto = "/uploads/" + uniqueFileName;
                }

            contact.DateAdded = DateTime.Now;

            _ctx.Contacts.Add(contact);
            await _ctx.SaveChangesAsync();

            return RedirectToAction(nameof(Add));
            }
        [HttpGet]
        public async Task<IActionResult> Delete (int id)
            {
            // Find the contact by ID
            var contact = await _ctx.Contacts.FindAsync(id);
            if (contact == null)
                {
                return NotFound();
                }

            // Delete the profile photo from the uploads folder, if it exists
            if (!string.IsNullOrEmpty(contact.ProfilePhoto))
                {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, contact.ProfilePhoto.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    {
                    System.IO.File.Delete(filePath);
                    }
                }

            // Remove the contact from the database
            _ctx.Contacts.Remove(contact);
            await _ctx.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
            }
        [HttpGet]
        public async Task<IActionResult> Edit (int id)
            {
            var contact = await _ctx.Contacts.FindAsync(id);
            if (contact == null)
                {
                return NotFound();
                }

            ViewBag.Categories = new SelectList(_ctx.Categories, "CategoryId", "Name", contact.CategoryId);
            return View(contact);
            }

        // POST: Contact/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit (Contact contact, IFormFile? ProfilePhoto)
            {
            if (!ModelState.IsValid)
                {
                ViewBag.Categories = new SelectList(_ctx.Categories, "CategoryId", "Name", contact.CategoryId);
                return View(contact);
                }

            var contactInDb = await _ctx.Contacts.FindAsync(contact.ContactId);
            if (contactInDb == null)
                {
                return NotFound();
                }

            // Update properties
            contactInDb.FirstName = contact.FirstName;
            contactInDb.LastName = contact.LastName;
            contactInDb.Email = contact.Email;
            contactInDb.PhoneNumber = contact.PhoneNumber;
            contactInDb.CategoryId = contact.CategoryId;

            if (ProfilePhoto != null && ProfilePhoto.Length > 0)
                {
                // Delete old photo if it exists
                if (contactInDb.ProfilePhoto != null)
                    {
                    var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, contactInDb.ProfilePhoto.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                        {
                        System.IO.File.Delete(oldFilePath);
                        }
                    }

                // Save new photo
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    {
                    Directory.CreateDirectory(uploadsFolder);
                    }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfilePhoto.FileName;
                var newFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                    await ProfilePhoto.CopyToAsync(stream);
                    }

                contactInDb.ProfilePhoto = "/uploads/" + uniqueFileName;
                }

            await _ctx.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
            }

        }
    }
