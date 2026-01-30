using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000222 RID: 546
public class FossilSiteConfig_Ice : IEntityConfig
{
	// Token: 0x06000B0E RID: 2830 RVA: 0x00042B18 File Offset: 0x00040D18
	public GameObject CreatePrefab()
	{
		string id = "FossilIce";
		string name = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_ICE.NAME;
		string desc = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_ICE.DESC;
		float mass = 4000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("fossil_ice_kanim"), "object", Grid.SceneLayer.BuildingBack, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Fossil, true);
		component.Temperature = 230f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<Demolishable>().allowDemolition = false;
		gameObject.AddOrGetDef<MinorFossilDigSite.Def>().fossilQuestCriteriaID = FossilSiteConfig_Ice.FossilQuestCriteriaID;
		gameObject.AddOrGetDef<FossilHuntInitializer.Def>();
		gameObject.AddOrGet<MinorDigSiteWorkable>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x00042BF0 File Offset: 0x00040DF0
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<EntombVulnerable>().SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x00042C21 File Offset: 0x00040E21
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007BF RID: 1983
	public static readonly HashedString FossilQuestCriteriaID = "LostIceFossil";

	// Token: 0x040007C0 RID: 1984
	public const string ID = "FossilIce";
}
