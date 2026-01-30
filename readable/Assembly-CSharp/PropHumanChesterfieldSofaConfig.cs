using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200040C RID: 1036
public class PropHumanChesterfieldSofaConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600155B RID: 5467 RVA: 0x0007A06C File Offset: 0x0007826C
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x0007A073 File Offset: 0x00078273
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x0007A078 File Offset: 0x00078278
	public GameObject CreatePrefab()
	{
		string id = "PropHumanChesterfieldSofa";
		string name = STRINGS.BUILDINGS.PREFABS.PROPHUMANCHESTERFIELDSOFA.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPHUMANCHESTERFIELDSOFA.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_couch_kanim"), "off", Grid.SceneLayer.Building, 3, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x0007A10B File Offset: 0x0007830B
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x0007A122 File Offset: 0x00078322
	public void OnSpawn(GameObject inst)
	{
	}
}
