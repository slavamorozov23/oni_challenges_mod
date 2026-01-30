using System;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02001041 RID: 4161
	public class EclipseEvent : GameplayEvent<EclipseEvent.StatesInstance>
	{
		// Token: 0x0600810C RID: 33036 RVA: 0x0033DC2B File Offset: 0x0033BE2B
		public EclipseEvent() : base("EclipseEvent", 0, 0, null, null)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.ECLIPSE.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.ECLIPSE.DESCRIPTION;
		}

		// Token: 0x0600810D RID: 33037 RVA: 0x0033DC5C File Offset: 0x0033BE5C
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new EclipseEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x040061B7 RID: 25015
		public const string ID = "EclipseEvent";

		// Token: 0x040061B8 RID: 25016
		public const float duration = 30f;

		// Token: 0x0200273C RID: 10044
		public class StatesInstance : GameplayEventStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, EclipseEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600C83F RID: 51263 RVA: 0x004261B3 File Offset: 0x004243B3
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, EclipseEvent eclipseEvent) : base(master, eventInstance, eclipseEvent)
			{
			}
		}

		// Token: 0x0200273D RID: 10045
		public class States : GameplayEventStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, EclipseEvent>
		{
			// Token: 0x0600C840 RID: 51264 RVA: 0x004261C0 File Offset: 0x004243C0
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.planning.GoTo(this.eclipse);
				this.eclipse.ToggleNotification((EclipseEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null)).Enter(delegate(EclipseEvent.StatesInstance smi)
				{
					TimeOfDay.Instance.SetEclipse(true);
				}).Exit(delegate(EclipseEvent.StatesInstance smi)
				{
					TimeOfDay.Instance.SetEclipse(false);
				}).ScheduleGoTo(30f, this.finished);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600C841 RID: 51265 RVA: 0x0042626C File Offset: 0x0042446C
			public override EventInfoData GenerateEventPopupData(EclipseEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.SUN,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x0400AEA8 RID: 44712
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x0400AEA9 RID: 44713
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State eclipse;

			// Token: 0x0400AEAA RID: 44714
			public GameStateMachine<EclipseEvent.States, EclipseEvent.StatesInstance, GameplayEventManager, object>.State finished;
		}
	}
}
