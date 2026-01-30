using System;
using TUNING;
using UnityEngine;

// Token: 0x02000438 RID: 1080
public class StaterpillarLiquidConnectorConfig : IBuildingConfig
{
	// Token: 0x06001666 RID: 5734 RVA: 0x0007F641 File Offset: 0x0007D841
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x0007F648 File Offset: 0x0007D848
	public override BuildingDef CreateBuildingDef()
	{
		string id = StaterpillarLiquidConnectorConfig.ID;
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
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		buildingDef.PlayConstructionSounds = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x0007F6EC File Offset: 0x0007D8EC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x0007F704 File Offset: 0x0007D904
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Storage>();
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
		conduitDispenser.isOn = false;
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.GetComponent<KSelectable>().IsSelectable = false;
	}

	// Token: 0x04000D53 RID: 3411
	public static readonly string ID = "StaterpillarLiquidConnector";

	// Token: 0x04000D54 RID: 3412
	private const int WIDTH = 1;

	// Token: 0x04000D55 RID: 3413
	private const int HEIGHT = 2;
}
