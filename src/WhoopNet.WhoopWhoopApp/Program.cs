using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Reflection;
using WhoopNet.WhoopWhoopApp.Components;
using WhoopNet.WhoopWhoopApp.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(options =>
{
    //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = OAuthConstants.CodeChallengeKey; //OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOAuth("",
    options =>
{
    var oauthSettings = Configuration.GetSection("OAuthSettings").Get<OAuthSettings>();
    options.ClientId = oauthSettings.ClientId;
    options.ClientSecret = oauthSettings.ClientSecret;
    options.Authority = oauthSettings.Authority;
    options.CallbackPath = oauthSettings.CallbackPath;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Add("profile");
    options.Scope.Add("email");
    // Add any additional scopes or configuration here
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
