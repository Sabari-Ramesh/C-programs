
using CheckSUm.Data;
using CheckSUm.Helper;
using CheckSUm.Repository;
using CheckSUm.Service;
using Microsoft.EntityFrameworkCore;

namespace CheckSUm
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
            builder.Services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<AccountRepo>();
            builder.Services.AddScoped<CheckSum>();
            builder.Services.AddScoped<ModTenHelper>();
            builder.Services.AddScoped<SHAHelper>();
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
