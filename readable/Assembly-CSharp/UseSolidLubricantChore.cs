using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020004C8 RID: 1224
public class UseSolidLubricantChore : Chore<UseSolidLubricantChore.Instance>
{
	// Token: 0x060019BD RID: 6589 RVA: 0x0008FEC4 File Offset: 0x0008E0C4
	public UseSolidLubricantChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.SolidOilChange, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new UseSolidLubricantChore.Instance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(UseSolidLubricantChore.SolidLubricantIsNotNull, null);
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x0008FF28 File Offset: 0x0008E128
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null context.consumer");
			return;
		}
		BionicOilMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicOilMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null RationMonitor.Instance");
			return;
		}
		Pickupable closestSolidLubricant = smi.GetClosestSolidLubricant();
		if (closestSolidLubricant == null)
		{
			global::Debug.LogError("ReloadElectrobankChore null electrobank.gameObject");
			return;
		}
		base.smi.sm.solidLubricantSource.Set(closestSolidLubricant.gameObject, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x0008FFE0 File Offset: 0x0008E1E0
	public static void ConsumeLubricant(UseSolidLubricantChore.Instance smi)
	{
		PrimaryElement component = smi.sm.pickedUpSolidLubricant.Get(smi).GetComponent<PrimaryElement>();
		float num = Mathf.Min(component.Mass, 200f - smi.oilMonitor.oilAmount.value);
		smi.oilMonitor.RefillOil(num);
		if (num >= component.Mass)
		{
			Util.KDestroyGameObject(component.gameObject);
			smi.sm.pickedUpSolidLubricant.Set(null, smi);
		}
		else
		{
			component.Mass -= num;
		}
		BionicOilMonitor.ApplyLubricationEffects(smi.master.GetComponent<Effects>(), component.GetComponent<PrimaryElement>().ElementID);
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x00090084 File Offset: 0x0008E284
	public static void SetOverrideAnimSymbol(UseSolidLubricantChore.Instance smi, bool overriding)
	{
		string text = "lubricant";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.pickedUpSolidLubricant.Get(smi);
		if (gameObject != null)
		{
			KBatchedAnimTracker component3 = gameObject.GetComponent<KBatchedAnimTracker>();
			if (component3 != null)
			{
				component3.enabled = !overriding;
			}
			Storage.MakeItemInvisible(gameObject, overriding, false);
		}
		if (!overriding)
		{
			component2.RemoveSymbolOverride(text, 0);
			component.SetSymbolVisiblity(text, false);
			return;
		}
		KAnim.Build.Symbol symbolByIndex = gameObject.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
		component2.AddSymbolOverride(text, symbolByIndex, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x04000EE6 RID: 3814
	public const float LOOP_LENGTH = 6.666f;

	// Token: 0x04000EE7 RID: 3815
	public static readonly Chore.Precondition SolidLubricantIsNotNull = new Chore.Precondition
	{
		id = "SolidLubricantIsNotNull ",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return null != context.consumerState.consumer.GetSMI<BionicOilMonitor.Instance>().GetClosestSolidLubricant();
		}
	};

	// Token: 0x02001332 RID: 4914
	public class States : GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore>
	{
		// Token: 0x06008B34 RID: 35636 RVA: 0x0035E218 File Offset: 0x0035C418
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.solidLubricantSource, this.pickedUpSolidLubricant, this.amountRequested, this.actualunits, this.consume, null).OnTargetLost(this.solidLubricantSource, this.lubricantLost);
			this.consume.DefaultState(this.consume.pre).ToggleAnims("anim_bionic_kanim", 0f).Enter("Add Symbol Override", delegate(UseSolidLubricantChore.Instance smi)
			{
				UseSolidLubricantChore.SetOverrideAnimSymbol(smi, true);
			}).Exit("Revert Symbol Override", delegate(UseSolidLubricantChore.Instance smi)
			{
				UseSolidLubricantChore.SetOverrideAnimSymbol(smi, false);
			});
			this.consume.pre.PlayAnim("lubricate_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.consume.loop).ScheduleGoTo(4.7f, this.consume.loop);
			this.consume.loop.PlayAnim("lubricate_loop", KAnim.PlayMode.Loop).ScheduleGoTo(6.666f, this.consume.pst);
			this.consume.pst.PlayAnim("lubricate_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3.5f, this.complete);
			this.complete.Enter(new StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State.Callback(UseSolidLubricantChore.ConsumeLubricant)).ReturnSuccess();
			this.lubricantLost.Target(this.dupe).ReturnFailure();
		}

		// Token: 0x04006A90 RID: 27280
		public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FetchSubState fetch;

		// Token: 0x04006A91 RID: 27281
		public UseSolidLubricantChore.States.InstallState consume;

		// Token: 0x04006A92 RID: 27282
		public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State complete;

		// Token: 0x04006A93 RID: 27283
		public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State lubricantLost;

		// Token: 0x04006A94 RID: 27284
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter dupe;

		// Token: 0x04006A95 RID: 27285
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter solidLubricantSource;

		// Token: 0x04006A96 RID: 27286
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter pickedUpSolidLubricant;

		// Token: 0x04006A97 RID: 27287
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.TargetParameter messstation;

		// Token: 0x04006A98 RID: 27288
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FloatParameter actualunits;

		// Token: 0x04006A99 RID: 27289
		public StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FloatParameter amountRequested = new StateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.FloatParameter(LubricationStickConfig.MASS_PER_RECIPE);

		// Token: 0x020027EE RID: 10222
		public class InstallState : GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State
		{
			// Token: 0x0400B105 RID: 45317
			public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State pre;

			// Token: 0x0400B106 RID: 45318
			public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State loop;

			// Token: 0x0400B107 RID: 45319
			public GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.State pst;
		}
	}

	// Token: 0x02001333 RID: 4915
	public class Instance : GameStateMachine<UseSolidLubricantChore.States, UseSolidLubricantChore.Instance, UseSolidLubricantChore, object>.GameInstance
	{
		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06008B36 RID: 35638 RVA: 0x0035E3D9 File Offset: 0x0035C5D9
		public BionicOilMonitor.Instance oilMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicOilMonitor.Instance>();
			}
		}

		// Token: 0x06008B37 RID: 35639 RVA: 0x0035E3F1 File Offset: 0x0035C5F1
		public Instance(UseSolidLubricantChore master, GameObject duplicant) : base(master)
		{
		}
	}
}
