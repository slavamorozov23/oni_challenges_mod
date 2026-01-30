using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000329 RID: 809
public class GoldBellyCrownConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060010B3 RID: 4275 RVA: 0x000630B9 File Offset: 0x000612B9
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x000630C0 File Offset: 0x000612C0
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x000630C4 File Offset: 0x000612C4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("GoldBellyCrown", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.GOLD_BELLY_CROWN.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.GOLD_BELLY_CROWN.DESC, 250f, true, Assets.GetAnim("bammoth_crown_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.5f, true, 0, SimHashes.GoldAmalgam, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.BONUS.TIER2);
		decorProvider.overrideName = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.GOLD_BELLY_CROWN.NAME;
		return gameObject;
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x00063171 File Offset: 0x00061371
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x00063173 File Offset: 0x00061373
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A9D RID: 2717
	public const string ID = "GoldBellyCrown";

	// Token: 0x04000A9E RID: 2718
	public const float MASS_PER_UNIT = 250f;
}
