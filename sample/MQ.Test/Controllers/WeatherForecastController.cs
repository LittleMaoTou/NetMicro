using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetMicro.EventBus.Abstractions;
using NetMicro.EventBus.Abstractions.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQ.Test.Controllers
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
        private readonly IEventBus _bus;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEventBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _bus.Publish("test", "我是测试mq");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [NonAction]
        [EventHandler("test")]
        public void Test(string text)
        {
            Console.WriteLine(text);
        }
    }
}
