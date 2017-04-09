using RdKafka;
using System;
using System.Text;

namespace Crawler.Host.Components.Kafka
{
    public class ProduceHelper
    {
        private Producer _produce;
        public ProduceHelper(string conn)
        {
            _produce = new Producer(conn);
        }
        public void CreateTopic(string topicName)
        {
            //  _produce.Topic(topic)
        }
        public void PushMessage(string topicName, string message)
        {
            using (Topic topic = _produce.Topic(topicName))
            {
               
                byte[] data = Encoding.UTF8.GetBytes(message);
                DeliveryReport deliveryReport = topic.Produce(data).Result;
                Console.WriteLine($"Produced to Partition: {deliveryReport.Partition}, Offset: {deliveryReport.Offset}");
            }

        }
    }

}
