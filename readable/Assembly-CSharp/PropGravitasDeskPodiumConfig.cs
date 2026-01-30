using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B9 RID: 953
public class PropGravitasDeskPodiumConfig : IEntityConfig
{
	// Token: 0x0600139E RID: 5022 RVA: 0x00070BA8 File Offset: 0x0006EDA8
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasDeskPodium";
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
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextDeskPodiumEntry));
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x00070C4D File Offset: 0x0006EE4D
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x00070C64 File Offset: 0x0006EE64
	public void OnSpawn(GameObject inst)
	{
	}
}
