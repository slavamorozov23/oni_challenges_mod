using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001EF RID: 495
public class MushBarConfig : IEntityConfig
{
	// Token: 0x06000A04 RID: 2564 RVA: 0x0003FD6C File Offset: 0x0003DF6C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("MushBar", STRINGS.ITEMS.FOOD.MUSHBAR.NAME, STRINGS.ITEMS.FOOD.MUSHBAR.DESC, 1f, false, Assets.GetAnim("mushbar_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.MUSHBAR);
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x0003FDD0 File Offset: 0x0003DFD0
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x0003FDD2 File Offset: 0x0003DFD2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000739 RID: 1849
	public const string ID = "MushBar";

	// Token: 0x0400073A RID: 1850
	public const string ANIM = "mushbar_kanim";

	// Token: 0x0400073B RID: 1851
	public static ComplexRecipe recipe;
}
