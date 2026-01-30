using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200083D RID: 2109
public class ChainedBuilding : GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>
{
	// Token: 0x0600398C RID: 14732 RVA: 0x00141760 File Offset: 0x0013F960
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.unlinked;
		StatusItem statusItem = new StatusItem("NotLinkedToHeadStatusItem", BUILDING.STATUSITEMS.NOTLINKEDTOHEAD.NAME, BUILDING.STATUSITEMS.NOTLINKEDTOHEAD.TOOLTIP, "status_item_not_linked", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
		statusItem.resolveTooltipCallback = delegate(string tooltip, object obj)
		{
			ChainedBuilding.StatesInstance statesInstance = (ChainedBuilding.StatesInstance)obj;
			return tooltip.Replace("{headBuilding}", Strings.Get("STRINGS.BUILDINGS.PREFABS." + statesInstance.def.headBuildingTag.Name.ToUpper() + ".NAME")).Replace("{linkBuilding}", Strings.Get("STRINGS.BUILDINGS.PREFABS." + statesInstance.def.linkBuildingTag.Name.ToUpper() + ".NAME"));
		};
		this.root.OnSignal(this.doRelink, this.DEBUG_relink);
		this.unlinked.ParamTransition<bool>(this.isConnectedToHead, this.linked, GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.IsTrue).ToggleStatusItem(statusItem, (ChainedBuilding.StatesInstance smi) => smi);
		this.linked.ParamTransition<bool>(this.isConnectedToHead, this.unlinked, GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.IsFalse);
		this.DEBUG_relink.Enter(delegate(ChainedBuilding.StatesInstance smi)
		{
			smi.DEBUG_Relink();
		});
	}

	// Token: 0x0400233B RID: 9019
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State unlinked;

	// Token: 0x0400233C RID: 9020
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State linked;

	// Token: 0x0400233D RID: 9021
	private GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.State DEBUG_relink;

	// Token: 0x0400233E RID: 9022
	private StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.BoolParameter isConnectedToHead = new StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.BoolParameter();

	// Token: 0x0400233F RID: 9023
	private StateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.Signal doRelink;

	// Token: 0x020017E3 RID: 6115
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007906 RID: 30982
		public Tag headBuildingTag;

		// Token: 0x04007907 RID: 30983
		public Tag linkBuildingTag;

		// Token: 0x04007908 RID: 30984
		public ObjectLayer objectLayer;
	}

	// Token: 0x020017E4 RID: 6116
	public class StatesInstance : GameStateMachine<ChainedBuilding, ChainedBuilding.StatesInstance, IStateMachineTarget, ChainedBuilding.Def>.GameInstance
	{
		// Token: 0x06009CDE RID: 40158 RVA: 0x0039B5DC File Offset: 0x003997DC
		public StatesInstance(IStateMachineTarget master, ChainedBuilding.Def def) : base(master, def)
		{
			BuildingDef def2 = master.GetComponent<Building>().Def;
			this.widthInCells = def2.WidthInCells;
			int cell = Grid.PosToCell(this);
			this.neighbourCheckCells = new List<int>
			{
				Grid.OffsetCell(cell, -(this.widthInCells - 1) / 2 - 1, 0),
				Grid.OffsetCell(cell, this.widthInCells / 2 + 1, 0)
			};
		}

		// Token: 0x06009CDF RID: 40159 RVA: 0x0039B64C File Offset: 0x0039984C
		public override void StartSM()
		{
			base.StartSM();
			bool foundHead = false;
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			this.CollectToChain(ref pooledHashSet, ref foundHead, null);
			this.PropogateFoundHead(foundHead, pooledHashSet);
			this.PropagateChangedEvent(this, pooledHashSet);
			pooledHashSet.Recycle();
		}

		// Token: 0x06009CE0 RID: 40160 RVA: 0x0039B688 File Offset: 0x00399888
		public void DEBUG_Relink()
		{
			bool foundHead = false;
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			this.CollectToChain(ref pooledHashSet, ref foundHead, null);
			this.PropogateFoundHead(foundHead, pooledHashSet);
			pooledHashSet.Recycle();
		}

		// Token: 0x06009CE1 RID: 40161 RVA: 0x0039B6B8 File Offset: 0x003998B8
		protected override void OnCleanUp()
		{
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			foreach (int cell in this.neighbourCheckCells)
			{
				bool foundHead = false;
				this.CollectNeighbourToChain(cell, ref pooledHashSet, ref foundHead, this);
				this.PropogateFoundHead(foundHead, pooledHashSet);
				this.PropagateChangedEvent(this, pooledHashSet);
				pooledHashSet.Clear();
			}
			pooledHashSet.Recycle();
			base.OnCleanUp();
		}

		// Token: 0x06009CE2 RID: 40162 RVA: 0x0039B73C File Offset: 0x0039993C
		public HashSet<ChainedBuilding.StatesInstance> GetLinkedBuildings(ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain)
		{
			bool flag = false;
			this.CollectToChain(ref chain, ref flag, null);
			return chain;
		}

		// Token: 0x06009CE3 RID: 40163 RVA: 0x0039B758 File Offset: 0x00399958
		private void PropogateFoundHead(bool foundHead, HashSet<ChainedBuilding.StatesInstance> chain)
		{
			foreach (ChainedBuilding.StatesInstance statesInstance in chain)
			{
				statesInstance.sm.isConnectedToHead.Set(foundHead, statesInstance, false);
			}
		}

		// Token: 0x06009CE4 RID: 40164 RVA: 0x0039B7B4 File Offset: 0x003999B4
		private void PropagateChangedEvent(ChainedBuilding.StatesInstance changedLink, HashSet<ChainedBuilding.StatesInstance> chain)
		{
			foreach (ChainedBuilding.StatesInstance statesInstance in chain)
			{
				statesInstance.Trigger(-1009905786, changedLink);
			}
		}

		// Token: 0x06009CE5 RID: 40165 RVA: 0x0039B808 File Offset: 0x00399A08
		private void CollectToChain(ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain, ref bool foundHead, ChainedBuilding.StatesInstance ignoredLink = null)
		{
			if (ignoredLink != null && ignoredLink == this)
			{
				return;
			}
			if (chain.Contains(this))
			{
				return;
			}
			chain.Add(this);
			if (base.HasTag(base.def.headBuildingTag))
			{
				foundHead = true;
			}
			foreach (int cell in this.neighbourCheckCells)
			{
				this.CollectNeighbourToChain(cell, ref chain, ref foundHead, null);
			}
		}

		// Token: 0x06009CE6 RID: 40166 RVA: 0x0039B890 File Offset: 0x00399A90
		private void CollectNeighbourToChain(int cell, ref HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain, ref bool foundHead, ChainedBuilding.StatesInstance ignoredLink = null)
		{
			GameObject gameObject = Grid.Objects[cell, (int)base.def.objectLayer];
			if (gameObject == null)
			{
				return;
			}
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (!component.HasTag(base.def.linkBuildingTag) && !component.IsPrefabID(base.def.headBuildingTag))
			{
				return;
			}
			ChainedBuilding.StatesInstance smi = gameObject.GetSMI<ChainedBuilding.StatesInstance>();
			if (smi != null)
			{
				smi.CollectToChain(ref chain, ref foundHead, ignoredLink);
			}
		}

		// Token: 0x04007909 RID: 30985
		private int widthInCells;

		// Token: 0x0400790A RID: 30986
		private List<int> neighbourCheckCells;
	}
}
