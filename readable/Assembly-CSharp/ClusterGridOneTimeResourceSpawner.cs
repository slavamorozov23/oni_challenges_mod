using System;
using System.Collections.Generic;

// Token: 0x02000B6C RID: 2924
public class ClusterGridOneTimeResourceSpawner : GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>
{
	// Token: 0x06005697 RID: 22167 RVA: 0x001F853C File Offset: 0x001F673C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.enter;
		this.enter.ParamTransition<bool>(this.HasSpawnedResources, this.spawned, GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.IsTrue).ParamTransition<bool>(this.HasSpawnedResources, this.spawning, GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.IsFalse);
		this.spawning.ParamTransition<bool>(this.HasSpawnedResources, this.spawned, GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.IsTrue).Enter(new StateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.State.Callback(ClusterGridOneTimeResourceSpawner.SpawnResources));
		this.spawned.DoNothing();
	}

	// Token: 0x06005698 RID: 22168 RVA: 0x001F85C5 File Offset: 0x001F67C5
	public static void SpawnResources(ClusterGridOneTimeResourceSpawner.Instance smi)
	{
		smi.SpawnResources();
	}

	// Token: 0x04003A73 RID: 14963
	public GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.State enter;

	// Token: 0x04003A74 RID: 14964
	public GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.State spawning;

	// Token: 0x04003A75 RID: 14965
	public GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.State spawned;

	// Token: 0x04003A76 RID: 14966
	public StateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.BoolParameter HasSpawnedResources;

	// Token: 0x02001CE1 RID: 7393
	public struct Data
	{
		// Token: 0x04008989 RID: 35209
		public Tag itemID;

		// Token: 0x0400898A RID: 35210
		public float mass;
	}

	// Token: 0x02001CE2 RID: 7394
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400898B RID: 35211
		public List<ClusterGridOneTimeResourceSpawner.Data> thingsToSpawn;
	}

	// Token: 0x02001CE3 RID: 7395
	public new class Instance : GameStateMachine<ClusterGridOneTimeResourceSpawner, ClusterGridOneTimeResourceSpawner.Instance, IStateMachineTarget, ClusterGridOneTimeResourceSpawner.Def>.GameInstance
	{
		// Token: 0x0600AF0D RID: 44813 RVA: 0x003D521E File Offset: 0x003D341E
		public Instance(IStateMachineTarget master, ClusterGridOneTimeResourceSpawner.Def def) : base(master, def)
		{
		}

		// Token: 0x0600AF0E RID: 44814 RVA: 0x003D5228 File Offset: 0x003D3428
		public void SpawnResources()
		{
			StarmapHexCellInventory hexCellInventory = this.GetHexCellInventory();
			foreach (ClusterGridOneTimeResourceSpawner.Data data in base.def.thingsToSpawn)
			{
				hexCellInventory.AddItem(data.itemID, data.mass, Element.State.Vacuum).RecalculateState();
			}
			base.sm.HasSpawnedResources.Set(true, this, false);
		}

		// Token: 0x0600AF0F RID: 44815 RVA: 0x003D52AC File Offset: 0x003D34AC
		public StarmapHexCellInventory GetHexCellInventory()
		{
			ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
			return ClusterGrid.Instance.AddOrGetHexCellInventory(component.Location);
		}
	}
}
