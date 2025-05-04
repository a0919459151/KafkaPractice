using KafkaPractice.KafkaClient;

string bootstrapServers = "localhost:9092";
string topic = "test-topic";
string groupId = $"{topic}-group";
string clientId = topic;

int numberOfConsumers = 1; // 你想要運行的消費者實例數量
var configBuilder = new KafkaConsumerConfigBuilder(bootstrapServers, groupId, clientId);
var tasks = KafkaConsumerLauncher.StartManyConsumers(topic, configBuilder, numberOfConsumers);
await Task.WhenAll(tasks);
