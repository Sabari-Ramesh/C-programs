
namespace JWTTestForAFC
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

            //Configure the JWT
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            builder.Services.AddSingleton(new JwtHelper(
                jwtSettings["SecretKey"],
                jwtSettings["Issuer"],
                jwtSettings["Audience"],
                int.Parse(jwtSettings["AccessTokenExpirationMinutes"])
            ));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //MiddleWare
            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
