using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020005C3 RID: 1475
public class LureableMonitor : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>
{
	// Token: 0x060021D3 RID: 8659 RVA: 0x000C49D8 File Offset: 0x000C2BD8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		this.cooldown.ScheduleGoTo((LureableMonitor.Instance smi) => smi.def.cooldown, this.nolure);
		this.nolure.PreBrainUpdate(delegate(LureableMonitor.Instance smi)
		{
			smi.FindLure();
		}).ParamTransition<GameObject>(this.targetLure, this.haslure, (LureableMonitor.Instance smi, GameObject p) => p != null);
		this.haslure.ParamTransition<GameObject>(this.targetLure, this.nolure, (LureableMonitor.Instance smi, GameObject p) => p == null).PreBrainUpdate(delegate(LureableMonitor.Instance smi)
		{
			smi.FindLure();
		}).ToggleBehaviour(GameTags.Creatures.MoveToLure, (LureableMonitor.Instance smi) => smi.HasLure(), delegate(LureableMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
	}

	// Token: 0x040013B7 RID: 5047
	public StateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.TargetParameter targetLure;

	// Token: 0x040013B8 RID: 5048
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State nolure;

	// Token: 0x040013B9 RID: 5049
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State haslure;

	// Token: 0x040013BA RID: 5050
	public GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.State cooldown;

	// Token: 0x0200147F RID: 5247
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06008FFE RID: 36862 RVA: 0x0036D434 File Offset: 0x0036B634
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			foreach (Tag a in this.lures)
			{
				if (a == GameTags.Creatures.FlyersLure)
				{
					list.Add(new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_FLYING_TRAP, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_FLYING_TRAP, Descriptor.DescriptorType.Effect, false));
				}
				else if (a == GameTags.Creatures.FishTrapLure)
				{
					list.Add(new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_FISH_TRAP, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_FISH_TRAP, Descriptor.DescriptorType.Effect, false));
				}
			}
			return list;
		}

		// Token: 0x04006ECC RID: 28364
		public float cooldown = 20f;

		// Token: 0x04006ECD RID: 28365
		public Tag[] lures;
	}

	// Token: 0x02001480 RID: 5248
	public new class Instance : GameStateMachine<LureableMonitor, LureableMonitor.Instance, IStateMachineTarget, LureableMonitor.Def>.GameInstance
	{
		// Token: 0x06009000 RID: 36864 RVA: 0x0036D4D6 File Offset: 0x0036B6D6
		public Instance(IStateMachineTarget master, LureableMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009001 RID: 36865 RVA: 0x0036D4E0 File Offset: 0x0036B6E0
		private static Util.IterationInstruction FindLureCounter(object obj, ref LureableMonitor.Instance.FindLureCounterContext context)
		{
			Lure.Instance instance = obj as Lure.Instance;
			if (instance == null || !instance.IsActive() || !instance.HasAnyLure(context.inst.def.lures))
			{
				return Util.IterationInstruction.Continue;
			}
			int navigationCost = context.inst.navigator.GetNavigationCost(Grid.PosToCell(instance.transform.GetPosition()), instance.LurePoints);
			if (navigationCost != -1 && (context.cost == -1 || navigationCost < context.cost))
			{
				context.cost = navigationCost;
				context.result = instance.gameObject;
			}
			return Util.IterationInstruction.Continue;
		}

		// Token: 0x06009002 RID: 36866 RVA: 0x0036D56C File Offset: 0x0036B76C
		public void FindLure()
		{
			LureableMonitor.Instance.FindLureCounterContext findLureCounterContext = default(LureableMonitor.Instance.FindLureCounterContext);
			findLureCounterContext.inst = this;
			findLureCounterContext.cost = -1;
			findLureCounterContext.result = null;
			int num;
			int num2;
			Grid.CellToXY(Grid.PosToCell(base.smi.transform.GetPosition()), out num, out num2);
			GameScenePartitioner.Instance.ReadonlyVisitEntries<LureableMonitor.Instance.FindLureCounterContext>(num - 1, num2 - 1, 2, 2, GameScenePartitioner.Instance.lure, new GameScenePartitioner.VisitorRef<LureableMonitor.Instance.FindLureCounterContext>(LureableMonitor.Instance.FindLureCounter), ref findLureCounterContext);
			base.sm.targetLure.Set(findLureCounterContext.result, this, false);
		}

		// Token: 0x06009003 RID: 36867 RVA: 0x0036D5FA File Offset: 0x0036B7FA
		public bool HasLure()
		{
			return base.sm.targetLure.Get(this) != null;
		}

		// Token: 0x06009004 RID: 36868 RVA: 0x0036D613 File Offset: 0x0036B813
		public GameObject GetTargetLure()
		{
			return base.sm.targetLure.Get(this);
		}

		// Token: 0x04006ECE RID: 28366
		[MyCmpReq]
		private Navigator navigator;

		// Token: 0x0200289D RID: 10397
		private struct FindLureCounterContext
		{
			// Token: 0x0400B2F7 RID: 45815
			public LureableMonitor.Instance inst;

			// Token: 0x0400B2F8 RID: 45816
			public int cost;

			// Token: 0x0400B2F9 RID: 45817
			public GameObject result;
		}
	}
}
