using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000091 RID: 145
public class WarmVestConfig : IEquipmentConfig
{
	// Token: 0x060002E3 RID: 739 RVA: 0x00014FFC File Offset: 0x000131FC
	public EquipmentDef CreateEquipmentDef()
	{
		new Dictionary<string, float>().Add("BasicFabric", (float)TUNING.EQUIPMENT.VESTS.WARM_VEST_MASS);
		ClothingWearer.ClothingInfo clothingInfo = ClothingWearer.ClothingInfo.WARM_CLOTHING;
		List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Warm_Vest", TUNING.EQUIPMENT.CLOTHING.SLOT, SimHashes.Carbon, (float)TUNING.EQUIPMENT.VESTS.WARM_VEST_MASS, TUNING.EQUIPMENT.VESTS.WARM_VEST_ICON0, TUNING.EQUIPMENT.VESTS.SNAPON0, TUNING.EQUIPMENT.VESTS.WARM_VEST_ANIM0, 4, attributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, null, null);
		int decorMod = ClothingWearer.ClothingInfo.WARM_CLOTHING.decorMod;
		Descriptor item = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.WARM_CLOTHING.conductivityMod)), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.WARM_CLOTHING.conductivityMod)), Descriptor.DescriptorType.Effect, false);
		Descriptor item2 = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, decorMod), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, decorMod), Descriptor.DescriptorType.Effect, false);
		equipmentDef.additionalDescriptors.Add(item);
		if (decorMod != 0)
		{
			equipmentDef.additionalDescriptors.Add(item2);
		}
		equipmentDef.OnEquipCallBack = delegate(Equippable eq)
		{
			ClothingWearer.ClothingInfo.OnEquipVest(eq, clothingInfo);
		};
		equipmentDef.OnUnequipCallBack = new Action<Equippable>(ClothingWearer.ClothingInfo.OnUnequipVest);
		equipmentDef.RecipeDescription = STRINGS.EQUIPMENT.PREFABS.WARM_VEST.RECIPE_DESC;
		return equipmentDef;
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0001514C File Offset: 0x0001334C
	public static void SetupVest(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
		Equippable equippable = go.GetComponent<Equippable>();
		if (equippable == null)
		{
			equippable = go.AddComponent<Equippable>();
		}
		equippable.SetQuality(global::QualityLevel.Poor);
		go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00015195 File Offset: 0x00013395
	public void DoPostConfigure(GameObject go)
	{
		WarmVestConfig.SetupVest(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.PedestalDisplayable, false);
	}

	// Token: 0x040001AD RID: 429
	public const string ID = "Warm_Vest";

	// Token: 0x040001AE RID: 430
	public static ComplexRecipe recipe;
}
