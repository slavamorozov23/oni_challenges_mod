using System;
using TUNING;
using UnityEngine;

// Token: 0x0200046B RID: 1131
public class WireRefinedHighWattageConfig : BaseWireConfig
{
	// Token: 0x060017A8 RID: 6056 RVA: 0x0008612C File Offset: 0x0008432C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WireRefinedHighWattage";
		string anim = "utilities_electric_conduct_hiwatt_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.PENALTY.TIER3, none);
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
		return buildingDef;
	}

	// Token: 0x060017A9 RID: 6057 RVA: 0x00086176 File Offset: 0x00084376
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max50000, go);
	}

	// Token: 0x060017AA RID: 6058 RVA: 0x00086180 File Offset: 0x00084380
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
	}

	// Token: 0x04000DE1 RID: 3553
	public const string ID = "WireRefinedHighWattage";
}
