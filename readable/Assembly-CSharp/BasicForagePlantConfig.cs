using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class BasicForagePlantConfig : IEntityConfig
{
	// Token: 0x06000781 RID: 1921 RVA: 0x000339E0 File Offset: 0x00031BE0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BasicForagePlant", STRINGS.ITEMS.FOOD.BASICFORAGEPLANT.NAME, STRINGS.ITEMS.FOOD.BASICFORAGEPLANT.DESC, 1f, false, Assets.GetAnim("muckrootvegetable_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.3f, 0.3f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.BASICFORAGEPLANT);
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00033A44 File Offset: 0x00031C44
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x00033A46 File Offset: 0x00031C46
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005AF RID: 1455
	public const string ID = "BasicForagePlant";
}
