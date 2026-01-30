using System;
using TUNING;
using UnityEngine;

// Token: 0x0200029B RID: 667
public class LiquidConduitPreferentialFlowConfig : IBuildingConfig
{
	// Token: 0x06000D91 RID: 3473 RVA: 0x00050600 File Offset: 0x0004E800
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidConduitPreferentialFlow";
		int width = 2;
		int height = 2;
		string anim = "valveliquid_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Deprecated = true;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, buildingDef.PrefabID);
		return buildingDef;
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x000506C8 File Offset: 0x0004E8C8
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x000506D9 File Offset: 0x0004E8D9
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000D94 RID: 3476 RVA: 0x000506E9 File Offset: 0x0004E8E9
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x000506FC File Offset: 0x0004E8FC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<ConduitPreferentialFlow>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x0005072A File Offset: 0x0004E92A
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x0400092B RID: 2347
	public const string ID = "LiquidConduitPreferentialFlow";

	// Token: 0x0400092C RID: 2348
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x0400092D RID: 2349
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 1));
}
