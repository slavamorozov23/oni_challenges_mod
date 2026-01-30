using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020008AE RID: 2222
public class MoltDropperMonitor : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>
{
	// Token: 0x06003D46 RID: 15686 RVA: 0x001560A8 File Offset: 0x001542A8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.NewDay, (MoltDropperMonitor.Instance smi) => GameClock.Instance, delegate(MoltDropperMonitor.Instance smi)
		{
			smi.spawnedThisCycle = false;
		});
		this.satisfied.UpdateTransition(this.drop, (MoltDropperMonitor.Instance smi, float dt) => smi.ShouldDropElement(), UpdateRate.SIM_4000ms, false);
		this.drop.DefaultState(this.drop.dropping);
		this.drop.dropping.EnterTransition(this.drop.complete, (MoltDropperMonitor.Instance smi) => !smi.def.synchWithBehaviour).ToggleBehaviour(GameTags.Creatures.ReadyToMolt, (MoltDropperMonitor.Instance smi) => true, delegate(MoltDropperMonitor.Instance smi)
		{
			smi.GoTo(this.drop.complete);
		});
		this.drop.complete.Enter(delegate(MoltDropperMonitor.Instance smi)
		{
			smi.Drop();
		}).TriggerOnEnter(GameHashes.Molt, null).EventTransition(GameHashes.NewDay, (MoltDropperMonitor.Instance smi) => GameClock.Instance, this.satisfied, null);
	}

	// Token: 0x040025D3 RID: 9683
	public StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter droppedThisCycle = new StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter(false);

	// Token: 0x040025D4 RID: 9684
	public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State satisfied;

	// Token: 0x040025D5 RID: 9685
	public MoltDropperMonitor.DropStates drop;

	// Token: 0x020018AC RID: 6316
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007BA2 RID: 31650
		public bool synchWithBehaviour;

		// Token: 0x04007BA3 RID: 31651
		public string onGrowDropID;

		// Token: 0x04007BA4 RID: 31652
		public float massToDrop;

		// Token: 0x04007BA5 RID: 31653
		public string amountName;

		// Token: 0x04007BA6 RID: 31654
		public Func<MoltDropperMonitor.Instance, bool> isReadyToMolt;
	}

	// Token: 0x020018AD RID: 6317
	public class DropStates : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State
	{
		// Token: 0x04007BA7 RID: 31655
		public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State dropping;

		// Token: 0x04007BA8 RID: 31656
		public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State complete;
	}

	// Token: 0x020018AE RID: 6318
	public new class Instance : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.GameInstance
	{
		// Token: 0x06009FD1 RID: 40913 RVA: 0x003A7D50 File Offset: 0x003A5F50
		public Instance(IStateMachineTarget master, MoltDropperMonitor.Def def) : base(master, def)
		{
			if (!string.IsNullOrEmpty(def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(def.amountName).Lookup(base.smi.gameObject);
				amountInstance.OnMaxValueReached = (System.Action)Delegate.Combine(amountInstance.OnMaxValueReached, new System.Action(this.OnAmountMaxValueReached));
			}
		}

		// Token: 0x06009FD2 RID: 40914 RVA: 0x003A7DB8 File Offset: 0x003A5FB8
		private void OnAmountMaxValueReached()
		{
			this.lastTineAmountReachedMax = GameClock.Instance.GetTime();
		}

		// Token: 0x06009FD3 RID: 40915 RVA: 0x003A7DCC File Offset: 0x003A5FCC
		protected override void OnCleanUp()
		{
			if (!string.IsNullOrEmpty(base.def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(base.def.amountName).Lookup(base.smi.gameObject);
				amountInstance.OnMaxValueReached = (System.Action)Delegate.Remove(amountInstance.OnMaxValueReached, new System.Action(this.OnAmountMaxValueReached));
			}
			base.OnCleanUp();
		}

		// Token: 0x06009FD4 RID: 40916 RVA: 0x003A7E3C File Offset: 0x003A603C
		public bool ShouldDropElement()
		{
			return base.def.isReadyToMolt(this);
		}

		// Token: 0x06009FD5 RID: 40917 RVA: 0x003A7E50 File Offset: 0x003A6050
		public void Drop()
		{
			GameObject gameObject = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, base.def.onGrowDropID, Grid.SceneLayer.Ore);
			gameObject.SetActive(true);
			gameObject.GetComponent<PrimaryElement>().Mass = base.def.massToDrop;
			this.spawnedThisCycle = true;
			this.timeOfLastDrop = GameClock.Instance.GetTime();
			if (!string.IsNullOrEmpty(base.def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(base.def.amountName).Lookup(base.smi.gameObject);
				amountInstance.value = amountInstance.GetMin();
			}
		}

		// Token: 0x06009FD6 RID: 40918 RVA: 0x003A7EF4 File Offset: 0x003A60F4
		private int GetDropSpawnLocation()
		{
			int num = Grid.PosToCell(base.gameObject);
			int num2 = Grid.CellAbove(num);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				return num2;
			}
			return num;
		}

		// Token: 0x04007BA9 RID: 31657
		[MyCmpGet]
		public KPrefabID prefabID;

		// Token: 0x04007BAA RID: 31658
		[Serialize]
		public bool spawnedThisCycle;

		// Token: 0x04007BAB RID: 31659
		[Serialize]
		public float timeOfLastDrop;

		// Token: 0x04007BAC RID: 31660
		[Serialize]
		public float lastTineAmountReachedMax;
	}
}
