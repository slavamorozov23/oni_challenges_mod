using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class DewDripConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060007C6 RID: 1990 RVA: 0x00035349 File Offset: 0x00033549
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x00035350 File Offset: 0x00033550
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x00035354 File Offset: 0x00033554
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DewDripConfig.ID, ITEMS.INDUSTRIAL_PRODUCTS.DEWDRIP.NAME, ITEMS.INDUSTRIAL_PRODUCTS.DEWDRIP.DESC, 1f, true, Assets.GetAnim("brackorb_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient
		});
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<PrefabAttributeModifiers>().AddAttributeDescriptor(this.decorModifier);
		return gameObject;
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x000353D5 File Offset: 0x000335D5
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x000353D7 File Offset: 0x000335D7
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005E6 RID: 1510
	public static string ID = "DewDrip";

	// Token: 0x040005E7 RID: 1511
	private AttributeModifier decorModifier = new AttributeModifier("Decor", 0.1f, ITEMS.INDUSTRIAL_PRODUCTS.DEWDRIP.NAME, true, false, true);
}
