using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000346 RID: 838
public class SelfChargingElectrobankConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001155 RID: 4437 RVA: 0x00066940 File Offset: 0x00064B40
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1.Append(DlcManager.DLC3);
	}

	// Token: 0x06001156 RID: 4438 RVA: 0x00066951 File Offset: 0x00064B51
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001157 RID: 4439 RVA: 0x00066954 File Offset: 0x00064B54
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("SelfChargingElectrobank", STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SELFCHARGING.NAME, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SELFCHARGING.DESC, 20f, true, Assets.GetAnim("electrobank_large_uranium_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, 0, SimHashes.EnrichedUranium, new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.PedestalDisplayable
		});
		RadiationEmitter radiationEmitter = gameObject.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 5;
		radiationEmitter.emitRadiusY = radiationEmitter.emitRadiusX;
		radiationEmitter.emitRads = 120f;
		radiationEmitter.emissionOffset = new Vector3(0f, 0f, 0f);
		if (!Assets.IsTagCountable(GameTags.ChargedPortableBattery))
		{
			Assets.AddCountableTag(GameTags.ChargedPortableBattery);
		}
		gameObject.GetComponent<KCollider2D>();
		gameObject.AddTag(GameTags.IndustrialProduct);
		SelfChargingElectrobank selfChargingElectrobank = gameObject.AddComponent<SelfChargingElectrobank>();
		selfChargingElectrobank.rechargeable = false;
		selfChargingElectrobank.keepEmpty = true;
		selfChargingElectrobank.radioactivityTuning = radiationEmitter.emitRads;
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.PENALTY.TIER0);
		return gameObject;
	}

	// Token: 0x06001158 RID: 4440 RVA: 0x00066A7E File Offset: 0x00064C7E
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001159 RID: 4441 RVA: 0x00066A80 File Offset: 0x00064C80
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000B00 RID: 2816
	public const string ID = "SelfChargingElectrobank";

	// Token: 0x04000B01 RID: 2817
	public const float MASS = 20f;

	// Token: 0x04000B02 RID: 2818
	public const float POWER_DURATION = 90000f;

	// Token: 0x04000B03 RID: 2819
	public const float SELF_CHARGE_WATTAGE = 60f;
}
