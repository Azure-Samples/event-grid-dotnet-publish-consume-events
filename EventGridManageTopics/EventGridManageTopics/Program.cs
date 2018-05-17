// 
// Copyright (c) Microsoft.  All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.EventGrid;
using Microsoft.Azure.Management.EventGrid.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest;

namespace EventGridManagementTopics
{
    /// <summary>
    /// Azure EventGrid Management Sample - Demonstrate how to create and manage EventGrid topics using EventGrid Management SDK.
    ///
    /// Documentation References:
    /// - EventGrid .NET SDK documentation - https://docs.microsoft.com/en-us/dotnet/api/overview/azure/eventgrid?view=azure-dotnet
    /// </summary>
    public class EventGridManagementSample
    {
        // Enter the Azure subscription ID you want to use for this sample.
        const string SubscriptionId = "replace-with-your-subscription-id";

        // Specify a resource group name of your choice. Specifying a new value will create a new resource group.
        const string ResourceGroupName = "TestResourceGroup";

        // Using a random topic name. Optionally, replace this with a topic name of your choice.
        static readonly string TopicName = "topicsample" + Guid.NewGuid().ToString().Substring(0, 8);

        // To run the sample, you must first create an Azure service principal. To create the service principal, follow one of these guides:
        // Azure Portal: https://azure.microsoft.com/documentation/articles/resource-group-create-service-principal-portal/)
        // PowerShell: https://azure.microsoft.com/documentation/articles/resource-group-authenticate-service-principal/
        // Azure CLI: https://azure.microsoft.com/documentation/articles/resource-group-authenticate-service-principal-cli/
        // Creating the service principal will generate the values you need to specify for the constants below.

        // Use the values generated when you created the Azure service principal.
        const string ApplicationId = "replace-with-your-application-id";
        const string Password = "replace-with-your-application-password";
        const string TenantId = "replace-with-your-tenant-id";

        // These values are used by the sample as defaults to create a new EventGrid topic.
        const string DefaultLocation = "westus";

        //The following method will enable you to use the token to create credentials
        static async Task<string> GetAuthorizationHeaderAsync()
        {
            ClientCredential cc = new ClientCredential(ApplicationId, Password);
            var context = new AuthenticationContext("https://login.windows.net/" + TenantId);
            var result = await context.AcquireTokenAsync("https://management.azure.com/", cc);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token. Please verify the values for your applicationId, Password, and Tenant.");
            }

            string token = result.AccessToken;
            return token;
        }

        public static void Main(string[] args)
        {
            PerformTopicOperations().Wait();
        }

        static async Task PerformTopicOperations()
        {
            string token = await GetAuthorizationHeaderAsync();
            TokenCredentials credential = new TokenCredentials(token);
            ResourceManagementClient resourcesClient = new ResourceManagementClient(credential)
            {
                SubscriptionId = SubscriptionId
            };

            EventGridManagementClient eventGridManagementClient = new EventGridManagementClient(credential)
            {
                SubscriptionId = SubscriptionId,
                LongRunningOperationRetryTimeout = 2
            };

            try
            {
                // Register the EventGrid Resource Provider with the Subscription
                await RegisterEventGridResourceProviderAsync(resourcesClient);

                // Create a new resource group
                await CreateResourceGroupAsync(ResourceGroupName, resourcesClient);

                // Create a new Event Grid topic in a resource group
                await CreateEventGridTopicAsync(ResourceGroupName, TopicName, eventGridManagementClient);

                // Get a list of EventGrid topics within a specific resource group
                IEnumerable<Topic> topicsInResourceGroup = await eventGridManagementClient.Topics.ListByResourceGroupAsync(ResourceGroupName);

                // Get all the EventGrid topics for a given subscription
                IEnumerable<Topic> topicsInSubscription = await eventGridManagementClient.Topics.ListBySubscriptionAsync();

                // Get the keys for a given EventGrid topic
                TopicSharedAccessKeys topicKeys = await eventGridManagementClient.Topics.ListSharedAccessKeysAsync(ResourceGroupName, TopicName);

                Console.WriteLine($"The key1 value of topic {TopicName} is: {topicKeys.Key1}");
                Console.WriteLine($"The key2 value of topic {TopicName} is: {topicKeys.Key2}");

                // Regenerate a key for a topic
                TopicSharedAccessKeys newTopicKeys = await eventGridManagementClient.Topics.RegenerateKeyAsync(ResourceGroupName, TopicName, "key1");

                // Delete an EventGrid topic with the given topic name and a resource group
                await DeleteEventGridTopicAsync(ResourceGroupName, TopicName, eventGridManagementClient);

                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public static async Task RegisterEventGridResourceProviderAsync(ResourceManagementClient resourcesClient)
        {
            Console.WriteLine("Registering EventGrid Resource Provider with subscription...");
            await resourcesClient.Providers.RegisterAsync("Microsoft.EventGrid");
            Console.WriteLine("EventGrid Resource Provider registered.");
        }

        static async Task CreateResourceGroupAsync(string rgname, ResourceManagementClient resourcesClient)
        {
            Console.WriteLine("Creating a resource group...");
            var resourceGroup = await resourcesClient.ResourceGroups.CreateOrUpdateAsync(
                    rgname,
                    new ResourceGroup
                    {
                        Location = DefaultLocation
                    });
            Console.WriteLine("Resource group created with name " + resourceGroup.Name);
        }

        static async Task CreateEventGridTopicAsync(string rgname, string topicName, EventGridManagementClient EventGridMgmtClient)
        {
            Console.WriteLine("Creating a EventGrid topic...");

            Dictionary<string, string> defaultTags = new Dictionary<string, string>
            {
                {"key1","value1"},
                {"key2","value2"}
            };

            Topic topic = new Topic()
            {
                Tags = defaultTags,
                Location = DefaultLocation,
                InputSchema = InputSchema.EventGridSchema,
                InputSchemaMapping = null
            };

            Topic createdTopic = await EventGridMgmtClient.Topics.CreateOrUpdateAsync(rgname, topicName, topic);
            Console.WriteLine("EventGrid topic created with name " + createdTopic.Name);
        }

        static async Task DeleteEventGridTopicAsync(string rgname, string topicName, EventGridManagementClient EventGridMgmtClient)
        {
            Console.WriteLine("Deleting a EventGrid topic...");
            await EventGridMgmtClient.Topics.DeleteAsync(rgname, topicName);
            Console.WriteLine("EventGrid topic " + topicName + " deleted");
        }
    }
}