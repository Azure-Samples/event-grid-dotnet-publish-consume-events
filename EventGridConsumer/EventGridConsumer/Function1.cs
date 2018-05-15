using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class SubscriptionValidationEventData
{
    public string ValidationCode { get; set; }

    public string ValidationUrl { get; set; }
}

class SubscriptionValidationResponse
{
    public string ValidationResponse { get; set; }
}

class ContosoItemReceivedEventData
{
    public string ItemSku { get; set; }
}

namespace EventGridConsumer
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"C# HTTP trigger function begun");
            string response = string.Empty;
            const string SubscriptionValidationEvent = "Microsoft.EventGrid.SubscriptionValidationEvent";
            const string StorageBlobCreatedEvent = "Microsoft.Storage.BlobCreated";
            const string CustomTopicEvent = "Contoso.Items.ItemReceived";

            string requestContent = await req.Content.ReadAsStringAsync();
            EventGridEvent[] eventGridEvents = JsonConvert.DeserializeObject<EventGridEvent[]>(requestContent);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                JObject dataObject = eventGridEvent.Data as JObject;

                // Deserialize the event data into the appropriate type based on event type
                if (string.Equals(eventGridEvent.EventType, SubscriptionValidationEvent, StringComparison.OrdinalIgnoreCase))
                {
                    var eventData = dataObject.ToObject<SubscriptionValidationEventData>();
                    log.Info($"Got SubscriptionValidation event data, validation code: {eventData.ValidationCode}, topic: {eventGridEvent.Topic}");
                    // Do any additional validation (as required) and then return back the below response
                    var responseData = new SubscriptionValidationResponse();
                    responseData.ValidationResponse = eventData.ValidationCode;
                    return req.CreateResponse(HttpStatusCode.OK, responseData);
                }
                else if (string.Equals(eventGridEvent.EventType, StorageBlobCreatedEvent, StringComparison.OrdinalIgnoreCase))
                {
                    var eventData = dataObject.ToObject<StorageBlobCreatedEventData>();
                    log.Info($"Got BlobCreated event data, blob URI {eventData.Url}");
                }
                else if (string.Equals(eventGridEvent.EventType, CustomTopicEvent, StringComparison.OrdinalIgnoreCase))
                {
                    var eventData = dataObject.ToObject<ContosoItemReceivedEventData>();
                    log.Info($"Got ContosoItemReceived event data, item SKU {eventData.ItemSku}");
                }
            }

            return req.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
