using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class CreatureFeederConfig : IBuildingConfig
{
	// Token: 0x060001E7 RID: 487 RVA: 0x0000DE74 File Offset: 0x0000C074
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CreatureFeeder";
		int width = 1;
		int height = 2;
		string anim = "feeder_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000DEC5 File Offset: 0x0000C0C5
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000DEC8 File Offset: 0x0000C0C8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 2000f;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.allowItemRemoval = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		go.AddOrGet<StorageLocker>().choreTypeID = Db.Get().ChoreTypes.RanchingFetch.Id;
		go.AddOrGet<UserNameable>();
		go.AddOrGet<TreeFilterable>();
		go.AddOrGet<CreatureFeeder>();
		go.GetComponent<KPrefabID>().prefabInitFn += this.OnPrefabInit;
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000DF60 File Offset: 0x0000C160
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<StorageController.Def>();
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0000DF6C File Offset: 0x0000C16C
	public override void ConfigurePost(BuildingDef def)
	{
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, Diet> keyValuePair in DietManager.CollectDiets(new Tag[]
		{
			GameTags.Creatures.Species.LightBugSpecies,
			GameTags.Creatures.Species.HatchSpecies,
			GameTags.Creatures.Species.MoleSpecies,
			GameTags.Creatures.Species.CrabSpecies,
			GameTags.Creatures.Species.StaterpillarSpecies,
			GameTags.Creatures.Species.DivergentSpecies,
			GameTags.Creatures.Species.DeerSpecies,
			GameTags.Creatures.Species.BellySpecies,
			GameTags.Creatures.Species.SealSpecies,
			GameTags.Creatures.Species.StegoSpecies,
			GameTags.Creatures.Species.RaptorSpecies,
			GameTags.Creatures.Species.ChameleonSpecies,
			GameTags.Creatures.Species.MooSpecies
		}))
		{
			Diet value = keyValuePair.Value;
			if (value.CanEatPreyCritter)
			{
				Diet.Info[] preyInfos = value.preyInfos;
				for (int i = 0; i < preyInfos.Length; i++)
				{
					foreach (Tag item in preyInfos[i].consumedTags)
					{
						CreatureFeederConfig.forbiddenTags.Add(item);
					}
				}
			}
			list.Add(keyValuePair.Key);
		}
		def.BuildingComplete.GetComponent<Storage>().storageFilters = list;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0000E104 File Offset: 0x0000C304
	private void OnPrefabInit(GameObject instance)
	{
		TreeFilterable component = instance.GetComponent<TreeFilterable>();
		foreach (Tag item in CreatureFeederConfig.forbiddenTags)
		{
			component.ForbiddenTags.Add(item);
		}
	}

	// Token: 0x04000134 RID: 308
	public const string ID = "CreatureFeeder";

	// Token: 0x04000135 RID: 309
	private static HashSet<Tag> forbiddenTags = new HashSet<Tag>();
}
