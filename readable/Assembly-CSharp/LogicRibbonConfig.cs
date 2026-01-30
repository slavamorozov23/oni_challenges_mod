using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002C7 RID: 711
public class LogicRibbonConfig : BaseLogicWireConfig
{
	// Token: 0x06000E7B RID: 3707 RVA: 0x000544F0 File Offset: 0x000526F0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicRibbon";
		string anim = "logic_ribbon_kanim";
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none);
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x00054533 File Offset: 0x00052733
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.FourBit, go);
	}

	// Token: 0x0400096C RID: 2412
	public const string ID = "LogicRibbon";
}
