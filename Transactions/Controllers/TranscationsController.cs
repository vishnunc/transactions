using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Transactions.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ILogger<TransactionsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public String Get()
        {
            var rng = new Random();
            return "It works!";
        }

        [HttpPost]
        public JsonElement postTransaction([FromBody] JsonElement body)
        {
            publish(body.ToString());
            return body;
        }

        public void publish(String body)
        {
            var config = new ProducerConfig { BootstrapServers = "52.15.233.255:9092" };
            string somestring = body;//JsonSerializer.Serialize(some);
            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var dr = p.ProduceAsync("program-creation", new Message<Null, string> { Value = somestring });
                    //Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                    Console.WriteLine("Publish Completed!");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
