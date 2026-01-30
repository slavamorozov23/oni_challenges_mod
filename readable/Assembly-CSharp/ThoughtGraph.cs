using System;
using System.Collections.Generic;

// Token: 0x0200064A RID: 1610
public class ThoughtGraph : GameStateMachine<ThoughtGraph, ThoughtGraph.Instance>
{
	// Token: 0x06002738 RID: 10040 RVA: 0x000E16B0 File Offset: 0x000DF8B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.initialdelay;
		this.initialdelay.ScheduleGoTo(1f, this.nothoughts);
		this.nothoughts.OnSignal(this.thoughtsChanged, this.displayingthought, (ThoughtGraph.Instance smi, StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.SignalParameter param) => smi.HasThoughts()).OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (ThoughtGraph.Instance smi, StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.SignalParameter param) => smi.HasThoughts());
		this.displayingthought.DefaultState(this.displayingthought.pre).Enter("CreateBubble", delegate(ThoughtGraph.Instance smi)
		{
			smi.CreateBubble();
		}).Exit("DestroyBubble", delegate(ThoughtGraph.Instance smi)
		{
			smi.DestroyBubble();
		}).ScheduleGoTo((ThoughtGraph.Instance smi) => this.thoughtDisplayTime.Get(smi), this.displayingthought.finishing);
		this.displayingthought.pre.ScheduleGoTo((ThoughtGraph.Instance smi) => TuningData<ThoughtGraph.Tuning>.Get().preLengthInSeconds, this.displayingthought.talking);
		this.displayingthought.talking.Enter(new StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State.Callback(ThoughtGraph.BeginTalking));
		this.displayingthought.finishing.EnterTransition(this.cooldown, (ThoughtGraph.Instance smi) => !smi.Kpid.HasTag(GameTags.DoNotInterruptMe)).TagTransition(GameTags.DoNotInterruptMe, this.cooldown, true);
		this.cooldown.OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (ThoughtGraph.Instance smi, StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.SignalParameter param) => smi.HasImmediateThought()).ScheduleGoTo(20f, this.nothoughts);
	}

	// Token: 0x06002739 RID: 10041 RVA: 0x000E18AE File Offset: 0x000DFAAE
	private static void BeginTalking(ThoughtGraph.Instance smi)
	{
		if (smi.currentThought == null)
		{
			return;
		}
		if (SpeechMonitor.IsAllowedToPlaySpeech(smi.Kpid, smi.AnimController))
		{
			smi.currentThought.PlayAsSpeech(smi.SpeechMonitorInstance);
		}
	}

	// Token: 0x0400172F RID: 5935
	public StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.Signal thoughtsChanged;

	// Token: 0x04001730 RID: 5936
	public StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.Signal thoughtsChangedImmediate;

	// Token: 0x04001731 RID: 5937
	public StateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.FloatParameter thoughtDisplayTime;

	// Token: 0x04001732 RID: 5938
	public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State initialdelay;

	// Token: 0x04001733 RID: 5939
	public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State nothoughts;

	// Token: 0x04001734 RID: 5940
	public ThoughtGraph.DisplayingThoughtState displayingthought;

	// Token: 0x04001735 RID: 5941
	public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State cooldown;

	// Token: 0x02001527 RID: 5415
	public class Tuning : TuningData<ThoughtGraph.Tuning>
	{
		// Token: 0x040070DF RID: 28895
		public float preLengthInSeconds;
	}

	// Token: 0x02001528 RID: 5416
	public class DisplayingThoughtState : GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040070E0 RID: 28896
		public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State pre;

		// Token: 0x040070E1 RID: 28897
		public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State talking;

		// Token: 0x040070E2 RID: 28898
		public GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.State finishing;
	}

	// Token: 0x02001529 RID: 5417
	public new class Instance : GameStateMachine<ThoughtGraph, ThoughtGraph.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x0600925C RID: 37468 RVA: 0x00373C0C File Offset: 0x00371E0C
		// (set) Token: 0x0600925D RID: 37469 RVA: 0x00373C14 File Offset: 0x00371E14
		public KPrefabID Kpid { get; private set; }

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x0600925E RID: 37470 RVA: 0x00373C1D File Offset: 0x00371E1D
		// (set) Token: 0x0600925F RID: 37471 RVA: 0x00373C25 File Offset: 0x00371E25
		public KBatchedAnimController AnimController { get; private set; }

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06009260 RID: 37472 RVA: 0x00373C2E File Offset: 0x00371E2E
		public SpeechMonitor.Instance SpeechMonitorInstance
		{
			get
			{
				if (this.speechMonitorInstance == null)
				{
					this.speechMonitorInstance = base.master.gameObject.GetSMI<SpeechMonitor.Instance>();
				}
				return this.speechMonitorInstance;
			}
		}

		// Token: 0x06009261 RID: 37473 RVA: 0x00373C54 File Offset: 0x00371E54
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.Kpid = master.GetComponent<KPrefabID>();
			this.AnimController = master.GetComponent<KBatchedAnimController>();
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x06009262 RID: 37474 RVA: 0x00373C92 File Offset: 0x00371E92
		public bool HasThoughts()
		{
			return this.thoughts.Count > 0;
		}

		// Token: 0x06009263 RID: 37475 RVA: 0x00373CA4 File Offset: 0x00371EA4
		public bool HasImmediateThought()
		{
			bool result = false;
			for (int i = 0; i < this.thoughts.Count; i++)
			{
				if (this.thoughts[i].showImmediately)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06009264 RID: 37476 RVA: 0x00373CE4 File Offset: 0x00371EE4
		public void AddThought(Thought thought)
		{
			if (this.thoughts.Contains(thought))
			{
				return;
			}
			this.thoughts.Add(thought);
			if (thought.showImmediately)
			{
				base.sm.thoughtsChangedImmediate.Trigger(base.smi);
				return;
			}
			base.sm.thoughtsChanged.Trigger(base.smi);
		}

		// Token: 0x06009265 RID: 37477 RVA: 0x00373D41 File Offset: 0x00371F41
		public void RemoveThought(Thought thought)
		{
			if (!this.thoughts.Contains(thought))
			{
				return;
			}
			this.thoughts.Remove(thought);
			base.sm.thoughtsChanged.Trigger(base.smi);
		}

		// Token: 0x06009266 RID: 37478 RVA: 0x00373D75 File Offset: 0x00371F75
		private int SortThoughts(Thought a, Thought b)
		{
			if (a.showImmediately == b.showImmediately)
			{
				return b.priority.CompareTo(a.priority);
			}
			if (!a.showImmediately)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06009267 RID: 37479 RVA: 0x00373DA4 File Offset: 0x00371FA4
		public void CreateBubble()
		{
			if (this.thoughts.Count == 0)
			{
				return;
			}
			this.thoughts.Sort(new Comparison<Thought>(this.SortThoughts));
			Thought thought = this.thoughts[0];
			if (thought.modeSprite != null)
			{
				NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(base.gameObject, true, thought.hoverText, thought.bubbleSprite, thought.sprite, thought.modeSprite);
			}
			else
			{
				NameDisplayScreen.Instance.SetThoughtBubbleDisplay(base.gameObject, true, thought.hoverText, thought.bubbleSprite, thought.sprite);
			}
			base.sm.thoughtDisplayTime.Set(thought.showTime, this, false);
			this.currentThought = thought;
			if (thought.showImmediately)
			{
				this.thoughts.RemoveAt(0);
			}
		}

		// Token: 0x06009268 RID: 37480 RVA: 0x00373E7D File Offset: 0x0037207D
		public void DestroyBubble()
		{
			NameDisplayScreen.Instance.SetThoughtBubbleDisplay(base.gameObject, false, null, null, null);
			NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(base.gameObject, false, null, null, null, null);
		}

		// Token: 0x040070E3 RID: 28899
		private List<Thought> thoughts = new List<Thought>();

		// Token: 0x040070E4 RID: 28900
		public Thought currentThought;

		// Token: 0x040070E7 RID: 28903
		private SpeechMonitor.Instance speechMonitorInstance;
	}
}
