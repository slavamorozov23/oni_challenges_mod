using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A6 RID: 934
public class PropDlc2GeothermalCartConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600134C RID: 4940 RVA: 0x0006F942 File Offset: 0x0006DB42
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x0600134D RID: 4941 RVA: 0x0006F949 File Offset: 0x0006DB49
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x0006F94C File Offset: 0x0006DB4C
	public GameObject CreatePrefab()
	{
		string id = "PropDlc2GeothermalCart";
		string name = STRINGS.BUILDINGS.PREFABS.PROPDLC2GEOTHERMALCART.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPDLC2GEOTHERMALCART.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_geothermal_cart_kanim"), "on", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject);
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x0006F9E5 File Offset: 0x0006DBE5
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x0006F9FC File Offset: 0x0006DBFC
	public void OnSpawn(GameObject inst)
	{
	}
}
