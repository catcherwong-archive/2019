namespace SendingMessage.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Polly;
    using RabbitMQ.Client;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var message = Encoding.UTF8.GetBytes("hello, retry pattern");

            var retry = Policy
                .Handle<Exception>()
                .WaitAndRetry(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            try
            {
                retry.Execute(() =>
                {
                    Console.WriteLine($"begin at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.");
                    var factory = new ConnectionFactory
                    {
                        HostName = "localhost",
                        UserName = "guest",
                        Password = "guest"
                    };

                    var connection = factory.CreateConnection();
                    var model = connection.CreateModel();
                    model.ExchangeDeclare("retrypattern", ExchangeType.Topic, true, false, null);
                    model.BasicPublish("retrypattern", "retrypattern.#", false, null, message);
                });
            }
            catch
            {
                Console.WriteLine("exception here.");
            }

            return new string[] { "value1", "value2" };
        }
    }
}
