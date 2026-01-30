using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020007E4 RID: 2020
[SerializationConfig(MemberSerialization.OptIn)]
public class RailGunPayload : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>
{
	// Token: 0x060035D4 RID: 13780 RVA: 0x0012FA5C File Offset: 0x0012DC5C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded.idle;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.grounded.DefaultState(this.grounded.idle).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(true, smi, false);
		}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.RailgunpayloadNeedsEmptying, null).ToggleTag(GameTags.RailGunPayloadEmptyable).ToggleTag(GameTags.ClusterEntityGrounded).EventHandler(GameHashes.DroppedAll, delegate(RailGunPayload.StatesInstance smi)
		{
			smi.OnDroppedAll();
		}).OnSignal(this.launch, this.takeoff);
		this.grounded.idle.PlayAnim("idle");
		this.grounded.crater.Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.animController.randomiseLoopedOffset = true;
			Prioritizable.AddRef(smi.gameObject);
		}).Exit(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.animController.randomiseLoopedOffset = false;
		}).PlayAnim("landed", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStore, this.grounded.idle, null);
		this.takeoff.DefaultState(this.takeoff.launch).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(false, smi, false);
		}).PlayAnim("launching").OnSignal(this.beginTravelling, this.travel);
		this.takeoff.launch.Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.StartTakeoff();
		}).GoTo(this.takeoff.airborne);
		this.takeoff.airborne.Update("Launch", delegate(RailGunPayload.StatesInstance smi, float dt)
		{
			smi.UpdateLaunch(dt);
		}, UpdateRate.SIM_EVERY_TICK, false);
		this.travel.DefaultState(this.travel.travelling).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			this.onSurface.Set(false, smi, false);
		}).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.MoveToSpace();
		}).PlayAnim("idle").ToggleTag(GameTags.EntityInSpace).ToggleMainStatusItem(Db.Get().BuildingStatusItems.InFlight, (RailGunPayload.StatesInstance smi) => smi.GetComponent<ClusterTraveler>());
		this.travel.travelling.EventTransition(GameHashes.ClusterDestinationReached, this.travel.transferWorlds, null);
		this.travel.transferWorlds.Exit(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.StartLand();
		}).GoTo(this.landing.landing);
		this.landing.DefaultState(this.landing.landing).ParamTransition<bool>(this.onSurface, this.grounded.crater, GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IsTrue).ParamTransition<int>(this.destinationWorld, this.takeoff, (RailGunPayload.StatesInstance smi, int p) => p != -1).Enter(delegate(RailGunPayload.StatesInstance smi)
		{
			smi.MoveToWorld();
		});
		this.landing.landing.PlayAnim("falling", KAnim.PlayMode.Loop).UpdateTransition(this.landing.impact, (RailGunPayload.StatesInstance smi, float dt) => smi.UpdateLanding(dt), UpdateRate.SIM_200ms, false).ToggleGravity(this.landing.impact);
		this.landing.impact.PlayAnim("land").TriggerOnEnter(GameHashes.JettisonCargo, null).OnAnimQueueComplete(this.grounded.crater);
	}

	// Token: 0x040020B2 RID: 8370
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IntParameter destinationWorld = new StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.IntParameter(-1);

	// Token: 0x040020B3 RID: 8371
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.BoolParameter onSurface = new StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.BoolParameter(false);

	// Token: 0x040020B4 RID: 8372
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.Signal beginTravelling;

	// Token: 0x040020B5 RID: 8373
	public StateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.Signal launch;

	// Token: 0x040020B6 RID: 8374
	public RailGunPayload.TakeoffStates takeoff;

	// Token: 0x040020B7 RID: 8375
	public RailGunPayload.TravelStates travel;

	// Token: 0x040020B8 RID: 8376
	public RailGunPayload.LandingStates landing;

	// Token: 0x040020B9 RID: 8377
	public RailGunPayload.GroundedStates grounded;

	// Token: 0x02001740 RID: 5952
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007727 RID: 30503
		public bool attractToBeacons;

		// Token: 0x04007728 RID: 30504
		public string clusterAnimSymbolSwapTarget;

		// Token: 0x04007729 RID: 30505
		public List<string> randomClusterSymbolSwaps;

		// Token: 0x0400772A RID: 30506
		public string worldAnimSymbolSwapTarget;

		// Token: 0x0400772B RID: 30507
		public List<string> randomWorldSymbolSwaps;
	}

	// Token: 0x02001741 RID: 5953
	public class TakeoffStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x0400772C RID: 30508
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State launch;

		// Token: 0x0400772D RID: 30509
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State airborne;
	}

	// Token: 0x02001742 RID: 5954
	public class TravelStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x0400772E RID: 30510
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State travelling;

		// Token: 0x0400772F RID: 30511
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State transferWorlds;
	}

	// Token: 0x02001743 RID: 5955
	public class LandingStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x04007730 RID: 30512
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State landing;

		// Token: 0x04007731 RID: 30513
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State impact;
	}

	// Token: 0x02001744 RID: 5956
	public class GroundedStates : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State
	{
		// Token: 0x04007732 RID: 30514
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State crater;

		// Token: 0x04007733 RID: 30515
		public GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.State idle;
	}

	// Token: 0x02001745 RID: 5957
	public class StatesInstance : GameStateMachine<RailGunPayload, RailGunPayload.StatesInstance, IStateMachineTarget, RailGunPayload.Def>.GameInstance
	{
		// Token: 0x06009A78 RID: 39544 RVA: 0x00391364 File Offset: 0x0038F564
		public StatesInstance(IStateMachineTarget master, RailGunPayload.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			DebugUtil.Assert(def.clusterAnimSymbolSwapTarget == null == (def.worldAnimSymbolSwapTarget == null), "Must specify both or neither symbol swap targets!");
			DebugUtil.Assert((def.randomClusterSymbolSwaps == null && def.randomWorldSymbolSwaps == null) || def.randomClusterSymbolSwaps.Count == def.randomWorldSymbolSwaps.Count, "Must specify the same number of swaps for both world and cluster!");
			if (def.clusterAnimSymbolSwapTarget != null && def.worldAnimSymbolSwapTarget != null)
			{
				if (this.randomSymbolSwapIndex == -1)
				{
					this.randomSymbolSwapIndex = UnityEngine.Random.Range(0, def.randomClusterSymbolSwaps.Count);
				}
				base.GetComponent<BallisticClusterGridEntity>().SwapSymbolFromSameAnim(def.clusterAnimSymbolSwapTarget, def.randomClusterSymbolSwaps[this.randomSymbolSwapIndex]);
				KAnim.Build.Symbol symbol = this.animController.AnimFiles[0].GetData().build.GetSymbol(def.randomWorldSymbolSwaps[this.randomSymbolSwapIndex]);
				this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride(def.worldAnimSymbolSwapTarget, symbol, 0);
			}
		}

		// Token: 0x06009A79 RID: 39545 RVA: 0x00391488 File Offset: 0x0038F688
		public void Launch(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
			base.sm.destinationWorld.Set(asteroidWorldIdAtLocation, this, false);
			this.GoTo(base.sm.takeoff);
		}

		// Token: 0x06009A7A RID: 39546 RVA: 0x003914D0 File Offset: 0x0038F6D0
		public void Travel(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
			base.sm.destinationWorld.Set(asteroidWorldIdAtLocation, this, false);
			this.GoTo(base.sm.travel);
		}

		// Token: 0x06009A7B RID: 39547 RVA: 0x00391516 File Offset: 0x0038F716
		public void StartTakeoff()
		{
			if (GameComps.Fallers.Has(base.gameObject))
			{
				GameComps.Fallers.Remove(base.gameObject);
			}
		}

		// Token: 0x06009A7C RID: 39548 RVA: 0x0039153C File Offset: 0x0038F73C
		public void StartLand()
		{
			WorldContainer worldContainer = ClusterManager.Instance.GetWorld(base.sm.destinationWorld.Get(this));
			if (worldContainer == null)
			{
				worldContainer = ClusterManager.Instance.GetStartWorld();
			}
			int num = Grid.InvalidCell;
			if (base.def.attractToBeacons)
			{
				num = ClusterManager.Instance.GetLandingBeaconLocation(worldContainer.id);
			}
			int num4;
			if (num != Grid.InvalidCell)
			{
				int num2;
				int num3;
				Grid.CellToXY(num, out num2, out num3);
				int minInclusive = Mathf.Max(num2 - 3, (int)worldContainer.minimumBounds.x);
				int maxExclusive = Mathf.Min(num2 + 3, (int)worldContainer.maximumBounds.x);
				num4 = Mathf.RoundToInt((float)UnityEngine.Random.Range(minInclusive, maxExclusive));
			}
			else
			{
				num4 = Mathf.RoundToInt(UnityEngine.Random.Range(worldContainer.minimumBounds.x + 3f, worldContainer.maximumBounds.x - 3f));
			}
			Vector3 position = new Vector3((float)num4 + 0.5f, worldContainer.maximumBounds.y - 1f, Grid.GetLayerZ(Grid.SceneLayer.Front));
			base.transform.SetPosition(position);
			if (GameComps.Fallers.Has(base.gameObject))
			{
				GameComps.Fallers.Remove(base.gameObject);
			}
			GameComps.Fallers.Add(base.gameObject, new Vector2(0f, -10f));
			base.sm.destinationWorld.Set(-1, this, false);
		}

		// Token: 0x06009A7D RID: 39549 RVA: 0x003916A4 File Offset: 0x0038F8A4
		public void UpdateLaunch(float dt)
		{
			if (base.gameObject.GetMyWorld() != null)
			{
				Vector3 position = base.transform.GetPosition() + new Vector3(0f, this.takeoffVelocity * dt, 0f);
				base.transform.SetPosition(position);
				return;
			}
			base.sm.beginTravelling.Trigger(this);
			ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
			if (ClusterGrid.Instance.GetAsteroidAtCell(component.Location) != null)
			{
				base.GetComponent<ClusterTraveler>().AdvancePathOneStep();
			}
		}

		// Token: 0x06009A7E RID: 39550 RVA: 0x00391738 File Offset: 0x0038F938
		public bool UpdateLanding(float dt)
		{
			if (base.gameObject.GetMyWorld() != null)
			{
				Vector3 position = base.transform.GetPosition();
				position.y -= 0.5f;
				int cell = Grid.PosToCell(position);
				if (Grid.IsWorldValidCell(cell) && Grid.IsSolidCell(cell))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009A7F RID: 39551 RVA: 0x0039178E File Offset: 0x0038F98E
		public void OnDroppedAll()
		{
			base.gameObject.DeleteObject();
		}

		// Token: 0x06009A80 RID: 39552 RVA: 0x0039179B File Offset: 0x0038F99B
		public bool IsTraveling()
		{
			return base.IsInsideState(base.sm.travel.travelling);
		}

		// Token: 0x06009A81 RID: 39553 RVA: 0x003917B4 File Offset: 0x0038F9B4
		public void MoveToSpace()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = false;
			}
			base.gameObject.transform.SetPosition(Grid.OffWorldPosition);
		}

		// Token: 0x06009A82 RID: 39554 RVA: 0x003917F0 File Offset: 0x0038F9F0
		public void MoveToWorld()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = true;
			}
			Storage component2 = base.GetComponent<Storage>();
			if (component2 != null)
			{
				component2.SetContentsDeleteOffGrid(true);
			}
		}

		// Token: 0x04007734 RID: 30516
		[Serialize]
		public float takeoffVelocity;

		// Token: 0x04007735 RID: 30517
		[Serialize]
		private int randomSymbolSwapIndex = -1;

		// Token: 0x04007736 RID: 30518
		public KBatchedAnimController animController;
	}
}
