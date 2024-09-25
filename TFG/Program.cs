using Microsoft.EntityFrameworkCore;
using TFG.Application.Services;
using TFG.Infrastructure;
using TFG.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterAppInfrastructure(builder.Configuration);
builder.Services.RegisterAppServices();
builder.Services.AddRazorPages();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var context = services.GetRequiredService<ApplicationDbContext>();
	context.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
	endpoints.MapRazorPages();
	endpoints.MapControllers();
	endpoints.MapFallbackToFile("index.html");
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseCors(cors => cors
	.AllowAnyMethod()
	.AllowAnyHeader()
	.SetIsOriginAllowed(origin => true)
	.AllowCredentials()
);

app.MapControllers();

if (app.Environment.IsDevelopment())
{
	app.MapSwagger();
}

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.Run();
