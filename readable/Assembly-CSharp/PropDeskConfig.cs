using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003A4 RID: 932
public class PropDeskConfig : IEntityConfig
{
	// Token: 0x06001342 RID: 4930 RVA: 0x0006F7A8 File Offset: 0x0006D9A8
	public GameObject CreatePrefab()
	{
		string id = "PropDesk";
		string name = STRINGS.BUILDINGS.PREFABS.PROPDESK.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPDESK.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("setpiece_desk_kanim"), "off", Grid.SceneLayer.Building, 3, 2, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextEmail));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x0006F84F File Offset: 0x0006DA4F
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x0006F866 File Offset: 0x0006DA66
	public void OnSpawn(GameObject inst)
	{
	}
}
