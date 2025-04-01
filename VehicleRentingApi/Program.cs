
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VehicleRentingApi.Data;
using VehicleRentingApi.Entities;

namespace VehicleRentingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer =true,
                        ValidateAudience =true, 
                        ValidateLifetime =true,
                        ValidateIssuerSigningKey =true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
                    };
                });
            builder.Services.AddAuthorization();
            // Add services to the container.

            //commented becaule i am using minimal api's
            //builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Add JWT Authentication to Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer Token"
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
            new string[] {}
        }
    });
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddControllers();
            var app = builder.Build();
            app.UseCors("AllowAngularApp");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            app.MapPost("/User", async([FromServices]AppDbContext db, [FromBody]User user) =>
            {
                if (await db.Users.AnyAsync(p => p.Code == user.Code))
                {
                    return Results.BadRequest("User Already Exists");
                }
                var hashedPW=new PasswordHasher<User>().HashPassword(user,user.Password!);
                user.Password = hashedPW;
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
                return Results.Created($"/User/{user.Code}",user);
            }).RequireAuthorization();


            app.MapGet("/cars", async ([FromServices] AppDbContext db) =>
            {

                return await db.RentalCars.ToListAsync();
            }).RequireAuthorization();
            app.MapPost("/cars", async ([FromServices] AppDbContext db, [FromBody] RentalCar car) =>
            {

                await db.RentalCars.AddAsync(car);
                await db.SaveChangesAsync();
                return Results.Created($"/bookings/{car.CarId}", car);
            }).RequireAuthorization();



            app.MapGet("/customers", async ([FromServices] AppDbContext db) => await db.Customers.ToListAsync()).RequireAuthorization();
            app.MapPost("/customers", async ([FromServices] AppDbContext db, [FromBody] Customer cust) =>
            {
                await db.Customers.AddAsync(cust);
                await db.SaveChangesAsync();
            }).RequireAuthorization();

            app.MapPost("/bookings", async ([FromServices] AppDbContext db, [FromBody] RentalCarBooking booking) =>
            {
                db.RentalCarBookings.Add(booking);
                await db.SaveChangesAsync();
                return Results.Created($"/bookings/{booking.BookingId}", booking);
            }).RequireAuthorization();

            app.Run();
        }
    }
}
