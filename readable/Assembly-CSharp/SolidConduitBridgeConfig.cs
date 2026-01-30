using System;
using TUNING;
using UnityEngine;

// Token: 0x02000422 RID: 1058
public class SolidConduitBridgeConfig : IBuildingConfig
{
	// Token: 0x060015D7 RID: 5591 RVA: 0x0007CDE0 File Offset: 0x0007AFE0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidConduitBridge";
		int width = 3;
		int height = 1;
		string anim = "utilities_conveyorbridge_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ObjectLayer = ObjectLayer.SolidConduitConnection;
		buildingDef.SceneLayer = Grid.SceneLayer.SolidConduitBridges;
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.OutputConduitType = ConduitType.Solid;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidConduitBridge");
		return buildingDef;
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x0007CEB5 File Offset: 0x0007B0B5
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x0007CED2 File Offset: 0x0007B0D2
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x0007CEFA File Offset: 0x0007B0FA
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<SolidConduitBridge>();
	}

	// Token: 0x04000D03 RID: 3331
	public const string ID = "SolidConduitBridge";

	// Token: 0x04000D04 RID: 3332
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;
}
