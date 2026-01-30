using System;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004C9 RID: 1225
public class VomitChore : Chore<VomitChore.StatesInstance>
{
	// Token: 0x060019C2 RID: 6594 RVA: 0x00090198 File Offset: 0x0008E398
	private static KAnimFile GetAnimFileName(VomitChore.StatesInstance smi)
	{
		string s = "anim_vomit_kanim";
		GameObject gameObject = smi.sm.vomiter.Get(smi);
		if (gameObject == null)
		{
			return Assets.GetAnim(s);
		}
		MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
		if (component == null)
		{
			return Assets.GetAnim(s);
		}
		if (component.model == BionicMinionConfig.MODEL)
		{
			return Assets.GetAnim("anim_bionic_vomit_kanim");
		}
		return Assets.GetAnim(s);
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x0009021C File Offset: 0x0008E41C
	public VomitChore(ChoreType chore_type, IStateMachineTarget target, StatusItem status_item, Notification notification, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.Vomit, target, target.GetComponent<ChoreProvider>(), true, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new VomitChore.StatesInstance(this, target.gameObject, status_item, notification);
	}

	// Token: 0x02001335 RID: 4917
	public class StatesInstance : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.GameInstance
	{
		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06008B3C RID: 35644 RVA: 0x0035E434 File Offset: 0x0035C634
		// (set) Token: 0x06008B3B RID: 35643 RVA: 0x0035E42B File Offset: 0x0035C62B
		public SimHashes elementToVomit { get; private set; } = SimHashes.DirtyWater;

		// Token: 0x06008B3D RID: 35645 RVA: 0x0035E43C File Offset: 0x0035C63C
		public StatesInstance(VomitChore master, GameObject vomiter, StatusItem status_item, Notification notification) : base(master)
		{
			base.sm.vomiter.Set(vomiter, base.smi, false);
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(vomiter);
			this.statusItem = status_item;
			this.notification = notification;
			this.vomitCellQuery = new SafetyQuery(Game.Instance.safetyConditions.VomitCellChecker, base.GetComponent<KMonoBehaviour>(), 10);
			MinionIdentity component = vomiter.GetComponent<MinionIdentity>();
			if (component != null && component.model == BionicMinionConfig.MODEL)
			{
				this.elementToVomit = SimHashes.LiquidGunk;
			}
		}

		// Token: 0x06008B3E RID: 35646 RVA: 0x0035E4F0 File Offset: 0x0035C6F0
		private static bool CanEmitLiquid(int cell)
		{
			bool result = true;
			if (!Grid.IsValidCell(cell) || Grid.Solid[cell] || (Grid.Properties[cell] & 2) != 0)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06008B3F RID: 35647 RVA: 0x0035E526 File Offset: 0x0035C726
		public void SpawnDirtyWater(float dt)
		{
			this.SpawnVomitLiquid(dt, SimHashes.DirtyWater);
		}

		// Token: 0x06008B40 RID: 35648 RVA: 0x0035E534 File Offset: 0x0035C734
		public void SpawnVomitLiquid(float dt, SimHashes element)
		{
			if (dt > 0f)
			{
				float totalTime = base.GetComponent<KBatchedAnimController>().CurrentAnim.totalTime;
				float num = dt / totalTime;
				Sicknesses sicknesses = base.master.GetComponent<MinionModifiers>().sicknesses;
				SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
				int num2 = 0;
				while (num2 < sicknesses.Count && sicknesses[num2].modifier.sicknessType != Sickness.SicknessType.Pathogen)
				{
					num2++;
				}
				Facing component = base.sm.vomiter.Get(base.smi).GetComponent<Facing>();
				int num3 = Grid.PosToCell(component.transform.GetPosition());
				int num4 = component.GetFrontCell();
				if (!VomitChore.StatesInstance.CanEmitLiquid(num4))
				{
					num4 = num3;
				}
				Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
				if (equippable != null)
				{
					equippable.GetComponent<Storage>().AddLiquid(element, STRESS.VOMIT_AMOUNT * num, this.bodyTemperature.value, invalid.idx, invalid.count, false, true);
					return;
				}
				SimMessages.AddRemoveSubstance(num4, element, CellEventLogger.Instance.Vomit, STRESS.VOMIT_AMOUNT * num, this.bodyTemperature.value, invalid.idx, invalid.count, true, -1);
			}
		}

		// Token: 0x06008B41 RID: 35649 RVA: 0x0035E65C File Offset: 0x0035C85C
		public int GetVomitCell()
		{
			this.vomitCellQuery.Reset();
			Navigator component = base.GetComponent<Navigator>();
			component.RunQuery(this.vomitCellQuery);
			int num = this.vomitCellQuery.GetResultCell();
			if (Grid.InvalidCell == num)
			{
				num = Grid.PosToCell(component);
			}
			return num;
		}

		// Token: 0x04006A9B RID: 27291
		public StatusItem statusItem;

		// Token: 0x04006A9C RID: 27292
		private AmountInstance bodyTemperature;

		// Token: 0x04006A9D RID: 27293
		public Notification notification;

		// Token: 0x04006A9E RID: 27294
		private SafetyQuery vomitCellQuery;
	}

	// Token: 0x02001336 RID: 4918
	public class States : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore>
	{
		// Token: 0x06008B42 RID: 35650 RVA: 0x0035E6A4 File Offset: 0x0035C8A4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.moveto;
			base.Target(this.vomiter);
			this.root.ToggleAnims("anim_emotes_default_kanim", 0f);
			this.moveto.TriggerOnEnter(GameHashes.BeginWalk, null).TriggerOnExit(GameHashes.EndWalk, null).ToggleAnims("anim_loco_vomiter_kanim", 0f).MoveTo((VomitChore.StatesInstance smi) => smi.GetVomitCell(), this.vomit, this.vomit, false);
			this.vomit.DefaultState(this.vomit.buildup).ToggleAnims(new Func<VomitChore.StatesInstance, KAnimFile>(VomitChore.GetAnimFileName)).ToggleStatusItem((VomitChore.StatesInstance smi) => smi.statusItem, null).DoNotification((VomitChore.StatesInstance smi) => smi.notification).DoTutorial(Tutorial.TutorialMessages.TM_Mopping).Enter(delegate(VomitChore.StatesInstance smi)
			{
				if (smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
				{
					smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
				}
			}).Exit(delegate(VomitChore.StatesInstance smi)
			{
				smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
				float num = Mathf.Min(smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).value, 20f);
				smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).ApplyDelta(-num);
				if (num >= 1f)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Mathf.FloorToInt(num).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, smi.master.transform, 1.5f, false);
				}
			});
			this.vomit.buildup.PlayAnim("vomit_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.vomit.release);
			this.vomit.release.ToggleEffect("Vomiting").PlayAnim("vomit_loop", KAnim.PlayMode.Once).Update("SpawnVomitLiquid", delegate(VomitChore.StatesInstance smi, float dt)
			{
				smi.SpawnVomitLiquid(dt, smi.elementToVomit);
			}, UpdateRate.SIM_200ms, false).OnAnimQueueComplete(this.vomit.release_pst);
			this.vomit.release_pst.PlayAnim("vomit_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover);
			this.recover.PlayAnim("breathe_pre").QueueAnim("breathe_loop", true, null).ScheduleGoTo(8f, this.recover_pst);
			this.recover_pst.QueueAnim("breathe_pst", false, null).OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x04006AA0 RID: 27296
		public StateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.TargetParameter vomiter;

		// Token: 0x04006AA1 RID: 27297
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State moveto;

		// Token: 0x04006AA2 RID: 27298
		public VomitChore.States.VomitState vomit;

		// Token: 0x04006AA3 RID: 27299
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State recover;

		// Token: 0x04006AA4 RID: 27300
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State recover_pst;

		// Token: 0x04006AA5 RID: 27301
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State complete;

		// Token: 0x020027F0 RID: 10224
		public class VomitState : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State
		{
			// Token: 0x0400B10B RID: 45323
			public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State buildup;

			// Token: 0x0400B10C RID: 45324
			public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State release;

			// Token: 0x0400B10D RID: 45325
			public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State release_pst;
		}
	}
}
