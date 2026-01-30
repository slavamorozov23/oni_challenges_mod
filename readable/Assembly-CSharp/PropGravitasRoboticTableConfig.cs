using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003C7 RID: 967
public class PropGravitasRoboticTableConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060013E5 RID: 5093 RVA: 0x000717F1 File Offset: 0x0006F9F1
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x000717F8 File Offset: 0x0006F9F8
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x000717FC File Offset: 0x0006F9FC
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasRobitcTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASROBTICTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASROBTICTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_robotic_table_kanim"), "off", Grid.SceneLayer.Building, 3, 3, tier, tier2, SimHashes.Creature, new List<Tag>
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

	// Token: 0x060013E8 RID: 5096 RVA: 0x00071895 File Offset: 0x0006FA95
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x000718AC File Offset: 0x0006FAAC
	public void OnSpawn(GameObject inst)
	{
	}
}
