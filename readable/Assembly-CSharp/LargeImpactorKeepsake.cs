using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200028E RID: 654
public class LargeImpactorKeepsake : GameStateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>
{
	// Token: 0x06000D4A RID: 3402 RVA: 0x0004EB4C File Offset: 0x0004CD4C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.notification;
		this.notification.ParamTransition<bool>(this.HasNotificationBeenAknowledged, this.idle, GameStateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>.IsTrue).ToggleNotification(new Func<LargeImpactorKeepsake.Instance, Notification>(LargeImpactorKeepsake.GetNotification));
		this.idle.DoNothing();
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x0004EBA2 File Offset: 0x0004CDA2
	public static Notification GetNotification(LargeImpactorKeepsake.Instance smi)
	{
		return smi.notification;
	}

	// Token: 0x0400090E RID: 2318
	private GameStateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>.State notification;

	// Token: 0x0400090F RID: 2319
	private GameStateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>.State idle;

	// Token: 0x04000910 RID: 2320
	private StateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>.BoolParameter HasNotificationBeenAknowledged;

	// Token: 0x020011F6 RID: 4598
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020011F7 RID: 4599
	public new class Instance : GameStateMachine<LargeImpactorKeepsake, LargeImpactorKeepsake.Instance, IStateMachineTarget, LargeImpactorKeepsake.Def>.GameInstance
	{
		// Token: 0x06008630 RID: 34352 RVA: 0x00349694 File Offset: 0x00347894
		public Instance(IStateMachineTarget master, LargeImpactorKeepsake.Def def) : base(master, def)
		{
			this.notification = this.CreateDeathNotification();
		}

		// Token: 0x06008631 RID: 34353 RVA: 0x003496AC File Offset: 0x003478AC
		private Notification CreateDeathNotification()
		{
			string title = MISC.NOTIFICATIONS.LARGE_IMPACTOR_KEEPSAKE.NAME;
			NotificationType type = NotificationType.Event;
			Func<List<Notification>, object, string> tooltip = (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.LARGE_IMPACTOR_KEEPSAKE.TOOLTIP;
			object tooltip_data = null;
			bool expires = false;
			float delay = 0f;
			Transform transform = base.gameObject.transform;
			return new Notification(title, type, tooltip, tooltip_data, expires, delay, new Notification.ClickCallback(this.MarkAsAknowledgedAndFocusCamera), this, transform, true, true, false);
		}

		// Token: 0x06008632 RID: 34354 RVA: 0x00349710 File Offset: 0x00347910
		private void MarkAsAknowledgedAndFocusCamera(object data)
		{
			if (data == null)
			{
				return;
			}
			LargeImpactorKeepsake.Instance instance = (LargeImpactorKeepsake.Instance)data;
			instance.sm.HasNotificationBeenAknowledged.Set(true, instance, false);
			GameUtil.FocusCamera(base.gameObject.transform, true, true);
		}

		// Token: 0x0400664F RID: 26191
		public Notification notification;
	}
}
