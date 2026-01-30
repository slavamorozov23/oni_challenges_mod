using System;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001049 RID: 4169
	public class SatelliteCrashEvent : GameplayEvent<SatelliteCrashEvent.StatesInstance>
	{
		// Token: 0x0600812D RID: 33069 RVA: 0x0033E3D4 File Offset: 0x0033C5D4
		public SatelliteCrashEvent() : base("SatelliteCrash", 0, 0, null, null)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.SATELLITE_CRASH.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.SATELLITE_CRASH.DESCRIPTION;
		}

		// Token: 0x0600812E RID: 33070 RVA: 0x0033E405 File Offset: 0x0033C605
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SatelliteCrashEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x02002749 RID: 10057
		public class StatesInstance : GameplayEventStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, SatelliteCrashEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600C889 RID: 51337 RVA: 0x004280BD File Offset: 0x004262BD
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SatelliteCrashEvent crashEvent) : base(master, eventInstance, crashEvent)
			{
			}

			// Token: 0x0600C88A RID: 51338 RVA: 0x004280C8 File Offset: 0x004262C8
			public Notification Plan()
			{
				Vector3 position = new Vector3((float)(Grid.WidthInCells / 2 + UnityEngine.Random.Range(-Grid.WidthInCells / 3, Grid.WidthInCells / 3)), (float)(Grid.HeightInCells - 1), Grid.GetLayerZ(Grid.SceneLayer.FXFront));
				GameObject spawn = Util.KInstantiate(Assets.GetPrefab(SatelliteCometConfig.ID), position);
				spawn.SetActive(true);
				Notification notification = EventInfoScreen.CreateNotification(base.smi.sm.GenerateEventPopupData(base.smi), null);
				notification.clickFocus = spawn.transform;
				Comet component = spawn.GetComponent<Comet>();
				component.OnImpact = (System.Action)Delegate.Combine(component.OnImpact, new System.Action(delegate()
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = spawn.transform.position;
					notification.clickFocus = gameObject.transform;
					GridVisibility.Reveal(Grid.PosToXY(gameObject.transform.position).x, Grid.PosToXY(gameObject.transform.position).y, 6, 4f);
				}));
				return notification;
			}
		}

		// Token: 0x0200274A RID: 10058
		public class States : GameplayEventStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, SatelliteCrashEvent>
		{
			// Token: 0x0600C88B RID: 51339 RVA: 0x004281A0 File Offset: 0x004263A0
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.notify;
				this.notify.ToggleNotification((SatelliteCrashEvent.StatesInstance smi) => smi.Plan());
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600C88C RID: 51340 RVA: 0x004281EC File Offset: 0x004263EC
			public override EventInfoData GenerateEventPopupData(SatelliteCrashEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.location = GAMEPLAY_EVENTS.LOCATIONS.SURFACE;
				eventInfoData.whenDescription = GAMEPLAY_EVENTS.TIMES.NOW;
				eventInfoData.AddDefaultOption(delegate
				{
					smi.GoTo(smi.sm.ending);
				});
				return eventInfoData;
			}

			// Token: 0x0400AED8 RID: 44760
			public GameStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, object>.State notify;

			// Token: 0x0400AED9 RID: 44761
			public GameStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, object>.State ending;
		}
	}
}
