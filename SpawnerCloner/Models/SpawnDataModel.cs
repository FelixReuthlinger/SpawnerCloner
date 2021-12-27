using JetBrains.Annotations;
using Jotunn.Managers;
using UnityEngine;

namespace SpawnerCloner.Models {
    public class SpawnDataModel {
        [UsedImplicitly] public string PrefabName;
        [UsedImplicitly] public int MinLevel;
        [UsedImplicitly] public int MaxLevel;
        [UsedImplicitly] public float Weight;

        [CanBeNull]
        public SpawnArea.SpawnData ToSpawnData() {
            GameObject creaturePrefab = PrefabManager.Instance.GetPrefab(PrefabName);
            if (creaturePrefab == null) return null;
            return new SpawnArea.SpawnData() {
                m_prefab = creaturePrefab,
                m_minLevel = MinLevel,
                m_maxLevel = MaxLevel,
                m_weight = Weight
            };
        }

        [CanBeNull]
        public static SpawnDataModel FromSpawnData(SpawnArea.SpawnData fromGameObject) {
            if (fromGameObject == null) return null;
            return new SpawnDataModel() {
                PrefabName = fromGameObject.m_prefab != null ? fromGameObject.m_prefab.name : null,
                MinLevel = fromGameObject.m_minLevel,
                MaxLevel = fromGameObject.m_maxLevel,
                Weight = fromGameObject.m_weight
            };
        }
    }
}