
using FinancialTransfers.Application.Interfaces;
using FinancialTransfers.Application.Services;
using FinancialTransfers.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinancialTransfers.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

            builder.Services.AddScoped<ITransferService, TransferService>();

            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()   
                          .AllowAnyMethod()   
                          .AllowAnyHeader();  
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll"); 

            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!context.Accounts.Any())
                {
                    context.Accounts.AddRange(
                        new FinancialTransfers.Domain.Entities.Account
                        {
                            Name = "?????? ????????",
                            Type = FinancialTransfers.Domain.Enums.AccountType.Treasury,
                            Balance = 10000,
                            Currency = "SAR",
                            IsActive = true // ????? ?????? ???
                        },
                        new FinancialTransfers.Domain.Entities.Account
                        {
                            Name = "???? ???????",
                            BankName = "???????",
                            IBAN = "SA123456789",
                            Type = FinancialTransfers.Domain.Enums.AccountType.Bank,
                            Balance = 5000,
                            Currency = "SAR",
                            IsActive = true // ????? ?????? ???
                        }
                    );
                    context.SaveChanges();
                }
            }

            app.Run();
        }
    }
}
