using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using UnityEngine;

// Token: 0x02000A08 RID: 2568
public class BionicBatteryMonitor : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>
{
	// Token: 0x06004B2E RID: 19246 RVA: 0x001B51F0 File Offset: 0x001B33F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.firstSpawn;
		this.firstSpawn.ParamTransition<bool>(this.InitialElectrobanksSpawned, this.online, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsTrue).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SpawnAndInstallInitialElectrobanks));
		this.online.TriggerOnEnter(GameHashes.BionicOnline, null).Transition(this.offline, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.DoesNotHaveCharge), UpdateRate.SIM_200ms).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.ReorganizeElectrobankStorage)).Update(new Action<BionicBatteryMonitor.Instance, float>(BionicBatteryMonitor.DischargeUpdate), UpdateRate.SIM_200ms, false).DefaultState(this.online.idle);
		this.online.idle.ParamTransition<int>(this.ChargedElectrobankCount, this.online.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int).OnSignal(this.OnElectrobankStorageChanged, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Parameter<StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.SignalParameter>.Callback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleChanged, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleBlocksTick, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep));
		this.online.upkeep.ParamTransition<int>(this.ChargedElectrobankCount, this.online.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.ScheduleChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.ScheduleBlocksTick, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).OnSignal(this.OnElectrobankStorageChanged, this.online.idle, (BionicBatteryMonitor.Instance smi, StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.SignalParameter param) => !BionicBatteryMonitor.WantsToUpkeep(smi)).DefaultState(this.online.upkeep.seekElectrobank);
		this.online.upkeep.seekElectrobank.ToggleUrge(Db.Get().Urges.ReloadElectrobank).ToggleChore((BionicBatteryMonitor.Instance smi) => new ReloadElectrobankChore(smi.master), this.online.idle);
		this.online.critical.DefaultState(this.online.critical.seekElectrobank).ParamTransition<int>(this.ChargedElectrobankCount, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTOne_Int).DoTutorial(Tutorial.TutorialMessages.TM_BionicBattery);
		this.online.critical.seekElectrobank.ToggleUrge(Db.Get().Urges.ReloadElectrobank).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new ReloadElectrobankChore(smi.master), null);
		this.offline.DefaultState(this.offline.waitingForBatteryDelivery).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new BeOfflineChore(smi.master), null).ToggleUrge(Db.Get().Urges.BeOffline).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SetOffline)).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DropAllDischargedElectrobanks)).TriggerOnEnter(GameHashes.BionicOffline, null);
		this.offline.waitingForBatteryDelivery.ParamTransition<int>(this.ChargedElectrobankCount, this.offline.waitingForBatteryInstallation, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTZero_Int).Toggle("Enable Delivery of new Electrobanks", new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.EnableManualDelivery), new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DisableManualDelivery)).Toggle("Enable User Prioritization", new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.EnablePrioritizationComponent), new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DisablePrioritizationComponent));
		this.offline.waitingForBatteryInstallation.Toggle("Enable User Prioritization", new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.EnablePrioritizationComponent), new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DisablePrioritizationComponent)).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.StartReanimateWorkChore)).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.CancelReanimateWorkChore)).WorkableCompleteTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.reboot).DefaultState(this.offline.waitingForBatteryInstallation.waiting);
		this.offline.waitingForBatteryInstallation.waiting.ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicWaitingForReboot, null).WorkableStartTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.waitingForBatteryInstallation.working);
		this.offline.waitingForBatteryInstallation.working.WorkableStopTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.waitingForBatteryInstallation.waiting);
		this.offline.reboot.PlayAnim("power_up").OnAnimQueueComplete(this.online).ScheduleGoTo(10f, this.online).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.AutomaticallyDropAllDepletedElectrobanks)).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SetOnline));
	}

	// Token: 0x06004B2F RID: 19247 RVA: 0x001B5747 File Offset: 0x001B3947
	public static ReanimateBionicWorkable GetReanimateWorkable(BionicBatteryMonitor.Instance smi)
	{
		return smi.reanimateWorkable;
	}

	// Token: 0x06004B30 RID: 19248 RVA: 0x001B574F File Offset: 0x001B394F
	public static bool DoesNotHaveCharge(BionicBatteryMonitor.Instance smi)
	{
		return smi.CurrentCharge <= 0f;
	}

	// Token: 0x06004B31 RID: 19249 RVA: 0x001B5761 File Offset: 0x001B3961
	public static bool IsCriticallyLow(BionicBatteryMonitor.Instance smi)
	{
		return smi.ChargedElectrobankCount <= 1;
	}

	// Token: 0x06004B32 RID: 19250 RVA: 0x001B576F File Offset: 0x001B396F
	public static bool ChargeIsBelowNotificationThreshold(BionicBatteryMonitor.Instance smi)
	{
		return smi.CurrentCharge <= 30000f;
	}

	// Token: 0x06004B33 RID: 19251 RVA: 0x001B5781 File Offset: 0x001B3981
	public static bool IsAnyElectrobankAvailableToBeFetched(BionicBatteryMonitor.Instance smi)
	{
		return smi.GetClosestElectrobank() != null;
	}

	// Token: 0x06004B34 RID: 19252 RVA: 0x001B578F File Offset: 0x001B398F
	public static bool WantsToInstallNewBattery(BionicBatteryMonitor.Instance smi)
	{
		return BionicBatteryMonitor.IsCriticallyLow(smi) || (smi.InUpkeepTime && smi.ChargedElectrobankCount < smi.ElectrobankCountCapacity);
	}

	// Token: 0x06004B35 RID: 19253 RVA: 0x001B57B3 File Offset: 0x001B39B3
	public static bool WantsToUpkeep(BionicBatteryMonitor.Instance smi)
	{
		return BionicBatteryMonitor.WantsToInstallNewBattery(smi);
	}

	// Token: 0x06004B36 RID: 19254 RVA: 0x001B57BB File Offset: 0x001B39BB
	public static bool WantsToUpkeep(BionicBatteryMonitor.Instance smi, StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.SignalParameter param)
	{
		return BionicBatteryMonitor.WantsToInstallNewBattery(smi);
	}

	// Token: 0x06004B37 RID: 19255 RVA: 0x001B57C3 File Offset: 0x001B39C3
	public static void SpawnAndInstallInitialElectrobanks(BionicBatteryMonitor.Instance smi)
	{
		smi.SpawnAndInstallInitialElectrobanks();
	}

	// Token: 0x06004B38 RID: 19256 RVA: 0x001B57CB File Offset: 0x001B39CB
	public static void RefreshCharge(BionicBatteryMonitor.Instance smi)
	{
		smi.RefreshCharge();
	}

	// Token: 0x06004B39 RID: 19257 RVA: 0x001B57D3 File Offset: 0x001B39D3
	public static void EnableManualDelivery(BionicBatteryMonitor.Instance smi)
	{
		smi.SetManualDeliveryEnableState(true);
	}

	// Token: 0x06004B3A RID: 19258 RVA: 0x001B57DC File Offset: 0x001B39DC
	public static void DisableManualDelivery(BionicBatteryMonitor.Instance smi)
	{
		smi.SetManualDeliveryEnableState(false);
	}

	// Token: 0x06004B3B RID: 19259 RVA: 0x001B57E5 File Offset: 0x001B39E5
	public static void StartReanimateWorkChore(BionicBatteryMonitor.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06004B3C RID: 19260 RVA: 0x001B57ED File Offset: 0x001B39ED
	public static void CancelReanimateWorkChore(BionicBatteryMonitor.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x06004B3D RID: 19261 RVA: 0x001B57F5 File Offset: 0x001B39F5
	public static void SetOffline(BionicBatteryMonitor.Instance smi)
	{
		smi.SetOnlineState(false);
	}

	// Token: 0x06004B3E RID: 19262 RVA: 0x001B57FE File Offset: 0x001B39FE
	public static void SetOnline(BionicBatteryMonitor.Instance smi)
	{
		smi.SetOnlineState(true);
	}

	// Token: 0x06004B3F RID: 19263 RVA: 0x001B5807 File Offset: 0x001B3A07
	public static void AutomaticallyDropAllDepletedElectrobanks(BionicBatteryMonitor.Instance smi)
	{
		smi.AutomaticallyDropAllDepletedElectrobanks();
	}

	// Token: 0x06004B40 RID: 19264 RVA: 0x001B580F File Offset: 0x001B3A0F
	public static void ReorganizeElectrobankStorage(BionicBatteryMonitor.Instance smi)
	{
		smi.ReorganizeElectrobanks();
	}

	// Token: 0x06004B41 RID: 19265 RVA: 0x001B5817 File Offset: 0x001B3A17
	public static void DropAllDischargedElectrobanks(BionicBatteryMonitor.Instance smi)
	{
		smi.DropAllDischargedElectrobanks();
	}

	// Token: 0x06004B42 RID: 19266 RVA: 0x001B581F File Offset: 0x001B3A1F
	public static void EnablePrioritizationComponent(BionicBatteryMonitor.Instance smi)
	{
		Prioritizable.AddRef(smi.gameObject);
		smi.gameObject.Trigger(1980521255, null);
	}

	// Token: 0x06004B43 RID: 19267 RVA: 0x001B583D File Offset: 0x001B3A3D
	public static void DisablePrioritizationComponent(BionicBatteryMonitor.Instance smi)
	{
		Prioritizable.RemoveRef(smi.gameObject);
		smi.gameObject.Trigger(1980521255, null);
	}

	// Token: 0x06004B44 RID: 19268 RVA: 0x001B585C File Offset: 0x001B3A5C
	public static void DischargeUpdate(BionicBatteryMonitor.Instance smi, float dt)
	{
		float joules = Mathf.Min(dt * smi.Wattage, smi.CurrentCharge);
		smi.ConsumePower(joules);
	}

	// Token: 0x06004B45 RID: 19269 RVA: 0x001B5884 File Offset: 0x001B3A84
	private static BionicBatteryMonitor.WattageModifier MakeDifficultyModifier(string id, string desc, float watts)
	{
		return new BionicBatteryMonitor.WattageModifier(id, desc + ": <b>" + ((watts >= 0f) ? "+</b>" : "-</b>") + GameUtil.GetFormattedWattage(Mathf.Abs(watts), GameUtil.WattageFormatterUnit.Automatic, true), watts, watts);
	}

	// Token: 0x06004B46 RID: 19270 RVA: 0x001B58BC File Offset: 0x001B3ABC
	public static BionicBatteryMonitor.WattageModifier GetDifficultyModifier()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.BionicWattage);
		BionicBatteryMonitor.WattageModifier result;
		if (BionicBatteryMonitor.difficultyWattages.TryGetValue(currentQualitySetting.id, out result))
		{
			return result;
		}
		return BionicBatteryMonitor.difficultyWattages["Default"];
	}

	// Token: 0x040031D0 RID: 12752
	public const int DEFAULT_ELECTROBANK_COUNT = 4;

	// Token: 0x040031D1 RID: 12753
	public const int BIONIC_SKILL_EXTRA_BATTERY_COUNT = 2;

	// Token: 0x040031D2 RID: 12754
	public const int MAX_ELECTROBANK_COUNT = 6;

	// Token: 0x040031D3 RID: 12755
	public const float DEFAULT_WATTS = 200f;

	// Token: 0x040031D4 RID: 12756
	public const string INITIAL_ELECTROBANK_TYPE_ID = "DisposableElectrobank_RawMetal";

	// Token: 0x040031D5 RID: 12757
	public static readonly string ChargedBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_charged_electrobank\">";

	// Token: 0x040031D6 RID: 12758
	public static readonly string DischargedBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_discharged_electrobank\">";

	// Token: 0x040031D7 RID: 12759
	public static readonly string CriticalBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_critical_electrobank\">";

	// Token: 0x040031D8 RID: 12760
	public static readonly string SavingBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_saving_electrobank\">";

	// Token: 0x040031D9 RID: 12761
	public static readonly string EmptySlotBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_empty_slot_electrobank\">";

	// Token: 0x040031DA RID: 12762
	private const string ANIM_NAME_REBOOT = "power_up";

	// Token: 0x040031DB RID: 12763
	public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State firstSpawn;

	// Token: 0x040031DC RID: 12764
	public BionicBatteryMonitor.OnlineStates online;

	// Token: 0x040031DD RID: 12765
	public BionicBatteryMonitor.OfflineStates offline;

	// Token: 0x040031DE RID: 12766
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Signal OnClosestAvailableElectrobankChangedSignal;

	// Token: 0x040031DF RID: 12767
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IntParameter ChargedElectrobankCount;

	// Token: 0x040031E0 RID: 12768
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IntParameter DepletedElectrobankCount;

	// Token: 0x040031E1 RID: 12769
	private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.BoolParameter InitialElectrobanksSpawned;

	// Token: 0x040031E2 RID: 12770
	private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.BoolParameter IsOnline;

	// Token: 0x040031E3 RID: 12771
	private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Signal OnElectrobankStorageChanged;

	// Token: 0x040031E4 RID: 12772
	private static readonly Dictionary<string, BionicBatteryMonitor.WattageModifier> difficultyWattages = new Dictionary<string, BionicBatteryMonitor.WattageModifier>
	{
		{
			"VeryHard",
			BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.VERYHARD.NAME, 200f)
		},
		{
			"Hard",
			BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.HARD.NAME, 100f)
		},
		{
			"Default",
			BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.DEFAULT.NAME, 0f)
		},
		{
			"Easy",
			BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.EASY.NAME, -100f)
		},
		{
			"VeryEasy",
			BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.VERYEASY.NAME, -150f)
		}
	};

	// Token: 0x02001A78 RID: 6776
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001A79 RID: 6777
	public struct WattageModifier
	{
		// Token: 0x0600A59A RID: 42394 RVA: 0x003B75F6 File Offset: 0x003B57F6
		public WattageModifier(string id, string name, float value, float potentialValue)
		{
			this.id = id;
			this.name = name;
			this.value = value;
			this.potentialValue = potentialValue;
		}

		// Token: 0x040081C4 RID: 33220
		public float potentialValue;

		// Token: 0x040081C5 RID: 33221
		public float value;

		// Token: 0x040081C6 RID: 33222
		public string name;

		// Token: 0x040081C7 RID: 33223
		public string id;
	}

	// Token: 0x02001A7A RID: 6778
	public class OnlineStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040081C8 RID: 33224
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State idle;

		// Token: 0x040081C9 RID: 33225
		public BionicBatteryMonitor.UpkeepStates upkeep;

		// Token: 0x040081CA RID: 33226
		public BionicBatteryMonitor.UpkeepStates critical;
	}

	// Token: 0x02001A7B RID: 6779
	public class UpkeepStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040081CB RID: 33227
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State seekElectrobank;
	}

	// Token: 0x02001A7C RID: 6780
	public class OfflineStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040081CC RID: 33228
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State waitingForBatteryDelivery;

		// Token: 0x040081CD RID: 33229
		public BionicBatteryMonitor.RebootWorkableState waitingForBatteryInstallation;

		// Token: 0x040081CE RID: 33230
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State reboot;
	}

	// Token: 0x02001A7D RID: 6781
	public class RebootWorkableState : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040081CF RID: 33231
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State waiting;

		// Token: 0x040081D0 RID: 33232
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State working;
	}

	// Token: 0x02001A7E RID: 6782
	public new class Instance : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.GameInstance
	{
		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x0600A59F RID: 42399 RVA: 0x003B7635 File Offset: 0x003B5835
		public float Wattage
		{
			get
			{
				return this.GetBaseWattage() + this.GetModifiersWattage();
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x0600A5A0 RID: 42400 RVA: 0x003B7644 File Offset: 0x003B5844
		public bool IsOnline
		{
			get
			{
				return base.sm.IsOnline.Get(this);
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x0600A5A1 RID: 42401 RVA: 0x003B7657 File Offset: 0x003B5857
		public bool InUpkeepTime
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x0600A5A2 RID: 42402 RVA: 0x003B7673 File Offset: 0x003B5873
		public bool HaveInitialElectrobanksBeenSpawned
		{
			get
			{
				return base.sm.InitialElectrobanksSpawned.Get(this);
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x0600A5A3 RID: 42403 RVA: 0x003B7686 File Offset: 0x003B5886
		public bool HasSpaceForNewElectrobank
		{
			get
			{
				return this.ElectrobankCount < this.ElectrobankCountCapacity;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x0600A5A4 RID: 42404 RVA: 0x003B7696 File Offset: 0x003B5896
		public int ElectrobankCount
		{
			get
			{
				return this.ChargedElectrobankCount + this.DepletedElectrobankCount;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x0600A5A5 RID: 42405 RVA: 0x003B76A5 File Offset: 0x003B58A5
		public int ChargedElectrobankCount
		{
			get
			{
				return base.sm.ChargedElectrobankCount.Get(this);
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x0600A5A6 RID: 42406 RVA: 0x003B76B8 File Offset: 0x003B58B8
		public int DepletedElectrobankCount
		{
			get
			{
				return base.sm.DepletedElectrobankCount.Get(this);
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x0600A5A7 RID: 42407 RVA: 0x003B76CB File Offset: 0x003B58CB
		public float CurrentCharge
		{
			get
			{
				return this.BionicBattery.value;
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x0600A5A8 RID: 42408 RVA: 0x003B76D8 File Offset: 0x003B58D8
		public int ElectrobankCountCapacity
		{
			get
			{
				return (int)base.gameObject.GetAttributes().Get(Db.Get().Attributes.BionicBatteryCountCapacity.Id).GetTotalValue();
			}
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x0600A5AA RID: 42410 RVA: 0x003B770D File Offset: 0x003B590D
		// (set) Token: 0x0600A5A9 RID: 42409 RVA: 0x003B7704 File Offset: 0x003B5904
		public ReanimateBionicWorkable reanimateWorkable { get; private set; }

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x0600A5AC RID: 42412 RVA: 0x003B771E File Offset: 0x003B591E
		// (set) Token: 0x0600A5AB RID: 42411 RVA: 0x003B7715 File Offset: 0x003B5915
		public List<BionicBatteryMonitor.WattageModifier> Modifiers { get; set; } = new List<BionicBatteryMonitor.WattageModifier>();

		// Token: 0x0600A5AD RID: 42413 RVA: 0x003B7728 File Offset: 0x003B5928
		public Instance(IStateMachineTarget master, BionicBatteryMonitor.Def def) : base(master, def)
		{
			this.storage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicBatteryStorage);
			this.reanimateWorkable = base.GetComponent<ReanimateBionicWorkable>();
			this.schedulable = base.GetComponent<Schedulable>();
			this.manualDelivery = base.GetComponent<ManualDeliveryKG>();
			this.selectable = base.GetComponent<KSelectable>();
			this.prefabID = base.GetComponent<KPrefabID>();
			this.dataHolder = base.GetComponent<MinionStorageDataHolder>();
			MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
			minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Combine(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			this.BionicBattery = Db.Get().Amounts.BionicInternalBattery.Lookup(base.gameObject);
			Storage storage = this.storage;
			storage.onDestroyItemsDropped = (Action<List<GameObject>>)Delegate.Combine(storage.onDestroyItemsDropped, new Action<List<GameObject>>(this.OnBatteriesDroppedFromDeath));
			Storage storage2 = this.storage;
			storage2.OnStorageChange = (Action<GameObject>)Delegate.Combine(storage2.OnStorageChange, new Action<GameObject>(this.OnElectrobankStorageChanged));
			base.Subscribe(540773776, new Action<object>(this.OnSkillsChanged));
			this.UpdateCapacityAmount();
			this.ApplyDifficultyModifiers();
		}

		// Token: 0x0600A5AE RID: 42414 RVA: 0x003B787E File Offset: 0x003B5A7E
		public override void StartSM()
		{
			this.closestElectrobankSensor = base.GetComponent<Sensors>().GetSensor<ClosestElectrobankSensor>();
			ClosestElectrobankSensor closestElectrobankSensor = this.closestElectrobankSensor;
			closestElectrobankSensor.OnItemChanged = (Action<Electrobank>)Delegate.Combine(closestElectrobankSensor.OnItemChanged, new Action<Electrobank>(this.OnClosestElectrobankChanged));
			base.StartSM();
		}

		// Token: 0x0600A5AF RID: 42415 RVA: 0x003B78C0 File Offset: 0x003B5AC0
		private void OnCopyMinionBegins(StoredMinionIdentity destination)
		{
			MinionStorageDataHolder.DataPackData data = new MinionStorageDataHolder.DataPackData
			{
				Bools = new bool[]
				{
					this.HaveInitialElectrobanksBeenSpawned,
					this.IsOnline
				}
			};
			this.dataHolder.UpdateData(data);
		}

		// Token: 0x0600A5B0 RID: 42416 RVA: 0x003B7900 File Offset: 0x003B5B00
		public override void PostParamsInitialized()
		{
			MinionStorageDataHolder.DataPack dataPack = this.dataHolder.GetDataPack<BionicBatteryMonitor.Instance>();
			if (dataPack != null && dataPack.IsStoringNewData)
			{
				MinionStorageDataHolder.DataPackData dataPackData = dataPack.ReadData();
				if (dataPackData != null)
				{
					bool value = (dataPackData.Bools != null && dataPackData.Bools.Length != 0) ? dataPackData.Bools[0] : this.HasSpaceForNewElectrobank;
					bool value2 = (dataPackData.Bools != null && dataPackData.Bools.Length > 1) ? dataPackData.Bools[1] : this.IsOnline;
					base.sm.InitialElectrobanksSpawned.Set(value, this, false);
					base.sm.IsOnline.Set(value2, this, false);
				}
			}
			this.RefreshCharge();
			base.PostParamsInitialized();
		}

		// Token: 0x0600A5B1 RID: 42417 RVA: 0x003B79AC File Offset: 0x003B5BAC
		public void DropAllDischargedElectrobanks()
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.EmptyPortableBattery, list);
			foreach (GameObject go in list)
			{
				this.storage.Drop(go, true);
			}
		}

		// Token: 0x0600A5B2 RID: 42418 RVA: 0x003B7A1C File Offset: 0x003B5C1C
		protected override void OnCleanUp()
		{
			if (this.dataHolder != null)
			{
				MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
				minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Remove(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			}
			this.UpdateNotifications();
			base.OnCleanUp();
		}

		// Token: 0x0600A5B3 RID: 42419 RVA: 0x003B7A6A File Offset: 0x003B5C6A
		private void OnSkillsChanged(object o)
		{
			if (this.storage.capacityKg != (float)this.ElectrobankCountCapacity)
			{
				this.OnBatteryCapacityChanged();
			}
		}

		// Token: 0x0600A5B4 RID: 42420 RVA: 0x003B7A88 File Offset: 0x003B5C88
		private void ApplyDifficultyModifiers()
		{
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.BionicWattage);
			BionicBatteryMonitor.WattageModifier item;
			if (BionicBatteryMonitor.difficultyWattages.TryGetValue(currentQualitySetting.id, out item))
			{
				this.Modifiers.Add(item);
			}
		}

		// Token: 0x0600A5B5 RID: 42421 RVA: 0x003B7AC8 File Offset: 0x003B5CC8
		private void UpdateCapacityAmount()
		{
			int num = this.ElectrobankCountCapacity - 4;
			this.BionicBattery.maxAttribute.ClearModifiers();
			this.BionicBattery.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.BionicInternalBattery.maxAttribute.Id, 120000f * (float)num, null, false, false, true));
		}

		// Token: 0x0600A5B6 RID: 42422 RVA: 0x003B7B28 File Offset: 0x003B5D28
		private void OnBatteryCapacityChanged()
		{
			this.UpdateCapacityAmount();
			for (int i = this.storage.Count - 1; i >= 0; i--)
			{
				if (this.storage.Count > this.ElectrobankCountCapacity)
				{
					GameObject gameObject = this.storage.items[i];
					Electrobank component = gameObject.GetComponent<Electrobank>();
					this.storage.Drop(gameObject, true);
					Vector3 position = gameObject.transform.position;
					position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
					gameObject.transform.position = position;
					if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
					{
						component.RemovePower(component.Charge, true);
					}
				}
			}
			base.smi.storage.capacityKg = (float)this.ElectrobankCountCapacity;
		}

		// Token: 0x0600A5B7 RID: 42423 RVA: 0x003B7BFC File Offset: 0x003B5DFC
		private void OnClosestElectrobankChanged(Electrobank newItem)
		{
			base.sm.OnClosestAvailableElectrobankChangedSignal.Trigger(this);
		}

		// Token: 0x0600A5B8 RID: 42424 RVA: 0x003B7C0F File Offset: 0x003B5E0F
		public float GetBaseWattage()
		{
			return 200f;
		}

		// Token: 0x0600A5B9 RID: 42425 RVA: 0x003B7C18 File Offset: 0x003B5E18
		public float GetModifiersWattage()
		{
			float num = 0f;
			foreach (BionicBatteryMonitor.WattageModifier wattageModifier in this.Modifiers)
			{
				num += wattageModifier.value;
			}
			return num;
		}

		// Token: 0x0600A5BA RID: 42426 RVA: 0x003B7C74 File Offset: 0x003B5E74
		private void OnElectrobankStorageChanged(object o)
		{
			this.ReorganizeElectrobanks();
			this.RefreshCharge();
			base.smi.sm.OnElectrobankStorageChanged.Trigger(this);
		}

		// Token: 0x0600A5BB RID: 42427 RVA: 0x003B7C98 File Offset: 0x003B5E98
		public void ReorganizeElectrobanks()
		{
			this.storage.items.Sort(delegate(GameObject b1, GameObject b2)
			{
				Electrobank component = b1.GetComponent<Electrobank>();
				Electrobank component2 = b2.GetComponent<Electrobank>();
				if (component == null)
				{
					return -1;
				}
				if (component2 == null)
				{
					return 1;
				}
				return component.Charge.CompareTo(component2.Charge);
			});
		}

		// Token: 0x0600A5BC RID: 42428 RVA: 0x003B7CCC File Offset: 0x003B5ECC
		public void CreateWorkableChore()
		{
			if (this.reanimateChore == null)
			{
				this.reanimateChore = new WorkChore<ReanimateBionicWorkable>(Db.Get().ChoreTypes.RescueIncapacitated, this.reanimateWorkable, null, true, null, null, null, true, null, false, false, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
				this.reanimateChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
			}
		}

		// Token: 0x0600A5BD RID: 42429 RVA: 0x003B7D28 File Offset: 0x003B5F28
		public void CancelWorkChore()
		{
			if (this.reanimateChore != null)
			{
				this.reanimateChore.Cancel("BionicBatteryMonitor.CancelChore");
				this.reanimateChore = null;
			}
		}

		// Token: 0x0600A5BE RID: 42430 RVA: 0x003B7D49 File Offset: 0x003B5F49
		public void SetOnlineState(bool online)
		{
			base.sm.IsOnline.Set(online, this, false);
			this.RefreshCharge();
		}

		// Token: 0x0600A5BF RID: 42431 RVA: 0x003B7D68 File Offset: 0x003B5F68
		public void SetManualDeliveryEnableState(bool enable)
		{
			if (!enable)
			{
				this.manualDelivery.capacity = 0f;
				this.manualDelivery.refillMass = 0f;
				this.manualDelivery.RequestedItemTag = null;
				this.manualDelivery.AbortDelivery("Manual delivery disabled");
				return;
			}
			Tag[] array = new Tag[GameTags.BionicIncompatibleBatteries.Count];
			GameTags.BionicIncompatibleBatteries.CopyTo(array, 0);
			base.smi.storage.capacityKg = (float)this.ElectrobankCountCapacity;
			base.smi.manualDelivery.capacity = 1f;
			base.smi.manualDelivery.refillMass = 1f;
			base.smi.manualDelivery.MinimumMass = 1f;
			this.manualDelivery.ForbiddenTags = array;
			this.manualDelivery.RequestedItemTag = GameTags.ChargedPortableBattery;
		}

		// Token: 0x0600A5C0 RID: 42432 RVA: 0x003B7E48 File Offset: 0x003B6048
		public GameObject GetFirstDischargedElectrobankInInventory()
		{
			return this.storage.FindFirst(GameTags.EmptyPortableBattery);
		}

		// Token: 0x0600A5C1 RID: 42433 RVA: 0x003B7E5A File Offset: 0x003B605A
		public Electrobank GetClosestElectrobank()
		{
			return this.closestElectrobankSensor.GetItem();
		}

		// Token: 0x0600A5C2 RID: 42434 RVA: 0x003B7E68 File Offset: 0x003B6068
		public void RefreshCharge()
		{
			ListPool<GameObject, BionicBatteryMonitor.Instance>.PooledList pooledList = ListPool<GameObject, BionicBatteryMonitor.Instance>.Allocate();
			ListPool<GameObject, BionicBatteryMonitor.Instance>.PooledList pooledList2 = ListPool<GameObject, BionicBatteryMonitor.Instance>.Allocate();
			this.storage.Find(GameTags.ChargedPortableBattery, pooledList);
			this.storage.Find(GameTags.EmptyPortableBattery, pooledList2);
			float num = 0f;
			if (this.IsOnline)
			{
				for (int i = 0; i < pooledList.Count; i++)
				{
					Electrobank component = pooledList[i].GetComponent<Electrobank>();
					num += component.Charge;
				}
			}
			this.BionicBattery.SetValue(num);
			base.sm.ChargedElectrobankCount.Set(pooledList.Count, this, false);
			pooledList.Recycle();
			base.sm.DepletedElectrobankCount.Set(pooledList2.Count, this, false);
			pooledList2.Recycle();
			this.UpdateNotifications();
		}

		// Token: 0x0600A5C3 RID: 42435 RVA: 0x003B7F30 File Offset: 0x003B6130
		public void ConsumePower(float joules)
		{
			ListPool<GameObject, BionicBatteryMonitor.Instance>.PooledList pooledList = ListPool<GameObject, BionicBatteryMonitor.Instance>.Allocate();
			this.storage.Find(GameTags.ChargedPortableBattery, pooledList);
			float num = joules;
			for (int i = 0; i < pooledList.Count; i++)
			{
				Electrobank component = pooledList[i].GetComponent<Electrobank>();
				float joules2 = Mathf.Min(component.Charge, num);
				float num2 = component.RemovePower(joules2, false);
				num -= num2;
				WorldResourceAmountTracker<ElectrobankTracker>.Get().RegisterAmountConsumed(component.ID, num2);
			}
			this.RefreshCharge();
			pooledList.Recycle();
		}

		// Token: 0x0600A5C4 RID: 42436 RVA: 0x003B7FB4 File Offset: 0x003B61B4
		public void DebugAddCharge(float joules)
		{
			float num = MathF.Min(joules, (float)this.ElectrobankCountCapacity * 120000f - this.CurrentCharge);
			ListPool<GameObject, BionicBatteryMonitor.Instance>.PooledList pooledList = ListPool<GameObject, BionicBatteryMonitor.Instance>.Allocate();
			this.storage.Find(GameTags.ChargedPortableBattery, pooledList);
			int num2 = 0;
			while (num > 0f && num2 < pooledList.Count)
			{
				Electrobank component = pooledList[num2].GetComponent<Electrobank>();
				float num3 = Mathf.Min(120000f - component.Charge, num);
				component.AddPower(num3);
				num -= num3;
				num2++;
			}
			if (num > 0f && pooledList.Count < this.ElectrobankCountCapacity)
			{
				int num4 = this.storage.items.Count - 1;
				while (num > 0f && num4 >= 0)
				{
					GameObject gameObject = this.storage.items[num4];
					if (!(gameObject == null))
					{
						Electrobank component2 = gameObject.GetComponent<Electrobank>();
						if (component2 == null && gameObject.HasTag(GameTags.EmptyPortableBattery))
						{
							this.storage.Drop(gameObject, true);
							GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_RawMetal"), base.transform.position);
							gameObject2.SetActive(true);
							component2 = gameObject2.GetComponent<Electrobank>();
							float joules2 = Mathf.Clamp(component2.Charge - num, 0f, float.MaxValue);
							component2.RemovePower(joules2, true);
							num -= component2.Charge;
							this.storage.Store(gameObject2, false, false, true, false);
						}
					}
					num4--;
				}
			}
			if (num > 0f && this.storage.items.Count < this.ElectrobankCountCapacity)
			{
				do
				{
					GameObject gameObject3 = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_RawMetal"), base.transform.position);
					gameObject3.SetActive(true);
					Electrobank component3 = gameObject3.GetComponent<Electrobank>();
					float joules3 = Mathf.Clamp(component3.Charge - num, 0f, float.MaxValue);
					component3.RemovePower(joules3, true);
					num -= component3.Charge;
					this.storage.Store(gameObject3, false, false, true, false);
				}
				while (num > 0f && this.storage.items.Count < this.ElectrobankCountCapacity && num > 0f);
			}
			this.RefreshCharge();
			pooledList.Recycle();
		}

		// Token: 0x0600A5C5 RID: 42437 RVA: 0x003B8224 File Offset: 0x003B6424
		private void UpdateNotifications()
		{
			this.criticalBatteryStatusItemGuid = this.selectable.ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicCriticalBattery, this.criticalBatteryStatusItemGuid, BionicBatteryMonitor.ChargeIsBelowNotificationThreshold(base.smi) && !this.prefabID.HasTag(GameTags.Incapacitated) && !this.prefabID.HasTag(GameTags.Dead), base.gameObject);
		}

		// Token: 0x0600A5C6 RID: 42438 RVA: 0x003B8294 File Offset: 0x003B6494
		public bool AddOrUpdateModifier(BionicBatteryMonitor.WattageModifier modifier, bool triggerCallbacks = true)
		{
			int num = this.Modifiers.FindIndex((BionicBatteryMonitor.WattageModifier mod) => mod.id == modifier.id);
			bool flag;
			if (num >= 0)
			{
				flag = (this.Modifiers[num].name != modifier.name || this.Modifiers[num].value != modifier.value || this.Modifiers[num].potentialValue != modifier.potentialValue);
				this.Modifiers[num] = modifier;
			}
			else
			{
				this.Modifiers.Add(modifier);
				flag = true;
			}
			if (flag)
			{
				this.Modifiers.Sort((BionicBatteryMonitor.WattageModifier a, BionicBatteryMonitor.WattageModifier b) => b.value.CompareTo(a.value));
			}
			if (triggerCallbacks)
			{
				base.BoxingTrigger<float>(1361471071, this.Wattage);
			}
			return flag;
		}

		// Token: 0x0600A5C7 RID: 42439 RVA: 0x003B839C File Offset: 0x003B659C
		public bool RemoveModifier(string modifierID, bool triggerCallbacks = true)
		{
			int num = this.Modifiers.FindIndex((BionicBatteryMonitor.WattageModifier mod) => mod.id == modifierID);
			if (num >= 0)
			{
				this.Modifiers.RemoveAt(num);
				if (triggerCallbacks)
				{
					base.BoxingTrigger<float>(1361471071, this.Wattage);
				}
				this.Modifiers.Sort((BionicBatteryMonitor.WattageModifier a, BionicBatteryMonitor.WattageModifier b) => b.value.CompareTo(a.value));
				return true;
			}
			return false;
		}

		// Token: 0x0600A5C8 RID: 42440 RVA: 0x003B8420 File Offset: 0x003B6620
		private void OnBatteriesDroppedFromDeath(List<GameObject> items)
		{
			if (items != null)
			{
				for (int i = 0; i < items.Count; i++)
				{
					Electrobank component = items[i].GetComponent<Electrobank>();
					if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
					{
						component.RemovePower(component.Charge, true);
					}
				}
			}
		}

		// Token: 0x0600A5C9 RID: 42441 RVA: 0x003B847C File Offset: 0x003B667C
		public void SpawnAndInstallInitialElectrobanks()
		{
			for (int i = 0; i < this.ElectrobankCountCapacity; i++)
			{
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_RawMetal"), base.transform.position);
				gameObject.SetActive(true);
				this.storage.Store(gameObject, false, false, true, false);
			}
			this.RefreshCharge();
			this.SetOnlineState(true);
			base.sm.InitialElectrobanksSpawned.Set(true, this, false);
		}

		// Token: 0x0600A5CA RID: 42442 RVA: 0x003B84F4 File Offset: 0x003B66F4
		public void AutomaticallyDropAllDepletedElectrobanks()
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.EmptyPortableBattery, list);
			for (int i = 0; i < list.Count; i++)
			{
				GameObject go = list[i];
				this.storage.Drop(go, true);
			}
		}

		// Token: 0x040081D1 RID: 33233
		public Storage storage;

		// Token: 0x040081D2 RID: 33234
		public KPrefabID prefabID;

		// Token: 0x040081D4 RID: 33236
		private Schedulable schedulable;

		// Token: 0x040081D5 RID: 33237
		private AmountInstance BionicBattery;

		// Token: 0x040081D6 RID: 33238
		private ManualDeliveryKG manualDelivery;

		// Token: 0x040081D7 RID: 33239
		private ClosestElectrobankSensor closestElectrobankSensor;

		// Token: 0x040081D8 RID: 33240
		private KSelectable selectable;

		// Token: 0x040081D9 RID: 33241
		private MinionStorageDataHolder dataHolder;

		// Token: 0x040081DA RID: 33242
		private Guid criticalBatteryStatusItemGuid;

		// Token: 0x040081DC RID: 33244
		private Chore reanimateChore;
	}
}
