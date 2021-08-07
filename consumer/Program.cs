/*
    Author:Cade
    Date:2020-06-08
    Func:MQTT consumer （multithreading）
*/
using System;
using System.Net.Mqtt;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace consumer
{
/*
    // class Receiver
    // {
    //     private string rcvTopic;

    //     public Receiver(string topic){  rcvTopic = topic; }

    //     //Func 
    //     public void receive()
    //     {
    //         var mqttClient = MqttClient.CreateAsync("10.86.5.205").Result ;
    //         var sess = mqttClient.ConnectAsync().Result;

    //         //the way of "ParameterizedThreadStart" should set the functio parameter to "object obj"
    //         // string rcvTopic = topic as string;

    //         // string sendTopic = "test";

    //         // Task.Run(() =>
    //         // {
    //         //     while (true)
    //         //     {
    //         //         Console.ForegroundColor = ConsoleColor.Yellow;

    //         //         Console.WriteLine("Enter the text to send.");

    //         //         Console.ForegroundColor = ConsoleColor.Cyan;

    //         //         var line = System.Console.ReadLine();

    //         //         //Encoding.UTF8.GetBytes can convert to byte[]
    //         //         var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(line));

    //         //         mqttClient.PublishAsync(new MqttApplicationMessage(sendTopic, data), MqttQualityOfService.ExactlyOnce).Wait();
    //         //     }
    //         // });

    //         mqttClient.SubscribeAsync(rcvTopic, MqttQualityOfService.ExactlyOnce);

    //         mqttClient.MessageStream.Subscribe(msg =>
    //         {
    //             Console.ForegroundColor = ConsoleColor.Green;

    //             Console.WriteLine(Encoding.UTF8.GetString(msg.Payload));

    //             Console.ResetColor();
    //         });
    //     }

    // }
*/

    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello, MQTT!");

            //----------------------------- Thread ------------------------------------
            /*带参数的线程*/
            //----first way
            // Thread th1 = new Thread(new ParameterizedThreadStart(receive));
            // th1.Start("HeartBeat");

            // Thread th2 = new Thread(new ParameterizedThreadStart(receive));
            // th2.Start("test");   

            //-----second way
            // Receiver recv1 = new Receiver("HeartBeat");
            //Receiver recv2 = new Receiver("test");            

            // Thread th1 = new Thread(new ThreadStart(recv1.receive));
            // th1.Start();

            // Thread th2 = new Thread(new ThreadStart(recv2.receive));
            // th2.Start();

            //-------------------------- Task -------------------------------------
            /**
                * mqttClinet include many data and function. Example:
                    * Data: MessageStream(IOservable<MqttApplication>), Id(string), IsConnected(bool) and so on.
                    * Function: Task<SessionState> ConnnectAsync(...), Task SubscribeAsync(...), Task PublishAsync(...) and so on.
            
            */
            var mqttClient = MqttClient.CreateAsync("10.86.5.205").Result ;
            var sess = mqttClient.ConnectAsync().Result;
            // Console.WriteLine(mqttClient.Id);
            // Console.WriteLine(mqttClient.IsConnected);


            //Heartbeat monitor
            string rcvTopic_Heartbeat = "HeartBeat";
            mqttClient.SubscribeAsync(rcvTopic_Heartbeat, MqttQualityOfService.AtMostOnce);

            mqttClient.MessageStream.Subscribe(msg =>
            {
                Console.ForegroundColor = ConsoleColor.Green;

                //msg include Topic(string) and Payload(byte[])
                //Encoding.UTF8.GetString can convert byte[] to string
                Console.WriteLine(Encoding.UTF8.GetString(msg.Payload));
                Console.ResetColor();
            });       

            string rcvTopic_msg = "test";
            receive(mqttClient,rcvTopic_msg);
        }

        public static void receive(IMqttClient mqttTemp,string tempstr)
        {
            mqttTemp.SubscribeAsync(tempstr, MqttQualityOfService.ExactlyOnce);

            mqttTemp.MessageStream.Subscribe(msg =>
            {
                Console.ForegroundColor = ConsoleColor.Green;

                //msg include Topic(string) and Payload(byte[])
                Console.WriteLine(Encoding.UTF8.GetString(msg.Payload));

                Console.ResetColor();
            });            
        }
    }
}