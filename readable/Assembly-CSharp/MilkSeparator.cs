using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007BD RID: 1981
public class MilkSeparator : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>
{
	// Token: 0x06003469 RID: 13417 RVA: 0x00129088 File Offset: 0x00127288
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.noOperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.RefreshMeters));
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).PlayAnim("on").DefaultState(this.operational.idle);
		this.operational.idle.EventTransition(GameHashes.OnStorageChange, this.operational.working.pre, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanBeginSeparate)).EnterTransition(this.operational.full, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.RequiresEmptying));
		this.operational.working.pre.QueueAnim("separating_pre", false, null).OnAnimQueueComplete(this.operational.working.work);
		this.operational.working.work.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.BeginSeparation)).PlayAnim("separating_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.operational.working.post, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanNOTKeepSeparating)).Exit(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.EndSeparation));
		this.operational.working.post.QueueAnim("separating_pst", false, null).OnAnimQueueComplete(this.operational.idle);
		this.operational.full.PlayAnim("ready").ToggleRecurringChore(new Func<MilkSeparator.Instance, Chore>(MilkSeparator.CreateEmptyChore), null).WorkableCompleteTransition((MilkSeparator.Instance smi) => smi.workable, this.operational.emptyComplete).ToggleStatusItem(Db.Get().BuildingStatusItems.MilkSeparatorNeedsEmptying, null);
		this.operational.emptyComplete.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.DropMilkFat)).ScheduleActionNextFrame("AfterMilkFatDrop", delegate(MilkSeparator.Instance smi)
		{
			smi.GoTo(this.operational.idle);
		});
	}

	// Token: 0x0600346A RID: 13418 RVA: 0x001292C5 File Offset: 0x001274C5
	public static void BeginSeparation(MilkSeparator.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x0600346B RID: 13419 RVA: 0x001292D4 File Offset: 0x001274D4
	public static void EndSeparation(MilkSeparator.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x0600346C RID: 13420 RVA: 0x001292E3 File Offset: 0x001274E3
	public static bool CanBeginSeparate(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached && smi.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x0600346D RID: 13421 RVA: 0x001292FB File Offset: 0x001274FB
	public static bool CanKeepSeparating(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached && smi.elementConverter.CanConvertAtAll();
	}

	// Token: 0x0600346E RID: 13422 RVA: 0x00129312 File Offset: 0x00127512
	public static bool CanNOTKeepSeparating(MilkSeparator.Instance smi)
	{
		return !MilkSeparator.CanKeepSeparating(smi);
	}

	// Token: 0x0600346F RID: 13423 RVA: 0x0012931D File Offset: 0x0012751D
	public static bool RequiresEmptying(MilkSeparator.Instance smi)
	{
		return smi.MilkFatLimitReached;
	}

	// Token: 0x06003470 RID: 13424 RVA: 0x00129325 File Offset: 0x00127525
	public static bool ThereIsCapacityForMilkFat(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached;
	}

	// Token: 0x06003471 RID: 13425 RVA: 0x00129330 File Offset: 0x00127530
	public static void DropMilkFat(MilkSeparator.Instance smi)
	{
		smi.DropMilkFat();
	}

	// Token: 0x06003472 RID: 13426 RVA: 0x00129338 File Offset: 0x00127538
	public static void RefreshMeters(MilkSeparator.Instance smi)
	{
		smi.RefreshMeters();
	}

	// Token: 0x06003473 RID: 13427 RVA: 0x00129340 File Offset: 0x00127540
	private static Chore CreateEmptyChore(MilkSeparator.Instance smi)
	{
		WorkChore<EmptyMilkSeparatorWorkable> workChore = new WorkChore<EmptyMilkSeparatorWorkable>(Db.Get().ChoreTypes.EmptyStorage, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		return workChore;
	}

	// Token: 0x04001F9D RID: 8093
	public const string WORK_PRE_ANIM_NAME = "separating_pre";

	// Token: 0x04001F9E RID: 8094
	public const string WORK_ANIM_NAME = "separating_loop";

	// Token: 0x04001F9F RID: 8095
	public const string WORK_POST_ANIM_NAME = "separating_pst";

	// Token: 0x04001FA0 RID: 8096
	public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State noOperational;

	// Token: 0x04001FA1 RID: 8097
	public MilkSeparator.OperationalStates operational;

	// Token: 0x020016F5 RID: 5877
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009951 RID: 39249 RVA: 0x0038C476 File Offset: 0x0038A676
		public Def()
		{
			this.MILK_FAT_TAG = ElementLoader.FindElementByHash(SimHashes.MilkFat).tag;
			this.MILK_TAG = ElementLoader.FindElementByHash(SimHashes.Milk).tag;
		}

		// Token: 0x04007649 RID: 30281
		public float MILK_FAT_CAPACITY = 100f;

		// Token: 0x0400764A RID: 30282
		public Tag MILK_TAG;

		// Token: 0x0400764B RID: 30283
		public Tag MILK_FAT_TAG;
	}

	// Token: 0x020016F6 RID: 5878
	public class WorkingStates : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
	{
		// Token: 0x0400764C RID: 30284
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State pre;

		// Token: 0x0400764D RID: 30285
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State work;

		// Token: 0x0400764E RID: 30286
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State post;
	}

	// Token: 0x020016F7 RID: 5879
	public class OperationalStates : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
	{
		// Token: 0x0400764F RID: 30287
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State idle;

		// Token: 0x04007650 RID: 30288
		public MilkSeparator.WorkingStates working;

		// Token: 0x04007651 RID: 30289
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State full;

		// Token: 0x04007652 RID: 30290
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State emptyComplete;
	}

	// Token: 0x020016F8 RID: 5880
	public new class Instance : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.GameInstance
	{
		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06009954 RID: 39252 RVA: 0x0038C4C3 File Offset: 0x0038A6C3
		public float MilkFatStored
		{
			get
			{
				return this.storage.GetAmountAvailable(base.def.MILK_FAT_TAG);
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06009955 RID: 39253 RVA: 0x0038C4DB File Offset: 0x0038A6DB
		public float MilkFatStoragePercentage
		{
			get
			{
				return Mathf.Clamp(this.MilkFatStored / base.def.MILK_FAT_CAPACITY, 0f, 1f);
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06009956 RID: 39254 RVA: 0x0038C4FE File Offset: 0x0038A6FE
		public bool MilkFatLimitReached
		{
			get
			{
				return this.MilkFatStored >= base.def.MILK_FAT_CAPACITY;
			}
		}

		// Token: 0x06009957 RID: 39255 RVA: 0x0038C518 File Offset: 0x0038A718
		public Instance(IStateMachineTarget master, MilkSeparator.Def def) : base(master, def)
		{
			KAnimControllerBase component = base.GetComponent<KBatchedAnimController>();
			this.fatMeter = new MeterController(component, "meter_target_1", "meter_fat", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target_1"
			});
		}

		// Token: 0x06009958 RID: 39256 RVA: 0x0038C55B File Offset: 0x0038A75B
		public override void StartSM()
		{
			base.StartSM();
			this.workable.OnWork_PST_Begins = new System.Action(this.Play_Empty_MeterAnimation);
			this.RefreshMeters();
		}

		// Token: 0x06009959 RID: 39257 RVA: 0x0038C580 File Offset: 0x0038A780
		private void Play_Empty_MeterAnimation()
		{
			this.fatMeter.SetPositionPercent(0f);
			this.fatMeter.meterController.Play("meter_fat_empty", KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x0600995A RID: 39258 RVA: 0x0038C5B8 File Offset: 0x0038A7B8
		public void DropMilkFat()
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Drop(base.def.MILK_FAT_TAG, list);
			Vector3 dropSpawnLocation = this.GetDropSpawnLocation();
			foreach (GameObject gameObject in list)
			{
				gameObject.transform.position = dropSpawnLocation;
			}
		}

		// Token: 0x0600995B RID: 39259 RVA: 0x0038C630 File Offset: 0x0038A830
		private Vector3 GetDropSpawnLocation()
		{
			bool flag;
			Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform(new HashedString("milkfat"), out flag).GetColumn(3);
			vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			int num = Grid.PosToCell(vector);
			if (Grid.IsValidCell(num) && !Grid.Solid[num])
			{
				return vector;
			}
			return base.transform.GetPosition();
		}

		// Token: 0x0600995C RID: 39260 RVA: 0x0038C69C File Offset: 0x0038A89C
		public void RefreshMeters()
		{
			if (this.fatMeter.meterController.currentAnim != "meter_fat")
			{
				this.fatMeter.meterController.Play("meter_fat", KAnim.PlayMode.Paused, 1f, 0f);
			}
			this.fatMeter.SetPositionPercent(this.MilkFatStoragePercentage);
		}

		// Token: 0x04007653 RID: 30291
		[MyCmpGet]
		public EmptyMilkSeparatorWorkable workable;

		// Token: 0x04007654 RID: 30292
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04007655 RID: 30293
		[MyCmpGet]
		public ElementConverter elementConverter;

		// Token: 0x04007656 RID: 30294
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04007657 RID: 30295
		private MeterController fatMeter;
	}
}
