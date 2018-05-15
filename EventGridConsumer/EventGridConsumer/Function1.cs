//----------------------------------------------------------------------------------
// Microsoft Azure EventGrid Team
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//----------------------------------------------------------------------------------

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

// This captures the schema of the "Data" portion of an EventGridEvent for a subscription validation event.
// For a HTTP trigger based Azure function, the webhook implementation should echo this code as part of the 
// validation response. For situations where webhooks cannot programmatically provide a validation response,
// a GET on the validationUrl can be used to manually complete the event subscription validation handshake.
class SubscriptionValidationEventData
{
    public string ValidationCode { get; set; }

    public string ValidationUrl { get; set; }
}

// This captures the schema of the event subscription validation response.
class SubscriptionValidationResponse
{
    public string ValidationResponse { get; set; }
}

// This captures the "Data" portion of an EventGridEvent on a custom topic
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
                    log.Info($"Got SubscriptionValidation event data, validationCode: {eventData.ValidationCode},  validationUrl: {eventData.ValidationUrl}, topic: {eventGridEvent.Topic}");
                    // Do any additional validation (as required) such as validating that the Azure resource ID of the topic matches
                    // the expected topic and then return back the below response
                    var responseData = new SubscriptionValidationResponse();
                    responseData.ValidationResponse = eventData.ValidationCode;
                    return req.CreateResponse(HttpStatusCode.OK, responseData);
                }
                else if (string.Equals(eventGridEvent.EventType, StorageBlobCreatedEvent, StringComparison.OrdinalIgnoreCase))
                {
                    // Deserialize the data portion of the event into StorageBlobCreatedEventData
                    var eventData = dataObject.ToObject<StorageBlobCreatedEventData>();
                    log.Info($"Got BlobCreated event data, blob URI {eventData.Url}");
                }
                else if (string.Equals(eventGridEvent.EventType, CustomTopicEvent, StringComparison.OrdinalIgnoreCase))
                {
                    // Deserialize the data portion of the event into ContosoItemReceivedEventData
                    var eventData = dataObject.ToObject<ContosoItemReceivedEventData>();
                    log.Info($"Got ContosoItemReceived event data, item SKU {eventData.ItemSku}");
                }
            }

            return req.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
