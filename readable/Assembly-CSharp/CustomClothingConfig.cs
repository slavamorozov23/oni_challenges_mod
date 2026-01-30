using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class CustomClothingConfig : IEquipmentConfig
{
	// Token: 0x060002BB RID: 699 RVA: 0x000138A4 File Offset: 0x00011AA4
	public EquipmentDef CreateEquipmentDef()
	{
		Dictionary<string, float> dictionary = new Dictionary<string, float>();
		dictionary.Add("Funky_Vest", (float)TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS);
		dictionary.Add("BasicFabric", 3f);
		ClothingWearer.ClothingInfo clothingInfo = ClothingWearer.ClothingInfo.CUSTOM_CLOTHING;
		List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("CustomClothing", TUNING.EQUIPMENT.CLOTHING.SLOT, SimHashes.Carbon, (float)TUNING.EQUIPMENT.VESTS.CUSTOM_CLOTHING_MASS, "shirt_decor01_kanim", TUNING.EQUIPMENT.VESTS.SNAPON0, "body_shirt_decor01_kanim", 4, attributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, null, null);
		Descriptor item = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.CUSTOM_CLOTHING.conductivityMod)), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.CUSTOM_CLOTHING.conductivityMod)), Descriptor.DescriptorType.Effect, false);
		Descriptor item2 = new Descriptor(string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, ClothingWearer.ClothingInfo.CUSTOM_CLOTHING.decorMod), string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, ClothingWearer.ClothingInfo.CUSTOM_CLOTHING.decorMod), Descriptor.DescriptorType.Effect, false);
		equipmentDef.additionalDescriptors.Add(item);
		equipmentDef.additionalDescriptors.Add(item2);
		equipmentDef.OnEquipCallBack = delegate(Equippable eq)
		{
			ClothingWearer.ClothingInfo.OnEquipVest(eq, clothingInfo);
		};
		equipmentDef.OnUnequipCallBack = new Action<Equippable>(ClothingWearer.ClothingInfo.OnUnequipVest);
		equipmentDef.RecipeDescription = STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.RECIPE_DESC;
		foreach (EquippableFacadeResource equippableFacadeResource in Db.GetEquippableFacades().resources)
		{
			if (!(equippableFacadeResource.DefID != "CustomClothing"))
			{
				TagManager.Create(equippableFacadeResource.Id, EquippableFacade.GetNameOverride("CustomClothing", equippableFacadeResource.Id));
			}
		}
		return equipmentDef;
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00013A7C File Offset: 0x00011C7C
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

	// Token: 0x060002BD RID: 701 RVA: 0x00013AC5 File Offset: 0x00011CC5
	public void DoPostConfigure(GameObject go)
	{
		CustomClothingConfig.SetupVest(go);
		go.GetComponent<KPrefabID>().AddTag(GameTags.PedestalDisplayable, false);
	}

	// Token: 0x04000198 RID: 408
	public const string ID = "CustomClothing";

	// Token: 0x04000199 RID: 409
	public static ComplexRecipe recipe;
}
