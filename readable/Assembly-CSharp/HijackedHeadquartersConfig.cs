using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000265 RID: 613
public class HijackedHeadquartersConfig : IBuildingConfig
{
	// Token: 0x06000C76 RID: 3190 RVA: 0x0004AEDF File Offset: 0x000490DF
	public static int GetDataBankCost(Tag printableTag, int printCount = 0)
	{
		if (HijackedHeadquartersConfig.PrintableCostOverrides.ContainsKey(printableTag))
		{
			return HijackedHeadquartersConfig.PrintableCostOverrides[printableTag];
		}
		return 25 + Math.Min(printCount, 10) * 25;
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x0004AF08 File Offset: 0x00049108
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HijackedHeadquarters";
		int width = 5;
		int height = 5;
		string anim = "hijacked_hq_kanim";
		int hitpoints = 250;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 3200f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.Floodable = false;
		buildingDef.Entombable = true;
		buildingDef.Overheatable = false;
		buildingDef.ShowInBuildMenu = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "medium";
		buildingDef.ForegroundLayer = Grid.SceneLayer.Ground;
		return buildingDef;
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0004AFB8 File Offset: 0x000491B8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Steel, true);
		component.Temperature = 294.15f;
		BuildingTemplates.ExtendBuildingToGravitas(go);
		Storage storage = go.AddComponent<Storage>();
		storage.capacityKg = 275f;
		Activatable activatable = go.AddComponent<Activatable>();
		activatable.synchronizeAnims = false;
		activatable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		activatable.SetWorkTime(30f);
		go.AddOrGetDef<HijackedHeadquarters.Def>();
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = DatabankHelper.ID;
		manualDeliveryKG.MinimumMass = 0f;
		manualDeliveryKG.refillMass = 25f;
		manualDeliveryKG.capacity = storage.capacityKg;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Operational;
		manualDeliveryKG.ShowStatusItem = false;
		manualDeliveryKG.RoundFetchAmountToInt = true;
		manualDeliveryKG.FillToCapacity = true;
		go.AddComponent<DropToUserCapacity>();
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<Activatable>().SetOffsets(OffsetGroups.LeftOrRight);
			StoryManager.Instance.ForceCreateStory(Db.Get().Stories.HijackedHeadquarters, game_object.GetMyWorldId());
		};
	}

	// Token: 0x04000890 RID: 2192
	public const string ID = "HijackedHeadquarters";

	// Token: 0x04000891 RID: 2193
	private const int WIDTH = 5;

	// Token: 0x04000892 RID: 2194
	private const int HEIGHT = 5;

	// Token: 0x04000893 RID: 2195
	public const int DEFAULT_DATABANK_PRINT_COST = 25;

	// Token: 0x04000894 RID: 2196
	public const int COST_INCREASE_PER_PRINT = 25;

	// Token: 0x04000895 RID: 2197
	public const int MAX_COST_INCREASES_PER_PRINT = 10;

	// Token: 0x04000896 RID: 2198
	private static Dictionary<Tag, int> PrintableCostOverrides = new Dictionary<Tag, int>();
}
