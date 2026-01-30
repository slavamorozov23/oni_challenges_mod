using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E4 RID: 484
public class FieldRationConfig : IEntityConfig
{
	// Token: 0x060009D3 RID: 2515 RVA: 0x0003F858 File Offset: 0x0003DA58
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FieldRation", STRINGS.ITEMS.FOOD.FIELDRATION.NAME, STRINGS.ITEMS.FOOD.FIELDRATION.DESC, 1f, false, Assets.GetAnim("fieldration_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FIELDRATION);
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x0003F8BC File Offset: 0x0003DABC
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x0003F8BE File Offset: 0x0003DABE
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000726 RID: 1830
	public const string ID = "FieldRation";
}
