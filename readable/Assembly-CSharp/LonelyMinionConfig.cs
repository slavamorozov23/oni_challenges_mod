using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000334 RID: 820
public class LonelyMinionConfig : IEntityConfig
{
	// Token: 0x060010F3 RID: 4339 RVA: 0x000650C0 File Offset: 0x000632C0
	public GameObject CreatePrefab()
	{
		string name = DUPLICANTS.MODEL.STANDARD.NAME;
		GameObject gameObject = EntityTemplates.CreateEntity(LonelyMinionConfig.ID, name, true);
		gameObject.AddComponent<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		gameObject.AddComponent<Storage>().doDiseaseTransfer = false;
		gameObject.AddComponent<StateMachineController>();
		LonelyMinion.Def def = gameObject.AddOrGetDef<LonelyMinion.Def>();
		def.Personality = Db.Get().Personalities.Get("JORGE");
		def.Personality.Disabled = true;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.defaultAnim = "idle_default";
		kbatchedAnimController.initialAnim = "idle_default";
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("body_comp_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_interacts_lonely_dupe_kanim")
		};
		this.ConfigurePackageOverride(gameObject);
		SymbolOverrideController symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		symbolOverrideController.applySymbolOverridesEveryFrame = true;
		symbolOverrideController.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol(string.Format("cheek_00{0}", def.Personality.headShape)), 1);
		BaseMinionConfig.ConfigureSymbols(gameObject, true);
		return gameObject;
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x00065201 File Offset: 0x00063401
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x00065203 File Offset: 0x00063403
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x00065208 File Offset: 0x00063408
	private void ConfigurePackageOverride(GameObject go)
	{
		GameObject gameObject = new GameObject("PackageSnapPoint");
		gameObject.transform.SetParent(go.transform);
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.transform.position = Vector3.forward * -0.1f;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("mushbar_kanim")
		};
		kbatchedAnimController.initialAnim = "object";
		component.SetSymbolVisiblity(LonelyMinionConfig.PARCEL_SNAPTO, false);
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddOrGet<KBatchedAnimTracker>();
		kbatchedAnimTracker.controller = component;
		kbatchedAnimTracker.symbol = LonelyMinionConfig.PARCEL_SNAPTO;
	}

	// Token: 0x04000ACA RID: 2762
	public static string ID = "LonelyMinion";

	// Token: 0x04000ACB RID: 2763
	public const int VOICE_IDX = -2;

	// Token: 0x04000ACC RID: 2764
	public const int STARTING_SKILL_POINTS = 3;

	// Token: 0x04000ACD RID: 2765
	public const int BASE_ATTRIBUTE_LEVEL = 7;

	// Token: 0x04000ACE RID: 2766
	public const int AGE_MIN = 2190;

	// Token: 0x04000ACF RID: 2767
	public const int AGE_MAX = 3102;

	// Token: 0x04000AD0 RID: 2768
	public const float MIN_IDLE_DELAY = 20f;

	// Token: 0x04000AD1 RID: 2769
	public const float MAX_IDLE_DELAY = 40f;

	// Token: 0x04000AD2 RID: 2770
	public const string IDLE_PREFIX = "idle_blinds";

	// Token: 0x04000AD3 RID: 2771
	public static readonly HashedString GreetingCriteraId = "Neighbor";

	// Token: 0x04000AD4 RID: 2772
	public static readonly HashedString FoodCriteriaId = "FoodQuality";

	// Token: 0x04000AD5 RID: 2773
	public static readonly HashedString DecorCriteriaId = "Decor";

	// Token: 0x04000AD6 RID: 2774
	public static readonly HashedString PowerCriteriaId = "SuppliedPower";

	// Token: 0x04000AD7 RID: 2775
	public static readonly HashedString CHECK_MAIL = "mail_pre";

	// Token: 0x04000AD8 RID: 2776
	public static readonly HashedString CHECK_MAIL_SUCCESS = "mail_success_pst";

	// Token: 0x04000AD9 RID: 2777
	public static readonly HashedString CHECK_MAIL_FAILURE = "mail_failure_pst";

	// Token: 0x04000ADA RID: 2778
	public static readonly HashedString CHECK_MAIL_DUPLICATE = "mail_duplicate_pst";

	// Token: 0x04000ADB RID: 2779
	public static readonly HashedString FOOD_SUCCESS = "food_like_loop";

	// Token: 0x04000ADC RID: 2780
	public static readonly HashedString FOOD_FAILURE = "food_dislike_loop";

	// Token: 0x04000ADD RID: 2781
	public static readonly HashedString FOOD_DUPLICATE = "food_duplicate_loop";

	// Token: 0x04000ADE RID: 2782
	public static readonly HashedString FOOD_IDLE = "idle_food_quest";

	// Token: 0x04000ADF RID: 2783
	public static readonly HashedString DECOR_IDLE = "idle_decor_quest";

	// Token: 0x04000AE0 RID: 2784
	public static readonly HashedString POWER_IDLE = "idle_power_quest";

	// Token: 0x04000AE1 RID: 2785
	public static readonly HashedString BLINDS_IDLE_0 = "idle_blinds_0";

	// Token: 0x04000AE2 RID: 2786
	public static readonly HashedString PARCEL_SNAPTO = "parcel_snapTo";

	// Token: 0x04000AE3 RID: 2787
	public const string PERSONALITY_ID = "JORGE";

	// Token: 0x04000AE4 RID: 2788
	public const string BODY_ANIM_FILE = "body_lonelyminion_kanim";
}
