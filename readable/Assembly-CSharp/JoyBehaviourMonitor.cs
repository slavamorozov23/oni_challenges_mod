using System;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000A32 RID: 2610
public class JoyBehaviourMonitor : GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance>
{
	// Token: 0x06004C3B RID: 19515 RVA: 0x001BAFA4 File Offset: 0x001B91A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.EventHandler(GameHashes.TagsChanged, delegate(JoyBehaviourMonitor.Instance smi, object data)
		{
			TagChangedEventData value = ((Boxed<TagChangedEventData>)data).value;
			if (!value.added)
			{
				return;
			}
			if (value.tag == GameTags.PleasantConversation && UnityEngine.Random.Range(0f, 100f) <= 1f)
			{
				smi.GoToOverjoyed();
			}
			smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PleasantConversation);
		}).EventHandler(GameHashes.ScheduleBlocksTick, delegate(JoyBehaviourMonitor.Instance smi)
		{
			if (smi.ShouldBeOverjoyed())
			{
				smi.GoToOverjoyed();
			}
		});
		this.overjoyed.Transition(this.neutral, (JoyBehaviourMonitor.Instance smi) => GameClock.Instance.GetTime() >= smi.transitionTime, UpdateRate.SIM_200ms).ToggleExpression((JoyBehaviourMonitor.Instance smi) => smi.happyExpression).ToggleAnims((JoyBehaviourMonitor.Instance smi) => smi.happyLocoAnim).ToggleAnims((JoyBehaviourMonitor.Instance smi) => smi.happyLocoWalkAnim).ToggleTag(GameTags.Overjoyed).Exit(delegate(JoyBehaviourMonitor.Instance smi)
		{
			smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PleasantConversation);
		}).OnSignal(this.exitEarly, this.neutral);
	}

	// Token: 0x040032A1 RID: 12961
	public StateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.Signal exitEarly;

	// Token: 0x040032A2 RID: 12962
	public GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x040032A3 RID: 12963
	public GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.State overjoyed;

	// Token: 0x02001AFF RID: 6911
	public new class Instance : GameStateMachine<JoyBehaviourMonitor, JoyBehaviourMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A804 RID: 43012 RVA: 0x003BE200 File Offset: 0x003BC400
		public Instance(IStateMachineTarget master, string happy_loco_anim, string happy_loco_walk_anim, Expression happy_expression) : base(master)
		{
			this.happyLocoAnim = happy_loco_anim;
			this.happyLocoWalkAnim = happy_loco_walk_anim;
			this.happyExpression = happy_expression;
			Attributes attributes = base.gameObject.GetAttributes();
			this.expectationAttribute = attributes.Add(Db.Get().Attributes.QualityOfLifeExpectation);
			this.qolAttribute = Db.Get().Attributes.QualityOfLife.Lookup(base.gameObject);
		}

		// Token: 0x0600A805 RID: 43013 RVA: 0x003BE288 File Offset: 0x003BC488
		public bool ShouldBeOverjoyed()
		{
			float totalValue = this.qolAttribute.GetTotalValue();
			float totalValue2 = this.expectationAttribute.GetTotalValue();
			float num = totalValue - totalValue2;
			if (num >= TRAITS.JOY_REACTIONS.MIN_MORALE_EXCESS)
			{
				float num2 = MathUtil.ReRange(num, TRAITS.JOY_REACTIONS.MIN_MORALE_EXCESS, TRAITS.JOY_REACTIONS.MAX_MORALE_EXCESS, TRAITS.JOY_REACTIONS.MIN_REACTION_CHANCE, TRAITS.JOY_REACTIONS.MAX_REACTION_CHANCE);
				return UnityEngine.Random.Range(0f, 100f) <= num2;
			}
			return false;
		}

		// Token: 0x0600A806 RID: 43014 RVA: 0x003BE2E9 File Offset: 0x003BC4E9
		public void GoToOverjoyed()
		{
			base.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
			base.smi.GoTo(base.smi.sm.overjoyed);
		}

		// Token: 0x04008370 RID: 33648
		public string happyLocoAnim = "";

		// Token: 0x04008371 RID: 33649
		public string happyLocoWalkAnim = "";

		// Token: 0x04008372 RID: 33650
		public Expression happyExpression;

		// Token: 0x04008373 RID: 33651
		[Serialize]
		public float transitionTime;

		// Token: 0x04008374 RID: 33652
		private AttributeInstance expectationAttribute;

		// Token: 0x04008375 RID: 33653
		private AttributeInstance qolAttribute;
	}
}
