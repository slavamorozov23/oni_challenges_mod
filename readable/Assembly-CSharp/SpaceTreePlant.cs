using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class SpaceTreePlant : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>
{
	// Token: 0x060008D0 RID: 2256 RVA: 0x0003BEA4 File Offset: 0x0003A0A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessVariable));
		this.growing.InitializeStates(this.masterTarget, this.dead).DefaultState(this.growing.idle);
		this.growing.idle.EventTransition(GameHashes.Grow, this.growing.complete, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature)).EventTransition(GameHashes.Wilt, this.growing.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).PlayAnim((SpaceTreePlant.Instance smi) => "grow", KAnim.PlayMode.Paused).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshGrowingAnimation)).Update(new Action<SpaceTreePlant.Instance, float>(SpaceTreePlant.RefreshGrowingAnimationUpdate), UpdateRate.SIM_4000ms, false);
		this.growing.complete.EnterTransition(this.production, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.TrunkHasAtLeastOneBranch)).PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.production);
		this.growing.wilted.EventTransition(GameHashes.WiltRecover, this.growing.idle, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted))).PlayAnim(new Func<SpaceTreePlant.Instance, string>(SpaceTreePlant.GetGrowingStatesWiltedAnim), KAnim.PlayMode.Loop);
		this.production.InitializeStates(this.masterTarget, this.dead).EventTransition(GameHashes.Grow, this.growing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature))).ParamTransition<bool>(this.ReadyForHarvest, this.harvest, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsTrue).ParamTransition<float>(this.Fullness, this.harvest, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsGTEOne).DefaultState(this.production.producing);
		this.production.producing.EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).OnSignal(this.BranchWiltConditionChanged, this.production.halted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter>.Callback(SpaceTreePlant.CanNOTProduce)).OnSignal(this.BranchGrownStatusChanged, this.production.halted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter>.Callback(SpaceTreePlant.CanNOTProduce)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).ToggleStatusItem(Db.Get().CreatureStatusItems.ProducingSugarWater, null).Update(new Action<SpaceTreePlant.Instance, float>(SpaceTreePlant.ProductionUpdate), UpdateRate.SIM_200ms, false);
		this.production.halted.EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).EventTransition(GameHashes.TreeBranchCountChanged, this.production.producing, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanProduce)).OnSignal(this.BranchWiltConditionChanged, this.production.producing, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter>.Callback(SpaceTreePlant.CanProduce)).OnSignal(this.BranchGrownStatusChanged, this.production.producing, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Parameter<StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter>.Callback(SpaceTreePlant.CanProduce)).ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionPaused, null).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation));
		this.production.wilted.EventTransition(GameHashes.WiltRecover, this.production.producing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted))).ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionWilted, null).PlayAnim("idle_empty", KAnim.PlayMode.Once).EventHandler(GameHashes.EntombDefenseReactionBegins, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.InformBranchesTrunkWantsToBreakFree));
		this.harvest.InitializeStates(this.masterTarget, this.dead).EventTransition(GameHashes.Grow, this.growing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature))).ParamTransition<float>(this.Fullness, this.harvestCompleted, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsLTOne).EventHandler(GameHashes.EntombDefenseReactionBegins, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.InformBranchesTrunkWantsToBreakFree)).ToggleStatusItem(Db.Get().CreatureStatusItems.ReadyForHarvest, null).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SetReadyToHarvest)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.EnablePiping)).DefaultState(this.harvest.prevented);
		this.harvest.prevented.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.PlayHarvestReadyOnUntentombed)).EventTransition(GameHashes.HarvestDesignationChanged, this.harvest.manualHarvest, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanBeManuallyHarvested)).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (SpaceTreePlant.Instance smi) => SpaceTreePlant.HasPipeConnected(smi) && smi.IsPipingEnabled).ParamTransition<bool>(this.PipingEnabled, this.harvest.pipes, (SpaceTreePlant.Instance smi, bool pipeEnable) => pipeEnable && SpaceTreePlant.HasPipeConnected(smi));
		this.harvest.manualHarvest.DefaultState(this.harvest.manualHarvest.awaitingForFarmer).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.ShowSkillRequiredStatusItemIfSkillMissing)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.StartHarvestWorkChore)).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.PlayHarvestReadyOnUntentombed)).EventTransition(GameHashes.HarvestDesignationChanged, this.harvest.prevented, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanBeManuallyHarvested))).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (SpaceTreePlant.Instance smi) => SpaceTreePlant.HasPipeConnected(smi) && smi.IsPipingEnabled).ParamTransition<bool>(this.PipingEnabled, this.harvest.pipes, (SpaceTreePlant.Instance smi, bool pipeEnable) => pipeEnable && SpaceTreePlant.HasPipeConnected(smi)).WorkableCompleteTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.farmerWorkCompleted).Exit(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.CancelHarvestWorkChore)).Exit(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.HideSkillRequiredStatusItemIfSkillMissing));
		this.harvest.manualHarvest.awaitingForFarmer.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).WorkableStartTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.farmerWorking);
		this.harvest.manualHarvest.farmerWorking.WorkableStopTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.awaitingForFarmer);
		this.harvest.farmerWorkCompleted.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.DropInventory));
		this.harvest.pipes.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).PlayAnim("harvest_ready", KAnim.PlayMode.Loop).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshOnPipesHarvestAnimations)).EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.prevented, (SpaceTreePlant.Instance smi) => !smi.IsPipingEnabled || !SpaceTreePlant.HasPipeConnected(smi)).ParamTransition<bool>(this.PipingEnabled, this.harvest.prevented, (SpaceTreePlant.Instance smi, bool pipeEnable) => !pipeEnable || !SpaceTreePlant.HasPipeConnected(smi));
		this.harvestCompleted.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsetReadyToHarvest)).GoTo(this.production);
		this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(SpaceTreePlant.Instance smi)
		{
			if (!smi.IsWildPlanted && !smi.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
			{
				Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
				Notification notification = SpaceTreePlant.CreateDeathNotification(smi);
				notifier.Add(notification, "");
			}
			GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
			smi.Trigger(1623392196, null);
			smi.GetComponent<KBatchedAnimController>().StopAndClear();
			UnityEngine.Object.Destroy(smi.GetComponent<KBatchedAnimController>());
		}).ScheduleAction("Delayed Destroy", 0.5f, new Action<SpaceTreePlant.Instance>(SpaceTreePlant.SelfDestroy));
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0003C78A File Offset: 0x0003A98A
	public Workable GetWorkable(SpaceTreePlant.Instance smi)
	{
		return smi.GetWorkable();
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0003C792 File Offset: 0x0003A992
	public static void EnablePiping(SpaceTreePlant.Instance smi)
	{
		smi.SetPipingState(true);
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x0003C79B File Offset: 0x0003A99B
	public static void InformBranchesTrunkWantsToBreakFree(SpaceTreePlant.Instance smi)
	{
		smi.InformBranchesTrunkWantsToUnentomb();
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0003C7A3 File Offset: 0x0003A9A3
	public static void UnsubscribeToUpdateNewBranchesReadyForHarvest(SpaceTreePlant.Instance smi)
	{
		smi.UnsubscribeToUpdateNewBranchesReadyForHarvest();
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0003C7AB File Offset: 0x0003A9AB
	public static void SubscribeToUpdateNewBranchesReadyForHarvest(SpaceTreePlant.Instance smi)
	{
		smi.SubscribeToUpdateNewBranchesReadyForHarvest();
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0003C7B3 File Offset: 0x0003A9B3
	public static void RefreshFullnessVariable(SpaceTreePlant.Instance smi)
	{
		smi.RefreshFullnessVariable();
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0003C7BB File Offset: 0x0003A9BB
	public static void ShowSkillRequiredStatusItemIfSkillMissing(SpaceTreePlant.Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(true);
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0003C7C9 File Offset: 0x0003A9C9
	public static void HideSkillRequiredStatusItemIfSkillMissing(SpaceTreePlant.Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(false);
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x0003C7D7 File Offset: 0x0003A9D7
	public static void StartHarvestWorkChore(SpaceTreePlant.Instance smi)
	{
		smi.CreateHarvestChore();
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0003C7DF File Offset: 0x0003A9DF
	public static void CancelHarvestWorkChore(SpaceTreePlant.Instance smi)
	{
		smi.CancelHarvestChore();
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0003C7E7 File Offset: 0x0003A9E7
	public static bool HasPipeConnected(SpaceTreePlant.Instance smi)
	{
		return smi.HasPipeConnected;
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x0003C7EF File Offset: 0x0003A9EF
	public static bool CanBeManuallyHarvested(SpaceTreePlant.Instance smi)
	{
		return smi.CanBeManuallyHarvested;
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x0003C7F7 File Offset: 0x0003A9F7
	public static void SetReadyToHarvest(SpaceTreePlant.Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(true, smi, false);
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0003C80D File Offset: 0x0003AA0D
	public static void UnsetReadyToHarvest(SpaceTreePlant.Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(false, smi, false);
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x0003C823 File Offset: 0x0003AA23
	public static void RefreshOnPipesHarvestAnimations(SpaceTreePlant.Instance smi)
	{
		if (smi.IsReadyForHarvest)
		{
			SpaceTreePlant.PlayHarvestReadyOnUntentombed(smi);
			return;
		}
		SpaceTreePlant.RefreshFullnessAnimation(smi);
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0003C83A File Offset: 0x0003AA3A
	public static void RefreshFullnessAnimation(SpaceTreePlant.Instance smi)
	{
		smi.RefreshFullnessTreeTrunkAnimation();
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0003C842 File Offset: 0x0003AA42
	public static void ProductionUpdate(SpaceTreePlant.Instance smi, float dt)
	{
		smi.ProduceUpdate(dt);
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x0003C84B File Offset: 0x0003AA4B
	public static void DropInventory(SpaceTreePlant.Instance smi)
	{
		smi.DropInventory();
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0003C853 File Offset: 0x0003AA53
	public static void AddHarvestReadyTag(SpaceTreePlant.Instance smi)
	{
		smi.SetReadyForHarvestTag(true);
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0003C85C File Offset: 0x0003AA5C
	public static void RemoveHarvestReadyTag(SpaceTreePlant.Instance smi)
	{
		smi.SetReadyForHarvestTag(false);
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0003C865 File Offset: 0x0003AA65
	public static string GetGrowingStatesWiltedAnim(SpaceTreePlant.Instance smi)
	{
		return smi.GetTrunkWiltAnimation();
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0003C86D File Offset: 0x0003AA6D
	public static void RefreshGrowingAnimation(SpaceTreePlant.Instance smi)
	{
		smi.RefreshGrowingAnimation();
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0003C875 File Offset: 0x0003AA75
	public static void RefreshGrowingAnimationUpdate(SpaceTreePlant.Instance smi, float dt)
	{
		smi.RefreshGrowingAnimation();
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x0003C87D File Offset: 0x0003AA7D
	public static bool TrunkHasAtLeastOneBranch(SpaceTreePlant.Instance smi)
	{
		return smi.HasAtLeastOneBranch;
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x0003C885 File Offset: 0x0003AA85
	public static bool IsTrunkMature(SpaceTreePlant.Instance smi)
	{
		return smi.IsMature;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0003C88D File Offset: 0x0003AA8D
	public static bool IsTrunkWilted(SpaceTreePlant.Instance smi)
	{
		return smi.IsWilting;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0003C895 File Offset: 0x0003AA95
	public static bool CanNOTProduce(SpaceTreePlant.Instance smi, StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter param)
	{
		return SpaceTreePlant.CanNOTProduce(smi);
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0003C89D File Offset: 0x0003AA9D
	public static bool CanNOTProduce(SpaceTreePlant.Instance smi)
	{
		return !SpaceTreePlant.CanProduce(smi);
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0003C8A8 File Offset: 0x0003AAA8
	public static void PlayHarvestReadyOnUntentombed(SpaceTreePlant.Instance smi)
	{
		if (!smi.IsEntombed)
		{
			smi.PlayHarvestReadyAnimation();
		}
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x0003C8B8 File Offset: 0x0003AAB8
	public static void SelfDestroy(SpaceTreePlant.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0003C8C5 File Offset: 0x0003AAC5
	public static bool CanProduce(SpaceTreePlant.Instance smi, StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.SignalParameter param)
	{
		return SpaceTreePlant.CanProduce(smi);
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0003C8CD File Offset: 0x0003AACD
	public static bool CanProduce(SpaceTreePlant.Instance smi)
	{
		return !smi.IsUprooted && !smi.IsWilting && smi.IsMature && !smi.IsReadyForHarvest && smi.HasAtLeastOneHealthyFullyGrownBranch();
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x0003C8F8 File Offset: 0x0003AAF8
	public static Notification CreateDeathNotification(SpaceTreePlant.Instance smi)
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + smi.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x0400069E RID: 1694
	public const float WILD_PLANTED_SUGAR_WATER_PRODUCTION_SPEED_MODIFIER = 4f;

	// Token: 0x0400069F RID: 1695
	public static Tag SpaceTreeReadyForHarvest = TagManager.Create("SpaceTreeReadyForHarvest");

	// Token: 0x040006A0 RID: 1696
	public const string GROWN_WILT_ANIM_NAME = "idle_empty";

	// Token: 0x040006A1 RID: 1697
	public const string WILT_ANIM_NAME = "wilt";

	// Token: 0x040006A2 RID: 1698
	public const string GROW_ANIM_NAME = "grow";

	// Token: 0x040006A3 RID: 1699
	public const string GROW_PST_ANIM_NAME = "grow_pst";

	// Token: 0x040006A4 RID: 1700
	public const string FILL_ANIM_NAME = "grow_fill";

	// Token: 0x040006A5 RID: 1701
	public const string MANUAL_HARVEST_READY_ANIM_NAME = "harvest_ready";

	// Token: 0x040006A6 RID: 1702
	private const int FILLING_ANIMATION_FRAME_COUNT = 42;

	// Token: 0x040006A7 RID: 1703
	private const int WILT_LEVELS = 3;

	// Token: 0x040006A8 RID: 1704
	private const float PIPING_ENABLE_TRESHOLD = 0.25f;

	// Token: 0x040006A9 RID: 1705
	public const SimHashes ProductElement = SimHashes.SugarWater;

	// Token: 0x040006AA RID: 1706
	public SpaceTreePlant.GrowingState growing;

	// Token: 0x040006AB RID: 1707
	public SpaceTreePlant.ProductionStates production;

	// Token: 0x040006AC RID: 1708
	public SpaceTreePlant.HarvestStates harvest;

	// Token: 0x040006AD RID: 1709
	public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State harvestCompleted;

	// Token: 0x040006AE RID: 1710
	public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State dead;

	// Token: 0x040006AF RID: 1711
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.BoolParameter ReadyForHarvest;

	// Token: 0x040006B0 RID: 1712
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.BoolParameter PipingEnabled;

	// Token: 0x040006B1 RID: 1713
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.FloatParameter Fullness;

	// Token: 0x040006B2 RID: 1714
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Signal BranchWiltConditionChanged;

	// Token: 0x040006B3 RID: 1715
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Signal BranchGrownStatusChanged;

	// Token: 0x020011CB RID: 4555
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040065E0 RID: 26080
		public int OptimalAmountOfBranches;

		// Token: 0x040065E1 RID: 26081
		public float OptimalProductionDuration;
	}

	// Token: 0x020011CC RID: 4556
	public class GrowingState : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x040065E2 RID: 26082
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State idle;

		// Token: 0x040065E3 RID: 26083
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State complete;

		// Token: 0x040065E4 RID: 26084
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;
	}

	// Token: 0x020011CD RID: 4557
	public class ProductionStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x040065E5 RID: 26085
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;

		// Token: 0x040065E6 RID: 26086
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State halted;

		// Token: 0x040065E7 RID: 26087
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State producing;
	}

	// Token: 0x020011CE RID: 4558
	public class HarvestStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x040065E8 RID: 26088
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;

		// Token: 0x040065E9 RID: 26089
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State prevented;

		// Token: 0x040065EA RID: 26090
		public SpaceTreePlant.ManualHarvestStates manualHarvest;

		// Token: 0x040065EB RID: 26091
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State farmerWorkCompleted;

		// Token: 0x040065EC RID: 26092
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State pipes;
	}

	// Token: 0x020011CF RID: 4559
	public class ManualHarvestStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State
	{
		// Token: 0x040065ED RID: 26093
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State awaitingForFarmer;

		// Token: 0x040065EE RID: 26094
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State farmerWorking;
	}

	// Token: 0x020011D0 RID: 4560
	public new class Instance : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.GameInstance
	{
		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x0600858E RID: 34190 RVA: 0x00347E21 File Offset: 0x00346021
		public float OptimalProductionDuration
		{
			get
			{
				if (!this.IsWildPlanted)
				{
					return base.def.OptimalProductionDuration;
				}
				return base.def.OptimalProductionDuration * 4f;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x0600858F RID: 34191 RVA: 0x00347E48 File Offset: 0x00346048
		public float CurrentProductionProgress
		{
			get
			{
				return base.sm.Fullness.Get(this);
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06008590 RID: 34192 RVA: 0x00347E5B File Offset: 0x0034605B
		public bool IsWilting
		{
			get
			{
				return base.gameObject.HasTag(GameTags.Wilting);
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06008591 RID: 34193 RVA: 0x00347E6D File Offset: 0x0034606D
		public bool IsMature
		{
			get
			{
				return this.growingComponent.IsGrown();
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06008592 RID: 34194 RVA: 0x00347E7A File Offset: 0x0034607A
		public bool HasAtLeastOneBranch
		{
			get
			{
				return this.BranchCount > 0;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06008593 RID: 34195 RVA: 0x00347E85 File Offset: 0x00346085
		public bool IsReadyForHarvest
		{
			get
			{
				return base.sm.ReadyForHarvest.Get(base.smi);
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06008594 RID: 34196 RVA: 0x00347E9D File Offset: 0x0034609D
		public bool CanBeManuallyHarvested
		{
			get
			{
				return this.UserAllowsHarvest && !this.HasPipeConnected;
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06008595 RID: 34197 RVA: 0x00347EB2 File Offset: 0x003460B2
		public bool UserAllowsHarvest
		{
			get
			{
				return this.harvestDesignatable == null || (this.harvestDesignatable.HarvestWhenReady && this.harvestDesignatable.MarkedForHarvest);
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06008596 RID: 34198 RVA: 0x00347EDE File Offset: 0x003460DE
		public bool HasPipeConnected
		{
			get
			{
				return this.conduitDispenser.IsConnected;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06008597 RID: 34199 RVA: 0x00347EEB File Offset: 0x003460EB
		public bool IsUprooted
		{
			get
			{
				return this.uprootMonitor != null && this.uprootMonitor.IsUprooted;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06008598 RID: 34200 RVA: 0x00347F08 File Offset: 0x00346108
		public bool IsWildPlanted
		{
			get
			{
				return !this.receptacleMonitor.Replanted;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06008599 RID: 34201 RVA: 0x00347F18 File Offset: 0x00346118
		public bool IsEntombed
		{
			get
			{
				return this.entombDefenseSMI != null && this.entombDefenseSMI.IsEntombed;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600859A RID: 34202 RVA: 0x00347F2F File Offset: 0x0034612F
		public bool IsPipingEnabled
		{
			get
			{
				return base.sm.PipingEnabled.Get(this);
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x0600859B RID: 34203 RVA: 0x00347F42 File Offset: 0x00346142
		public int BranchCount
		{
			get
			{
				if (this.tree != null)
				{
					return this.tree.CurrentBranchCount;
				}
				return 0;
			}
		}

		// Token: 0x0600859C RID: 34204 RVA: 0x00347F59 File Offset: 0x00346159
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x0600859D RID: 34205 RVA: 0x00347F61 File Offset: 0x00346161
		public Instance(IStateMachineTarget master, SpaceTreePlant.Def def) : base(master, def)
		{
		}

		// Token: 0x0600859E RID: 34206 RVA: 0x00347F74 File Offset: 0x00346174
		public override void StartSM()
		{
			this.tree = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
			this.tree.ActionPerBranch(new Action<GameObject>(this.SubscribeToBranchCallbacks));
			this.tree.Subscribe(-1586842875, new Action<object>(this.SubscribeToNewBranches));
			this.entombDefenseSMI = base.smi.GetSMI<UnstableEntombDefense.Instance>();
			base.StartSM();
			this.SetPipingState(this.IsPipingEnabled);
			this.RefreshFullnessVariable();
			SpaceTreeSyrupHarvestWorkable spaceTreeSyrupHarvestWorkable = this.workable;
			spaceTreeSyrupHarvestWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(spaceTreeSyrupHarvestWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnManualHarvestWorkableStateChanges));
		}

		// Token: 0x0600859F RID: 34207 RVA: 0x00348016 File Offset: 0x00346216
		private void OnManualHarvestWorkableStateChanges(Workable workable, Workable.WorkableEvent workableEvent)
		{
			if (workableEvent == Workable.WorkableEvent.WorkStarted)
			{
				this.InformBranchesTrunkIsBeingHarvestedManually();
				return;
			}
			if (workableEvent == Workable.WorkableEvent.WorkStopped)
			{
				this.InformBranchesTrunkIsNoLongerBeingHarvestedManually();
			}
		}

		// Token: 0x060085A0 RID: 34208 RVA: 0x0034802C File Offset: 0x0034622C
		private void SubscribeToNewBranches(object obj)
		{
			if (obj == null)
			{
				return;
			}
			PlantBranch.Instance instance = (PlantBranch.Instance)obj;
			this.SubscribeToBranchCallbacks(instance.gameObject);
		}

		// Token: 0x060085A1 RID: 34209 RVA: 0x00348050 File Offset: 0x00346250
		private void SubscribeToBranchCallbacks(GameObject branch)
		{
			branch.Subscribe(-724860998, new Action<object>(this.OnBranchWiltStateChanged));
			branch.Subscribe(712767498, new Action<object>(this.OnBranchWiltStateChanged));
			branch.Subscribe(-254803949, new Action<object>(this.OnBranchGrowStatusChanged));
		}

		// Token: 0x060085A2 RID: 34210 RVA: 0x003480A5 File Offset: 0x003462A5
		private void OnBranchGrowStatusChanged(object obj)
		{
			base.sm.BranchGrownStatusChanged.Trigger(this);
		}

		// Token: 0x060085A3 RID: 34211 RVA: 0x003480B8 File Offset: 0x003462B8
		private void OnBranchWiltStateChanged(object obj)
		{
			base.sm.BranchWiltConditionChanged.Trigger(this);
		}

		// Token: 0x060085A4 RID: 34212 RVA: 0x003480CB File Offset: 0x003462CB
		public void SubscribeToUpdateNewBranchesReadyForHarvest()
		{
			this.onNewBranchesReadyHandle = this.tree.Subscribe(-1586842875, new Action<object>(this.OnNewBranchSpawnedWhileTreeIsReadyForHarvest));
		}

		// Token: 0x060085A5 RID: 34213 RVA: 0x003480EF File Offset: 0x003462EF
		public void UnsubscribeToUpdateNewBranchesReadyForHarvest()
		{
			this.tree.Unsubscribe(ref this.onNewBranchesReadyHandle);
		}

		// Token: 0x060085A6 RID: 34214 RVA: 0x00348102 File Offset: 0x00346302
		private void OnNewBranchSpawnedWhileTreeIsReadyForHarvest(object data)
		{
			if (data == null)
			{
				return;
			}
			((PlantBranch.Instance)data).gameObject.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
		}

		// Token: 0x060085A7 RID: 34215 RVA: 0x0034811D File Offset: 0x0034631D
		public void SetPipingState(bool enable)
		{
			base.sm.PipingEnabled.Set(enable, this, false);
			this.SetConduitDispenserAbilityToDispense(enable);
		}

		// Token: 0x060085A8 RID: 34216 RVA: 0x0034813A File Offset: 0x0034633A
		private void SetConduitDispenserAbilityToDispense(bool canDispense)
		{
			this.conduitDispenser.SetOnState(canDispense);
		}

		// Token: 0x060085A9 RID: 34217 RVA: 0x00348148 File Offset: 0x00346348
		public void SetReadyForHarvestTag(bool isReady)
		{
			if (isReady)
			{
				base.gameObject.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				if (this.tree == null)
				{
					return;
				}
				this.tree.ActionPerBranch(delegate(GameObject branch)
				{
					branch.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				});
				return;
			}
			else
			{
				base.gameObject.RemoveTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				if (this.tree == null)
				{
					return;
				}
				this.tree.ActionPerBranch(delegate(GameObject branch)
				{
					branch.RemoveTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				});
				return;
			}
		}

		// Token: 0x060085AA RID: 34218 RVA: 0x003481E0 File Offset: 0x003463E0
		public bool HasAtLeastOneHealthyFullyGrownBranch()
		{
			if (this.tree == null || this.BranchCount <= 0)
			{
				return false;
			}
			bool healthyGrownBranchFound = false;
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
				if (smi != null && !smi.isMasterNull)
				{
					healthyGrownBranchFound = (healthyGrownBranchFound || (smi.IsBranchFullyGrown && !smi.wiltCondition.IsWilting()));
				}
			});
			return healthyGrownBranchFound;
		}

		// Token: 0x060085AB RID: 34219 RVA: 0x0034822C File Offset: 0x0034642C
		public void CreateHarvestChore()
		{
			if (this.harvestChore == null)
			{
				this.harvestChore = new WorkChore<SpaceTreeSyrupHarvestWorkable>(Db.Get().ChoreTypes.Harvest, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x060085AC RID: 34220 RVA: 0x00348272 File Offset: 0x00346472
		public void CancelHarvestChore()
		{
			if (this.harvestChore != null)
			{
				this.harvestChore.Cancel("SpaceTreeSyrupProduction.CancelHarvestChore()");
				this.harvestChore = null;
			}
		}

		// Token: 0x060085AD RID: 34221 RVA: 0x00348294 File Offset: 0x00346494
		public void ProduceUpdate(float dt)
		{
			float mass = Mathf.Min(dt / base.smi.OptimalProductionDuration * base.smi.GetProductionSpeed() * this.storage.capacityKg, this.storage.RemainingCapacity());
			float lowTemp = ElementLoader.GetElement(SimHashes.SugarWater.CreateTag()).lowTemp;
			float num = 8f;
			float temperature = Mathf.Max(this.pe.Temperature, lowTemp + num);
			this.storage.AddLiquid(SimHashes.SugarWater, mass, temperature, byte.MaxValue, 0, false, true);
		}

		// Token: 0x060085AE RID: 34222 RVA: 0x00348324 File Offset: 0x00346524
		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			Storage storage = this.storage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			foreach (GameObject gameObject in list)
			{
				Vector3 position = gameObject.transform.position;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				gameObject.transform.SetPosition(position);
			}
		}

		// Token: 0x060085AF RID: 34223 RVA: 0x003483B4 File Offset: 0x003465B4
		public void PlayHarvestReadyAnimation()
		{
			if (this.animController != null)
			{
				this.animController.Play("harvest_ready", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x060085B0 RID: 34224 RVA: 0x003483E4 File Offset: 0x003465E4
		public void InformBranchesTrunkIsBeingHarvestedManually()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(2137182770, null);
			});
		}

		// Token: 0x060085B1 RID: 34225 RVA: 0x00348410 File Offset: 0x00346610
		public void InformBranchesTrunkIsNoLongerBeingHarvestedManually()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(-808006162, null);
			});
		}

		// Token: 0x060085B2 RID: 34226 RVA: 0x0034843C File Offset: 0x0034663C
		public void InformBranchesTrunkWantsToUnentomb()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(570354093, null);
			});
		}

		// Token: 0x060085B3 RID: 34227 RVA: 0x00348468 File Offset: 0x00346668
		public void RefreshFullnessVariable()
		{
			float num = this.storage.MassStored() / this.storage.capacityKg;
			base.sm.Fullness.Set(num, this, false);
			Boxed<float> boxed = Boxed<float>.Get(num);
			for (int i = 0; i < this.tree.CurrentBranchCount; i++)
			{
				GameObject branch = this.tree.GetBranch(i);
				if (branch != null)
				{
					branch.Trigger(-824970674, boxed);
				}
			}
			Boxed<float>.Release(boxed);
			if (num < 0.25f)
			{
				this.SetPipingState(false);
			}
		}

		// Token: 0x060085B4 RID: 34228 RVA: 0x003484F8 File Offset: 0x003466F8
		public float GetProductionSpeed()
		{
			if (this.tree == null)
			{
				return 0f;
			}
			float totalProduction = 0f;
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
				if (smi != null && !smi.isMasterNull)
				{
					totalProduction += smi.Productivity;
				}
			});
			return totalProduction / (float)base.def.OptimalAmountOfBranches;
		}

		// Token: 0x060085B5 RID: 34229 RVA: 0x00348550 File Offset: 0x00346750
		public string GetTrunkWiltAnimation()
		{
			int num = Mathf.Clamp(Mathf.FloorToInt(this.growing.PercentOfCurrentHarvest() / 0.33333334f), 0, 2);
			return "wilt" + (num + 1).ToString();
		}

		// Token: 0x060085B6 RID: 34230 RVA: 0x00348590 File Offset: 0x00346790
		public void RefreshFullnessTreeTrunkAnimation()
		{
			int num = Mathf.FloorToInt(this.CurrentProductionProgress * 42f);
			if (this.animController.currentAnim != "grow_fill")
			{
				this.animController.Play("grow_fill", KAnim.PlayMode.Paused, 1f, 0f);
				this.animController.SetPositionPercent(this.CurrentProductionProgress);
				this.animController.enabled = false;
				this.animController.enabled = true;
				return;
			}
			if (this.animController.currentFrame != num)
			{
				this.animController.SetPositionPercent(this.CurrentProductionProgress);
			}
		}

		// Token: 0x060085B7 RID: 34231 RVA: 0x00348634 File Offset: 0x00346834
		public void RefreshGrowingAnimation()
		{
			this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
		}

		// Token: 0x040065EF RID: 26095
		[MyCmpReq]
		private ReceptacleMonitor receptacleMonitor;

		// Token: 0x040065F0 RID: 26096
		[MyCmpReq]
		private KBatchedAnimController animController;

		// Token: 0x040065F1 RID: 26097
		[MyCmpReq]
		private Growing growingComponent;

		// Token: 0x040065F2 RID: 26098
		[MyCmpReq]
		private ConduitDispenser conduitDispenser;

		// Token: 0x040065F3 RID: 26099
		[MyCmpReq]
		private Storage storage;

		// Token: 0x040065F4 RID: 26100
		[MyCmpReq]
		private SpaceTreeSyrupHarvestWorkable workable;

		// Token: 0x040065F5 RID: 26101
		[MyCmpGet]
		private PrimaryElement pe;

		// Token: 0x040065F6 RID: 26102
		[MyCmpGet]
		private HarvestDesignatable harvestDesignatable;

		// Token: 0x040065F7 RID: 26103
		[MyCmpGet]
		private UprootedMonitor uprootMonitor;

		// Token: 0x040065F8 RID: 26104
		[MyCmpGet]
		private Growing growing;

		// Token: 0x040065F9 RID: 26105
		private PlantBranchGrower.Instance tree;

		// Token: 0x040065FA RID: 26106
		private UnstableEntombDefense.Instance entombDefenseSMI;

		// Token: 0x040065FB RID: 26107
		private Chore harvestChore;

		// Token: 0x040065FC RID: 26108
		private int onNewBranchesReadyHandle = -1;
	}
}
