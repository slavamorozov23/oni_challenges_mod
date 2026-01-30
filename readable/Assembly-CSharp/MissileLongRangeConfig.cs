using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200033E RID: 830
public class MissileLongRangeConfig : IEntityConfig
{
	// Token: 0x0600112C RID: 4396 RVA: 0x0006644C File Offset: 0x0006464C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("MissileLongRange", ITEMS.MISSILE_LONGRANGE.NAME, ITEMS.MISSILE_LONGRANGE.DESC, 200f, true, Assets.GetAnim("longrange_missile_kanim"), "object", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 1f, true, 0, SimHashes.Iron, new List<Tag>());
		gameObject.AddTag(GameTags.LongRangeMissile);
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddTag(GameTags.PedestalDisplayable);
		MissileLongRangeProjectile.Def def = gameObject.AddOrGetDef<MissileLongRangeProjectile.Def>();
		def.starmapOverrideSymbol = "payload";
		def.missileName = "STRINGS.ITEMS.MISSILE_LONGRANGE.NAME";
		def.missileDesc = "STRINGS.ITEMS.MISSILE_LONGRANGE.DESC";
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 200f;
		return gameObject;
	}

	// Token: 0x0600112D RID: 4397 RVA: 0x00066501 File Offset: 0x00064701
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x00066503 File Offset: 0x00064703
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AF6 RID: 2806
	public const string ID = "MissileLongRange";

	// Token: 0x04000AF7 RID: 2807
	public const float MASS_PER_MISSILE = 200f;

	// Token: 0x04000AF8 RID: 2808
	public const int DAMAGE_PER_MISSILE = 10;

	// Token: 0x02001232 RID: 4658
	public class DamageEventPayload
	{
		// Token: 0x0600873B RID: 34619 RVA: 0x0034B1EC File Offset: 0x003493EC
		public DamageEventPayload(int damage = 10)
		{
			this.damage = damage;
		}

		// Token: 0x04006719 RID: 26393
		public int damage;

		// Token: 0x0400671A RID: 26394
		public static MissileLongRangeConfig.DamageEventPayload sharedInstance = new MissileLongRangeConfig.DamageEventPayload(10);
	}
}
