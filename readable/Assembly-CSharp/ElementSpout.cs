using System;
using UnityEngine;

// Token: 0x0200096E RID: 2414
public class ElementSpout : StateMachineComponent<ElementSpout.StatesInstance>
{
	// Token: 0x060044AC RID: 17580 RVA: 0x0018BF00 File Offset: 0x0018A100
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Grid.Objects[cell, 2] = base.gameObject;
		base.smi.StartSM();
	}

	// Token: 0x060044AD RID: 17581 RVA: 0x0018BF41 File Offset: 0x0018A141
	public void SetEmitter(ElementEmitter emitter)
	{
		this.emitter = emitter;
	}

	// Token: 0x060044AE RID: 17582 RVA: 0x0018BF4A File Offset: 0x0018A14A
	public void ConfigureEmissionSettings(float emissionPollFrequency = 3f, float emissionIrregularity = 1.5f, float maxPressure = 1.5f, float perEmitAmount = 0.5f)
	{
		this.maxPressure = maxPressure;
		this.emissionPollFrequency = emissionPollFrequency;
		this.emissionIrregularity = emissionIrregularity;
		this.perEmitAmount = perEmitAmount;
	}

	// Token: 0x04002E1B RID: 11803
	[SerializeField]
	private ElementEmitter emitter;

	// Token: 0x04002E1C RID: 11804
	[MyCmpAdd]
	private KBatchedAnimController anim;

	// Token: 0x04002E1D RID: 11805
	public float maxPressure = 1.5f;

	// Token: 0x04002E1E RID: 11806
	public float emissionPollFrequency = 3f;

	// Token: 0x04002E1F RID: 11807
	public float emissionIrregularity = 1.5f;

	// Token: 0x04002E20 RID: 11808
	public float perEmitAmount = 0.5f;

	// Token: 0x020019A1 RID: 6561
	public class StatesInstance : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.GameInstance
	{
		// Token: 0x0600A2AC RID: 41644 RVA: 0x003AFC13 File Offset: 0x003ADE13
		public StatesInstance(ElementSpout smi) : base(smi)
		{
		}

		// Token: 0x0600A2AD RID: 41645 RVA: 0x003AFC1C File Offset: 0x003ADE1C
		private bool CanEmitOnCell(int cell, float max_pressure, Element.State expected_state)
		{
			return Grid.Mass[cell] < max_pressure && (Grid.Element[cell].IsState(expected_state) || Grid.Element[cell].IsVacuum);
		}

		// Token: 0x0600A2AE RID: 41646 RVA: 0x003AFC4C File Offset: 0x003ADE4C
		public bool CanEmitAnywhere()
		{
			int cell = Grid.PosToCell(base.smi.transform.GetPosition());
			int cell2 = Grid.CellLeft(cell);
			int cell3 = Grid.CellRight(cell);
			int cell4 = Grid.CellAbove(cell);
			Element.State state = ElementLoader.FindElementByHash(base.smi.master.emitter.outputElement.elementHash).state;
			return false || this.CanEmitOnCell(cell, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell2, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell3, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell4, base.smi.master.maxPressure, state);
		}
	}

	// Token: 0x020019A2 RID: 6562
	public class States : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout>
	{
		// Token: 0x0600A2AF RID: 41647 RVA: 0x003AFD24 File Offset: 0x003ADF24
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.DefaultState(this.idle.unblocked).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("idle", KAnim.PlayMode.Once);
			}).ScheduleGoTo((ElementSpout.StatesInstance smi) => smi.master.emissionPollFrequency, this.emit);
			this.idle.unblocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutPressureBuilding, null).Transition(this.idle.blocked, (ElementSpout.StatesInstance smi) => !smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.idle.blocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure, null).Transition(this.idle.blocked, (ElementSpout.StatesInstance smi) => smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.emit.DefaultState(this.emit.unblocked).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				float num = 1f + UnityEngine.Random.Range(0f, smi.master.emissionIrregularity);
				float massGenerationRate = smi.master.perEmitAmount / num;
				smi.master.emitter.SetEmitting(true);
				smi.master.emitter.emissionFrequency = 1f;
				smi.master.emitter.outputElement.massGenerationRate = massGenerationRate;
				smi.ScheduleGoTo(num, this.idle);
			});
			this.emit.unblocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutEmitting, null).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("emit", KAnim.PlayMode.Once);
				smi.master.emitter.SetEmitting(true);
			}).Transition(this.emit.blocked, (ElementSpout.StatesInstance smi) => !smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.emit.blocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure, null).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("idle", KAnim.PlayMode.Once);
				smi.master.emitter.SetEmitting(false);
			}).Transition(this.emit.unblocked, (ElementSpout.StatesInstance smi) => smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04007ECA RID: 32458
		public ElementSpout.States.Idle idle;

		// Token: 0x04007ECB RID: 32459
		public ElementSpout.States.Emitting emit;

		// Token: 0x020029B4 RID: 10676
		public class Idle : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State
		{
			// Token: 0x0400B868 RID: 47208
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State unblocked;

			// Token: 0x0400B869 RID: 47209
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State blocked;
		}

		// Token: 0x020029B5 RID: 10677
		public class Emitting : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State
		{
			// Token: 0x0400B86A RID: 47210
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State unblocked;

			// Token: 0x0400B86B RID: 47211
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State blocked;
		}
	}
}
