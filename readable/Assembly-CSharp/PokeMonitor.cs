using System;
using UnityEngine;

// Token: 0x020005C5 RID: 1477
public class PokeMonitor : StateMachineComponent<PokeMonitor.Instance>
{
	// Token: 0x060021E3 RID: 8675 RVA: 0x000C4E55 File Offset: 0x000C3055
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x000C4E68 File Offset: 0x000C3068
	private static void ClearTarget(PokeMonitor.Instance smi)
	{
		smi.AbortPoke();
	}

	// Token: 0x02001484 RID: 5252
	public class States : GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor>
	{
		// Token: 0x06009015 RID: 36885 RVA: 0x0036D8B8 File Offset: 0x0036BAB8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Never;
			default_state = this.noTarget;
			this.noTarget.ParamTransition<GameObject>(this.target, this.hasTarget, GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.IsNotNull);
			this.hasTarget.ParamTransition<GameObject>(this.target, this.noTarget, GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.IsNull).ToggleBehaviour(GameTags.Creatures.UrgeToPoke, (PokeMonitor.Instance smi) => true, new Action<PokeMonitor.Instance>(PokeMonitor.ClearTarget));
		}

		// Token: 0x04006EDB RID: 28379
		public StateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.TargetParameter target;

		// Token: 0x04006EDC RID: 28380
		public GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.State noTarget;

		// Token: 0x04006EDD RID: 28381
		public GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.State hasTarget;
	}

	// Token: 0x02001485 RID: 5253
	public class Instance : GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.GameInstance
	{
		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06009017 RID: 36887 RVA: 0x0036D94B File Offset: 0x0036BB4B
		public GameObject Target
		{
			get
			{
				return base.sm.target.Get(this);
			}
		}

		// Token: 0x06009018 RID: 36888 RVA: 0x0036D95E File Offset: 0x0036BB5E
		public Instance(PokeMonitor master) : base(master)
		{
		}

		// Token: 0x06009019 RID: 36889 RVA: 0x0036D981 File Offset: 0x0036BB81
		public void InitiatePoke(GameObject target)
		{
			this.InitiatePoke(target, new CellOffset[]
			{
				new CellOffset(0, 0)
			});
		}

		// Token: 0x0600901A RID: 36890 RVA: 0x0036D99E File Offset: 0x0036BB9E
		public void InitiatePoke(GameObject target, CellOffset[] pokeOffesets)
		{
			base.sm.target.Set(target, this, false);
			this.TargetOffsets = pokeOffesets;
		}

		// Token: 0x0600901B RID: 36891 RVA: 0x0036D9BB File Offset: 0x0036BBBB
		public void AbortPoke()
		{
			base.sm.target.Set(null, this);
			this.TargetOffsets = new CellOffset[]
			{
				new CellOffset(0, 0)
			};
		}

		// Token: 0x04006EDE RID: 28382
		public CellOffset[] TargetOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
	}
}
