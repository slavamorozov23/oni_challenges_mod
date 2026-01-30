using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003C3 RID: 963
public class PropGravitasLabTableConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060013D2 RID: 5074 RVA: 0x0007140E File Offset: 0x0006F60E
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x00071415 File Offset: 0x0006F615
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x00071418 File Offset: 0x0006F618
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasLabTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASLABTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASLABTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_lab_table_kanim"), "off", Grid.SceneLayer.Building, 3, 2, tier, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x060013D5 RID: 5077 RVA: 0x000714B1 File Offset: 0x0006F6B1
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x000714C8 File Offset: 0x0006F6C8
	public void OnSpawn(GameObject inst)
	{
	}
}
