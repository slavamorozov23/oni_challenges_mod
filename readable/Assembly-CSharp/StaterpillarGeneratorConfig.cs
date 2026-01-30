using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000436 RID: 1078
public class StaterpillarGeneratorConfig : IBuildingConfig
{
	// Token: 0x06001658 RID: 5720 RVA: 0x0007F3E0 File Offset: 0x0007D5E0
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001659 RID: 5721 RVA: 0x0007F3E8 File Offset: 0x0007D5E8
	public override BuildingDef CreateBuildingDef()
	{
		string id = StaterpillarGeneratorConfig.ID;
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
		buildingDef.GeneratorWattageRating = 1600f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 2f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.OverheatTemperature = 423.15f;
		buildingDef.PermittedRotations = PermittedRotations.FlipV;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 1);
		buildingDef.PlayConstructionSounds = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x0600165A RID: 5722 RVA: 0x0007F4B9 File Offset: 0x0007D6B9
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x0600165B RID: 5723 RVA: 0x0007F4D0 File Offset: 0x0007D6D0
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x0600165C RID: 5724 RVA: 0x0007F4D2 File Offset: 0x0007D6D2
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x0600165D RID: 5725 RVA: 0x0007F4D4 File Offset: 0x0007D6D4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<StaterpillarGenerator>().powerDistributionOrder = 9;
		go.GetComponent<Deconstructable>().SetAllowDeconstruction(false);
		go.AddOrGet<Modifiers>();
		go.AddOrGet<Effects>();
		go.GetComponent<KSelectable>().IsSelectable = false;
	}

	// Token: 0x04000D4D RID: 3405
	public static readonly string ID = "StaterpillarGenerator";

	// Token: 0x04000D4E RID: 3406
	private const int WIDTH = 1;

	// Token: 0x04000D4F RID: 3407
	private const int HEIGHT = 2;
}
