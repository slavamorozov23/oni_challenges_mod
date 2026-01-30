using System;
using TUNING;
using UnityEngine;

// Token: 0x02000411 RID: 1041
public class ShearingStationConfig : IBuildingConfig
{
	// Token: 0x06001572 RID: 5490 RVA: 0x0007A470 File Offset: 0x00078670
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ShearingStation";
		int width = 3;
		int height = 3;
		string anim = "shearing_station_kanim";
		int hitpoints = 100;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.Floodable = true;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.DefaultAnimState = "on";
		buildingDef.ShowInBuildMenu = true;
		return buildingDef;
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x0007A530 File Offset: 0x00078730
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x0007A588 File Offset: 0x00078788
	public override void DoPostConfigureComplete(GameObject go)
	{
		RanchStation.Def def = go.AddOrGetDef<RanchStation.Def>();
		def.IsCritterEligibleToBeRanchedCb = delegate(GameObject creature_go, RanchStation.Instance ranch_station_smi)
		{
			IShearable smi = creature_go.GetSMI<IShearable>();
			return smi != null && smi.IsFullyGrown();
		};
		def.OnRanchCompleteCb = delegate(GameObject creature_go, WorkerBase rancher_wb)
		{
			IShearable smi = creature_go.GetSMI<IShearable>();
			global::Tuple<Tag, float> itemDroppedOnShear = smi.GetItemDroppedOnShear();
			this.DropShearable(rancher_wb.gameObject, creature_go, itemDroppedOnShear.first, itemDroppedOnShear.second);
			smi.Shear();
		};
		def.RancherInteractAnim = "anim_interacts_shearingstation_kanim";
		def.WorkTime = 12f;
		def.RanchedPreAnim = "shearing_pre";
		def.RanchedLoopAnim = "shearing_loop";
		def.RanchedPstAnim = "shearing_pst";
		def.RancherWipesBrowAnim = false;
		go.AddOrGet<SkillPerkMissingComplainer>().requiredSkillPerk = Db.Get().SkillPerks.CanUseRanchStation.Id;
		Prioritizable.AddRef(go);
	}

	// Token: 0x06001575 RID: 5493 RVA: 0x0007A648 File Offset: 0x00078848
	private void DropShearable(GameObject go, GameObject critter, Tag item_dropped, float mass)
	{
		PrimaryElement component = critter.GetComponent<PrimaryElement>();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(item_dropped), null, null);
		int cell = Grid.CellLeft(Grid.PosToCell(go));
		gameObject.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
		PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
		component2.Temperature = component.Temperature;
		component2.Mass = mass;
		component2.AddDisease(component.DiseaseIdx, component.DiseaseCount, "Shearing");
		gameObject.SetActive(true);
		Vector2 initial_velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f) * 1f, UnityEngine.Random.value * 2f + 2f);
		if (GameComps.Fallers.Has(gameObject))
		{
			GameComps.Fallers.Remove(gameObject);
		}
		GameComps.Fallers.Add(gameObject, initial_velocity);
	}

	// Token: 0x04000CD4 RID: 3284
	public const string ID = "ShearingStation";
}
