using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000258 RID: 600
public class GravitasTrashCanConfig : IEntityConfig
{
	// Token: 0x06000C29 RID: 3113 RVA: 0x00049280 File Offset: 0x00047480
	public GameObject CreatePrefab()
	{
		string id = "GravitasTrashCan";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTRASHCAN.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASTRASHCAN.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_trash_can_kanim"), "on", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x00049313 File Offset: 0x00047513
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x0004932A File Offset: 0x0004752A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400085F RID: 2143
	public const string ID = "GravitasTrashCan";
}
