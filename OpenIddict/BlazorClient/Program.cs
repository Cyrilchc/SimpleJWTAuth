using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Yarp.ReverseProxy.Transforms;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(50);
    options.SlidingExpiration = true;
}).AddOpenIdConnect(options =>
{
    options.ClientId = "BlazorClient";
    options.ClientSecret = "10676513-8abe-41d8-8148-c3e1774fbb13";

    options.RequireHttpsMetadata = false; // Seulement en développement
    options.GetClaimsFromUserInfoEndpoint = true;
    options.SaveTokens = true;

    options.ResponseType = OpenIdConnectResponseType.Code;
    options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

    options.Authority = "https://localhost:5000";

    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("roles");
    options.Scope.Add("demo_api");

    options.MapInboundClaims = false;

    options.TokenValidationParameters.NameClaimType = "name";
    options.TokenValidationParameters.RoleClaimType = "role";
});

builder.Services.AddAuthorization(options => options.AddPolicy("CookieAuthenticationPolicy", builder =>
{
    builder.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    builder.RequireAuthenticatedUser();
}));

builder.Services.AddReverseProxy()
             .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
             .AddTransforms(builder => builder.AddRequestTransform(async context =>
             {
                 // Attach the access token retrieved from the authentication cookie.
                 //
                 // Note: in a real world application, the expiration date of the access token
                 // should be checked before sending a request to avoid getting a 401 response.
                 // Once expired, a new access token could be retrieved using the OAuth 2.0
                 // refresh token grant (which could be done transparently).
                 var token = await context.HttpContext.GetTokenAsync("access_token");

                 context.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
             }));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<HttpClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.UseEndpoints(options =>
{
    options.MapControllers();
    options.MapDefaultControllerRoute();
});
app.Run();
