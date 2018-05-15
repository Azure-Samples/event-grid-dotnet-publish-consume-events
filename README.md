# Project Name

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

 The publishing sample is a regular Console application. The consuming sample is an Azure Functions template based project. You can build this in Visual Studio, and to publish this Function to the cloud, please refer to the steps described in https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs.


## Resources

(Any additional resources or related projects)

- https://docs.microsoft.com/en-us/azure/event-grid/overview
- https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs
