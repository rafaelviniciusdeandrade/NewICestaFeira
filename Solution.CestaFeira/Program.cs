using System.Reflection;
using CestaFeira.Domain.Dtos.AppSettings;
using CestaFeira.Data;
using CestaFeira.Web.Services.Interfaces;
using CestaFeira.Web.Services.Usuario;
using CestaFeira.Web.Services.Produto;
using CestaFeira.CrossCutting;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using CestaFeira.Domain;
using CestaFeira.Web.Services.Pedido;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityInfrastructure(builder.Configuration);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configure session
builder.Services.AddDistributedMemoryCache(); // Necess�rio para armazenar dados de sess�o
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expira��o da sess�o
    options.Cookie.HttpOnly = true; // Define o cookie como HttpOnly
    options.Cookie.IsEssential = true; // Necess�rio para pol�ticas de consentimento de cookies
});

// Configura��o de autentica��o
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login"; // Caminho para a p�gina de login
        options.LogoutPath = "/Usuario/Logout"; // Caminho para a p�gina de logout
        options.AccessDeniedPath = "/Home/AccessDenied"; // Caminho para acesso negado
    });

// Adicionar servi�os de autoriza��o se necess�rio
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("ADM"));
    options.AddPolicy("RequireProdRole", policy => policy.RequireRole("PROD"));
});

// Adiciona outros servi�os
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddFluentValidationConfiguration();
builder.Services.AddAutoMapperConfiguration();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IUsuarioService, UsuarioServices>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
});

var mapperConfig = MapperProfile.Configure();
builder.Services.AddMediatRConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll"); // Use CORS
app.UseSession(); // Middleware para sess�es
app.UseAuthentication(); // Middleware para autentica��o
app.UseAuthorization(); // Middleware para autoriza��o

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();
