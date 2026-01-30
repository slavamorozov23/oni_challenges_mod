using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public class DehydratedBerryPieConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000971 RID: 2417 RVA: 0x0003EE90 File Offset: 0x0003D090
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0003EE97 File Offset: 0x0003D097
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0003EE9A File Offset: 0x0003D09A
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0003EE9C File Offset: 0x0003D09C
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x0003EEA0 File Offset: 0x0003D0A0
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_berry_pie_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedBerryPieConfig.ID.Name, STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.BERRY_PIE);
		return gameObject;
	}

	// Token: 0x040006FE RID: 1790
	public static Tag ID = new Tag("DehydratedBerryPie");

	// Token: 0x040006FF RID: 1791
	public const float MASS = 1f;

	// Token: 0x04000700 RID: 1792
	public const string ANIM_FILE = "dehydrated_food_berry_pie_kanim";

	// Token: 0x04000701 RID: 1793
	public const string INITIAL_ANIM = "idle";
}
