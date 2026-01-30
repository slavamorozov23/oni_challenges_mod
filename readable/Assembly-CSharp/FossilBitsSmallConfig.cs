using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200021B RID: 539
public class FossilBitsSmallConfig : IEntityConfig
{
	// Token: 0x06000AEC RID: 2796 RVA: 0x00041E50 File Offset: 0x00040050
	public GameObject CreatePrefab()
	{
		string id = "FossilBitsSmall";
		string name = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_BITS.NAME;
		string desc = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_BITS.DESC;
		float mass = 1500f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("fossil_bits1x2_kanim"), "object", Grid.SceneLayer.BuildingBack, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Fossil, true);
		component.Temperature = 315f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<FossilBits>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00041F05 File Offset: 0x00040105
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		EntombVulnerable component = inst.GetComponent<EntombVulnerable>();
		component.SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
		component.SetShowStatusItemOnEntombed(false);
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x00041F3D File Offset: 0x0004013D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007AF RID: 1967
	public const string ID = "FossilBitsSmall";
}
