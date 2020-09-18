﻿using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;
using Random = UnityEngine.Random;
#if SN1
using RecipeData = SMLHelper.V2.Crafting.TechData;
using Sprite = Atlas.Sprite;
#endif

namespace TechPistol.Module
{
    internal class PistolFragmentPrefab : Spawnable
    {
        public PistolFragmentPrefab() : base(
            "TechPistolFragment", 
            "Damaged Pistol Fragment", 
            "Incomplete or Broken fragment of an advanced pistol of unknown origins."
            )
        {
        }

        public override List<LootDistributionData.BiomeData> BiomesToSpawnIn => GetBiomeDistribution();


        private List<LootDistributionData.BiomeData> GetBiomeDistribution()
        {
#if SN1
            List<LootDistributionData.BiomeData> biomeDatas = new List<LootDistributionData.BiomeData>()
            {
                new LootDistributionData.BiomeData(){ biome = BiomeType.SafeShallows_TechSite_Scattered, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.JellyShroomCaves_AbandonedBase_Inside, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.JellyShroomCaves_AbandonedBase_Outside, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.JellyshroomCaves_CaveFloor, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.JellyshroomCaves_CaveSand, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.LostRiverJunction_Ground, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.LostRiverCorridor_Ground, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.LostRiverCorridor_LakeFloor, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.LostRiverJunction_LakeFloor, count = 1, probability = 0.2f },

            };
#elif BZ
            List<LootDistributionData.BiomeData> biomeDatas = new List<LootDistributionData.BiomeData>()
            {
                new LootDistributionData.BiomeData(){ biome = BiomeType.GlacialBasin_BikeCrashSite, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.GlacialBasin_Generic, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.GlacialConnection_Ground, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.Glacier_Generic, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.LilyPads_Crevice_Ground, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.LilyPads_Crevice_Grass, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.LilyPads_Deep_Grass, count = 1, probability = 0.2f },
                new LootDistributionData.BiomeData(){ biome = BiomeType.LilyPads_Deep_Ground, count = 1, probability = 0.2f },
            };
#endif
            return biomeDatas;
        }

        public override WorldEntityInfo EntityInfo => new WorldEntityInfo() {cellLevel = LargeWorldEntity.CellLevel.Medium, classId = ClassID, localScale = Vector3.one, prefabZUp = false, slotType = EntitySlot.Type.Small, techType = TechType };

        public override GameObject GetGameObject()
        {
            GameObject prefab = Main.assetBundle.LoadAsset<GameObject>("TechPistol.prefab");
            GameObject gameObject = GameObject.Instantiate(prefab);
            gameObject.SetActive(false);
            prefab.SetActive(false);

            gameObject.transform.localEulerAngles = new Vector3(90f, 0f, 0f);

            MeshRenderer[] componentsInChildren = gameObject.transform.Find("HandGun").gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in componentsInChildren)
            {
                if (meshRenderer.name.StartsWith("Gun"))
                {
                    Texture emissionMap = meshRenderer.material.GetTexture("_EmissionMap");

                    meshRenderer.material.shader = Shader.Find("MarmosetUBER");
                    meshRenderer.material.EnableKeyword("_Glow");
                    meshRenderer.material.SetTexture("_Illum", emissionMap);
                    meshRenderer.material.SetColor("_EmissionColor", new Color(1f, 1f, 1f));
                }
            }

            GameObject.Destroy(gameObject.transform.Find(PistolBehaviour.GunMain + "/ModeChange")?.gameObject);
            GameObject.Destroy(gameObject.transform.Find(PistolBehaviour.Point)?.gameObject);
            GameObject.Destroy(gameObject.GetComponent<PistolBehaviour>());

            SkyApplier skyApplier = gameObject.EnsureComponent<SkyApplier>();
            skyApplier.renderers = componentsInChildren;
            skyApplier.anchorSky = Skies.Auto;

            gameObject.EnsureComponent<PrefabIdentifier>().ClassId = base.ClassID;
            gameObject.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Medium;
            gameObject.EnsureComponent<TechTag>().type = base.TechType;

            return gameObject;
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> pistolFragment)
        {
            pistolFragment.Set(GetGameObject());
            yield break;
        }

        protected override Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromTexture(Main.assetBundle.LoadAsset<Texture2D>("Icon"));
        }
    }
}