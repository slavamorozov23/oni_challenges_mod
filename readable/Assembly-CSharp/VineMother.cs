using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000AAE RID: 2734
public class VineMother : PlantBranchGrowerBase<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>
{
	// Token: 0x06004F49 RID: 20297 RVA: 0x001CC8F8 File Offset: 0x001CAAF8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.growing;
		this.growing.InitializeStates(this.masterTarget, this.dead).DefaultState(this.growing.growing);
		this.growing.growing.ParamTransition<bool>(this.IsGrown, this.grown, GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.IsTrue).PlayAnim("grow", KAnim.PlayMode.Once).OnAnimQueueComplete(this.growing.growing_pst);
		this.growing.growing_pst.Enter(new StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State.Callback(VineMother.MarkAsGrown)).PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown);
		this.grown.InitializeStates(this.masterTarget, this.dead).DefaultState(this.grown.growingBranches);
		this.grown.growingBranches.EventTransition(GameHashes.Wilt, this.grown.wilt, (VineMother.Instance smi) => smi.IsWilting).ParamTransition<GameObject>(this.LeftBranch, this.grown.idle, (VineMother.Instance smi, GameObject b) => VineMother.HasGrownAllBranches(smi)).ParamTransition<GameObject>(this.RightBranch, this.grown.idle, (VineMother.Instance smi, GameObject b) => VineMother.HasGrownAllBranches(smi)).PlayAnim("idle_full", KAnim.PlayMode.Loop).Enter(new StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State.Callback(VineMother.SpawnBranchesIfNewGameSpawn)).Update(new Action<VineMother.Instance, float>(VineMother.AttemptToSpawnBranches), UpdateRate.SIM_4000ms, false).DefaultState(this.grown.growingBranches.growing);
		this.grown.growingBranches.growing.ParamTransition<GameObject>(this.LeftBranch, this.grown.growingBranches.blocked, (VineMother.Instance smi, GameObject b) => VineMother.HasNoBranches(smi)).ParamTransition<GameObject>(this.RightBranch, this.grown.growingBranches.blocked, (VineMother.Instance smi, GameObject b) => VineMother.HasNoBranches(smi));
		this.grown.growingBranches.blocked.ParamTransition<GameObject>(this.LeftBranch, this.grown.growingBranches.growing, GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.IsNotNull).ParamTransition<GameObject>(this.RightBranch, this.grown.growingBranches.growing, GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.IsNotNull);
		this.grown.idle.EventTransition(GameHashes.Wilt, this.grown.wilt, (VineMother.Instance smi) => smi.IsWilting).ParamTransition<GameObject>(this.LeftBranch, this.grown.growingBranches, GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.IsNull).ParamTransition<GameObject>(this.RightBranch, this.grown.growingBranches, GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.IsNull).PlayAnim("idle_full", KAnim.PlayMode.Loop);
		this.grown.wilt.EventTransition(GameHashes.WiltRecover, this.grown.idle, (VineMother.Instance smi) => !smi.IsWilting).PlayAnim("wilt3", KAnim.PlayMode.Loop);
		this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(VineMother.Instance smi)
		{
			if (!smi.IsWild && !smi.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
			{
				Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
				Notification notification = VineMother.CreateDeathNotification(smi);
				notifier.Add(notification, "");
			}
			GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
			smi.Trigger(1623392196, null);
			smi.DestroySelf(null);
		});
	}

	// Token: 0x06004F4A RID: 20298 RVA: 0x001CCCA4 File Offset: 0x001CAEA4
	private static void MarkAsGrown(VineMother.Instance smi)
	{
		smi.sm.IsGrown.Set(true, smi, false);
	}

	// Token: 0x06004F4B RID: 20299 RVA: 0x001CCCBA File Offset: 0x001CAEBA
	private static bool HasNoBranches(VineMother.Instance smi)
	{
		return smi.LeftBranch == null && smi.RightBranch == null;
	}

	// Token: 0x06004F4C RID: 20300 RVA: 0x001CCCD8 File Offset: 0x001CAED8
	private static bool HasGrownAllBranches(VineMother.Instance smi)
	{
		return smi.HasGrownAllBranches;
	}

	// Token: 0x06004F4D RID: 20301 RVA: 0x001CCCE0 File Offset: 0x001CAEE0
	private static void SpawnBranchesIfNewGameSpawn(VineMother.Instance smi)
	{
		if (smi.IsNewGameSpawned)
		{
			VineMother.AttemptToSpawnBranches(smi);
		}
	}

	// Token: 0x06004F4E RID: 20302 RVA: 0x001CCCF0 File Offset: 0x001CAEF0
	private static void AttemptToSpawnBranches(VineMother.Instance smi, float dt)
	{
		VineMother.AttemptToSpawnBranches(smi);
	}

	// Token: 0x06004F4F RID: 20303 RVA: 0x001CCCF8 File Offset: 0x001CAEF8
	private static void AttemptToSpawnBranches(VineMother.Instance smi)
	{
		smi.AttemptToSpawnBranches();
	}

	// Token: 0x06004F50 RID: 20304 RVA: 0x001CCD00 File Offset: 0x001CAF00
	public static Notification CreateDeathNotification(VineMother.Instance smi)
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + smi.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x040034FD RID: 13565
	private const string GROW_ANIM_NAME = "grow";

	// Token: 0x040034FE RID: 13566
	private const string GROW_PST_ANIM_NAME = "grow_pst";

	// Token: 0x040034FF RID: 13567
	private const string IDLE_ANIM_NAME = "idle_full";

	// Token: 0x04003500 RID: 13568
	private const string WILT_ANIM_NAME = "wilt3";

	// Token: 0x04003501 RID: 13569
	public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State dead;

	// Token: 0x04003502 RID: 13570
	public VineMother.GrowingStates growing;

	// Token: 0x04003503 RID: 13571
	public VineMother.GrownStates grown;

	// Token: 0x04003504 RID: 13572
	public StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.BoolParameter IsGrown;

	// Token: 0x04003505 RID: 13573
	public StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.TargetParameter LeftBranch;

	// Token: 0x04003506 RID: 13574
	public StateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.TargetParameter RightBranch;

	// Token: 0x02001BF1 RID: 7153
	public class Def : PlantBranchGrowerBase<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.PlantBranchGrowerBaseDef
	{
	}

	// Token: 0x02001BF2 RID: 7154
	public class GrowingBranchesStates : GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State
	{
		// Token: 0x04008667 RID: 34407
		public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State growing;

		// Token: 0x04008668 RID: 34408
		public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State blocked;
	}

	// Token: 0x02001BF3 RID: 7155
	public class GrownStates : GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.PlantAliveSubState
	{
		// Token: 0x04008669 RID: 34409
		public VineMother.GrowingBranchesStates growingBranches;

		// Token: 0x0400866A RID: 34410
		public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State idle;

		// Token: 0x0400866B RID: 34411
		public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State wilt;
	}

	// Token: 0x02001BF4 RID: 7156
	public class GrowingStates : GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.PlantAliveSubState
	{
		// Token: 0x0400866C RID: 34412
		public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State growing;

		// Token: 0x0400866D RID: 34413
		public GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.State growing_pst;
	}

	// Token: 0x02001BF5 RID: 7157
	public new class Instance : GameStateMachine<VineMother, VineMother.Instance, IStateMachineTarget, VineMother.Def>.GameInstance
	{
		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x0600AC0D RID: 44045 RVA: 0x003CB309 File Offset: 0x003C9509
		public GameObject LeftBranch
		{
			get
			{
				return base.sm.LeftBranch.Get(this);
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x0600AC0E RID: 44046 RVA: 0x003CB31C File Offset: 0x003C951C
		public GameObject RightBranch
		{
			get
			{
				return base.sm.RightBranch.Get(this);
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x0600AC0F RID: 44047 RVA: 0x003CB32F File Offset: 0x003C952F
		public bool HasGrownAllBranches
		{
			get
			{
				return this.LeftBranch != null && this.RightBranch != null;
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x0600AC10 RID: 44048 RVA: 0x003CB34D File Offset: 0x003C954D
		public bool IsGrown
		{
			get
			{
				return this.growing.IsGrown();
			}
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x0600AC11 RID: 44049 RVA: 0x003CB35A File Offset: 0x003C955A
		public bool IsWild
		{
			get
			{
				return !this.receptacleMonitor.Replanted;
			}
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x0600AC12 RID: 44050 RVA: 0x003CB36C File Offset: 0x003C956C
		public bool IsOnPlanterBox
		{
			get
			{
				return !this.IsWild && this.receptacleMonitor.smi.ReceptacleObject != null && this.receptacleMonitor.smi.ReceptacleObject is PlantablePlot && (this.receptacleMonitor.smi.ReceptacleObject as PlantablePlot).IsOffGround;
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x0600AC13 RID: 44051 RVA: 0x003CB3CC File Offset: 0x003C95CC
		public int PlanterboxCell
		{
			get
			{
				if (!this.IsWild)
				{
					return Grid.PosToCell(this.receptacleMonitor.smi.ReceptacleObject);
				}
				return Grid.InvalidCell;
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x0600AC14 RID: 44052 RVA: 0x003CB3F1 File Offset: 0x003C95F1
		public bool IsWilting
		{
			get
			{
				return this.wiltCondition.IsWilting();
			}
		}

		// Token: 0x0600AC15 RID: 44053 RVA: 0x003CB400 File Offset: 0x003C9600
		public Instance(IStateMachineTarget master, VineMother.Def def) : base(master, def)
		{
			this.growing = base.GetComponent<Growing>();
			this.receptacleMonitor = base.GetComponent<ReceptacleMonitor>();
			this.wiltCondition = base.GetComponent<WiltCondition>();
			base.Subscribe(1119167081, new Action<object>(this.OnSpawnedByDiscovered));
			base.Subscribe(-266953818, delegate(object obj)
			{
				this.UpdateAutoHarvestValue();
			});
		}

		// Token: 0x0600AC16 RID: 44054 RVA: 0x003CB46C File Offset: 0x003C966C
		public void AttemptToSpawnBranches()
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (this.LeftBranch == null)
			{
				int cell2 = Grid.OffsetCell(cell, CellOffset.left);
				if (VineBranch.IsCellAvailable(base.gameObject, cell2, null))
				{
					GameObject gameObject = this.SpawnBranchOnCell(cell2);
					base.sm.LeftBranch.Set(gameObject, this, false);
					if (this.IsNewGameSpawned)
					{
						gameObject.Trigger(1119167081, null);
					}
				}
			}
			if (this.RightBranch == null)
			{
				int cell3 = Grid.OffsetCell(cell, CellOffset.right);
				if (VineBranch.IsCellAvailable(base.gameObject, cell3, null))
				{
					GameObject gameObject2 = this.SpawnBranchOnCell(cell3);
					base.sm.RightBranch.Set(gameObject2, this, false);
					if (this.IsNewGameSpawned)
					{
						gameObject2.Trigger(1119167081, null);
					}
				}
			}
			if (this.IsNewGameSpawned)
			{
				this.IsNewGameSpawned = false;
			}
		}

		// Token: 0x0600AC17 RID: 44055 RVA: 0x003CB549 File Offset: 0x003C9749
		public void DestroySelf(object o)
		{
			CreatureHelpers.DeselectCreature(base.gameObject);
			Util.KDestroyGameObject(base.gameObject);
		}

		// Token: 0x0600AC18 RID: 44056 RVA: 0x003CB561 File Offset: 0x003C9761
		private void OnSpawnedByDiscovered(object o)
		{
			this.IsNewGameSpawned = true;
			VineMother.MarkAsGrown(this);
		}

		// Token: 0x0600AC19 RID: 44057 RVA: 0x003CB570 File Offset: 0x003C9770
		private GameObject SpawnBranchOnCell(int cell)
		{
			Vector3 position = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.def.BRANCH_PREFAB_NAME), position);
			gameObject.SetActive(true);
			gameObject.GetSMI<VineBranch.Instance>().SetupRootInformation(this);
			return gameObject;
		}

		// Token: 0x0600AC1A RID: 44058 RVA: 0x003CB5B4 File Offset: 0x003C97B4
		public void UpdateAutoHarvestValue()
		{
			HarvestDesignatable component = base.GetComponent<HarvestDesignatable>();
			if (component != null)
			{
				if (this.LeftBranch != null)
				{
					VineBranch.Instance smi = this.LeftBranch.GetSMI<VineBranch.Instance>();
					if (smi != null)
					{
						smi.SetAutoHarvestInChainReaction(component.HarvestWhenReady);
					}
				}
				if (this.RightBranch != null)
				{
					VineBranch.Instance smi2 = this.RightBranch.GetSMI<VineBranch.Instance>();
					if (smi2 != null)
					{
						smi2.SetAutoHarvestInChainReaction(component.HarvestWhenReady);
					}
				}
			}
		}

		// Token: 0x0400866E RID: 34414
		public bool IsNewGameSpawned;

		// Token: 0x0400866F RID: 34415
		private Growing growing;

		// Token: 0x04008670 RID: 34416
		private ReceptacleMonitor receptacleMonitor;

		// Token: 0x04008671 RID: 34417
		private WiltCondition wiltCondition;
	}
}
