using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SeminarManagement_PRN221.Pages.Admin.Manage_Account
{
    public class IndexModel : PageModel
    {
        private readonly SeminarManagementDbContext context;
        public List<User> Users { get; set; } = new List<User>();

        public IndexModel(SeminarManagementDbContext context)
        {
            this.context = context;
        }

        public void OnGet()
        {
            Users = context.Users.OrderByDescending(p => p.UserId).ToList();
        }
    }
}
