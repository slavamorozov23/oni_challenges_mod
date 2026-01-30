using System;

// Token: 0x02000059 RID: 89
public class LightController : GameStateMachine<LightController, LightController.Instance>
{
	// Token: 0x060001BB RID: 443 RVA: 0x0000C0A4 File Offset: 0x0000A2A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (LightController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (LightController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).Enter("SetActive", delegate(LightController.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		});
	}

	// Token: 0x0400010C RID: 268
	public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x0400010D RID: 269
	public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02001089 RID: 4233
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200108A RID: 4234
	public new class Instance : GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008252 RID: 33362 RVA: 0x00341CD3 File Offset: 0x0033FED3
		public Instance(IStateMachineTarget master, LightController.Def def) : base(master, def)
		{
		}
	}
}
