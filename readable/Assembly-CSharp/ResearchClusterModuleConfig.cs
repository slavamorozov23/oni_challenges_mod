using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003E2 RID: 994
public class ResearchClusterModuleConfig : IBuildingConfig
{
	// Token: 0x0600146A RID: 5226 RVA: 0x00073F10 File Offset: 0x00072110
	public override string[] GetRequiredDlcIds()
	{
		return new string[]
		{
			"EXPANSION1_ID"
		};
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x00073F20 File Offset: 0x00072120
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ResearchClusterModule", 3, 2, "rocket_research_module_small_kanim", 1000, 60f, TUNING.BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER2, new string[]
		{
			SimHashes.Steel.ToString()
		}, 9999f, BuildLocationRule.Anywhere, DECOR.NONE, NOISE_POLLUTION.NONE, 0.2f);
		buildingDef.InputConduitType = ConduitType.None;
		buildingDef.OutputConduitType = ConduitType.None;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.UseHighEnergyParticleInputPort = false;
		buildingDef.UseHighEnergyParticleOutputPort = false;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 0);
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.DragBuild = false;
		buildingDef.Replaceable = true;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.Overheatable = true;
		buildingDef.Floodable = false;
		buildingDef.Disinfectable = true;
		buildingDef.Entombable = true;
		buildingDef.Repairable = true;
		buildingDef.IsFoundation = false;
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.TRANSPORT);
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x000740AC File Offset: 0x000722AC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 2), GameTags.Rocket, null)
		};
		go.AddOrGet<LoopingSounds>();
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x00074110 File Offset: 0x00072310
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 50f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showCapacityStatusItem = true;
		storage.storageFilters = STORAGEFILTERS.SO_DATABANKS;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.allowItemRemoval = false;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR_PLUS, 0f, 0f);
		RocketModuleHexCellCollector.Def def = go.AddOrGetDef<RocketModuleHexCellCollector.Def>();
		def.collectSpeed = 0.083333336f;
		def.formatCapacityBarAsUnits = true;
		go.AddOrGetDef<ResearchClusterModule.Def>();
	}

	// Token: 0x04000C59 RID: 3161
	public const string ID = "ResearchClusterModule";

	// Token: 0x04000C5A RID: 3162
	public const float CAPACITY = 50f;

	// Token: 0x04000C5B RID: 3163
	public const int CYCLE_COUNT_TO_FILL_STORAGE = 1;

	// Token: 0x04000C5C RID: 3164
	public const float COLLECT_SPEED = 0.083333336f;

	// Token: 0x04000C5D RID: 3165
	private const int WIDTH = 3;

	// Token: 0x04000C5E RID: 3166
	private const int HEIGHT = 2;
}
