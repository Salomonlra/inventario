using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Inventario.Frontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

    

            // Configuración de autenticación
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
               
            })
            .AddCookie(options =>
            {
                // Redireccionar al login si el usuario no está autenticado
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Login";
            })
            .AddOpenIdConnect(Auth0Constants.AuthenticationScheme, options =>
            {
                options.Authority = $"https://inventarioapp.us.auth0.com";
                options.ClientId = "3wNngsdh3oznga1vOgwwEv3O1lldX82C";
                options.ClientSecret = "pHkPnUNXluYnQ3WmHfkmj8lDnFeFNNu-ZtlkxB8Uof3nZfwLeAmMu_QXJ1X3kCeg";
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.SkipUnrecognizedRequests = true;


                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");

                options.CallbackPath = new PathString("/signin-auth0");
                
                options.ClaimsIssuer = "Auth0";
                options.SaveTokens = true;

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "name"
                };

                options.Events = new OpenIdConnectEvents
                {
                    // OnRedirectToIdentityProvider = context =>
                    // {
                    // Redirigir explícitamente a esta URI después del login
                    //     context.ProtocolMessage.RedirectUri = "https://localhost:7056/signin-auth0";
                    //     return Task.CompletedTask;
                    //  },
                    //OnAuthorizationCodeReceived = async ctx =>
                    //{
                    //    Console.WriteLine("OIDC::OnAuthorizationCodeRecieved(): ");
                    //    Console.WriteLine(ctx.ProtocolMessage.Code);

                    //},
                    //OnTokenResponseReceived = ctx =>
                    //{
                    //    Console.WriteLine("OIDC::OnTokenResponseReceived()");
                    //    Console.WriteLine($"Access Token: {ctx.TokenEndpointResponse?.AccessToken}");
                    //    Console.WriteLine($"ID Token: {ctx.TokenEndpointResponse?.IdToken}");
                    //    return Task.CompletedTask;
                    //},
                    //OnMessageReceived = ctx =>
                    //{
                    //    Console.WriteLine("OIDC::OnMessageReceived(): ");
                    //    return Task.CompletedTask;
                    //},
                    //OnTokenResponseReceived = ctx =>
                    //{
                    //    Console.WriteLine("OIDC::OnTokenResponseReceived(): ");
                    //    return Task.CompletedTask;
                    //},
                    OnRedirectToIdentityProviderForSignOut = context =>
                    {
                        var clientId = "3wNngsdh3oznga1vOgwwEv3O1lldX82C";
                        var logoutUri = $"https://inventarioapp.us.auth0.com/v2/logout?client_id={clientId}";

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                            }

                            logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // ¡Muy importante!
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            app.Run();
        }
    }
}
