using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200032C RID: 812
public class IceBellyPoopConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060010C4 RID: 4292 RVA: 0x000648EA File Offset: 0x00062AEA
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x000648F1 File Offset: 0x00062AF1
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x000648F4 File Offset: 0x00062AF4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("IceBellyPoop", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.DESC, 100f, false, Assets.GetAnim("bammoth_poop_kanim"), "idle3", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>().offset = new Vector2(0f, 0.05f);
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.PENALTY.TIER3);
		decorProvider.overrideName = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.NAME;
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x000649BB File Offset: 0x00062BBB
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x000649BD File Offset: 0x00062BBD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AC1 RID: 2753
	public const string ID = "IceBellyPoop";
}
