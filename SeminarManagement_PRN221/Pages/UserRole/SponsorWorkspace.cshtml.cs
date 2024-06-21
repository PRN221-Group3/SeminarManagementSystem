using System.Security.Claims;
using BusinessObject.Models;
using DataAccess.DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.BaseRepo;
using Repositories.Interfaces;

namespace SeminarManagement_PRN221.Pages.UserRole.SponsorWorkspace
{
	[Authorize(Roles = "Sponsor")]
	public class SponsorEventsModel : PageModel
	{
		public SponsorEventsModel()
		{

		}
	}
}