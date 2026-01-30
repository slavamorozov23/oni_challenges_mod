using System;
using System.Collections.Generic;

// Token: 0x020005AF RID: 1455
public class CreatureThoughtGraph : GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>
{
	// Token: 0x06002167 RID: 8551 RVA: 0x000C20A4 File Offset: 0x000C02A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.initialdelay;
		this.initialdelay.ScheduleGoTo(1f, this.nothoughts);
		this.nothoughts.OnSignal(this.thoughtsChanged, this.displayingthought, (CreatureThoughtGraph.Instance smi, StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.SignalParameter param) => smi.HasThoughts()).OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (CreatureThoughtGraph.Instance smi, StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.SignalParameter param) => smi.HasThoughts());
		this.displayingthought.Enter("CreateBubble", delegate(CreatureThoughtGraph.Instance smi)
		{
			smi.CreateBubble();
		}).Exit("DestroyBubble", delegate(CreatureThoughtGraph.Instance smi)
		{
			smi.DestroyBubble();
		}).ScheduleGoTo((CreatureThoughtGraph.Instance smi) => this.thoughtDisplayTime.Get(smi), this.cooldown);
		this.cooldown.OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (CreatureThoughtGraph.Instance smi, StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.SignalParameter param) => smi.HasImmediateThought()).ScheduleGoTo(20f, this.nothoughts);
	}

	// Token: 0x04001377 RID: 4983
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Signal thoughtsChanged;

	// Token: 0x04001378 RID: 4984
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Signal thoughtsChangedImmediate;

	// Token: 0x04001379 RID: 4985
	public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.FloatParameter thoughtDisplayTime;

	// Token: 0x0400137A RID: 4986
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State initialdelay;

	// Token: 0x0400137B RID: 4987
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State nothoughts;

	// Token: 0x0400137C RID: 4988
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State displayingthought;

	// Token: 0x0400137D RID: 4989
	public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State cooldown;

	// Token: 0x0200143A RID: 5178
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200143B RID: 5179
	public new class Instance : GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.GameInstance
	{
		// Token: 0x06008F00 RID: 36608 RVA: 0x0036A259 File Offset: 0x00368459
		public Instance(IStateMachineTarget master, CreatureThoughtGraph.Def def) : base(master, def)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x06008F01 RID: 36609 RVA: 0x0036A280 File Offset: 0x00368480
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
		}

		// Token: 0x06008F02 RID: 36610 RVA: 0x0036A288 File Offset: 0x00368488
		public bool HasThoughts()
		{
			return this.thoughts.Count > 0;
		}

		// Token: 0x06008F03 RID: 36611 RVA: 0x0036A298 File Offset: 0x00368498
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

		// Token: 0x06008F04 RID: 36612 RVA: 0x0036A2D8 File Offset: 0x003684D8
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

		// Token: 0x06008F05 RID: 36613 RVA: 0x0036A335 File Offset: 0x00368535
		public void RemoveThought(Thought thought)
		{
			if (!this.thoughts.Contains(thought))
			{
				return;
			}
			this.thoughts.Remove(thought);
			base.sm.thoughtsChanged.Trigger(base.smi);
		}

		// Token: 0x06008F06 RID: 36614 RVA: 0x0036A369 File Offset: 0x00368569
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

		// Token: 0x06008F07 RID: 36615 RVA: 0x0036A398 File Offset: 0x00368598
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

		// Token: 0x06008F08 RID: 36616 RVA: 0x0036A471 File Offset: 0x00368671
		public void DestroyBubble()
		{
			NameDisplayScreen.Instance.SetThoughtBubbleDisplay(base.gameObject, false, null, null, null);
			NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(base.gameObject, false, null, null, null, null);
		}

		// Token: 0x04006DE3 RID: 28131
		private List<Thought> thoughts = new List<Thought>();

		// Token: 0x04006DE4 RID: 28132
		public Thought currentThought;
	}
}
