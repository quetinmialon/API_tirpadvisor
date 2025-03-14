using DotNetEnv;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using tripAdvisorAPI;
using tripAdvisorAPI.Initializer;
using tripAdvisorAPI.Services;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
var environment = builder.Environment.EnvironmentName;

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
var databasePort = Environment.GetEnvironmentVariable("DATABASE_PORT");
string connectionString;

if (!string.IsNullOrEmpty(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={databasePort};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}
else
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(connectionString)
);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<TripService>();
builder.Services.AddScoped<ActivityService>();
builder.Services.AddScoped<FirebaseService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\nEnter 'Bearer YOUR_JWT_TOKEN' into the field.",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

if (FirebaseApp.DefaultInstance == null)
{
    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.FromFile("firebase-key.json"),
    });
}

builder.Services.AddSingleton(FirebaseAuth.DefaultInstance);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>{
        var firebaseProjectId = builder.Configuration["Authentification:Audience"];
        options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
            ValidateAudience = true,
            ValidAudience = firebaseProjectId,
            ValidateLifetime = true,
        };
    });

var app = builder.Build();
app.UseCors(options => options.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader());
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    DbInitializer.initialize(context);
}

app.Run();
