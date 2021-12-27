using BepInEx;
using Jotunn;
using Jotunn.Managers;

namespace SpawnerCloner {
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Main.ModGuid)]
    internal class SpawnClonerPlugin : BaseUnityPlugin {
        public const string PluginGuid = "org.bepinex.plugins.spawner.cloner";
        public const string PluginName = "SpawnerCloner";
        public const string PluginVersion = "0.1.0";

        private void Awake() {
            PrefabManager.OnPrefabsRegistered += CloneSpawners;
            CommandManager.Instance.AddConsoleCommand(new SpawnerWriterController());
        }

        public void CloneSpawners() {
            SpawnerReader.LoadConfig();
            PrefabManager.OnPrefabsRegistered -= CloneSpawners;
        }
    }
}