using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A57 RID: 2647
public class VignetteManager : GameStateMachine<VignetteManager, VignetteManager.Instance>
{
	// Token: 0x06004CFD RID: 19709 RVA: 0x001BFF88 File Offset: 0x001BE188
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.ParamTransition<bool>(this.isOn, this.on, GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.IsTrue);
		this.on.Exit("VignetteOff", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.Reset();
		}).ParamTransition<bool>(this.isRedAlert, this.on.red, (VignetteManager.Instance smi, bool p) => this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isRedAlert, this.on.yellow, (VignetteManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isYellowAlert, this.on.yellow, (VignetteManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isOn, this.off, GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.IsFalse);
		this.on.red.Enter("EnterEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(1585324898, null);
		}).Exit("ExitEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-1393151672, null);
		}).Enter("EnableVignette", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 0f, 0f, 0.3f));
		}).Enter("SoundsOnRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("RedAlert_LP", false), null, true, false, true).ToggleNotification((VignetteManager.Instance smi) => smi.redAlertNotification);
		this.on.yellow.Enter("EnterEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-741654735, null);
		}).Exit("ExitEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-2062778933, null);
		}).Enter("EnableVignette", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 1f, 0f, 0.3f));
		}).Enter("SoundsOnYellowAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_OFF", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("YellowAlert_LP", false), null, true, false, true);
	}

	// Token: 0x0400334C RID: 13132
	public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x0400334D RID: 13133
	public VignetteManager.OnStates on;

	// Token: 0x0400334E RID: 13134
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isRedAlert = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x0400334F RID: 13135
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isYellowAlert = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x04003350 RID: 13136
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isOn = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x02001B65 RID: 7013
	public class OnStates : GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040084CE RID: 33998
		public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State yellow;

		// Token: 0x040084CF RID: 33999
		public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State red;
	}

	// Token: 0x02001B66 RID: 7014
	public new class Instance : GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A9C6 RID: 43462 RVA: 0x003C2832 File Offset: 0x003C0A32
		public static void DestroyInstance()
		{
			VignetteManager.Instance.instance = null;
		}

		// Token: 0x0600A9C7 RID: 43463 RVA: 0x003C283A File Offset: 0x003C0A3A
		public static VignetteManager.Instance Get()
		{
			return VignetteManager.Instance.instance;
		}

		// Token: 0x0600A9C8 RID: 43464 RVA: 0x003C2844 File Offset: 0x003C0A44
		public Instance(IStateMachineTarget master) : base(master)
		{
			VignetteManager.Instance.instance = this;
		}

		// Token: 0x0600A9C9 RID: 43465 RVA: 0x003C28A0 File Offset: 0x003C0AA0
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

		// Token: 0x0600A9CA RID: 43466 RVA: 0x003C2913 File Offset: 0x003C0B13
		public bool IsOn()
		{
			return base.sm.isYellowAlert.Get(base.smi) || base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x0600A9CB RID: 43467 RVA: 0x003C2945 File Offset: 0x003C0B45
		public bool IsRedAlert()
		{
			return base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x0600A9CC RID: 43468 RVA: 0x003C295D File Offset: 0x003C0B5D
		public bool IsYellowAlert()
		{
			return base.sm.isYellowAlert.Get(base.smi);
		}

		// Token: 0x0600A9CD RID: 43469 RVA: 0x003C2975 File Offset: 0x003C0B75
		public bool IsRedAlertToggledOn()
		{
			return this.isToggled;
		}

		// Token: 0x0600A9CE RID: 43470 RVA: 0x003C297D File Offset: 0x003C0B7D
		public void ToggleRedAlert(bool on)
		{
			this.isToggled = on;
			this.Refresh();
		}

		// Token: 0x0600A9CF RID: 43471 RVA: 0x003C298C File Offset: 0x003C0B8C
		public void HasTopPriorityChore(bool on)
		{
			this.hasTopPriorityChore = on;
			this.Refresh();
		}

		// Token: 0x0600A9D0 RID: 43472 RVA: 0x003C299C File Offset: 0x003C0B9C
		private void Refresh()
		{
			base.sm.isYellowAlert.Set(this.hasTopPriorityChore, base.smi, false);
			base.sm.isRedAlert.Set(this.isToggled, base.smi, false);
			base.sm.isOn.Set(this.hasTopPriorityChore || this.isToggled, base.smi, false);
		}

		// Token: 0x040084D0 RID: 34000
		private static VignetteManager.Instance instance;

		// Token: 0x040084D1 RID: 34001
		private bool isToggled;

		// Token: 0x040084D2 RID: 34002
		private bool hasTopPriorityChore;

		// Token: 0x040084D3 RID: 34003
		public Notification redAlertNotification = new Notification(MISC.NOTIFICATIONS.REDALERT.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.REDALERT.TOOLTIP, null, false, 0f, null, null, null, true, false, false);
	}
}
