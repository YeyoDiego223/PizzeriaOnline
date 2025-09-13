using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Data;
using Microsoft.AspNetCore.Identity;
using PizzeriaOnline.Hubs;
using PizzeriaOnline.Services;

var builder = WebApplication.CreateBuilder(args);

// --- TUS SERVICIOS ORIGINALES ---
builder.Services.AddScoped<QnAService>();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddScoped<TiendaService>();

// --- LÓGICA DE BASE DE DATOS MEJORADA ---
if (builder.Environment.IsDevelopment())
{
    // Para desarrollo local, usa SQLite
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    // Para producción (Render), lee la variable de entorno
    var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ProductionConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
}

// --- CONFIGURACIÓN DE IDENTIDAD Y SESIÓN ---
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4); // Aumentamos el tiempo
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// --- CONFIGURACIÓN DEL PIPELINE HTTP ---
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<PedidoHub>("/pedidoHub");

app.Run();