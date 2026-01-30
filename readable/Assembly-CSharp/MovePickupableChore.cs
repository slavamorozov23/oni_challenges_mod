using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020004AF RID: 1199
public class MovePickupableChore : Chore<MovePickupableChore.StatesInstance>
{
	// Token: 0x06001973 RID: 6515 RVA: 0x0008E218 File Offset: 0x0008C418
	public MovePickupableChore(IStateMachineTarget target, GameObject pickupable, Action<Chore> onEnd) : base((!Movable.IsCritterPickupable(pickupable)) ? Db.Get().ChoreTypes.Fetch : Db.Get().ChoreTypes.Ranch, target, target.GetComponent<ChoreProvider>(), false, null, null, onEnd, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MovePickupableChore.StatesInstance(this);
		Pickupable component = pickupable.GetComponent<Pickupable>();
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, target.GetComponent<Storage>());
		this.AddPrecondition(ChorePreconditions.instance.IsNotARobot, "FetchDrone");
		this.AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, this);
		if (Movable.IsCritterPickupable(pickupable))
		{
			this.AddPrecondition(MovePickupableChore.CanReachCritter, pickupable);
			this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanWrangleCreatures);
			IApproachable approachable = pickupable.GetComponent<IApproachable>();
			this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCell, new Func<int>(() => approachable.GetCell()));
		}
		else
		{
			this.AddPrecondition(ChorePreconditions.instance.CanPickup, component);
		}
		PrimaryElement primaryElement = component.PrimaryElement;
		base.smi.sm.requestedamount.Set(primaryElement.Mass, base.smi, false);
		base.smi.sm.pickupablesource.Set(pickupable.gameObject, base.smi, false);
		base.smi.sm.deliverypoint.Set(target.gameObject, base.smi, false);
		this.movePlacer = target.gameObject;
		this.OnReachableChanged(BoxedBools.Box(MinionGroupProber.Get().IsReachable(Grid.PosToCell(pickupable), OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(Grid.PosToCell(target.gameObject), OffsetGroups.Standard)));
		this.pickupableOnReachableChangedHandlerID = pickupable.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		this.targetOnReachableChangedHandlerID = target.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		Prioritizable component2 = target.GetComponent<Prioritizable>();
		if (!component2.IsPrioritizable())
		{
			component2.AddRef();
		}
		base.SetPrioritizable(target.GetComponent<Prioritizable>());
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x0008E440 File Offset: 0x0008C640
	public override void Cleanup()
	{
		base.Cleanup();
		if (this.target != null)
		{
			this.target.Unsubscribe(this.targetOnReachableChangedHandlerID);
		}
		GameObject gameObject = base.smi.sm.pickupablesource.Get(base.smi);
		if (gameObject != null)
		{
			gameObject.Unsubscribe(this.pickupableOnReachableChangedHandlerID);
		}
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x0008E4A0 File Offset: 0x0008C6A0
	private void OnReachableChanged(object data)
	{
		Color color = ((Boxed<bool>)data).value ? Color.white : new Color(0.91f, 0.21f, 0.2f);
		this.SetColor(this.movePlacer, color);
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x0008E4E3 File Offset: 0x0008C6E3
	private void SetColor(GameObject visualizer, Color color)
	{
		if (visualizer != null)
		{
			visualizer.GetComponentInChildren<MeshRenderer>().material.color = color;
		}
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x0008E500 File Offset: 0x0008C700
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("MovePickupable null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("MovePickupable null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("MovePickupable null smi.sm");
			return;
		}
		if (base.smi.sm.pickupablesource == null)
		{
			global::Debug.LogError("MovePickupable null smi.sm.pickupablesource");
			return;
		}
		base.smi.sm.deliverer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x04000ED0 RID: 3792
	private int pickupableOnReachableChangedHandlerID;

	// Token: 0x04000ED1 RID: 3793
	private int targetOnReachableChangedHandlerID;

	// Token: 0x04000ED2 RID: 3794
	public GameObject movePlacer;

	// Token: 0x04000ED3 RID: 3795
	public static Chore.Precondition CanReachCritter = new Chore.Precondition
	{
		id = "CanReachCritter",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			GameObject gameObject = (GameObject)data;
			return !(gameObject == null) && gameObject.HasTag(GameTags.Reachable);
		}
	};

	// Token: 0x020012F5 RID: 4853
	public class StatesInstance : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.GameInstance
	{
		// Token: 0x06008A3A RID: 35386 RVA: 0x00357FD3 File Offset: 0x003561D3
		public StatesInstance(MovePickupableChore master) : base(master)
		{
		}
	}

	// Token: 0x020012F6 RID: 4854
	public class States : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore>
	{
		// Token: 0x06008A3B RID: 35387 RVA: 0x00357FDC File Offset: 0x003561DC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.deliverypoint);
			this.fetch.Target(this.deliverer).DefaultState(this.fetch.approach).Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				this.pickupablesource.Get<Pickupable>(smi).ClearReservations();
			}).ToggleReserve(this.deliverer, this.pickupablesource, this.requestedamount, this.actualamount).EnterTransition(this.fetch.approachCritter, (MovePickupableChore.StatesInstance smi) => this.IsCritter(smi)).OnTargetLost(this.pickupablesource, null);
			this.fetch.approachCritter.Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				GameObject gameObject = this.pickupablesource.Get(smi);
				if (!gameObject.HasTag(GameTags.Creatures.Bagged))
				{
					IdleStates.Instance smi2 = gameObject.GetSMI<IdleStates.Instance>();
					if (!smi2.IsNullOrStopped())
					{
						smi2.GoTo(smi2.sm.root);
					}
					FlopStates.Instance smi3 = gameObject.GetSMI<FlopStates.Instance>();
					if (!smi3.IsNullOrStopped())
					{
						smi3.GoTo(smi3.sm.root);
					}
					gameObject.GetComponent<Navigator>().Stop(false, true);
				}
			}).MoveTo<Capturable>(this.pickupablesource, this.fetch.wrangle, new Func<MovePickupableChore.StatesInstance, NavTactic>(this.GetNavTactic), null, null);
			this.fetch.wrangle.EnterTransition(this.fetch.approach, (MovePickupableChore.StatesInstance smi) => this.pickupablesource.Get(smi).HasTag(GameTags.Creatures.Bagged)).ToggleWork<Capturable>(this.pickupablesource, this.fetch.approach, null, null);
			this.fetch.approach.MoveTo<IApproachable>(this.pickupablesource, this.fetch.pickup, new Func<MovePickupableChore.StatesInstance, NavTactic>(this.GetNavTactic), null, null);
			this.fetch.pickup.DoPickup(this.pickupablesource, this.pickup, this.actualamount, this.approachstorage, this.delivering.deliverfail).Exit(delegate(MovePickupableChore.StatesInstance smi)
			{
				GameObject gameObject = this.pickup.Get(smi);
				Movable movable = (gameObject != null) ? gameObject.GetComponent<Movable>() : null;
				if (movable != null && movable.onPickupComplete != null)
				{
					movable.onPickupComplete(gameObject);
				}
			});
			this.approachstorage.DefaultState(this.approachstorage.deliveryStorage);
			this.approachstorage.deliveryStorage.InitializeStates(new Func<MovePickupableChore.StatesInstance, NavTactic>(this.GetNavTactic), this.deliverer, this.deliverypoint, this.delivering.storing, this.delivering.deliverfail, null).Target(this.deliverer).EventHandler(GameHashes.OnStorageChange, delegate(MovePickupableChore.StatesInstance smi, object data)
			{
				GameObject x = data as GameObject;
				if (x != null)
				{
					GameObject gameObject = this.pickup.Get(smi);
					if (gameObject == null || x == gameObject)
					{
						smi.GoTo(this.delivering.deliverfail);
					}
				}
			});
			this.delivering.storing.Target(this.deliverer).DoDelivery(this.deliverer, this.deliverypoint, this.success, this.delivering.deliverfail);
			this.delivering.deliverfail.ReturnFailure();
			this.success.Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				Storage component = this.deliverypoint.Get(smi).GetComponent<Storage>();
				Storage component2 = this.deliverer.Get(smi).GetComponent<Storage>();
				float num = this.actualamount.Get(smi);
				GameObject gameObject = this.pickup.Get(smi);
				num += gameObject.GetComponent<PrimaryElement>().Mass;
				this.actualamount.Set(num, smi, false);
				component2.Transfer(this.pickup.Get(smi), component, false, false);
				this.DropPickupable(component, gameObject);
				CancellableMove component3 = component.GetComponent<CancellableMove>();
				Movable component4 = gameObject.GetComponent<Movable>();
				component3.RemoveMovable(component4);
				component4.ClearMove();
				if (!this.IsDeliveryComplete(smi))
				{
					GameObject go = this.pickupablesource.Get(smi);
					int num2 = Grid.PosToCell(this.deliverypoint.Get(smi));
					if (this.pickupablesource.Get(smi) == null || Grid.PosToCell(go) == num2)
					{
						GameObject nextTarget = component3.GetNextTarget();
						this.pickupablesource.Set(nextTarget, smi, false);
						PrimaryElement component5 = nextTarget.GetComponent<PrimaryElement>();
						smi.sm.requestedamount.Set(component5.Mass, smi, false);
					}
					smi.GoTo(this.fetch);
				}
			}).ReturnSuccess();
		}

		// Token: 0x06008A3C RID: 35388 RVA: 0x0035824C File Offset: 0x0035644C
		private NavTactic GetNavTactic(MovePickupableChore.StatesInstance smi)
		{
			WorkerBase component = this.deliverer.Get(smi).GetComponent<WorkerBase>();
			if (component != null && component.IsFetchDrone())
			{
				return NavigationTactics.FetchDronePickup;
			}
			return NavigationTactics.ReduceTravelDistance;
		}

		// Token: 0x06008A3D RID: 35389 RVA: 0x00358288 File Offset: 0x00356488
		private void DropPickupable(Storage storage, GameObject delivered)
		{
			if (delivered.GetComponent<Capturable>() != null)
			{
				List<GameObject> items = storage.items;
				int count = items.Count;
				Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(storage), Grid.SceneLayer.Creatures);
				for (int i = count - 1; i >= 0; i--)
				{
					GameObject gameObject = items[i];
					storage.Drop(gameObject, true);
					gameObject.transform.SetPosition(position);
					gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
				}
			}
			else
			{
				storage.DropAll(false, false, default(Vector3), true, null);
			}
			Movable component = delivered.GetComponent<Movable>();
			if (component.onDeliveryComplete != null)
			{
				component.onDeliveryComplete(delivered);
			}
		}

		// Token: 0x06008A3E RID: 35390 RVA: 0x0035832C File Offset: 0x0035652C
		private bool IsDeliveryComplete(MovePickupableChore.StatesInstance smi)
		{
			GameObject gameObject = smi.sm.deliverypoint.Get(smi);
			return !(gameObject != null) || gameObject.GetComponent<CancellableMove>().IsDeliveryComplete();
		}

		// Token: 0x06008A3F RID: 35391 RVA: 0x00358364 File Offset: 0x00356564
		private bool IsCritter(MovePickupableChore.StatesInstance smi)
		{
			GameObject gameObject = this.pickupablesource.Get(smi);
			return gameObject != null && gameObject.GetComponent<Capturable>() != null;
		}

		// Token: 0x040069C2 RID: 27074
		public static CellOffset[] critterCellOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};

		// Token: 0x040069C3 RID: 27075
		public static HashedString[] critterReleaseWorkAnims = new HashedString[]
		{
			"place",
			"release"
		};

		// Token: 0x040069C4 RID: 27076
		public static KAnimFile[] critterReleaseAnim = new KAnimFile[]
		{
			Assets.GetAnim("anim_restrain_creature_kanim")
		};

		// Token: 0x040069C5 RID: 27077
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter deliverer;

		// Token: 0x040069C6 RID: 27078
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter pickupablesource;

		// Token: 0x040069C7 RID: 27079
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter pickup;

		// Token: 0x040069C8 RID: 27080
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter deliverypoint;

		// Token: 0x040069C9 RID: 27081
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.FloatParameter requestedamount;

		// Token: 0x040069CA RID: 27082
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.FloatParameter actualamount;

		// Token: 0x040069CB RID: 27083
		public MovePickupableChore.States.FetchState fetch;

		// Token: 0x040069CC RID: 27084
		public MovePickupableChore.States.ApproachStorage approachstorage;

		// Token: 0x040069CD RID: 27085
		public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State success;

		// Token: 0x040069CE RID: 27086
		public MovePickupableChore.States.DeliveryState delivering;

		// Token: 0x020027C3 RID: 10179
		public class ApproachStorage : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x0400B053 RID: 45139
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Storage> deliveryStorage;

			// Token: 0x0400B054 RID: 45140
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Storage> unbagCritter;
		}

		// Token: 0x020027C4 RID: 10180
		public class DeliveryState : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x0400B055 RID: 45141
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State storing;

			// Token: 0x0400B056 RID: 45142
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State deliverfail;
		}

		// Token: 0x020027C5 RID: 10181
		public class FetchState : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x0400B057 RID: 45143
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Pickupable> approach;

			// Token: 0x0400B058 RID: 45144
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State pickup;

			// Token: 0x0400B059 RID: 45145
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State approachCritter;

			// Token: 0x0400B05A RID: 45146
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State wrangle;
		}
	}
}
