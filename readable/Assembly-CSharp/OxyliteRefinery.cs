using System;
using KSerialization;
using UnityEngine;

// Token: 0x020007D6 RID: 2006
[SerializationConfig(MemberSerialization.OptIn)]
public class OxyliteRefinery : StateMachineComponent<OxyliteRefinery.StatesInstance>
{
	// Token: 0x0600354E RID: 13646 RVA: 0x0012D40C File Offset: 0x0012B60C
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0400203D RID: 8253
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x0400203E RID: 8254
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400203F RID: 8255
	public Tag emitTag;

	// Token: 0x04002040 RID: 8256
	public float emitMass;

	// Token: 0x04002041 RID: 8257
	public Vector3 dropOffset;

	// Token: 0x0200172D RID: 5933
	public class StatesInstance : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.GameInstance
	{
		// Token: 0x06009A40 RID: 39488 RVA: 0x00390549 File Offset: 0x0038E749
		public StatesInstance(OxyliteRefinery smi) : base(smi)
		{
		}

		// Token: 0x06009A41 RID: 39489 RVA: 0x00390554 File Offset: 0x0038E754
		public void TryEmit()
		{
			Storage storage = base.smi.master.storage;
			GameObject gameObject = storage.FindFirst(base.smi.master.emitTag);
			if (gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass >= base.master.emitMass)
			{
				Vector3 position = base.transform.GetPosition() + base.master.dropOffset;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				gameObject.transform.SetPosition(position);
				storage.Drop(gameObject, true);
			}
		}
	}

	// Token: 0x0200172E RID: 5934
	public class States : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery>
	{
		// Token: 0x06009A42 RID: 39490 RVA: 0x003905EC File Offset: 0x0038E7EC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (OxyliteRefinery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (OxyliteRefinery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.EventTransition(GameHashes.OnStorageChange, this.converting, (OxyliteRefinery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter(delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Transition(this.waiting, (OxyliteRefinery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.TryEmit();
			});
		}

		// Token: 0x04007709 RID: 30473
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State disabled;

		// Token: 0x0400770A RID: 30474
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State waiting;

		// Token: 0x0400770B RID: 30475
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State converting;
	}
}
