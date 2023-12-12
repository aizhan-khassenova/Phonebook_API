using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DapperASPNetCore.Context
{
	// Класс для доступа к строке подключения для работы с базой данных, которая читается из конфигурации приложения
	public class DapperContext
    {
        // Переменная для доступа к конфигурации приложения
        private readonly IConfiguration _configuration;

		// Переменная, которая будет содержать строку подключения к базе данных
		private readonly string _connectionString;

		// Конструктор класса, принимающий конфигурацию приложения в качестве параметра
		public DapperContext(IConfiguration configuration)
		{
			// Теперь _configuration содержит конфигурацию приложения
			_configuration = configuration;

			// Получение строки подключения к базе данных из конфигурации приложения при помощи метода GetConnectionString
			// Теперь _connectionString содержит полученную строку подключения
			_connectionString = _configuration.GetConnectionString("SqlConnection");
		}

		// Метод, который создает и возвращает объект конфигурации приложения с помощью _connectionString
		// Метод предназначен для создания объектов подключения к базе данных
		public IDbConnection CreateConnection()
			=> new SqlConnection(_connectionString);
	}
}
