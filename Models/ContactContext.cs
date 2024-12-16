using Microsoft.EntityFrameworkCore;
namespace ContactManagerApp.Models
    {
    public class ContactContext : DbContext
        {
        public ContactContext(DbContextOptions<ContactContext> options) : base(options) { }
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating (ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<Contact>().HasData(
                new Contact() { ContactId = 1, FirstName = "Amitabh", LastName = "Bansal", PhoneNumber = "6290203230", Email="amitabhbansal585@gmail.com", CategoryId=1},
                new Contact() { ContactId = 2, FirstName = "Madhu", LastName = "Bansal", PhoneNumber = "9903914711", Email = "amitabhbansal5858@gmail.com", CategoryId = 1 },
                new Contact() { ContactId = 3, FirstName = "Adhikansh", LastName = "Bajaj", PhoneNumber = "9902658414", Email = "adhikanshbajaj@gmail.com", CategoryId = 2 });

            modelBuilder.Entity<Category>().HasData(
                new Category() { CategoryId = 1, Name = "Family" },
                new Category() { CategoryId = 2, Name = "Friend" },
                new Category() { CategoryId = 3, Name = "Work" });
            }

        }
    }
