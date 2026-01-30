using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000469 RID: 1129
public class WireRefinedBridgeHighWattageConfig : WireBridgeHighWattageConfig
{
	// Token: 0x060017A0 RID: 6048 RVA: 0x00086004 File Offset: 0x00084204
	protected override string GetID()
	{
		return "WireRefinedBridgeHighWattage";
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x0008600C File Offset: 0x0008420C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = base.CreateBuildingDef();
		buildingDef.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("heavywatttile_conductive_kanim")
		};
		buildingDef.Mass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		buildingDef.SceneLayer = Grid.SceneLayer.WireBridges;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridgeHighWattage");
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WIRE);
		return buildingDef;
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x00086094 File Offset: 0x00084294
	protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = base.AddNetworkLink(go);
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max50000;
		return wireUtilityNetworkLink;
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x000860A4 File Offset: 0x000842A4
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
	}

	// Token: 0x04000DDF RID: 3551
	public new const string ID = "WireRefinedBridgeHighWattage";
}
