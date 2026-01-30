using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class FeatherFabricConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060007DD RID: 2013 RVA: 0x00035A94 File Offset: 0x00033C94
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00035A9B File Offset: 0x00033C9B
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00035AA0 File Offset: 0x00033CA0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(FeatherFabricConfig.ID, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.FEATHER_FABRIC.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.FEATHER_FABRIC.DESC, 1f, true, Assets.GetAnim("feather_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + BasicFabricTuning.SORTORDER, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.BuildingFiber,
			GameTags.PedestalDisplayable
		});
		gameObject.AddOrGet<EntitySplitter>();
		KBoxCollider2D kboxCollider2D = gameObject.AddOrGet<KBoxCollider2D>();
		kboxCollider2D.offset = new Vector2f(0f, 0.3f);
		kboxCollider2D.size = new Vector2f(0.8f, 0.8f);
		gameObject.AddOrGet<PrefabAttributeModifiers>().AddAttributeDescriptor(this.decorModifier);
		return gameObject;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00035B7A File Offset: 0x00033D7A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00035B7C File Offset: 0x00033D7C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F5 RID: 1525
	public static string ID = "FeatherFabric";

	// Token: 0x040005F6 RID: 1526
	private AttributeModifier decorModifier = new AttributeModifier("Decor", 0.1f, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME, true, false, true);
}
