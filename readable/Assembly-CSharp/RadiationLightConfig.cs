using System;
using TUNING;
using UnityEngine;

// Token: 0x020003D6 RID: 982
public class RadiationLightConfig : IBuildingConfig
{
	// Token: 0x06001429 RID: 5161 RVA: 0x0007279A File Offset: 0x0007099A
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x000727A4 File Offset: 0x000709A4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RadiationLight";
		int width = 1;
		int height = 1;
		string anim = "radiation_lamp_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnWall;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = CellOffset.none;
		return buildingDef;
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x0007283C File Offset: 0x00070A3C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = 50f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = this.FUEL_ELEMENT;
		manualDeliveryKG.capacity = 50f;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
		radiationEmitter.emitAngle = 90f;
		radiationEmitter.emitDirection = 0f;
		radiationEmitter.emissionOffset = Vector3.right;
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.emitRadiusX = 16;
		radiationEmitter.emitRadiusY = 4;
		radiationEmitter.emitRads = 240f;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(this.FUEL_ELEMENT, 0.016666668f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.008333334f, this.WASTE_ELEMENT, 0f, false, true, 0f, 0.5f, 0.5f, byte.MaxValue, 0, true)
		};
		ElementDropper elementDropper = go.AddOrGet<ElementDropper>();
		elementDropper.emitTag = this.WASTE_ELEMENT.CreateTag();
		elementDropper.emitMass = 5f;
		RadiationLight radiationLight = go.AddComponent<RadiationLight>();
		radiationLight.elementToConsume = this.FUEL_ELEMENT;
		radiationLight.consumptionRate = 0.016666668f;
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x000729B2 File Offset: 0x00070BB2
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x000729B4 File Offset: 0x00070BB4
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x04000C21 RID: 3105
	public const string ID = "RadiationLight";

	// Token: 0x04000C22 RID: 3106
	private Tag FUEL_ELEMENT = SimHashes.UraniumOre.CreateTag();

	// Token: 0x04000C23 RID: 3107
	private SimHashes WASTE_ELEMENT = SimHashes.DepletedUranium;

	// Token: 0x04000C24 RID: 3108
	private const float FUEL_PER_CYCLE = 10f;

	// Token: 0x04000C25 RID: 3109
	private const float CYCLES_PER_REFILL = 5f;

	// Token: 0x04000C26 RID: 3110
	private const float FUEL_TO_WASTE_RATIO = 0.5f;

	// Token: 0x04000C27 RID: 3111
	private const float FUEL_STORAGE_AMOUNT = 50f;

	// Token: 0x04000C28 RID: 3112
	private const float FUEL_CONSUMPTION_RATE = 0.016666668f;

	// Token: 0x04000C29 RID: 3113
	private const short RAD_LIGHT_SIZE_X = 16;

	// Token: 0x04000C2A RID: 3114
	private const short RAD_LIGHT_SIZE_Y = 4;
}
