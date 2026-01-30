using System;
using UnityEngine;

// Token: 0x02000760 RID: 1888
public class FoodSmoker : GameStateMachine<FoodSmoker, FoodSmoker.StatesInstance, IStateMachineTarget, FoodSmoker.Def>
{
	// Token: 0x06002FC9 RID: 12233 RVA: 0x00114090 File Offset: 0x00112290
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.working;
		this.working.Enter(delegate(FoodSmoker.StatesInstance smi)
		{
			smi.complexFabricator.SetQueueDirty();
			smi.operational.SetFlag(FoodSmoker.foodSmokerFlag, true);
		}).EnterTransition(this.requestEmpty, (FoodSmoker.StatesInstance smi) => smi.RequiresEmptying()).EventHandlerTransition(GameHashes.FabricatorOrderCompleted, this.requestEmpty, (FoodSmoker.StatesInstance smi, object data) => smi.RequiresEmptying());
		this.requestEmpty.ToggleRecurringChore(new Func<FoodSmoker.StatesInstance, Chore>(this.CreateChore), new Action<FoodSmoker.StatesInstance, Chore>(FoodSmoker.SetRemoteChore), (FoodSmoker.StatesInstance smi) => smi.RequiresEmptying()).EventHandlerTransition(GameHashes.OnStorageChange, this.working, (FoodSmoker.StatesInstance smi, object data) => !smi.RequiresEmptying()).Enter(delegate(FoodSmoker.StatesInstance smi)
		{
			smi.operational.SetFlag(FoodSmoker.foodSmokerFlag, false);
		}).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding, null);
	}

	// Token: 0x06002FCA RID: 12234 RVA: 0x001141D4 File Offset: 0x001123D4
	private static void SetRemoteChore(FoodSmoker.StatesInstance smi, Chore chore)
	{
		smi.remoteChore.SetChore(chore);
	}

	// Token: 0x06002FCB RID: 12235 RVA: 0x001141E4 File Offset: 0x001123E4
	private Chore CreateChore(FoodSmoker.StatesInstance smi)
	{
		WorkChore<FoodSmokerWorkableEmpty> workChore = new WorkChore<FoodSmokerWorkableEmpty>(Db.Get().ChoreTypes.Cook, smi.master.GetComponent<FoodSmokerWorkableEmpty>(), null, true, new Action<Chore>(smi.OnEmptyComplete), null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanGasRange.Id);
		return workChore;
	}

	// Token: 0x04001C7B RID: 7291
	private static readonly Operational.Flag foodSmokerFlag = new Operational.Flag("food_smoker", Operational.Flag.Type.Requirement);

	// Token: 0x04001C7C RID: 7292
	private GameStateMachine<FoodSmoker, FoodSmoker.StatesInstance, IStateMachineTarget, FoodSmoker.Def>.State working;

	// Token: 0x04001C7D RID: 7293
	private GameStateMachine<FoodSmoker, FoodSmoker.StatesInstance, IStateMachineTarget, FoodSmoker.Def>.State requestEmpty;

	// Token: 0x0200164D RID: 5709
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200164E RID: 5710
	public class StatesInstance : GameStateMachine<FoodSmoker, FoodSmoker.StatesInstance, IStateMachineTarget, FoodSmoker.Def>.GameInstance
	{
		// Token: 0x060096BC RID: 38588 RVA: 0x00380E78 File Offset: 0x0037F078
		public StatesInstance(IStateMachineTarget master, FoodSmoker.Def def) : base(master, def)
		{
		}

		// Token: 0x060096BD RID: 38589 RVA: 0x00380E82 File Offset: 0x0037F082
		public bool RequiresEmptying()
		{
			return !this.complexFabricator.outStorage.IsEmpty();
		}

		// Token: 0x060096BE RID: 38590 RVA: 0x00380E98 File Offset: 0x0037F098
		public void OnEmptyComplete(Chore obj)
		{
			Vector3 position = Grid.CellToPosLCC(Grid.PosToCell(this), Grid.SceneLayer.Ore);
			this.complexFabricator.outStorage.DropAll(position, false, true, default(Vector3), true, null);
		}

		// Token: 0x04007467 RID: 29799
		[MyCmpAdd]
		public ManuallySetRemoteWorkTargetComponent remoteChore;

		// Token: 0x04007468 RID: 29800
		[MyCmpReq]
		public Operational operational;

		// Token: 0x04007469 RID: 29801
		[MyCmpReq]
		public ComplexFabricator complexFabricator;
	}
}
