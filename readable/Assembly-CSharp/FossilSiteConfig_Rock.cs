using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000224 RID: 548
public class FossilSiteConfig_Rock : IEntityConfig
{
	// Token: 0x06000B18 RID: 2840 RVA: 0x00042D60 File Offset: 0x00040F60
	public GameObject CreatePrefab()
	{
		string id = "FossilRock";
		string name = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_ROCK.NAME;
		string desc = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_ROCK.DESC;
		float mass = 4000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("fossil_rock_kanim"), "object", Grid.SceneLayer.BuildingBack, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Fossil, true);
		component.Temperature = 315f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<Demolishable>().allowDemolition = false;
		gameObject.AddOrGetDef<MinorFossilDigSite.Def>().fossilQuestCriteriaID = FossilSiteConfig_Rock.FossilQuestCriteriaID;
		gameObject.AddOrGetDef<FossilHuntInitializer.Def>();
		gameObject.AddOrGet<MinorDigSiteWorkable>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x00042E38 File Offset: 0x00041038
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<EntombVulnerable>().SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x00042E69 File Offset: 0x00041069
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007C3 RID: 1987
	public static readonly HashedString FossilQuestCriteriaID = "LostRockFossil";

	// Token: 0x040007C4 RID: 1988
	public const string ID = "FossilRock";
}
