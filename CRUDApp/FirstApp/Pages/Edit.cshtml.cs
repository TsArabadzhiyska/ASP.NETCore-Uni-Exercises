using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FirstApp.Pages
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _db;
        private StreamingContext e;

        [BindProperty]
        public Customer Customer { get; set; }
        public EditModel(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await _db.Customers.FindAsync(id);
            if (Customer == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _db.Attach(Customer).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {

                throw new Exception($"Customer {Customer.Id} not found", e);
            }
            return RedirectToPage("./Index");
        }
    }
}