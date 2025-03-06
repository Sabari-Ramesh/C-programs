
using EmailDemo.Data;
using EmailDemo.Helper;
using EmailDemo.Repository;
using EmailDemo.Service;
using Microsoft.EntityFrameworkCore;

namespace EmailDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //builder.Services.AddScoped<UserInfoService>();
            //builder.Services.AddScoped<IUserInfo, UserInfoRepo>();
            builder.Services.AddScoped<IUserInfo, UserInfoRepo>();
            builder.Services.AddSingleton<EmailTokenService>();
            builder.Services.AddScoped<UserInfoService>();
            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
