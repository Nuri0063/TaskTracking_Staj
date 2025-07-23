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
        policy.WithOrigins("http://localhost:4200") //angular tarafýndaki frontend adresi
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials() // SingalR
              .SetIsOriginAllowed(origin => true);
    });
});



var app = builder.Build();
app.UseCors("AllowAll"); //.net'in angular ile iletiþimde engel koymamasý için

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); þu an http protokolü kullandýðýmýz ve bunu https'e çevirmeye çalýþtýðý için API iletiþimi sekteye uðruyordu. Bundan dolayý þu anlýk bu satýrý kaldýrdýk

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notification"); //SingalR

app.Run();
