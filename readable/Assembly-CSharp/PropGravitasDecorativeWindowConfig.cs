using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B8 RID: 952
public class PropGravitasDecorativeWindowConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001398 RID: 5016 RVA: 0x00070AE7 File Offset: 0x0006ECE7
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x00070AEE File Offset: 0x0006ECEE
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600139A RID: 5018 RVA: 0x00070AF4 File Offset: 0x0006ECF4
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDecorativeWindow";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDECORATIVEWINDOW.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDECORATIVEWINDOW.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER2;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_top_window_kanim"), "on", Grid.SceneLayer.Building, 2, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Glass, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600139B RID: 5019 RVA: 0x00070B87 File Offset: 0x0006ED87
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x0600139C RID: 5020 RVA: 0x00070B9E File Offset: 0x0006ED9E
	public void OnSpawn(GameObject inst)
	{
	}
}
