using System.Threading.Tasks;
using NServiceBus;

namespace Core_6
{
    #region Event
    public class SomethingHappened :
        IEvent
    {
        public string SomeProperty { get; set; }
    }
    #endregion

    #region EventHandler
    public class SomethingHappenedHandler :
        IHandleMessages<SomethingHappened>
    {
        public Task Handle(SomethingHappened message, IMessageHandlerContext context)
        {
            // Do something with the event here

            return Task.CompletedTask;
        }
    }
    #endregion

    public class Config
    {
        void Setup(EndpointConfiguration endpointConfiguration)
        {
            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

            #region RegisterPublisher
            var routing = transport.Routing();
            routing.RegisterPublisher(typeof(SomethingHappened), "PublisherEndpoint");
            #endregion
        }

        void ExerciseConfig(EndpointConfiguration endpointConfiguration)
        {
            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

            #region BillingRouting
            var routing = transport.Routing();
            #endregion

            #region OrderPlacedPublisher
            routing.RegisterPublisher(typeof(OrderPlaced), "Sales");
            #endregion
        }
    }

    class OrderPlaced { }

}
