using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003EC RID: 1004
public class SweepBotConfig : IEntityConfig
{
	// Token: 0x060014AC RID: 5292 RVA: 0x00076330 File Offset: 0x00074530
	public GameObject CreatePrefab()
	{
		string id = "SweepBot";
		string text = this.name;
		string text2 = this.desc;
		float mass = SweepBotConfig.MASS;
		EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, text, text2, mass, Assets.GetAnim("sweep_bot_kanim"), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, none, default(EffectorValues), SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.GetComponent<KBatchedAnimController>().isMovable = true;
		KPrefabID kprefabID = gameObject.AddOrGet<KPrefabID>();
		kprefabID.AddTag(GameTags.Creature, false);
		kprefabID.AddTag(GameTags.Robot, false);
		kprefabID.AddTag(GameTags.OrnamentDisplayer, false);
		gameObject.AddComponent<Pickupable>();
		gameObject.AddOrGet<Clearable>().isClearable = false;
		Trait trait = Db.Get().CreateTrait("SweepBotBaseTrait", this.name, this.name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.InternalBattery.maxAttribute.Id, 9000f, this.name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.InternalBattery.deltaAttribute.Id, -17.142857f, this.name, false, false, true));
		Modifiers modifiers = gameObject.AddOrGet<Modifiers>();
		modifiers.initialTraits.Add("SweepBotBaseTrait");
		modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
		modifiers.initialAmounts.Add(Db.Get().Amounts.InternalBattery.Id);
		gameObject.AddOrGet<KBatchedAnimController>().SetSymbolVisiblity("snapto_pivot", false);
		gameObject.AddOrGet<Traits>();
		gameObject.AddOrGet<Effects>();
		gameObject.AddOrGetDef<AnimInterruptMonitor.Def>();
		gameObject.AddOrGetDef<StorageUnloadMonitor.Def>();
		RobotBatteryMonitor.Def def = gameObject.AddOrGetDef<RobotBatteryMonitor.Def>();
		def.batteryAmountId = Db.Get().Amounts.InternalBattery.Id;
		def.canCharge = true;
		def.lowBatteryWarningPercent = 0.5f;
		gameObject.AddOrGetDef<SweepBotReactMonitor.Def>();
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		gameObject.AddOrGetDef<SweepBotTrappedMonitor.Def>();
		gameObject.AddOrGetDef<DrinkMilkMonitor.Def>().consumesMilk = false;
		gameObject.AddOrGet<AnimEventHandler>();
		gameObject.AddOrGet<SnapOn>().snapPoints = new List<SnapOn.SnapPoint>(new SnapOn.SnapPoint[]
		{
			new SnapOn.SnapPoint
			{
				pointName = "carry",
				automatic = false,
				context = "",
				buildFile = null,
				overrideSymbol = "snapTo_ornament"
			}
		});
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		gameObject.AddComponent<Storage>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.capacityKg = 500f;
		storage.storageFXOffset = new Vector3(0f, 0.5f, 0f);
		gameObject.AddOrGet<MovingOrnamentReceptacle>().AddDepositTag(GameTags.PedestalDisplayable);
		gameObject.AddOrGet<DecorProvider>();
		gameObject.AddOrGet<UserNameable>();
		gameObject.AddOrGet<CharacterOverlay>();
		gameObject.AddOrGet<ItemPedestal>();
		Navigator navigator = gameObject.AddOrGet<Navigator>();
		navigator.NavGridName = "WalkerBabyNavGrid";
		navigator.CurrentNavType = NavType.Floor;
		navigator.defaultSpeed = 1f;
		navigator.updateProber = true;
		navigator.maxProbeRadiusX = 32;
		navigator.maxProbeRadiusY = 1;
		navigator.sceneLayer = Grid.SceneLayer.Creatures;
		kprefabID.AddTag(GameTags.Creatures.Walker, false);
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new FallStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new SweepBotTrappedStates.Def(), true, -1).Add(new DeliverToSweepLockerStates.Def(), true, -1).Add(new ReturnToChargeStationStates.Def(), true, -1).PushInterruptGroup().Add(new DrinkMilkStates.Def
		{
			shouldBeBehindMilkTank = true
		}, true, -1).PopInterruptGroup().Add(new SweepStates.Def(), true, -1).Add(new IdleStates.Def(), true, -1);
		gameObject.AddOrGet<LoopingSounds>();
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Robots.Models.SweepBot, null);
		return gameObject;
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x000766D6 File Offset: 0x000748D6
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x000766D8 File Offset: 0x000748D8
	public void OnSpawn(GameObject inst)
	{
		StorageUnloadMonitor.Instance smi = inst.GetSMI<StorageUnloadMonitor.Instance>();
		smi.sm.internalStorage.Set(inst.GetComponents<Storage>()[1], smi, false);
		inst.GetComponent<MovingOrnamentReceptacle>();
		inst.GetSMI<CreatureFallMonitor.Instance>().anim = "idle_loop";
	}

	// Token: 0x04000C88 RID: 3208
	public const string ID = "SweepBot";

	// Token: 0x04000C89 RID: 3209
	public const string BASE_TRAIT_ID = "SweepBotBaseTrait";

	// Token: 0x04000C8A RID: 3210
	public const float STORAGE_CAPACITY = 500f;

	// Token: 0x04000C8B RID: 3211
	public const float BATTERY_CAPACITY = 9000f;

	// Token: 0x04000C8C RID: 3212
	public const float BATTERY_DEPLETION_RATE = 17.142857f;

	// Token: 0x04000C8D RID: 3213
	public const float MAX_SWEEP_AMOUNT = 10f;

	// Token: 0x04000C8E RID: 3214
	public const float MOP_SPEED = 10f;

	// Token: 0x04000C8F RID: 3215
	private string name = STRINGS.ROBOTS.MODELS.SWEEPBOT.NAME;

	// Token: 0x04000C90 RID: 3216
	private string desc = STRINGS.ROBOTS.MODELS.SWEEPBOT.DESC;

	// Token: 0x04000C91 RID: 3217
	public static float MASS = 25f;
}
