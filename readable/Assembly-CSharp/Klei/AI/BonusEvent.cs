using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200103F RID: 4159
	public class BonusEvent : GameplayEvent<BonusEvent.StatesInstance>
	{
		// Token: 0x060080FF RID: 33023 RVA: 0x0033D92C File Offset: 0x0033BB2C
		public BonusEvent(string id, string overrideEffect = null, int numTimesAllowed = 1, bool preSelectMinion = false, int priority = 0) : base(id, priority, 0, null, null)
		{
			this.title = Strings.Get("STRINGS.GAMEPLAY_EVENTS.BONUS." + id.ToUpper() + ".NAME");
			this.description = Strings.Get("STRINGS.GAMEPLAY_EVENTS.BONUS." + id.ToUpper() + ".DESCRIPTION");
			this.effect = ((overrideEffect != null) ? overrideEffect : id);
			this.numTimesAllowed = numTimesAllowed;
			this.preSelectMinion = preSelectMinion;
			this.animFileName = id.ToLower() + "_kanim";
			base.AddPrecondition(GameplayEventPreconditions.Instance.LiveMinions(1));
		}

		// Token: 0x06008100 RID: 33024 RVA: 0x0033D9D8 File Offset: 0x0033BBD8
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new BonusEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x06008101 RID: 33025 RVA: 0x0033D9E2 File Offset: 0x0033BBE2
		public BonusEvent TriggerOnNewBuilding(int triggerCount, params string[] buildings)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.NewBuilding;
			this.buildingTrigger = new HashSet<Tag>(buildings.ToTagList());
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x06008102 RID: 33026 RVA: 0x0033DA18 File Offset: 0x0033BC18
		public BonusEvent TriggerOnUseBuilding(int triggerCount, params string[] buildings)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.UseBuilding;
			this.buildingTrigger = new HashSet<Tag>(buildings.ToTagList());
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x06008103 RID: 33027 RVA: 0x0033DA4E File Offset: 0x0033BC4E
		public BonusEvent TriggerOnWorkableComplete(int triggerCount, params Type[] types)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.WorkableComplete;
			this.workableType = new HashSet<Type>(types);
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x06008104 RID: 33028 RVA: 0x0033DA7F File Offset: 0x0033BC7F
		public BonusEvent SetExtraCondition(BonusEvent.ConditionFn extraCondition)
		{
			this.extraCondition = extraCondition;
			return this;
		}

		// Token: 0x06008105 RID: 33029 RVA: 0x0033DA89 File Offset: 0x0033BC89
		public BonusEvent SetRoomConstraints(bool hasOwnableInRoom, params RoomType[] types)
		{
			this.roomHasOwnable = hasOwnableInRoom;
			this.roomRestrictions = ((types == null) ? null : new HashSet<RoomType>(types));
			return this;
		}

		// Token: 0x06008106 RID: 33030 RVA: 0x0033DAA5 File Offset: 0x0033BCA5
		public string GetEffectTooltip(Effect effect)
		{
			return effect.Name + "\n\n" + Effect.CreateTooltip(effect, true, "\n    • ", true);
		}

		// Token: 0x06008107 RID: 33031 RVA: 0x0033DAC4 File Offset: 0x0033BCC4
		public override Sprite GetDisplaySprite()
		{
			Effect effect = Db.Get().effects.Get(this.effect);
			if (effect.SelfModifiers.Count > 0)
			{
				return Assets.GetSprite(Db.Get().Attributes.TryGet(effect.SelfModifiers[0].AttributeId).uiFullColourSprite);
			}
			return null;
		}

		// Token: 0x06008108 RID: 33032 RVA: 0x0033DB28 File Offset: 0x0033BD28
		public override string GetDisplayString()
		{
			Effect effect = Db.Get().effects.Get(this.effect);
			if (effect.SelfModifiers.Count > 0)
			{
				return Db.Get().Attributes.TryGet(effect.SelfModifiers[0].AttributeId).Name;
			}
			return null;
		}

		// Token: 0x040061A8 RID: 25000
		public const int PRE_SELECT_MINION_TIMEOUT = 5;

		// Token: 0x040061A9 RID: 25001
		public string effect;

		// Token: 0x040061AA RID: 25002
		public bool preSelectMinion;

		// Token: 0x040061AB RID: 25003
		public int numTimesToTrigger;

		// Token: 0x040061AC RID: 25004
		public BonusEvent.TriggerType triggerType;

		// Token: 0x040061AD RID: 25005
		public HashSet<Tag> buildingTrigger;

		// Token: 0x040061AE RID: 25006
		public HashSet<Type> workableType;

		// Token: 0x040061AF RID: 25007
		public HashSet<RoomType> roomRestrictions;

		// Token: 0x040061B0 RID: 25008
		public BonusEvent.ConditionFn extraCondition;

		// Token: 0x040061B1 RID: 25009
		public bool roomHasOwnable;

		// Token: 0x02002735 RID: 10037
		public enum TriggerType
		{
			// Token: 0x0400AE8E RID: 44686
			None,
			// Token: 0x0400AE8F RID: 44687
			NewBuilding,
			// Token: 0x0400AE90 RID: 44688
			UseBuilding,
			// Token: 0x0400AE91 RID: 44689
			WorkableComplete,
			// Token: 0x0400AE92 RID: 44690
			AchievementUnlocked
		}

		// Token: 0x02002736 RID: 10038
		// (Invoke) Token: 0x0600C820 RID: 51232
		public delegate bool ConditionFn(BonusEvent.GameplayEventData data);

		// Token: 0x02002737 RID: 10039
		public class GameplayEventData
		{
			// Token: 0x0400AE93 RID: 44691
			public GameHashes eventTrigger;

			// Token: 0x0400AE94 RID: 44692
			public BuildingComplete building;

			// Token: 0x0400AE95 RID: 44693
			public Workable workable;

			// Token: 0x0400AE96 RID: 44694
			public WorkerBase worker;
		}

		// Token: 0x02002738 RID: 10040
		public class States : GameplayEventStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, BonusEvent>
		{
			// Token: 0x0600C824 RID: 51236 RVA: 0x0042561C File Offset: 0x0042381C
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.load;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.load.Enter(new StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State.Callback(this.AssignPreSelectedMinionIfNeeded)).Transition(this.waitNewBuilding, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.NewBuilding, UpdateRate.SIM_200ms).Transition(this.waitUseBuilding, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.UseBuilding, UpdateRate.SIM_200ms).Transition(this.waitforWorkables, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.WorkableComplete, UpdateRate.SIM_200ms).Transition(this.waitForAchievement, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.AchievementUnlocked, UpdateRate.SIM_200ms).Transition(this.immediate, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.None, UpdateRate.SIM_200ms);
				this.waitNewBuilding.EventHandlerTransition(GameHashes.NewBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.BuildingEventTrigger));
				this.waitUseBuilding.EventHandlerTransition(GameHashes.UseBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.BuildingEventTrigger));
				this.waitforWorkables.EventHandlerTransition(GameHashes.UseBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.WorkableEventTrigger));
				this.immediate.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					GameObject gameObject = smi.sm.chosen.Get(smi);
					if (gameObject == null)
					{
						gameObject = smi.gameplayEvent.GetRandomMinionPrioritizeFiltered().gameObject;
						smi.sm.chosen.Set(gameObject, smi, false);
					}
				}).GoTo(this.active);
				this.active.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					smi.sm.chosen.Get(smi).GetComponent<Effects>().Add(smi.gameplayEvent.effect, true);
				}).Enter(delegate(BonusEvent.StatesInstance smi)
				{
					base.MonitorStart(this.chosen, smi);
				}).Exit(delegate(BonusEvent.StatesInstance smi)
				{
					base.MonitorStop(this.chosen, smi);
				}).ScheduleGoTo(delegate(BonusEvent.StatesInstance smi)
				{
					Effect effect = this.GetEffect(smi);
					if (effect != null)
					{
						return effect.duration;
					}
					return 0f;
				}, this.ending).DefaultState(this.active.notify).OnTargetLost(this.chosen, this.ending).Target(this.chosen).TagTransition(GameTags.Dead, this.ending, false);
				this.active.notify.ToggleNotification((BonusEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.active.seenNotification.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					smi.eventInstance.seenNotification = true;
				});
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600C825 RID: 51237 RVA: 0x004258C0 File Offset: 0x00423AC0
			public override EventInfoData GenerateEventPopupData(BonusEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				GameObject gameObject = smi.sm.chosen.Get(smi);
				if (gameObject == null)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						"Minion not set for " + smi.gameplayEvent.Id
					});
					return null;
				}
				Effect effect = this.GetEffect(smi);
				if (effect == null)
				{
					return null;
				}
				eventInfoData.clickFocus = gameObject.transform;
				eventInfoData.minions = new GameObject[]
				{
					gameObject
				};
				eventInfoData.SetTextParameter("dupe", gameObject.GetProperName());
				if (smi.building != null)
				{
					eventInfoData.SetTextParameter("building", UI.FormatAsLink(smi.building.GetProperName(), smi.building.GetProperName().ToUpper()));
				}
				EventInfoData.Option option = eventInfoData.AddDefaultOption(delegate
				{
					smi.GoTo(smi.sm.active.seenNotification);
				});
				GAMEPLAY_EVENTS.BONUS_EVENT_DESCRIPTION.Replace("{effects}", Effect.CreateTooltip(effect, false, " ", false)).Replace("{durration}", GameUtil.GetFormattedCycles(effect.duration, "F1", false));
				foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
				{
					Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
					string text = string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, attribute.Name, attributeModifier.GetFormattedString());
					text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_TOTAL, GameUtil.GetFormattedCycles(effect.duration, "F1", false));
					Sprite sprite = Assets.GetSprite(attribute.uiFullColourSprite);
					option.AddPositiveIcon(sprite, text, 1.75f);
				}
				return eventInfoData;
			}

			// Token: 0x0600C826 RID: 51238 RVA: 0x00425B08 File Offset: 0x00423D08
			private void AssignPreSelectedMinionIfNeeded(BonusEvent.StatesInstance smi)
			{
				if (smi.gameplayEvent.preSelectMinion && smi.sm.chosen.Get(smi) == null)
				{
					smi.sm.chosen.Set(smi.gameplayEvent.GetRandomMinionPrioritizeFiltered().gameObject, smi, false);
					smi.timesTriggered = 0;
				}
			}

			// Token: 0x0600C827 RID: 51239 RVA: 0x00425B68 File Offset: 0x00423D68
			private bool IsCorrectMinion(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				if (!smi.gameplayEvent.preSelectMinion || !(smi.sm.chosen.Get(smi) != gameplayEventData.worker.gameObject))
				{
					return true;
				}
				if (GameUtil.GetCurrentTimeInCycles() - smi.lastTriggered > 5f && smi.PercentageUntilTriggered() < 0.5f)
				{
					smi.sm.chosen.Set(gameplayEventData.worker.gameObject, smi, false);
					smi.timesTriggered = 0;
					return true;
				}
				return false;
			}

			// Token: 0x0600C828 RID: 51240 RVA: 0x00425BF0 File Offset: 0x00423DF0
			private bool OtherConditionsAreSatisfied(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				if (smi.gameplayEvent.roomRestrictions != null)
				{
					Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(gameplayEventData.worker.gameObject);
					if (roomOfGameObject == null)
					{
						return false;
					}
					if (!smi.gameplayEvent.roomRestrictions.Contains(roomOfGameObject.roomType))
					{
						return false;
					}
					if (smi.gameplayEvent.roomHasOwnable)
					{
						bool flag = false;
						using (List<Ownables>.Enumerator enumerator = roomOfGameObject.GetOwners().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.gameObject == gameplayEventData.worker.gameObject)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							return false;
						}
					}
				}
				return smi.gameplayEvent.extraCondition == null || smi.gameplayEvent.extraCondition(gameplayEventData);
			}

			// Token: 0x0600C829 RID: 51241 RVA: 0x00425CD8 File Offset: 0x00423ED8
			private bool IncrementAndTrigger(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				smi.timesTriggered++;
				smi.lastTriggered = GameUtil.GetCurrentTimeInCycles();
				if (smi.timesTriggered < smi.gameplayEvent.numTimesToTrigger)
				{
					return false;
				}
				smi.building = gameplayEventData.building;
				smi.sm.chosen.Set(gameplayEventData.worker.gameObject, smi, false);
				return true;
			}

			// Token: 0x0600C82A RID: 51242 RVA: 0x00425D40 File Offset: 0x00423F40
			private bool BuildingEventTrigger(BonusEvent.StatesInstance smi, object data)
			{
				BonusEvent.GameplayEventData gameplayEventData = data as BonusEvent.GameplayEventData;
				if (gameplayEventData == null)
				{
					return false;
				}
				this.AssignPreSelectedMinionIfNeeded(smi);
				return !(gameplayEventData.building == null) && (smi.gameplayEvent.buildingTrigger.Count <= 0 || smi.gameplayEvent.buildingTrigger.Contains(gameplayEventData.building.prefabid.PrefabID())) && this.OtherConditionsAreSatisfied(smi, gameplayEventData) && this.IsCorrectMinion(smi, gameplayEventData) && this.IncrementAndTrigger(smi, gameplayEventData);
			}

			// Token: 0x0600C82B RID: 51243 RVA: 0x00425DC8 File Offset: 0x00423FC8
			private bool WorkableEventTrigger(BonusEvent.StatesInstance smi, object data)
			{
				BonusEvent.GameplayEventData gameplayEventData = data as BonusEvent.GameplayEventData;
				if (gameplayEventData == null)
				{
					return false;
				}
				this.AssignPreSelectedMinionIfNeeded(smi);
				return (smi.gameplayEvent.workableType.Count <= 0 || smi.gameplayEvent.workableType.Contains(gameplayEventData.workable.GetType())) && this.OtherConditionsAreSatisfied(smi, gameplayEventData) && this.IsCorrectMinion(smi, gameplayEventData) && this.IncrementAndTrigger(smi, gameplayEventData);
			}

			// Token: 0x0600C82C RID: 51244 RVA: 0x00425E3A File Offset: 0x0042403A
			private bool ChosenMinionDied(BonusEvent.StatesInstance smi, object data)
			{
				return smi.sm.chosen.Get(smi) == data as GameObject;
			}

			// Token: 0x0600C82D RID: 51245 RVA: 0x00425E58 File Offset: 0x00424058
			private Effect GetEffect(BonusEvent.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.chosen.Get(smi);
				if (gameObject == null)
				{
					return null;
				}
				EffectInstance effectInstance = gameObject.GetComponent<Effects>().Get(smi.gameplayEvent.effect);
				if (effectInstance == null)
				{
					global::Debug.LogWarning(string.Format("Effect {0} not found on {1} in BonusEvent", smi.gameplayEvent.effect, gameObject));
					return null;
				}
				return effectInstance.effect;
			}

			// Token: 0x0400AE97 RID: 44695
			public StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.TargetParameter chosen;

			// Token: 0x0400AE98 RID: 44696
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State load;

			// Token: 0x0400AE99 RID: 44697
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitNewBuilding;

			// Token: 0x0400AE9A RID: 44698
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitUseBuilding;

			// Token: 0x0400AE9B RID: 44699
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitForAchievement;

			// Token: 0x0400AE9C RID: 44700
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitforWorkables;

			// Token: 0x0400AE9D RID: 44701
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State immediate;

			// Token: 0x0400AE9E RID: 44702
			public BonusEvent.States.ActiveStates active;

			// Token: 0x0400AE9F RID: 44703
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State ending;

			// Token: 0x02003A21 RID: 14881
			public class ActiveStates : GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400EB18 RID: 60184
				public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State notify;

				// Token: 0x0400EB19 RID: 60185
				public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State seenNotification;
			}
		}

		// Token: 0x02002739 RID: 10041
		public class StatesInstance : GameplayEventStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, BonusEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600C833 RID: 51251 RVA: 0x00425F1B File Offset: 0x0042411B
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, BonusEvent bonusEvent) : base(master, eventInstance, bonusEvent)
			{
				this.lastTriggered = GameUtil.GetCurrentTimeInCycles();
			}

			// Token: 0x0600C834 RID: 51252 RVA: 0x00425F31 File Offset: 0x00424131
			public float PercentageUntilTriggered()
			{
				return (float)this.timesTriggered / (float)base.smi.gameplayEvent.numTimesToTrigger;
			}

			// Token: 0x0400AEA0 RID: 44704
			[Serialize]
			public int timesTriggered;

			// Token: 0x0400AEA1 RID: 44705
			[Serialize]
			public float lastTriggered;

			// Token: 0x0400AEA2 RID: 44706
			public BuildingComplete building;
		}
	}
}
