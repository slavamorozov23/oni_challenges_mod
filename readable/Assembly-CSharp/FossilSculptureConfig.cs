using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000221 RID: 545
public class FossilSculptureConfig : IBuildingConfig
{
	// Token: 0x06000B08 RID: 2824 RVA: 0x0004291E File Offset: 0x00040B1E
	public override string[] GetRequiredDlcIds()
	{
		return new string[]
		{
			"DLC4_ID"
		};
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00042930 File Offset: 0x00040B30
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FossilSculpture", 3, 3, "fossilsculpture_kanim", 100, 240f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.FOSSILS, 800f, BuildLocationRule.OnFloor, DECOR.BONUS.TIER5, NOISE_POLLUTION.NONE, 0.2f);
		buildingDef.InputConduitType = ConduitType.None;
		buildingDef.OutputConduitType = ConduitType.None;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.UseHighEnergyParticleInputPort = false;
		buildingDef.UseHighEnergyParticleOutputPort = false;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 0);
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.DragBuild = false;
		buildingDef.Replaceable = true;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.EnergyConsumptionWhenActive = 0f;
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanArtGreat.Id;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.DefaultAnimState = "slab";
		buildingDef.UseStructureTemperature = true;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Disinfectable = true;
		buildingDef.Entombable = true;
		buildingDef.Invincible = false;
		buildingDef.Repairable = false;
		buildingDef.IsFoundation = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AddSearchTerms(SEARCH_TERMS.DINOSAUR);
		buildingDef.AddSearchTerms(SEARCH_TERMS.STATUE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.ARTWORK);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x00042ABB File Offset: 0x00040CBB
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
		go.AddOrGet<BuildingComplete>().isArtable = true;
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x00042AE1 File Offset: 0x00040CE1
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x00042AE3 File Offset: 0x00040CE3
	public override void DoPostConfigureComplete(GameObject go)
	{
		Sculpture sculpture = go.AddOrGet<Sculpture>();
		sculpture.requiredSkillPerk = Db.Get().SkillPerks.CanArtGreat.Id;
		sculpture.defaultAnimName = "slab";
	}

	// Token: 0x040007BE RID: 1982
	public const string ID = "FossilSculpture";
}
