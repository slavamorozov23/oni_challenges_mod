using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001EE RID: 494
public class MeatConfig : IEntityConfig
{
	// Token: 0x06000A00 RID: 2560 RVA: 0x0003FCF8 File Offset: 0x0003DEF8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Meat", STRINGS.ITEMS.FOOD.MEAT.NAME, STRINGS.ITEMS.FOOD.MEAT.DESC, 1f, false, Assets.GetAnim("creaturemeat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.MEAT);
		return gameObject;
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0003FD5E File Offset: 0x0003DF5E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x0003FD60 File Offset: 0x0003DF60
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000738 RID: 1848
	public const string ID = "Meat";
}
