using System;
using System.Threading.Tasks;
using MassTransit.RabbitMqTransport;
using MassTransit;
using NordPoolMessage;

namespace EventReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Event Receiver";
            Console.WriteLine("Event Receiver");
            RunMassTransitEventReceiverWithRabbit();
        }

        private static void RunMassTransitEventReceiverWithRabbit()
        {
            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost:5672"), settings =>
                {
                    settings.Password("guest");
                    settings.Username("guest");
                });

                rabbit.ReceiveEndpoint(rabbitMqHost, "queues.events", conf =>
                {
                    conf.Consumer<NameRegisteredConsumerMgmt>();
                });
            });
            rabbitBusControl.Start();
            Console.WriteLine(" press any key to close ");
            Console.ReadKey();
            rabbitBusControl.Stop();
        }
    }

    public class NameRegisteredConsumerMgmt : IConsumer<ITeamMemberRegistered>
    {
        public Task Consume(ConsumeContext<ITeamMemberRegistered> context)
        {
            ITeamMemberRegistered newName = context.Message;
            Console.WriteLine("A new team member has joined and event received for below member");
            Console.WriteLine(newName.Id);
            Console.WriteLine(newName.Name);
           
            return Task.FromResult(context.Message);
        }
    }
}
