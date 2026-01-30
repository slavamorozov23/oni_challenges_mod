using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003BC RID: 956
public class PropGravitasDisplay4Config : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060013AF RID: 5039 RVA: 0x00070E22 File Offset: 0x0006F022
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060013B0 RID: 5040 RVA: 0x00070E29 File Offset: 0x0006F029
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060013B1 RID: 5041 RVA: 0x00070E2C File Offset: 0x0006F02C
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDisplay4";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDISPLAY4.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDISPLAY4.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_display4_kanim"), "off", Grid.SceneLayer.Building, 1, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextDimensionalLore));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x00070ED1 File Offset: 0x0006F0D1
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x00070EE8 File Offset: 0x0006F0E8
	public void OnSpawn(GameObject inst)
	{
	}
}
