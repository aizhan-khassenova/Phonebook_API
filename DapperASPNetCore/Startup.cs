using DapperASPNetCore.Context;
using DapperASPNetCore.Contracts;
using DapperASPNetCore.Controllers;
using DapperASPNetCore.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DapperASPNetCore
{
	// Startup используется для настройки и конфигурации различных компонентов, сервисов приложения, конвейера обработки HTTP-запросов,
	// включая регистрацию зависимостей, настройку маршрутизации, обработку ошибок, а также включение тестирования Swagger для API
	public class Startup
	{
		// Конструктор класса, позволяющий использовать конфигурацию приложения во время настройки
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// Свойство для доступа к настройкам конфигурации приложения внутри класса
		public IConfiguration Configuration { get; }

		// Внедрение зависимостей(DI) - способ организации кода, когда один объект(компонент) "внедряет" (передает) другому объекту необходимые для его работы части или сервисы.
		// Это позволяет объектам взаимодействовать между собой, обмениваясь необходимыми ресурсами, вместо того чтобы создавать эти ресурсы сами.

		// Метод ConfigureServices используется для настройки и конфигурации сервисов приложения
		public void ConfigureServices(IServiceCollection services) // Параметр services представляет коллекцию сервисов для внедрения зависимостей
		{
			// Регистрация класса DapperContext как одноэлементного сервиса
			services.AddSingleton<DapperContext>();

			// Регистрация интерфейсов и их реализаций как сервисов
			// Для каждого HTTP-запроса будет создан свой экземпляр соответствующего сервиса, и этот экземпляр будет использоваться во всех компонентах, требующих этот сервис в рамках одного запроса
			services.AddScoped<ICityRepository, CityRepository>(); 
			services.AddScoped<IStreetRepository, StreetRepository>();
			services.AddScoped<IHouseRepository, HouseRepository>();
			services.AddScoped<IApartmentRepository, ApartmentRepository>();
			services.AddScoped<IPhoneRepository, PhoneRepository>();
			services.AddScoped<IPhonebookRepository, PhonebookRepository>();

			// Model (Модель): Это класс или компонент, который определяет бизнес-логику и данные приложения
			// Controller (Контроллер): Это класс или компонент, который обрабатывает запросы от клиентов,
				// взаимодействует с моделью для получения данных или изменения состояния, и отправляет данные в виде ответа клиенту
			// View (Вид): Это класс или компонент, который отображает данные пользователю и обеспечивает пользовательский интерфейс для взаимодействия с приложением

			// Регистрация контроллеров для платформы MVC
			// Это делает контроллеры доступными для обработки HTTP - запросов от клиентов
			// Контроллеры могут зависеть от сервисов, которые зарегистрированы ранее, и DI автоматически внедряет эти зависимости в контроллеры при их создании
			services.AddControllers();

			// Добавление поддержки Swagger для тестирования API
			services.AddSwaggerGen();

			// Добавление сервиса CORS в коллекцию служб ASP.NET Core
			// options - это объект, предоставляющий параметры конфигурации для CORS
			services.AddCors(options =>
			{
				// Здесь определяется политика CORS с именем "VueCorsPolicy"
				// Политика CORS определяет, какие запросы с разных источников будут разрешены
				options.AddPolicy("VueCorsPolicy", builder =>
				{
					// Указывает разрешенные источники запросов
					//builder.WithOrigins("http://172.17.1.31:80")
					builder.AllowAnyOrigin()
						// Разрешает использование любых HTTP-методов (GET, POST, PUT, DELETE и так далее)
						.AllowAnyMethod()
						// Разрешает использование любых HTTP-заголовков в запросах
						.AllowAnyHeader();
						
				});
			});
		}

		// Метод Configure для настройки конвейера обработки HTTP-запросов
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // Объект app используется для настройки конвейера обработки HTTP-запросов
																				// Объект env представляет собой информацию о текущей среде выполнения приложения Development
		{
			// Проверка, находится ли приложение в режиме разработки
			if (env.IsDevelopment())
			{
				// Если да, то включается страница с информацией об ошибках, которая помогает отслеживать и исправлять ошибки
				app.UseDeveloperExceptionPage();
			}

			// Включает использование ранее определенной политики CORS с именем "VueCorsPolicy"
			// Он добавляется в конвейер обработки запросов ASP.NET Core
			app.UseCors("VueCorsPolicy");

			// Метод UseRouting включает маршрутизацию HTTP-запросов, что позволяет определить, какие контроллеры и методы обрабатывают конкретные запросы
			app.UseRouting();

			// Метод UseHttpsRedirection включает перенаправление на HTTPS, что обеспечивает безопасное соединение с приложением
			app.UseHttpsRedirection();

			// Метод UseEndpoints настраивает финальные точки для обработки запросов и связывает их с контроллерами и их методами
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			// Включение Swagger UI
			app.UseSwagger(); 

			// Настройка Swagger UI, который предоставляет интерфейс для взаимодействия с API
			app.UseSwaggerUI(c =>
			{
				// Настройка пути к Swagger JSON-документу и его отображению на веб-странице
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1");
				c.RoutePrefix = string.Empty;
			});
		}
	}
}
