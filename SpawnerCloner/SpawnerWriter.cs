using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using SpawnerCloner.Models;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SpawnerCloner {
    public static class SpawnerWriter {
        private static readonly string DefaultFileName = $"{SpawnClonerPlugin.PluginGuid}.defaults.yaml";
        private static readonly string DefaultFile = Path.Combine(Paths.ConfigPath, DefaultFileName);
        private const string InvalidObjectRegex = @"\([0-9]+\)";

        public static void Run() {
            WriteAll(GetAllSpawners(), DefaultFile);
        }

        private static Dictionary<string, SpawnerModel> GetAllSpawners() {
            return PrefabManager.Cache
                .GetPrefabs(typeof(SpawnArea))
                .Where(kv => !Regex.IsMatch(kv.Key, InvalidObjectRegex))
                .ToDictionary(
                    pair => pair.Key,
                    pair => {
                        Jotunn.Logger.LogInfo($"serializing spawner '{pair.Key}'");
                        GameObject spawnerPrefab = PrefabManager.Instance.GetPrefab(pair.Key);
                        return SpawnerModel.ReadFromGameObject(spawnerPrefab);
                    });
        }

        private static void WriteAll(Dictionary<string, SpawnerModel> spawners, string file) {
            Jotunn.Logger.LogInfo($"Writing YAML default contents to file '{file}'");
            var yamlContent = new SerializerBuilder()
                .DisableAliases()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build()
                .Serialize(spawners);
            File.WriteAllText(file, yamlContent);
        }
    }

    public class SpawnerWriterController : ConsoleCommand {
        public override void Run(string[] args) {
            SpawnerWriter.Run();
        }

        public override string Name => "spawner_cloner_write_defaults_to_file";
        public override string Help => "Write all spawner information to a YAML file inside the BepInEx config folder.";
    }
}