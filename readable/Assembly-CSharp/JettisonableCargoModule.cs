using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B81 RID: 2945
public class JettisonableCargoModule : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>
{
	// Token: 0x060057C9 RID: 22473 RVA: 0x001FEC54 File Offset: 0x001FCE54
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		});
		this.grounded.DefaultState(this.grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false);
		this.grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsFalse);
		this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsTrue);
		this.not_grounded.DefaultState(this.not_grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.not_grounded.empty, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsFalse).OnSignal(this.emptyCargo, this.not_grounded.emptying);
		this.not_grounded.emptying.PlayAnim("deploying").Update(delegate(JettisonableCargoModule.StatesInstance smi, float dt)
		{
			if (smi.CheckReadyForFinalDeploy())
			{
				smi.FinalDeploy();
				smi.GoTo(smi.sm.not_grounded.empty);
			}
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.ClusterLocationChanged, (JettisonableCargoModule.StatesInstance smi) => Game.Instance, this.not_grounded, null).Exit(delegate(JettisonableCargoModule.StatesInstance smi)
		{
			smi.CancelPendingDeploy();
		});
		this.not_grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.not_grounded.loaded, GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.IsTrue);
	}

	// Token: 0x04003AD9 RID: 15065
	public StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.BoolParameter hasCargo;

	// Token: 0x04003ADA RID: 15066
	public StateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.Signal emptyCargo;

	// Token: 0x04003ADB RID: 15067
	public JettisonableCargoModule.GroundedStates grounded;

	// Token: 0x04003ADC RID: 15068
	public JettisonableCargoModule.NotGroundedStates not_grounded;

	// Token: 0x02001D01 RID: 7425
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040089FF RID: 35327
		public DefComponent<Storage> landerContainer;

		// Token: 0x04008A00 RID: 35328
		public Tag landerPrefabID;

		// Token: 0x04008A01 RID: 35329
		public Vector3 cargoDropOffset;

		// Token: 0x04008A02 RID: 35330
		public string clusterMapFXPrefabID;
	}

	// Token: 0x02001D02 RID: 7426
	public class GroundedStates : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State
	{
		// Token: 0x04008A03 RID: 35331
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State loaded;

		// Token: 0x04008A04 RID: 35332
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State empty;
	}

	// Token: 0x02001D03 RID: 7427
	public class NotGroundedStates : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State
	{
		// Token: 0x04008A05 RID: 35333
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State loaded;

		// Token: 0x04008A06 RID: 35334
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State emptying;

		// Token: 0x04008A07 RID: 35335
		public GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.State empty;
	}

	// Token: 0x02001D04 RID: 7428
	public class StatesInstance : GameStateMachine<JettisonableCargoModule, JettisonableCargoModule.StatesInstance, IStateMachineTarget, JettisonableCargoModule.Def>.GameInstance, IEmptyableCargo
	{
		// Token: 0x0600AF90 RID: 44944 RVA: 0x003D6637 File Offset: 0x003D4837
		public StatesInstance(IStateMachineTarget master, JettisonableCargoModule.Def def) : base(master, def)
		{
			this.landerContainer = def.landerContainer.Get(this);
		}

		// Token: 0x0600AF91 RID: 44945 RVA: 0x003D6654 File Offset: 0x003D4854
		private void ChooseLanderLocation()
		{
			ClusterGridEntity stableOrbitAsteroid = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetStableOrbitAsteroid();
			if (stableOrbitAsteroid != null)
			{
				WorldContainer component = stableOrbitAsteroid.GetComponent<WorldContainer>();
				Placeable component2 = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<Placeable>();
				component2.restrictWorldId = component.id;
				component.LookAtSurface();
				ClusterManager.Instance.SetActiveWorld(component.id);
				ManagementMenu.Instance.CloseAll();
				PlaceTool.Instance.Activate(component2, new Action<Placeable, int>(this.OnLanderPlaced));
			}
		}

		// Token: 0x0600AF92 RID: 44946 RVA: 0x003D66EC File Offset: 0x003D48EC
		private void OnLanderPlaced(Placeable lander, int cell)
		{
			this.landerPlaced = true;
			this.landerPlacementCell = cell;
			if (lander.GetComponent<MinionStorage>() != null)
			{
				this.OpenMoveChoreForChosenDuplicant();
			}
			ManagementMenu.Instance.ToggleClusterMap();
			base.sm.emptyCargo.Trigger(base.smi);
			ClusterMapScreen.Instance.SelectEntity(base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<ClusterGridEntity>(), true);
		}

		// Token: 0x0600AF93 RID: 44947 RVA: 0x003D6758 File Offset: 0x003D4958
		private void OpenMoveChoreForChosenDuplicant()
		{
			RocketPassengerMonitor.Instance smi = this.ChosenDuplicant.GetSMI<RocketPassengerMonitor.Instance>();
			if (smi != null)
			{
				RocketModuleCluster component = base.master.GetComponent<RocketModuleCluster>();
				Clustercraft craft = component.CraftInterface.GetComponent<Clustercraft>();
				MinionStorage storage = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<MinionStorage>();
				this.EnableTeleport(true);
				smi.SetModuleDeployChore(this.landerPlacementCell, delegate(Chore obj)
				{
					Game.Instance.assignmentManager.RemoveFromWorld(this.ChosenDuplicant.assignableProxy.Get(), craft.ModuleInterface.GetInteriorWorld().id);
					storage.SerializeMinion(this.ChosenDuplicant.gameObject);
					this.EnableTeleport(false);
				});
			}
		}

		// Token: 0x0600AF94 RID: 44948 RVA: 0x003D67E0 File Offset: 0x003D49E0
		private void EnableTeleport(bool enable)
		{
			ClustercraftExteriorDoor component = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().GetComponent<ClustercraftExteriorDoor>();
			ClustercraftInteriorDoor interiorDoor = component.GetInteriorDoor();
			AccessControl component2 = component.GetInteriorDoor().GetComponent<AccessControl>();
			NavTeleporter component3 = base.GetComponent<NavTeleporter>();
			if (enable)
			{
				component3.SetOverrideCell(this.landerPlacementCell);
				interiorDoor.GetComponent<NavTeleporter>().SetTarget(component3);
				component3.SetTarget(interiorDoor.GetComponent<NavTeleporter>());
				using (List<MinionIdentity>.Enumerator enumerator = Components.MinionIdentities.GetWorldItems(interiorDoor.GetMyWorldId(), false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MinionIdentity minionIdentity = enumerator.Current;
						component2.SetPermission(minionIdentity.assignableProxy.Get(), (minionIdentity == this.ChosenDuplicant) ? AccessControl.Permission.Both : AccessControl.Permission.Neither);
					}
					return;
				}
			}
			component3.SetOverrideCell(-1);
			interiorDoor.GetComponent<NavTeleporter>().SetTarget(null);
			component3.SetTarget(null);
			component2.SetPermission(this.ChosenDuplicant.assignableProxy.Get(), AccessControl.Permission.Neither);
		}

		// Token: 0x0600AF95 RID: 44949 RVA: 0x003D68F8 File Offset: 0x003D4AF8
		public void FinalDeploy()
		{
			this.landerPlaced = false;
			Placeable component = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<Placeable>();
			this.landerContainer.FindFirst(base.def.landerPrefabID);
			this.landerContainer.Drop(component.gameObject, true);
			TreeFilterable component2 = base.GetComponent<TreeFilterable>();
			TreeFilterable component3 = component.GetComponent<TreeFilterable>();
			if (component3 != null)
			{
				component3.UpdateFilters(component2.AcceptedTags);
			}
			Storage component4 = component.GetComponent<Storage>();
			if (component4 != null)
			{
				Storage[] components = base.gameObject.GetComponents<Storage>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].Transfer(component4, false, true);
				}
			}
			Vector3 position = Grid.CellToPosCBC(this.landerPlacementCell, Grid.SceneLayer.Building);
			component.transform.SetPosition(position);
			component.gameObject.SetActive(true);
			base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().gameObject.Trigger(1792516731, component);
			component.Trigger(1792516731, base.gameObject);
			GameObject gameObject = Assets.TryGetPrefab(base.smi.def.clusterMapFXPrefabID);
			if (gameObject != null)
			{
				this.clusterMapFX = GameUtil.KInstantiate(gameObject, Grid.SceneLayer.Background, null, 0);
				this.clusterMapFX.SetActive(true);
				this.clusterMapFX.GetComponent<ClusterFXEntity>().Init(component.GetMyWorldLocation(), Vector3.zero);
				component.Subscribe(1969584890, delegate(object data)
				{
					if (!this.clusterMapFX.IsNullOrDestroyed())
					{
						Util.KDestroyGameObject(this.clusterMapFX);
					}
				});
				component.Subscribe(1591811118, delegate(object data)
				{
					if (!this.clusterMapFX.IsNullOrDestroyed())
					{
						Util.KDestroyGameObject(this.clusterMapFX);
					}
				});
			}
		}

		// Token: 0x0600AF96 RID: 44950 RVA: 0x003D6AA0 File Offset: 0x003D4CA0
		public bool CheckReadyForFinalDeploy()
		{
			MinionStorage component = this.landerContainer.FindFirst(base.def.landerPrefabID).GetComponent<MinionStorage>();
			return !(component != null) || component.GetStoredMinionInfo().Count > 0;
		}

		// Token: 0x0600AF97 RID: 44951 RVA: 0x003D6AE2 File Offset: 0x003D4CE2
		public void CancelPendingDeploy()
		{
			this.landerPlaced = false;
			if (this.ChosenDuplicant != null && this.CheckIfLoaded())
			{
				this.ChosenDuplicant.GetSMI<RocketPassengerMonitor.Instance>().CancelModuleDeployChore();
			}
		}

		// Token: 0x0600AF98 RID: 44952 RVA: 0x003D6B14 File Offset: 0x003D4D14
		public bool CheckIfLoaded()
		{
			bool flag = false;
			using (List<GameObject>.Enumerator enumerator = this.landerContainer.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.PrefabID() == base.def.landerPrefabID)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x0600AF99 RID: 44953 RVA: 0x003D6BAC File Offset: 0x003D4DAC
		public bool IsValidDropLocation()
		{
			return base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetStableOrbitAsteroid() != null;
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x0600AF9A RID: 44954 RVA: 0x003D6BC9 File Offset: 0x003D4DC9
		// (set) Token: 0x0600AF9B RID: 44955 RVA: 0x003D6BD1 File Offset: 0x003D4DD1
		public bool AutoDeploy { get; set; }

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x0600AF9C RID: 44956 RVA: 0x003D6BDA File Offset: 0x003D4DDA
		public bool CanAutoDeploy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600AF9D RID: 44957 RVA: 0x003D6BDD File Offset: 0x003D4DDD
		public void EmptyCargo()
		{
			this.ChooseLanderLocation();
		}

		// Token: 0x0600AF9E RID: 44958 RVA: 0x003D6BE8 File Offset: 0x003D4DE8
		public bool CanEmptyCargo()
		{
			return base.sm.hasCargo.Get(base.smi) && this.IsValidDropLocation() && (!this.ChooseDuplicant || (this.ChosenDuplicant != null && !this.ChosenDuplicant.HasTag(GameTags.Dead))) && !this.landerPlaced;
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x0600AF9F RID: 44959 RVA: 0x003D6C48 File Offset: 0x003D4E48
		public bool ChooseDuplicant
		{
			get
			{
				GameObject gameObject = this.landerContainer.FindFirst(base.def.landerPrefabID);
				return !(gameObject == null) && gameObject.GetComponent<MinionStorage>() != null;
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x0600AFA0 RID: 44960 RVA: 0x003D6C83 File Offset: 0x003D4E83
		public bool ModuleDeployed
		{
			get
			{
				return this.landerPlaced;
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x0600AFA1 RID: 44961 RVA: 0x003D6C8B File Offset: 0x003D4E8B
		// (set) Token: 0x0600AFA2 RID: 44962 RVA: 0x003D6C93 File Offset: 0x003D4E93
		public MinionIdentity ChosenDuplicant
		{
			get
			{
				return this.chosenDuplicant;
			}
			set
			{
				this.chosenDuplicant = value;
			}
		}

		// Token: 0x04008A08 RID: 35336
		private Storage landerContainer;

		// Token: 0x04008A09 RID: 35337
		private bool landerPlaced;

		// Token: 0x04008A0A RID: 35338
		private MinionIdentity chosenDuplicant;

		// Token: 0x04008A0B RID: 35339
		private int landerPlacementCell;

		// Token: 0x04008A0C RID: 35340
		public GameObject clusterMapFX;
	}
}
