using System;
using UnityEngine;

// Token: 0x020005B5 RID: 1461
public class ClimbableTreeMonitor : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>
{
	// Token: 0x06002184 RID: 8580 RVA: 0x000C2A60 File Offset: 0x000C0C60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToClimbTree, (ClimbableTreeMonitor.Instance smi) => smi.UpdateHasClimbable(), delegate(ClimbableTreeMonitor.Instance smi)
		{
			smi.OnClimbComplete();
		});
	}

	// Token: 0x04001390 RID: 5008
	private const int MAX_NAV_COST = 2147483647;

	// Token: 0x0200144C RID: 5196
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006E24 RID: 28196
		public float searchMinInterval = 60f;

		// Token: 0x04006E25 RID: 28197
		public float searchMaxInterval = 120f;
	}

	// Token: 0x0200144D RID: 5197
	public new class Instance : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget, ClimbableTreeMonitor.Def>.GameInstance
	{
		// Token: 0x06008F4B RID: 36683 RVA: 0x0036AF5E File Offset: 0x0036915E
		public Instance(IStateMachineTarget master, ClimbableTreeMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x06008F4C RID: 36684 RVA: 0x0036AF6E File Offset: 0x0036916E
		private void RefreshSearchTime()
		{
			this.nextSearchTime = Time.time + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x06008F4D RID: 36685 RVA: 0x0036AF9C File Offset: 0x0036919C
		public bool UpdateHasClimbable()
		{
			if (this.climbTarget == null)
			{
				if (Time.time < this.nextSearchTime)
				{
					return false;
				}
				this.FindClimbableTree();
				this.RefreshSearchTime();
			}
			return this.climbTarget != null;
		}

		// Token: 0x06008F4E RID: 36686 RVA: 0x0036AFD4 File Offset: 0x003691D4
		private static Util.IterationInstruction FindClimbableTree(object obj, ref ClimbableTreeMonitor.Instance.FindClimableTreeContext context)
		{
			KMonoBehaviour kmonoBehaviour = obj as KMonoBehaviour;
			if (kmonoBehaviour.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				return Util.IterationInstruction.Continue;
			}
			int cell = Grid.PosToCell(kmonoBehaviour);
			if (!context.navigator.CanReach(cell))
			{
				return Util.IterationInstruction.Continue;
			}
			ForestTreeSeedMonitor component = kmonoBehaviour.GetComponent<ForestTreeSeedMonitor>();
			StorageLocker component2 = kmonoBehaviour.GetComponent<StorageLocker>();
			if (component != null)
			{
				if (!component.ExtraSeedAvailable)
				{
					return Util.IterationInstruction.Continue;
				}
			}
			else
			{
				if (!(component2 != null))
				{
					return Util.IterationInstruction.Continue;
				}
				Storage component3 = component2.GetComponent<Storage>();
				if (!component3.allowItemRemoval)
				{
					return Util.IterationInstruction.Continue;
				}
				if (component3.IsEmpty())
				{
					return Util.IterationInstruction.Continue;
				}
			}
			context.targets.Add(kmonoBehaviour);
			return Util.IterationInstruction.Continue;
		}

		// Token: 0x06008F4F RID: 36687 RVA: 0x0036B068 File Offset: 0x00369268
		private void FindClimbableTree()
		{
			this.climbTarget = null;
			Vector3 position = base.master.transform.GetPosition();
			Extents extents = new Extents(Grid.PosToCell(position), 10);
			ClimbableTreeMonitor.Instance.FindClimableTreeContext findClimableTreeContext;
			findClimableTreeContext.navigator = base.GetComponent<Navigator>();
			findClimableTreeContext.targets = ListPool<KMonoBehaviour, ClimbableTreeMonitor>.Allocate();
			GameScenePartitioner.Instance.ReadonlyVisitEntries<ClimbableTreeMonitor.Instance.FindClimableTreeContext>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.plants, ClimbableTreeMonitor.Instance.FindClimbableTreeVisitor, ref findClimableTreeContext);
			GameScenePartitioner.Instance.ReadonlyVisitEntries<ClimbableTreeMonitor.Instance.FindClimableTreeContext>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.completeBuildings, ClimbableTreeMonitor.Instance.FindClimbableTreeVisitor, ref findClimableTreeContext);
			if (findClimableTreeContext.targets.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, findClimableTreeContext.targets.Count);
				KMonoBehaviour kmonoBehaviour = findClimableTreeContext.targets[index];
				this.climbTarget = kmonoBehaviour.gameObject;
			}
			findClimableTreeContext.targets.Recycle();
		}

		// Token: 0x06008F50 RID: 36688 RVA: 0x0036B161 File Offset: 0x00369361
		public void OnClimbComplete()
		{
			this.climbTarget = null;
		}

		// Token: 0x04006E26 RID: 28198
		public GameObject climbTarget;

		// Token: 0x04006E27 RID: 28199
		public float nextSearchTime;

		// Token: 0x04006E28 RID: 28200
		private static GameScenePartitioner.VisitorRef<ClimbableTreeMonitor.Instance.FindClimableTreeContext> FindClimbableTreeVisitor = new GameScenePartitioner.VisitorRef<ClimbableTreeMonitor.Instance.FindClimableTreeContext>(ClimbableTreeMonitor.Instance.FindClimbableTree);

		// Token: 0x02002895 RID: 10389
		private struct FindClimableTreeContext
		{
			// Token: 0x0400B2E0 RID: 45792
			public Navigator navigator;

			// Token: 0x0400B2E1 RID: 45793
			public ListPool<KMonoBehaviour, ClimbableTreeMonitor>.PooledList targets;
		}
	}
}
