using System;
using Microsoft.ServiceBus.Messaging;

namespace EventHubIntegration
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Producer;

    class Program
    {
        // connection string to the Event Hubs namespace
        private const string connectionString = "Endpoint=sb://anvihineywelleventhub1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=MsAGvHxBw33LFwrv3JBiXFcpzdyBl0F45aCZ5AEuqDk=";

        // name of the event hub
        private const string eventHubName = "oneeventhub";

        // number of events to be sent to the event hub
        private const int numOfEvents = 1;

        static EventHubProducerClient producerClient;

        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Hello World!");
        //}

        static async Task Main()
        {
            // Create a producer client that you can use to send events to an event hub
           
           // var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);

            producerClient = new EventHubProducerClient(connectionString, eventHubName);
            // Create a batch of events 
            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
            var eventBody = new BinaryData("Anvi's message-3");
            var eventData = new EventData(eventBody);


            for (int i = 1; i <= numOfEvents; i++)
            {
                //if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {i}"))))
                //{
                //    // if it is too large for the batch
                //    throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
                //}
                if (!eventBatch.TryAdd(eventData))
                {
                    // if it is too large for the batch
                    throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
                }
            }

            try
            {
                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
                Console.WriteLine($"A batch of {numOfEvents} events has been published.");
            }
            finally
            {
                await producerClient.DisposeAsync();
            }
        }
    }
}
