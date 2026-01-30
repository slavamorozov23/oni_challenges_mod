using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200024C RID: 588
public class GravitasCreatureManipulatorConfig : IBuildingConfig
{
	// Token: 0x06000BF3 RID: 3059 RVA: 0x00048618 File Offset: 0x00046818
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasCreatureManipulator";
		int width = 3;
		int height = 4;
		string anim = "gravitas_critter_manipulator_kanim";
		int hitpoints = 250;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 3200f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.Floodable = false;
		buildingDef.Entombable = true;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "medium";
		buildingDef.ForegroundLayer = Grid.SceneLayer.Ground;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x000486B4 File Offset: 0x000468B4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		BuildingTemplates.ExtendBuildingToGravitas(go);
		go.AddComponent<Storage>();
		Activatable activatable = go.AddComponent<Activatable>();
		activatable.synchronizeAnims = false;
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		activatable.SetWorkTime(30f);
		GravitasCreatureManipulator.Def def = go.AddOrGetDef<GravitasCreatureManipulator.Def>();
		def.pickupOffset = new CellOffset(-1, 0);
		def.dropOffset = new CellOffset(1, 0);
		def.numSpeciesToUnlockMorphMode = 5;
		def.workingDuration = 15f;
		def.cooldownDuration = 540f;
		MakeBaseSolid.Def def2 = go.AddOrGetDef<MakeBaseSolid.Def>();
		def2.solidOffsets = new CellOffset[4];
		for (int i = 0; i < 4; i++)
		{
			def2.solidOffsets[i] = new CellOffset(0, i);
		}
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<Activatable>().SetOffsets(OffsetGroups.LeftOrRight);
		};
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x000487CC File Offset: 0x000469CC
	public static Option<string> GetBodyContentForSpeciesTag(Tag species)
	{
		Option<string> nameForSpeciesTag = GravitasCreatureManipulatorConfig.GetNameForSpeciesTag(species);
		Option<string> descriptionForSpeciesTag = GravitasCreatureManipulatorConfig.GetDescriptionForSpeciesTag(species);
		if (nameForSpeciesTag.HasValue && descriptionForSpeciesTag.HasValue)
		{
			return GravitasCreatureManipulatorConfig.GetBodyContent(nameForSpeciesTag.Value, descriptionForSpeciesTag.Value);
		}
		return Option.None;
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x0004881C File Offset: 0x00046A1C
	public static string GetBodyContentForUnknownSpecies()
	{
		return GravitasCreatureManipulatorConfig.GetBodyContent(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.UNKNOWN_TITLE, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES.UNKNOWN);
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00048837 File Offset: 0x00046A37
	public static string GetBodyContent(string name, string desc)
	{
		return "<size=125%><b>" + name + "</b></size><line-height=150%>\n</line-height>" + desc;
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x0004884C File Offset: 0x00046A4C
	public static Option<string> GetNameForSpeciesTag(Tag species)
	{
		StringEntry entry;
		if (!Strings.TryGet("STRINGS.CREATURES.FAMILY_PLURAL." + species.ToString().ToUpper(), out entry))
		{
			return Option.None;
		}
		return Option.Some<string>(entry);
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00048894 File Offset: 0x00046A94
	public static Option<string> GetDescriptionForSpeciesTag(Tag species)
	{
		StringEntry entry;
		if (!Strings.TryGet("STRINGS.CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.SPECIES_ENTRIES." + species.ToString().ToUpper().Replace("SPECIES", ""), out entry))
		{
			return Option.None;
		}
		return Option.Some<string>(entry);
	}

	// Token: 0x04000853 RID: 2131
	public const string ID = "GravitasCreatureManipulator";

	// Token: 0x04000854 RID: 2132
	public const string CODEX_ENTRY_ID = "STORYTRAITCRITTERMANIPULATOR";

	// Token: 0x04000855 RID: 2133
	public const string INITIAL_LORE_UNLOCK_ID = "story_trait_critter_manipulator_initial";

	// Token: 0x04000856 RID: 2134
	public const string PARKING_LORE_UNLOCK_ID = "story_trait_critter_manipulator_parking";

	// Token: 0x04000857 RID: 2135
	public const string COMPLETED_LORE_UNLOCK_ID = "story_trait_critter_manipulator_complete";

	// Token: 0x04000858 RID: 2136
	private const int HEIGHT = 4;

	// Token: 0x020011EA RID: 4586
	public static class CRITTER_LORE_UNLOCK_ID
	{
		// Token: 0x0600860D RID: 34317 RVA: 0x00349397 File Offset: 0x00347597
		public static string For(Tag species)
		{
			return "story_trait_critter_manipulator_" + species.ToString().ToLower();
		}
	}
}
