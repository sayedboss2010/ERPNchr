
using DAL.IoC;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

//cookies - caches - session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddPersistence(builder.Configuration);

// 👈 أضف هنا Hosted Service قبل Build
builder.Services.AddHostedService<ERPNchr.BackgroundTasks.AttendanceSyncService>();

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
app.UseAuthorization();

// Set the default route to the login page in the Account area
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=LogIn}/{id?}",
    defaults: new { area = "Account" }
);

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Account}/{action=LogIn}/{id?}"
);

await app.RunAsync();
