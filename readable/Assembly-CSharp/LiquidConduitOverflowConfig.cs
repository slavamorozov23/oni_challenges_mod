using System;
using TUNING;
using UnityEngine;

// Token: 0x0200029A RID: 666
public class LiquidConduitOverflowConfig : IBuildingConfig
{
	// Token: 0x06000D8A RID: 3466 RVA: 0x00050484 File Offset: 0x0004E684
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidConduitOverflow";
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

	// Token: 0x06000D8B RID: 3467 RVA: 0x0005054C File Offset: 0x0004E74C
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000D8C RID: 3468 RVA: 0x0005055D File Offset: 0x0004E75D
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x0005056D File Offset: 0x0004E76D
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x00050580 File Offset: 0x0004E780
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<ConduitOverflow>().portInfo = this.secondaryPort;
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x000505AE File Offset: 0x0004E7AE
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<RequireInputs>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
		UnityEngine.Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000928 RID: 2344
	public const string ID = "LiquidConduitOverflow";

	// Token: 0x04000929 RID: 2345
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x0400092A RID: 2346
	private ConduitPortInfo secondaryPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(1, 1));
}
