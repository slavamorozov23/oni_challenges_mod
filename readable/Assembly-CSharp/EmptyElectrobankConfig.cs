using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000322 RID: 802
public class EmptyElectrobankConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600108E RID: 4238 RVA: 0x00062BF7 File Offset: 0x00060DF7
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x0600108F RID: 4239 RVA: 0x00062BFE File Offset: 0x00060DFE
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x00062C04 File Offset: 0x00060E04
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("EmptyElectrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_EMPTY.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_EMPTY.DESC, 20f, true, Assets.GetAnim("electrobank_large_depleted_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.Katairite, new List<Tag>
		{
			GameTags.EmptyPortableBattery,
			GameTags.PedestalDisplayable
		});
		if (!Assets.IsTagCountable(GameTags.EmptyPortableBattery))
		{
			Assets.AddCountableTag(GameTags.EmptyPortableBattery);
		}
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x00062CC2 File Offset: 0x00060EC2
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x00062CC4 File Offset: 0x00060EC4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A92 RID: 2706
	public const string ID = "EmptyElectrobank";

	// Token: 0x04000A93 RID: 2707
	public const float MASS = 20f;
}
