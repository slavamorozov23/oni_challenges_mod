using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000200 RID: 512
public class SmokedDinosaurMeatConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000A56 RID: 2646 RVA: 0x0004081A File Offset: 0x0003EA1A
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x00040821 File Offset: 0x0003EA21
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x00040824 File Offset: 0x0003EA24
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SmokedDinosaurMeat", STRINGS.ITEMS.FOOD.SMOKEDDINOSAURMEAT.NAME, STRINGS.ITEMS.FOOD.SMOKEDDINOSAURMEAT.DESC, 1f, false, Assets.GetAnim("dinobrisket_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SMOKED_DINOSAURMEAT);
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x00040888 File Offset: 0x0003EA88
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0004088A File Offset: 0x0003EA8A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000760 RID: 1888
	public const string ID = "SmokedDinosaurMeat";

	// Token: 0x04000761 RID: 1889
	public static ComplexRecipe recipe;
}
