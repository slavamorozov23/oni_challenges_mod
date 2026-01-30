using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000223 RID: 547
public class FossilSiteConfig_Resin : IEntityConfig
{
	// Token: 0x06000B13 RID: 2835 RVA: 0x00042C3C File Offset: 0x00040E3C
	public GameObject CreatePrefab()
	{
		string id = "FossilResin";
		string name = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_RESIN.NAME;
		string desc = CODEX.STORY_TRAITS.FOSSILHUNT.ENTITIES.FOSSIL_RESIN.DESC;
		float mass = 4000f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("fossil_resin_kanim"), "object", Grid.SceneLayer.BuildingBack, 2, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Fossil, true);
		component.Temperature = 315f;
		gameObject.AddOrGet<Operational>();
		gameObject.AddOrGet<EntombVulnerable>();
		gameObject.AddOrGet<Demolishable>().allowDemolition = false;
		gameObject.AddOrGetDef<MinorFossilDigSite.Def>().fossilQuestCriteriaID = FossilSiteConfig_Resin.FossilQuestCriteriaID;
		gameObject.AddOrGetDef<FossilHuntInitializer.Def>();
		gameObject.AddOrGet<MinorDigSiteWorkable>();
		gameObject.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		return gameObject;
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x00042D14 File Offset: 0x00040F14
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<EntombVulnerable>().SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x00042D45 File Offset: 0x00040F45
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040007C1 RID: 1985
	public static readonly HashedString FossilQuestCriteriaID = "LostResinFossil";

	// Token: 0x040007C2 RID: 1986
	public const string ID = "FossilResin";
}
