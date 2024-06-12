using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Manage_Account
{
    public class DeleteModel : PageModel
    {
        private readonly SeminarManagementDbContext _context;

        public DeleteModel(SeminarManagementDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guid UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            user.IsDeleted = true;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Account deleted successfully!";

            return RedirectToPage("/Admin/Manage-Account/Manage");
        }
    }
}
