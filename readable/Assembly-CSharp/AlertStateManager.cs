using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000A07 RID: 2567
public class AlertStateManager : GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>
{
	// Token: 0x06004B29 RID: 19241 RVA: 0x001B4F0C File Offset: 0x001B310C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.off.ParamTransition<bool>(this.isOn, this.on, GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.IsTrue);
		this.on.Exit("VignetteOff", delegate(AlertStateManager.Instance smi)
		{
			Vignette.Instance.Reset();
		}).ParamTransition<bool>(this.isRedAlert, this.on.red, (AlertStateManager.Instance smi, bool p) => this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isRedAlert, this.on.yellow, (AlertStateManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isYellowAlert, this.on.yellow, (AlertStateManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isOn, this.off, GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.IsFalse);
		this.on.red.Enter("EnterEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(1585324898, null);
		}).Exit("ExitEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(-1393151672, null);
		}).Enter("SoundsOnRedAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false));
		}).ToggleNotification((AlertStateManager.Instance smi) => smi.redAlertNotification);
		this.on.yellow.Enter("EnterEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(-741654735, null);
		}).Exit("ExitEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(-2062778933, null);
		}).Enter("SoundsOnYellowAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_OFF", false));
		});
	}

	// Token: 0x040031CB RID: 12747
	public GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State off;

	// Token: 0x040031CC RID: 12748
	public AlertStateManager.OnStates on;

	// Token: 0x040031CD RID: 12749
	public StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter isRedAlert = new StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter();

	// Token: 0x040031CE RID: 12750
	public StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter isYellowAlert = new StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter();

	// Token: 0x040031CF RID: 12751
	public StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter isOn = new StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter();

	// Token: 0x02001A74 RID: 6772
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001A75 RID: 6773
	public class OnStates : GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State
	{
		// Token: 0x040081B4 RID: 33204
		public GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State yellow;

		// Token: 0x040081B5 RID: 33205
		public GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State red;
	}

	// Token: 0x02001A76 RID: 6774
	public new class Instance : GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.GameInstance
	{
		// Token: 0x0600A584 RID: 42372 RVA: 0x003B7370 File Offset: 0x003B5570
		public Instance(IStateMachineTarget master, AlertStateManager.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A585 RID: 42373 RVA: 0x003B73C8 File Offset: 0x003B55C8
		public void UpdateState(float dt)
		{
			if (this.IsRedAlert())
			{
				base.smi.GoTo(base.sm.on.red);
				return;
			}
			if (this.IsYellowAlert())
			{
				base.smi.GoTo(base.sm.on.yellow);
				return;
			}
			if (!this.IsOn())
			{
				base.smi.GoTo(base.sm.off);
			}
		}

		// Token: 0x0600A586 RID: 42374 RVA: 0x003B743B File Offset: 0x003B563B
		public bool IsOn()
		{
			return base.sm.isYellowAlert.Get(base.smi) || base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x0600A587 RID: 42375 RVA: 0x003B746D File Offset: 0x003B566D
		public bool IsRedAlert()
		{
			return base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x0600A588 RID: 42376 RVA: 0x003B7485 File Offset: 0x003B5685
		public bool IsYellowAlert()
		{
			return base.sm.isYellowAlert.Get(base.smi);
		}

		// Token: 0x0600A589 RID: 42377 RVA: 0x003B749D File Offset: 0x003B569D
		public bool IsRedAlertToggledOn()
		{
			return this.isToggled;
		}

		// Token: 0x0600A58A RID: 42378 RVA: 0x003B74A5 File Offset: 0x003B56A5
		public void ToggleRedAlert(bool on)
		{
			this.isToggled = on;
			this.Refresh();
		}

		// Token: 0x0600A58B RID: 42379 RVA: 0x003B74B4 File Offset: 0x003B56B4
		public void SetHasTopPriorityChore(bool on)
		{
			this.hasTopPriorityChore = on;
			this.Refresh();
		}

		// Token: 0x0600A58C RID: 42380 RVA: 0x003B74C4 File Offset: 0x003B56C4
		private void Refresh()
		{
			base.sm.isYellowAlert.Set(this.hasTopPriorityChore, base.smi, false);
			base.sm.isRedAlert.Set(this.isToggled, base.smi, false);
			base.sm.isOn.Set(this.hasTopPriorityChore || this.isToggled, base.smi, false);
		}

		// Token: 0x040081B6 RID: 33206
		private bool isToggled;

		// Token: 0x040081B7 RID: 33207
		private bool hasTopPriorityChore;

		// Token: 0x040081B8 RID: 33208
		public Notification redAlertNotification = new Notification(MISC.NOTIFICATIONS.REDALERT.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.REDALERT.TOOLTIP, null, false, 0f, null, null, null, true, false, false);
	}
}
