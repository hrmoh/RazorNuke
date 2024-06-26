using Audit.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using RazorNuke.DbContext;
using RazorNuke.Services;
using RazorNuke.Services.Implementation;
using RSecurityBackend.Authorization;
using RSecurityBackend.DbContext;
using RSecurityBackend.Models.Auth.Db;
using RSecurityBackend.Models.Auth.Memory;
using RSecurityBackend.Models.Mail;
using RSecurityBackend.Services;
using RSecurityBackend.Services.Implementation;
using RSecurityBackend.Utilities;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(
                 HtmlEncoder.Create(allowedRanges: [ UnicodeRanges.BasicLatin,
                    UnicodeRanges.Arabic ]));

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/index", "{*url}");
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Add service and create Policy with options
builder.Services.AddCors(options =>
{
    options.AddPolicy("RServiceCorsPolicy",
        builder => builder.SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("paging-headers")
        .AllowCredentials()
        );
});

builder.Services.AddDbContextPool<RDbContext>(
                        options => options.UseSqlServer(
                            builder.Configuration.GetConnectionString("DefaultConnection"),
                            providerOptions =>
                            {
                                providerOptions.EnableRetryOnFailure();
                                providerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            }
                            )
                        );

Audit.Core.Configuration.DataProvider = new RAuditDataProvider(builder.Configuration.GetConnectionString("DefaultConnection"));
Audit.Core.Configuration.AuditDisabled = bool.Parse(builder.Configuration["AuditNetEnabled"] ?? false.ToString()) == false;

builder.Services.AddIdentityCore<RAppUser>(
                options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = false;
                }
                ).AddErrorDescriber<PersianIdentityErrorDescriber>();

new IdentityBuilder(typeof(RAppUser), typeof(RAppRole), builder.Services)
                .AddRoleManager<RoleManager<RAppRole>>()
                .AddSignInManager<SignInManager<RAppUser>>()
                .AddEntityFrameworkStores<RDbContext>()
                .AddErrorDescriber<PersianIdentityErrorDescriber>();

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "bearer";
}).AddJwtBearer("bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidAudience = "Everyone",
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("RSecurityBackend")["ApplicationName"],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{builder.Configuration.GetSection("Security")["Secret"]}")),

        ValidateLifetime = true, //validate the expiration and not before values in the token

        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Append("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };

});

builder.Services.AddAuthorization(options =>
{
    //this is the default policy to make sure the use session has not yet been deleted by him/her from another client
    //or by an admin (Authorize with no policy should fail on deleted sessions)
    var defPolicy = new AuthorizationPolicyBuilder();
    defPolicy.Requirements.Add(new UserGroupPermissionRequirement("null", "null"));
    options.DefaultPolicy = defPolicy.Build();


    foreach (SecurableItem Item in SecurableItem.Items)
    {
        foreach (SecurableItemOperation Operation in Item.Operations)
        {
            options.AddPolicy($"{Item.ShortName}:{Operation.ShortName}", policy => policy.Requirements.Add(new UserGroupPermissionRequirement(Item.ShortName, Operation.ShortName)));
        }
    }
    foreach (SecurableItem Item in SecurableItem.WorkspaceItems)
    {
        foreach (SecurableItemOperation Operation in Item.Operations)
        {
            options.AddPolicy($"{Item.ShortName}:{Operation.ShortName}", policy => policy.Requirements.Add(new UserGroupPermissionRequirement(Item.ShortName, Operation.ShortName)));
        }
    }
});


//IHttpContextAccessor
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


//authorization handler
builder.Services.AddScoped<IAuthorizationHandler, UserGroupPermissionHandler>();


//security context maps to main db context
builder.Services.AddTransient<RSecurityDbContext<RAppUser, RAppRole, Guid>, RDbContext>();

//captcha service
builder.Services.AddTransient<ICaptchaService, CaptchaServiceEF>();


//generic image file service
builder.Services.AddTransient<IImageFileService, ImageFileServiceEF>();

//app user services
builder.Services.AddTransient<IAppUserService, AppUserService>();

//user groups services
builder.Services.AddTransient<IUserRoleService, RoleServiceBase>();

//audit service
builder.Services.AddTransient<IAuditLogService, AuditLogServiceEF>();

//user permission checker
builder.Services.AddTransient<IUserPermissionChecker, UserPermissionChecker>();

//workspace service
builder.Services.AddTransient<IWorkspaceService, WorkspaceService>();

//workspace role service
builder.Services.AddTransient<IWorkspaceRolesService, WorkspaceRolesServiceBase>();

//secret generator
builder.Services.AddTransient<ISecretGenerator, SecretGenerator>();

// email service
builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
builder.Services.Configure<SmptConfig>(builder.Configuration);

//messaging service
builder.Services.AddTransient<IRNotificationService, RNotificationService>();

//long running job service
builder.Services.AddTransient<ILongRunningJobProgressService, LongRunningJobProgressServiceEF>();

//generic options service
builder.Services.AddTransient<IRGenericOptionsService, RGenericOptionsServiceEF>();

//razor pages service
builder.Services.AddTransient<IRazorNukePageService, RazorNukePageService>();

//upload limit for IIS
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.Parse(builder.Configuration.GetSection("IIS")["UploadLimit"] ?? "52428800");
});

builder.Services.AddHostedService<QueuedHostedService>();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});


app.Run();
