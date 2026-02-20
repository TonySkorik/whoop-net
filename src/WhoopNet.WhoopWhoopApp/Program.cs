using Microsoft.Extensions.Options;
using System.Reflection;
using WhoopNet.WhoopWhoopApp.Components;
using WhoopNet.WhoopWhoopApp.Configuration;
using WhoopNet.WhoopWhoopApp.OAuth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddAuthentication()
	.AddWhoop(options =>
	{
		var appSettings = builder.Configuration.Get<AppSettings>() ?? throw new InvalidOperationException("Configuration is null");

		options.ClientId = appSettings.Auth.ClientId;
		options.ClientSecret = appSettings.Auth.ClientSecret;

		options.Scope.Add(WhoopNet.Models.Scopes.Profile);
		options.Scope.Add(WhoopNet.Models.Scopes.Recovery);
		options.Scope.Add(WhoopNet.Models.Scopes.Cycles);
		options.Scope.Add(WhoopNet.Models.Scopes.Sleep);
		options.Scope.Add(WhoopNet.Models.Scopes.Workout);
		options.Scope.Add(WhoopNet.Models.Scopes.BodyMeasurement);

		options.CallbackPath = "/oauth/redirect";
	});

// Api backend
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddSingleton(implementationFactory: (sp) => sp.GetRequiredService<IOptions<AppSettings>>().Value);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

// Api backend

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
