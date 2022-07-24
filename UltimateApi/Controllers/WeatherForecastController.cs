using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace UltimateApi.Controllers
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
        private ILoggerManager _loger;


        public WeatherForecastController(
            ILogger<WeatherForecastController> logger, ILoggerManager loger)
        {
            _logger = logger;
            _loger = loger;

        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _loger.LogInfo("Here is info message from our values controller."); 
            _loger.LogDebug("Here is debug message from our values controller.");
            _loger.LogWarn("Here is warn message from our values controller.");
            _loger.LogError("Here is an error message from our values controller.");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}