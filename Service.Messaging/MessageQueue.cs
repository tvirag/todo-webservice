using RabbitMQ.Client;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.Context;
using RawRabbit.vNext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Messaging
{
    public class MessageQueue: IDisposable
    {
        private IBusClient client;
        public MessageQueue()
        {
            var  busConfig = new RawRabbitConfiguration
            {
                Username = "guest",
                Password = "guest",
                Port = 5672,
                VirtualHost = "/",
                Hostnames = { "192.168.2.141" }
            };
            this.client = BusClientFactory.CreateDefault(busConfig);
        }
        public void Respond<T1,T2>(Func<T1, Task<T2>> callback)
        {


            client.RespondAsync<T1, T2>(async (msg, context) =>
            {
                return await Task.Run(() =>
                {

                    Console.WriteLine(msg);
                    return callback(msg);
                });
            });
        }
        public void Read<T1, T2>(Func<T1, Task<T2>> callback) 
        {


            client.SubscribeAsync<T1>(async (msg, context) =>
            {
                await Task.Run(() =>
                {
                   
                    Console.WriteLine(msg);
                    return callback(msg);
                });
            });

            

        }
        public async void Submit<T>(T message)
        {

            await client.PublishAsync(message);

        }

        public async Task<T2> Direct<T1, T2>(T1 message)
        {

           
            var response = await client.RequestAsync<T1, T2>(message);

            return response;

           

        }

        void IDisposable.Dispose()
        {
            //((IDisposable)this.client).Dispose();
        }
    }

    
}
