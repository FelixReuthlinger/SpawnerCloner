using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using Jotunn;
using SpawnerCloner.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SpawnerCloner {
    public static class SpawnerReader {
        private const string ConfigName = $"{SpawnClonerPlugin.PluginGuid}.custom.*.yaml";

        public static void LoadConfig() {
            List<string> configPaths =
                Directory.GetFiles(Paths.ConfigPath, ConfigName, SearchOption.AllDirectories).ToList();
            if (!configPaths.Any()) {
                Logger.LogInfo($"no config file found inside {Paths.ConfigPath} matching to pattern {ConfigName}");
                return;
            }

            foreach (var configPath in configPaths) {
                Logger.LogInfo($"config file found: {configPath}");
            }

            IDeserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            List<Dictionary<string, SpawnerModel>> configs = new(configPaths.Count);
            configs.AddRange(configPaths.Select(file => ReadFromFile(file, deserializer)));

            var allConfigs = configs.Aggregate((a, b) =>
                a.Concat(b).ToDictionary(kv => kv.Key, kv => kv.Value));

            foreach (KeyValuePair<string, SpawnerModel> keyValuePair in allConfigs) {
                keyValuePair.Value.RegisterSpawner(keyValuePair.Key);
            }
        }

        private static Dictionary<string, SpawnerModel> ReadFromFile(string file, IDeserializer deserializer) {
            try {
                var yamlContent = File.ReadAllText(file);
                return deserializer.Deserialize<Dictionary<string, SpawnerModel>>(yamlContent);
            }
            catch (Exception e) {
                Logger.LogWarning($"Unable to parse config file '{file}' due to {e.Message}");
            }

            return new Dictionary<string, SpawnerModel>();
        }
    }
}