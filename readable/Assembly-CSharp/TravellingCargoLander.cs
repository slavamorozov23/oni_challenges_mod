using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000820 RID: 2080
[SerializationConfig(MemberSerialization.OptIn)]
public class TravellingCargoLander : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>
{
	// Token: 0x06003898 RID: 14488 RVA: 0x0013C65C File Offset: 0x0013A85C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.init;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.InitializeOperationalFlag(RocketModule.landedFlag, false).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		});
		this.init.ParamTransition<bool>(this.isLanding, this.landing.landing, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsTrue).ParamTransition<bool>(this.isLanded, this.grounded, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsTrue).GoTo(this.travel);
		this.travel.DefaultState(this.travel.travelling).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.MoveToSpace();
		}).PlayAnim("idle").ToggleTag(GameTags.EntityInSpace).ToggleMainStatusItem(Db.Get().BuildingStatusItems.InFlight, (TravellingCargoLander.StatesInstance smi) => smi.GetComponent<ClusterTraveler>());
		this.travel.travelling.EventTransition(GameHashes.ClusterDestinationReached, this.travel.transferWorlds, null);
		this.travel.transferWorlds.Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.StartLand();
		}).GoTo(this.landing.landing);
		this.landing.Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			this.isLanding.Set(true, smi, false);
		}).Exit(delegate(TravellingCargoLander.StatesInstance smi)
		{
			this.isLanding.Set(false, smi, false);
		});
		this.landing.landing.PlayAnim("landing", KAnim.PlayMode.Loop).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.ResetAnimPosition();
		}).Update(delegate(TravellingCargoLander.StatesInstance smi, float dt)
		{
			smi.LandingUpdate(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).Transition(this.landing.impact, (TravellingCargoLander.StatesInstance smi) => smi.flightAnimOffset <= 0f, UpdateRate.SIM_200ms).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.MoveToWorld();
		});
		this.landing.impact.PlayAnim("grounded_pre").OnAnimQueueComplete(this.grounded);
		this.grounded.DefaultState(this.grounded.loaded).ToggleTag(GameTags.ClusterEntityGrounded).ToggleOperationalFlag(RocketModule.landedFlag).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			this.isLanded.Set(true, smi, false);
		});
		this.grounded.loaded.PlayAnim("grounded").ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsFalse).OnSignal(this.emptyCargo, this.grounded.emptying).Enter(delegate(TravellingCargoLander.StatesInstance smi)
		{
			smi.DoLand();
		});
		this.grounded.emptying.PlayAnim("deploying").TriggerOnEnter(GameHashes.JettisonCargo, null).OnAnimQueueComplete(this.grounded.empty);
		this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IsTrue);
	}

	// Token: 0x04002261 RID: 8801
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IntParameter destinationWorld = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.IntParameter(-1);

	// Token: 0x04002262 RID: 8802
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter isLanding = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter(false);

	// Token: 0x04002263 RID: 8803
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter isLanded = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter(false);

	// Token: 0x04002264 RID: 8804
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter hasCargo = new StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.BoolParameter(false);

	// Token: 0x04002265 RID: 8805
	public StateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.Signal emptyCargo;

	// Token: 0x04002266 RID: 8806
	public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State init;

	// Token: 0x04002267 RID: 8807
	public TravellingCargoLander.TravelStates travel;

	// Token: 0x04002268 RID: 8808
	public TravellingCargoLander.LandingStates landing;

	// Token: 0x04002269 RID: 8809
	public TravellingCargoLander.GroundedStates grounded;

	// Token: 0x020017BE RID: 6078
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007896 RID: 30870
		public int landerWidth = 1;

		// Token: 0x04007897 RID: 30871
		public float landingSpeed = 5f;

		// Token: 0x04007898 RID: 30872
		public bool deployOnLanding;
	}

	// Token: 0x020017BF RID: 6079
	public class TravelStates : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State
	{
		// Token: 0x04007899 RID: 30873
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State travelling;

		// Token: 0x0400789A RID: 30874
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State transferWorlds;
	}

	// Token: 0x020017C0 RID: 6080
	public class LandingStates : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State
	{
		// Token: 0x0400789B RID: 30875
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State landing;

		// Token: 0x0400789C RID: 30876
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State impact;
	}

	// Token: 0x020017C1 RID: 6081
	public class GroundedStates : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State
	{
		// Token: 0x0400789D RID: 30877
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State loaded;

		// Token: 0x0400789E RID: 30878
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State emptying;

		// Token: 0x0400789F RID: 30879
		public GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.State empty;
	}

	// Token: 0x020017C2 RID: 6082
	public class StatesInstance : GameStateMachine<TravellingCargoLander, TravellingCargoLander.StatesInstance, IStateMachineTarget, TravellingCargoLander.Def>.GameInstance
	{
		// Token: 0x06009C66 RID: 40038 RVA: 0x003994C9 File Offset: 0x003976C9
		public StatesInstance(IStateMachineTarget master, TravellingCargoLander.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
		}

		// Token: 0x06009C67 RID: 40039 RVA: 0x003994EC File Offset: 0x003976EC
		public void Travel(AxialI source, AxialI destination)
		{
			base.GetComponent<BallisticClusterGridEntity>().Configure(source, destination);
			int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
			base.sm.destinationWorld.Set(asteroidWorldIdAtLocation, this, false);
			this.GoTo(base.sm.travel);
		}

		// Token: 0x06009C68 RID: 40040 RVA: 0x00399534 File Offset: 0x00397734
		public void StartLand()
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(base.sm.destinationWorld.Get(this));
			Vector3 position = Grid.CellToPosCBC(ClusterManager.Instance.GetRandomSurfaceCell(world.id, base.def.landerWidth, true), this.animController.sceneLayer);
			base.transform.SetPosition(position);
		}

		// Token: 0x06009C69 RID: 40041 RVA: 0x00399598 File Offset: 0x00397798
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

		// Token: 0x06009C6A RID: 40042 RVA: 0x003995F0 File Offset: 0x003977F0
		public void MoveToSpace()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = false;
			}
			base.gameObject.transform.SetPosition(new Vector3(-1f, -1f, Grid.GetLayerZ(this.animController.sceneLayer)));
		}

		// Token: 0x06009C6B RID: 40043 RVA: 0x00399644 File Offset: 0x00397844
		public void MoveToWorld()
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = true;
			}
		}

		// Token: 0x06009C6C RID: 40044 RVA: 0x00399668 File Offset: 0x00397868
		public void ResetAnimPosition()
		{
			this.animController.Offset = Vector3.up * this.flightAnimOffset;
		}

		// Token: 0x06009C6D RID: 40045 RVA: 0x00399685 File Offset: 0x00397885
		public void LandingUpdate(float dt)
		{
			this.flightAnimOffset = Mathf.Max(this.flightAnimOffset - dt * base.def.landingSpeed, 0f);
			this.ResetAnimPosition();
		}

		// Token: 0x06009C6E RID: 40046 RVA: 0x003996B4 File Offset: 0x003978B4
		public void DoLand()
		{
			this.animController.Offset = Vector3.zero;
			OccupyArea component = base.smi.GetComponent<OccupyArea>();
			if (component != null)
			{
				component.ApplyToCells = true;
			}
			if (base.def.deployOnLanding && this.CheckIfLoaded())
			{
				base.sm.emptyCargo.Trigger(this);
			}
		}

		// Token: 0x06009C6F RID: 40047 RVA: 0x00399714 File Offset: 0x00397914
		public bool CheckIfLoaded()
		{
			bool flag = false;
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component != null)
			{
				flag |= (component.GetStoredMinionInfo().Count > 0);
			}
			Storage component2 = base.GetComponent<Storage>();
			if (component2 != null && !component2.IsEmpty())
			{
				flag = true;
			}
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x040078A0 RID: 30880
		[Serialize]
		public float flightAnimOffset = 50f;

		// Token: 0x040078A1 RID: 30881
		public KBatchedAnimController animController;
	}
}
