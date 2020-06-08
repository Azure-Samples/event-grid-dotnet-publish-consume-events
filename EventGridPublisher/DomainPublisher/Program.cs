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
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;

namespace EventGridPublisher
{
    // This captures the "Data" portion of an EventGridEvent on a domain
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    class ContosoItemReceivedEventData
    {
        public string ItemSku { get; set; }

        // [JsonProperty(PropertyName = "color1\\.color2")]
        public string Color { get; set; }

        public int Model { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            // TODO: Enter values for <domain-name> and <region>. You can find this domain endpoint value
            // in the "Overview" section in the "Event Grid Domains" blade in Azure Portal.
            var domainEndpoint = "https://<YOUR-DOMAIN-NAME>.<REGION-NAME>-1.eventgrid.azure.net/api/events";

            // TODO: Enter value for <domain-key>. You can find this in the "Access Keys" section in the
            // "Event Grid Domains" blade in Azure Portal.
            var domainKey = "<YOUR-DOMAIN-KEY>";

            var domainHostname = new Uri(domainEndpoint).Host;
            var domainKeyCredentials = new TopicCredentials(domainKey);
            var client = new EventGridClient(domainKeyCredentials);

            await client.PublishEventsAsync(domainHostname, GetEventsList());
            Console.Write("Published events to Event Grid domain.");
            Console.ReadLine();
        }

        static IList<EventGridEvent> GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 1; i++)
            {
                eventsList.Add(
                    new EventGridEvent
                    {
                        Id = Guid.NewGuid().ToString(),
                        EventType = "Contoso.Items.ItemReceived",

                        // TODO: Specify the name of the topic (under the domain) to which this event is destined for.
                        // Currently using a topic name "domaintopic0"
                        Topic = $"domaintopic{i}",
                        Data = new ContosoItemReceivedEventData()
                        {
                            ItemSku = "Contoso Item SKU #1",
                            Color = "green",
                            Model = 11
                        },
                        EventTime = DateTime.UtcNow,
                        Subject = "BLUE",
                        DataVersion = "2.0"
                    });
            }

            return eventsList;
        }
    }
}

