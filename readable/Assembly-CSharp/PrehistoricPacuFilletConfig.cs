using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public class PrehistoricPacuFilletConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000A2B RID: 2603 RVA: 0x0004024A File Offset: 0x0003E44A
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC4;
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x00040251 File Offset: 0x0003E451
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x00040254 File Offset: 0x0003E454
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("PrehistoricPacuFillet", STRINGS.ITEMS.FOOD.PREHISTORICPACUFILLET.NAME, STRINGS.ITEMS.FOOD.PREHISTORICPACUFILLET.DESC, 1f, false, Assets.GetAnim("jawboFillet_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.JAWBOFILLET);
		return gameObject;
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x000402BA File Offset: 0x0003E4BA
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x000402BC File Offset: 0x0003E4BC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400074D RID: 1869
	public const string ID = "PrehistoricPacuFillet";
}
