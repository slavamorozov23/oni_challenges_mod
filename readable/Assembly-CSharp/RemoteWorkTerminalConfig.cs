using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003DF RID: 991
public class RemoteWorkTerminalConfig : IBuildingConfig
{
	// Token: 0x06001457 RID: 5207 RVA: 0x00073930 File Offset: 0x00071B30
	public override BuildingDef CreateBuildingDef()
	{
		string id = RemoteWorkTerminalConfig.ID;
		int width = 3;
		int height = 3;
		string anim = "remote_work_terminal_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.AddSearchTerms(SEARCH_TERMS.ROBOT);
		return buildingDef;
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x000739B0 File Offset: 0x00071BB0
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<RemoteWorkTerminal>().workTime = float.PositiveInfinity;
		go.AddComponent<RemoteWorkTerminalSM>();
		go.AddOrGet<Operational>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 100f;
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate
		});
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = RemoteWorkTerminalConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(RemoteWorkTerminalConfig.INPUT_MATERIAL, 0.013333334f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGet<ElementConverterOperationalRequirement>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x00073A99 File Offset: 0x00071C99
	public override string[] GetRequiredDlcIds()
	{
		return new string[]
		{
			"DLC3_ID"
		};
	}

	// Token: 0x04000C45 RID: 3141
	public static string ID = "RemoteWorkTerminal";

	// Token: 0x04000C46 RID: 3142
	public static readonly Tag INPUT_MATERIAL = DatabankHelper.TAG;

	// Token: 0x04000C47 RID: 3143
	public const float INPUT_CAPACITY = 10f;

	// Token: 0x04000C48 RID: 3144
	public const float INPUT_CONSUMPTION_RATE_PER_S = 0.013333334f;

	// Token: 0x04000C49 RID: 3145
	public const float INPUT_REFILL_RATIO = 0.5f;
}
