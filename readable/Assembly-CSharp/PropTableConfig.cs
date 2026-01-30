using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003D4 RID: 980
public class PropTableConfig : IEntityConfig
{
	// Token: 0x06001421 RID: 5153 RVA: 0x0007261C File Offset: 0x0007081C
	public GameObject CreatePrefab()
	{
		string id = "PropTable";
		string name = STRINGS.BUILDINGS.PREFABS.PROPTABLE.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPTABLE.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("table_breakroom_kanim"), "off", Grid.SceneLayer.Building, 3, 1, tier, PermittedRotations.R90, Orientation.Neutral, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Unobtanium, true);
		component.Temperature = 294.15f;
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextJournalEntry));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x000726C3 File Offset: 0x000708C3
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x000726DA File Offset: 0x000708DA
	public void OnSpawn(GameObject inst)
	{
	}
}
