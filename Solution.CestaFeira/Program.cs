using System.Reflection;
using CestaFeira.Domain.Dtos.AppSettings;
using MediatR;
using CestaFeira.Domain;
using CestaFeira.Data;
using CestaFeira.Web.Services.Interfaces;
using CestaFeira.Web.Services.Usuario;
using CestaFeira.CrossCutting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityInfrastructure(builder.Configuration);

//builder.Services.AddCorsConfiguration();

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddFluentValidationConfiguration();

builder.Services.AddAutoMapperConfiguration();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IUsuarioService, UsuarioServices>();

//builder.Services.AddMediatR(typeof(ListaCompleteQueryHandler).Assembly);

// Registrando outros serviços necessários, se houver
//builder.Services.AddScoped<IRequestHandler<ListaCompleteQuery, List<ListaDtoComplete>>, ListaCompleteQueryHandler>();



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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();
