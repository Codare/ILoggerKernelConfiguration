using System;
using System.Collections.Generic;
using Destructurama.Attributed;
using Kernel.CrossCuttingConcerns.ILoggerExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ILoggerSandpit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

         

        public ValuesController(ILogger<ValuesController> logger)
        {
            //Log.Logger.Error("12", );
            //ILogger logger2 = Log.Logger;
            //logger2.

            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //or using?
            //_logger.BeginScope("Downloading P45 documents");

            //dynamic war = new ExpandoObject();
            //war.message = "Red alert please trigger an alert";
            //war.moreCustomDimensions = 1;
            //war.triggerAlert = true;

            //extension to take bool => triggerAlert and make the expando behind the scenes.
            //Also extension to allow an expando to be taken in aka a dynamic object

            //_logger.Log(LogLevel.Warning, new EventId(1, "Big Ben"), war, null, null);

            var position = new { Latitude = 25, Longitude = 134 };
            var elapsedMs = 34;
            var topSecret = "Top SECRET";

            _logger.LogCritical("Normal message Processed.", triggerAlert: true);

            _logger.LogCritical("Processed {@Position} in the middle of a {TopSecret} mission {Elapsed:000} ms.", triggerAlert: true, position, topSecret, elapsedMs);

            _logger.LogCritical("Processed {1} in the middle of a {2} mission {3} ms.", triggerAlert: true, position, "topSecret------", elapsedMs);

            _logger.LogCritical("Processed {@Position} in {Elapsed:000} ms.", triggerAlert: true, position, elapsedMs);

            _logger.LogCritical(new Exception("THe one to watch"), "No params trigger alert no exception.", true);

            _logger.LogCritical(new Exception("THe one to watch"), "The one to watch Processed {@Position} in {Elapsed:000} ms.", true, position, elapsedMs);

            //Standard usage
            //_logger.LogCritical(new EventId(1), new Exception("123 Exception"), "my formatted Message: {User}", new { User = "Adrian" });
            //_logger.LogCritical(new Exception("123 exception"), "my mess");
            _logger.LogCritical(new Exception("123 exception"), "my formatted Message: {User}", new { User = "Adrian" });

            //var avoidLegalAction = GetExampleClassWhichNeedsToBeSanitizedBeforeItCanBeLogged();
            var smle = new sml { CustomMasked = "123456789" };

            var cont = new Cont();
            cont.Sml = smle;

            //string strDemo = "{DefaultMasked}, {ShowFirstAndLastThreeAndCustomMaskInTheMiddle}, {ShowFirstAndLastThreeAndCustomMaskInTheMiddle1}, {ShowFirstAndLastThreeAndDefaultMaskeInTheMiddle}, {ShowFirstThreeThenCustomMask}, {ShowFirstThreeThenCustomMaskPreserveLength}, {ShowFirstThreeThenDefaultMasked}, {ShowFirstThreeThenDefaultMaskedPreserveLength}, {ShowLastThreeThenCustomMask}, {ShowLastThreeThenCustomMaskPreserveLength}, {ShowLastThreeThenDefaultMasked}, {ShowLastThreeThenDefaultMaskedPreserveLength}, {creditCardInformation}";

            string strDemo = "CustomMasked: {@Container}";
            ////Mitrefinch ILogger Extensions.
            //_logger.LogCritical("mess", true);
            //_logger.LogCritical(new Exception("123"), "my formatted Message: { User}", true);
            //_logger.LogCritical(new Exception("456"), "my formatted Message: {User}", true, new { User = "Adrian" });
            //_logger.LogCritical(exception: new Exception("The real one to watch"), message: strDemo, args: removeSensitiveInformationFromClassUsingAttributes);

            _logger.LogCritical(exception: new Exception("The real one to watch plain format message "), message: "Hi redact: {@Container}", args: cont);
            _logger.LogCritical(exception: new Exception("The real one to watch"), message: "1234 get wiv the wicked Here is {@Sml}", args: smle);

            _logger.LogCritical(exception: new Exception("The real one to watch plain format message "), triggerAlert: false, message: "Hi redact: {@Container}", args: cont);
            _logger.LogCritical(exception: new Exception("The real one to watch"), message: "1234 get wiv the wicked Here is {@Sml}", triggerAlert: false, args: smle);

            _logger.LogCritical(exception: new Exception("The real one to watch plain format message "), triggerAlert: true, message: "Hi redact: {@Container}", args: cont);
            _logger.LogCritical(exception: new Exception("The real one to watch"), message: "1234 get wiv the wicked Here is {@Sml}", triggerAlert: true, args: smle);


            //_logger.LogCritical(exception: new Exception("The real one to watch"), message: strDemo, triggerAlert: true, args: removeSensitiveInformationFromClassUsingAttributes);
            //_logger.LogCritical(exception: new Exception("The real one to watch plain format message "), message: "Hi redact: {@RemoveSensitiveInformationFromClassUsingAttributes}", triggerAlert: true, args: removeSensitiveInformationFromClassUsingAttributes);
            //_logger.LogCritical(exception: new Exception("The real one to watch"), message: "1234 get wiv the wicked Here is {@Container}", triggerAlert: true, args: container);

            return new string[] { "value1", "value2" };
        }

        private sml GetExampleClassWhichNeedsToBeSanitizedBeforeItCanBeLogged()
        {
            var example = "123456789";

            return new sml
            {
                CustomMasked = example,
                //Cvc = example,
                //DefaultMasked = example,
                //ShowFirstAndLastThreeAndCustomMaskInTheMiddle = example,
                //ShowFirstAndLastThreeAndCustomMaskInTheMiddle1 = example,
                //ShowFirstAndLastThreeAndDefaultMaskeInTheMiddle = example,
                //ShowFirstThreeThenCustomMask = example,
                //ShowFirstThreeThenCustomMaskPreserveLength = example,
                //ShowFirstThreeThenDefaultMasked = example,
                //ShowFirstThreeThenDefaultMaskedPreserveLength = example,
                //ShowLastThreeThenCustomMask = example,
                //ShowLastThreeThenCustomMaskPreserveLength = example,
                //ShowLastThreeThenDefaultMasked = example,
                //ShowLastThreeThenDefaultMaskedPreserveLength = example,
                //creditCardInformation = example,
            };
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

    public class Cont
    {
        public sml Sml { get; set; }
    }

    public class sml
    {
        //[NotLogged] public string creditCardInformation { get; set; }

        //[LogMasked(Text = "REMOVED")] public string Cvc { get; set; }

        //[LogMasked]
        //public string DefaultMasked { get; set; }

        /// <summary>
        ///  123456789 results in "REMOVED"
        /// </summary>
        [LogMasked(Text = "REMOVED")]
        public string CustomMasked { get; set; }

        ///// <summary>
        /////  123456789 results in "123***"
        ///// </summary>
        //[LogMasked(ShowFirst = 3)]
        //public string ShowFirstThreeThenDefaultMasked { get; set; }

        ///// <summary>
        /////  123456789 results in "123******"
        ///// </summary>
        //[LogMasked(ShowFirst = 3, PreserveLength = true)]
        //public string ShowFirstThreeThenDefaultMaskedPreserveLength { get; set; }

        ///// <summary>
        ///// 123456789 results in "***789"
        ///// </summary>
        //[LogMasked(ShowLast = 3)]
        //public string ShowLastThreeThenDefaultMasked { get; set; }

        ///// <summary>
        ///// 123456789 results in "******789"
        ///// </summary>
        //[LogMasked(ShowLast = 3, PreserveLength = true)]
        //public string ShowLastThreeThenDefaultMaskedPreserveLength { get; set; }

        ///// <summary>
        /////  123456789 results in "123REMOVED"
        ///// </summary>
        //[LogMasked(Text = "REMOVED", ShowFirst = 3)]
        //public string ShowFirstThreeThenCustomMask { get; set; }

        ///// <summary>
        /////  123456789 results in "REMOVED789"
        ///// </summary>
        //[LogMasked(Text = "REMOVED", ShowLast = 3)]
        //public string ShowLastThreeThenCustomMask { get; set; }

        ///// <summary>
        /////  123456789 results in "******789"
        ///// </summary>
        //[LogMasked(ShowLast = 3, PreserveLength = true)]
        //public string ShowLastThreeThenCustomMaskPreserveLength { get; set; }

        ///// <summary>
        /////  123456789 results in "123******"
        ///// </summary>
        //[LogMasked(ShowFirst = 3, PreserveLength = true)]
        //public string ShowFirstThreeThenCustomMaskPreserveLength { get; set; }

        ///// <summary>
        ///// 123456789 results in "123***789"
        ///// </summary>
        //[LogMasked(ShowFirst = 3, ShowLast = 3)]
        //public string ShowFirstAndLastThreeAndDefaultMaskeInTheMiddle { get; set; }

        ///// <summary>
        /////  123456789 results in "123REMOVED789"
        ///// </summary>
        //[LogMasked(Text = "REMOVED", ShowFirst = 3, ShowLast = 3)]
        //public string ShowFirstAndLastThreeAndCustomMaskInTheMiddle { get; set; }

        ///// <summary>
        /////  NOTE PreserveLength=true is ignored in this case
        /////  123456789 results in "123REMOVED789"
        ///// </summary>
        //[LogMasked(Text = "REMOVED", ShowFirst = 3, ShowLast = 3, PreserveLength = true)]
        //public string ShowFirstAndLastThreeAndCustomMaskInTheMiddle1 { get; set; }
    }
}
