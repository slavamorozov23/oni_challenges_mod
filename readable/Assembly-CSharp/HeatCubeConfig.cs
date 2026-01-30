using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200032B RID: 811
public class HeatCubeConfig : IEntityConfig
{
	// Token: 0x060010C0 RID: 4288 RVA: 0x00064870 File Offset: 0x00062A70
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("HeatCube", "Heat Cube", "A cube that holds heat.", 1000f, true, Assets.GetAnim("copper_kanim"), "idle_tallstone", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, true, SORTORDER.BUILDINGELEMENTS, SimHashes.Diamond, new List<Tag>
		{
			GameTags.MiscPickupable,
			GameTags.IndustrialIngredient
		});
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x000648DE File Offset: 0x00062ADE
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x000648E0 File Offset: 0x00062AE0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AC0 RID: 2752
	public const string ID = "HeatCube";
}
