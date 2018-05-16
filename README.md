# Microsoft Azure Event Grid Publish/Consume Samples for C#

This contains C# samples for publishing events to Azure Event Grid and consuming events from Azure Event Grid.

## Features

These samples demonstrates the following features:

* How to publish events to Azure Event Grid.
* How to consume events delivered by Azure Event Grid.

The samples use the Event Grid data plane SDK (Microsoft.Azure.EventGrid).

## Getting Started

### Prerequisites

- .NET Framework 4.5 or higher

### Installation

- Visual Studio 2017 Version 15.5 or later, with "Azure Development" workload enabled.
- Azure Functions Extension in Visual Studio 2017.

 Clone this repository onto your local machine. You will find two samples, one that shows how to publish events to Azure Event Grid, and one that shows how to consume events from Azure Event Grid. Compile the samples inside Visual Studio, the required Microsoft Azure Event Grid SDK components will automatically be downloaded from nuget.org.

 ### Running the Samples

 1. EventGridPublisher: This sample demonstrates how to publish events to an Azure Event Grid topic. This is a regular Console application. Before running this sample, you will need to create an Event Grid topic using the steps described at https://docs.microsoft.com/en-us/azure/event-grid/scripts/event-grid-cli-create-custom-topic and then replace the topic-name, region, and topic-key fields in the sample with the name of your topic, location where you created the topic, and the key of the topic respectively. Once you have set these values, you can run this application from Visual Studio to publish events to this topic.

 2. EventGridConsumer: This sample demonstrates how to consume events delivered by Azure Event Grid. This sample is an Azure Functions based project. You can build this in Visual Studio. To publish this Function to the cloud, please refer to the steps described in https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs. Once you have published this as an Azure function, you can create an event subscription and provide this Azure function as the endpoint for your event subscription. https://docs.microsoft.com/en-us/azure/event-grid/scripts/event-grid-cli-subscribe-custom-topic describes how to create an event subscription.

## Resources

(Any additional resources or related projects)

- https://docs.microsoft.com/en-us/azure/event-grid/overview
- https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs
