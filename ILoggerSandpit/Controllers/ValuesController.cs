using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace ILoggerSandpit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //int e = 0;
            //var sum = 1 / e;

            _logger.BeginScope("Downloading P45 documents");

            var itemCount = 9;
            for (var itemNumber = 0; itemNumber < itemCount; ++itemNumber)
                _logger.LogDebug("Processing item {ItemNumber} of {ItemCount}", itemNumber, itemCount);

            _logger.LogTrace("Trace from logger {0}, {1}", 1, 8);

            _logger.LogInformation("Information from logger {0}, {1}", 1, 8);

            _logger.LogInformation("Information from logger");
            _logger.LogDebug("Debug from logger");
            _logger.LogWarning("Warning from logger");
            _logger.LogError("Error from logger");
            _logger.LogCritical("Critical from logger");

            _logger.LogInformation("No contextual properties");

            using (LogContext.PushProperty("A", 1))
            {
                _logger.LogInformation("Carries property A = 1");

                using (LogContext.PushProperty("A", 2))
                using (LogContext.PushProperty("B", 1))
                {
                    _logger.LogInformation("Carries A = 2 and B = 1");
                }

                _logger.LogInformation("Carries property A = 1, again");
            }

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
