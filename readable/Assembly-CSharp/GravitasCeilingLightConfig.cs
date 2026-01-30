using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000248 RID: 584
public class GravitasCeilingLightConfig : IEntityConfig
{
	// Token: 0x06000BD5 RID: 3029 RVA: 0x000480C8 File Offset: 0x000462C8
	public GameObject CreatePrefab()
	{
		string id = "GravitasCeilingLight";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCEILINGLIGHT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCEILINGLIGHT.DESC;
		float mass = 25f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_light2_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, PermittedRotations.Unrotatable, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.IronOre, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextEmail));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x0004816F File Offset: 0x0004636F
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x00048186 File Offset: 0x00046386
	public void OnSpawn(GameObject inst)
	{
	}
}
