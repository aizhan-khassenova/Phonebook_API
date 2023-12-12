using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// Создание хоста для настройки приложения
			// После этой конфигурации строится хост приложения, запускается сервер, выполненяется приложение и обрабатываются входящие запросы
			CreateHostBuilder(args).Build().Run(); // Метод CreateHostBuilder используется для настройки и создания хоста приложения
                                                   // Метод Build строит хост приложения на основе настроек
                                                   // Метод Run запускает хост приложения, что означает запуск сервера, выполнение приложения и обработку входящих запросов

		}
 
		// Метод CreateHostBuilder используется для настройки хоста приложения перед его созданием
		public static IHostBuilder CreateHostBuilder(string[] args) =>

			// Метод CreateDefaultBuilder создает хост приложения
			Host.CreateDefaultBuilder(args)

				// Метод ConfigureWebHostDefaults конфигурирует хост приложения
				.ConfigureWebHostDefaults(webBuilder =>
				{
					// Метод UseStartup указывает, что для конфигурации приложения будет использоваться класс Startup 
					webBuilder.UseStartup<Startup>();
				});
	}
}
