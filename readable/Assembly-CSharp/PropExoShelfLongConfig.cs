using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000408 RID: 1032
public class PropExoShelfLongConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001545 RID: 5445 RVA: 0x00079C52 File Offset: 0x00077E52
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x00079C59 File Offset: 0x00077E59
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x00079C5C File Offset: 0x00077E5C
	public GameObject CreatePrefab()
	{
		string id = "PropExoShelfLong";
		string name = STRINGS.BUILDINGS.PREFABS.PROPEXOSHELFLONG.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPEXOSHELFLONG.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("poi_shelf_long_kanim"), "off", Grid.SceneLayer.Building, 3, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x00079CEF File Offset: 0x00077EEF
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x00079D06 File Offset: 0x00077F06
	public void OnSpawn(GameObject inst)
	{
	}
}
