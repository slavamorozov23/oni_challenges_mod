using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000328 RID: 808
public class GeneShufflerRechargeConfig : IEntityConfig
{
	// Token: 0x060010AE RID: 4270 RVA: 0x00063028 File Offset: 0x00061228
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("GeneShufflerRecharge", ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME, ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.DESC, 5f, true, Assets.GetAnim("vacillator_charge_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IndustrialIngredient,
			GameTags.PedestalDisplayable
		});
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x0006309C File Offset: 0x0006129C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x0006309E File Offset: 0x0006129E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A9A RID: 2714
	public const string ID = "GeneShufflerRecharge";

	// Token: 0x04000A9B RID: 2715
	public static readonly Tag tag = TagManager.Create("GeneShufflerRecharge");

	// Token: 0x04000A9C RID: 2716
	public const float MASS = 5f;
}
