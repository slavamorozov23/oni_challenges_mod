using System;
using System.Collections.Generic;
using FoodRehydrator;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020004A0 RID: 1184
public class EatChore : Chore<EatChore.StatesInstance>
{
	// Token: 0x06001915 RID: 6421 RVA: 0x0008BF34 File Offset: 0x0008A134
	public static IDiningSeat ResolveDiningSeat(GameObject messStation)
	{
		if (messStation == null)
		{
			global::Debug.LogWarning("messStation GameObject is null");
			return null;
		}
		IDiningSeat result;
		if (!messStation.TryGetComponent<IDiningSeat>(out result))
		{
			global::Debug.LogWarning("messStation GameObject has no IDiningSeat component");
			return null;
		}
		return result;
	}

	// Token: 0x06001916 RID: 6422 RVA: 0x0008BF70 File Offset: 0x0008A170
	private static KAnimFile ResolveEatAnim(IDiningSeat diningSeat, bool dinerIsBionic)
	{
		HashedString hashedString = (diningSeat != null) ? (dinerIsBionic ? diningSeat.ReloadElectrobankAnim : diningSeat.EatAnim) : MessStation.eatAnim;
		KAnimFile anim = Assets.GetAnim(hashedString);
		if (anim == null)
		{
			global::Debug.LogError(string.Format("Animation asset [{0}] does not exist", hashedString));
			return null;
		}
		return anim;
	}

	// Token: 0x06001917 RID: 6423 RVA: 0x0008BFC1 File Offset: 0x0008A1C1
	private static KAnimFile ResolveEatAnim(GameObject messStation, bool dinerIsBionic)
	{
		return EatChore.ResolveEatAnim(EatChore.ResolveDiningSeat(messStation), dinerIsBionic);
	}

