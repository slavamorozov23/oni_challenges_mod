using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003D7 RID: 983
public class RailGunConfig : IBuildingConfig
{
	// Token: 0x0600142F RID: 5167 RVA: 0x000729D9 File Offset: 0x00070BD9
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x000729E0 File Offset: 0x00070BE0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RailGun";
		int width = 5;
		int height = 6;
		string anim = "rail_gun_kanim";
		int hitpoints = 250;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = 400f;
		buildingDef.DefaultAnimState = "off";
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(-2, 0);
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(-2, 1);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(RailGun.PORT_ID, new CellOffset(-2, 2), STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("HEP_STORAGE", new CellOffset(2, 0), STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_ACTIVE, STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_INACTIVE, false, false)
		};
		buildingDef.AddSearchTerms(SEARCH_TERMS.TRANSPORT);
		return buildingDef;
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x00072B48 File Offset: 0x00070D48
	private void AttachPorts(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.liquidInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.gasInputPort;
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.solidInputPort;
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x00072B80 File Offset: 0x00070D80
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		RailGun railGun = go.AddOrGet<RailGun>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<LoopingSounds>();
		ClusterDestinationSelector clusterDestinationSelector = go.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.changeTargetButtonTooltipString = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CHANGE_DESTINATION_BUTTON_TOOLTIP_RAILGUN;
		clusterDestinationSelector.clearTargetButtonTooltipString = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CLEAR_DESTINATION_BUTTON_TOOLTIP_RAILGUN;
		clusterDestinationSelector.assignable = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		railGun.liquidPortInfo = this.liquidInputPort;
		railGun.gasPortInfo = this.gasInputPort;
		railGun.solidPortInfo = this.solidInputPort;
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.capacity = 200f;
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.showInUI = false;
		highEnergyParticleStorage.PORT_ID = "HEP_STORAGE";
		highEnergyParticleStorage.showCapacityStatusItem = true;
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x00072C34 File Offset: 0x00070E34
	public override void DoPostConfigureComplete(GameObject go)
	{
		List<Tag> list = new List<Tag>();
		list.AddRange(STORAGEFILTERS.STORAGE_LOCKERS_STANDARD);
		list.AddRange(STORAGEFILTERS.GASES);
		list.AddRange(STORAGEFILTERS.FOOD);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.storageFilters = list;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.capacityKg = 1200f;
		go.GetComponent<HighEnergyParticlePort>().requireOperational = false;
		RailGunConfig.AddVisualizer(go);
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x00072CB2 File Offset: 0x00070EB2
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		this.AttachPorts(go);
		RailGunConfig.AddVisualizer(go);
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x00072CC1 File Offset: 0x00070EC1
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		this.AttachPorts(go);
		RailGunConfig.AddVisualizer(go);
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x00072CD0 File Offset: 0x00070ED0
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = -2;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.AllOrNothingVisibility = true;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<SkyVisibilityVisualizer>().SkyVisibilityCb = new Func<int, bool>(RailGunConfig.RailGunSkyVisibility);
		};
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x00072D24 File Offset: 0x00070F24
	private static bool RailGunSkyVisibility(int cell)
	{
		DebugUtil.DevAssert(ClusterManager.Instance != null, "RailGun assumes DLC", null);
		if (Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != 255)
		{
			int num = (int)ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]).maximumBounds.y;
			int num2 = cell;
			while (Grid.CellRow(num2) <= num)
			{
				if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
				{
					return false;
				}
				num2 = Grid.CellAbove(num2);
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000C2B RID: 3115
	public const string ID = "RailGun";

	// Token: 0x04000C2C RID: 3116
	public const string PORT_ID = "HEP_STORAGE";

	// Token: 0x04000C2D RID: 3117
	public const int RANGE = 20;

	// Token: 0x04000C2E RID: 3118
	public const float BASE_PARTICLE_COST = 0f;

	// Token: 0x04000C2F RID: 3119
	public const float HEX_PARTICLE_COST = 10f;

	// Token: 0x04000C30 RID: 3120
	public const float HEP_CAPACITY = 200f;

	// Token: 0x04000C31 RID: 3121
	public const float TAKEOFF_VELOCITY = 35f;

	// Token: 0x04000C32 RID: 3122
	public const int MAINTENANCE_AFTER_NUM_PAYLOADS = 6;

	// Token: 0x04000C33 RID: 3123
	public const int MAINTENANCE_COOLDOWN = 30;

	// Token: 0x04000C34 RID: 3124
	public const float CAPACITY = 1200f;

	// Token: 0x04000C35 RID: 3125
	private ConduitPortInfo solidInputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(-1, 0));

	// Token: 0x04000C36 RID: 3126
	private ConduitPortInfo liquidInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));

	// Token: 0x04000C37 RID: 3127
	private ConduitPortInfo gasInputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(1, 0));
}
