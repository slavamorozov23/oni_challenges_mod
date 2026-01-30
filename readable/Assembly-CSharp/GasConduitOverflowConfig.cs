using System;
using TUNING;
using UnityEngine;

// Token: 0x0200022C RID: 556
public class GasConduitOverflowConfig : IBuildingConfig
{
	// Token: 0x06000B40 RID: 2880 RVA: 0x00043964 File Offset: 0x00041B64
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasConduitOverflow";
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

	// Token: 0x06000B41 RID: 2881 RVA: 0x00043A2C File Offset: 0x00041C2C
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x00043A3D File Offset: 0x00041C3D
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x00043A4D File Offset: 0x00041C4D
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x00043A60 File Offset: 0x00041C60
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<ConduitOverflow>().portInfo = this.secondaryPort;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x00043A9F File Offset: 0x00041C9F
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
	}

	// Token: 0x040007D6 RID: 2006
	public const string ID = "GasConduitOverflow";

	// Token: 0x040007D7 RID: 2007
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x040007D8 RID: 2008
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(1, 1));
}
