using System;
using UnityEngine;

// Token: 0x0200027A RID: 634
public class WoodLogConfig : IOreConfig
{
	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0004CBBC File Offset: 0x0004ADBC
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.WoodLog;
		}
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x0004CBC4 File Offset: 0x0004ADC4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.prefabInitFn += this.OnInit;
		component.prefabSpawnFn += this.OnSpawn;
		component.RemoveTag(GameTags.HideFromSpawnTool);
		return gameObject;
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0004CC11 File Offset: 0x0004AE11
	public void OnInit(GameObject inst)
	{
		PrimaryElement component = inst.GetComponent<PrimaryElement>();
		component.SetElement(this.ElementID, true);
		Element element = component.Element;
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x0004CC2C File Offset: 0x0004AE2C
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<PrimaryElement>().SetElement(this.ElementID, true);
	}

	// Token: 0x040008E5 RID: 2277
	public const string ID = "WoodLog";

	// Token: 0x040008E6 RID: 2278
	public const float C02MassEmissionWhenBurned = 0.142f;

	// Token: 0x040008E7 RID: 2279
	public const float HeatWhenBurned = 7500f;

	// Token: 0x040008E8 RID: 2280
	public const float EnergyWhenBurned = 250f;

	// Token: 0x040008E9 RID: 2281
	public static readonly Tag TAG = TagManager.Create("WoodLog");
}
