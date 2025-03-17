using Microsoft.EntityFrameworkCore;
using RoleBasedJWT.Data;
using RoleBasedJWT.Extensions;
using RoleBasedJWT.Helper;
//using RoleBasedJWT.Middleware;
using RoleBasedJWT.Repository;
using RoleBasedJWT.Service;

namespace RoleBasedJWT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Mapper Configuration
            builder.Services.AddAutoMapper(typeof(Program));


            //Dependency Injection
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserRepo>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<AccountRepository>();
            builder.Services.AddScoped<JwtHelper>();

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

         //   app.UseMiddleware<JwtMiddleware>();
            app.UseAuthentication(); // Enable JWT Authentication
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}




/*
       var jwtSettings = builder.Configuration.GetSection("Jwt");
        builder.Services.AddScoped<JwtHelper>();
        var secret = jwtSettings["Secret"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expireTimeInHours = int.Parse(jwtSettings["ExpireTimeInHours"]);

        // Add Authentication Middleware with JWT Bearer

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero, 
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role,
                };
            });

*/





// Add Authorization Middleware
// builder.Services.AddAuthorization();