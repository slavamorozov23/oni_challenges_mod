using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200024D RID: 589
public class GravitasDeskConfig : IEntityConfig
{
	// Token: 0x06000BFB RID: 3067 RVA: 0x000488F4 File Offset: 0x00046AF4
	public GameObject CreatePrefab()
	{
		string id = "GravitasDesk";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASDESK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_desk2_kanim"), "off", Grid.SceneLayer.Building, 4, 3, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntryThenNext("story_trait_hijackheadquarters_complete", UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS.SEARCH6, new Action<InfoDialogScreen>(LoreBearerUtil.UnlockNextEmail), true));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x000489AE File Offset: 0x00046BAE
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x000489C5 File Offset: 0x00046BC5
	public void OnSpawn(GameObject inst)
	{
	}
}
