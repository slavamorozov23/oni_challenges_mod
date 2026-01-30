using System;
using Database;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200035B RID: 859
public class MonumentBottomConfig : IBuildingConfig
{
	// Token: 0x060011E6 RID: 4582 RVA: 0x000687E8 File Offset: 0x000669E8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MonumentBottom";
		int width = 5;
		int height = 5;
		string anim = "monument_base_a_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			7500f,
			2500f
		};
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString(),
			SimHashes.Obsidian.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE, tier, 0.2f);
		BuildingTemplates.CreateMonumentBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = "MonumentBottom";
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.STATUE);
		buildingDef.AddSearchTerms(SEARCH_TERMS.MORALE);
		return buildingDef;
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x000688E8 File Offset: 0x00066AE8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), "MonumentMiddle", null)
		};
		go.AddOrGet<MonumentPart>().part = MonumentPartResource.Part.Bottom;
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x0006894C File Offset: 0x00066B4C
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x060011E9 RID: 4585 RVA: 0x0006894E File Offset: 0x00066B4E
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x00068950 File Offset: 0x00066B50
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<KBatchedAnimController>().initialAnim = "option_a";
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			MonumentPart monumentPart = game_object.AddOrGet<MonumentPart>();
			monumentPart.part = MonumentPartResource.Part.Bottom;
			monumentPart.stateUISymbol = "base";
		};
	}

	// Token: 0x04000B3A RID: 2874
	public const string ID = "MonumentBottom";
}
