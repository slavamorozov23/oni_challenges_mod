using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000AA2 RID: 2722
public class PlantBranch : GameStateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>
{
	// Token: 0x06004EED RID: 20205 RVA: 0x001CA852 File Offset: 0x001C8A52
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x040034C4 RID: 13508
	private StateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>.TargetParameter Trunk;

	// Token: 0x02001BC8 RID: 7112
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040085AE RID: 34222
		public Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback;

		// Token: 0x040085AF RID: 34223
		public Action<PlantBranch.Instance> onEarlySpawn;
	}

	// Token: 0x02001BC9 RID: 7113
	public new class Instance : GameStateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>.GameInstance, IWiltCause
	{
		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x0600AB0A RID: 43786 RVA: 0x003C7284 File Offset: 0x003C5484
		public bool HasTrunk
		{
			get
			{
				return this.trunk != null && !this.trunk.IsNullOrDestroyed() && !this.trunk.isMasterNull;
			}
		}

		// Token: 0x0600AB0B RID: 43787 RVA: 0x003C72AB File Offset: 0x003C54AB
		public Instance(IStateMachineTarget master, PlantBranch.Def def) : base(master, def)
		{
			this.SetOccupyGridSpace(true);
			base.Subscribe(1272413801, new Action<object>(this.OnHarvest));
		}

		// Token: 0x0600AB0C RID: 43788 RVA: 0x003C72E4 File Offset: 0x003C54E4
		public override void StartSM()
		{
			base.StartSM();
			Action<PlantBranch.Instance> onEarlySpawn = base.def.onEarlySpawn;
			if (onEarlySpawn != null)
			{
				onEarlySpawn(this);
			}
			this.trunk = this.GetTrunk();
			if (!this.HasTrunk)
			{
				global::Debug.LogWarning("Tree Branch loaded with missing trunk reference. Destroying...");
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.SubscribeToTrunk();
			Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback = base.def.animationSetupCallback;
			if (animationSetupCallback == null)
			{
				return;
			}
			animationSetupCallback(this.trunk, this);
		}

		// Token: 0x0600AB0D RID: 43789 RVA: 0x003C735A File Offset: 0x003C555A
		private void OnHarvest(object data)
		{
			if (this.HasTrunk)
			{
				this.trunk.OnBrancHarvested(this);
			}
		}

		// Token: 0x0600AB0E RID: 43790 RVA: 0x003C7370 File Offset: 0x003C5570
		protected override void OnCleanUp()
		{
			this.UnsubscribeToTrunk();
			this.SetOccupyGridSpace(false);
			base.OnCleanUp();
		}

		// Token: 0x0600AB0F RID: 43791 RVA: 0x003C7388 File Offset: 0x003C5588
		private void SetOccupyGridSpace(bool active)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!active)
			{
				if (Grid.Objects[cell, 5] == base.gameObject)
				{
					Grid.Objects[cell, 5] = null;
				}
				return;
			}
			GameObject gameObject = Grid.Objects[cell, 5];
			if (gameObject != null && gameObject != base.gameObject)
			{
				global::Debug.LogWarningFormat(base.gameObject, "PlantBranch.SetOccupyGridSpace already occupied by {0}", new object[]
				{
					gameObject
				});
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			Grid.Objects[cell, 5] = base.gameObject;
		}

		// Token: 0x0600AB10 RID: 43792 RVA: 0x003C7428 File Offset: 0x003C5628
		public void SetTrunk(PlantBranchGrower.Instance trunk)
		{
			this.trunk = trunk;
			base.smi.sm.Trunk.Set(trunk.gameObject, this, false);
			this.SubscribeToTrunk();
			Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback = base.def.animationSetupCallback;
			if (animationSetupCallback == null)
			{
				return;
			}
			animationSetupCallback(trunk, this);
		}

		// Token: 0x0600AB11 RID: 43793 RVA: 0x003C7477 File Offset: 0x003C5677
		public PlantBranchGrower.Instance GetTrunk()
		{
			if (base.smi.sm.Trunk.IsNull(this))
			{
				return null;
			}
			return base.sm.Trunk.Get(this).GetSMI<PlantBranchGrower.Instance>();
		}

		// Token: 0x0600AB12 RID: 43794 RVA: 0x003C74AC File Offset: 0x003C56AC
		private void SubscribeToTrunk()
		{
			if (!this.HasTrunk)
			{
				return;
			}
			if (this.trunkWiltHandle == -1)
			{
				this.trunkWiltHandle = this.trunk.gameObject.Subscribe(-724860998, new Action<object>(this.OnTrunkWilt));
			}
			if (this.trunkWiltRecoverHandle == -1)
			{
				this.trunkWiltRecoverHandle = this.trunk.gameObject.Subscribe(712767498, new Action<object>(this.OnTrunkRecover));
			}
			base.BoxingTrigger(912965142, !this.trunk.GetComponent<WiltCondition>().IsWilting());
			ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
			PlantablePlot receptacle = this.trunk.GetComponent<ReceptacleMonitor>().GetReceptacle();
			component.SetReceptacle(receptacle);
			this.trunk.RefreshBranchZPositionOffset(base.gameObject);
			base.GetComponent<BudUprootedMonitor>().SetParentObject(this.trunk.GetComponent<KPrefabID>());
		}

		// Token: 0x0600AB13 RID: 43795 RVA: 0x003C7584 File Offset: 0x003C5784
		private void UnsubscribeToTrunk()
		{
			if (!this.HasTrunk)
			{
				return;
			}
			this.trunk.gameObject.Unsubscribe(this.trunkWiltHandle);
			this.trunk.gameObject.Unsubscribe(this.trunkWiltRecoverHandle);
			this.trunkWiltHandle = -1;
			this.trunkWiltRecoverHandle = -1;
			this.trunk.OnBranchRemoved(base.gameObject);
		}

		// Token: 0x0600AB14 RID: 43796 RVA: 0x003C75E5 File Offset: 0x003C57E5
		private void OnTrunkWilt(object data = null)
		{
			base.BoxingTrigger(912965142, false);
		}

		// Token: 0x0600AB15 RID: 43797 RVA: 0x003C75F3 File Offset: 0x003C57F3
		private void OnTrunkRecover(object data = null)
		{
			base.BoxingTrigger(912965142, true);
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x0600AB16 RID: 43798 RVA: 0x003C7601 File Offset: 0x003C5801
		public string WiltStateString
		{
			get
			{
				return "    • " + DUPLICANTS.STATS.TRUNKHEALTH.NAME;
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x0600AB17 RID: 43799 RVA: 0x003C7617 File Offset: 0x003C5817
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.UnhealthyRoot
				};
			}
		}

		// Token: 0x040085B0 RID: 34224
		public PlantBranchGrower.Instance trunk;

		// Token: 0x040085B1 RID: 34225
		private int trunkWiltHandle = -1;

		// Token: 0x040085B2 RID: 34226
		private int trunkWiltRecoverHandle = -1;
	}
}
