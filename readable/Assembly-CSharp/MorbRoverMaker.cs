using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200035E RID: 862
public class MorbRoverMaker : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>
{
	// Token: 0x060011F8 RID: 4600 RVA: 0x00068CD8 File Offset: 0x00066ED8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.no_operational;
		this.root.Update(new Action<MorbRoverMaker.Instance, float>(MorbRoverMaker.GermsRequiredFeedbackUpdate), UpdateRate.SIM_1000ms, false);
		this.no_operational.Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.DisableManualDelivery(smi, "Disable manual delivery while no operational. in case players disabled the machine on purpose for this reason");
		}).TagTransition(GameTags.Operational, this.operational, false);
		this.operational.TagTransition(GameTags.Operational, this.no_operational, true).DefaultState(this.operational.covered);
		this.operational.covered.ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerDusty, null).ParamTransition<bool>(this.WasUncoverByDuplicant, this.operational.idle, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsTrue).Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.DisableManualDelivery(smi, "Machine can't ask for materials if it has not been investigated by a dupe");
		}).DefaultState(this.operational.covered.idle);
		this.operational.covered.idle.PlayAnim("dusty").ParamTransition<bool>(this.UncoverOrderRequested, this.operational.covered.careOrderGiven, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsTrue);
		this.operational.covered.careOrderGiven.PlayAnim("dusty").Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.StartWorkChore_RevealMachine)).Exit(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.CancelWorkChore_RevealMachine)).WorkableCompleteTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_RevealMachine(), this.operational.covered.complete).ParamTransition<bool>(this.UncoverOrderRequested, this.operational.covered.idle, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsFalse);
		this.operational.covered.complete.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.SetUncovered));
		this.operational.idle.Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.EnableManualDelivery(smi, "Operational and discovered");
		}).EnterTransition(this.operational.crafting, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting)).EnterTransition(this.operational.waitingForMorb, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.IsCraftingCompleted)).EventTransition(GameHashes.OnStorageChange, this.operational.crafting, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting)).PlayAnim("idle").ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null);
		this.operational.crafting.DefaultState(this.operational.crafting.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerCraftingBody, null);
		this.operational.crafting.conflict.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.ResetRoverBodyCraftingProgress)).GoTo(this.operational.idle);
		this.operational.crafting.pre.EventTransition(GameHashes.OnStorageChange, this.operational.crafting.conflict, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Not(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting))).PlayAnim("crafting_pre").OnAnimQueueComplete(this.operational.crafting.loop);
		this.operational.crafting.loop.EventTransition(GameHashes.OnStorageChange, this.operational.crafting.conflict, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Not(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting))).Update(new Action<MorbRoverMaker.Instance, float>(MorbRoverMaker.CraftingUpdate), UpdateRate.SIM_200ms, false).PlayAnim("crafting_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.CraftProgress, this.operational.crafting.pst, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsOne);
		this.operational.crafting.pst.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.ConsumeRoverBodyCraftingMaterials)).PlayAnim("crafting_pst").OnAnimQueueComplete(this.operational.waitingForMorb);
		this.operational.waitingForMorb.PlayAnim("crafting_complete").ParamTransition<long>(this.Germs, this.operational.doctor, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Parameter<long>.Callback(MorbRoverMaker.HasEnoughGerms)).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null);
		this.operational.doctor.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.StartWorkChore_ReleaseRover)).Exit(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.CancelWorkChore_ReleaseRover)).WorkableCompleteTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.finish).DefaultState(this.operational.doctor.needed);
		this.operational.doctor.needed.PlayAnim("waiting", KAnim.PlayMode.Loop).WorkableStartTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.doctor.working).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerReadyForDoctor, null);
		this.operational.doctor.working.WorkableStopTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.doctor.needed);
		this.operational.finish.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.SpawnRover)).GoTo(this.operational.idle);
	}

	// Token: 0x060011F9 RID: 4601 RVA: 0x000692A1 File Offset: 0x000674A1
	public static bool ShouldBeCrafting(MorbRoverMaker.Instance smi)
	{
		return smi.HasMaterialsForRover && smi.RoverDevelopment_Progress < 1f;
	}

	// Token: 0x060011FA RID: 4602 RVA: 0x000692BA File Offset: 0x000674BA
	public static bool IsCraftingCompleted(MorbRoverMaker.Instance smi)
	{
		return smi.RoverDevelopment_Progress == 1f;
	}

	// Token: 0x060011FB RID: 4603 RVA: 0x000692C9 File Offset: 0x000674C9
	public static bool HasEnoughGerms(MorbRoverMaker.Instance smi, long germCount)
	{
		return germCount >= smi.def.GERMS_PER_ROVER;
	}

	// Token: 0x060011FC RID: 4604 RVA: 0x000692DC File Offset: 0x000674DC
	public static void StartWorkChore_ReleaseRover(MorbRoverMaker.Instance smi)
	{
		smi.CreateWorkChore_ReleaseRover();
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x000692E4 File Offset: 0x000674E4
	public static void CancelWorkChore_ReleaseRover(MorbRoverMaker.Instance smi)
	{
		smi.CancelWorkChore_ReleaseRover();
	}

	// Token: 0x060011FE RID: 4606 RVA: 0x000692EC File Offset: 0x000674EC
	public static void StartWorkChore_RevealMachine(MorbRoverMaker.Instance smi)
	{
		smi.CreateWorkChore_RevealMachine();
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x000692F4 File Offset: 0x000674F4
	public static void CancelWorkChore_RevealMachine(MorbRoverMaker.Instance smi)
	{
		smi.CancelWorkChore_RevealMachine();
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x000692FC File Offset: 0x000674FC
	public static void SetUncovered(MorbRoverMaker.Instance smi)
	{
		smi.Uncover();
	}

	// Token: 0x06001201 RID: 4609 RVA: 0x00069304 File Offset: 0x00067504
	public static void SpawnRover(MorbRoverMaker.Instance smi)
	{
		smi.SpawnRover();
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x0006930C File Offset: 0x0006750C
	public static void EnableManualDelivery(MorbRoverMaker.Instance smi, string reason)
	{
		smi.EnableManualDelivery(reason);
	}

	// Token: 0x06001203 RID: 4611 RVA: 0x00069315 File Offset: 0x00067515
	public static void DisableManualDelivery(MorbRoverMaker.Instance smi, string reason)
	{
		smi.DisableManualDelivery(reason);
	}

	// Token: 0x06001204 RID: 4612 RVA: 0x0006931E File Offset: 0x0006751E
	public static void ConsumeRoverBodyCraftingMaterials(MorbRoverMaker.Instance smi)
	{
		smi.ConsumeRoverBodyCraftingMaterials();
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x00069326 File Offset: 0x00067526
	public static void ResetRoverBodyCraftingProgress(MorbRoverMaker.Instance smi)
	{
		smi.SetRoverDevelopmentProgress(0f);
	}

	// Token: 0x06001206 RID: 4614 RVA: 0x00069334 File Offset: 0x00067534
	public static void CraftingUpdate(MorbRoverMaker.Instance smi, float dt)
	{
		float roverDevelopmentProgress = Mathf.Clamp((smi.RoverDevelopment_Progress * smi.def.ROVER_CRAFTING_DURATION + dt) / smi.def.ROVER_CRAFTING_DURATION, 0f, 1f);
		smi.SetRoverDevelopmentProgress(roverDevelopmentProgress);
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x00069378 File Offset: 0x00067578
	public static void GermsRequiredFeedbackUpdate(MorbRoverMaker.Instance smi, float dt)
	{
		if (GameClock.Instance.GetTime() - smi.lastTimeGermsAdded > smi.def.FEEDBACK_NO_GERMS_DETECTED_TIMEOUT & smi.MorbDevelopment_Progress < 1f & !smi.IsInsideState(smi.sm.operational.doctor) & smi.HasBeenRevealed)
		{
			smi.ShowGermRequiredStatusItemAlert();
			return;
		}
		smi.HideGermRequiredStatusItemAlert();
	}

	// Token: 0x04000B3D RID: 2877
	private const string ROBOT_PROGRESS_METER_TARGET_NAME = "meter_robot_target";

	// Token: 0x04000B3E RID: 2878
	private const string ROBOT_PROGRESS_METER_ANIMATION_NAME = "meter_robot";

	// Token: 0x04000B3F RID: 2879
	private const string COVERED_IDLE_ANIM_NAME = "dusty";

	// Token: 0x04000B40 RID: 2880
	private const string IDLE_ANIM_NAME = "idle";

	// Token: 0x04000B41 RID: 2881
	private const string CRAFT_PRE_ANIM_NAME = "crafting_pre";

	// Token: 0x04000B42 RID: 2882
	private const string CRAFT_LOOP_ANIM_NAME = "crafting_loop";

	// Token: 0x04000B43 RID: 2883
	private const string CRAFT_PST_ANIM_NAME = "crafting_pst";

	// Token: 0x04000B44 RID: 2884
	private const string CRAFT_COMPLETED_ANIM_NAME = "crafting_complete";

	// Token: 0x04000B45 RID: 2885
	private const string WAITING_FOR_DOCTOR_ANIM_NAME = "waiting";

	// Token: 0x04000B46 RID: 2886
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.BoolParameter UncoverOrderRequested;

	// Token: 0x04000B47 RID: 2887
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.BoolParameter WasUncoverByDuplicant;

	// Token: 0x04000B48 RID: 2888
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.LongParameter Germs;

	// Token: 0x04000B49 RID: 2889
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.FloatParameter CraftProgress;

	// Token: 0x04000B4A RID: 2890
	public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State no_operational;

	// Token: 0x04000B4B RID: 2891
	public MorbRoverMaker.OperationalStates operational;

	// Token: 0x0200123E RID: 4670
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600875A RID: 34650 RVA: 0x0034B438 File Offset: 0x00349638
		public float GetConduitMaxPackageMass()
		{
			ConduitType germ_INTAKE_CONDUIT_TYPE = this.GERM_INTAKE_CONDUIT_TYPE;
			if (germ_INTAKE_CONDUIT_TYPE == ConduitType.Gas)
			{
				return 1f;
			}
			if (germ_INTAKE_CONDUIT_TYPE != ConduitType.Liquid)
			{
				return 1f;
			}
			return 10f;
		}

		// Token: 0x04006736 RID: 26422
		public float FEEDBACK_NO_GERMS_DETECTED_TIMEOUT = 2f;

		// Token: 0x04006737 RID: 26423
		public Tag ROVER_PREFAB_ID;

		// Token: 0x04006738 RID: 26424
		public float INITIAL_MORB_DEVELOPMENT_PERCENTAGE;

		// Token: 0x04006739 RID: 26425
		public float ROVER_CRAFTING_DURATION;

		// Token: 0x0400673A RID: 26426
		public float METAL_PER_ROVER;

		// Token: 0x0400673B RID: 26427
		public long GERMS_PER_ROVER;

		// Token: 0x0400673C RID: 26428
		public int MAX_GERMS_TAKEN_PER_PACKAGE;

		// Token: 0x0400673D RID: 26429
		public int GERM_TYPE;

		// Token: 0x0400673E RID: 26430
		public SimHashes ROVER_MATERIAL;

		// Token: 0x0400673F RID: 26431
		public ConduitType GERM_INTAKE_CONDUIT_TYPE;
	}

	// Token: 0x0200123F RID: 4671
	public class CoverStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04006740 RID: 26432
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State idle;

		// Token: 0x04006741 RID: 26433
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State careOrderGiven;

		// Token: 0x04006742 RID: 26434
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State complete;
	}

	// Token: 0x02001240 RID: 4672
	public class OperationalStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04006743 RID: 26435
		public MorbRoverMaker.CoverStates covered;

		// Token: 0x04006744 RID: 26436
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State idle;

		// Token: 0x04006745 RID: 26437
		public MorbRoverMaker.CraftingStates crafting;

		// Token: 0x04006746 RID: 26438
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State waitingForMorb;

		// Token: 0x04006747 RID: 26439
		public MorbRoverMaker.DoctorStates doctor;

		// Token: 0x04006748 RID: 26440
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State finish;
	}

	// Token: 0x02001241 RID: 4673
	public class DoctorStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04006749 RID: 26441
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State needed;

		// Token: 0x0400674A RID: 26442
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State working;
	}

	// Token: 0x02001242 RID: 4674
	public class CraftingStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x0400674B RID: 26443
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State conflict;

		// Token: 0x0400674C RID: 26444
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State pre;

		// Token: 0x0400674D RID: 26445
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State loop;

		// Token: 0x0400674E RID: 26446
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State pst;
	}

	// Token: 0x02001243 RID: 4675
	public new class Instance : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06008760 RID: 34656 RVA: 0x0034B49A File Offset: 0x0034969A
		public long MorbDevelopment_GermsCollected
		{
			get
			{
				return base.sm.Germs.Get(base.smi);
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06008761 RID: 34657 RVA: 0x0034B4B2 File Offset: 0x003496B2
		public long MorbDevelopment_RemainingGerms
		{
			get
			{
				return base.def.GERMS_PER_ROVER - this.MorbDevelopment_GermsCollected;
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06008762 RID: 34658 RVA: 0x0034B4C6 File Offset: 0x003496C6
		public float MorbDevelopment_Progress
		{
			get
			{
				return Mathf.Clamp((float)this.MorbDevelopment_GermsCollected / (float)base.def.GERMS_PER_ROVER, 0f, 1f);
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06008763 RID: 34659 RVA: 0x0034B4EB File Offset: 0x003496EB
		public bool HasMaterialsForRover
		{
			get
			{
				return this.storage.GetMassAvailable(base.def.ROVER_MATERIAL) >= base.def.METAL_PER_ROVER;
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06008764 RID: 34660 RVA: 0x0034B513 File Offset: 0x00349713
		public float RoverDevelopment_Progress
		{
			get
			{
				return base.sm.CraftProgress.Get(base.smi);
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06008765 RID: 34661 RVA: 0x0034B52B File Offset: 0x0034972B
		public bool HasBeenRevealed
		{
			get
			{
				return base.sm.WasUncoverByDuplicant.Get(base.smi);
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06008766 RID: 34662 RVA: 0x0034B543 File Offset: 0x00349743
		public bool CanPumpGerms
		{
			get
			{
				return this.operational && this.MorbDevelopment_Progress < 1f && this.HasBeenRevealed;
			}
		}

		// Token: 0x06008767 RID: 34663 RVA: 0x0034B567 File Offset: 0x00349767
		public Workable GetWorkable_RevealMachine()
		{
			return this.workable_reveal;
		}

		// Token: 0x06008768 RID: 34664 RVA: 0x0034B56F File Offset: 0x0034976F
		public Workable GetWorkable_ReleaseRover()
		{
			return this.workable_release;
		}

		// Token: 0x06008769 RID: 34665 RVA: 0x0034B578 File Offset: 0x00349778
		public void ShowGermRequiredStatusItemAlert()
		{
			if (this.germsRequiredAlertStatusItemHandle == default(Guid))
			{
				this.germsRequiredAlertStatusItemHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerNoGermsConsumedAlert, base.smi);
			}
		}

		// Token: 0x0600876A RID: 34666 RVA: 0x0034B5C4 File Offset: 0x003497C4
		public void HideGermRequiredStatusItemAlert()
		{
			if (this.germsRequiredAlertStatusItemHandle != default(Guid))
			{
				this.selectable.RemoveStatusItem(this.germsRequiredAlertStatusItemHandle, false);
				this.germsRequiredAlertStatusItemHandle = default(Guid);
			}
		}

		// Token: 0x0600876B RID: 34667 RVA: 0x0034B608 File Offset: 0x00349808
		public Instance(IStateMachineTarget master, MorbRoverMaker.Def def) : base(master, def)
		{
			this.RobotProgressMeter = new MeterController(this.buildingAnimCtr, "meter_robot_target", "meter_robot", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
		}

		// Token: 0x0600876C RID: 34668 RVA: 0x0034B670 File Offset: 0x00349870
		public override void StartSM()
		{
			Building component = base.GetComponent<Building>();
			this.inputCell = component.GetUtilityInputCell();
			this.outputCell = component.GetUtilityOutputCell();
			base.StartSM();
			if (!this.HasBeenRevealed)
			{
				base.sm.Germs.Set(0L, base.smi, false);
				this.AddGerms((long)((float)base.def.GERMS_PER_ROVER * base.def.INITIAL_MORB_DEVELOPMENT_PERCENTAGE), false);
			}
			Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE).AddConduitUpdater(new Action<float>(this.Flow), ConduitFlowPriority.Default);
			this.UpdateMeters();
		}

		// Token: 0x0600876D RID: 34669 RVA: 0x0034B70C File Offset: 0x0034990C
		public void AddGerms(long amount, bool playAnimations = true)
		{
			long value = this.MorbDevelopment_GermsCollected + amount;
			base.sm.Germs.Set(value, base.smi, false);
			this.UpdateMeters();
			if (amount > 0L)
			{
				if (playAnimations)
				{
					this.capsule.PlayPumpGermsAnimation();
				}
				Action<long> germsAdded = this.GermsAdded;
				if (germsAdded != null)
				{
					germsAdded(amount);
				}
				this.lastTimeGermsAdded = GameClock.Instance.GetTime();
			}
		}

		// Token: 0x0600876E RID: 34670 RVA: 0x0034B778 File Offset: 0x00349978
		public long RemoveGerms(long amount)
		{
			long num = amount.Min(this.MorbDevelopment_GermsCollected);
			long value = this.MorbDevelopment_GermsCollected - num;
			base.sm.Germs.Set(value, base.smi, false);
			this.UpdateMeters();
			return num;
		}

		// Token: 0x0600876F RID: 34671 RVA: 0x0034B7BB File Offset: 0x003499BB
		public void EnableManualDelivery(string reason)
		{
			this.manualDelivery.Pause(false, reason);
		}

		// Token: 0x06008770 RID: 34672 RVA: 0x0034B7CA File Offset: 0x003499CA
		public void DisableManualDelivery(string reason)
		{
			this.manualDelivery.Pause(true, reason);
		}

		// Token: 0x06008771 RID: 34673 RVA: 0x0034B7D9 File Offset: 0x003499D9
		public void SetRoverDevelopmentProgress(float value)
		{
			base.sm.CraftProgress.Set(value, base.smi, false);
			this.UpdateMeters();
		}

		// Token: 0x06008772 RID: 34674 RVA: 0x0034B7FC File Offset: 0x003499FC
		public void UpdateMeters()
		{
			this.RobotProgressMeter.SetPositionPercent(this.RoverDevelopment_Progress);
			this.capsule.SetMorbDevelopmentProgress(this.MorbDevelopment_Progress);
			this.capsule.SetGermMeterProgress(this.HasBeenRevealed ? this.MorbDevelopment_Progress : 0f);
		}

		// Token: 0x06008773 RID: 34675 RVA: 0x0034B84B File Offset: 0x00349A4B
		public void Uncover()
		{
			base.sm.WasUncoverByDuplicant.Set(true, base.smi, false);
			System.Action onUncovered = this.OnUncovered;
			if (onUncovered == null)
			{
				return;
			}
			onUncovered();
		}

		// Token: 0x06008774 RID: 34676 RVA: 0x0034B878 File Offset: 0x00349A78
		public void CreateWorkChore_ReleaseRover()
		{
			if (this.workChore_releaseRover == null)
			{
				this.workChore_releaseRover = new WorkChore<MorbRoverMakerWorkable>(Db.Get().ChoreTypes.Doctor, this.workable_release, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06008775 RID: 34677 RVA: 0x0034B8BE File Offset: 0x00349ABE
		public void CancelWorkChore_ReleaseRover()
		{
			if (this.workChore_releaseRover != null)
			{
				this.workChore_releaseRover.Cancel("MorbRoverMaker.CancelWorkChore_ReleaseRover");
				this.workChore_releaseRover = null;
			}
		}

		// Token: 0x06008776 RID: 34678 RVA: 0x0034B8E0 File Offset: 0x00349AE0
		public void CreateWorkChore_RevealMachine()
		{
			if (this.workChore_revealMachine == null)
			{
				this.workChore_revealMachine = new WorkChore<MorbRoverMakerRevealWorkable>(Db.Get().ChoreTypes.Repair, this.workable_reveal, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06008777 RID: 34679 RVA: 0x0034B926 File Offset: 0x00349B26
		public void CancelWorkChore_RevealMachine()
		{
			if (this.workChore_revealMachine != null)
			{
				this.workChore_revealMachine.Cancel("MorbRoverMaker.CancelWorkChore_RevealMachine");
				this.workChore_revealMachine = null;
			}
		}

		// Token: 0x06008778 RID: 34680 RVA: 0x0034B948 File Offset: 0x00349B48
		public void ConsumeRoverBodyCraftingMaterials()
		{
			float num = 0f;
			this.storage.ConsumeAndGetDisease(base.def.ROVER_MATERIAL.CreateTag(), base.def.METAL_PER_ROVER, out num, out this.lastastMaterialsConsumedDiseases, out this.lastastMaterialsConsumedTemp);
		}

		// Token: 0x06008779 RID: 34681 RVA: 0x0034B990 File Offset: 0x00349B90
		public void SpawnRover()
		{
			if (this.RoverDevelopment_Progress == 1f)
			{
				this.RemoveGerms(base.def.GERMS_PER_ROVER);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(base.def.ROVER_PREFAB_ID), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0);
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (this.lastastMaterialsConsumedDiseases.idx != 255)
				{
					component.AddDisease(this.lastastMaterialsConsumedDiseases.idx, this.lastastMaterialsConsumedDiseases.count, "From the materials provided for its creation");
				}
				if (this.lastastMaterialsConsumedTemp > 0f)
				{
					component.SetMassTemperature(component.Mass, this.lastastMaterialsConsumedTemp);
				}
				gameObject.SetActive(true);
				this.SetRoverDevelopmentProgress(0f);
				Action<GameObject> onRoverSpawned = this.OnRoverSpawned;
				if (onRoverSpawned == null)
				{
					return;
				}
				onRoverSpawned(gameObject);
			}
		}

		// Token: 0x0600877A RID: 34682 RVA: 0x0034BA68 File Offset: 0x00349C68
		private void Flow(float dt)
		{
			if (this.CanPumpGerms)
			{
				ConduitFlow flowManager = Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE);
				int num = 0;
				if (flowManager.HasConduit(this.inputCell) && flowManager.HasConduit(this.outputCell))
				{
					ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
					ConduitFlow.ConduitContents contents2 = flowManager.GetContents(this.outputCell);
					float num2 = Mathf.Min(contents.mass, base.def.GetConduitMaxPackageMass() * dt);
					if (flowManager.CanMergeContents(contents, contents2, num2))
					{
						float amountAllowedForMerging = flowManager.GetAmountAllowedForMerging(contents, contents2, num2);
						if (amountAllowedForMerging > 0f)
						{
							ConduitFlow conduitFlow = (base.def.GERM_INTAKE_CONDUIT_TYPE == ConduitType.Liquid) ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow;
							int num3 = contents.diseaseCount;
							if (contents.diseaseIdx != 255 && (int)contents.diseaseIdx == base.def.GERM_TYPE)
							{
								num = (int)this.MorbDevelopment_RemainingGerms.Min((long)base.def.MAX_GERMS_TAKEN_PER_PACKAGE).Min((long)contents.diseaseCount);
								num3 -= num;
							}
							float num4 = conduitFlow.AddElement(this.outputCell, contents.element, amountAllowedForMerging, contents.temperature, contents.diseaseIdx, num3);
							if (amountAllowedForMerging != num4)
							{
								global::Debug.Log("[Morb Rover Maker] Mass Differs By: " + (amountAllowedForMerging - num4).ToString());
							}
							flowManager.RemoveElement(this.inputCell, num4);
						}
					}
				}
				if (num > 0)
				{
					this.AddGerms((long)num, true);
				}
			}
		}

		// Token: 0x0600877B RID: 34683 RVA: 0x0034BBEA File Offset: 0x00349DEA
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE).RemoveConduitUpdater(new Action<float>(this.Flow));
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x0600877C RID: 34684 RVA: 0x0034BC13 File Offset: 0x00349E13
		public string SidescreenButtonText
		{
			get
			{
				return this.HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY : (base.sm.UncoverOrderRequested.Get(base.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN);
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x0600877D RID: 34685 RVA: 0x0034BC4D File Offset: 0x00349E4D
		public string SidescreenButtonTooltip
		{
			get
			{
				return this.HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY_TOOLTIP : (base.sm.UncoverOrderRequested.Get(base.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN_TOOLTIP : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN_TOOLTIP);
			}
		}

		// Token: 0x0600877E RID: 34686 RVA: 0x0034BC87 File Offset: 0x00349E87
		public bool SidescreenEnabled()
		{
			return true;
		}

		// Token: 0x0600877F RID: 34687 RVA: 0x0034BC8A File Offset: 0x00349E8A
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x06008780 RID: 34688 RVA: 0x0034BC8D File Offset: 0x00349E8D
		public int HorizontalGroupID()
		{
			return 0;
		}

		// Token: 0x06008781 RID: 34689 RVA: 0x0034BC90 File Offset: 0x00349E90
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06008782 RID: 34690 RVA: 0x0034BC94 File Offset: 0x00349E94
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06008783 RID: 34691 RVA: 0x0034BC9C File Offset: 0x00349E9C
		public void OnSidescreenButtonPressed()
		{
			if (this.HasBeenRevealed)
			{
				this.storage.DropAll(false, false, default(Vector3), true, null);
				return;
			}
			bool flag = base.smi.sm.UncoverOrderRequested.Get(base.smi);
			base.smi.sm.UncoverOrderRequested.Set(!flag, base.smi, false);
		}

		// Token: 0x0400674F RID: 26447
		public Action<long> GermsAdded;

		// Token: 0x04006750 RID: 26448
		public System.Action OnUncovered;

		// Token: 0x04006751 RID: 26449
		public Action<GameObject> OnRoverSpawned;

		// Token: 0x04006752 RID: 26450
		[MyCmpGet]
		private MorbRoverMakerRevealWorkable workable_reveal;

		// Token: 0x04006753 RID: 26451
		[MyCmpGet]
		private MorbRoverMakerWorkable workable_release;

		// Token: 0x04006754 RID: 26452
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04006755 RID: 26453
		[MyCmpGet]
		private KBatchedAnimController buildingAnimCtr;

		// Token: 0x04006756 RID: 26454
		[MyCmpGet]
		private ManualDeliveryKG manualDelivery;

		// Token: 0x04006757 RID: 26455
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04006758 RID: 26456
		[MyCmpGet]
		private MorbRoverMaker_Capsule capsule;

		// Token: 0x04006759 RID: 26457
		[MyCmpGet]
		private KSelectable selectable;

		// Token: 0x0400675A RID: 26458
		private MeterController RobotProgressMeter;

		// Token: 0x0400675B RID: 26459
		private int inputCell = -1;

		// Token: 0x0400675C RID: 26460
		private int outputCell = -1;

		// Token: 0x0400675D RID: 26461
		private Chore workChore_revealMachine;

		// Token: 0x0400675E RID: 26462
		private Chore workChore_releaseRover;

		// Token: 0x0400675F RID: 26463
		[Serialize]
		private float lastastMaterialsConsumedTemp = -1f;

		// Token: 0x04006760 RID: 26464
		[Serialize]
		private SimUtil.DiseaseInfo lastastMaterialsConsumedDiseases = SimUtil.DiseaseInfo.Invalid;

		// Token: 0x04006761 RID: 26465
		public float lastTimeGermsAdded = -1f;

		// Token: 0x04006762 RID: 26466
		private Guid germsRequiredAlertStatusItemHandle;
	}
}
