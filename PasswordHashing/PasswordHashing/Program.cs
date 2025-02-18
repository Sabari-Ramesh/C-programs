using Microsoft.EntityFrameworkCore;
using PasswordHashing.Data;
using PasswordHashing.Service;
using System.Linq.Expressions;

namespace PasswordHashing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register UserService in the DI container
            builder.Services.AddScoped<UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Resolve UserService from the DI container
            using (var scope = app.Services.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<UserService>();

                Console.WriteLine("Enter 1 to add User\nEnter 2 to login User:");
                int value = Convert.ToInt32(Console.ReadLine());

                try
                {
                    switch (value)
                    {
                        case 1:
                            {
                                Console.Write("Enter UserName: ");
                                string userName = Console.ReadLine();
                                Console.Write("Password: ");
                                string password = Console.ReadLine();
                                userService.addUser(userName, password);
                                Console.WriteLine("User registered successfully!");
                                break;
                            }
                        case 2:
                            {
                                Console.Write("Enter UserName: ");
                                string userName = Console.ReadLine();
                                Console.Write("Password: ");
                                string password = Console.ReadLine();
                                bool login = userService.loginUser(userName, password);
                                string success = login ? "Login Successful" : "Login Failed";
                                Console.WriteLine(success);
                                break;
                            }
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            app.MapControllers();
            app.Run();
        }

    }
}
