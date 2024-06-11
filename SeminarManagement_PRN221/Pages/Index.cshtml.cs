using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;

namespace SeminarManagement_PRN221.Pages
{
    public class IndexModel : PageModel
    {

        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
        }
    }
}
