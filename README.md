---
page_type: sample
languages:
- csharp
products:
- azure
description: "This contains C# samples for publishing events to Azure Event Grid and consuming events from Azure Event Grid."
urlFragment: event-grid-dotnet-publish-consume-events
---

# Microsoft Azure Event Grid Publish/Consume Samples for C#

This contains C# samples for publishing events to Azure Event Grid and consuming events from Azure Event Grid. It also contains a set of management samples that demonstrates how to manage topics and event subscriptions.

## Features

These samples demonstrates the following features:

Data Plane:

* How to publish events to Azure Event Grid.
* How to consume events delivered by Azure Event Grid.

The above two samples use the Event Grid data plane SDK (Microsoft.Azure.EventGrid).

Management Plane:

* How to create a topic and an event subscription to a topic.
* How to create an event subscription to a Storage account.
* How to create an event subscription to an Azure subscription / Resource Group.

The above three samples use the Event Grid management plane SDK (Microsoft.Azure.Management.EventGrid).

## Getting Started

### Prerequisites

- .NET Framework 4.5.2 or higher (for EventGridConsumer which is based on Azure Functions V1 Runtime)
- .NET Core 2.0 or higher (for all samples except EventGridConsumer)

### Installation

- Visual Studio 2017 Version 15.5 or later, with "Azure Development" workload enabled.
- Azure Functions Extension in Visual Studio 2017.

 Clone this repository onto your local machine. Compile the samples inside Visual Studio, the required Microsoft Azure Event Grid SDK components will automatically be downloaded from nuget.org.

 ### Running the Samples

 The following are the steps to run the data plane samples and see events flowing through Event Grid:

 1. Create an Event Grid topic: You will need to first create an Event Grid topic. The steps are described at https://docs.microsoft.com/en-us/azure/event-grid/scripts/event-grid-cli-create-custom-topic. Make a note of the topic name and resource group name. 

 2. Publish an Azure function: In this step, we will be using the EventGridConsumer sample and publishing it as an Azure function. Here are the steps:

    a. Build the EventGridConsumer project in Visual Studio.

    b. Right click on the project in Visual Studio, and click Publish to publish this Function to the cloud as an Azure Function. For more details, please refer to the steps described in https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs.

    c. Once you have published this as an Azure function, navigate to the newly published Function in Azure Portal.

    d. Click on "Get Function URL" and copy the function URL.

    e. Create an event subscription to the topic you created in step 1, and provide this Azure function as the endpoint for your event subscription. https://docs.microsoft.com/en-us/azure/event-grid/scripts/event-grid-cli-subscribe-custom-topic describes how to create an event subscription.

    f. Navigate to the Function (you created in step 2) in Azure Portal.

    g. Click on the "Logs" link at the bottom of the page.

 3. Start publishing events: In this step, we will be using the EventGridPublisher sample to start publishing events to the EventGrid topic you created in step1. Here are the steps:
 
    a. Load EventGridPublisher project in Visual Studio.

    b. In Program.cs, replace the topic-name, region, and topic-key fields with the name of your topic, location where you created the topic,and the key of the topic respectively.

    c. Run this application from Visual Studio to publish events to this topic.
    
4. Verify you received the events: In this step, we will be verifying that the events are delivered to your event subscription. Here are the steps:

    a. In the Logs view of the Azure Function, verify that you can see the logs that show the receipt of the EventGridEvent.
 
## Resources

(Any additional resources or related projects)

- https://docs.microsoft.com/en-us/azure/event-grid/overview
- https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs
