using System;
using TUNING;
using UnityEngine;

// Token: 0x0200022D RID: 557
public class GasConduitPreferentialFlowConfig : IBuildingConfig
{
	// Token: 0x06000B47 RID: 2887 RVA: 0x00043AE0 File Offset: 0x00041CE0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasConduitPreferentialFlow";
		int width = 2;
		int height = 2;
		string anim = "valvegas_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Deprecated = true;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, buildingDef.PrefabID);
		return buildingDef;
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x00043BA8 File Offset: 0x00041DA8
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x00043BB9 File Offset: 0x00041DB9
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000B4A RID: 2890 RVA: 0x00043BC9 File Offset: 0x00041DC9
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x00043BDC File Offset: 0x00041DDC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<ConduitPreferentialFlow>().portInfo = this.secondaryPort;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x00043C1B File Offset: 0x00041E1B
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
	}

	// Token: 0x040007D9 RID: 2009
	public const string ID = "GasConduitPreferentialFlow";

	// Token: 0x040007DA RID: 2010
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x040007DB RID: 2011
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(0, 1));
}
