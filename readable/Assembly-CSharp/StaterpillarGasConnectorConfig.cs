using System;
using TUNING;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public class StaterpillarGasConnectorConfig : IBuildingConfig
{
	// Token: 0x06001660 RID: 5728 RVA: 0x0007F51D File Offset: 0x0007D71D
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001661 RID: 5729 RVA: 0x0007F524 File Offset: 0x0007D724
	public override BuildingDef CreateBuildingDef()
	{
		string id = StaterpillarGasConnectorConfig.ID;
		int width = 1;
		int height = 2;
		string anim = "egg_caterpillar_kanim";
		int hitpoints = 1000;
		float construction_time = 10f;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] construction_materials = all_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFoundationRotatable;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.OverheatTemperature = 423.15f;
		buildingDef.PermittedRotations = PermittedRotations.FlipV;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		buildingDef.PlayConstructionSounds = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001662 RID: 5730 RVA: 0x0007F5C8 File Offset: 0x0007D7C8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06001663 RID: 5731 RVA: 0x0007F5E0 File Offset: 0x0007D7E0
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Storage>();
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
		conduitDispenser.isOn = false;
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.GetComponent<KSelectable>().IsSelectable = false;
	}

	// Token: 0x04000D50 RID: 3408
	public static readonly string ID = "StaterpillarGasConnector";

	// Token: 0x04000D51 RID: 3409
	private const int WIDTH = 1;

	// Token: 0x04000D52 RID: 3410
	private const int HEIGHT = 2;
}
