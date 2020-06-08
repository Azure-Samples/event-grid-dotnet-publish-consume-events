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
using System.Collections.Generic;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;

namespace EventGridPublisher
{
    // This captures the "Data" portion of an EventGridEvent on a custom topic
    class ContosoItemReceivedEventData
    {
        [JsonProperty(PropertyName = "itemSku")]
        public string ItemSku { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            // TODO: Enter values for <topic-name> and <region>. You can find this topic endpoint value
            // in the "Overview" section in the "Event Grid Topics" blade in Azure Portal.
            var topicEndpoint = "https://<YOUR-TOPIC-NAME>.<REGION-NAME>-1.eventgrid.azure.net/api/events";

            // TODO: Enter value for <topic-key>. You can find this in the "Access Keys" section in the
            // "Event Grid Topics" blade in Azure Portal.
            var topicKey = "<YOUR-TOPIC-KEY>";

            var topicHostname = new Uri(topicEndpoint).Host;
            var topicCredentials = new TopicCredentials(topicKey);
            var client = new EventGridClient(topicCredentials);

            await client.PublishEventsAsync(topicHostname, GetEventsList());
            Console.Write("Published events to Event Grid topic.");
            Console.ReadLine();
        }

        static IList<EventGridEvent> GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 1; i <= 2; i++)
            {
                eventsList.Add(new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "Contoso.Items.ItemReceived",
                    Data = new ContosoItemReceivedEventData()
                    {
                        ItemSku = $"Contoso Item SKU #{i}"
                    },
                    EventTime = DateTime.UtcNow,
                    Subject = $"Door{i}",
                    DataVersion = "2.0"
                });
            }

            return eventsList;
        }
    }
}

