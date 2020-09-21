using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DurableInjectOptions
{
    public class MyOrchestratorFunction
    {
        private readonly IMyConfig Conf;

        public MyOrchestratorFunction(IOptions<MyConfig> conf)
        {
            this.Conf = conf.Value;
        }

        [FunctionName("MyOrchestratorFunction")]
        public async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            await context.CreateTimer(context.CurrentUtcDateTime + new TimeSpan(0, 0, Conf.Timeout), CancellationToken.None);

            var outputs = new List<string>
            {
                await context.CallActivityAsync<string>("MyOrchestratorFunction_PrintThis", Conf.Endpoint)
            };

            return outputs;
        }

        [FunctionName("MyOrchestratorFunction_PrintThis")]
        public static string PrintThis([ActivityTrigger] string s, ILogger log)
        {
            log.LogInformation($"Printing --> {s}.");

            return s;
        }

        [FunctionName("MyOrchestratorFunction_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("MyOrchestratorFunction", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}