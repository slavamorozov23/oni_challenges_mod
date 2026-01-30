using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class GarbageElectrobankConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060010A4 RID: 4260 RVA: 0x00062EF4 File Offset: 0x000610F4
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x00062EFB File Offset: 0x000610FB
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x00062F00 File Offset: 0x00061100
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("GarbageElectrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_GARBAGE.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_GARBAGE.DESC, 20f, true, Assets.GetAnim("electrobank_large_destroyed_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Katairite, new List<Tag>
		{
			GameTags.PedestalDisplayable
		});
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x00062F9D File Offset: 0x0006119D
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x00062F9F File Offset: 0x0006119F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A97 RID: 2711
	public const string ID = "GarbageElectrobank";

	// Token: 0x04000A98 RID: 2712
	public const float MASS = 20f;
}
