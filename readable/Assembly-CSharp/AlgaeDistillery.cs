using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000705 RID: 1797
[SerializationConfig(MemberSerialization.OptIn)]
public class AlgaeDistillery : StateMachineComponent<AlgaeDistillery.StatesInstance>
{
	// Token: 0x06002C8D RID: 11405 RVA: 0x0010359A File Offset: 0x0010179A
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x04001A79 RID: 6777
	[SerializeField]
	public Tag emitTag;

	// Token: 0x04001A7A RID: 6778
	[SerializeField]
	public float emitMass;

	// Token: 0x04001A7B RID: 6779
	[SerializeField]
	public Vector3 emitOffset;

	// Token: 0x04001A7C RID: 6780
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001A7D RID: 6781
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x04001A7E RID: 6782
	[MyCmpReq]
	private Operational operational;

	// Token: 0x020015C7 RID: 5575
	public class StatesInstance : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.GameInstance
	{
		// Token: 0x06009494 RID: 38036 RVA: 0x003795C0 File Offset: 0x003777C0
		public StatesInstance(AlgaeDistillery smi) : base(smi)
		{
		}

		// Token: 0x06009495 RID: 38037 RVA: 0x003795CC File Offset: 0x003777CC
		public void TryEmit()
		{
			Storage storage = base.smi.master.storage;
			GameObject gameObject = storage.FindFirst(base.smi.master.emitTag);
			if (gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass >= base.master.emitMass)
			{
				storage.Drop(gameObject, true).transform.SetPosition(base.transform.GetPosition() + base.master.emitOffset);
			}
		}
	}

	// Token: 0x020015C8 RID: 5576
	public class States : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery>
	{
		// Token: 0x06009496 RID: 38038 RVA: 0x00379650 File Offset: 0x00377850
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (AlgaeDistillery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (AlgaeDistillery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (AlgaeDistillery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (AlgaeDistillery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.TryEmit();
			});
		}

		// Token: 0x040072AC RID: 29356
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State disabled;

		// Token: 0x040072AD RID: 29357
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State waiting;

		// Token: 0x040072AE RID: 29358
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State converting;

		// Token: 0x040072AF RID: 29359
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State overpressure;
	}
}
