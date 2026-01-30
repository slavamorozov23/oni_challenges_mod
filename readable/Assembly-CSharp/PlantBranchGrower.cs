using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000AA3 RID: 2723
public class PlantBranchGrower : PlantBranchGrowerBase<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>
{
	// Token: 0x06004EEF RID: 20207 RVA: 0x001CA86C File Offset: 0x001C8A6C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.wilt;
		this.worldgen.Update(new Action<PlantBranchGrower.Instance, float>(PlantBranchGrower.WorldGenUpdate), UpdateRate.RENDER_EVERY_TICK, false);
		this.wilt.TagTransition(GameTags.Wilting, this.maturing, true);
		this.maturing.TagTransition(GameTags.Wilting, this.wilt, false).EnterTransition(this.growingBranches, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature)).EventTransition(GameHashes.Grow, this.growingBranches, null);
		this.growingBranches.TagTransition(GameTags.Wilting, this.wilt, false).EventTransition(GameHashes.ConsumePlant, this.maturing, GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Not(new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature))).EventTransition(GameHashes.TreeBranchCountChanged, this.fullyGrown, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.AllBranchesCreated)).ToggleStatusItem((PlantBranchGrower.Instance smi) => smi.def.growingBranchesStatusItem, null).Update(new Action<PlantBranchGrower.Instance, float>(PlantBranchGrower.GrowBranchUpdate), UpdateRate.SIM_4000ms, false);
		this.fullyGrown.TagTransition(GameTags.Wilting, this.wilt, false).EventTransition(GameHashes.ConsumePlant, this.maturing, GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Not(new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature))).EventTransition(GameHashes.TreeBranchCountChanged, this.growingBranches, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.NotAllBranchesCreated));
	}

	// Token: 0x06004EF0 RID: 20208 RVA: 0x001CA9DC File Offset: 0x001C8BDC
	public static bool NotAllBranchesCreated(PlantBranchGrower.Instance smi)
	{
		return smi.CurrentBranchCount < smi.MaxBranchesAllowedAtOnce;
	}

	// Token: 0x06004EF1 RID: 20209 RVA: 0x001CA9EC File Offset: 0x001C8BEC
	public static bool AllBranchesCreated(PlantBranchGrower.Instance smi)
	{
		return smi.CurrentBranchCount >= smi.MaxBranchesAllowedAtOnce;
	}

	// Token: 0x06004EF2 RID: 20210 RVA: 0x001CA9FF File Offset: 0x001C8BFF
	public static bool IsMature(PlantBranchGrower.Instance smi)
	{
		return smi.IsGrown;
	}

	// Token: 0x06004EF3 RID: 20211 RVA: 0x001CAA07 File Offset: 0x001C8C07
	public static void GrowBranchUpdate(PlantBranchGrower.Instance smi, float dt)
	{
		smi.SpawnRandomBranch(0f);
	}

	// Token: 0x06004EF4 RID: 20212 RVA: 0x001CAA18 File Offset: 0x001C8C18
	public static void WorldGenUpdate(PlantBranchGrower.Instance smi, float dt)
	{
		float growth_percentage = UnityEngine.Random.Range(0f, 1f);
		if (!smi.SpawnRandomBranch(growth_percentage))
		{
			smi.GoTo(smi.sm.defaultState);
		}
	}

	// Token: 0x040034C5 RID: 13509
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State worldgen;

	// Token: 0x040034C6 RID: 13510
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State wilt;

	// Token: 0x040034C7 RID: 13511
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State maturing;

	// Token: 0x040034C8 RID: 13512
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State growingBranches;

	// Token: 0x040034C9 RID: 13513
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State fullyGrown;

	// Token: 0x02001BCA RID: 7114
	public class Def : PlantBranchGrowerBase<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.PlantBranchGrowerBaseDef
	{
		// Token: 0x040085B3 RID: 34227
		public CellOffset[] BRANCH_OFFSETS;

		// Token: 0x040085B4 RID: 34228
		public bool harvestOnDrown;

		// Token: 0x040085B5 RID: 34229
		public bool propagateHarvestDesignation = true;

		// Token: 0x040085B6 RID: 34230
		public Func<int, bool> additionalBranchGrowRequirements;

		// Token: 0x040085B7 RID: 34231
		public Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchHarvested;

		// Token: 0x040085B8 RID: 34232
		public Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchSpawned;

		// Token: 0x040085B9 RID: 34233
		public StatusItem growingBranchesStatusItem = Db.Get().MiscStatusItems.GrowingBranches;

		// Token: 0x040085BA RID: 34234
		public Action<PlantBranchGrower.Instance> onEarlySpawn;
	}

	// Token: 0x02001BCB RID: 7115
	public new class Instance : GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.GameInstance
	{
		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x0600AB19 RID: 43801 RVA: 0x003C7648 File Offset: 0x003C5848
		public bool IsUprooted
		{
			get
			{
				return this.uprootMonitor != null && this.uprootMonitor.IsUprooted;
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x0600AB1A RID: 43802 RVA: 0x003C7665 File Offset: 0x003C5865
		public bool IsGrown
		{
			get
			{
				return this.growing == null || this.growing.PercentGrown() >= 1f;
			}
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x0600AB1B RID: 43803 RVA: 0x003C7686 File Offset: 0x003C5886
		public int MaxBranchesAllowedAtOnce
		{
			get
			{
				if (base.def.MAX_BRANCH_COUNT >= 0)
				{
					return Mathf.Min(base.def.MAX_BRANCH_COUNT, base.def.BRANCH_OFFSETS.Length);
				}
				return base.def.BRANCH_OFFSETS.Length;
			}
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600AB1C RID: 43804 RVA: 0x003C76C4 File Offset: 0x003C58C4
		public int CurrentBranchCount
		{
			get
			{
				int num = 0;
				if (this.branches != null)
				{
					int i = 0;
					while (i < this.branches.Length)
					{
						num += ((this.GetBranch(i++) != null) ? 1 : 0);
					}
				}
				return num;
			}
		}

		// Token: 0x0600AB1D RID: 43805 RVA: 0x003C7708 File Offset: 0x003C5908
		public GameObject GetBranch(int idx)
		{
			if (this.branches != null && this.branches[idx] != null)
			{
				KPrefabID kprefabID = this.branches[idx].Get();
				if (kprefabID != null)
				{
					return kprefabID.gameObject;
				}
			}
			return null;
		}

		// Token: 0x0600AB1E RID: 43806 RVA: 0x003C7746 File Offset: 0x003C5946
		protected override void OnCleanUp()
		{
			this.SetTrunkOccupyingCellsAsPlant(false);
			base.OnCleanUp();
		}

		// Token: 0x0600AB1F RID: 43807 RVA: 0x003C7758 File Offset: 0x003C5958
		public Instance(IStateMachineTarget master, PlantBranchGrower.Def def) : base(master, def)
		{
			this.growing = base.GetComponent<IManageGrowingStates>();
			this.growing = ((this.growing != null) ? this.growing : base.gameObject.GetSMI<IManageGrowingStates>());
			this.SetTrunkOccupyingCellsAsPlant(true);
			base.Subscribe(1119167081, new Action<object>(this.OnNewGameSpawn));
			base.Subscribe(144050788, new Action<object>(this.OnUpdateRoom));
		}

		// Token: 0x0600AB20 RID: 43808 RVA: 0x003C77D4 File Offset: 0x003C59D4
		public override void StartSM()
		{
			base.StartSM();
			Action<PlantBranchGrower.Instance> onEarlySpawn = base.def.onEarlySpawn;
			if (onEarlySpawn != null)
			{
				onEarlySpawn(this);
			}
			this.DefineBranchArray();
			base.Subscribe(-216549700, new Action<object>(this.OnUprooted));
			base.Subscribe(-266953818, delegate(object obj)
			{
				this.UpdateAutoHarvestValue(null);
			});
			if (base.def.harvestOnDrown)
			{
				base.Subscribe(-750750377, new Action<object>(this.OnUprooted));
			}
		}

		// Token: 0x0600AB21 RID: 43809 RVA: 0x003C785C File Offset: 0x003C5A5C
		private void OnUpdateRoom(object data)
		{
			if (this.branches == null)
			{
				return;
			}
			this.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(144050788, data);
			});
		}

		// Token: 0x0600AB22 RID: 43810 RVA: 0x003C7894 File Offset: 0x003C5A94
		private void SetTrunkOccupyingCellsAsPlant(bool doSet)
		{
			CellOffset[] occupiedCellsOffsets = base.GetComponent<OccupyArea>().OccupiedCellsOffsets;
			int cell = Grid.PosToCell(base.gameObject);
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int cell2 = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (doSet)
				{
					Grid.Objects[cell2, 5] = base.gameObject;
				}
				else if (Grid.Objects[cell2, 5] == base.gameObject)
				{
					Grid.Objects[cell2, 5] = null;
				}
			}
		}

		// Token: 0x0600AB23 RID: 43811 RVA: 0x003C7914 File Offset: 0x003C5B14
		private void OnNewGameSpawn(object data)
		{
			this.DefineBranchArray();
			float percentage = 1f;
			if ((double)UnityEngine.Random.value < 0.1)
			{
				percentage = UnityEngine.Random.Range(0.75f, 0.99f);
			}
			else
			{
				this.GoTo(base.sm.worldgen);
			}
			this.growing.OverrideMaturityLevel(percentage);
		}

		// Token: 0x0600AB24 RID: 43812 RVA: 0x003C7970 File Offset: 0x003C5B70
		public void ManuallyDefineBranchArray(KPrefabID[] _branches)
		{
			this.DefineBranchArray();
			for (int i = 0; i < Mathf.Min(this.branches.Length, _branches.Length); i++)
			{
				KPrefabID kprefabID = _branches[i];
				if (kprefabID != null)
				{
					if (this.branches[i] == null)
					{
						this.branches[i] = new Ref<KPrefabID>();
					}
					this.branches[i].Set(kprefabID);
				}
				else
				{
					this.branches[i] = null;
				}
			}
		}

		// Token: 0x0600AB25 RID: 43813 RVA: 0x003C79DB File Offset: 0x003C5BDB
		private void DefineBranchArray()
		{
			if (this.branches == null)
			{
				this.branches = new Ref<KPrefabID>[base.def.BRANCH_OFFSETS.Length];
			}
		}

		// Token: 0x0600AB26 RID: 43814 RVA: 0x003C7A00 File Offset: 0x003C5C00
		public void ActionPerBranch(Action<GameObject> action)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null && action != null)
				{
					action(branch.gameObject);
				}
			}
		}

		// Token: 0x0600AB27 RID: 43815 RVA: 0x003C7A40 File Offset: 0x003C5C40
		public GameObject[] GetExistingBranches()
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null)
				{
					list.Add(branch.gameObject);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600AB28 RID: 43816 RVA: 0x003C7A8C File Offset: 0x003C5C8C
		public void OnBranchRemoved(GameObject _branch)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null && branch == _branch)
				{
					this.branches[i] = null;
				}
			}
			base.gameObject.Trigger(-1586842875, null);
		}

		// Token: 0x0600AB29 RID: 43817 RVA: 0x003C7AE0 File Offset: 0x003C5CE0
		public void OnBrancHarvested(PlantBranch.Instance branch)
		{
			Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchHarvested = base.def.onBranchHarvested;
			if (onBranchHarvested == null)
			{
				return;
			}
			onBranchHarvested(branch, this);
		}

		// Token: 0x0600AB2A RID: 43818 RVA: 0x003C7AFC File Offset: 0x003C5CFC
		private void OnUprooted(object data = null)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null)
				{
					branch.Trigger(-216549700, null);
				}
			}
		}

		// Token: 0x0600AB2B RID: 43819 RVA: 0x003C7B3C File Offset: 0x003C5D3C
		public List<int> GetAvailableSpawnPositions()
		{
			PlantBranchGrower.Instance.spawn_choices.Clear();
			int cell = Grid.PosToCell(this);
			for (int i = 0; i < base.def.BRANCH_OFFSETS.Length; i++)
			{
				int cell2 = Grid.OffsetCell(cell, base.def.BRANCH_OFFSETS[i]);
				if (this.GetBranch(i) == null && this.CanBranchGrowInCell(cell2))
				{
					PlantBranchGrower.Instance.spawn_choices.Add(i);
				}
			}
			return PlantBranchGrower.Instance.spawn_choices;
		}

		// Token: 0x0600AB2C RID: 43820 RVA: 0x003C7BB4 File Offset: 0x003C5DB4
		public void RefreshBranchZPositionOffset(GameObject _branch)
		{
			if (this.branches != null)
			{
				for (int i = 0; i < this.branches.Length; i++)
				{
					GameObject branch = this.GetBranch(i);
					if (branch != null && branch == _branch)
					{
						Vector3 position = branch.transform.position;
						position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) - 0.8f / (float)this.branches.Length * (float)i;
						branch.transform.SetPosition(position);
					}
				}
			}
		}

		// Token: 0x0600AB2D RID: 43821 RVA: 0x003C7C30 File Offset: 0x003C5E30
		public bool SpawnRandomBranch(float growth_percentage = 0f)
		{
			if (this.IsUprooted)
			{
				return false;
			}
			if (this.CurrentBranchCount >= this.MaxBranchesAllowedAtOnce)
			{
				return false;
			}
			List<int> availableSpawnPositions = this.GetAvailableSpawnPositions();
			availableSpawnPositions.Shuffle<int>();
			if (availableSpawnPositions.Count > 0)
			{
				int idx = availableSpawnPositions[0];
				PlantBranch.Instance instance = this.SpawnBranchAtIndex(idx);
				IManageGrowingStates manageGrowingStates = instance.GetComponent<IManageGrowingStates>();
				manageGrowingStates = ((manageGrowingStates != null) ? manageGrowingStates : instance.gameObject.GetSMI<IManageGrowingStates>());
				if (manageGrowingStates != null)
				{
					manageGrowingStates.OverrideMaturityLevel(growth_percentage);
				}
				instance.StartSM();
				base.gameObject.Trigger(-1586842875, instance);
				Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchSpawned = base.def.onBranchSpawned;
				if (onBranchSpawned != null)
				{
					onBranchSpawned(instance, this);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600AB2E RID: 43822 RVA: 0x003C7CD4 File Offset: 0x003C5ED4
		private PlantBranch.Instance SpawnBranchAtIndex(int idx)
		{
			if (idx < 0 || idx >= this.branches.Length)
			{
				return null;
			}
			GameObject branch = this.GetBranch(idx);
			if (branch != null)
			{
				return branch.GetSMI<PlantBranch.Instance>();
			}
			Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), base.def.BRANCH_OFFSETS[idx]), Grid.SceneLayer.BuildingFront);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.def.BRANCH_PREFAB_NAME), position);
			gameObject.SetActive(true);
			PlantBranch.Instance smi = gameObject.GetSMI<PlantBranch.Instance>();
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null)
			{
				MutantPlant component2 = smi.GetComponent<MutantPlant>();
				if (component2 != null)
				{
					component.CopyMutationsTo(component2);
					PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = component2.GetSubSpeciesInfo();
					PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(subSpeciesInfo, component2);
					PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(subSpeciesInfo.ID);
				}
			}
			this.UpdateAutoHarvestValue(smi);
			smi.SetTrunk(this);
			this.branches[idx] = new Ref<KPrefabID>();
			this.branches[idx].Set(smi.GetComponent<KPrefabID>());
			return smi;
		}

		// Token: 0x0600AB2F RID: 43823 RVA: 0x003C7DD8 File Offset: 0x003C5FD8
		private bool CanBranchGrowInCell(int cell)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			if (Grid.Solid[cell])
			{
				return false;
			}
			if (Grid.Objects[cell, 1] != null)
			{
				return false;
			}
			if (Grid.Objects[cell, 5] != null)
			{
				return false;
			}
			if (Grid.Foundation[cell])
			{
				return false;
			}
			int cell2 = Grid.CellAbove(cell);
			return Grid.IsValidCell(cell2) && !Grid.IsSubstantialLiquid(cell2, 0.35f) && (base.def.additionalBranchGrowRequirements == null || base.def.additionalBranchGrowRequirements(cell));
		}

		// Token: 0x0600AB30 RID: 43824 RVA: 0x003C7E7C File Offset: 0x003C607C
		public void UpdateAutoHarvestValue(PlantBranch.Instance specificBranch = null)
		{
			HarvestDesignatable component = base.GetComponent<HarvestDesignatable>();
			if (component != null && this.branches != null)
			{
				if (specificBranch != null)
				{
					HarvestDesignatable component2 = specificBranch.GetComponent<HarvestDesignatable>();
					if (component2 != null)
					{
						component2.SetHarvestWhenReady(component.HarvestWhenReady);
					}
					return;
				}
				if (base.def.propagateHarvestDesignation)
				{
					for (int i = 0; i < this.branches.Length; i++)
					{
						GameObject branch = this.GetBranch(i);
						if (branch != null)
						{
							HarvestDesignatable component3 = branch.GetComponent<HarvestDesignatable>();
							if (component3 != null)
							{
								component3.SetHarvestWhenReady(component.HarvestWhenReady);
							}
						}
					}
				}
			}
		}

		// Token: 0x040085BB RID: 34235
		private IManageGrowingStates growing;

		// Token: 0x040085BC RID: 34236
		[MyCmpGet]
		private UprootedMonitor uprootMonitor;

		// Token: 0x040085BD RID: 34237
		[Serialize]
		private Ref<KPrefabID>[] branches;

		// Token: 0x040085BE RID: 34238
		private static List<int> spawn_choices = new List<int>();
	}
}