	// Token: 0x06001918 RID: 6424 RVA: 0x0008BFD0 File Offset: 0x0008A1D0
	public EatChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.Eat, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new EatChore.StatesInstance(this);
		this.showAvailabilityInHoverText = false;
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(EatChore.EdibleIsNotNull, null);
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x0008C038 File Offset: 0x0008A238
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("EATCHORE null context.consumer");
			return;
		}
		RationMonitor.Instance smi = context.consumerState.consumer.GetSMI<RationMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("EATCHORE null RationMonitor.Instance");
			return;
		}
		Edible edible = smi.GetEdible();
		if (edible.gameObject == null)
		{
			global::Debug.LogError("EATCHORE null edible.gameObject");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("EATCHORE null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("EATCHORE null smi.sm");
			return;
		}
		if (base.smi.sm.ediblesource == null)
		{
			global::Debug.LogError("EATCHORE null smi.sm.ediblesource");
			return;
		}
		base.smi.sm.ediblesource.Set(edible.gameObject, base.smi, false);
		KCrashReporter.Assert(edible.FoodInfo.CaloriesPerUnit > 0f, edible.GetProperName() + " has invalid calories per unit. Will result in NaNs", null);
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(this.gameObject);
		float num = (amountInstance.GetMax() - amountInstance.value) / edible.FoodInfo.CaloriesPerUnit;
		KCrashReporter.Assert(num > 0f, "EatChore is requesting an invalid amount of food", null);
		base.smi.sm.requestedfoodunits.Set(num, base.smi, false);
		base.smi.sm.eater.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x0008C1CC File Offset: 0x0008A3CC
	public static bool IsMessStationNonOperational(GameObject messStation)
	{
		if (messStation == null)
		{
			return true;
		}
		IDiningSeat diningSeat = EatChore.ResolveDiningSeat(messStation);
		if (diningSeat == null)
		{
			return true;
		}
		Operational operational = diningSeat.FindOperational();
		return operational == null || !operational.IsOperational;
	}

	// Token: 0x0600191B RID: 6427 RVA: 0x0008C20B File Offset: 0x0008A40B
	private static bool IsMessStationNonOperational(EatChore.StatesInstance _, GameObject messStation)
	{
		return EatChore.IsMessStationNonOperational(messStation);
	}

	// Token: 0x04000EAB RID: 3755
	public static readonly Chore.Precondition EdibleIsNotNull = new Chore.Precondition
	{
		id = "EdibleIsNotNull",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return null != context.consumerState.consumer.GetSMI<RationMonitor.Instance>().GetEdible();
		}
	};

	// Token: 0x020012D0 RID: 4816
	public class StatesInstance : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.GameInstance
	{
		// Token: 0x060089B1 RID: 35249 RVA: 0x00353E4C File Offset: 0x0035204C
		public StatesInstance(EatChore master) : base(master)
		{
		}

		// Token: 0x060089B2 RID: 35250 RVA: 0x00353E58 File Offset: 0x00352058
		private static Assignable GetPreferredMessStation(GameObject diner)
		{
			Ownables soleOwner = diner.GetComponent<MinionIdentity>().GetSoleOwner();
			Navigator ownerNavigator;
			diner.TryGetComponent<Navigator>(out ownerNavigator);
			foreach (Assignable assignable in Game.Instance.assignmentManager.GetPreferredAssignables(soleOwner, ownerNavigator, Db.Get().AssignableSlots.MessStation))
			{
				IDiningSeat diningSeat = EatChore.ResolveDiningSeat(assignable.gameObject);
				if (diningSeat != null)
				{
					Operational operational = diningSeat.FindOperational();
					if ((!(operational != null) || operational.IsOperational) && assignable.GetComponent<Reservable>().IsReservableBy(diner))
					{
						return assignable;
					}
				}
			}
			return null;
		}

		// Token: 0x060089B3 RID: 35251 RVA: 0x00353F18 File Offset: 0x00352118
		public static Assignable ReserveMessStation(GameObject messStation, GameObject diner)
		{
			if (messStation != null)
			{
				messStation.GetComponent<Reservable>().ClearReservation();
			}
			Assignable preferredMessStation = EatChore.StatesInstance.GetPreferredMessStation(diner);
			if (preferredMessStation != null)
			{
				Reservable component = preferredMessStation.GetComponent<Reservable>();
				if (!component.Reserve(diner))
				{
					if (component.IsReservableBy(diner))
					{
						global::Debug.Log("Failed to reserve dining seat. We have already reserved it.");
					}
					else
					{
						global::Debug.LogWarning("Failed to reserve dining seat. Someone else has already reserved it!");
					}
				}
			}
			return preferredMessStation;
		}

		// Token: 0x060089B4 RID: 35252 RVA: 0x00353F7C File Offset: 0x0035217C
		public void UpdateMessStation()
		{
			Assignable value = EatChore.StatesInstance.ReserveMessStation(base.sm.messstation.Get(base.smi), base.sm.eater.Get(base.smi));
			base.sm.messstation.Set(value, base.smi);
		}

		// Token: 0x060089B5 RID: 35253 RVA: 0x00353FD4 File Offset: 0x003521D4
		public void ClearMessStation()
		{
			GameObject gameObject = base.smi.sm.messstation.Get(base.smi);
			if (gameObject != null)
			{
				gameObject.GetComponent<Reservable>().ClearReservation();
			}
			base.sm.messstation.Set(null, base.smi);
		}

		// Token: 0x060089B6 RID: 35254 RVA: 0x00354028 File Offset: 0x00352228
		public static bool UseSalt(GameObject messStation)
		{
			if (messStation == null)
			{
				return false;
			}
			IDiningSeat diningSeat = EatChore.ResolveDiningSeat(messStation);
			return diningSeat != null && diningSeat.HasSalt;
		}

		// Token: 0x060089B7 RID: 35255 RVA: 0x00354052 File Offset: 0x00352252
		public bool UseSalt()
		{
			return base.smi.sm.messstation != null && EatChore.StatesInstance.UseSalt(base.sm.messstation.Get(base.smi));
		}

		// Token: 0x060089B8 RID: 35256 RVA: 0x00354084 File Offset: 0x00352284
		public static ValueTuple<GameObject, int> CreateLocator(Sensors sensors, Transform transform, string locatorName)
		{
			int num = sensors.GetSensor<SafeCellSensor>().GetCellQuery();
			if (num == Grid.InvalidCell)
			{
				num = Grid.PosToCell(transform.GetPosition());
			}
			Vector3 pos = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
			Grid.Reserved[num] = true;
			return new ValueTuple<GameObject, int>(ChoreHelpers.CreateLocator(locatorName, pos), num);
		}

		// Token: 0x060089B9 RID: 35257 RVA: 0x003540D4 File Offset: 0x003522D4
		public void CreateLocator()
		{
			ValueTuple<GameObject, int> valueTuple = EatChore.StatesInstance.CreateLocator(base.sm.eater.Get<Sensors>(base.smi), base.sm.eater.Get<Transform>(base.smi), "EatLocator");
			GameObject item = valueTuple.Item1;
			this.locatorCell = valueTuple.Item2;
			base.sm.locator.Set(item, this, false);
		}

		// Token: 0x060089BA RID: 35258 RVA: 0x0035413F File Offset: 0x0035233F
		public void DestroyLocator()
		{
			Grid.Reserved[this.locatorCell] = false;
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x060089BB RID: 35259 RVA: 0x0035417C File Offset: 0x0035237C
		public static KAnimFile OnEnterMessStation(GameObject messStation, GameObject diner, GameObject food, bool dinerIsBionic, float? effectDurationOverride = null)
		{
			IDiningSeat diningSeat = EatChore.ResolveDiningSeat(messStation);
			if (diningSeat == null)
			{
				return null;
			}
			KAnimControllerBase component = diner.GetComponent<KAnimControllerBase>();
			KAnimFile kanimFile = EatChore.ResolveEatAnim(diningSeat, dinerIsBionic);
			component.AddAnimOverrides(kanimFile, 0f);
			Edible edible;
			if (food != null && food.TryGetComponent<Edible>(out edible))
			{
				edible.workLayer = Grid.SceneLayer.BuildingFront;
			}
			EffectInstance effectInstance = null;
			Effects component2 = diner.GetComponent<Effects>();
			Storage storage = diningSeat.FindStorage();
			if (storage != null && storage.Has(TableSaltConfig.TAG))
			{
				storage.ConsumeIgnoringDisease(TableSaltConfig.TAG, TableSaltTuning.CONSUMABLE_RATE);
				effectInstance = component2.Add("MessTableSalt", true);
			}
			diningSeat.Diner = diner.GetComponent<KPrefabID>();
			messStation.Trigger(1356255274, null);
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(messStation);
			KPrefabID component3 = messStation.GetComponent<KPrefabID>();
			if (effectDurationOverride != null)
			{
				List<EffectInstance> list = null;
				if (roomOfGameObject != null)
				{
					roomOfGameObject.roomType.TriggerRoomEffects(component3, component2, out list);
				}
				if (effectInstance != null)
				{
					if (list == null)
					{
						list = new List<EffectInstance>();
					}
					list.Add(effectInstance);
				}
				if (list == null)
				{
					return kanimFile;
				}
				using (List<EffectInstance>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EffectInstance effectInstance2 = enumerator.Current;
						effectInstance2.timeRemaining = effectDurationOverride.Value;
					}
					return kanimFile;
				}
			}
			if (roomOfGameObject != null)
			{
				roomOfGameObject.roomType.TriggerRoomEffects(component3, component2);
			}
			return kanimFile;
		}

		// Token: 0x060089BC RID: 35260 RVA: 0x003542DC File Offset: 0x003524DC
		public static void OnExitMessStation(GameObject messStation, GameObject diner, KAnimFile eatAnim)
		{
			diner.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(eatAnim);
			IDiningSeat diningSeat = EatChore.ResolveDiningSeat(messStation);
			if (diningSeat != null)
			{
				diningSeat.Diner = null;
			}
		}

		// Token: 0x04006935 RID: 26933
		private int locatorCell;

		// Token: 0x04006936 RID: 26934
		public KAnimFile eatAnim;
	}

	// Token: 0x020012D1 RID: 4817
	public class States : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore>
	{
		// Token: 0x060089BD RID: 35261 RVA: 0x00354308 File Offset: 0x00352508
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.chooseaction;
			base.Target(this.eater);
			this.root.Enter("SetMessStation", delegate(EatChore.StatesInstance smi)
			{
				smi.UpdateMessStation();
			}).EventHandler(GameHashes.AssignablesChanged, delegate(EatChore.StatesInstance smi)
			{
				smi.UpdateMessStation();
			}).Exit(delegate(EatChore.StatesInstance smi)
			{
				smi.ClearMessStation();
			});
			this.chooseaction.EnterTransition(this.rehydrate, (EatChore.StatesInstance smi) => this.ediblesource.Get(smi).HasTag(GameTags.Dehydrated)).EnterTransition(this.fetch, (EatChore.StatesInstance smi) => true);
			this.rehydrate.Enter(delegate(EatChore.StatesInstance smi)
			{
				DehydratedFoodPackage component = this.ediblesource.Get(smi).GetComponent<Pickupable>().storage.gameObject.GetComponent<DehydratedFoodPackage>();
				this.rehydrate.foodpackage.Set(component, smi);
				GameObject rehydrator = component.Rehydrator;
				this.rehydrate.rehydrator.Set((rehydrator != null) ? component.Rehydrator.GetComponent<AccessabilityManager>() : null, smi, false);
				AccessabilityManager accessabilityManager = this.rehydrate.rehydrator.Get(smi);
				if (!(accessabilityManager != null))
				{
					smi.GoTo(null);
					return;
				}
				GameObject worker = this.eater.Get(smi);
				if (accessabilityManager.CanAccess(worker))
				{
					accessabilityManager.Reserve(this.eater.Get(smi));
					return;
				}
				smi.GoTo(null);
			}).Exit(delegate(EatChore.StatesInstance smi)
			{
				AccessabilityManager accessabilityManager = this.rehydrate.rehydrator.Get(smi);
				if (accessabilityManager != null)
				{
					accessabilityManager.Unreserve();
				}
			}).DefaultState(this.rehydrate.approach);
			this.rehydrate.approach.InitializeStates(this.eater, this.rehydrate.foodpackage, this.rehydrate.work, null, null, NavigationTactics.ReduceTravelDistance).OnTargetLost(this.ediblesource, null);
			this.rehydrate.work.ToggleWork("Rehydrate", delegate(EatChore.StatesInstance smi)
			{
				WorkerBase workerBase = this.eater.Get<WorkerBase>(smi);
				DehydratedFoodPackage pkg = this.rehydrate.foodpackage.Get<DehydratedFoodPackage>(smi);
				workerBase.StartWork(new DehydratedFoodPackage.RehydrateStartWorkItem(pkg, delegate(GameObject result)
				{
					this.ediblechunk.Set(result, smi, false);
				}));
			}, delegate(EatChore.StatesInstance smi)
			{
				AccessabilityManager accessabilityManager = this.rehydrate.rehydrator.Get(smi);
				return !(accessabilityManager == null) && accessabilityManager.CanAccess(this.eater.Get<WorkerBase>(smi).gameObject);
			}, this.eatatmessstation, null);
			this.fetch.InitializeStates(this.eater, this.ediblesource, this.ediblechunk, this.requestedfoodunits, this.actualfoodunits, this.choosewheretoeat, null);
			this.choosewheretoeat.ParamTransition<GameObject>(this.messstation, this.eatonfloorstate, (EatChore.StatesInstance smi, GameObject p) => p == null || EatChore.IsMessStationNonOperational(p)).GoTo(this.eatatmessstation);
			this.eatatmessstation.DefaultState(this.eatatmessstation.moveto).ParamTransition<GameObject>(this.messstation, null, (EatChore.StatesInstance smi, GameObject p) => p == null || EatChore.IsMessStationNonOperational(p));
			this.eatatmessstation.moveto.InitializeStates(this.eater, this.messstation, this.eatatmessstation.eat, this.eatonfloorstate, null, null);
			this.eatatmessstation.eat.Enter("OnEnterMessStation", delegate(EatChore.StatesInstance smi)
			{
				smi.eatAnim = EatChore.StatesInstance.OnEnterMessStation(this.messstation.Get(smi), this.eater.Get(smi), this.ediblechunk.Get(smi), false, null);
			}).Transition(this.eatonfloorstate, (EatChore.StatesInstance smi) => smi.eatAnim == null, UpdateRate.SIM_200ms).DoEat(this.ediblechunk, this.actualfoodunits, null, null).Exit(delegate(EatChore.StatesInstance smi)
			{
				EatChore.StatesInstance.OnExitMessStation(this.messstation.Get(smi), this.eater.Get(smi), smi.eatAnim);
			});
			this.eatonfloorstate.DefaultState(this.eatonfloorstate.moveto).Enter("CreateLocator", delegate(EatChore.StatesInstance smi)
			{
				smi.CreateLocator();
			}).Exit("DestroyLocator", delegate(EatChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			});
			this.eatonfloorstate.moveto.InitializeStates(this.eater, this.locator, this.eatonfloorstate.eat, this.eatonfloorstate.eat, null, null);
			this.eatonfloorstate.eat.ToggleAnims("anim_eat_floor_kanim", 0f).DoEat(this.ediblechunk, this.actualfoodunits, null, null);
		}

		// Token: 0x04006937 RID: 26935
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter eater;

		// Token: 0x04006938 RID: 26936
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter ediblesource;

		// Token: 0x04006939 RID: 26937
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter ediblechunk;

		// Token: 0x0400693A RID: 26938
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter messstation;

		// Token: 0x0400693B RID: 26939
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FloatParameter requestedfoodunits;

		// Token: 0x0400693C RID: 26940
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FloatParameter actualfoodunits;

		// Token: 0x0400693D RID: 26941
		public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter locator;

		// Token: 0x0400693E RID: 26942
		public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State chooseaction;

		// Token: 0x0400693F RID: 26943
		public EatChore.States.RehydrateSubState rehydrate;

		// Token: 0x04006940 RID: 26944
		public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.FetchSubState fetch;

		// Token: 0x04006941 RID: 26945
		public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State choosewheretoeat;

		// Token: 0x04006942 RID: 26946
		public EatChore.States.EatOnFloorState eatonfloorstate;

		// Token: 0x04006943 RID: 26947
		public EatChore.States.EatAtMessStationState eatatmessstation;

		// Token: 0x020027A8 RID: 10152
		public class EatOnFloorState : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State
		{
			// Token: 0x0400AFD5 RID: 45013
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ApproachSubState<IApproachable> moveto;

			// Token: 0x0400AFD6 RID: 45014
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State eat;
		}

		// Token: 0x020027A9 RID: 10153
		public class EatAtMessStationState : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State
		{
			// Token: 0x0400AFD7 RID: 45015
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ApproachSubState<IApproachable> moveto;

			// Token: 0x0400AFD8 RID: 45016
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State eat;
		}

		// Token: 0x020027AA RID: 10154
		public class RehydrateSubState : GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State
		{
			// Token: 0x0400AFD9 RID: 45017
			public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.TargetParameter foodpackage;

			// Token: 0x0400AFDA RID: 45018
			public StateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ObjectParameter<AccessabilityManager> rehydrator;

			// Token: 0x0400AFDB RID: 45019
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.ApproachSubState<DehydratedFoodPackage> approach;

			// Token: 0x0400AFDC RID: 45020
			public GameStateMachine<EatChore.States, EatChore.StatesInstance, EatChore, object>.State work;
		}
	}
}
