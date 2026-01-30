using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000254 RID: 596
public class GravitasPosterPlantsConfig : IEntityConfig
{
	// Token: 0x06000C19 RID: 3097 RVA: 0x00048F6C File Offset: 0x0004716C
	public GameObject CreatePrefab()
	{
		string id = "GravitasPosterPlants";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASPOSTERPLANTS.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASPOSTERPLANTS.DESC;
		float mass = 25f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_poster_plant_growth_kanim"), "off", Grid.SceneLayer.Building, 2, 2, tier, PermittedRotations.Unrotatable, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextEmail));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x00049013 File Offset: 0x00047213
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x0004902A File Offset: 0x0004722A
	public void OnSpawn(GameObject inst)
	{
	}
}
