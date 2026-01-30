using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200033C RID: 828
public class MissileBasicConfig : IEntityConfig
{
	// Token: 0x0600111F RID: 4383 RVA: 0x00065ED8 File Offset: 0x000640D8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("MissileBasic", ITEMS.MISSILE_BASIC.NAME, ITEMS.MISSILE_BASIC.DESC, 10f, true, Assets.GetAnim("missile_kanim"), "object", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Iron, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGetDef<MissileProjectile.Def>();
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 50f;
		return gameObject;
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x00065F63 File Offset: 0x00064163
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x00065F65 File Offset: 0x00064165
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AF1 RID: 2801
	public const string ID = "MissileBasic";

	// Token: 0x04000AF2 RID: 2802
	public const float MASS_PER_MISSILE = 10f;
}
