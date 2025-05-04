using Confluent.Kafka;

// 使用 Confluent.Kafka 套件來建立 Kafka 消費者
var config = new ConsumerConfig
{
    GroupId = "test-consumer-group", // 消費者群組 ID
    BootstrapServers = "localhost:9092", // Kafka 伺服器位址
    EnableAutoCommit = true, // 啟用自動提交
    AutoCommitIntervalMs = 5000, // 自動提交間隔時間為 5 秒
    AutoOffsetReset = AutoOffsetReset.Earliest, // 設定從最早的位移開始消費
    EnableAutoOffsetStore = false, // 禁用自動位移儲存
};

// 建立消費者實例
using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
consumer.Subscribe("test-topic"); // 訂閱 Kafka 主題
Console.WriteLine("Consumer started. Waiting for messages..."); // 提示消費者已啟動
while (true)
{
    try
    {
        var result = consumer.Consume(); // 消費訊息
        ProccessMessage(result);
        consumer.StoreOffset(result); // 儲存位移
    }
    catch (Exception)
    {
        Console.WriteLine("Error occure");
    }
}

void ProccessMessage(ConsumeResult<Ignore, string> result)
{
    Console.WriteLine($"Consumed message '{result.Message.Value}' at: '{result.TopicPartitionOffset}'."); // 輸出訊息內容及位移
    // throw new Exception("test"); // 測試用例，會導致消費者關閉
}