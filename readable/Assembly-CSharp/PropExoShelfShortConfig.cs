using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000409 RID: 1033
public class PropExoShelfShortConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600154B RID: 5451 RVA: 0x00079D10 File Offset: 0x00077F10
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x00079D17 File Offset: 0x00077F17
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x00079D1C File Offset: 0x00077F1C
	public GameObject CreatePrefab()
	{
		string id = "PropExoShelfShort";
		string name = STRINGS.BUILDINGS.PREFABS.PROPEXOSHELSHORT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPEXOSHELSHORT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_shelf_short_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x00079DAF File Offset: 0x00077FAF
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x00079DC6 File Offset: 0x00077FC6
	public void OnSpawn(GameObject inst)
	{
	}
}
