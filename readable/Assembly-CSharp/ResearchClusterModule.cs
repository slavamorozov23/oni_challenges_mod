using System;
using UnityEngine;

// Token: 0x020007ED RID: 2029
public class ResearchClusterModule : GameStateMachine<ResearchClusterModule, ResearchClusterModule.Instance, IStateMachineTarget, ResearchClusterModule.Def>
{
	// Token: 0x0600364E RID: 13902 RVA: 0x00132A60 File Offset: 0x00130C60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.grounded;
		this.root.EventHandler(GameHashes.RocketLanded, new StateMachine<ResearchClusterModule, ResearchClusterModule.Instance, IStateMachineTarget, ResearchClusterModule.Def>.State.Callback(ResearchClusterModule.DropInventory));
		this.grounded.TagTransition(GameTags.RocketNotOnGround, this.space, false);
		this.space.TagTransition(GameTags.RocketNotOnGround, this.grounded, true).DefaultState(this.space.idle);
		this.space.idle.EventHandlerTransition(GameHashes.OnStorageChange, this.space.full, new Func<ResearchClusterModule.Instance, object, bool>(ResearchClusterModule.IsStorageFull)).Target(this.ClusterCraft).EventHandlerTransition(GameHashes.TagsChanged, this.space.collecting, new Func<ResearchClusterModule.Instance, object, bool>(ResearchClusterModule.IsCollectingDatabanks));
		this.space.collecting.EventHandlerTransition(GameHashes.OnStorageChange, this.space.full, new Func<ResearchClusterModule.Instance, object, bool>(ResearchClusterModule.IsStorageFull)).Target(this.ClusterCraft).EventHandlerTransition(GameHashes.TagsChanged, this.space.collecting, new Func<ResearchClusterModule.Instance, object, bool>(ResearchClusterModule.IsNotCollectingDatabanks));
		this.space.full.EventHandlerTransition(GameHashes.OnStorageChange, this.space.idle, new Func<ResearchClusterModule.Instance, object, bool>(ResearchClusterModule.StorageIsNotFull));
	}

	// Token: 0x0600364F RID: 13903 RVA: 0x00132BB8 File Offset: 0x00130DB8
	public static void DropInventory(ResearchClusterModule.Instance smi)
	{
		smi.DropInventory();
	}

	// Token: 0x06003650 RID: 13904 RVA: 0x00132BC0 File Offset: 0x00130DC0
	public static bool IsNotCollectingDatabanks(ResearchClusterModule.Instance smi, object o)
	{
		return !smi.IsCollectingDatabanks;
	}

	// Token: 0x06003651 RID: 13905 RVA: 0x00132BCB File Offset: 0x00130DCB
	public static bool IsCollectingDatabanks(ResearchClusterModule.Instance smi, object o)
	{
		return smi.IsCollectingDatabanks;
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x00132BD3 File Offset: 0x00130DD3
	public static bool IsStorageFull(ResearchClusterModule.Instance smi, object o)
	{
		return smi.IsStorageFull;
	}

	// Token: 0x06003653 RID: 13907 RVA: 0x00132BDB File Offset: 0x00130DDB
	public static bool StorageIsNotFull(ResearchClusterModule.Instance smi, object o)
	{
		return !smi.IsStorageFull;
	}

	// Token: 0x040020FC RID: 8444
	public GameStateMachine<ResearchClusterModule, ResearchClusterModule.Instance, IStateMachineTarget, ResearchClusterModule.Def>.State grounded;

	// Token: 0x040020FD RID: 8445
	public ResearchClusterModule.InSpaceStates space;

	// Token: 0x040020FE RID: 8446
	public StateMachine<ResearchClusterModule, ResearchClusterModule.Instance, IStateMachineTarget, ResearchClusterModule.Def>.TargetParameter ClusterCraft;

	// Token: 0x02001759 RID: 5977
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200175A RID: 5978
	public class InSpaceStates : GameStateMachine<ResearchClusterModule, ResearchClusterModule.Instance, IStateMachineTarget, ResearchClusterModule.Def>.State
	{
		// Token: 0x0400778E RID: 30606
		public GameStateMachine<ResearchClusterModule, ResearchClusterModule.Instance, IStateMachineTarget, ResearchClusterModule.Def>.State idle;

		// Token: 0x0400778F RID: 30607
		public GameStateMachine<ResearchClusterModule, ResearchClusterModule.Instance, IStateMachineTarget, ResearchClusterModule.Def>.State collecting;

		// Token: 0x04007790 RID: 30608
		public GameStateMachine<ResearchClusterModule, ResearchClusterModule.Instance, IStateMachineTarget, ResearchClusterModule.Def>.State full;
	}

	// Token: 0x0200175B RID: 5979
	public new class Instance : GameStateMachine<ResearchClusterModule, ResearchClusterModule.Instance, IStateMachineTarget, ResearchClusterModule.Def>.GameInstance
	{
		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06009AE6 RID: 39654 RVA: 0x00392E8F File Offset: 0x0039108F
		public bool IsStorageFull
		{
			get
			{
				return this.storage != null && this.storage.IsFull();
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06009AE7 RID: 39655 RVA: 0x00392EAC File Offset: 0x003910AC
		public bool IsCollectingDatabanks
		{
			get
			{
				return this.collector != null && this.collector.IsCollecting;
			}
		}

		// Token: 0x06009AE8 RID: 39656 RVA: 0x00392EC3 File Offset: 0x003910C3
		public Instance(IStateMachineTarget master, ResearchClusterModule.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<Storage>();
			this.collector = base.gameObject.GetSMI<RocketModuleHexCellCollector.Instance>();
		}

		// Token: 0x06009AE9 RID: 39657 RVA: 0x00392EEA File Offset: 0x003910EA
		public override void StartSM()
		{
			this.clustercraft = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			base.sm.ClusterCraft.Set(this.clustercraft.gameObject, this, false);
			base.StartSM();
		}

		// Token: 0x06009AEA RID: 39658 RVA: 0x00392F28 File Offset: 0x00391128
		public void DropInventory()
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}

		// Token: 0x04007791 RID: 30609
		private Storage storage;

		// Token: 0x04007792 RID: 30610
		private RocketModuleHexCellCollector.Instance collector;

		// Token: 0x04007793 RID: 30611
		private Clustercraft clustercraft;
	}
}
