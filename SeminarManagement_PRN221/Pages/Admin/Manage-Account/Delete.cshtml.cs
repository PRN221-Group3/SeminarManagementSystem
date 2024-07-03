using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SeminarManagement_PRN221.Pages.Manage_Account
{
    [Authorize(Roles = "Operator")]
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

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Forbid();
            }

            if (id.ToString() == currentUserId)
            {
                TempData["ErrorMessage"] = "You cannot delete your own account.";
                return RedirectToPage("/Admin/Manage-Account/Manage");
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
