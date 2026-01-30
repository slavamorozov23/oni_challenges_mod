using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000BF1 RID: 3057
public class Telephone : StateMachineComponent<Telephone.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06005BC6 RID: 23494 RVA: 0x00212DFC File Offset: 0x00210FFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Components.Telephones.Add(this);
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

	// Token: 0x06005BC7 RID: 23495 RVA: 0x00212E5B File Offset: 0x0021105B
	protected override void OnCleanUp()
	{
		Components.Telephones.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06005BC8 RID: 23496 RVA: 0x00212E70 File Offset: 0x00211070
	public void AddModifierDescriptions(List<Descriptor> descs, string effect_id)
	{
		Effect effect = Db.Get().effects.Get(effect_id);
		string text;
		string text2;
		if (effect.Id == this.babbleEffect)
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_BABBLE;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_BABBLE_TOOLTIP;
		}
		else if (effect.Id == this.chatEffect)
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_CHAT;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_CHAT_TOOLTIP;
		}
		else
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_LONG_DISTANCE;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_LONG_DISTANCE_TOOLTIP;
		}
		foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
		{
			Descriptor item = new Descriptor(text.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()), text2.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Effect, false);
			item.IncreaseIndent();
			descs.Add(item);
		}
	}

	// Token: 0x06005BC9 RID: 23497 RVA: 0x00212FDC File Offset: 0x002111DC
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		this.AddModifierDescriptions(list, this.babbleEffect);
		this.AddModifierDescriptions(list, this.chatEffect);
		this.AddModifierDescriptions(list, this.longDistanceEffect);
		return list;
	}

	// Token: 0x06005BCA RID: 23498 RVA: 0x00213042 File Offset: 0x00211242
	public void HangUp()
	{
		this.isInUse = false;
		this.wasAnswered = false;
		this.RemoveTag(GameTags.LongDistanceCall);
	}

	// Token: 0x04003D10 RID: 15632
	public string babbleEffect;

	// Token: 0x04003D11 RID: 15633
	public string chatEffect;

	// Token: 0x04003D12 RID: 15634
	public string longDistanceEffect;

	// Token: 0x04003D13 RID: 15635
	public string trackingEffect;

	// Token: 0x04003D14 RID: 15636
	public bool isInUse;

	// Token: 0x04003D15 RID: 15637
	public bool wasAnswered;

	// Token: 0x02001D89 RID: 7561
	public class States : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone>
	{
		// Token: 0x0600B15B RID: 45403 RVA: 0x003DCD20 File Offset: 0x003DAF20
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			Telephone.States.CreateStatusItems();
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleRecurringChore(new Func<Telephone.StatesInstance, Chore>(this.CreateChore), null).Enter(delegate(Telephone.StatesInstance smi)
			{
				using (List<Telephone>.Enumerator enumerator = Components.Telephones.Items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.isInUse)
						{
							smi.GoTo(this.ready.speaker);
						}
					}
				}
			});
			this.ready.idle.WorkableStartTransition((Telephone.StatesInstance smi) => smi.master.GetComponent<TelephoneCallerWorkable>(), this.ready.calling.dial).TagTransition(GameTags.TelephoneRinging, this.ready.ringing, false).PlayAnim("off");
			this.ready.calling.ScheduleGoTo(15f, this.ready.talking.babbling);
			this.ready.calling.dial.PlayAnim("on_pre").OnAnimQueueComplete(this.ready.calling.animHack);
			this.ready.calling.animHack.ScheduleActionNextFrame("animHack_delay", delegate(Telephone.StatesInstance smi)
			{
				smi.GoTo(this.ready.calling.pre);
			});
			this.ready.calling.pre.PlayAnim("on").Enter(delegate(Telephone.StatesInstance smi)
			{
				this.RingAllTelephones(smi);
			}).OnAnimQueueComplete(this.ready.calling.wait);
			this.ready.calling.wait.PlayAnim("on", KAnim.PlayMode.Loop).Transition(this.ready.talking.chatting, (Telephone.StatesInstance smi) => smi.CallAnswered(), UpdateRate.SIM_4000ms);
			this.ready.ringing.PlayAnim("on_receiving", KAnim.PlayMode.Loop).Transition(this.ready.answer, (Telephone.StatesInstance smi) => smi.GetComponent<Telephone>().isInUse, UpdateRate.SIM_33ms).TagTransition(GameTags.TelephoneRinging, this.ready.speaker, true).ScheduleGoTo(15f, this.ready.speaker).Exit(delegate(Telephone.StatesInstance smi)
			{
				smi.GetComponent<Telephone>().RemoveTag(GameTags.TelephoneRinging);
			});
			this.ready.answer.PlayAnim("on_pre_loop_receiving").OnAnimQueueComplete(this.ready.talking.chatting);
			this.ready.talking.ScheduleGoTo(25f, this.ready.hangup).Enter(delegate(Telephone.StatesInstance smi)
			{
				this.UpdatePartyLine(smi);
			});
			this.ready.talking.babbling.PlayAnim("on_loop", KAnim.PlayMode.Loop).Transition(this.ready.talking.chatting, (Telephone.StatesInstance smi) => smi.CallAnswered(), UpdateRate.SIM_33ms).ToggleStatusItem(Telephone.States.babbling, null);
			this.ready.talking.chatting.PlayAnim("on_loop_pre").QueueAnim("on_loop", true, null).Transition(this.ready.talking.babbling, (Telephone.StatesInstance smi) => !smi.CallAnswered(), UpdateRate.SIM_33ms).ToggleStatusItem(Telephone.States.partyLine, null);
			this.ready.speaker.PlayAnim("on_loop_nobody", KAnim.PlayMode.Loop).Transition(this.ready, (Telephone.StatesInstance smi) => !smi.CallAnswered(), UpdateRate.SIM_4000ms).Transition(this.ready.answer, (Telephone.StatesInstance smi) => smi.GetComponent<Telephone>().isInUse, UpdateRate.SIM_33ms);
			this.ready.hangup.OnAnimQueueComplete(this.ready);
		}

		// Token: 0x0600B15C RID: 45404 RVA: 0x003DD158 File Offset: 0x003DB358
		private Chore CreateChore(Telephone.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<TelephoneCallerWorkable>();
			WorkChore<TelephoneCallerWorkable> workChore = new WorkChore<TelephoneCallerWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x0600B15D RID: 45405 RVA: 0x003DD1B8 File Offset: 0x003DB3B8
		public void UpdatePartyLine(Telephone.StatesInstance smi)
		{
			int myWorldId = smi.GetMyWorldId();
			bool flag = false;
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				telephone.RemoveTag(GameTags.TelephoneRinging);
				if (telephone.isInUse && myWorldId != telephone.GetMyWorldId())
				{
					flag = true;
					telephone.AddTag(GameTags.LongDistanceCall);
				}
			}
			Telephone component = smi.GetComponent<Telephone>();
			component.RemoveTag(GameTags.TelephoneRinging);
			if (flag)
			{
				component.AddTag(GameTags.LongDistanceCall);
			}
		}

		// Token: 0x0600B15E RID: 45406 RVA: 0x003DD260 File Offset: 0x003DB460
		public void RingAllTelephones(Telephone.StatesInstance smi)
		{
			Telephone component = smi.master.GetComponent<Telephone>();
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				if (component != telephone && telephone.GetComponent<Operational>().IsOperational)
				{
					TelephoneCallerWorkable component2 = telephone.GetComponent<TelephoneCallerWorkable>();
					if (component2 != null && component2.worker == null)
					{
						telephone.AddTag(GameTags.TelephoneRinging);
					}
				}
			}
		}

		// Token: 0x0600B15F RID: 45407 RVA: 0x003DD2FC File Offset: 0x003DB4FC
		private static void CreateStatusItems()
		{
			if (Telephone.States.partyLine == null)
			{
				Telephone.States.partyLine = new StatusItem("PartyLine", BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO, "", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
				Telephone.States.partyLine.resolveStringCallback = delegate(string str, object obj)
				{
					Telephone component = ((Telephone.StatesInstance)obj).GetComponent<Telephone>();
					int num = 0;
					foreach (Telephone telephone in Components.Telephones.Items)
					{
						if (telephone.isInUse && telephone != component)
						{
							num++;
							if (num == 1)
							{
								str = str.Replace("{Asteroid}", telephone.GetMyWorld().GetProperName());
								str = str.Replace("{Duplicant}", telephone.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
							}
						}
					}
					if (num > 1)
					{
						str = string.Format(BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO_NUM, num);
					}
					return str;
				};
				Telephone.States.partyLine.resolveTooltipCallback = delegate(string str, object obj)
				{
					Telephone component = ((Telephone.StatesInstance)obj).GetComponent<Telephone>();
					foreach (Telephone telephone in Components.Telephones.Items)
					{
						if (telephone.isInUse && telephone != component)
						{
							string text = BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO;
							text = text.Replace("{Duplicant}", telephone.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
							text = text.Replace("{Asteroid}", telephone.GetMyWorld().GetProperName());
							str = str + text + "\n";
						}
					}
					return str;
				};
			}
			if (Telephone.States.babbling == null)
			{
				Telephone.States.babbling = new StatusItem("Babbling", BUILDING.STATUSITEMS.TELEPHONE.BABBLE.NAME, BUILDING.STATUSITEMS.TELEPHONE.BABBLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
				Telephone.States.babbling.resolveTooltipCallback = delegate(string str, object obj)
				{
					Telephone.StatesInstance statesInstance = (Telephone.StatesInstance)obj;
					str = str.Replace("{Duplicant}", statesInstance.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
					return str;
				};
			}
		}

		// Token: 0x04008B8F RID: 35727
		private GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State unoperational;

		// Token: 0x04008B90 RID: 35728
		private Telephone.States.ReadyStates ready;

		// Token: 0x04008B91 RID: 35729
		private static StatusItem partyLine;

		// Token: 0x04008B92 RID: 35730
		private static StatusItem babbling;

		// Token: 0x02002A46 RID: 10822
		public class ReadyStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
		{
			// Token: 0x0400BAC6 RID: 47814
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State idle;

			// Token: 0x0400BAC7 RID: 47815
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State ringing;

			// Token: 0x0400BAC8 RID: 47816
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State answer;

			// Token: 0x0400BAC9 RID: 47817
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State speaker;

			// Token: 0x0400BACA RID: 47818
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State hangup;

			// Token: 0x0400BACB RID: 47819
			public Telephone.States.ReadyStates.CallingStates calling;

			// Token: 0x0400BACC RID: 47820
			public Telephone.States.ReadyStates.TalkingStates talking;

			// Token: 0x02003A54 RID: 14932
			public class CallingStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
			{
				// Token: 0x0400EB97 RID: 60311
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State dial;

				// Token: 0x0400EB98 RID: 60312
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State animHack;

				// Token: 0x0400EB99 RID: 60313
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State pre;

				// Token: 0x0400EB9A RID: 60314
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State wait;
			}

			// Token: 0x02003A55 RID: 14933
			public class TalkingStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
			{
				// Token: 0x0400EB9B RID: 60315
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State babbling;

				// Token: 0x0400EB9C RID: 60316
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State chatting;
			}
		}
	}

	// Token: 0x02001D8A RID: 7562
	public class StatesInstance : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.GameInstance
	{
		// Token: 0x0600B165 RID: 45413 RVA: 0x003DD49A File Offset: 0x003DB69A
		public StatesInstance(Telephone smi) : base(smi)
		{
		}

		// Token: 0x0600B166 RID: 45414 RVA: 0x003DD4A4 File Offset: 0x003DB6A4
		public bool CallAnswered()
		{
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				if (telephone.isInUse && telephone != base.smi.GetComponent<Telephone>())
				{
					telephone.wasAnswered = true;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B167 RID: 45415 RVA: 0x003DD520 File Offset: 0x003DB720
		public bool CallEnded()
		{
			using (List<Telephone>.Enumerator enumerator = Components.Telephones.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.isInUse)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
