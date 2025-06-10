using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace YamlDotNetDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dict = new Dictionary<string, int>();
            var text = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}/config.txt");
            foreach (var item in text)
            {
                var len = item.Split("=");
                dict.Add(len[0], int.Parse(len[1]));
            }

            var pathList = new List<string> { "/richisland/gsdd", "/richisland/nlgsdd", "/richisland/ytyh" };
            var serializer = new SerializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
            var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();

            foreach (var item in pathList)
            {
                var ymlFile = Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}{item}", "docker-compose.yaml");
                if (File.Exists(ymlFile))
                {
                    var ymlContent = File.ReadAllText(ymlFile);
                    var buildConfig = deserializer.Deserialize<DockerComposeConfig>(ymlContent);
                    if (buildConfig != null)
                    {
                        if (buildConfig.Version == null)
                            buildConfig.Version = "''3''"; // 设置默认版本
                        foreach (var ser in buildConfig.Services)
                        {
                            if (ser.Value.Ports == null)
                            {
                                ser.Value.Ports = new List<string> { $"{dict[ser.Key]}:{dict[ser.Key]}" };
                            }
                            if (ser.Value.Logging == null)
                                ser.Value.Logging = new Dictionary<string, string>
                                {
                                    { "driver", "none" }
                                };
                        }
                        if (buildConfig.Volumes == null)
                            buildConfig.Volumes = new Dictionary<string, Volume>();
                        var newYamlContent = serializer.Serialize(buildConfig);

                        Console.WriteLine($"正在处理{item}/docker-compose.yaml");
                        if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/new/{item}"))
                            Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}/new/{item}");

                        File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}/new/{item}/docker-compose.yaml", newYamlContent);
                        Thread.Sleep(1000);
                    }
                }
            }

            Console.WriteLine("处理成功");
            Console.ReadKey();
        }
    }
}
