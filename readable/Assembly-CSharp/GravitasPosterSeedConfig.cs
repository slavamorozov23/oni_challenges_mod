using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000255 RID: 597
public class GravitasPosterSeedConfig : IEntityConfig
{
	// Token: 0x06000C1D RID: 3101 RVA: 0x00049034 File Offset: 0x00047234
	public GameObject CreatePrefab()
	{
		string id = "GravitasPosterSeed";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASPOSTERSEED.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASPOSTERSEED.DESC;
		float mass = 25f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_poster_seed_plant_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, PermittedRotations.Unrotatable, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, LoreBearerUtil.UnlockSpecificEntryThenNext("story_trait_hijackheadquarters_initial", UI.USERMENUACTIONS.READLORE.SEARCH_MIRROR_SUCCESS.SEARCH1, new Action<InfoDialogScreen>(LoreBearerUtil.UnlockNextEmail), true));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x000490F0 File Offset: 0x000472F0
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x00049107 File Offset: 0x00047307
	public void OnSpawn(GameObject inst)
	{
	}
}
