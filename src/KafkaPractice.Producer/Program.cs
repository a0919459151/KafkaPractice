using Confluent.Kafka;

var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092",
    Acks = Acks.Leader,
};

using var producer = new ProducerBuilder<Null, string>(config).Build();
Console.WriteLine("Producer started. Sending messages..."); // 提示生產者已啟動

while (true)
{
    await SendTestMessage(3);
    await Task.Delay(1000);
}

async Task SendTestMessage(int v)
{
    foreach (var i in Enumerable.Range(0, v))
    {
        var dr = await producer.ProduceAsync("test-topic", new Message<Null, string> { Value = $"Message {i}" }); // 發送訊息到 Kafka 主題
        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'."); // 輸出訊息內容及位移
    }
}
