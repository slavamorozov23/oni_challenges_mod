using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001EC RID: 492
public class KelpConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009F5 RID: 2549 RVA: 0x0003FBE8 File Offset: 0x0003DDE8
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x0003FBEF File Offset: 0x0003DDEF
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0003FBF4 File Offset: 0x0003DDF4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(KelpConfig.ID, ITEMS.INGREDIENTS.KELP.NAME, ITEMS.INGREDIENTS.KELP.DESC, 1f, false, Assets.GetAnim("kelp_leaf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.PedestalDisplayable
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x0003FC6F File Offset: 0x0003DE6F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x0003FC71 File Offset: 0x0003DE71
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000735 RID: 1845
	public static string ID = "Kelp";

	// Token: 0x04000736 RID: 1846
	public const float MASS_PER_UNIT = 1f;
}
