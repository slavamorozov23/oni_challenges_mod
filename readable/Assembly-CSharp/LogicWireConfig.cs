using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002D0 RID: 720
public class LogicWireConfig : BaseLogicWireConfig
{
	// Token: 0x06000EA3 RID: 3747 RVA: 0x0005511C File Offset: 0x0005331C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicWire";
		string anim = "logic_wires_kanim";
		float construction_time = 3f;
		float[] tier_TINY = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier_TINY, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none);
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x0005515F File Offset: 0x0005335F
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.OneBit, go);
	}

	// Token: 0x04000977 RID: 2423
	public const string ID = "LogicWire";
}
