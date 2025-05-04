using Confluent.Kafka;

namespace KafkaPractice.KafkaClient
{
    public class KafkaConsumerLauncher
    {
        public static List<Task> StartManyConsumers(string topic, KafkaConsumerConfigBuilder configBuilder, int count)
        {
            var consumerTasks = new List<Task>();
            for (int i = 0; i < count; i++)
            {
                consumerTasks.Add(Task.Run(() => StartConsumer(topic, configBuilder)));
            }
            return consumerTasks;
        }

        public static void StartConsumer(string topic, KafkaConsumerConfigBuilder configBuilder)
        {
            var random = new Random();
            var config = configBuilder.Build();

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(topic);
            try
            {
                while (true)
                {
                    var result = consumer.Consume();
                    if (result != null)
                    {
                        Console.WriteLine($"[{consumer.MemberId}]Receive Message: '{result.Message.Value}', Partition: {result.Partition.Value}, Offset: {result.Offset.Value}");

                        Thread.Sleep(random.Next(500, 3000));  // 模擬任務處理

                        if (!config.EnableAutoCommit!.Value)
                        {
                            consumer.Commit();
                            Console.WriteLine($"[{consumer.Name}]Committed Partition: {result.Partition.Value}, Offset: {result.Offset.Value}");
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
                Console.WriteLine($"Consumer '{config.ClientId}' stopped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Consumer '{config.ClientId}' encountered an error: {ex.Message}");
            }
        }
    }
}
