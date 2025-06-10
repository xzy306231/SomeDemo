using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace YamlDotNetDemo
{
    public class DockerComposeConfig
    {
        [YamlMember(Alias = "version")]
        public string Version { get; set; }

        [YamlMember(Alias = "services")]
        public Dictionary<string, Service> Services { get; set; }

        [YamlMember(Alias = "networks")]
        public Dictionary<string, Network> Networks { get; set; }

        [YamlMember(Alias = "volumes")]
        public Dictionary<string, Volume> Volumes { get; set; }
    }

    public class Service
    {
        [YamlMember(Alias = "container_name")]
        public string ContainerName { get; set; }

        [YamlMember(Alias = "image")]
        public string Image { get; set; }

        [YamlMember(Alias = "restart")]
        public string Restart { get; set; } = "always";

        [YamlMember(Alias = "privileged")]
        public bool Privileged { get; set; }

        [YamlMember(Alias = "volumes")]
        public List<string> Volumes { get; set; }

        [YamlMember(Alias = "ports")]
        public List<string> Ports { get; set; }

        [YamlMember(Alias = "environment")]
        public Dictionary<string, string> Environment { get; set; }
        public Dictionary<string, string> Logging { get; set; }

        [YamlMember(Alias = "networks")]
        public Dictionary<string, ServiceNetwork> Networks { get; set; }

    }

    public class ServiceNetwork
    {
       // [YamlMember(Alias = "ipv4_address")]
        public string Ipv4_address { get; set; }
    }

    public class Network
    {
        [YamlMember(Alias = "external")]
        public External External { get; set; }
    }

    public class External
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }
    }

    public class Volume
    {
        // 空对象，用于处理 volumes: {}
    }

    public class Logging
    {
        [YamlMember(Alias = "driver")]
        public string Driver { get; set; }
    }
}
