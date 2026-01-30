using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000368 RID: 872
public class MultiMinionDiningTableConfig : IBuildingConfig
{
	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06001230 RID: 4656 RVA: 0x00069F20 File Offset: 0x00068120
	public static int SeatCount
	{
		get
		{
			return MultiMinionDiningTableConfig.seats.Length;
		}
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x00069F2C File Offset: 0x0006812C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MultiMinionDiningTable";
		int width = 5;
		int height = 1;
		string anim = "multi_dupe_table_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] woods = MATERIALS.WOODS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, woods, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, none, 0.2f);
		buildingDef.WorkTime = 20f;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AddSearchTerms(SEARCH_TERMS.DINING);
		return buildingDef;
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x00069F9F File Offset: 0x0006819F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.DiningTableType, false);
		go.AddOrGetDef<RocketUsageRestriction.Def>();
		go.AddOrGet<MultiMinionDiningTable>();
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x00069FC8 File Offset: 0x000681C8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KAnimControllerBase>().initialAnim = "off";
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = TableSaltTuning.SALTSHAKERSTORAGEMASS * (float)MultiMinionDiningTableConfig.SeatCount;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = TableSaltConfig.ID.ToTag();
		manualDeliveryKG.capacity = TableSaltTuning.SALTSHAKERSTORAGEMASS * (float)MultiMinionDiningTableConfig.SeatCount;
		manualDeliveryKG.refillMass = TableSaltTuning.CONSUMABLE_RATE * (float)MultiMinionDiningTableConfig.SeatCount;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FoodFetch.IdHash;
		manualDeliveryKG.ShowStatusItem = false;
	}

	// Token: 0x04000B7C RID: 2940
	public const string ID = "MultiMinionDiningTable";

	// Token: 0x04000B7D RID: 2941
	public static readonly MultiMinionDiningTableConfig.Seat[] seats = new MultiMinionDiningTableConfig.Seat[]
	{
		new MultiMinionDiningTableConfig.Seat("anim_eat_table_kanim", "anim_bionic_eat_table_kanim", "saltshaker", new CellOffset(0, 0)),
		new MultiMinionDiningTableConfig.Seat("anim_eat_table_L_kanim", "anim_bionic_eat_table_L_kanim", "saltshaker_L", new CellOffset(-1, 0)),
		new MultiMinionDiningTableConfig.Seat("anim_eat_table_R_kanim", "anim_bionic_eat_table_R_kanim", "saltshaker_R", new CellOffset(1, 0))
	};

	// Token: 0x0200124D RID: 4685
	public struct Seat
	{
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060087A6 RID: 34726 RVA: 0x0034C4A1 File Offset: 0x0034A6A1
		public readonly HashedString EatAnim
		{
			get
			{
				return this.eatAnim;
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x060087A7 RID: 34727 RVA: 0x0034C4A9 File Offset: 0x0034A6A9
		public readonly HashedString ReloadElectrobankAnim
		{
			get
			{
				return this.reloadElectrobankAnim;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060087A8 RID: 34728 RVA: 0x0034C4B1 File Offset: 0x0034A6B1
		public readonly HashedString SaltSymbol
		{
			get
			{
				return this.saltSymbol;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060087A9 RID: 34729 RVA: 0x0034C4B9 File Offset: 0x0034A6B9
		public readonly CellOffset TableRelativeLocation
		{
			get
			{
				return this.tableRelativeLocation;
			}
		}

		// Token: 0x060087AA RID: 34730 RVA: 0x0034C4C1 File Offset: 0x0034A6C1
		public Seat(HashedString eatAnim, HashedString reloadElectrobankAnim, HashedString saltSymbol, CellOffset tableRelativeLocation)
		{
			this.eatAnim = eatAnim;
			this.reloadElectrobankAnim = reloadElectrobankAnim;
			this.saltSymbol = saltSymbol;
			this.tableRelativeLocation = tableRelativeLocation;
		}

		// Token: 0x04006783 RID: 26499
		private readonly HashedString eatAnim;

		// Token: 0x04006784 RID: 26500
		private readonly HashedString reloadElectrobankAnim;

		// Token: 0x04006785 RID: 26501
		private readonly HashedString saltSymbol;

		// Token: 0x04006786 RID: 26502
		private CellOffset tableRelativeLocation;
	}
}
