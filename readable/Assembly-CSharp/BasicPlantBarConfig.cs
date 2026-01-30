using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class BasicPlantBarConfig : IEntityConfig
{
	// Token: 0x06000963 RID: 2403 RVA: 0x0003ED34 File Offset: 0x0003CF34
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BasicPlantBar", STRINGS.ITEMS.FOOD.BASICPLANTBAR.NAME, STRINGS.ITEMS.FOOD.BASICPLANTBAR.DESC, 1f, false, Assets.GetAnim("liceloaf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BASICPLANTBAR);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x0003ED98 File Offset: 0x0003CF98
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0003ED9A File Offset: 0x0003CF9A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006F8 RID: 1784
	public const string ID = "BasicPlantBar";

	// Token: 0x040006F9 RID: 1785
	public const string ANIM = "liceloaf_kanim";

	// Token: 0x040006FA RID: 1786
	public static ComplexRecipe recipe;
}
