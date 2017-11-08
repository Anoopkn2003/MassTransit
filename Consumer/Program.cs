using System;
using MassTransit;
using MassTransit.RabbitMqTransport;
using StructureMap;
using Unity;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Consumer is Listening ");
            RunMassTransitConsumerWithRabbit();
        }
        
        private static void RunMassTransitConsumerWithRabbit()
        {
            //Below container if using unity
            //IUnityContainer container = new UnityContainer();
            //container.RegisterType(typeof(ITeamMemberRepository), typeof(TeamMemberRepository));

            var container = new Container(conf =>
            {
                conf.For<ITeamMemberRepository>().Use<TeamMemberRepository>();
            });
            
            IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost:5672"), settings =>
                {
                    settings.Password("guest");
                    settings.Username("guest");
                });

                rabbit.ReceiveEndpoint(rabbitMqHost, "queue", conf =>
                {
                   conf.Consumer<RegisterTeamMemberConsumer>(container);
                   
                    //Below code if using unity IoC
                   //conf.Consumer(() => (RegisterTeamMemberConsumer)container.Resolve(typeof(RegisterTeamMemberConsumer)));

                });
            });

            rabbitBusControl.Start();
            Console.WriteLine(" press any key to close ");
            Console.ReadKey();
            rabbitBusControl.Stop();
        }
    }

    
  }
