using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class GravitasClockSquareConfig : IEntityConfig
{
	// Token: 0x06000BD9 RID: 3033 RVA: 0x00048190 File Offset: 0x00046390
	public GameObject CreatePrefab()
	{
		string id = "GravitasClockSquare";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCLOCKSQUARE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASCLOCKSQUARE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_clock_square_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, PermittedRotations.Unrotatable, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Glass, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextEmail));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x00048237 File Offset: 0x00046437
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x0004824E File Offset: 0x0004644E
	public void OnSpawn(GameObject inst)
	{
	}
}
