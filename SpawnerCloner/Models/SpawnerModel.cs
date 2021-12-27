using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Jotunn.Managers;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace SpawnerCloner.Models {
    public class SpawnerModel {
        [UsedImplicitly] public string OriginalPrefabName;
        [UsedImplicitly] public string NewSpawnerName;
        [UsedImplicitly] public string NewSpawnerHoverText;
        [UsedImplicitly] public float SpawnRadius;
        [UsedImplicitly] public float NearRadius;
        [UsedImplicitly] public float FarRadius;
        [UsedImplicitly] public float TriggerDistance;
        [UsedImplicitly] public float LevelUpChance;
        [UsedImplicitly] public int MaxNear;
        [UsedImplicitly] public int MaxTotal;
        [UsedImplicitly] public float SpawnTimer;
        [UsedImplicitly] public float SpawnIntervalSeconds;
        [UsedImplicitly] public bool SpawnOnGroundOnly;
        [UsedImplicitly] public bool SetPatrolSpawnPoint;

        [UsedImplicitly] public float SizeScaleFactor;

        [UsedImplicitly] public List<SpawnDataModel> Spawns;

        [UsedImplicitly] public float SpawnerHealth;
        [UsedImplicitly] public HitData.DamageModifiers DamageModifiers;

        public void RegisterSpawner() {
            // clone the object from existing spawner
            GameObject clonedSpawner = PrefabManager.Instance.CreateClonedPrefab(NewSpawnerName, OriginalPrefabName);
            if (!clonedSpawner.TryGetComponent(out SpawnArea areaSpawner)) {
                Logger.LogError(
                    $"chosen original prefab '{OriginalPrefabName}' for spawner doesn't have a 'SpawnArea' " +
                    $"component, cannot use this prefab to clone it");
                return;
            }

            // set creatures spawned by this spawner
            areaSpawner.m_prefabs = Spawns
                .Select(spawnCreature => spawnCreature.ToSpawnData())
                .Where(spawnCreature => spawnCreature != null)
                .ToList();

            // set the base values
            areaSpawner.m_spawnRadius = SpawnRadius;
            areaSpawner.m_nearRadius = NearRadius;
            areaSpawner.m_farRadius = FarRadius;
            areaSpawner.m_triggerDistance = TriggerDistance;
            areaSpawner.m_levelupChance = LevelUpChance;
            areaSpawner.m_maxNear = MaxNear;
            areaSpawner.m_maxTotal = MaxTotal;
            areaSpawner.m_spawnTimer = SpawnTimer;
            areaSpawner.m_spawnIntervalSec = SpawnIntervalSeconds;
            areaSpawner.m_onGroundOnly = SpawnOnGroundOnly;
            areaSpawner.m_setPatrolSpawnPoint = SetPatrolSpawnPoint;

            // rescale the model by same factor in all 3 dimensions
            clonedSpawner.transform.localScale = new Vector3()
                {x = SizeScaleFactor, y = SizeScaleFactor, z = SizeScaleFactor};

            // exchange the text shown on mouse over
            if (clonedSpawner.TryGetComponent(out HoverText hoverText)) {
                hoverText.m_text = NewSpawnerHoverText;
            }

            // set the destruction configs
            if (clonedSpawner.TryGetComponent(out Destructible destructible)) {
                destructible.m_damages = DamageModifiers;
                destructible.m_health = SpawnerHealth;
            }

            // finally add the object to the game
            PrefabManager.Instance.AddPrefab(clonedSpawner);
            PrefabManager.Instance.RegisterToZNetScene(clonedSpawner);
            Logger.LogInfo($"registered spawner {clonedSpawner.name}");
        }

        [CanBeNull]
        public static SpawnerModel ReadFromGameObject([CanBeNull] GameObject fromGameObject) {
            SpawnArea spawnArea = null;
            Destructible destructible = null;
            if (fromGameObject != null && !fromGameObject.TryGetComponent(out spawnArea)) {
                Logger.LogError(
                    $"chosen prefab '{fromGameObject.name}' for spawner doesn't have a 'SpawnArea' " +
                    $"component, cannot use this prefab to clone it");
                return null;
            }

            if (fromGameObject != null && !fromGameObject.TryGetComponent(out destructible)) {
                Logger.LogError(
                    $"chosen prefab '{fromGameObject.name}' for spawner doesn't have a 'Destructible' " +
                    $"component, cannot use this prefab to clone it");
                return null;
            }

            if (fromGameObject != null && destructible != null && spawnArea != null)
                return new SpawnerModel() {
                    OriginalPrefabName = fromGameObject.name,
                    NewSpawnerName = "",
                    NewSpawnerHoverText = "",
                    Spawns = spawnArea.m_prefabs
                        .Where(prefab => prefab != null)
                        .Select(SpawnDataModel.FromSpawnData)
                        .ToList(),
                    SpawnRadius = spawnArea.m_spawnRadius,
                    NearRadius = spawnArea.m_nearRadius,
                    FarRadius = spawnArea.m_farRadius,
                    TriggerDistance = spawnArea.m_triggerDistance,
                    LevelUpChance = spawnArea.m_levelupChance,
                    MaxNear = spawnArea.m_maxNear,
                    MaxTotal = spawnArea.m_maxTotal,
                    SpawnTimer = spawnArea.m_spawnTimer,
                    SpawnIntervalSeconds = spawnArea.m_spawnIntervalSec,
                    SpawnOnGroundOnly = spawnArea.m_onGroundOnly,
                    SetPatrolSpawnPoint = spawnArea.m_setPatrolSpawnPoint,
                    SizeScaleFactor = 1f,
                    DamageModifiers = destructible.m_damages,
                    SpawnerHealth = destructible.m_health
                };
            return null;
        }
    }
}