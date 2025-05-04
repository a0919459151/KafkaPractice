using System.Net;
using Confluent.Kafka;

// 使用 Confluent.Kafka 套件來建立 Kafka 生產者
var config = new ProducerConfig 
{
    BootstrapServers = "localhost:9092", // 設定 Kafka 伺服器位址
    ClientId = Dns.GetHostName(), // 設定客戶端 ID
    Acks = Acks.All, // 設定確認模式為所有副本
    MessageTimeoutMs = 300000, // 設定訊息超時時間為 300 秒
    BatchNumMessages = 10000, // 設定批次訊息數量為 10000
    LingerMs = 5, // 設定延遲時間為 5 毫秒
    CompressionType = CompressionType.Gzip, // 設定壓縮類型為 Gzip
};

using var producer = new ProducerBuilder<Null, string>(config).Build();
Console.WriteLine("Producer started. Sending messages..."); // 提示生產者已啟動
int i = 0; // 計數器
while (true)
{
    var dr = await producer.ProduceAsync("test-topic", new Message<Null, string> { Value = $"Message {i}" }); // 發送訊息到 Kafka 主題
    Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'."); // 輸出訊息內容及位移
    await Task.Delay(3000);
    i++; // 增加計數器
}
