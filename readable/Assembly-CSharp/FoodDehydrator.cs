using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200075D RID: 1885
public class FoodDehydrator : GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>
{
	// Token: 0x06002FC0 RID: 12224 RVA: 0x00113A5C File Offset: 0x00111C5C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		this.waitingForFuelStatus.resolveStringCallback = ((string str, object obj) => string.Format(str, FOODDEHYDRATORTUNING.FUEL_TAG.ProperName(), GameUtil.GetFormattedMass(5.0000005f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")));
		default_state = this.waitingForFuel;
		this.waitingForFuel.Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, false);
		}).EventTransition(GameHashes.OnStorageChange, this.working, (FoodDehydrator.StatesInstance smi) => smi.GetAvailableFuel() >= 5.0000005f).ToggleStatusItem(this.waitingForFuelStatus, null);
		this.working.Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.complexFabricator.SetQueueDirty();
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, true);
		}).EventHandler(GameHashes.FabricatorOrdersUpdated, delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.UpdateFoodSymbol();
		}).EnterTransition(this.requestEmpty, (FoodDehydrator.StatesInstance smi) => smi.RequiresEmptying()).EventTransition(GameHashes.OnStorageChange, this.waitingForFuel, (FoodDehydrator.StatesInstance smi) => smi.GetAvailableFuel() <= 0f).EventHandlerTransition(GameHashes.FabricatorOrderCompleted, this.requestEmpty, (FoodDehydrator.StatesInstance smi, object data) => smi.RequiresEmptying()).EventHandler(GameHashes.FabricatorOrderStarted, delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.UpdateFoodSymbol();
		});
		this.requestEmpty.ToggleRecurringChore(new Func<FoodDehydrator.StatesInstance, Chore>(this.CreateChore), (FoodDehydrator.StatesInstance smi) => smi.RequiresEmptying()).EventHandlerTransition(GameHashes.OnStorageChange, this.working, (FoodDehydrator.StatesInstance smi, object data) => !smi.RequiresEmptying()).Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, false);
			smi.UpdateFoodSymbol();
		}).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding, null);
	}

	// Token: 0x06002FC1 RID: 12225 RVA: 0x00113CA8 File Offset: 0x00111EA8
	private Chore CreateChore(FoodDehydrator.StatesInstance smi)
	{
		WorkChore<FoodDehydratorWorkableEmpty> workChore = new WorkChore<FoodDehydratorWorkableEmpty>(Db.Get().ChoreTypes.FoodFetch, smi.master.GetComponent<FoodDehydratorWorkableEmpty>(), null, true, new Action<Chore>(smi.OnEmptyComplete), null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		return workChore;
	}

	// Token: 0x04001C6F RID: 7279
	private StatusItem waitingForFuelStatus = new StatusItem("waitingForFuelStatus", BUILDING.STATUSITEMS.ENOUGH_FUEL.NAME, BUILDING.STATUSITEMS.ENOUGH_FUEL.TOOLTIP, "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);

	// Token: 0x04001C70 RID: 7280
	private static readonly Operational.Flag foodDehydratorFlag = new Operational.Flag("food_dehydrator", Operational.Flag.Type.Requirement);

	// Token: 0x04001C71 RID: 7281
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State waitingForFuel;

	// Token: 0x04001C72 RID: 7282
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State working;

	// Token: 0x04001C73 RID: 7283
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State requestEmpty;

	// Token: 0x02001647 RID: 5703
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009694 RID: 38548 RVA: 0x00380A08 File Offset: 0x0037EC08
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			Descriptor item = new Descriptor(UI.BUILDINGEFFECTS.FOOD_DEHYDRATOR_WATER_OUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.FOOD_DEHYDRATOR_WATER_OUTPUT, Descriptor.DescriptorType.Effect, false);
			list.Add(item);
			return list;
		}
	}

	// Token: 0x02001648 RID: 5704
	public class StatesInstance : GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.GameInstance
	{
		// Token: 0x06009696 RID: 38550 RVA: 0x00380A46 File Offset: 0x0037EC46
		public StatesInstance(IStateMachineTarget master, FoodDehydrator.Def def) : base(master, def)
		{
			this.SetupFoodSymbol();
		}

		// Token: 0x06009697 RID: 38551 RVA: 0x00380A56 File Offset: 0x0037EC56
		public float GetAvailableFuel()
		{
			return this.complexFabricator.inStorage.GetMassAvailable(FOODDEHYDRATORTUNING.FUEL_TAG);
		}

		// Token: 0x06009698 RID: 38552 RVA: 0x00380A6D File Offset: 0x0037EC6D
		public bool RequiresEmptying()
		{
			return !this.complexFabricator.outStorage.IsEmpty();
		}

		// Token: 0x06009699 RID: 38553 RVA: 0x00380A84 File Offset: 0x0037EC84
		public void OnEmptyComplete(Chore obj)
		{
			Vector3 position = Grid.CellToPosLCC(Grid.PosToCell(this), Grid.SceneLayer.Ore);
			this.complexFabricator.outStorage.DropAll(position, false, true, default(Vector3), true, null);
		}

		// Token: 0x0600969A RID: 38554 RVA: 0x00380AC0 File Offset: 0x0037ECC0
		public void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "food_symbol");
			gameObject.SetActive(false);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			bool flag;
			Vector3 position = component.GetSymbolTransform(FoodDehydrator.StatesInstance.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			component.SetSymbolVisiblity(FoodDehydrator.StatesInstance.HASH_FOOD, false);
			this.foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
			KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = new HashedString("food");
			kbatchedAnimTracker.offset = Vector3.zero;
		}

		// Token: 0x0600969B RID: 38555 RVA: 0x00380BA8 File Offset: 0x0037EDA8
		public void UpdateFoodSymbol()
		{
			ComplexRecipe currentWorkingOrder = this.complexFabricator.CurrentWorkingOrder;
			if (this.complexFabricator.CurrentWorkingOrder != null)
			{
				this.foodKBAC.gameObject.SetActive(true);
				GameObject prefab = Assets.GetPrefab(currentWorkingOrder.ingredients[this.foodIngredientIdx].material);
				this.foodKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			if (this.complexFabricator.outStorage.items.Count > 0)
			{
				this.foodKBAC.gameObject.SetActive(true);
				this.foodKBAC.SwapAnims(this.complexFabricator.outStorage.items[0].GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			this.foodKBAC.gameObject.SetActive(false);
		}

		// Token: 0x04007447 RID: 29767
		[MyCmpReq]
		public Operational operational;

		// Token: 0x04007448 RID: 29768
		[MyCmpReq]
		public ComplexFabricator complexFabricator;

		// Token: 0x04007449 RID: 29769
		private static string HASH_FOOD = "food";

		// Token: 0x0400744A RID: 29770
		private KBatchedAnimController foodKBAC;

		// Token: 0x0400744B RID: 29771
		private int foodIngredientIdx;
	}
}
