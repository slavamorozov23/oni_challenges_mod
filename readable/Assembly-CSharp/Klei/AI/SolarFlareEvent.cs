using System;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x0200104B RID: 4171
	public class SolarFlareEvent : GameplayEvent<SolarFlareEvent.StatesInstance>
	{
		// Token: 0x06008131 RID: 33073 RVA: 0x0033E451 File Offset: 0x0033C651
		public SolarFlareEvent() : base("SolarFlareEvent", 0, 0, null, null)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.DESCRIPTION;
		}

		// Token: 0x06008132 RID: 33074 RVA: 0x0033E482 File Offset: 0x0033C682
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SolarFlareEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x040061E6 RID: 25062
		public const string ID = "SolarFlareEvent";

		// Token: 0x040061E7 RID: 25063
		public const float DURATION = 7f;

		// Token: 0x0200274D RID: 10061
		public class StatesInstance : GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600C894 RID: 51348 RVA: 0x004283F8 File Offset: 0x004265F8
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SolarFlareEvent solarFlareEvent) : base(master, eventInstance, solarFlareEvent)
			{
			}
		}

		// Token: 0x0200274E RID: 10062
		public class States : GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>
		{
			// Token: 0x0600C895 RID: 51349 RVA: 0x00428403 File Offset: 0x00426603
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.idle;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.idle.DoNothing();
				this.start.ScheduleGoTo(7f, this.finished);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600C896 RID: 51350 RVA: 0x00428444 File Offset: 0x00426644
			public override EventInfoData GenerateEventPopupData(SolarFlareEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.SUN,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x0400AEDF RID: 44767
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State idle;

			// Token: 0x0400AEE0 RID: 44768
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State start;

			// Token: 0x0400AEE1 RID: 44769
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State finished;
		}
	}
}
