using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001047 RID: 4167
	public class PartyEvent : GameplayEvent<PartyEvent.StatesInstance>
	{
		// Token: 0x06008129 RID: 33065 RVA: 0x0033E324 File Offset: 0x0033C524
		public PartyEvent() : base("Party", 0, 0, null, null)
		{
			this.animFileName = "event_pop_up_assets_kanim";
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.DESCRIPTION;
		}

		// Token: 0x0600812A RID: 33066 RVA: 0x0033E370 File Offset: 0x0033C570
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new PartyEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x040061DD RID: 25053
		public const string cancelEffect = "NoFunAllowed";

		// Token: 0x040061DE RID: 25054
		public const float FUTURE_TIME = 60f;

		// Token: 0x040061DF RID: 25055
		public const float DURATION = 60f;

		// Token: 0x02002745 RID: 10053
		public class StatesInstance : GameplayEventStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, PartyEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600C86D RID: 51309 RVA: 0x00427299 File Offset: 0x00425499
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, PartyEvent partyEvent) : base(master, eventInstance, partyEvent)
			{
			}

			// Token: 0x0600C86E RID: 51310 RVA: 0x004272A4 File Offset: 0x004254A4
			public void AddNewChore(Room room)
			{
				List<KPrefabID> list = room.buildings.FindAll((KPrefabID match) => match.HasTag(RoomConstraints.ConstraintTags.RecBuilding));
				if (list.Count == 0)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Tried adding a party chore but the room wasn't valid! This probably happened on load? It's because rooms aren't built yet!"
					});
					return;
				}
				int num = 0;
				bool flag = false;
				int locator_cell = Grid.InvalidCell;
				Predicate<Chore> <>9__2;
				while (num < 20 && !flag)
				{
					num++;
					KPrefabID cmp = list[UnityEngine.Random.Range(0, list.Count)];
					CellOffset offset = new CellOffset(UnityEngine.Random.Range(-2, 3), 0);
					locator_cell = Grid.OffsetCell(Grid.PosToCell(cmp), offset);
					if (!Grid.HasDoor[locator_cell] && Game.Instance.roomProber.GetCavityForCell(locator_cell) == room.cavity)
					{
						List<Chore> list2 = this.chores;
						Predicate<Chore> match2;
						if ((match2 = <>9__2) == null)
						{
							match2 = (<>9__2 = ((Chore match) => Grid.PosToCell(match.target.gameObject) == locator_cell));
						}
						if (list2.Find(match2) == null)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return;
				}
				Vector3 pos = Grid.CellToPosCBC(locator_cell, Grid.SceneLayer.Move);
				GameObject locator = ChoreHelpers.CreateLocator("PartyWorkable", pos);
				PartyPointWorkable partyPointWorkable = locator.AddOrGet<PartyPointWorkable>();
				partyPointWorkable.SetWorkTime((float)UnityEngine.Random.Range(10, 30));
				partyPointWorkable.basePriority = RELAXATION.PRIORITY.SPECIAL_EVENT;
				partyPointWorkable.faceTargetWhenWorking = true;
				PartyChore item = new PartyChore(locator.GetComponent<IStateMachineTarget>(), partyPointWorkable, null, null, delegate(Chore data)
				{
					if (this.chores != null)
					{
						this.chores.Remove(data);
						Util.KDestroyGameObject(locator);
					}
				});
				this.chores.Add(item);
			}

			// Token: 0x0600C86F RID: 51311 RVA: 0x00427448 File Offset: 0x00425648
			public void ClearChores()
			{
				if (this.chores != null)
				{
					for (int i = this.chores.Count - 1; i >= 0; i--)
					{
						if (this.chores[i] != null)
						{
							Util.KDestroyGameObject(this.chores[i].gameObject);
						}
					}
				}
				this.chores = null;
			}

			// Token: 0x0600C870 RID: 51312 RVA: 0x004274A0 File Offset: 0x004256A0
			public void UpdateChores(Room room)
			{
				if (room == null)
				{
					return;
				}
				if (this.chores == null)
				{
					this.chores = new List<Chore>();
				}
				for (int i = this.chores.Count; i < Components.LiveMinionIdentities.Count * 2; i++)
				{
					this.AddNewChore(room);
				}
			}

			// Token: 0x0400AEC8 RID: 44744
			private List<Chore> chores;

			// Token: 0x0400AEC9 RID: 44745
			public Notification mainNotification;
		}

		// Token: 0x02002746 RID: 10054
		public class States : GameplayEventStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, PartyEvent>
		{
			// Token: 0x0600C871 RID: 51313 RVA: 0x004274EC File Offset: 0x004256EC
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning.prepare_entities;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.root.Enter(new StateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State.Callback(this.PopulateTargetsAndText));
				this.planning.DefaultState(this.planning.prepare_entities);
				this.planning.prepare_entities.Enter(delegate(PartyEvent.StatesInstance smi)
				{
					if (this.planner.Get(smi) != null && this.guest.Get(smi) != null)
					{
						smi.GoTo(this.planning.wait_for_input);
						return;
					}
					smi.GoTo(this.ending);
				});
				this.planning.wait_for_input.ToggleNotification((PartyEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.warmup.ToggleNotification((PartyEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.warmup.wait.ScheduleGoTo(60f, this.warmup.start);
				this.warmup.start.Enter(new StateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State.Callback(this.PopulateTargetsAndText)).Enter(delegate(PartyEvent.StatesInstance smi)
				{
					if (this.GetChosenRoom(smi) == null)
					{
						smi.GoTo(this.canceled);
						return;
					}
					smi.GoTo(this.partying);
				});
				this.partying.ToggleNotification((PartyEvent.StatesInstance smi) => new Notification(GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.UNDERWAY, NotificationType.Good, (List<Notification> a, object b) => GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.UNDERWAY_TOOLTIP, null, false, 0f, null, null, this.roomObject.Get(smi).transform, true, false, false)).Update(delegate(PartyEvent.StatesInstance smi, float dt)
				{
					smi.UpdateChores(this.GetChosenRoom(smi));
				}, UpdateRate.SIM_4000ms, false).ScheduleGoTo(60f, this.ending);
				this.ending.ReturnSuccess();
				this.canceled.DoNotification((PartyEvent.StatesInstance smi) => GameplayEventManager.CreateStandardCancelledNotification(this.GenerateEventPopupData(smi))).Enter(delegate(PartyEvent.StatesInstance smi)
				{
					if (this.planner.Get(smi) != null)
					{
						this.planner.Get(smi).GetComponent<Effects>().Add("NoFunAllowed", true);
					}
					if (this.guest.Get(smi) != null)
					{
						this.guest.Get(smi).GetComponent<Effects>().Add("NoFunAllowed", true);
					}
				}).ReturnFailure();
			}

			// Token: 0x0600C872 RID: 51314 RVA: 0x00427659 File Offset: 0x00425859
			public Room GetChosenRoom(PartyEvent.StatesInstance smi)
			{
				return Game.Instance.roomProber.GetRoomOfGameObject(this.roomObject.Get(smi));
			}

			// Token: 0x0600C873 RID: 51315 RVA: 0x00427678 File Offset: 0x00425878
			public override EventInfoData GenerateEventPopupData(PartyEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				Room chosenRoom = this.GetChosenRoom(smi);
				string location = (chosenRoom != null) ? chosenRoom.GetProperName() : GAMEPLAY_EVENTS.LOCATIONS.NONE_AVAILABLE.ToString();
				Effect effect = Db.Get().effects.Get("Socialized");
				Effect effect2 = Db.Get().effects.Get("NoFunAllowed");
				eventInfoData.location = location;
				eventInfoData.whenDescription = string.Format(GAMEPLAY_EVENTS.TIMES.IN_CYCLES, 0.1f);
				eventInfoData.minions = new GameObject[]
				{
					smi.sm.guest.Get(smi),
					smi.sm.planner.Get(smi)
				};
				bool flag = true;
				EventInfoData.Option option = eventInfoData.AddOption(GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.ACCEPT_OPTION_NAME, GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.ACCEPT_OPTION_DESC);
				option.callback = delegate()
				{
					smi.GoTo(smi.sm.warmup.wait);
				};
				option.AddPositiveIcon(Assets.GetSprite("overlay_materials"), Effect.CreateFullTooltip(effect, true), 1f);
				option.tooltip = GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.ACCEPT_OPTION_DESC;
				if (!flag)
				{
					option.AddInformationIcon("Cake must be built", 1f);
					option.allowed = false;
					option.tooltip = GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.ACCEPT_OPTION_INVALID_TOOLTIP;
				}
				EventInfoData.Option option2 = eventInfoData.AddOption(GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.REJECT_OPTION_NAME, GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.REJECT_OPTION_DESC);
				option2.callback = delegate()
				{
					smi.GoTo(smi.sm.canceled);
				};
				option2.AddNegativeIcon(Assets.GetSprite("overlay_decor"), Effect.CreateFullTooltip(effect2, true), 1f);
				eventInfoData.AddDefaultConsiderLaterOption(null);
				eventInfoData.SetTextParameter("host", this.planner.Get(smi).GetProperName());
				eventInfoData.SetTextParameter("dupe", this.guest.Get(smi).GetProperName());
				eventInfoData.SetTextParameter("goodEffect", effect.Name);
				eventInfoData.SetTextParameter("badEffect", effect2.Name);
				return eventInfoData;
			}

			// Token: 0x0600C874 RID: 51316 RVA: 0x004278D4 File Offset: 0x00425AD4
			public void PopulateTargetsAndText(PartyEvent.StatesInstance smi)
			{
				if (this.roomObject.Get(smi) == null)
				{
					Room room = Game.Instance.roomProber.rooms.Find((Room match) => match.roomType == Db.Get().RoomTypes.RecRoom);
					this.roomObject.Set((room != null) ? room.GetPrimaryEntities()[0] : null, smi);
				}
				if (Components.LiveMinionIdentities.Count > 0)
				{
					if (this.planner.Get(smi) == null)
					{
						MinionIdentity value = Components.LiveMinionIdentities[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count)];
						this.planner.Set(value, smi);
					}
					if (this.guest.Get(smi) == null)
					{
						MinionIdentity value2 = Components.LiveMinionIdentities[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count)];
						this.guest.Set(value2, smi);
					}
				}
			}

			// Token: 0x0400AECA RID: 44746
			public StateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.TargetParameter roomObject;

			// Token: 0x0400AECB RID: 44747
			public StateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.TargetParameter planner;

			// Token: 0x0400AECC RID: 44748
			public StateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.TargetParameter guest;

			// Token: 0x0400AECD RID: 44749
			public PartyEvent.States.PlanningStates planning;

			// Token: 0x0400AECE RID: 44750
			public PartyEvent.States.WarmupStates warmup;

			// Token: 0x0400AECF RID: 44751
			public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State partying;

			// Token: 0x0400AED0 RID: 44752
			public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State ending;

			// Token: 0x0400AED1 RID: 44753
			public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State canceled;

			// Token: 0x02003A30 RID: 14896
			public class PlanningStates : GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400EB46 RID: 60230
				public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State prepare_entities;

				// Token: 0x0400EB47 RID: 60231
				public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State wait_for_input;
			}

			// Token: 0x02003A31 RID: 14897
			public class WarmupStates : GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400EB48 RID: 60232
				public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State wait;

				// Token: 0x0400EB49 RID: 60233
				public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State start;
			}
		}
	}
}
