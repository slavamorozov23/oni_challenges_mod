using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000778 RID: 1912
public class HeatCompressor : StateMachineComponent<HeatCompressor.StatesInstance>
{
	// Token: 0x060030A2 RID: 12450 RVA: 0x00118D24 File Offset: 0x00116F24
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("HeatCube"), base.transform.GetPosition());
		gameObject.SetActive(true);
		this.heatCubeStorage.Store(gameObject, true, false, true, false);
		base.smi.StartSM();
	}

	// Token: 0x060030A3 RID: 12451 RVA: 0x00118DD3 File Offset: 0x00116FD3
	public void SetStorage(Storage inputStorage, Storage outputStorage, Storage heatCubeStorage)
	{
		this.inputStorage = inputStorage;
		this.outputStorage = outputStorage;
		this.heatCubeStorage = heatCubeStorage;
	}

	// Token: 0x060030A4 RID: 12452 RVA: 0x00118DEC File Offset: 0x00116FEC
	public void CompressHeat(HeatCompressor.StatesInstance smi, float dt)
	{
		smi.heatRemovalTimer -= dt;
		float num = this.heatRemovalRate * dt / (float)this.inputStorage.items.Count;
		foreach (GameObject gameObject in this.inputStorage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			float lowTemp = component.Element.lowTemp;
			GameUtil.DeltaThermalEnergy(component, -num, lowTemp);
			this.energyCompressed += num;
		}
		if (smi.heatRemovalTimer <= 0f)
		{
			for (int i = this.inputStorage.items.Count; i > 0; i--)
			{
				GameObject gameObject2 = this.inputStorage.items[i - 1];
				if (gameObject2)
				{
					this.inputStorage.Transfer(gameObject2, this.outputStorage, false, true);
				}
			}
			smi.StartNewHeatRemoval();
		}
		foreach (GameObject gameObject3 in this.heatCubeStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject3.GetComponent<PrimaryElement>(), this.energyCompressed / (float)this.heatCubeStorage.items.Count, 100000f);
		}
		this.energyCompressed = 0f;
	}

	// Token: 0x060030A5 RID: 12453 RVA: 0x00118F60 File Offset: 0x00117160
	public void EjectHeatCube()
	{
		this.heatCubeStorage.DropAll(base.transform.GetPosition(), false, false, default(Vector3), true, null);
	}

	// Token: 0x04001D03 RID: 7427
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001D04 RID: 7428
	private MeterController meter;

	// Token: 0x04001D05 RID: 7429
	public Storage inputStorage;

	// Token: 0x04001D06 RID: 7430
	public Storage outputStorage;

	// Token: 0x04001D07 RID: 7431
	public Storage heatCubeStorage;

	// Token: 0x04001D08 RID: 7432
	public float heatRemovalRate = 100f;

	// Token: 0x04001D09 RID: 7433
	public float heatRemovalTime = 100f;

	// Token: 0x04001D0A RID: 7434
	[Serialize]
	public float energyCompressed;

	// Token: 0x04001D0B RID: 7435
	public float heat_sink_active_time = 9000f;

	// Token: 0x04001D0C RID: 7436
	[Serialize]
	public float time_active;

	// Token: 0x04001D0D RID: 7437
	public float MAX_CUBE_TEMPERATURE = 3000f;

	// Token: 0x02001687 RID: 5767
	public class StatesInstance : GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.GameInstance
	{
		// Token: 0x06009799 RID: 38809 RVA: 0x00385431 File Offset: 0x00383631
		public StatesInstance(HeatCompressor master) : base(master)
		{
		}

		// Token: 0x0600979A RID: 38810 RVA: 0x0038543C File Offset: 0x0038363C
		public void UpdateMeter()
		{
			float remainingCharge = this.GetRemainingCharge();
			base.master.meter.SetPositionPercent(remainingCharge);
		}

		// Token: 0x0600979B RID: 38811 RVA: 0x00385464 File Offset: 0x00383664
		public float GetRemainingCharge()
		{
			PrimaryElement primaryElement = base.smi.master.heatCubeStorage.FindFirstWithMass(GameTags.IndustrialIngredient, 0f);
			float result = 1f;
			if (primaryElement != null)
			{
				result = Mathf.Clamp01(primaryElement.GetComponent<PrimaryElement>().Temperature / base.smi.master.MAX_CUBE_TEMPERATURE);
			}
			return result;
		}

		// Token: 0x0600979C RID: 38812 RVA: 0x003854C3 File Offset: 0x003836C3
		public bool CanWork()
		{
			return this.GetRemainingCharge() < 1f && base.smi.master.heatCubeStorage.items.Count > 0;
		}

		// Token: 0x0600979D RID: 38813 RVA: 0x003854F1 File Offset: 0x003836F1
		public void StartNewHeatRemoval()
		{
			this.heatRemovalTimer = base.smi.master.heatRemovalTime;
		}

		// Token: 0x04007524 RID: 29988
		[Serialize]
		public float heatRemovalTimer;
	}

	// Token: 0x02001688 RID: 5768
	public class States : GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor>
	{
		// Token: 0x0600979E RID: 38814 RVA: 0x0038550C File Offset: 0x0038370C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inactive;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventTransition(GameHashes.OperationalChanged, this.inactive, (HeatCompressor.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.inactive.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.UpdateMeter();
			}).PlayAnim("idle").Transition(this.dropCube, (HeatCompressor.StatesInstance smi) => smi.GetRemainingCharge() >= 1f, UpdateRate.SIM_200ms).Transition(this.active, (HeatCompressor.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational && smi.CanWork(), UpdateRate.SIM_200ms);
			this.active.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
				smi.StartNewHeatRemoval();
			}).PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(HeatCompressor.StatesInstance smi, float dt)
			{
				smi.master.time_active += dt;
				smi.UpdateMeter();
				smi.master.CompressHeat(smi, dt);
			}, UpdateRate.SIM_200ms, false).Transition(this.dropCube, (HeatCompressor.StatesInstance smi) => smi.GetRemainingCharge() >= 1f, UpdateRate.SIM_200ms).Transition(this.inactive, (HeatCompressor.StatesInstance smi) => !smi.CanWork(), UpdateRate.SIM_200ms).Exit(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.dropCube.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.master.EjectHeatCube();
				smi.GoTo(this.inactive);
			});
		}

		// Token: 0x04007525 RID: 29989
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State active;

		// Token: 0x04007526 RID: 29990
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State inactive;

		// Token: 0x04007527 RID: 29991
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State dropCube;
	}
}
