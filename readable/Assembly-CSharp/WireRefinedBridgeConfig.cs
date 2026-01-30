using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000468 RID: 1128
public class WireRefinedBridgeConfig : WireBridgeConfig
{
	// Token: 0x0600179C RID: 6044 RVA: 0x00085F6A File Offset: 0x0008416A
	protected override string GetID()
	{
		return "WireRefinedBridge";
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x00085F74 File Offset: 0x00084174
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = base.CreateBuildingDef();
		buildingDef.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("utilityelectricbridgeconductive_kanim")
		};
		buildingDef.Mass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WIRE);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridge");
		return buildingDef;
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x00085FEC File Offset: 0x000841EC
	protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = base.AddNetworkLink(go);
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max2000;
		return wireUtilityNetworkLink;
	}

	// Token: 0x04000DDE RID: 3550
	public new const string ID = "WireRefinedBridge";
}
