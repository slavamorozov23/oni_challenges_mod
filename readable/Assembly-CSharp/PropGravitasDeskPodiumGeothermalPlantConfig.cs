using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003BB RID: 955
public class PropGravitasDeskPodiumGeothermalPlantConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060013A9 RID: 5033 RVA: 0x00070D4E File Offset: 0x0006EF4E
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060013AA RID: 5034 RVA: 0x00070D55 File Offset: 0x0006EF55
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060013AB RID: 5035 RVA: 0x00070D58 File Offset: 0x0006EF58
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDeskPodiumGeothermalPlant";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESKPODIUM.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESKPODIUM.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_desk_podium_kanim"), "off", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new string[]
		{
			"dlc2geoplantinput"
		});
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060013AC RID: 5036 RVA: 0x00070E01 File Offset: 0x0006F001
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x00070E18 File Offset: 0x0006F018
	public void OnSpawn(GameObject inst)
	{
	}
}
