using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001E2 RID: 482
public class DinosaurMeatConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060009C6 RID: 2502 RVA: 0x0003F754 File Offset: 0x0003D954
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0003F75B File Offset: 0x0003D95B
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x0003F760 File Offset: 0x0003D960
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("DinosaurMeat", STRINGS.ITEMS.FOOD.DINOSAURMEAT.NAME, STRINGS.ITEMS.FOOD.DINOSAURMEAT.DESC, 1f, false, Assets.GetAnim("dinomeat_raw_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.DINOSAURMEAT);
		return gameObject;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0003F7C6 File Offset: 0x0003D9C6
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x0003F7C8 File Offset: 0x0003D9C8
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000724 RID: 1828
	public const string ID = "DinosaurMeat";
}
