using System;
using TUNING;
using UnityEngine;

// Token: 0x0200042D RID: 1069
public class SolidVentConfig : IBuildingConfig
{
	// Token: 0x0600160F RID: 5647 RVA: 0x0007DC30 File Offset: 0x0007BE30
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidVent";
		int width = 1;
		int height = 1;
		string anim = "conveyer_dropper_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidVent");
		return buildingDef;
	}

	// Token: 0x06001610 RID: 5648 RVA: 0x0007DCDC File Offset: 0x0007BEDC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x06001611 RID: 5649 RVA: 0x0007DCE5 File Offset: 0x0007BEE5
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x0007DD0D File Offset: 0x0007BF0D
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<SimpleVent>();
		go.AddOrGet<SolidConduitConsumer>();
		go.AddOrGet<SolidConduitDropper>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.capacityKg = 100f;
		storage.showInUI = true;
	}

	// Token: 0x04000D14 RID: 3348
	public const string ID = "SolidVent";

	// Token: 0x04000D15 RID: 3349
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;
}
