using DapperASPNetCore.Context;
using DapperASPNetCore.Contracts;
using DapperASPNetCore.Controllers;
using DapperASPNetCore.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DapperASPNetCore
{
	// Startup ������������ ��� ��������� � ������������ ��������� �����������, �������� ����������, ��������� ��������� HTTP-��������,
	// ������� ����������� ������������, ��������� �������������, ��������� ������, � ����� ��������� ������������ Swagger ��� API
	public class Startup
	{
		// ����������� ������, ����������� ������������ ������������ ���������� �� ����� ���������
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// �������� ��� ������� � ���������� ������������ ���������� ������ ������
		public IConfiguration Configuration { get; }

		// ��������� ������������(DI) - ������ ����������� ����, ����� ���� ������(���������) "��������" (��������) ������� ������� ����������� ��� ��� ������ ����� ��� �������.
		// ��� ��������� �������� ����������������� ����� �����, ����������� ������������ ���������, ������ ���� ����� ��������� ��� ������� ����.

		// ����� ConfigureServices ������������ ��� ��������� � ������������ �������� ����������
		public void ConfigureServices(IServiceCollection services) // �������� services ������������ ��������� �������� ��� ��������� ������������
		{
			// ����������� ������ DapperContext ��� ��������������� �������
			services.AddSingleton<DapperContext>();

			// ����������� ����������� � �� ���������� ��� ��������
			// ��� ������� HTTP-������� ����� ������ ���� ��������� ���������������� �������, � ���� ��������� ����� �������������� �� ���� �����������, ��������� ���� ������ � ������ ������ �������
			services.AddScoped<ICityRepository, CityRepository>(); 
			services.AddScoped<IStreetRepository, StreetRepository>();
			services.AddScoped<IHouseRepository, HouseRepository>();
			services.AddScoped<IApartmentRepository, ApartmentRepository>();
			services.AddScoped<IPhoneRepository, PhoneRepository>();
			services.AddScoped<IPhonebookRepository, PhonebookRepository>();

			// Model (������): ��� ����� ��� ���������, ������� ���������� ������-������ � ������ ����������
			// Controller (����������): ��� ����� ��� ���������, ������� ������������ ������� �� ��������,
				// ��������������� � ������� ��� ��������� ������ ��� ��������� ���������, � ���������� ������ � ���� ������ �������
			// View (���): ��� ����� ��� ���������, ������� ���������� ������ ������������ � ������������ ���������������� ��������� ��� �������������� � �����������

			// ����������� ������������ ��� ��������� MVC
			// ��� ������ ����������� ���������� ��� ��������� HTTP - �������� �� ��������
			// ����������� ����� �������� �� ��������, ������� ���������������� �����, � DI ������������� �������� ��� ����������� � ����������� ��� �� ��������
			services.AddControllers();

			// ���������� ��������� Swagger ��� ������������ API
			services.AddSwaggerGen();

			// ���������� ������� CORS � ��������� ����� ASP.NET Core
			// options - ��� ������, ��������������� ��������� ������������ ��� CORS
			services.AddCors(options =>
			{
				// ����� ������������ �������� CORS � ������ "VueCorsPolicy"
				// �������� CORS ����������, ����� ������� � ������ ���������� ����� ���������
				options.AddPolicy("VueCorsPolicy", builder =>
				{
					// ��������� ����������� ��������� ��������
					//builder.WithOrigins("http://172.17.1.31:80")
					builder.AllowAnyOrigin()
						// ��������� ������������� ����� HTTP-������� (GET, POST, PUT, DELETE � ��� �����)
						.AllowAnyMethod()
						// ��������� ������������� ����� HTTP-���������� � ��������
						.AllowAnyHeader();
						
				});
			});

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = "your_issuer", // �������� �� ����
					ValidAudience = "your_audience", // �������� �� ����
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key")) // �������� �� ����
				};
			});
		}

		// ����� Configure ��� ��������� ��������� ��������� HTTP-��������
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // ������ app ������������ ��� ��������� ��������� ��������� HTTP-��������
																				// ������ env ������������ ����� ���������� � ������� ����� ���������� ���������� Development
		{
			// ��������, ��������� �� ���������� � ������ ����������
			if (env.IsDevelopment())
			{
				// ���� ��, �� ���������� �������� � ����������� �� �������, ������� �������� ����������� � ���������� ������
				app.UseDeveloperExceptionPage();
			}

			// �������� ������������� ����� ������������ �������� CORS � ������ "VueCorsPolicy"
			// �� ����������� � �������� ��������� �������� ASP.NET Core
			app.UseCors("VueCorsPolicy");

			// ����� UseRouting �������� ������������� HTTP-��������, ��� ��������� ����������, ����� ����������� � ������ ������������ ���������� �������
			app.UseRouting();

			// ����� UseHttpsRedirection �������� ��������������� �� HTTPS, ��� ������������ ���������� ���������� � �����������
			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			// ����� UseEndpoints ����������� ��������� ����� ��� ��������� �������� � ��������� �� � ������������� � �� ��������
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			// ��������� Swagger UI
			app.UseSwagger(); 

			// ��������� Swagger UI, ������� ������������� ��������� ��� �������������� � API
			app.UseSwaggerUI(c =>
			{
				// ��������� ���� � Swagger JSON-��������� � ��� ����������� �� ���-��������
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1");
				c.RoutePrefix = string.Empty;
			});
		}
	}
}
