using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace testNetCoreNlog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation($"{nameof(WeatherForecast)} --- ");
            
            var rng = new Random();

            var weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                                             {
                                                 Date = DateTime.Now.AddDays(index),
                                                 TemperatureC = rng.Next(-20, 55),
                                                 Summary = Summaries[rng.Next(Summaries.Length)]
                                             })
                                             .ToArray();
            
            _logger.LogInformation($" data --  {JsonSerializer.Serialize(weatherForecasts.Take(2))}");

            return weatherForecasts;
        }
    }
}