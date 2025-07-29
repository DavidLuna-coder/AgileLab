using ApexCharts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TFG.Api.Middlewares;
using TFG.Application.Security;
using TFG.Application.Services;
using TFG.Infrastructure;
using TFG.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(builder => builder.AddConsole());
builder.Services.AddControllers(options =>
{
	options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new() { Title = "TFG.Api", Version = "v1" });

	// Definir esquema de seguridad
	c.AddSecurityDefinition("Bearer", new()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Introduce el token JWT con el esquema 'Bearer'. Ejemplo: {tu token}"
	});

	// Requerir seguridad en todos los endpoints
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

builder.AddSqlServerDbContext<ApplicationDbContext>("DefaultConnection",
	configureDbContextOptions: (options) =>
	{
		options
		.UseSeeding((context, _) =>
		{
			ApplicationDbContext.SeedConfig(context);
		})
		.UseAsyncSeeding(async (context, _, cancellationToken) =>
		{
			await ApplicationDbContext.SeedConfigAsync(context);
		});
	}
);
builder.AddServiceDefaults();
builder.Services.RegisterAppInfrastructure(builder.Configuration);
builder.RegisterAppServices();
builder.Services.AddRazorPages();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PermissionBehavior<,>));

// Registro automático de todos los IPermissionValidator<T> en el ensamblado principal
var permissionValidatorType = typeof(TFG.Application.Security.IPermissionValidator<>);
var assembly = Assembly.GetExecutingAssembly();

foreach (var type in assembly.GetTypes())
{
    var interfaces = type.GetInterfaces();
    foreach (var iface in interfaces)
    {
        if (iface.IsGenericType && iface.GetGenericTypeDefinition() == permissionValidatorType)
        {
            builder.Services.AddTransient(iface, type);
        }
    }
}

// Configuración del intervalo del servicio en background desde appsettings o variable de entorno
var snapshotIntervalMinutes = builder.Configuration.GetValue<int?>("ProjectSnapshot:IntervalMinutes") ?? 5;
var snapshotInterval = TimeSpan.FromMinutes(snapshotIntervalMinutes);

builder.Services.AddHostedService(provider =>
    new TFG.Application.Services.Experiences.Commands.ProjectSnapshotBackgroundService(
        provider,
        provider.GetRequiredService<ILogger<TFG.Application.Services.Experiences.Commands.ProjectSnapshotBackgroundService>>(),
        snapshotInterval)
);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var context = services.GetRequiredService<ApplicationDbContext>();
	context.Database.Migrate();
	// Seed admin user
	await AdminUserSeeder.SeedAdminUserAsync(services);
}

// Configure the HTTP request pipeline.

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();


app.UseHttpsRedirection();

app.UseCors(cors => cors
	.AllowAnyMethod()
	.AllowAnyHeader()
	.SetIsOriginAllowed(origin => true)
	.AllowCredentials()
);

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

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
