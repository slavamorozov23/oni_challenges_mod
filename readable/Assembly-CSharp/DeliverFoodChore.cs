using System;

// Token: 0x0200049E RID: 1182
public class DeliverFoodChore : Chore<DeliverFoodChore.StatesInstance>
{
	// Token: 0x06001912 RID: 6418 RVA: 0x0008BDCC File Offset: 0x00089FCC
	public DeliverFoodChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.DeliverFood, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new DeliverFoodChore.StatesInstance(this);
		this.AddPrecondition(ChorePreconditions.instance.IsChattable, target);
	}

	// Token: 0x06001913 RID: 6419 RVA: 0x0008BE20 File Offset: 0x0008A020
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.requestedrationcount.Set(base.smi.GetComponent<StateMachineController>().GetSMI<RationMonitor.Instance>().GetRationsRemaining(), base.smi, false);
		base.smi.sm.ediblesource.Set(context.consumerState.gameObject.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().GetEdible(), base.smi);
		base.smi.sm.deliverypoint.Set(this.gameObject, base.smi, false);
		base.smi.sm.deliverer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x020012CC RID: 4812
	public class StatesInstance : GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.GameInstance
	{
		// Token: 0x060089AA RID: 35242 RVA: 0x00353CD0 File Offset: 0x00351ED0
		public StatesInstance(DeliverFoodChore master) : base(master)
		{
		}
	}

	// Token: 0x020012CD RID: 4813
	public class States : GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore>
	{
		// Token: 0x060089AB RID: 35243 RVA: 0x00353CDC File Offset: 0x00351EDC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			this.fetch.InitializeStates(this.deliverer, this.ediblesource, this.ediblechunk, this.requestedrationcount, this.actualrationcount, this.movetodeliverypoint, null);
			this.movetodeliverypoint.InitializeStates(this.deliverer, this.deliverypoint, this.drop, null, null, null);
			this.drop.InitializeStates(this.deliverer, this.ediblechunk, this.deliverypoint, this.success, null);
			this.success.ReturnSuccess();
		}

		// Token: 0x04006928 RID: 26920
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter deliverer;

		// Token: 0x04006929 RID: 26921
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter ediblesource;

		// Token: 0x0400692A RID: 26922
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter ediblechunk;

		// Token: 0x0400692B RID: 26923
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter deliverypoint;

		// Token: 0x0400692C RID: 26924
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FloatParameter requestedrationcount;

		// Token: 0x0400692D RID: 26925
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FloatParameter actualrationcount;

		// Token: 0x0400692E RID: 26926
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FetchSubState fetch;

		// Token: 0x0400692F RID: 26927
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.ApproachSubState<Chattable> movetodeliverypoint;

		// Token: 0x04006930 RID: 26928
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.DropSubState drop;

		// Token: 0x04006931 RID: 26929
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.State success;
	}
}
