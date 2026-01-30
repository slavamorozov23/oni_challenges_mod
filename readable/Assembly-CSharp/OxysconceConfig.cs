using System;
using TUNING;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class OxysconceConfig : IBuildingConfig
{
	// Token: 0x060012B6 RID: 4790 RVA: 0x0006CE21 File Offset: 0x0006B021
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x0006CE28 File Offset: 0x0006B028
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Oxysconce";
		int width = 1;
		int height = 1;
		string anim = "oxy_sconce_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ViewMode = OverlayModes.Oxygen.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.Breakable = true;
		return buildingDef;
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x0006CEA8 File Offset: 0x0006B0A8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		new CellOffset(0, 0);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 240f;
		storage.showInUI = true;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = SimHashes.OxyRock.CreateTag();
		manualDeliveryKG.capacity = 240f;
		manualDeliveryKG.refillMass = 96f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		go.AddOrGet<StorageMeter>();
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x0006CF3D File Offset: 0x0006B13D
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			Tutorial.Instance.oxygenGenerators.Add(game_object);
		};
	}

	// Token: 0x04000BDD RID: 3037
	public const string ID = "Oxysconce";

	// Token: 0x04000BDE RID: 3038
	private const float OXYLITE_STORAGE = 240f;
}
