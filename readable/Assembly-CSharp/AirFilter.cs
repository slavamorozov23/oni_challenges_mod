using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020006CB RID: 1739
[SerializationConfig(MemberSerialization.OptIn)]
public class AirFilter : StateMachineComponent<AirFilter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06002A96 RID: 10902 RVA: 0x000F979F File Offset: 0x000F799F
	public bool HasFilter()
	{
		return this.elementConverter.HasEnoughMass(this.filterTag, false);
	}

	// Token: 0x06002A97 RID: 10903 RVA: 0x000F97B3 File Offset: 0x000F79B3
	public bool IsConvertable()
	{
		return this.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x000F97C1 File Offset: 0x000F79C1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x000F97D4 File Offset: 0x000F79D4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x04001955 RID: 6485
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001956 RID: 6486
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04001957 RID: 6487
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x04001958 RID: 6488
	[MyCmpGet]
	private ElementConsumer elementConsumer;

	// Token: 0x04001959 RID: 6489
	public Tag filterTag;

	// Token: 0x0200158D RID: 5517
	public class StatesInstance : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.GameInstance
	{
		// Token: 0x060093B5 RID: 37813 RVA: 0x00376796 File Offset: 0x00374996
		public StatesInstance(AirFilter smi) : base(smi)
		{
		}
	}

	// Token: 0x0200158E RID: 5518
	public class States : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter>
	{
		// Token: 0x060093B6 RID: 37814 RVA: 0x003767A0 File Offset: 0x003749A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.waiting;
			this.waiting.EventTransition(GameHashes.OnStorageChange, this.hasFilter, (AirFilter.StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.hasFilter, (AirFilter.StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational);
			this.hasFilter.EventTransition(GameHashes.OperationalChanged, this.waiting, (AirFilter.StatesInstance smi) => !smi.master.operational.IsOperational).Enter("EnableConsumption", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit("DisableConsumption", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			}).DefaultState(this.hasFilter.idle);
			this.hasFilter.idle.EventTransition(GameHashes.OnStorageChange, this.hasFilter.converting, (AirFilter.StatesInstance smi) => smi.master.IsConvertable());
			this.hasFilter.converting.Enter("SetActive(true)", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.hasFilter.idle, (AirFilter.StatesInstance smi) => !smi.master.IsConvertable());
		}

		// Token: 0x0400720E RID: 29198
		public AirFilter.States.ReadyStates hasFilter;

		// Token: 0x0400720F RID: 29199
		public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State waiting;

		// Token: 0x020028B8 RID: 10424
		public class ReadyStates : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State
		{
			// Token: 0x0400B348 RID: 45896
			public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State idle;

			// Token: 0x0400B349 RID: 45897
			public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State converting;
		}
	}
}
