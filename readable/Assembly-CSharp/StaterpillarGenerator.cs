using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000803 RID: 2051
public class StaterpillarGenerator : Generator
{
	// Token: 0x0600372B RID: 14123 RVA: 0x00136550 File Offset: 0x00134750
	protected override void OnSpawn()
	{
		Staterpillar staterpillar = this.parent.Get();
		if (staterpillar == null || staterpillar.GetGenerator() != this)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		this.smi = new StaterpillarGenerator.StatesInstance(this);
		this.smi.StartSM();
		base.OnSpawn();
	}

	// Token: 0x0600372C RID: 14124 RVA: 0x001365AC File Offset: 0x001347AC
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = base.GetComponent<Generator>().WattageRating;
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x04002184 RID: 8580
	private StaterpillarGenerator.StatesInstance smi;

	// Token: 0x04002185 RID: 8581
	[Serialize]
	public Ref<Staterpillar> parent = new Ref<Staterpillar>();

	// Token: 0x02001789 RID: 6025
	public class StatesInstance : GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.GameInstance
	{
		// Token: 0x06009B70 RID: 39792 RVA: 0x0039503F File Offset: 0x0039323F
		public StatesInstance(StaterpillarGenerator master) : base(master)
		{
		}

		// Token: 0x040077F4 RID: 30708
		private Attributes attributes;
	}

	// Token: 0x0200178A RID: 6026
	public class States : GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator>
	{
		// Token: 0x06009B71 RID: 39793 RVA: 0x00395048 File Offset: 0x00393248
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EventTransition(GameHashes.OperationalChanged, this.idle, (StaterpillarGenerator.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.idle.EventTransition(GameHashes.OperationalChanged, this.root, (StaterpillarGenerator.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).Enter(delegate(StaterpillarGenerator.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			});
		}

		// Token: 0x040077F5 RID: 30709
		public GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.State idle;
	}
}
