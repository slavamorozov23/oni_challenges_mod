using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200040D RID: 1037
public class PropHumanMurphyBedConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001561 RID: 5473 RVA: 0x0007A12C File Offset: 0x0007832C
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001562 RID: 5474 RVA: 0x0007A133 File Offset: 0x00078333
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x0007A138 File Offset: 0x00078338
	public GameObject CreatePrefab()
	{
		string id = "PropHumanMurphyBed";
		string name = STRINGS.BUILDINGS.PREFABS.PROPHUMANMURPHYBED.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPHUMANMURPHYBED.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_murphybed_kanim"), "on", Grid.SceneLayer.Building, 5, 4, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x0007A1CB File Offset: 0x000783CB
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x0007A1E2 File Offset: 0x000783E2
	public void OnSpawn(GameObject inst)
	{
	}
}
