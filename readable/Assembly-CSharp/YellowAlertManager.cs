using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A59 RID: 2649
public class YellowAlertManager : GameStateMachine<YellowAlertManager, YellowAlertManager.Instance>
{
	// Token: 0x06004D05 RID: 19717 RVA: 0x001C048C File Offset: 0x001BE68C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.ParamTransition<bool>(this.isOn, this.on, GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.IsTrue);
		this.on.Enter("EnterEvent", delegate(YellowAlertManager.Instance smi)
		{
			Game.Instance.Trigger(-741654735, null);
		}).Exit("ExitEvent", delegate(YellowAlertManager.Instance smi)
		{
			Game.Instance.Trigger(-2062778933, null);
		}).Enter("EnableVignette", delegate(YellowAlertManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 1f, 0f, 0.1f));
		}).Exit("DisableVignette", delegate(YellowAlertManager.Instance smi)
		{
			Vignette.Instance.Reset();
		}).Enter("Sounds", delegate(YellowAlertManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("RedAlert_LP", false), null, true, true, true).ToggleNotification((YellowAlertManager.Instance smi) => smi.notification).ParamTransition<bool>(this.isOn, this.off, GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.IsFalse);
		this.on_pst.Enter("Sounds", delegate(YellowAlertManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false));
		});
	}

	// Token: 0x04003353 RID: 13139
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04003354 RID: 13140
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x04003355 RID: 13141
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State on_pst;

	// Token: 0x04003356 RID: 13142
	public StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.BoolParameter isOn = new StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x02001B6B RID: 7019
	public new class Instance : GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A9ED RID: 43501 RVA: 0x003C2F4B File Offset: 0x003C114B
		public static void DestroyInstance()
		{
			YellowAlertManager.Instance.instance = null;
		}

		// Token: 0x0600A9EE RID: 43502 RVA: 0x003C2F53 File Offset: 0x003C1153
		public static YellowAlertManager.Instance Get()
		{
			return YellowAlertManager.Instance.instance;
		}

		// Token: 0x0600A9EF RID: 43503 RVA: 0x003C2F5C File Offset: 0x003C115C
		public Instance(IStateMachineTarget master) : base(master)
		{
			YellowAlertManager.Instance.instance = this;
		}

		// Token: 0x0600A9F0 RID: 43504 RVA: 0x003C2FB8 File Offset: 0x003C11B8
		public bool IsOn()
		{
			return base.sm.isOn.Get(base.smi);
		}

		// Token: 0x0600A9F1 RID: 43505 RVA: 0x003C2FD0 File Offset: 0x003C11D0
		public void HasTopPriorityChore(bool on)
		{
			this.hasTopPriorityChore = on;
			this.Refresh();
		}

		// Token: 0x0600A9F2 RID: 43506 RVA: 0x003C2FDF File Offset: 0x003C11DF
		private void Refresh()
		{
			base.sm.isOn.Set(this.hasTopPriorityChore, base.smi, false);
		}

		// Token: 0x040084EB RID: 34027
		private static YellowAlertManager.Instance instance;

		// Token: 0x040084EC RID: 34028
		private bool hasTopPriorityChore;

		// Token: 0x040084ED RID: 34029
		public Notification notification = new Notification(MISC.NOTIFICATIONS.YELLOWALERT.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.YELLOWALERT.TOOLTIP, null, false, 0f, null, null, null, true, false, false);
	}
}
