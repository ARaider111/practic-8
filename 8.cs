using System;
using System.IO;
using RestSharp;
using WeatherApp;

namespace WeatherApp
{
    class WeatherData
    {
        public string CityName { get; set; }
        public MainData Main { get; set; }
        public WindData Wind { get; set; }
    }

    public class MainData
    {
        public float Temp { get; set; }
        public float FeelsLike { get; set; }
        public int Humidity { get; set; }
    }

    public class WindData
    {
        public float Speed { get; set; }
    }

    class Program
    {
        static string cityFileName = "cityFileName.txt";
        static void Main(string[] args)
        {
            string defaultCity;
            defaultCity = File.ReadAllText(cityFileName);
            Console.WriteLine($"Используется город по умолчанию: {defaultCity}");
            GetWeather(defaultCity);

            while (true)
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("1. Изменить город по умолчанию");
                Console.WriteLine("2. Ввести город, погоду в котором вы хотите узнать");
                Console.WriteLine("3. Вывести погоду города по умолчанию");
                Console.WriteLine("4. Выход из программы");
                Console.WriteLine("--------------------------------------------------");
                int n = int.Parse(Console.ReadLine());

                switch (n)
                {
                    case 1:
                        Console.Write("Введите город по умолчанию (на английском): ");
                        defaultCity = Console.ReadLine();
                        File.WriteAllText(cityFileName, defaultCity);
                        Console.WriteLine("Город успешно изменен!");
                        break;
                    case 2:
                        Console.WriteLine("Введите название города (на английском)");
                        string city = Console.ReadLine();
                        GetWeather(city);
                        break;
                    case 3:
                        GetWeather(defaultCity);
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Неккоректный ввод. Попробуйте еще раз");
                        break;
                }
            }
        }

        static void GetWeather(string defaultCity)
        {
            string apiKey = "7b60b2532884be12d2a9a12e67a4d4e1";
            var client = new RestClient("https://api.openweathermap.org/data/2.5");

            var request = new RestRequest($"weather?q={defaultCity}&appid={apiKey}&units=metric", Method.Get);
            var response = client.Execute<WeatherData>(request);

            if (response.Data != null && response.IsSuccessful)
            {
                var weather = response.Data;
                Console.WriteLine($"Город: {defaultCity}");
                Console.WriteLine($"Температура: {weather.Main.Temp}°C");
                Console.WriteLine($"Ощущается как: {weather.Main.FeelsLike}°C");
                Console.WriteLine($"Влажность: {weather.Main.Humidity}%");
                Console.WriteLine($"Скорость ветра: {weather.Wind.Speed} м/c");
            }
            else
            {
                Console.WriteLine($"Ошибка при получении погоды для города {defaultCity}. Причина: {response.ErrorMessage}");
                Console.WriteLine($"Ответ сервера: {response.Content}");
            }

        }
    }
    }