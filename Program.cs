using EventVenueBookingSystem.Data;
using EventVenueBookingSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// ? Configure SQL Server connection
builder.Services.AddDbContext<EventVenueBookingSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConn")));

// ? Register Azure Blob Storage service
builder.Services.AddScoped<AzureBlobStorageService>();

// ? MVC support
builder.Services.AddControllersWithViews();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["AzureStorage:ConnectionString:blob"]!, preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["AzureStorage:ConnectionString:queue"]!, preferMsi: true);
});

var app = builder.Build();

// ? Error handling for production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// ? Serve static files like images
app.UseStaticFiles();

// ? Routing and endpoints
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// ? Run the app
app.Run();