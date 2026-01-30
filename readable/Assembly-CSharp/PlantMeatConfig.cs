using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F6 RID: 502
public class PlantMeatConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x000401CD File Offset: 0x0003E3CD
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x000401D4 File Offset: 0x0003E3D4
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x000401D8 File Offset: 0x0003E3D8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PlantMeat", STRINGS.ITEMS.FOOD.PLANTMEAT.NAME, STRINGS.ITEMS.FOOD.PLANTMEAT.DESC, 1f, false, Assets.GetAnim("critter_trap_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.PLANTMEAT);
		return gameObject;
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x0004023E File Offset: 0x0003E43E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x00040240 File Offset: 0x0003E440
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400074C RID: 1868
	public const string ID = "PlantMeat";
}
