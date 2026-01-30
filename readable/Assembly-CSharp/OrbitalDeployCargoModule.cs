using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B96 RID: 2966
public class OrbitalDeployCargoModule : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>
{
	// Token: 0x0600588E RID: 22670 RVA: 0x002020D4 File Offset: 0x002002D4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.ClusterDestinationReached, delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			if (smi.AutoDeploy && smi.IsValidDropLocation())
			{
				smi.DeployCargoPods();
			}
		});
		this.grounded.DefaultState(this.grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false);
		this.grounded.loading.PlayAnim((OrbitalDeployCargoModule.StatesInstance smi) => smi.GetLoadingAnimName(), KAnim.PlayMode.Once).ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).OnAnimQueueComplete(this.grounded.loaded);
		this.grounded.loaded.ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).EventTransition(GameHashes.OnStorageChange, this.grounded.loading, (OrbitalDeployCargoModule.StatesInstance smi) => smi.NeedsVisualUpdate());
		this.grounded.empty.Enter(delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			this.numVisualCapsules.Set(0, smi, false);
		}).PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsTrue);
		this.not_grounded.DefaultState(this.not_grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.not_grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).OnSignal(this.emptyCargo, this.not_grounded.emptying);
		this.not_grounded.emptying.PlayAnim("deploying").GoTo(this.not_grounded.empty);
		this.not_grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.not_grounded.loaded, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsTrue);
	}

	// Token: 0x04003B6B RID: 15211
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.BoolParameter hasCargo;

	// Token: 0x04003B6C RID: 15212
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.Signal emptyCargo;

	// Token: 0x04003B6D RID: 15213
	public OrbitalDeployCargoModule.GroundedStates grounded;

	// Token: 0x04003B6E RID: 15214
	public OrbitalDeployCargoModule.NotGroundedStates not_grounded;

	// Token: 0x04003B6F RID: 15215
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IntParameter numVisualCapsules;

	// Token: 0x02001D1D RID: 7453
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04008A60 RID: 35424
		public float numCapsules;
	}

	// Token: 0x02001D1E RID: 7454
	public class GroundedStates : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State
	{
		// Token: 0x04008A61 RID: 35425
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loading;

		// Token: 0x04008A62 RID: 35426
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loaded;

		// Token: 0x04008A63 RID: 35427
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State empty;
	}

	// Token: 0x02001D1F RID: 7455
	public class NotGroundedStates : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State
	{
		// Token: 0x04008A64 RID: 35428
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loaded;

		// Token: 0x04008A65 RID: 35429
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State emptying;

		// Token: 0x04008A66 RID: 35430
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State empty;
	}

	// Token: 0x02001D20 RID: 7456
	public class StatesInstance : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.GameInstance, IEmptyableCargo
	{
		// Token: 0x0600B020 RID: 45088 RVA: 0x003D9CCC File Offset: 0x003D7ECC
		public StatesInstance(IStateMachineTarget master, OrbitalDeployCargoModule.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<Storage>();
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new LoadingCompleteCondition(this.storage));
			base.gameObject.Subscribe(-1683615038, new Action<object>(this.SetupMeter));
		}

		// Token: 0x0600B021 RID: 45089 RVA: 0x003D9D22 File Offset: 0x003D7F22
		private void SetupMeter(object obj)
		{
			KBatchedAnimTracker componentInChildren = base.gameObject.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
		}

		// Token: 0x0600B022 RID: 45090 RVA: 0x003D9D3C File Offset: 0x003D7F3C
		protected override void OnCleanUp()
		{
			base.gameObject.Unsubscribe(-1683615038, new Action<object>(this.SetupMeter));
			base.OnCleanUp();
		}

		// Token: 0x0600B023 RID: 45091 RVA: 0x003D9D60 File Offset: 0x003D7F60
		public bool NeedsVisualUpdate()
		{
			int num = base.sm.numVisualCapsules.Get(this);
			int num2 = Mathf.FloorToInt(this.storage.MassStored() / 200f);
			if (num < num2)
			{
				base.sm.numVisualCapsules.Delta(1, this);
				return true;
			}
			return false;
		}

		// Token: 0x0600B024 RID: 45092 RVA: 0x003D9DB0 File Offset: 0x003D7FB0
		public string GetLoadingAnimName()
		{
			int num = base.sm.numVisualCapsules.Get(this);
			int num2 = Mathf.RoundToInt(this.storage.capacityKg / 200f);
			if (num == num2)
			{
				return "loading6_full";
			}
			if (num == num2 - 1)
			{
				return "loading5";
			}
			if (num == num2 - 2)
			{
				return "loading4";
			}
			if (num == num2 - 3 || num > 2)
			{
				return "loading3_repeat";
			}
			if (num == 2)
			{
				return "loading2";
			}
			if (num == 1)
			{
				return "loading1";
			}
			return "deployed";
		}

		// Token: 0x0600B025 RID: 45093 RVA: 0x003D9E34 File Offset: 0x003D8034
		public void DeployCargoPods()
		{
			Clustercraft component = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			ClusterGridEntity orbitAsteroid = component.GetOrbitAsteroid();
			if (orbitAsteroid != null)
			{
				WorldContainer component2 = orbitAsteroid.GetComponent<WorldContainer>();
				int id = component2.id;
				Vector3 position = new Vector3(component2.minimumBounds.x + 1f, component2.maximumBounds.y, Grid.GetLayerZ(Grid.SceneLayer.Front));
				while (this.storage.MassStored() > 0f)
				{
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RailGunPayload"), position);
					gameObject.GetComponent<Pickupable>().deleteOffGrid = false;
					float num = 0f;
					while (num < 200f && this.storage.MassStored() > 0f)
					{
						num += this.storage.Transfer(gameObject.GetComponent<Storage>(), GameTags.Stored, 200f - num, false, true);
					}
					gameObject.SetActive(true);
					gameObject.GetSMI<RailGunPayload.StatesInstance>().Travel(component.Location, component2.GetMyWorldLocation());
				}
			}
			this.CheckIfLoaded();
		}

		// Token: 0x0600B026 RID: 45094 RVA: 0x003D9F54 File Offset: 0x003D8154
		public bool CheckIfLoaded()
		{
			bool flag = this.storage.MassStored() > 0f;
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x0600B027 RID: 45095 RVA: 0x003D9F9D File Offset: 0x003D819D
		public bool IsValidDropLocation()
		{
			return base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetOrbitAsteroid() != null;
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x0600B028 RID: 45096 RVA: 0x003D9FBA File Offset: 0x003D81BA
		// (set) Token: 0x0600B029 RID: 45097 RVA: 0x003D9FC2 File Offset: 0x003D81C2
		public bool AutoDeploy
		{
			get
			{
				return this.autoDeploy;
			}
			set
			{
				this.autoDeploy = value;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x0600B02A RID: 45098 RVA: 0x003D9FCB File Offset: 0x003D81CB
		public bool CanAutoDeploy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600B02B RID: 45099 RVA: 0x003D9FCE File Offset: 0x003D81CE
		public void EmptyCargo()
		{
			this.DeployCargoPods();
		}

		// Token: 0x0600B02C RID: 45100 RVA: 0x003D9FD6 File Offset: 0x003D81D6
		public bool CanEmptyCargo()
		{
			return base.sm.hasCargo.Get(base.smi) && this.IsValidDropLocation();
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x0600B02D RID: 45101 RVA: 0x003D9FF8 File Offset: 0x003D81F8
		public bool ChooseDuplicant
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x0600B02E RID: 45102 RVA: 0x003D9FFB File Offset: 0x003D81FB
		// (set) Token: 0x0600B02F RID: 45103 RVA: 0x003D9FFE File Offset: 0x003D81FE
		public MinionIdentity ChosenDuplicant
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x0600B030 RID: 45104 RVA: 0x003DA000 File Offset: 0x003D8200
		public bool ModuleDeployed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04008A67 RID: 35431
		private Storage storage;

		// Token: 0x04008A68 RID: 35432
		[Serialize]
		private bool autoDeploy;
	}
}
