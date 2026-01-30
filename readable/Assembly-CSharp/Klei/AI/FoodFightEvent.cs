using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001042 RID: 4162
	public class FoodFightEvent : GameplayEvent<FoodFightEvent.StatesInstance>
	{
		// Token: 0x0600810E RID: 33038 RVA: 0x0033DC66 File Offset: 0x0033BE66
		public FoodFightEvent() : base("FoodFight", 0, 0, null, null)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.DESCRIPTION;
		}

		// Token: 0x0600810F RID: 33039 RVA: 0x0033DC97 File Offset: 0x0033BE97
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new FoodFightEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x040061B9 RID: 25017
		public const float FUTURE_TIME = 60f;

		// Token: 0x040061BA RID: 25018
		public const float DURATION = 60f;

		// Token: 0x0200273E RID: 10046
		public class StatesInstance : GameplayEventStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, FoodFightEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600C844 RID: 51268 RVA: 0x004262D6 File Offset: 0x004244D6
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, FoodFightEvent foodEvent) : base(master, eventInstance, foodEvent)
			{
			}

			// Token: 0x0600C845 RID: 51269 RVA: 0x004262E4 File Offset: 0x004244E4
			public void CreateChores(FoodFightEvent.StatesInstance smi)
			{
				this.chores = new List<FoodFightChore>();
				List<Room> list = Game.Instance.roomProber.rooms.FindAll((Room match) => match.roomType == Db.Get().RoomTypes.MessHall || match.roomType == Db.Get().RoomTypes.GreatHall);
				if (list == null || list.Count == 0)
				{
					return;
				}
				List<GameObject> buildingsOnFloor = list[UnityEngine.Random.Range(0, list.Count)].GetBuildingsOnFloor();
				for (int i = 0; i < Math.Min(Components.LiveMinionIdentities.Count, buildingsOnFloor.Count); i++)
				{
					IStateMachineTarget master = Components.LiveMinionIdentities[i];
					GameObject gameObject = buildingsOnFloor[UnityEngine.Random.Range(0, buildingsOnFloor.Count)];
					GameObject locator = ChoreHelpers.CreateLocator("FoodFightLocator", gameObject.transform.position);
					FoodFightChore foodFightChore = new FoodFightChore(master, locator);
					buildingsOnFloor.Remove(gameObject);
					FoodFightChore foodFightChore2 = foodFightChore;
					foodFightChore2.onExit = (Action<Chore>)Delegate.Combine(foodFightChore2.onExit, new Action<Chore>(delegate(Chore data)
					{
						Util.KDestroyGameObject(locator);
					}));
					this.chores.Add(foodFightChore);
				}
			}

			// Token: 0x0600C846 RID: 51270 RVA: 0x00426404 File Offset: 0x00424604
			public void ClearChores()
			{
				if (this.chores != null)
				{
					for (int i = this.chores.Count - 1; i >= 0; i--)
					{
						if (this.chores[i] != null)
						{
							this.chores[i].Cancel("end");
						}
					}
				}
				this.chores = null;
			}

			// Token: 0x0400AEAB RID: 44715
			public List<FoodFightChore> chores;
		}

		// Token: 0x0200273F RID: 10047
		public class States : GameplayEventStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, FoodFightEvent>
		{
			// Token: 0x0600C847 RID: 51271 RVA: 0x0042645C File Offset: 0x0042465C
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.root.Exit(delegate(FoodFightEvent.StatesInstance smi)
				{
					smi.ClearChores();
				});
				this.planning.ToggleNotification((FoodFightEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.warmup.ToggleNotification((FoodFightEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.warmup.wait.ScheduleGoTo(60f, this.warmup.start);
				this.warmup.start.Enter(delegate(FoodFightEvent.StatesInstance smi)
				{
					smi.CreateChores(smi);
				}).Update(delegate(FoodFightEvent.StatesInstance smi, float data)
				{
					int num = 0;
					foreach (FoodFightChore foodFightChore in smi.chores)
					{
						if (foodFightChore.smi.IsInsideState(foodFightChore.smi.sm.waitForParticipants))
						{
							num++;
						}
					}
					if (num >= smi.chores.Count || smi.timeinstate > 30f)
					{
						foreach (FoodFightChore foodFightChore2 in smi.chores)
						{
							foodFightChore2.gameObject.Trigger(-2043101269, null);
						}
						smi.GoTo(this.partying);
					}
				}, UpdateRate.RENDER_1000ms, false);
				this.partying.ToggleNotification((FoodFightEvent.StatesInstance smi) => new Notification(GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.UNDERWAY, NotificationType.Good, (List<Notification> a, object b) => GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.UNDERWAY_TOOLTIP, null, true, 0f, null, null, null, true, false, false)).ScheduleGoTo(60f, this.ending);
				this.ending.ReturnSuccess();
				this.canceled.DoNotification((FoodFightEvent.StatesInstance smi) => GameplayEventManager.CreateStandardCancelledNotification(this.GenerateEventPopupData(smi))).Enter(delegate(FoodFightEvent.StatesInstance smi)
				{
					foreach (object obj in Components.LiveMinionIdentities)
					{
						((MinionIdentity)obj).GetComponent<Effects>().Add("NoFunAllowed", true);
					}
				}).ReturnFailure();
			}

			// Token: 0x0600C848 RID: 51272 RVA: 0x004265C8 File Offset: 0x004247C8
			public override EventInfoData GenerateEventPopupData(FoodFightEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.location = GAMEPLAY_EVENTS.LOCATIONS.PRINTING_POD;
				eventInfoData.whenDescription = string.Format(GAMEPLAY_EVENTS.TIMES.IN_CYCLES, 0.1f);
				eventInfoData.AddOption(GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.ACCEPT_OPTION_NAME, null).callback = delegate()
				{
					smi.GoTo(smi.sm.warmup.wait);
				};
				eventInfoData.AddOption(GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.REJECT_OPTION_NAME, null).callback = delegate()
				{
					smi.GoTo(smi.sm.canceled);
				};
				return eventInfoData;
			}

			// Token: 0x0400AEAC RID: 44716
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x0400AEAD RID: 44717
			public FoodFightEvent.States.WarmupStates warmup;

			// Token: 0x0400AEAE RID: 44718
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State partying;

			// Token: 0x0400AEAF RID: 44719
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State ending;

			// Token: 0x0400AEB0 RID: 44720
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State canceled;

			// Token: 0x02003A28 RID: 14888
			public class WarmupStates : GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400EB2D RID: 60205
				public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State wait;

				// Token: 0x0400EB2E RID: 60206
				public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State start;
			}
		}
	}
}
