using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000AAD RID: 2733
public class VineBranch : PlantBranchGrowerBase<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>
{
	// Token: 0x06004F2A RID: 20266 RVA: 0x001CBB2F File Offset: 0x001C9D2F
	public static bool IsCellFoundation(int cell)
	{
		return Grid.IsSolidCell(cell) || Grid.HasDoor[cell];
	}

	// Token: 0x06004F2B RID: 20267 RVA: 0x001CBB48 File Offset: 0x001C9D48
	public static bool IsCellAvailable(GameObject questionerObj, int cell, Func<int, bool> foundationCheckFunction = null)
	{
		int num = Grid.PosToCell(questionerObj);
		int num2 = (int)Grid.WorldIdx[num];
		return cell != Grid.InvalidCell && (int)Grid.WorldIdx[cell] == num2 && ((foundationCheckFunction == null) ? (!VineBranch.IsCellFoundation(cell)) : (!foundationCheckFunction(cell))) && !Grid.IsLiquid(cell) && Grid.Objects[cell, 1] == null && Grid.Objects[cell, 5] == null;
	}

	// Token: 0x06004F2C RID: 20268 RVA: 0x001CBBC0 File Offset: 0x001C9DC0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.undevelopedBranch;
		this.undevelopedBranch.InitializeStates(this.masterTarget, this.Mother, this.dead, this.DieSignal).ParamTransition<bool>(this.MarkedForDeath, this.dead, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsTrue).ParamTransition<GameObject>(this.Mother, this.dead, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsNull).EventTransition(GameHashes.Grow, this.mature, (VineBranch.Instance smi) => smi.IsGrown).UpdateTransition(this.mature, (VineBranch.Instance smi, float dt) => smi.IsGrown, UpdateRate.SIM_4000ms, false).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RecalculateMyShape)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.SubscribreSurroundingCellChangeListeners)).Exit(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.UnSubscribreSurroundingSolidChangesListeners)).DefaultState(this.undevelopedBranch.growing);
		this.undevelopedBranch.wilted.PlayAnim(new Func<VineBranch.Instance, string>(VineBranch.GetWiltAnim), KAnim.PlayMode.Loop).EventHandler(GameHashes.BranchShapeChanged, delegate(VineBranch.Instance smi)
		{
			VineBranch.RefreshAnim(smi, VineBranch.GetWiltAnim(smi), KAnim.PlayMode.Loop);
		}).EventTransition(GameHashes.WiltRecover, this.undevelopedBranch.growing, (VineBranch.Instance smi) => !smi.IsWilting);
		this.undevelopedBranch.growing.EventTransition(GameHashes.Wilt, this.undevelopedBranch.wilted, (VineBranch.Instance smi) => smi.IsWilting).PlayAnim((VineBranch.Instance smi) => smi.Anims.grow, KAnim.PlayMode.Paused).EventHandler(GameHashes.BranchShapeChanged, delegate(VineBranch.Instance smi)
		{
			VineBranch.RefreshAnim(smi, smi.Anims.grow, KAnim.PlayMode.Paused);
		}).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, (VineBranch.Instance smi) => smi).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RefreshPositionPercent)).Update(new Action<VineBranch.Instance, float>(VineBranch.RefreshPositionPercent), UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.ConsumePlant, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RefreshPositionPercent)).DefaultState(this.undevelopedBranch.growing.wild);
		this.undevelopedBranch.growing.wild.ParamTransition<bool>(this.WildPlanted, this.undevelopedBranch.growing.domestic, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsFalse).ToggleAttributeModifier("Growing", (VineBranch.Instance smi) => smi.wildGrowingRate, null);
		this.undevelopedBranch.growing.domestic.ParamTransition<bool>(this.WildPlanted, this.undevelopedBranch.growing.wild, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsTrue).ToggleAttributeModifier("Growing", (VineBranch.Instance smi) => smi.baseGrowingRate, null);
		this.mature.InitializeStates(this.masterTarget, this.Mother, this.dead, this.DieSignal).ParamTransition<bool>(this.MarkedForDeath, this.dead, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsTrue).ParamTransition<GameObject>(this.Mother, this.dead, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsNull).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RecalculateShapeAndSpawnBranchesIfSpawnedByDiscovery)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.SetupFruitMeter)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.SubscribreSurroundingCellChangeListeners)).Exit(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.UnSubscribreSurroundingSolidChangesListeners)).Update(new Action<VineBranch.Instance, float>(VineBranch.SpawnBranchIfPossible), UpdateRate.SIM_4000ms, false).DefaultState(this.mature.healthy);
		this.mature.healthy.PlayAnim((VineBranch.Instance smi) => smi.Anims.idle, KAnim.PlayMode.Loop).EventHandler(GameHashes.BranchShapeChanged, delegate(VineBranch.Instance smi)
		{
			VineBranch.RefreshAnim(smi, smi.Anims.idle, KAnim.PlayMode.Loop);
		}).DefaultState(this.mature.healthy.growing);
		this.mature.healthy.growing.EventTransition(GameHashes.Grow, this.mature.healthy.harvestReady, (VineBranch.Instance smi) => smi.IsReadyForHarvest).UpdateTransition(this.mature.healthy.harvestReady, (VineBranch.Instance smi, float dt) => smi.IsReadyForHarvest, UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.Wilt, this.mature.wilted, (VineBranch.Instance smi) => smi.IsWilting).EventHandler(GameHashes.BranchShapeChanged, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RecreateFruitMeter)).EventHandler(GameHashes.BranchShapeChanged, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.UpdateFruitMeterGrowAnimations)).ToggleStatusItem(Db.Get().CreatureStatusItems.GrowingFruit, (VineBranch.Instance smi) => smi).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.UpdateFruitMeterGrowAnimations)).Update(new Action<VineBranch.Instance, float>(VineBranch.UpdateFruitMeterGrowAnimations), UpdateRate.SIM_200ms, false).DefaultState(this.mature.healthy.growing.wild);
		this.mature.healthy.growing.wild.ParamTransition<bool>(this.WildPlanted, this.mature.healthy.growing.domestic, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsFalse).ToggleAttributeModifier("Fruit Growing", (VineBranch.Instance smi) => smi.wildFruitGrowingRate, null);
		this.mature.healthy.growing.domestic.ParamTransition<bool>(this.WildPlanted, this.mature.healthy.growing.wild, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IsTrue).ToggleAttributeModifier("Fruit Growing", (VineBranch.Instance smi) => smi.baseFruitGrowingRate, null);
		this.mature.healthy.harvestReady.ToggleTag(GameTags.FullyGrown).EventTransition(GameHashes.Harvest, this.mature.healthy.harvest, null).EventHandler(GameHashes.BranchShapeChanged, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RecreateFruitMeter)).EventHandler(GameHashes.BranchShapeChanged, delegate(VineBranch.Instance smi)
		{
			VineBranch.PlayAnimsOnFruit(smi, smi.Anims.meter_harvest_ready, KAnim.PlayMode.Loop);
		}).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.MakeItHarvestable)).Exit(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.ResetOldAge)).ToggleAttributeModifier("GetOld", (VineBranch.Instance smi) => smi.getOldRate, null).Enter(delegate(VineBranch.Instance smi)
		{
			VineBranch.PlayAnimsOnFruit(smi, smi.Anims.meter_harvest_ready, KAnim.PlayMode.Loop);
		}).UpdateTransition(this.mature.healthy.selfHarvestFromOld, new Func<VineBranch.Instance, float, bool>(VineBranch.ShouldSelfHarvestFromOldAge), UpdateRate.SIM_4000ms, false);
		this.mature.healthy.harvest.Target(this.Fruit).OnAnimQueueComplete(this.mature.healthy.growing).Target(this.masterTarget).Enter(delegate(VineBranch.Instance smi)
		{
			VineBranch.PlayAnimsOnFruit(smi, smi.Anims.meter_harvest, KAnim.PlayMode.Once);
		}).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.MakeItNotHarvestable)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.ResetFruitGrowProgress)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.SpawnHarvestedFruit)).TriggerOnExit(GameHashes.HarvestComplete, null).ScheduleGoTo(3f, this.mature.healthy.growing);
		this.mature.healthy.selfHarvestFromOld.Target(this.Fruit).OnAnimQueueComplete(this.mature.healthy.growing).Target(this.masterTarget).Enter(delegate(VineBranch.Instance smi)
		{
			VineBranch.PlayAnimsOnFruit(smi, smi.Anims.meter_harvest, KAnim.PlayMode.Once);
		}).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.ForceCancelHarvest)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.MakeItNotHarvestable)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.ResetOldAge)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.ResetFruitGrowProgress)).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.SpawnHarvestedFruit)).TriggerOnExit(GameHashes.HarvestComplete, null).ScheduleGoTo(3f, this.mature.healthy.growing);
		this.mature.wilted.PlayAnim(new Func<VineBranch.Instance, string>(VineBranch.GetWiltAnim), KAnim.PlayMode.Loop).Enter(delegate(VineBranch.Instance smi)
		{
			VineBranch.PlayAnimsOnFruit(smi, VineBranch.GetFruitWiltAnim(smi), KAnim.PlayMode.Loop);
		}).EventHandler(GameHashes.BranchShapeChanged, delegate(VineBranch.Instance smi)
		{
			VineBranch.RefreshAnim(smi, VineBranch.GetWiltAnim(smi), KAnim.PlayMode.Loop);
		}).EventHandler(GameHashes.BranchShapeChanged, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.RecreateFruitMeter)).EventHandler(GameHashes.BranchShapeChanged, delegate(VineBranch.Instance smi)
		{
			VineBranch.PlayAnimsOnFruit(smi, smi.Anims.meter_wilted, KAnim.PlayMode.Loop);
		}).EventTransition(GameHashes.WiltRecover, this.mature.healthy, (VineBranch.Instance smi) => !smi.IsWilting).EventTransition(GameHashes.Harvest, this.mature.healthy.harvest, null);
		this.dead.Target(this.masterTarget).ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.HarvestOnDeath)).Enter(delegate(VineBranch.Instance smi)
		{
			if (!smi.gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted) && !smi.IsWild)
			{
				Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
				Notification notification = VineBranch.CreateDeathNotification(smi);
				notifier.Add(notification, "");
			}
			GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
			smi.Trigger(1623392196, null);
			smi.DestroySelf(null);
		});
	}

	// Token: 0x06004F2D RID: 20269 RVA: 0x001CC63B File Offset: 0x001CA83B
	private static bool ShouldSelfHarvestFromOldAge(VineBranch.Instance smi, float dt)
	{
		return smi.IsOld;
	}

	// Token: 0x06004F2E RID: 20270 RVA: 0x001CC643 File Offset: 0x001CA843
	private static string GetWiltAnim(VineBranch.Instance smi)
	{
		return smi.Anims.GetBaseWiltAnim(smi.Anims.GetWiltLevel(smi.GrowthPercentage));
	}

	// Token: 0x06004F2F RID: 20271 RVA: 0x001CC661 File Offset: 0x001CA861
	private static string GetFruitWiltAnim(VineBranch.Instance smi)
	{
		return smi.Anims.GetMeterWiltAnim(smi.Anims.GetWiltLevel(smi.FruitGrowthPercentage));
	}

	// Token: 0x06004F30 RID: 20272 RVA: 0x001CC67F File Offset: 0x001CA87F
	private static void PlayAnimsOnFruit(VineBranch.Instance smi, string animName, KAnim.PlayMode playmode)
	{
		smi.PlayAnimOnFruitMeter(animName, playmode);
	}

	// Token: 0x06004F31 RID: 20273 RVA: 0x001CC689 File Offset: 0x001CA889
	private static void UpdateFruitMeterGrowAnimations(VineBranch.Instance smi, float dt)
	{
		VineBranch.UpdateFruitMeterGrowAnimations(smi);
	}

	// Token: 0x06004F32 RID: 20274 RVA: 0x001CC691 File Offset: 0x001CA891
	private static void UpdateFruitMeterGrowAnimations(VineBranch.Instance smi)
	{
		smi.UpdateFruitGrowMeterPosition();
	}

	// Token: 0x06004F33 RID: 20275 RVA: 0x001CC699 File Offset: 0x001CA899
	private static void RecreateFruitMeter(VineBranch.Instance smi)
	{
		smi.CreateFruitMeter();
	}

	// Token: 0x06004F34 RID: 20276 RVA: 0x001CC6A1 File Offset: 0x001CA8A1
	private static void SetupFruitMeter(VineBranch.Instance smi)
	{
		smi.CreateFruitMeter();
	}

	// Token: 0x06004F35 RID: 20277 RVA: 0x001CC6A9 File Offset: 0x001CA8A9
	private static void SpawnBranchIfPossible(VineBranch.Instance smi, float dt)
	{
		smi.AttemptToSpawnBranch();
	}

	// Token: 0x06004F36 RID: 20278 RVA: 0x001CC6B1 File Offset: 0x001CA8B1
	private static void MakeItHarvestable(VineBranch.Instance smi)
	{
		smi.SetHarvestableState(true);
	}

	// Token: 0x06004F37 RID: 20279 RVA: 0x001CC6BA File Offset: 0x001CA8BA
	private static void ForceCancelHarvest(VineBranch.Instance smi)
	{
		smi.ForceCancelHarvest();
	}

	// Token: 0x06004F38 RID: 20280 RVA: 0x001CC6C2 File Offset: 0x001CA8C2
	private static void MakeItNotHarvestable(VineBranch.Instance smi)
	{
		smi.SetHarvestableState(false);
	}

	// Token: 0x06004F39 RID: 20281 RVA: 0x001CC6CB File Offset: 0x001CA8CB
	private static void RefreshPositionPercent(VineBranch.Instance smi, float dt)
	{
		VineBranch.RefreshPositionPercent(smi);
	}

	// Token: 0x06004F3A RID: 20282 RVA: 0x001CC6D3 File Offset: 0x001CA8D3
	private static void RefreshPositionPercent(VineBranch.Instance smi)
	{
		smi.AnimController.SetPositionPercent(smi.GrowthPercentage);
	}

	// Token: 0x06004F3B RID: 20283 RVA: 0x001CC6E6 File Offset: 0x001CA8E6
	private static void SubscribreSurroundingCellChangeListeners(VineBranch.Instance smi)
	{
		smi.SubscribeSurroundingSolidChangesListeners();
	}

	// Token: 0x06004F3C RID: 20284 RVA: 0x001CC6EE File Offset: 0x001CA8EE
	private static void UnSubscribreSurroundingSolidChangesListeners(VineBranch.Instance smi)
	{
		smi.UnSubscribreSurroundingSolidChangesListeners();
	}

	// Token: 0x06004F3D RID: 20285 RVA: 0x001CC6F6 File Offset: 0x001CA8F6
	private static void ResetFruitGrowProgress(VineBranch.Instance smi)
	{
		smi.ResetFruitGrowProgress();
	}

	// Token: 0x06004F3E RID: 20286 RVA: 0x001CC6FE File Offset: 0x001CA8FE
	private static void ResetOldAge(VineBranch.Instance smi)
	{
		smi.ResetOldAge();
	}

	// Token: 0x06004F3F RID: 20287 RVA: 0x001CC706 File Offset: 0x001CA906
	private static void SpawnHarvestedFruit(VineBranch.Instance smi)
	{
		smi.SpawnHarvestedFruit();
	}

	// Token: 0x06004F40 RID: 20288 RVA: 0x001CC70E File Offset: 0x001CA90E
	private static void RecalculateMyShape(VineBranch.Instance smi)
	{
		smi.RecalculateMyShape();
	}

	// Token: 0x06004F41 RID: 20289 RVA: 0x001CC716 File Offset: 0x001CA916
	private static void OnMotherRecovered(VineBranch.Instance smi)
	{
		smi.BoxingTrigger(912965142, true);
	}

	// Token: 0x06004F42 RID: 20290 RVA: 0x001CC724 File Offset: 0x001CA924
	private static void OnMotherWilted(VineBranch.Instance smi)
	{
		smi.BoxingTrigger(912965142, false);
	}

	// Token: 0x06004F43 RID: 20291 RVA: 0x001CC732 File Offset: 0x001CA932
	private static void RecalculateShapeAndSpawnBranchesIfSpawnedByDiscovery(VineBranch.Instance smi)
	{
		if (smi.IsNewGameSpawned)
		{
			smi.RecalculateMyShape();
			VineBranch.SpawnBranchIfPossible(smi, 0f);
		}
	}

	// Token: 0x06004F44 RID: 20292 RVA: 0x001CC74D File Offset: 0x001CA94D
	public static void HarvestOnDeath(VineBranch.Instance smi)
	{
		if (smi.IsReadyForHarvest)
		{
			VineBranch.SpawnHarvestedFruit(smi);
		}
	}

	// Token: 0x06004F45 RID: 20293 RVA: 0x001CC760 File Offset: 0x001CA960
	private static void RefreshAnim(VineBranch.Instance smi, string animName, KAnim.PlayMode playmode)
	{
		float elapsedTime = smi.AnimController.GetElapsedTime();
		smi.AnimController.Play(animName, playmode, 1f, 0f);
		smi.AnimController.SetElapsedTime(elapsedTime);
	}

	// Token: 0x06004F46 RID: 20294 RVA: 0x001CC7A4 File Offset: 0x001CA9A4
	private static Notification CreateDeathNotification(VineBranch.Instance smi)
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + smi.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06004F48 RID: 20296 RVA: 0x001CC80C File Offset: 0x001CAA0C
	// Note: this type is marked as 'beforefieldinit'.
	static VineBranch()
	{
		Dictionary<VineBranch.Shape, VineBranch.ShapeCategory> dictionary = new Dictionary<VineBranch.Shape, VineBranch.ShapeCategory>();
		dictionary[VineBranch.Shape.Top] = VineBranch.ShapeCategory.Line;
		dictionary[VineBranch.Shape.Bottom] = VineBranch.ShapeCategory.Line;
		dictionary[VineBranch.Shape.Left] = VineBranch.ShapeCategory.Line;
		dictionary[VineBranch.Shape.Right] = VineBranch.ShapeCategory.Line;
		dictionary[VineBranch.Shape.InCornerTopLeft] = VineBranch.ShapeCategory.InCorner;
		dictionary[VineBranch.Shape.InCornerTopRight] = VineBranch.ShapeCategory.InCorner;
		dictionary[VineBranch.Shape.InCornerBottomLeft] = VineBranch.ShapeCategory.InCorner;
		dictionary[VineBranch.Shape.InCornerBottomRight] = VineBranch.ShapeCategory.InCorner;
		dictionary[VineBranch.Shape.OutCornerTopLeft] = VineBranch.ShapeCategory.OutCorner;
		dictionary[VineBranch.Shape.OutCornerTopRight] = VineBranch.ShapeCategory.OutCorner;
		dictionary[VineBranch.Shape.OutCornerBottomLeft] = VineBranch.ShapeCategory.OutCorner;
		dictionary[VineBranch.Shape.OutCornerBottomRight] = VineBranch.ShapeCategory.OutCorner;
		dictionary[VineBranch.Shape.TopEnd] = VineBranch.ShapeCategory.DeadEnd;
		dictionary[VineBranch.Shape.BottomEnd] = VineBranch.ShapeCategory.DeadEnd;
		dictionary[VineBranch.Shape.LeftEnd] = VineBranch.ShapeCategory.DeadEnd;
		dictionary[VineBranch.Shape.RightEnd] = VineBranch.ShapeCategory.DeadEnd;
		VineBranch.GetShapeCategory = dictionary;
		Dictionary<VineBranch.ShapeCategory, VineBranch.AnimSet> dictionary2 = new Dictionary<VineBranch.ShapeCategory, VineBranch.AnimSet>();
		dictionary2[VineBranch.ShapeCategory.Line] = new VineBranch.AnimSet("line_");
		dictionary2[VineBranch.ShapeCategory.InCorner] = new VineBranch.AnimSet("incorner_");
		dictionary2[VineBranch.ShapeCategory.OutCorner] = new VineBranch.AnimSet("outcorner_");
		dictionary2[VineBranch.ShapeCategory.DeadEnd] = new VineBranch.AnimSet("end_");
		VineBranch.GetAnimSetByShapeCategory = dictionary2;
	}

	// Token: 0x040034EC RID: 13548
	private static Dictionary<VineBranch.Shape, VineBranch.ShapeCategory> GetShapeCategory;

	// Token: 0x040034ED RID: 13549
	private static Dictionary<VineBranch.ShapeCategory, VineBranch.AnimSet> GetAnimSetByShapeCategory;

	// Token: 0x040034EE RID: 13550
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.TargetParameter Fruit;

	// Token: 0x040034EF RID: 13551
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.TargetParameter Mother;

	// Token: 0x040034F0 RID: 13552
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.TargetParameter Branch;

	// Token: 0x040034F1 RID: 13553
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IntParameter BranchNumber;

	// Token: 0x040034F2 RID: 13554
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IntParameter BranchShape;

	// Token: 0x040034F3 RID: 13555
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.BoolParameter GrowingClockwise;

	// Token: 0x040034F4 RID: 13556
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IntParameter RootShape;

	// Token: 0x040034F5 RID: 13557
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.IntParameter RootDirection;

	// Token: 0x040034F6 RID: 13558
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.BoolParameter WildPlanted;

	// Token: 0x040034F7 RID: 13559
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.BoolParameter MarkedForDeath;

	// Token: 0x040034F8 RID: 13560
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Signal DieSignal;

	// Token: 0x040034F9 RID: 13561
	public StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Signal OnShapeChangedSignal;

	// Token: 0x040034FA RID: 13562
	public VineBranch.GrowingStates undevelopedBranch;

	// Token: 0x040034FB RID: 13563
	public VineBranch.GrownStates mature;

	// Token: 0x040034FC RID: 13564
	public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State dead;

	// Token: 0x02001BE6 RID: 7142
	public class AnimSet
	{
		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x0600AB91 RID: 43921 RVA: 0x003C92EF File Offset: 0x003C74EF
		public string pre_grow
		{
			get
			{
				return this.suffix + "pre_grow";
			}
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x0600AB92 RID: 43922 RVA: 0x003C9301 File Offset: 0x003C7501
		public string grow
		{
			get
			{
				return this.suffix + "grow";
			}
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x0600AB93 RID: 43923 RVA: 0x003C9313 File Offset: 0x003C7513
		public string grow_pst
		{
			get
			{
				return this.suffix + "grow_pst";
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x0600AB94 RID: 43924 RVA: 0x003C9325 File Offset: 0x003C7525
		public string idle
		{
			get
			{
				return this.suffix + "idle";
			}
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x0600AB95 RID: 43925 RVA: 0x003C9337 File Offset: 0x003C7537
		public string meter_target
		{
			get
			{
				return this.suffix + "meter_target";
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x0600AB96 RID: 43926 RVA: 0x003C9349 File Offset: 0x003C7549
		public string meter
		{
			get
			{
				return this.suffix + "meter";
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x0600AB97 RID: 43927 RVA: 0x003C935B File Offset: 0x003C755B
		public string meter_wilted
		{
			get
			{
				return this.suffix + "meter_wilted";
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x0600AB98 RID: 43928 RVA: 0x003C936D File Offset: 0x003C756D
		public string meter_harvest
		{
			get
			{
				return this.suffix + "meter_harvest";
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x0600AB99 RID: 43929 RVA: 0x003C937F File Offset: 0x003C757F
		public string meter_harvest_ready
		{
			get
			{
				return this.suffix + "meter_harvest_ready";
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x0600AB9A RID: 43930 RVA: 0x003C9391 File Offset: 0x003C7591
		private string wilted
		{
			get
			{
				return this.suffix + "wilted";
			}
		}

		// Token: 0x0600AB9B RID: 43931 RVA: 0x003C93A4 File Offset: 0x003C75A4
		public int GetWiltLevel(float growthPercentage)
		{
			int result;
			if (growthPercentage < 0.75f)
			{
				result = 1;
			}
			else if (growthPercentage < 1f)
			{
				result = 2;
			}
			else
			{
				result = 3;
			}
			return result;
		}

		// Token: 0x0600AB9C RID: 43932 RVA: 0x003C93CC File Offset: 0x003C75CC
		public string GetBaseWiltAnim(int level)
		{
			return this.GetWiltAnim(this.wilted, level);
		}

		// Token: 0x0600AB9D RID: 43933 RVA: 0x003C93DB File Offset: 0x003C75DB
		public string GetMeterWiltAnim(int level)
		{
			return this.GetWiltAnim(this.meter_wilted, level);
		}

		// Token: 0x0600AB9E RID: 43934 RVA: 0x003C93EA File Offset: 0x003C75EA
		private string GetWiltAnim(string wiltName, int level)
		{
			return wiltName + level.ToString();
		}

		// Token: 0x0600AB9F RID: 43935 RVA: 0x003C93F9 File Offset: 0x003C75F9
		public AnimSet(string suffix)
		{
			this.suffix = suffix;
		}

		// Token: 0x0400860F RID: 34319
		public string suffix;

		// Token: 0x04008610 RID: 34320
		private const int WILT_LEVELS = 3;
	}

	// Token: 0x02001BE7 RID: 7143
	public enum ShapeCategory
	{
		// Token: 0x04008612 RID: 34322
		Line,
		// Token: 0x04008613 RID: 34323
		InCorner,
		// Token: 0x04008614 RID: 34324
		OutCorner,
		// Token: 0x04008615 RID: 34325
		DeadEnd
	}

	// Token: 0x02001BE8 RID: 7144
	public enum Shape
	{
		// Token: 0x04008617 RID: 34327
		Top,
		// Token: 0x04008618 RID: 34328
		Bottom,
		// Token: 0x04008619 RID: 34329
		Left,
		// Token: 0x0400861A RID: 34330
		Right,
		// Token: 0x0400861B RID: 34331
		InCornerTopLeft,
		// Token: 0x0400861C RID: 34332
		InCornerTopRight,
		// Token: 0x0400861D RID: 34333
		InCornerBottomLeft,
		// Token: 0x0400861E RID: 34334
		InCornerBottomRight,
		// Token: 0x0400861F RID: 34335
		OutCornerTopLeft,
		// Token: 0x04008620 RID: 34336
		OutCornerTopRight,
		// Token: 0x04008621 RID: 34337
		OutCornerBottomLeft,
		// Token: 0x04008622 RID: 34338
		OutCornerBottomRight,
		// Token: 0x04008623 RID: 34339
		TopEnd,
		// Token: 0x04008624 RID: 34340
		BottomEnd,
		// Token: 0x04008625 RID: 34341
		LeftEnd,
		// Token: 0x04008626 RID: 34342
		RightEnd
	}

	// Token: 0x02001BE9 RID: 7145
	public class Def : PlantBranchGrowerBase<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.PlantBranchGrowerBaseDef
	{
		// Token: 0x04008627 RID: 34343
		public float GROWTH_RATE = 0.0016666667f;

		// Token: 0x04008628 RID: 34344
		public float WILD_GROWTH_RATE = 0.00041666668f;
	}

	// Token: 0x02001BEA RID: 7146
	public class GrowingSpeedState : GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State
	{
		// Token: 0x04008629 RID: 34345
		public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State wild;

		// Token: 0x0400862A RID: 34346
		public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State domestic;
	}

	// Token: 0x02001BEB RID: 7147
	public class BranchAliveSubstate : GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.PlantAliveSubState
	{
		// Token: 0x0600ABA2 RID: 43938 RVA: 0x003C9430 File Offset: 0x003C7630
		public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State InitializeStates(StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.TargetParameter plant, StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.TargetParameter mother, GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State death_state, StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.Signal dieSignal)
		{
			base.InitializeStates(plant, death_state);
			base.root.Target(plant).OnSignal(dieSignal, death_state).OnTargetLost(mother, death_state).Target(mother).EventHandler(GameHashes.Wilt, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.OnMotherWilted)).EventHandler(GameHashes.WiltRecover, new StateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State.Callback(VineBranch.OnMotherRecovered)).Target(plant);
			return this;
		}
	}

	// Token: 0x02001BEC RID: 7148
	public class GrowingStates : VineBranch.BranchAliveSubstate
	{
		// Token: 0x0400862B RID: 34347
		public VineBranch.GrowingSpeedState growing;

		// Token: 0x0400862C RID: 34348
		public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State wilted;
	}

	// Token: 0x02001BED RID: 7149
	public class FruitGrowingStates : GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State
	{
		// Token: 0x0400862D RID: 34349
		public VineBranch.GrowingSpeedState growing;

		// Token: 0x0400862E RID: 34350
		public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State wilted;

		// Token: 0x0400862F RID: 34351
		public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State harvestReady;

		// Token: 0x04008630 RID: 34352
		public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State selfHarvestFromOld;

		// Token: 0x04008631 RID: 34353
		public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State harvest;
	}

	// Token: 0x02001BEE RID: 7150
	public class GrownStates : VineBranch.BranchAliveSubstate
	{
		// Token: 0x04008632 RID: 34354
		public VineBranch.FruitGrowingStates healthy;

		// Token: 0x04008633 RID: 34355
		public GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.State wilted;
	}

	// Token: 0x02001BEF RID: 7151
	public new class Instance : GameStateMachine<VineBranch, VineBranch.Instance, IStateMachineTarget, VineBranch.Def>.GameInstance, IManageGrowingStates, IWiltCause
	{
		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x0600ABA7 RID: 43943 RVA: 0x003C94BB File Offset: 0x003C76BB
		public GameObject Mother
		{
			get
			{
				return base.sm.Mother.Get(this);
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x0600ABA8 RID: 43944 RVA: 0x003C94CE File Offset: 0x003C76CE
		public GameObject Branch
		{
			get
			{
				return base.sm.Branch.Get(this);
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x0600ABA9 RID: 43945 RVA: 0x003C94E1 File Offset: 0x003C76E1
		public VineBranch.Instance BranchSMI
		{
			get
			{
				if (!(this.Branch == null))
				{
					return this.Branch.GetSMI<VineBranch.Instance>();
				}
				return null;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x0600ABAA RID: 43946 RVA: 0x003C94FE File Offset: 0x003C76FE
		public int MyBranchNumber
		{
			get
			{
				return base.sm.BranchNumber.Get(this);
			}
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x0600ABAB RID: 43947 RVA: 0x003C9511 File Offset: 0x003C7711
		public bool IsGrowingClockwise
		{
			get
			{
				return base.sm.GrowingClockwise.Get(this);
			}
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x0600ABAC RID: 43948 RVA: 0x003C9524 File Offset: 0x003C7724
		public bool IsWild
		{
			get
			{
				return base.sm.WildPlanted.Get(this);
			}
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x0600ABAD RID: 43949 RVA: 0x003C9537 File Offset: 0x003C7737
		public bool MaxBranchNumberReached
		{
			get
			{
				return this.MyBranchNumber >= 12;
			}
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x0600ABAE RID: 43950 RVA: 0x003C9546 File Offset: 0x003C7746
		public bool CanChangeShape
		{
			get
			{
				return !this.isSpawningNextBranch && this.Branch == null && !this.MaxBranchNumberReached;
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x0600ABAF RID: 43951 RVA: 0x003C9569 File Offset: 0x003C7769
		public VineBranch.Shape MyShape
		{
			get
			{
				return (VineBranch.Shape)base.sm.BranchShape.Get(this);
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x0600ABB0 RID: 43952 RVA: 0x003C957C File Offset: 0x003C777C
		public VineBranch.ShapeCategory MyShapeCategory
		{
			get
			{
				return VineBranch.GetShapeCategory[this.MyShape];
			}
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x0600ABB1 RID: 43953 RVA: 0x003C958E File Offset: 0x003C778E
		public VineBranch.Shape RootShape
		{
			get
			{
				return (VineBranch.Shape)base.sm.RootShape.Get(this);
			}
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x0600ABB2 RID: 43954 RVA: 0x003C95A1 File Offset: 0x003C77A1
		public Direction RootDirection
		{
			get
			{
				return (Direction)base.sm.RootDirection.Get(this);
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x0600ABB3 RID: 43955 RVA: 0x003C95B4 File Offset: 0x003C77B4
		public VineBranch.AnimSet Anims
		{
			get
			{
				return VineBranch.GetAnimSetByShapeCategory[this.MyShapeCategory];
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x0600ABB4 RID: 43956 RVA: 0x003C95C6 File Offset: 0x003C77C6
		public bool IsOld
		{
			get
			{
				return this.oldAge.value >= this.oldAge.GetMax();
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x0600ABB5 RID: 43957 RVA: 0x003C95E3 File Offset: 0x003C77E3
		private bool IsMotherWilting
		{
			get
			{
				return this.MotherSMI != null && this.MotherSMI.IsWilting;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x0600ABB6 RID: 43958 RVA: 0x003C95FA File Offset: 0x003C77FA
		public bool IsWilting
		{
			get
			{
				return this.wiltCondition.IsWilting() || this.IsMotherWilting;
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x0600ABB7 RID: 43959 RVA: 0x003C9611 File Offset: 0x003C7811
		public bool IsGrown
		{
			get
			{
				return this.GrowthPercentage >= 1f;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x0600ABB8 RID: 43960 RVA: 0x003C9623 File Offset: 0x003C7823
		public float GrowthPercentage
		{
			get
			{
				return this.maturity.value / this.maturity.GetMax();
			}
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x0600ABB9 RID: 43961 RVA: 0x003C963C File Offset: 0x003C783C
		public bool IsReadyForHarvest
		{
			get
			{
				return this.FruitGrowthPercentage >= 1f;
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x0600ABBA RID: 43962 RVA: 0x003C964E File Offset: 0x003C784E
		public float FruitGrowthPercentage
		{
			get
			{
				return this.fruitMaturity.value / this.fruitMaturity.GetMax();
			}
		}

		// Token: 0x0600ABBB RID: 43963 RVA: 0x003C9668 File Offset: 0x003C7868
		public Instance(IStateMachineTarget master, VineBranch.Def def) : base(master, def)
		{
			Amounts amounts = base.gameObject.GetAmounts();
			this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
			this.fruitMaturity = amounts.Get(Db.Get().Amounts.Maturity2);
			this.baseGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.GROWTH_RATE, CREATURES.STATS.MATURITY.GROWING, false, false, true);
			this.wildGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.WILD_GROWTH_RATE, CREATURES.STATS.MATURITY.GROWINGWILD, false, false, true);
			this.baseFruitGrowingRate = new AttributeModifier(this.fruitMaturity.deltaAttribute.Id, def.GROWTH_RATE, CREATURES.STATS.MATURITY.GROWING, false, false, true);
			this.wildFruitGrowingRate = new AttributeModifier(this.fruitMaturity.deltaAttribute.Id, def.WILD_GROWTH_RATE, CREATURES.STATS.MATURITY.GROWINGWILD, false, false, true);
			this.oldAge = amounts.Add(new AmountInstance(Db.Get().Amounts.OldAge, base.gameObject));
			this.oldAge.maxAttribute.ClearModifiers();
			this.oldAge.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, 2400f, null, false, false, true));
			this.getOldRate = new AttributeModifier(this.oldAge.deltaAttribute.Id, 1f, null, false, false, true);
			this.wiltCondition = base.GetComponent<WiltCondition>();
			this.AnimController = base.GetComponent<KBatchedAnimController>();
			this.uprootMonitor = base.GetComponent<UprootedMonitor>();
			this.harvestable = base.GetComponent<Harvestable>();
			this.uprootMonitor.customFoundationCheckFn = new Func<int, bool>(this.IsCellFoundation);
			this.SetCellRegistrationAsPlant(true);
			base.Subscribe(1119167081, new Action<object>(this.OnSpawnedByDiscovery));
		}

		// Token: 0x0600ABBC RID: 43964 RVA: 0x003C989C File Offset: 0x003C7A9C
		public override void StartSM()
		{
			this.wasMarkedForDeadBeforeStartSM = base.sm.MarkedForDeath.Get(this);
			base.master.gameObject.AddTag(GameTags.GrowingPlant);
			base.StartSM();
			this.SetAnimOrientation(this.MyShape, this.IsGrowingClockwise);
			this.Schedule(1f, new Action<object>(this.DelayedResetUprootMonitor), null);
		}

		// Token: 0x0600ABBD RID: 43965 RVA: 0x003C9908 File Offset: 0x003C7B08
		public override void PostParamsInitialized()
		{
			base.PostParamsInitialized();
			this.MotherSMI = ((this.Mother == null) ? null : this.Mother.GetSMI<VineMother.Instance>());
			if (this.wasMarkedForDeadBeforeStartSM)
			{
				base.sm.MarkedForDeath.Set(true, this, false);
			}
			this.HideAllFruitSymbols();
		}

		// Token: 0x0600ABBE RID: 43966 RVA: 0x003C995F File Offset: 0x003C7B5F
		protected override void OnCleanUp()
		{
			this.DestroyFruitMeter();
			this.KillForwardBranch();
			this.SetCellRegistrationAsPlant(false);
			base.OnCleanUp();
		}

		// Token: 0x0600ABBF RID: 43967 RVA: 0x003C997A File Offset: 0x003C7B7A
		public void DestroySelf(object o)
		{
			CreatureHelpers.DeselectCreature(base.gameObject);
			Util.KDestroyGameObject(base.gameObject);
		}

		// Token: 0x0600ABC0 RID: 43968 RVA: 0x003C9994 File Offset: 0x003C7B94
		public void SetCellRegistrationAsPlant(bool doRegister)
		{
			int cell = Grid.PosToCell(this);
			if (doRegister && Grid.Objects[cell, 5] == null)
			{
				Grid.Objects[cell, 5] = base.gameObject;
				return;
			}
			if (!doRegister && Grid.Objects[cell, 5] == base.gameObject)
			{
				Grid.Objects[cell, 5] = null;
			}
		}

		// Token: 0x0600ABC1 RID: 43969 RVA: 0x003C99FB File Offset: 0x003C7BFB
		public void SetHarvestableState(bool canBeHarvested)
		{
			this.harvestable.SetCanBeHarvested(canBeHarvested);
		}

		// Token: 0x0600ABC2 RID: 43970 RVA: 0x003C9A0C File Offset: 0x003C7C0C
		public void SetAutoHarvestInChainReaction(bool autoharvest)
		{
			HarvestDesignatable component = base.GetComponent<HarvestDesignatable>();
			if (component != null)
			{
				component.SetHarvestWhenReady(autoharvest);
				if (this.BranchSMI != null)
				{
					this.BranchSMI.SetAutoHarvestInChainReaction(autoharvest);
				}
			}
		}

		// Token: 0x0600ABC3 RID: 43971 RVA: 0x003C9A44 File Offset: 0x003C7C44
		public void ForceCancelHarvest()
		{
			this.harvestable.ForceCancelHarvest(true);
		}

		// Token: 0x0600ABC4 RID: 43972 RVA: 0x003C9A57 File Offset: 0x003C7C57
		public void ResetOldAge()
		{
			this.oldAge.SetValue(0f);
		}

		// Token: 0x0600ABC5 RID: 43973 RVA: 0x003C9A6C File Offset: 0x003C7C6C
		private void OnSpawnedByDiscovery(object o)
		{
			float num = 1f - (float)this.MyBranchNumber / 12f;
			float num2 = (UnityEngine.Random.Range(0f, 1f) <= num) ? 1f : UnityEngine.Random.Range(0f, 1f);
			this.maturity.SetValue(this.maturity.maxAttribute.GetTotalValue() * num2);
			if (this.IsGrown)
			{
				this.IsNewGameSpawned = true;
				this.fruitMaturity.SetValue(this.fruitMaturity.maxAttribute.GetTotalValue() * UnityEngine.Random.Range(0f, 1f));
			}
		}

		// Token: 0x0600ABC6 RID: 43974 RVA: 0x003C9B0F File Offset: 0x003C7D0F
		public void ResetFruitGrowProgress()
		{
			this.fruitMaturity.SetValue(0f);
		}

		// Token: 0x0600ABC7 RID: 43975 RVA: 0x003C9B22 File Offset: 0x003C7D22
		public void SpawnHarvestedFruit()
		{
			base.GetComponent<Crop>().SpawnConfiguredFruit(null);
		}

		// Token: 0x0600ABC8 RID: 43976 RVA: 0x003C9B30 File Offset: 0x003C7D30
		public void HideAllFruitSymbols()
		{
			foreach (VineBranch.ShapeCategory key in VineBranch.GetAnimSetByShapeCategory.Keys)
			{
				VineBranch.AnimSet animSet = VineBranch.GetAnimSetByShapeCategory[key];
				this.AnimController.SetSymbolVisiblity(animSet.meter_target, false);
			}
		}

		// Token: 0x0600ABC9 RID: 43977 RVA: 0x003C9BA4 File Offset: 0x003C7DA4
		public void CreateFruitMeter()
		{
			this.DestroyFruitMeter();
			this.fruitMeter = new MeterController(this.AnimController, this.Anims.meter_target, this.Anims.meter, Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
			base.sm.Fruit.Set(this.fruitMeter.gameObject, this, false);
		}

		// Token: 0x0600ABCA RID: 43978 RVA: 0x003C9C04 File Offset: 0x003C7E04
		private void DestroyFruitMeter()
		{
			if (this.fruitMeter != null)
			{
				this.fruitMeter.Unlink();
				Util.KDestroyGameObject(this.fruitMeter.gameObject);
				this.fruitMeter = null;
				base.sm.Fruit.Set(null, this);
			}
		}

		// Token: 0x0600ABCB RID: 43979 RVA: 0x003C9C42 File Offset: 0x003C7E42
		public void PlayAnimOnFruitMeter(string animName, KAnim.PlayMode playMode)
		{
			if (this.fruitMeter != null)
			{
				this.fruitMeter.meterController.Play(animName, playMode, 1f, 0f);
			}
		}

		// Token: 0x0600ABCC RID: 43980 RVA: 0x003C9C70 File Offset: 0x003C7E70
		public void UpdateFruitGrowMeterPosition()
		{
			if (this.fruitMeter != null)
			{
				if (this.fruitMeter.meterController.currentAnim != this.Anims.meter)
				{
					this.PlayAnimOnFruitMeter(this.Anims.meter, KAnim.PlayMode.Paused);
				}
				this.fruitMeter.SetPositionPercent(this.FruitGrowthPercentage);
			}
		}

		// Token: 0x0600ABCD RID: 43981 RVA: 0x003C9CD0 File Offset: 0x003C7ED0
		private void KillForwardBranch()
		{
			if (this.Branch != null)
			{
				VineBranch.Instance smi = this.Branch.GetSMI<VineBranch.Instance>();
				if (smi != null)
				{
					smi.sm.DieSignal.Trigger(smi);
					smi.sm.MarkedForDeath.Set(true, smi, false);
				}
				base.sm.Branch.Set(null, this);
			}
		}

		// Token: 0x0600ABCE RID: 43982 RVA: 0x003C9D34 File Offset: 0x003C7F34
		public void SetupRootInformation(VineMother.Instance mother)
		{
			CellOffset cellOffsetDirection = Grid.GetCellOffsetDirection(Grid.PosToCell(this), Grid.PosToCell(mother));
			Direction value = (cellOffsetDirection == CellOffset.left) ? Direction.Left : ((cellOffsetDirection == CellOffset.right) ? Direction.Right : ((cellOffsetDirection == CellOffset.up) ? Direction.Up : Direction.Down));
			base.sm.RootDirection.Set((int)value, this, false);
			base.sm.RootShape.Set(1, this, false);
			base.sm.BranchNumber.Set(1, this, false);
			base.sm.WildPlanted.Set(mother.IsWild, this, false);
			base.sm.Mother.Set(mother.gameObject, this, false);
			this.MotherSMI = ((this.Mother == null) ? null : this.Mother.GetSMI<VineMother.Instance>());
			HarvestDesignatable component = mother.GetComponent<HarvestDesignatable>();
			base.GetComponent<HarvestDesignatable>().SetHarvestWhenReady(component.HarvestWhenReady);
		}

		// Token: 0x0600ABCF RID: 43983 RVA: 0x003C9E30 File Offset: 0x003C8030
		public void SetupRootInformation(VineBranch.Instance root)
		{
			CellOffset cellOffsetDirection = Grid.GetCellOffsetDirection(Grid.PosToCell(this), Grid.PosToCell(root));
			Direction value = (cellOffsetDirection == CellOffset.left) ? Direction.Left : ((cellOffsetDirection == CellOffset.right) ? Direction.Right : ((cellOffsetDirection == CellOffset.up) ? Direction.Up : Direction.Down));
			base.sm.RootDirection.Set((int)value, this, false);
			base.sm.RootShape.Set((int)root.MyShape, this, false);
			base.sm.BranchNumber.Set(root.MyBranchNumber + 1, this, false);
			base.sm.WildPlanted.Set(root.IsWild, this, false);
			base.sm.Mother.Set(root.Mother, this, false);
			this.MotherSMI = ((this.Mother == null) ? null : this.Mother.GetSMI<VineMother.Instance>());
			HarvestDesignatable component = root.GetComponent<HarvestDesignatable>();
			base.GetComponent<HarvestDesignatable>().SetHarvestWhenReady(component.HarvestWhenReady);
		}

		// Token: 0x0600ABD0 RID: 43984 RVA: 0x003C9F38 File Offset: 0x003C8138
		public void AttemptToSpawnBranch()
		{
			if (this.CanSpawnBranch())
			{
				this.isSpawningNextBranch = true;
				int cellToSpawnBranch = this.GetCellToSpawnBranch();
				GameObject gameObject = this.SpawnBranchOnCell(cellToSpawnBranch);
				base.sm.Branch.Set(gameObject, this, false);
				this.isSpawningNextBranch = false;
				if (this.IsNewGameSpawned)
				{
					gameObject.Trigger(1119167081, null);
				}
				this.ResetUprootMonitor();
			}
			if (this.IsNewGameSpawned)
			{
				this.IsNewGameSpawned = false;
			}
		}

		// Token: 0x0600ABD1 RID: 43985 RVA: 0x003C9FA8 File Offset: 0x003C81A8
		private GameObject SpawnBranchOnCell(int cell)
		{
			Vector3 position = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.def.BRANCH_PREFAB_NAME), position);
			gameObject.SetActive(true);
			gameObject.GetSMI<VineBranch.Instance>().SetupRootInformation(this);
			return gameObject;
		}

		// Token: 0x0600ABD2 RID: 43986 RVA: 0x003C9FEC File Offset: 0x003C81EC
		private bool IsMotherCellFoundation(int cell)
		{
			return this.MotherSMI != null && this.MotherSMI.IsOnPlanterBox && this.MotherSMI.PlanterboxCell == cell;
		}

		// Token: 0x0600ABD3 RID: 43987 RVA: 0x003CA013 File Offset: 0x003C8213
		private bool IsCellFoundation(int cell)
		{
			return VineBranch.IsCellFoundation(cell) || this.IsMotherCellFoundation(cell);
		}

		// Token: 0x0600ABD4 RID: 43988 RVA: 0x003CA028 File Offset: 0x003C8228
		private bool IsCellAvailable(int cell)
		{
			bool flag = VineBranch.IsCellAvailable(base.gameObject, cell, new Func<int, bool>(this.IsCellFoundation));
			if (flag && this.IsNewGameSpawned)
			{
				flag = (SaveGame.Instance.worldGenSpawner.GetSpawnableInCell(cell) == null);
			}
			return flag;
		}

		// Token: 0x0600ABD5 RID: 43989 RVA: 0x003CA070 File Offset: 0x003C8270
		public bool CanSpawnBranch()
		{
			bool flag = this.Branch == null;
			flag = (flag && !this.MaxBranchNumberReached);
			flag = (flag && this.IsGrown);
			if (flag)
			{
				int cellToSpawnBranch = this.GetCellToSpawnBranch();
				flag = (flag && cellToSpawnBranch != Grid.InvalidCell);
				flag = (flag && this.IsCellAvailable(cellToSpawnBranch));
			}
			return flag;
		}

		// Token: 0x0600ABD6 RID: 43990 RVA: 0x003CA0D4 File Offset: 0x003C82D4
		public int GetCellToSpawnBranch()
		{
			int cell = Grid.PosToCell(base.gameObject);
			switch (this.MyShape)
			{
			case VineBranch.Shape.Top:
			case VineBranch.Shape.Bottom:
				if (this.RootDirection != Direction.Left)
				{
					return Grid.OffsetCell(cell, -1, 0);
				}
				return Grid.OffsetCell(cell, 1, 0);
			case VineBranch.Shape.Left:
			case VineBranch.Shape.Right:
				if (this.RootDirection != Direction.Up)
				{
					return Grid.OffsetCell(cell, 0, 1);
				}
				return Grid.OffsetCell(cell, 0, -1);
			case VineBranch.Shape.InCornerTopLeft:
				if (this.RootDirection != Direction.Down)
				{
					return Grid.OffsetCell(cell, 0, -1);
				}
				return Grid.OffsetCell(cell, 1, 0);
			case VineBranch.Shape.InCornerTopRight:
				if (this.RootDirection != Direction.Down)
				{
					return Grid.OffsetCell(cell, 0, -1);
				}
				return Grid.OffsetCell(cell, -1, 0);
			case VineBranch.Shape.InCornerBottomLeft:
				if (this.RootDirection != Direction.Up)
				{
					return Grid.OffsetCell(cell, 0, 1);
				}
				return Grid.OffsetCell(cell, 1, 0);
			case VineBranch.Shape.InCornerBottomRight:
				if (this.RootDirection != Direction.Up)
				{
					return Grid.OffsetCell(cell, 0, 1);
				}
				return Grid.OffsetCell(cell, -1, 0);
			case VineBranch.Shape.OutCornerTopLeft:
				if (this.RootDirection != Direction.Up)
				{
					return Grid.OffsetCell(cell, 0, 1);
				}
				return Grid.OffsetCell(cell, -1, 0);
			case VineBranch.Shape.OutCornerTopRight:
				if (this.RootDirection != Direction.Up)
				{
					return Grid.OffsetCell(cell, 0, 1);
				}
				return Grid.OffsetCell(cell, 1, 0);
			case VineBranch.Shape.OutCornerBottomLeft:
				if (this.RootDirection != Direction.Down)
				{
					return Grid.OffsetCell(cell, 0, -1);
				}
				return Grid.OffsetCell(cell, -1, 0);
			case VineBranch.Shape.OutCornerBottomRight:
				if (this.RootDirection != Direction.Down)
				{
					return Grid.OffsetCell(cell, 0, -1);
				}
				return Grid.OffsetCell(cell, 1, 0);
			default:
				return Grid.InvalidCell;
			}
		}

		// Token: 0x0600ABD7 RID: 43991 RVA: 0x003CA240 File Offset: 0x003C8440
		public void SubscribeSurroundingSolidChangesListeners()
		{
			KPrefabID component = base.gameObject.GetComponent<KPrefabID>();
			this.UnSubscribreSurroundingSolidChangesListeners();
			CellOffset[] offsets = new CellOffset[]
			{
				new CellOffset(-1, -1),
				new CellOffset(0, -1),
				new CellOffset(1, -1),
				new CellOffset(-1, 0),
				new CellOffset(1, 0),
				new CellOffset(-1, 1),
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			};
			Extents extents = new Extents(Grid.PosToCell(base.gameObject), offsets);
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("VineBranchSurroundingListenerSolids", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSurroundingCellsBlockageChangedDetected));
			this.buildingsPartitionerEntry = GameScenePartitioner.Instance.Add("VineBranchSurroundingListenerBuildings", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnSurroundingCellsBlockageChangedDetected));
			this.plantsPartitionerEntry = GameScenePartitioner.Instance.Add("VineBranchSurroundingListenerPlants", component, extents, GameScenePartitioner.Instance.plantsChangedLayer, new Action<object>(this.OnSurroundingCellsBlockageChangedDetected));
			this.liquidsPartitionerEntry = GameScenePartitioner.Instance.Add("VineBranchSurroundingListenerLiquids", base.gameObject, extents, GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnSurroundingCellsBlockageChangedDetected));
		}

		// Token: 0x0600ABD8 RID: 43992 RVA: 0x003CA3B0 File Offset: 0x003C85B0
		public void UnSubscribreSurroundingSolidChangesListeners()
		{
			GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.buildingsPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.plantsPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.liquidsPartitionerEntry);
			this.solidPartitionerEntry = HandleVector<int>.InvalidHandle;
			this.buildingsPartitionerEntry = HandleVector<int>.InvalidHandle;
			this.plantsPartitionerEntry = HandleVector<int>.InvalidHandle;
			this.liquidsPartitionerEntry = HandleVector<int>.InvalidHandle;
		}

		// Token: 0x0600ABD9 RID: 43993 RVA: 0x003CA429 File Offset: 0x003C8629
		private void OnSurroundingCellsBlockageChangedDetected(object o)
		{
			if (this.CanChangeShape)
			{
				this.RecalculateMyShape();
			}
		}

		// Token: 0x0600ABDA RID: 43994 RVA: 0x003CA439 File Offset: 0x003C8639
		private void SetShape(VineBranch.Shape shape, bool clockwise)
		{
			base.sm.BranchShape.Set((int)shape, this, false);
			base.sm.GrowingClockwise.Set(clockwise, this, false);
			this.SetAnimOrientation(shape, clockwise);
			base.Trigger(838747413, null);
		}

		// Token: 0x0600ABDB RID: 43995 RVA: 0x003CA478 File Offset: 0x003C8678
		public void RecalculateMyShape()
		{
			VineBranch.Shape shape = VineBranch.Shape.TopEnd;
			bool clockwise = false;
			switch (this.RootDirection)
			{
			case Direction.Up:
				switch (base.smi.RootShape)
				{
				case VineBranch.Shape.Left:
				case VineBranch.Shape.InCornerTopLeft:
				case VineBranch.Shape.OutCornerBottomLeft:
					shape = this.ChooseCompatibleShape(new VineBranch.Shape[]
					{
						VineBranch.Shape.BottomEnd,
						VineBranch.Shape.InCornerBottomLeft,
						VineBranch.Shape.OutCornerTopLeft,
						VineBranch.Shape.Left
					});
					clockwise = (shape == VineBranch.Shape.OutCornerTopLeft);
					break;
				case VineBranch.Shape.Right:
				case VineBranch.Shape.InCornerTopRight:
				case VineBranch.Shape.OutCornerBottomRight:
					shape = this.ChooseCompatibleShape(new VineBranch.Shape[]
					{
						VineBranch.Shape.BottomEnd,
						VineBranch.Shape.InCornerBottomRight,
						VineBranch.Shape.OutCornerTopRight,
						VineBranch.Shape.Right
					});
					clockwise = (shape != VineBranch.Shape.OutCornerTopRight);
					break;
				}
				break;
			case Direction.Right:
				switch (base.smi.RootShape)
				{
				case VineBranch.Shape.Top:
				case VineBranch.Shape.InCornerTopRight:
				case VineBranch.Shape.OutCornerTopLeft:
					shape = this.ChooseCompatibleShape(new VineBranch.Shape[]
					{
						VineBranch.Shape.LeftEnd,
						VineBranch.Shape.InCornerTopLeft,
						VineBranch.Shape.OutCornerTopRight,
						VineBranch.Shape.Top
					});
					clockwise = (shape == VineBranch.Shape.OutCornerTopRight);
					break;
				case VineBranch.Shape.Bottom:
				case VineBranch.Shape.InCornerBottomRight:
				case VineBranch.Shape.OutCornerBottomLeft:
					shape = this.ChooseCompatibleShape(new VineBranch.Shape[]
					{
						VineBranch.Shape.LeftEnd,
						VineBranch.Shape.InCornerBottomLeft,
						VineBranch.Shape.OutCornerBottomRight,
						VineBranch.Shape.Bottom
					});
					clockwise = (shape != VineBranch.Shape.OutCornerBottomRight);
					break;
				}
				break;
			case Direction.Down:
				switch (base.smi.RootShape)
				{
				case VineBranch.Shape.Left:
				case VineBranch.Shape.InCornerBottomLeft:
				case VineBranch.Shape.OutCornerTopLeft:
					shape = this.ChooseCompatibleShape(new VineBranch.Shape[]
					{
						VineBranch.Shape.TopEnd,
						VineBranch.Shape.InCornerTopLeft,
						VineBranch.Shape.OutCornerBottomLeft,
						VineBranch.Shape.Left
					});
					clockwise = (shape != VineBranch.Shape.OutCornerBottomLeft);
					break;
				case VineBranch.Shape.Right:
				case VineBranch.Shape.InCornerBottomRight:
				case VineBranch.Shape.OutCornerTopRight:
					shape = this.ChooseCompatibleShape(new VineBranch.Shape[]
					{
						VineBranch.Shape.TopEnd,
						VineBranch.Shape.InCornerTopRight,
						VineBranch.Shape.OutCornerBottomRight,
						VineBranch.Shape.Right
					});
					clockwise = (shape == VineBranch.Shape.OutCornerBottomRight);
					break;
				}
				break;
			case Direction.Left:
			{
				VineBranch.Shape rootShape = this.RootShape;
				switch (rootShape)
				{
				case VineBranch.Shape.Top:
				case VineBranch.Shape.InCornerTopLeft:
					break;
				case VineBranch.Shape.Bottom:
				case VineBranch.Shape.InCornerBottomLeft:
					goto IL_84;
				case VineBranch.Shape.Left:
				case VineBranch.Shape.Right:
				case VineBranch.Shape.InCornerTopRight:
					goto IL_230;
				default:
					if (rootShape != VineBranch.Shape.OutCornerTopRight)
					{
						if (rootShape != VineBranch.Shape.OutCornerBottomRight)
						{
							goto IL_230;
						}
						goto IL_84;
					}
					break;
				}
				shape = this.ChooseCompatibleShape(new VineBranch.Shape[]
				{
					VineBranch.Shape.RightEnd,
					VineBranch.Shape.InCornerTopRight,
					VineBranch.Shape.OutCornerTopLeft,
					VineBranch.Shape.Top
				});
				clockwise = (shape != VineBranch.Shape.OutCornerTopLeft);
				break;
				IL_84:
				shape = this.ChooseCompatibleShape(new VineBranch.Shape[]
				{
					VineBranch.Shape.RightEnd,
					VineBranch.Shape.InCornerBottomRight,
					VineBranch.Shape.OutCornerBottomLeft,
					VineBranch.Shape.Bottom
				});
				clockwise = (shape == VineBranch.Shape.OutCornerBottomLeft);
				break;
			}
			}
			IL_230:
			base.smi.SetShape(shape, clockwise);
		}

		// Token: 0x0600ABDC RID: 43996 RVA: 0x003CA6C4 File Offset: 0x003C88C4
		private void SetAnimOrientation(VineBranch.Shape shape, bool clockwise)
		{
			this.AnimController.FlipX = false;
			this.AnimController.FlipY = false;
			this.AnimController.Rotation = 0f;
			switch (shape)
			{
			case VineBranch.Shape.Top:
				this.AnimController.FlipY = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 0 : 180);
				break;
			case VineBranch.Shape.Bottom:
				this.AnimController.FlipX = clockwise;
				break;
			case VineBranch.Shape.Left:
				this.AnimController.FlipX = clockwise;
				this.AnimController.Rotation = 90f;
				break;
			case VineBranch.Shape.Right:
				this.AnimController.FlipX = clockwise;
				this.AnimController.Rotation = 270f;
				break;
			case VineBranch.Shape.InCornerTopLeft:
				this.AnimController.FlipX = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 90 : 180);
				break;
			case VineBranch.Shape.InCornerTopRight:
				this.AnimController.FlipY = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 0 : 270);
				break;
			case VineBranch.Shape.InCornerBottomLeft:
				this.AnimController.FlipX = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 0 : 90);
				break;
			case VineBranch.Shape.InCornerBottomRight:
				this.AnimController.FlipY = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 90 : 0);
				break;
			case VineBranch.Shape.OutCornerTopLeft:
				this.AnimController.FlipY = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 90 : 0);
				break;
			case VineBranch.Shape.OutCornerTopRight:
				this.AnimController.FlipX = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 0 : 90);
				break;
			case VineBranch.Shape.OutCornerBottomLeft:
				this.AnimController.FlipY = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 0 : 270);
				break;
			case VineBranch.Shape.OutCornerBottomRight:
				this.AnimController.FlipX = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 90 : 180);
				break;
			case VineBranch.Shape.TopEnd:
				this.AnimController.FlipX = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 90 : 270);
				break;
			case VineBranch.Shape.BottomEnd:
				this.AnimController.FlipX = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 270 : 90);
				break;
			case VineBranch.Shape.LeftEnd:
				this.AnimController.FlipX = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 0 : 180);
				break;
			case VineBranch.Shape.RightEnd:
				this.AnimController.FlipX = clockwise;
				this.AnimController.Rotation = (float)(clockwise ? 180 : 0);
				break;
			}
			this.AnimController.Rotation *= -1f;
			this.AnimController.Offset = new Vector3(0f, (float)(this.AnimController.FlipY ? 1 : 0), 0f);
			bool flag = this.AnimController.FlipY && Mathf.Abs(this.AnimController.Rotation) == 90f;
			this.AnimController.Pivot = new Vector3(0f, flag ? -0.5f : 0.5f, 0f);
		}

		// Token: 0x0600ABDD RID: 43997 RVA: 0x003CAA20 File Offset: 0x003C8C20
		private VineBranch.Shape ChooseCompatibleShape(VineBranch.Shape[] possibleShapesOrderedByPriority)
		{
			bool flag = false;
			VineBranch.Shape result = VineBranch.Shape.TopEnd;
			int cell = Grid.PosToCell(base.gameObject);
			int cell2 = Grid.OffsetCell(cell, -1, 0);
			int cell3 = Grid.OffsetCell(cell, 1, 0);
			int cell4 = Grid.OffsetCell(cell, 0, 1);
			int cell5 = Grid.OffsetCell(cell, 0, -1);
			int cell6 = Grid.OffsetCell(cell, -1, 1);
			int cell7 = Grid.OffsetCell(cell, 1, 1);
			int cell8 = Grid.OffsetCell(cell, -1, -1);
			int cell9 = Grid.OffsetCell(cell, 1, -1);
			foreach (VineBranch.Shape shape in possibleShapesOrderedByPriority)
			{
				VineBranch.ShapeCategory shapeCategory = VineBranch.GetShapeCategory[shape];
				if (shapeCategory == VineBranch.ShapeCategory.DeadEnd)
				{
					result = shape;
				}
				if (!this.MaxBranchNumberReached || shapeCategory == VineBranch.ShapeCategory.DeadEnd)
				{
					switch (shape)
					{
					case VineBranch.Shape.Top:
						flag = (this.IsCellFoundation(cell4) && (this.IsCellAvailable(cell2) || this.IsCellAvailable(cell3)));
						break;
					case VineBranch.Shape.Bottom:
						flag = (this.IsCellFoundation(cell5) && (this.IsCellAvailable(cell2) || this.IsCellAvailable(cell3)));
						break;
					case VineBranch.Shape.Left:
						flag = (this.IsCellFoundation(cell2) && (this.IsCellAvailable(cell4) || this.IsCellAvailable(cell5)));
						break;
					case VineBranch.Shape.Right:
						flag = (this.IsCellFoundation(cell3) && (this.IsCellAvailable(cell4) || this.IsCellAvailable(cell5)));
						break;
					case VineBranch.Shape.InCornerTopLeft:
						flag = (this.IsCellFoundation(cell4) && this.IsCellFoundation(cell2));
						break;
					case VineBranch.Shape.InCornerTopRight:
						flag = (this.IsCellFoundation(cell4) && this.IsCellFoundation(cell3));
						break;
					case VineBranch.Shape.InCornerBottomLeft:
						flag = (this.IsCellFoundation(cell5) && this.IsCellFoundation(cell2));
						break;
					case VineBranch.Shape.InCornerBottomRight:
						flag = (this.IsCellFoundation(cell5) && this.IsCellFoundation(cell3));
						break;
					case VineBranch.Shape.OutCornerTopLeft:
						flag = ((this.IsCellAvailable(cell4) || this.IsCellAvailable(cell2)) && this.IsCellFoundation(cell6));
						break;
					case VineBranch.Shape.OutCornerTopRight:
						flag = ((this.IsCellAvailable(cell4) || this.IsCellAvailable(cell3)) && this.IsCellFoundation(cell7));
						break;
					case VineBranch.Shape.OutCornerBottomLeft:
						flag = ((this.IsCellAvailable(cell5) || this.IsCellAvailable(cell2)) && this.IsCellFoundation(cell8));
						break;
					case VineBranch.Shape.OutCornerBottomRight:
						flag = ((this.IsCellAvailable(cell5) || this.IsCellAvailable(cell3)) && this.IsCellFoundation(cell9));
						break;
					case VineBranch.Shape.TopEnd:
						flag = (!this.IsCellAvailable(cell2) && !this.IsCellAvailable(cell4) && !this.IsCellAvailable(cell3));
						break;
					case VineBranch.Shape.BottomEnd:
						flag = (!this.IsCellAvailable(cell2) && !this.IsCellAvailable(cell5) && !this.IsCellAvailable(cell3));
						break;
					case VineBranch.Shape.LeftEnd:
						flag = (!this.IsCellAvailable(cell4) && !this.IsCellAvailable(cell5) && !this.IsCellAvailable(cell2));
						break;
					case VineBranch.Shape.RightEnd:
						flag = (!this.IsCellAvailable(cell4) && !this.IsCellAvailable(cell5) && !this.IsCellAvailable(cell3));
						break;
					}
					if (flag)
					{
						return shape;
					}
				}
			}
			return result;
		}

		// Token: 0x0600ABDE RID: 43998 RVA: 0x003CAD3F File Offset: 0x003C8F3F
		private void DelayedResetUprootMonitor(object o)
		{
			this.ResetUprootMonitor();
		}

		// Token: 0x0600ABDF RID: 43999 RVA: 0x003CAD48 File Offset: 0x003C8F48
		public void ResetUprootMonitor()
		{
			CellOffset[] newMonitorCells = new CellOffset[0];
			if (!this.CanChangeShape && !this.MaxBranchNumberReached)
			{
				switch (this.MyShape)
				{
				case VineBranch.Shape.Top:
					newMonitorCells = new CellOffset[]
					{
						CellOffset.up
					};
					break;
				case VineBranch.Shape.Bottom:
					newMonitorCells = new CellOffset[]
					{
						CellOffset.down
					};
					break;
				case VineBranch.Shape.Left:
					newMonitorCells = new CellOffset[]
					{
						CellOffset.left
					};
					break;
				case VineBranch.Shape.Right:
					newMonitorCells = new CellOffset[]
					{
						CellOffset.right
					};
					break;
				case VineBranch.Shape.InCornerTopLeft:
					newMonitorCells = new CellOffset[]
					{
						CellOffset.up,
						CellOffset.left
					};
					break;
				case VineBranch.Shape.InCornerTopRight:
					newMonitorCells = new CellOffset[]
					{
						CellOffset.up,
						CellOffset.right
					};
					break;
				case VineBranch.Shape.InCornerBottomLeft:
					newMonitorCells = new CellOffset[]
					{
						CellOffset.down,
						CellOffset.left
					};
					break;
				case VineBranch.Shape.InCornerBottomRight:
					newMonitorCells = new CellOffset[]
					{
						CellOffset.down,
						CellOffset.right
					};
					break;
				case VineBranch.Shape.OutCornerTopLeft:
					newMonitorCells = new CellOffset[]
					{
						new CellOffset(-1, 1)
					};
					break;
				case VineBranch.Shape.OutCornerTopRight:
					newMonitorCells = new CellOffset[]
					{
						new CellOffset(1, 1)
					};
					break;
				case VineBranch.Shape.OutCornerBottomLeft:
					newMonitorCells = new CellOffset[]
					{
						new CellOffset(-1, -1)
					};
					break;
				case VineBranch.Shape.OutCornerBottomRight:
					newMonitorCells = new CellOffset[]
					{
						new CellOffset(1, -1)
					};
					break;
				case VineBranch.Shape.TopEnd:
					newMonitorCells = new CellOffset[]
					{
						this.IsGrowingClockwise ? CellOffset.left : CellOffset.right
					};
					break;
				case VineBranch.Shape.BottomEnd:
					newMonitorCells = new CellOffset[]
					{
						this.IsGrowingClockwise ? CellOffset.right : CellOffset.left
					};
					break;
				case VineBranch.Shape.LeftEnd:
					newMonitorCells = new CellOffset[]
					{
						this.IsGrowingClockwise ? CellOffset.down : CellOffset.up
					};
					break;
				case VineBranch.Shape.RightEnd:
					newMonitorCells = new CellOffset[]
					{
						this.IsGrowingClockwise ? CellOffset.up : CellOffset.down
					};
					break;
				}
			}
			this.uprootMonitor.SetNewMonitorCells(newMonitorCells);
		}

		// Token: 0x0600ABE0 RID: 44000 RVA: 0x003CAFB8 File Offset: 0x003C91B8
		public float TimeUntilNextHarvest()
		{
			float num = (this.maturity.GetDelta() <= 0f) ? 0f : ((this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta());
			float num2 = (this.fruitMaturity.GetDelta() <= 0f) ? 0f : ((this.fruitMaturity.GetMax() - this.fruitMaturity.value) / this.fruitMaturity.GetDelta());
			return num + num2;
		}

		// Token: 0x0600ABE1 RID: 44001 RVA: 0x003CB040 File Offset: 0x003C9240
		public float GetCurrentGrowthPercentage()
		{
			if (!this.IsGrown)
			{
				return this.GrowthPercentage;
			}
			return this.FruitGrowthPercentage;
		}

		// Token: 0x0600ABE2 RID: 44002 RVA: 0x003CB057 File Offset: 0x003C9257
		public float PercentGrown()
		{
			return this.GetCurrentGrowthPercentage();
		}

		// Token: 0x0600ABE3 RID: 44003 RVA: 0x003CB05F File Offset: 0x003C925F
		public Crop GetCropComponent()
		{
			return base.GetComponent<Crop>();
		}

		// Token: 0x0600ABE4 RID: 44004 RVA: 0x003CB067 File Offset: 0x003C9267
		public float DomesticGrowthTime()
		{
			return this.maturity.GetMax() / this.baseGrowingRate.Value;
		}

		// Token: 0x0600ABE5 RID: 44005 RVA: 0x003CB080 File Offset: 0x003C9280
		public float WildGrowthTime()
		{
			return this.maturity.GetMax() / this.wildGrowingRate.Value;
		}

		// Token: 0x0600ABE6 RID: 44006 RVA: 0x003CB09C File Offset: 0x003C929C
		public void OverrideMaturityLevel(float percent)
		{
			float value = this.maturity.GetMax() * percent;
			this.maturity.SetValue(value);
		}

		// Token: 0x0600ABE7 RID: 44007 RVA: 0x003CB0C4 File Offset: 0x003C92C4
		public bool IsWildPlanted()
		{
			return this.IsWild;
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x0600ABE8 RID: 44008 RVA: 0x003CB0CC File Offset: 0x003C92CC
		public string WiltStateString
		{
			get
			{
				return "    • " + DUPLICANTS.STATS.VINEMOTHERHEALTH.NAME;
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x0600ABE9 RID: 44009 RVA: 0x003CB0E2 File Offset: 0x003C92E2
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.UnhealthyRoot
				};
			}
		}

		// Token: 0x04008634 RID: 34356
		private bool isSpawningNextBranch;

		// Token: 0x04008635 RID: 34357
		public bool IsNewGameSpawned;

		// Token: 0x04008636 RID: 34358
		public AttributeModifier baseGrowingRate;

		// Token: 0x04008637 RID: 34359
		public AttributeModifier wildGrowingRate;

		// Token: 0x04008638 RID: 34360
		public AttributeModifier baseFruitGrowingRate;

		// Token: 0x04008639 RID: 34361
		public AttributeModifier wildFruitGrowingRate;

		// Token: 0x0400863A RID: 34362
		public AttributeModifier getOldRate;

		// Token: 0x0400863B RID: 34363
		public KBatchedAnimController AnimController;

		// Token: 0x0400863C RID: 34364
		private AmountInstance maturity;

		// Token: 0x0400863D RID: 34365
		private AmountInstance fruitMaturity;

		// Token: 0x0400863E RID: 34366
		private AmountInstance oldAge;

		// Token: 0x0400863F RID: 34367
		private WiltCondition wiltCondition;

		// Token: 0x04008640 RID: 34368
		private VineMother.Instance MotherSMI;

		// Token: 0x04008641 RID: 34369
		private UprootedMonitor uprootMonitor;

		// Token: 0x04008642 RID: 34370
		private Harvestable harvestable;

		// Token: 0x04008643 RID: 34371
		private MeterController fruitMeter;

		// Token: 0x04008644 RID: 34372
		private HandleVector<int>.Handle solidPartitionerEntry = HandleVector<int>.InvalidHandle;

		// Token: 0x04008645 RID: 34373
		private HandleVector<int>.Handle buildingsPartitionerEntry = HandleVector<int>.InvalidHandle;

		// Token: 0x04008646 RID: 34374
		private HandleVector<int>.Handle plantsPartitionerEntry = HandleVector<int>.InvalidHandle;

		// Token: 0x04008647 RID: 34375
		private HandleVector<int>.Handle liquidsPartitionerEntry = HandleVector<int>.InvalidHandle;

		// Token: 0x04008648 RID: 34376
		private bool wasMarkedForDeadBeforeStartSM;
	}
}
