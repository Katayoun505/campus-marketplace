using ITGN350_Assignment.Data;
using ITGN350_Assignment.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new Product
            {
                Name = "Laptop",
                Description = "Powerful laptop suitable for students and professionals.",
                Price = 800,
                ImageUrl = "/images/laptop.jpg"
            },
            new Product
            {
                Name = "Phone",
                Description = "Modern smartphone with excellent performance.",
                Price = 500,
                ImageUrl = "/images/phone.jpg"
            },
            new Product
            {
                Name = "Tablet",
                Description = "Portable tablet ideal for reading and studying.",
                Price = 300,
                ImageUrl = "/images/tablet.jpg"
            },
            new Product
            {
                Name = "Headphones",
                Description = "Comfortable headphones with clear sound quality.",
                Price = 150,
                ImageUrl = "/images/headphones.jpg"
            },
            new Product
            {
                Name = "Monitor",
                Description = "High-resolution monitor for productivity and work.",
                Price = 250,
                ImageUrl = "/images/monitor.jpg"
            },
            new Product
            {
                Name = "Keyboard",
                Description = "Modern keyboard with responsive keys.",
                Price = 90,
                ImageUrl = "/images/keyboard.jpg"
            }
        );

        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();