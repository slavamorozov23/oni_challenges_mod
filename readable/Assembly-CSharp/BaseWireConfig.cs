using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000025 RID: 37
public abstract class BaseWireConfig : IBuildingConfig
{
	// Token: 0x060000AA RID: 170
	public abstract override BuildingDef CreateBuildingDef();

	// Token: 0x060000AB RID: 171 RVA: 0x00006670 File Offset: 0x00004870
	public BuildingDef CreateBuildingDef(string id, string anim, float construction_time, float[] construction_mass, float insulation, EffectorValues decor, EffectorValues noise)
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 1, 1, anim, 10, construction_time, construction_mass, MATERIALS.ALL_METALS, 1600f, BuildLocationRule.Anywhere, decor, noise, 0.2f);
		buildingDef.ThermalConductivity = insulation;
		BuildingDef buildingDef2;
		(buildingDef2 = buildingDef).Floodable = false;
		BuildingDef buildingDef3 = buildingDef2;
		buildingDef3.Overheatable = false;
		buildingDef3.Entombable = false;
		buildingDef3.ViewMode = OverlayModes.Power.ID;
		buildingDef3.ObjectLayer = ObjectLayer.Wire;
		buildingDef3.TileLayer = ObjectLayer.WireTile;
		buildingDef3.ReplacementLayer = ObjectLayer.ReplacementWire;
		buildingDef3.AudioCategory = "Metal";
		buildingDef3.AudioSize = "small";
		buildingDef3.BaseTimeUntilRepair = -1f;
		buildingDef3.SceneLayer = Grid.SceneLayer.Wires;
		buildingDef3.isKAnimTile = true;
		buildingDef3.isUtility = true;
		buildingDef3.DragBuild = true;
		buildingDef3.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef3.AddSearchTerms(SEARCH_TERMS.WIRE);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, id);
		return buildingDef3;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x0000674A File Offset: 0x0000494A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<Wire>();
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddOrGet<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.isPhysicalBuilding = true;
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Electrical;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00006781 File Offset: 0x00004981
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().isDiggingRequired = false;
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddOrGet<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.isPhysicalBuilding = false;
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Electrical;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x000067AC File Offset: 0x000049AC
	protected void DoPostConfigureComplete(Wire.WattageRating rating, GameObject go)
	{
		go.GetComponent<Wire>().MaxWattageRating = rating;
		float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(rating);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.MAX_WATTAGE, GameUtil.GetFormattedWattage(maxWattageAsFloat, GameUtil.WattageFormatterUnit.Automatic, true)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MAX_WATTAGE, Array.Empty<object>()), Descriptor.DescriptorType.Effect);
		BuildingDef def = go.GetComponent<Building>().Def;
		if (def.EffectDescription == null)
		{
			def.EffectDescription = new List<Descriptor>();
		}
		def.EffectDescription.Add(item);
	}
}
