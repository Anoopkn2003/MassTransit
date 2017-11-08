using System;
using System.Threading.Tasks;
using MassTransit;
using NordPoolMessage;
using MassTransit.Log4NetIntegration;

namespace Publisher
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Publisher is on");
            Console.WriteLine("Registering team member");
            RunMassTransitPublisherWithRabbit();
          
        }

        private static void RunMassTransitPublisherWithRabbit()
        {
            string rabbitMqAddress = "rabbitmq://localhost:5672";
            string rabbitMqQueue = "queue";
            Uri rabbitMqRootUri = new Uri(rabbitMqAddress);

            MassTransit.Log4NetIntegration.Logging.Log4NetLogger.Use();
            //private static readonly ILog log = LogManager.GetLogger(typeof(MyApp));

        IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                rabbit.UseLog4Net();
                rabbit.Host(rabbitMqRootUri, settings =>
                {
                    settings.Password("guest");
                    settings.Username("guest");
                });
            });

            Task<ISendEndpoint> sendEndpointTask = rabbitBusControl.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;
            
            Task sendTask = sendEndpoint.Send<IRegisterTeamMember>(new
            {
               //{ id: 1,name: "Joyce"}
            MockyURL = "http://www.mocky.io/v2/59edab6c3300004e00b5c651"

            });
          
        }
    }
}
