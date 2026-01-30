using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000327 RID: 807
public class GasGrassHarvestedConfig : IEntityConfig
{
	// Token: 0x060010AA RID: 4266 RVA: 0x00062FAC File Offset: 0x000611AC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("GasGrassHarvested", CREATURES.SPECIES.GASGRASS.NAME, CREATURES.SPECIES.GASGRASS.DESC, 1f, false, Assets.GetAnim("harvested_gassygrass_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.Other
		});
		gameObject.AddOrGet<EntitySplitter>();
		return gameObject;
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x0006301C File Offset: 0x0006121C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x0006301E File Offset: 0x0006121E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A99 RID: 2713
	public const string ID = "GasGrassHarvested";
}
