using AutoMapper;
using BusinessObject.Mapper;
using BusinessObject.Models;
using DataAccess.DAO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories;
using Repositories.Interfaces;
using System.Security.Claims;
using BusinessObject.Models;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Get the connection string
var connectionString = builder.Configuration.GetConnectionString("LocalDB");

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
	options.Conventions.AuthorizeFolder("/Index");
	options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()); 
});

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(builder =>
	{
		builder.AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader();
	});
});

// Add Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Operator"));
    options.AddPolicy("RequireCheckingStaffRole", policy => policy.RequireRole("CheckingStaff"));
    options.AddPolicy("RequireSponsorRole", policy => policy.RequireRole("Sponsor"));
});

// Add Database Context
builder.Services.AddDbContext<SeminarManagementDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IEventSponsorRepository, EventSponsorRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IHallRepository, HallRepository>();
builder.Services.AddScoped<ISponsorRepository, SponsorRepository>();
builder.Services.AddScoped<IFeedBackRepository, FeedBackRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<ISponsorRepository, SponsorRepository>();
builder.Services.AddScoped<IEventSponsorRepository, EventSponsorRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IEventSponsorRepository, EventSponsorRepository>();
builder.Services.AddScoped<RoleDAO>();
builder.Services.AddScoped<UserDAO>();
builder.Services.AddScoped<BookingDAO>();
builder.Services.AddScoped<CategoryDAO>();
builder.Services.AddScoped<EventDAO>();
builder.Services.AddScoped<HallDAO>();
builder.Services.AddScoped<FeedBackDAO>();
builder.Services.AddScoped<TicketDAO>();
builder.Services.AddScoped<TransactionDAO>();
builder.Services.AddScoped<WalletDAO>();
builder.Services.AddScoped<SponsorDAO>();
builder.Services.AddScoped<EventSponsorDAO>();
builder.Services.AddScoped<EventSponsorDAO>();

// Configure AutoMapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
	HttpOnly = HttpOnlyPolicy.Always
});

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();