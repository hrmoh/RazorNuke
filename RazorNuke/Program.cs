using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RazorNuke.DbContext;
using RSecurityBackend.Authorization;
using RSecurityBackend.DbContext;
using RSecurityBackend.Models.Auth.Db;
using RSecurityBackend.Models.Mail;
using RSecurityBackend.Services;
using RSecurityBackend.Services.Implementation;
using RSecurityBackend.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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
if (bool.Parse(builder.Configuration["AuditNetEnabled"] ?? false.ToString()))
{
    builder.Services.AddMvc(mvc =>
                   mvc.AddAuditFilter(config => config
                   .LogRequestIf(r => r.Method != "GET")
                   .WithEventType("{controller}/{action} ({verb})")
                   .IncludeHeaders(ctx => !ctx.ModelState.IsValid)
                   .IncludeRequestBody()
                   .IncludeModelState()
               ));
}

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient();

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

app.Run();
