using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000321 RID: 801
public class ElectrobankConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001088 RID: 4232 RVA: 0x00062B1B File Offset: 0x00060D1B
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06001089 RID: 4233 RVA: 0x00062B22 File Offset: 0x00060D22
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600108A RID: 4234 RVA: 0x00062B28 File Offset: 0x00060D28
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Electrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK.DESC, 20f, true, Assets.GetAnim("electrobank_large_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Katairite, new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.PedestalDisplayable
		});
		if (!Assets.IsTagCountable(GameTags.ChargedPortableBattery))
		{
			Assets.AddCountableTag(GameTags.ChargedPortableBattery);
		}
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddComponent<Electrobank>().rechargeable = true;
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x0600108B RID: 4235 RVA: 0x00062BEB File Offset: 0x00060DEB
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x00062BED File Offset: 0x00060DED
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A8E RID: 2702
	public const string ID = "Electrobank";

	// Token: 0x04000A8F RID: 2703
	public const float MASS = 20f;

	// Token: 0x04000A90 RID: 2704
	public const float POWER_CAPACITY = 120000f;

	// Token: 0x04000A91 RID: 2705
	public static ComplexRecipe recipe;
}
