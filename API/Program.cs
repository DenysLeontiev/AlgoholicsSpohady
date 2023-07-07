using API.ExtensionMethods;
using API.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthentification(builder.Configuration);
builder.Services.ConfigureServices();
builder.Services.ConfigureCloudinaryAccount(builder.Configuration);
builder.Services.ConfigureAutoMapper(); // AutoMapper Config
builder.Services.ConfigureDataShapper();
builder.Services.ConfigureActionFilters();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<PresenceHub>("/hubs/presence"); // configure SignalR for PresenceTracker
app.MapHub<MessageHub>("/hubs/message"); // configure SignalR for Messaging

app.Run();
