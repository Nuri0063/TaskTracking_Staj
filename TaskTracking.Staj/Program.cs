using Microsoft.EntityFrameworkCore;
using TaskTracking.Staj.Data;
using TaskTracking.Staj.Hubs;
using TaskTracking.Staj.Interfaces;
using TaskTracking.Staj.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSignalR(); //SingalR
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200") //angular taraf�ndaki frontend adresi
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials() // SingalR
              .SetIsOriginAllowed(origin => true);
    });
});



var app = builder.Build();
app.UseCors("AllowAll"); //.net'in angular ile ileti�imde engel koymamas� i�in

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); �u an http protokol� kulland���m�z ve bunu https'e �evirmeye �al��t��� i�in API ileti�imi sekteye u�ruyordu. Bundan dolay� �u anl�k bu sat�r� kald�rd�k

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notification"); //SingalR

app.Run();
