using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class BasicFabricConfig : IEntityConfig
{
	// Token: 0x06000777 RID: 1911 RVA: 0x000336D4 File Offset: 0x000318D4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(BasicFabricConfig.ID, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.DESC, 1f, true, Assets.GetAnim("swampreedwool_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + BasicFabricTuning.SORTORDER, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.BuildingFiber,
			GameTags.PedestalDisplayable
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<PrefabAttributeModifiers>().AddAttributeDescriptor(this.decorModifier);
		return gameObject;
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00033775 File Offset: 0x00031975
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00033777 File Offset: 0x00031977
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005AA RID: 1450
	public static string ID = "BasicFabric";

	// Token: 0x040005AB RID: 1451
	private AttributeModifier decorModifier = new AttributeModifier("Decor", 0.1f, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME, true, false, true);
}
