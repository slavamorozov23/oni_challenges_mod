using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020001B7 RID: 439
public class SpaceTreeBranch : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>
{
	// Token: 0x06000894 RID: 2196 RVA: 0x00039E44 File Offset: 0x00038044
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.growing;
		this.root.EventTransition(GameHashes.Uprooted, this.die, null).EventHandler(GameHashes.Wilt, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.UpdateFlowerOnWilt)).EventHandler(GameHashes.WiltRecover, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.UpdateFlowerOnWiltRecover));
		this.growing.InitializeStates(this.masterTarget, this.die).EnterTransition(this.grown, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsBranchFullyGrown)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.ForbidBranchToBeHarvestedForWood)).EventTransition(GameHashes.Wilt, this.halt, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsWiltedConditionReportingWilted)).EventTransition(GameHashes.RootHealthChanged, this.halt, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.PlanterStorage, this.growing.planted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted))).EventTransition(GameHashes.PlanterStorage, this.growing.wild, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted)).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, null).Update("CheckGrown", delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			if (smi.GetcurrentGrowthPercentage() >= 1f)
			{
				smi.gameObject.Trigger(-254803949, null);
				smi.GoTo(this.grown);
			}
		}, UpdateRate.SIM_4000ms, false);
		this.growing.wild.DefaultState(this.growing.wild.visible).EnterTransition(this.growing.planted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted))).ToggleAttributeModifier("GrowingWild", (SpaceTreeBranch.Instance smi) => smi.wildGrowingRate, null);
		this.growing.wild.visible.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.wild.visible), KAnim.PlayMode.Paused).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.wild.hidden, false).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState));
		this.growing.wild.hidden.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.wild.visible, true).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.wild.hidden), KAnim.PlayMode.Once);
		this.growing.planted.DefaultState(this.growing.planted.visible).EnterTransition(this.growing.wild, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted)).ToggleAttributeModifier("Growing", (SpaceTreeBranch.Instance smi) => smi.baseGrowingRate, null);
		this.growing.planted.visible.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.planted.visible), KAnim.PlayMode.Paused).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.planted.hidden, false).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState));
		this.growing.planted.hidden.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.planted.visible, true).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.planted.hidden), KAnim.PlayMode.Once);
		this.halt.InitializeStates(this.masterTarget, this.die).DefaultState(this.halt.wilted).EventHandlerTransition(GameHashes.RootHealthChanged, this.growing, (SpaceTreeBranch.Instance smi, object o) => SpaceTreeBranch.IsTrunkHealthy(smi) && !SpaceTreeBranch.IsWiltedConditionReportingWilted(smi)).EventTransition(GameHashes.WiltRecover, this.growing, null).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.halt.hidden, false);
		this.halt.wilted.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.wilted), KAnim.PlayMode.Paused).EventTransition(GameHashes.RootHealthChanged, this.halt.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState)).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.halt.shaking, (SpaceTreeBranch.Instance o, object smi) => true);
		this.halt.trunkWilted.EventTransition(GameHashes.RootHealthChanged, this.halt.wilted, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy)).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.trunkWilted), KAnim.PlayMode.Once).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.halt.shaking, (SpaceTreeBranch.Instance o, object smi) => true);
		this.halt.shaking.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.shaking), KAnim.PlayMode.Once).ScheduleGoTo(1.8f, this.halt.wilted);
		this.halt.hidden.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.hidden), KAnim.PlayMode.Once).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.halt.wilted, true);
		this.grown.InitializeStates(this.masterTarget, this.die).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.EnableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.AllowItToBeHarvestForWood)).EventTransition(GameHashes.Harvest, this.harvestedForWood, null).EventTransition(GameHashes.ConsumePlant, this.growing, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsBranchFullyGrown))).DefaultState(this.grown.spawn);
		this.grown.spawn.EventTransition(GameHashes.Wilt, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.RootHealthChanged, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).ParamTransition<bool>(this.HasSpawn, this.grown.healthy, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.IsTrue).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.spawn), KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.spawnPST);
		this.grown.spawnPST.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.spawnPST), KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.healthy);
		this.grown.healthy.Enter(delegate(SpaceTreeBranch.Instance smi)
		{
			this.HasSpawn.Set(true, smi, false);
		}).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.EnableGlowFlowerMeter)).Exit(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).ToggleStatusItem(Db.Get().CreatureStatusItems.SpaceTreeBranchLightStatus, null).DefaultState(this.grown.healthy.filling);
		this.grown.healthy.filling.EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationOnUnentomb)).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).EventTransition(GameHashes.Wilt, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.RootHealthChanged, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.grown.healthy.trunkReadyForHarvest, false);
		this.grown.healthy.trunkReadyForHarvest.DefaultState(this.grown.healthy.trunkReadyForHarvest.idle).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.grown.healthy.filling, true);
		this.grown.healthy.trunkReadyForHarvest.idle.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.idle), KAnim.PlayMode.Loop).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayReadyForHarvestAnimationOnUnentomb)).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.grown.healthy.trunkReadyForHarvest.shaking, (SpaceTreeBranch.Instance o, object smi) => true).EventTransition(GameHashes.SpaceTreeManualHarvestBegan, this.grown.healthy.trunkReadyForHarvest.harvestInProgress, null).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "harvest_ready");
		}, UpdateRate.SIM_200ms, false);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.DefaultState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre).EventTransition(GameHashes.SpaceTreeManualHarvestStopped, this.grown.healthy.trunkReadyForHarvest.idle, null);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre), KAnim.PlayMode.Once).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_pre");
		}, UpdateRate.SIM_200ms, false).Transition(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.TransitToManualHarvest_Loop), UpdateRate.SIM_200ms);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop), KAnim.PlayMode.Loop).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_loop");
		}, UpdateRate.SIM_200ms, false).Transition(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.TransitToManualHarvest_Pst), UpdateRate.SIM_200ms);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst), KAnim.PlayMode.Once).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_pst");
		}, UpdateRate.SIM_200ms, false);
		this.grown.healthy.trunkReadyForHarvest.shaking.PlayAnim((SpaceTreeBranch.Instance smi) => smi.entombDefenseSMI.UnentombAnimName, KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.healthy.trunkReadyForHarvest.idle);
		this.grown.trunkWilted.DefaultState(this.grown.trunkWilted.wilted).EventTransition(GameHashes.RootHealthChanged, this.grown.spawn, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy)).EventTransition(GameHashes.WiltRecover, this.grown.spawn, null).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.grown.healthy.trunkReadyForHarvest, false);
		this.grown.trunkWilted.wilted.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.trunkWilted), KAnim.PlayMode.Once).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.grown.trunkWilted.shaking, (SpaceTreeBranch.Instance o, object smi) => true);
		this.grown.trunkWilted.shaking.PlayAnim((SpaceTreeBranch.Instance smi) => smi.entombDefenseSMI.UnentombAnimName, KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.trunkWilted.wilted);
		this.harvestedForWood.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.harvestedForWood), KAnim.PlayMode.Once).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SpawnWoodOnHarvest)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.ForbidBranchToBeHarvestedForWood)).Exit(delegate(SpaceTreeBranch.Instance smi)
		{
			smi.Trigger(113170146, null);
		}).OnAnimQueueComplete(this.growing);
		this.die.Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).DefaultState(this.die.entering);
		this.die.entering.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.die.entering), KAnim.PlayMode.Once).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SpawnWoodOnDeath)).OnAnimQueueComplete(this.die.selfDelete).ScheduleGoTo(2f, this.die.selfDelete);
		this.die.selfDelete.Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SelfDestroy));
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x0003AB77 File Offset: 0x00038D77
	public static bool TransitToManualHarvest_Loop(SpaceTreeBranch.Instance smi)
	{
		return smi.GetCurrentTrunkAnim() != null && smi.GetCurrentTrunkAnim() == "syrup_harvest_trunk_loop";
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0003ABA3 File Offset: 0x00038DA3
	public static bool TransitToManualHarvest_Pst(SpaceTreeBranch.Instance smi)
	{
		return smi.GetCurrentTrunkAnim() != null && smi.GetCurrentTrunkAnim() == "syrup_harvest_trunk_pst";
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0003ABCF File Offset: 0x00038DCF
	public static bool IsWiltedConditionReportingWilted(SpaceTreeBranch.Instance smi)
	{
		return smi.wiltCondition.IsWilting();
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0003ABDC File Offset: 0x00038DDC
	public static bool IsBranchFullyGrown(SpaceTreeBranch.Instance smi)
	{
		return smi.IsBranchFullyGrown;
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0003ABE4 File Offset: 0x00038DE4
	public static bool IsTrunkWildPlanted(SpaceTreeBranch.Instance smi)
	{
		return smi.IsTrunkWildPlanted;
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0003ABEC File Offset: 0x00038DEC
	public static bool IsEntombed(SpaceTreeBranch.Instance smi)
	{
		return smi.IsEntombed;
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x0003ABF4 File Offset: 0x00038DF4
	public static bool IsTrunkHealthy(SpaceTreeBranch.Instance smi)
	{
		return smi.IsTrunkHealthy;
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0003ABFC File Offset: 0x00038DFC
	public static void PlayFillAnimationForThisState(SpaceTreeBranch.Instance smi)
	{
		smi.PlayFillAnimation();
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x0003AC04 File Offset: 0x00038E04
	public static void OnTrunkSyrupFullnessChanged(SpaceTreeBranch.Instance smi, object obj)
	{
		smi.PlayFillAnimation(((Boxed<float>)obj).value);
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x0003AC17 File Offset: 0x00038E17
	public static void SynchAnimationWithTrunk(SpaceTreeBranch.Instance smi, HashedString animName)
	{
		smi.SynchCurrentAnimWithTrunkAnim(animName);
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x0003AC20 File Offset: 0x00038E20
	public static void EnableGlowFlowerMeter(SpaceTreeBranch.Instance smi)
	{
		smi.ActivateGlowFlowerMeter();
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x0003AC28 File Offset: 0x00038E28
	public static void DisableGlowFlowerMeter(SpaceTreeBranch.Instance smi)
	{
		smi.DeactivateGlowFlowerMeter();
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x0003AC30 File Offset: 0x00038E30
	public static void UpdateFlowerOnWilt(SpaceTreeBranch.Instance smi)
	{
		smi.PlayAnimOnFlower(smi.Animations.meterAnim_flowerWilted, KAnim.PlayMode.Loop);
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0003AC44 File Offset: 0x00038E44
	public static void UpdateFlowerOnWiltRecover(SpaceTreeBranch.Instance smi)
	{
		smi.PlayAnimOnFlower(smi.Animations.meterAnimNames, KAnim.PlayMode.Loop);
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x0003AC58 File Offset: 0x00038E58
	public static void EnableEntombDefenses(SpaceTreeBranch.Instance smi)
	{
		smi.GetSMI<UnstableEntombDefense.Instance>().SetActive(true);
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0003AC66 File Offset: 0x00038E66
	public static void DisableEntombDefenses(SpaceTreeBranch.Instance smi)
	{
		smi.GetSMI<UnstableEntombDefense.Instance>().SetActive(false);
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0003AC74 File Offset: 0x00038E74
	public static void AllowItToBeHarvestForWood(SpaceTreeBranch.Instance smi)
	{
		smi.harvestable.SetCanBeHarvested(true);
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0003AC82 File Offset: 0x00038E82
	public static void ForbidBranchToBeHarvestedForWood(SpaceTreeBranch.Instance smi)
	{
		smi.harvestable.SetCanBeHarvested(false);
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0003AC90 File Offset: 0x00038E90
	public static void SpawnWoodOnHarvest(SpaceTreeBranch.Instance smi)
	{
		smi.crop.SpawnConfiguredFruit(null);
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x0003AC9E File Offset: 0x00038E9E
	public static void SpawnWoodOnDeath(SpaceTreeBranch.Instance smi)
	{
		if (smi.harvestable != null && smi.harvestable.CanBeHarvested)
		{
			smi.crop.SpawnConfiguredFruit(null);
		}
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0003ACC7 File Offset: 0x00038EC7
	public static void OnConsumed(SpaceTreeBranch.Instance smi)
	{
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0003ACC9 File Offset: 0x00038EC9
	public static void SelfDestroy(SpaceTreeBranch.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x0003ACD6 File Offset: 0x00038ED6
	public static void PlayFillAnimationOnUnentomb(SpaceTreeBranch.Instance smi)
	{
		if (!smi.IsEntombed)
		{
			SpaceTreeBranch.PlayFillAnimationForThisState(smi);
		}
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x0003ACE6 File Offset: 0x00038EE6
	public static void PlayReadyForHarvestAnimationOnUnentomb(SpaceTreeBranch.Instance smi)
	{
		if (!smi.IsEntombed)
		{
			smi.PlayReadyForHarvestAnimation();
		}
	}

	// Token: 0x0400067A RID: 1658
	public const int FILL_ANIM_FRAME_COUNT = 42;

	// Token: 0x0400067B RID: 1659
	public const int SHAKE_ANIM_FRAME_COUNT = 54;

	// Token: 0x0400067C RID: 1660
	public const float SHAKE_ANIM_DURATION = 1.8f;

	// Token: 0x0400067D RID: 1661
	private StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.BoolParameter HasSpawn;

	// Token: 0x0400067E RID: 1662
	private SpaceTreeBranch.GrowingStates growing;

	// Token: 0x0400067F RID: 1663
	private SpaceTreeBranch.GrowHaltState halt;

	// Token: 0x04000680 RID: 1664
	private SpaceTreeBranch.GrownStates grown;

	// Token: 0x04000681 RID: 1665
	private GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State harvestedForWood;

	// Token: 0x04000682 RID: 1666
	private SpaceTreeBranch.DieStates die;

	// Token: 0x020011BD RID: 4541
	public class AnimSet
	{
		// Token: 0x04006595 RID: 26005
		public string[] meterTargets;

		// Token: 0x04006596 RID: 26006
		public string[] meterAnimNames;

		// Token: 0x04006597 RID: 26007
		public string undeveloped;

		// Token: 0x04006598 RID: 26008
		public string spawn;

		// Token: 0x04006599 RID: 26009
		public string spawn_pst;

		// Token: 0x0400659A RID: 26010
		public string fill;

		// Token: 0x0400659B RID: 26011
		public string ready_harvest;

		// Token: 0x0400659C RID: 26012
		public string[] meterAnim_flowerWilted;

		// Token: 0x0400659D RID: 26013
		public string wilted;

		// Token: 0x0400659E RID: 26014
		public string wilted_short_trunk_healthy;

		// Token: 0x0400659F RID: 26015
		public string wilted_short_trunk_wilted;

		// Token: 0x040065A0 RID: 26016
		public string hidden;

		// Token: 0x040065A1 RID: 26017
		public string die;

		// Token: 0x040065A2 RID: 26018
		public string manual_harvest_pre;

		// Token: 0x040065A3 RID: 26019
		public string manual_harvest_loop;

		// Token: 0x040065A4 RID: 26020
		public string manual_harvest_pst;
	}

	// Token: 0x020011BE RID: 4542
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040065A5 RID: 26021
		public int OPTIMAL_LUX_LEVELS;

		// Token: 0x040065A6 RID: 26022
		public float GROWTH_RATE = 0.0016666667f;

		// Token: 0x040065A7 RID: 26023
		public float WILD_GROWTH_RATE = 0.00041666668f;
	}

	// Token: 0x020011BF RID: 4543
	public class GrowingState : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x040065A8 RID: 26024
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State visible;

		// Token: 0x040065A9 RID: 26025
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State hidden;
	}

	// Token: 0x020011C0 RID: 4544
	public class GrowingStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
	{
		// Token: 0x040065AA RID: 26026
		public SpaceTreeBranch.GrowingState wild;

		// Token: 0x040065AB RID: 26027
		public SpaceTreeBranch.GrowingState planted;
	}

	// Token: 0x020011C1 RID: 4545
	public class GrownStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
	{
		// Token: 0x040065AC RID: 26028
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State spawn;

		// Token: 0x040065AD RID: 26029
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State spawnPST;

		// Token: 0x040065AE RID: 26030
		public SpaceTreeBranch.HealthyStates healthy;

		// Token: 0x040065AF RID: 26031
		public SpaceTreeBranch.WiltStates trunkWilted;
	}

	// Token: 0x020011C2 RID: 4546
	public class GrowHaltState : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
	{
		// Token: 0x040065B0 RID: 26032
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State wilted;

		// Token: 0x040065B1 RID: 26033
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State trunkWilted;

		// Token: 0x040065B2 RID: 26034
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;

		// Token: 0x040065B3 RID: 26035
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State hidden;
	}

	// Token: 0x020011C3 RID: 4547
	public class WiltStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x040065B4 RID: 26036
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State wilted;

		// Token: 0x040065B5 RID: 26037
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;
	}

	// Token: 0x020011C4 RID: 4548
	public class DieStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x040065B6 RID: 26038
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State entering;

		// Token: 0x040065B7 RID: 26039
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State selfDelete;
	}

	// Token: 0x020011C5 RID: 4549
	public class ReadyForHarvest : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x040065B8 RID: 26040
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State idle;

		// Token: 0x040065B9 RID: 26041
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;

		// Token: 0x040065BA RID: 26042
		public SpaceTreeBranch.ManualHarvestStates harvestInProgress;
	}

	// Token: 0x020011C6 RID: 4550
	public class ManualHarvestStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x040065BB RID: 26043
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State pre;

		// Token: 0x040065BC RID: 26044
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State loop;

		// Token: 0x040065BD RID: 26045
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State pst;
	}

	// Token: 0x020011C7 RID: 4551
	public class HealthyStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x040065BE RID: 26046
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State filling;

		// Token: 0x040065BF RID: 26047
		public SpaceTreeBranch.ReadyForHarvest trunkReadyForHarvest;
	}

	// Token: 0x020011C8 RID: 4552
	public new class Instance : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameInstance, IManageGrowingStates
	{
		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06008558 RID: 34136 RVA: 0x00347364 File Offset: 0x00345564
		public int CurrentAmountOfLux
		{
			get
			{
				return Grid.LightIntensity[this.cell];
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06008559 RID: 34137 RVA: 0x00347376 File Offset: 0x00345576
		public float Productivity
		{
			get
			{
				if (!this.IsBranchFullyGrown)
				{
					return 0f;
				}
				return Mathf.Clamp((float)this.CurrentAmountOfLux / (float)base.def.OPTIMAL_LUX_LEVELS, 0f, 1f);
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x0600855A RID: 34138 RVA: 0x003473A9 File Offset: 0x003455A9
		public bool IsTrunkHealthy
		{
			get
			{
				return this.trunk != null && !this.trunk.HasTag(GameTags.Wilting);
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x0600855B RID: 34139 RVA: 0x003473C8 File Offset: 0x003455C8
		public bool IsTrunkWildPlanted
		{
			get
			{
				return this.trunk != null && !this.trunk.GetComponent<ReceptacleMonitor>().Replanted;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x0600855C RID: 34140 RVA: 0x003473E7 File Offset: 0x003455E7
		public bool IsEntombed
		{
			get
			{
				return this.entombDefenseSMI != null && this.entombDefenseSMI.IsEntombed;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x0600855D RID: 34141 RVA: 0x003473FE File Offset: 0x003455FE
		public bool IsBranchFullyGrown
		{
			get
			{
				return this.GetcurrentGrowthPercentage() >= 1f;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x0600855E RID: 34142 RVA: 0x00347410 File Offset: 0x00345610
		private PlantBranchGrower.Instance trunk
		{
			get
			{
				if (this._trunk == null)
				{
					this._trunk = this.branch.GetTrunk();
					if (this._trunk != null)
					{
						this.trunkAnimController = this._trunk.GetComponent<KBatchedAnimController>();
					}
				}
				return this._trunk;
			}
		}

		// Token: 0x0600855F RID: 34143 RVA: 0x0034744C File Offset: 0x0034564C
		public void OverrideMaturityLevel(float percent)
		{
			float value = this.maturity.GetMax() * percent;
			this.maturity.SetValue(value);
		}

		// Token: 0x06008560 RID: 34144 RVA: 0x00347474 File Offset: 0x00345674
		public Instance(IStateMachineTarget master, SpaceTreeBranch.Def def) : base(master, def)
		{
			this.cell = Grid.PosToCell(this);
			Amounts amounts = base.gameObject.GetAmounts();
			this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
			this.baseGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.GROWTH_RATE, CREATURES.STATS.MATURITY.GROWING, false, false, true);
			this.wildGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.WILD_GROWTH_RATE, CREATURES.STATS.MATURITY.GROWINGWILD, false, false, true);
			base.Subscribe(1272413801, new Action<object>(this.ResetGrowth));
		}

		// Token: 0x06008561 RID: 34145 RVA: 0x0034753B File Offset: 0x0034573B
		public float GetcurrentGrowthPercentage()
		{
			return this.maturity.value / this.maturity.GetMax();
		}

		// Token: 0x06008562 RID: 34146 RVA: 0x00347554 File Offset: 0x00345754
		public void ResetGrowth(object data = null)
		{
			this.maturity.value = 0f;
			base.sm.HasSpawn.Set(false, this, false);
			base.smi.gameObject.Trigger(-254803949, null);
		}

		// Token: 0x06008563 RID: 34147 RVA: 0x00347590 File Offset: 0x00345790
		public override void StartSM()
		{
			this.branch = base.smi.GetSMI<PlantBranch.Instance>();
			this.entombDefenseSMI = base.smi.GetSMI<UnstableEntombDefense.Instance>();
			if (this.Animations.meterTargets != null)
			{
				this.CreateMeters(this.Animations.meterTargets, this.Animations.meterAnimNames);
			}
			base.StartSM();
		}

		// Token: 0x06008564 RID: 34148 RVA: 0x003475F0 File Offset: 0x003457F0
		public void CreateMeters(string[] meterTargets, string[] meterAnimNames)
		{
			this.flowerMeters = new MeterController[meterTargets.Length];
			for (int i = 0; i < this.flowerMeters.Length; i++)
			{
				this.flowerMeters[i] = new MeterController(this.animController, meterTargets[i], meterAnimNames[i], Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
			}
		}

		// Token: 0x06008565 RID: 34149 RVA: 0x00347640 File Offset: 0x00345840
		public void RefreshAnimation()
		{
			if (this.flowerMeters == null && this.Animations.meterTargets != null)
			{
				this.CreateMeters(this.Animations.meterTargets, this.Animations.meterAnimNames);
			}
			KAnim.PlayMode mode = base.IsInsideState(base.sm.grown.healthy) ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.Play(this.GetAnimationForState(this.GetCurrentState()), mode, 1f, 0f);
			}
			if (base.IsInsideState(base.smi.sm.grown.healthy))
			{
				this.ActivateGlowFlowerMeter();
				return;
			}
			this.DeactivateGlowFlowerMeter();
		}

		// Token: 0x06008566 RID: 34150 RVA: 0x003476F7 File Offset: 0x003458F7
		public HashedString GetCurrentTrunkAnim()
		{
			if (this.trunk != null && this.trunkAnimController != null)
			{
				return this.trunkAnimController.currentAnim;
			}
			return null;
		}

		// Token: 0x06008567 RID: 34151 RVA: 0x00347724 File Offset: 0x00345924
		public void SynchCurrentAnimWithTrunkAnim(HashedString trunkAnimNameToSynchTo)
		{
			if (this.trunk != null && this.trunkAnimController != null && this.trunkAnimController.currentAnim == trunkAnimNameToSynchTo)
			{
				float elapsedTime = this.trunkAnimController.GetElapsedTime();
				base.smi.animController.SetElapsedTime(elapsedTime);
			}
		}

		// Token: 0x06008568 RID: 34152 RVA: 0x00347778 File Offset: 0x00345978
		public string GetAnimationForState(StateMachine.BaseState state)
		{
			if (state == base.sm.growing.wild.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.growing.planted.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.growing.wild.hidden)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.growing.planted.hidden)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.grown.spawn)
			{
				return this.Animations.spawn;
			}
			if (state == base.sm.grown.spawnPST)
			{
				return this.Animations.spawn_pst;
			}
			if (state == base.sm.grown.healthy.filling)
			{
				return this.Animations.fill;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.idle)
			{
				return this.Animations.ready_harvest;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre)
			{
				return this.Animations.manual_harvest_pre;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop)
			{
				return this.Animations.manual_harvest_loop;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst)
			{
				return this.Animations.manual_harvest_pst;
			}
			if (state == base.sm.grown.trunkWilted)
			{
				return this.Animations.wilted;
			}
			if (state == base.sm.halt.wilted)
			{
				return this.Animations.wilted_short_trunk_healthy;
			}
			if (state == base.sm.halt.trunkWilted)
			{
				return this.Animations.wilted_short_trunk_wilted;
			}
			if (state == base.sm.halt.shaking)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.halt.hidden)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.harvestedForWood)
			{
				return this.Animations.die;
			}
			if (state == base.sm.die.entering)
			{
				return this.Animations.die;
			}
			return this.Animations.spawn;
		}

		// Token: 0x06008569 RID: 34153 RVA: 0x00347A0C File Offset: 0x00345C0C
		public string GetFillAnimNameForState(StateMachine.BaseState state)
		{
			string fill = this.Animations.fill;
			if (state == base.sm.grown.healthy.filling)
			{
				return this.Animations.fill;
			}
			if (state == base.sm.growing.wild.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.growing.planted.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.halt.wilted)
			{
				return this.Animations.wilted_short_trunk_healthy;
			}
			return fill;
		}

		// Token: 0x0600856A RID: 34154 RVA: 0x00347AB1 File Offset: 0x00345CB1
		public void PlayReadyForHarvestAnimation()
		{
			if (this.animController != null)
			{
				this.animController.Play(this.Animations.ready_harvest, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x0600856B RID: 34155 RVA: 0x00347AE7 File Offset: 0x00345CE7
		public void PlayFillAnimation()
		{
			this.PlayFillAnimation(this.lastFillAmountRecorded);
		}

		// Token: 0x0600856C RID: 34156 RVA: 0x00347AF8 File Offset: 0x00345CF8
		public void PlayFillAnimation(float fillLevel)
		{
			string fillAnimNameForState = this.GetFillAnimNameForState(base.smi.GetCurrentState());
			this.lastFillAmountRecorded = fillLevel;
			if (this.entombDefenseSMI.IsEntombed && this.entombDefenseSMI.IsActive)
			{
				return;
			}
			if (this.animController != null)
			{
				int num = Mathf.FloorToInt(fillLevel * 42f);
				if (this.animController.currentAnim != fillAnimNameForState)
				{
					this.animController.Play(fillAnimNameForState, KAnim.PlayMode.Once, 0f, 0f);
				}
				if (this.animController.currentFrame != num)
				{
					this.animController.SetPositionPercent(fillLevel);
				}
			}
		}

		// Token: 0x0600856D RID: 34157 RVA: 0x00347BA4 File Offset: 0x00345DA4
		public void ActivateGlowFlowerMeter()
		{
			if (this.flowerMeters != null)
			{
				for (int i = 0; i < this.flowerMeters.Length; i++)
				{
					this.flowerMeters[i].gameObject.SetActive(true);
					this.flowerMeters[i].meterController.Play(this.flowerMeters[i].meterController.currentAnim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
		}

		// Token: 0x0600856E RID: 34158 RVA: 0x00347C10 File Offset: 0x00345E10
		public void PlayAnimOnFlower(string[] animNames, KAnim.PlayMode playMode)
		{
			if (this.flowerMeters != null)
			{
				for (int i = 0; i < this.flowerMeters.Length; i++)
				{
					this.flowerMeters[i].meterController.Play(animNames[i], playMode, 1f, 0f);
				}
			}
		}

		// Token: 0x0600856F RID: 34159 RVA: 0x00347C60 File Offset: 0x00345E60
		public void DeactivateGlowFlowerMeter()
		{
			if (this.flowerMeters != null)
			{
				for (int i = 0; i < this.flowerMeters.Length; i++)
				{
					this.flowerMeters[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06008570 RID: 34160 RVA: 0x00347C9B File Offset: 0x00345E9B
		public float TimeUntilNextHarvest()
		{
			return (this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta();
		}

		// Token: 0x06008571 RID: 34161 RVA: 0x00347CC0 File Offset: 0x00345EC0
		public float PercentGrown()
		{
			return this.GetcurrentGrowthPercentage();
		}

		// Token: 0x06008572 RID: 34162 RVA: 0x00347CC8 File Offset: 0x00345EC8
		public Crop GetCropComponent()
		{
			return base.GetComponent<Crop>();
		}

		// Token: 0x06008573 RID: 34163 RVA: 0x00347CD0 File Offset: 0x00345ED0
		public float DomesticGrowthTime()
		{
			return this.maturity.GetMax() / base.smi.baseGrowingRate.Value;
		}

		// Token: 0x06008574 RID: 34164 RVA: 0x00347CEE File Offset: 0x00345EEE
		public float WildGrowthTime()
		{
			return this.maturity.GetMax() / base.smi.wildGrowingRate.Value;
		}

		// Token: 0x06008575 RID: 34165 RVA: 0x00347D0C File Offset: 0x00345F0C
		public bool IsWildPlanted()
		{
			return this.IsTrunkWildPlanted;
		}

		// Token: 0x040065C0 RID: 26048
		[MyCmpGet]
		public WiltCondition wiltCondition;

		// Token: 0x040065C1 RID: 26049
		[MyCmpGet]
		public Crop crop;

		// Token: 0x040065C2 RID: 26050
		[MyCmpGet]
		public Harvestable harvestable;

		// Token: 0x040065C3 RID: 26051
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x040065C4 RID: 26052
		public SpaceTreeBranch.AnimSet Animations = new SpaceTreeBranch.AnimSet();

		// Token: 0x040065C5 RID: 26053
		private int cell;

		// Token: 0x040065C6 RID: 26054
		private float lastFillAmountRecorded;

		// Token: 0x040065C7 RID: 26055
		private AmountInstance maturity;

		// Token: 0x040065C8 RID: 26056
		public AttributeModifier baseGrowingRate;

		// Token: 0x040065C9 RID: 26057
		public AttributeModifier wildGrowingRate;

		// Token: 0x040065CA RID: 26058
		public UnstableEntombDefense.Instance entombDefenseSMI;

		// Token: 0x040065CB RID: 26059
		private MeterController[] flowerMeters;

		// Token: 0x040065CC RID: 26060
		private PlantBranch.Instance branch;

		// Token: 0x040065CD RID: 26061
		private KBatchedAnimController trunkAnimController;

		// Token: 0x040065CE RID: 26062
		private PlantBranchGrower.Instance _trunk;
	}
}
