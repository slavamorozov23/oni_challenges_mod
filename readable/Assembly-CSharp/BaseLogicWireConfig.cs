using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000023 RID: 35
public abstract class BaseLogicWireConfig : IBuildingConfig
{
	// Token: 0x0600009F RID: 159
	public abstract override BuildingDef CreateBuildingDef();

	// Token: 0x060000A0 RID: 160 RVA: 0x000061DC File Offset: 0x000043DC
	public BuildingDef CreateBuildingDef(string id, string anim, float construction_time, float[] construction_mass, EffectorValues decor, EffectorValues noise)
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 1, 1, anim, 10, construction_time, construction_mass, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.Anywhere, decor, noise, 0.2f);
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		BuildingDef buildingDef2;
		(buildingDef2 = buildingDef).ObjectLayer = ObjectLayer.LogicWire;
		BuildingDef buildingDef3 = buildingDef2;
		buildingDef3.TileLayer = ObjectLayer.LogicWireTile;
		buildingDef3.ReplacementLayer = ObjectLayer.ReplacementLogicWire;
		buildingDef3.SceneLayer = Grid.SceneLayer.LogicWires;
		buildingDef3.Floodable = false;
		buildingDef3.Overheatable = false;
		buildingDef3.Entombable = false;
		buildingDef3.AudioCategory = "Metal";
		buildingDef3.AudioSize = "small";
		buildingDef3.BaseTimeUntilRepair = -1f;
		buildingDef3.isKAnimTile = true;
		buildingDef3.isUtility = true;
		buildingDef3.DragBuild = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, id);
		return buildingDef3;
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x0000628E File Offset: 0x0000448E
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LogicWire>();
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddOrGet<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Logic;
		kanimGraphTileVisualizer.isPhysicalBuilding = true;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x000062C5 File Offset: 0x000044C5
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().isDiggingRequired = false;
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddOrGet<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Logic;
		kanimGraphTileVisualizer.isPhysicalBuilding = false;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x000062F0 File Offset: 0x000044F0
	protected void DoPostConfigureComplete(LogicWire.BitDepth rating, GameObject go)
	{
		go.GetComponent<LogicWire>().MaxBitDepth = rating;
		int bitDepthAsInt = LogicWire.GetBitDepthAsInt(rating);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.MAX_BITS, bitDepthAsInt), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MAX_BITS, Array.Empty<object>()), Descriptor.DescriptorType.Effect);
		BuildingDef def = go.GetComponent<Building>().Def;
		if (def.EffectDescription == null)
		{
			def.EffectDescription = new List<Descriptor>();
		}
		def.EffectDescription.Add(item);
	}
}
