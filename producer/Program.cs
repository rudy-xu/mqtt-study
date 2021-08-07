/*
    Author:Cade
    Date:2020-06-08
    Func:MQTT producer (single thread)
*/
using System;
using System.Net.Mqtt;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace producer
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello, World!");

            var mqttClient = MqttClient.CreateAsync("10.86.5.205").Result ;

            var sess = mqttClient.ConnectAsync().Result;

            //string rcvTopic = "HeartBeat";
            string sendTopic = "test";

            mqttClient.SubscribeAsync(rcvTopic, MqttQualityOfService.ExactlyOnce);
            
            Task.Run(() =>
            {
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.WriteLine("Enter the text to send.");

                    Console.ForegroundColor = ConsoleColor.Cyan;

                    var line = System.Console.ReadLine();

                    var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(line));

                    mqttClient.PublishAsync(new MqttApplicationMessage(sendTopic, data), MqttQualityOfService.ExactlyOnce).Wait();
                }
            });

            // mqttClient.MessageStream.Subscribe(msg =>
            // {
            //     Console.ForegroundColor = ConsoleColor.Green;

            //     Console.WriteLine(Encoding.UTF8.GetString(msg.Payload));

            //     Console.ResetColor();
            // });
        }
    }
}