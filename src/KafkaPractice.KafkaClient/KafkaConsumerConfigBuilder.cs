using Confluent.Kafka;

namespace KafkaPractice.KafkaClient;

public class KafkaConsumerConfigBuilder
{
    private readonly ConsumerConfig _config;

    /// <summary>
    /// 建構函式，強制傳入必要的設定
    /// </summary>
    /// <param name="bootstrapServers">Kafka 伺服器地址</param>
    /// <param name="groupId">消費者群組 ID</param>
    /// <param name="clientId">客戶端 ID</param>
    public KafkaConsumerConfigBuilder(string bootstrapServers, string groupId, string clientId)
    {
        if (string.IsNullOrWhiteSpace(bootstrapServers)) throw new ArgumentNullException(nameof(bootstrapServers));
        if (string.IsNullOrWhiteSpace(groupId)) throw new ArgumentNullException(nameof(groupId));

        _config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            ClientId = clientId,
            GroupId = groupId,

            // Builder 預先設定默認值
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            StatisticsIntervalMs = 5000,
            SessionTimeoutMs = 10000
        };
    }

    public KafkaConsumerConfigBuilder WithAutoOffsetReset(AutoOffsetReset autoOffsetReset)
    {
        _config.AutoOffsetReset = autoOffsetReset;
        return this;
    }

    public KafkaConsumerConfigBuilder EnableAutoCommit(int intervalMs = 5000)
    {
        _config.EnableAutoCommit = true;
        _config.AutoCommitIntervalMs = intervalMs;
        return this;
    }

    /// <summary>
    /// 建立最終的 ConsumerConfig 物件
    /// </summary>
    /// <returns>一個設定好的 ConsumerConfig 實例</returns>
    public ConsumerConfig Build()
    {
        return _config;
    }
}