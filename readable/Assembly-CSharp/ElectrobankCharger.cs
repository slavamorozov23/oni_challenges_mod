using System;
using UnityEngine;

// Token: 0x0200074F RID: 1871
public class ElectrobankCharger : GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>
{
	// Token: 0x06002F52 RID: 12114 RVA: 0x00111338 File Offset: 0x0010F538
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noBattery;
		this.noBattery.PlayAnim("off").EventHandler(GameHashes.OnStorageChange, delegate(ElectrobankCharger.Instance smi, object data)
		{
			smi.QueueElectrobank(null);
		}).ParamTransition<bool>(this.hasElectrobank, this.charging, GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.IsTrue).Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.QueueElectrobank(null);
		});
		this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.charging, (ElectrobankCharger.Instance smi) => smi.master.GetComponent<Operational>().IsOperational);
		this.charging.QueueAnim("working_pre", false, null).QueueAnim("working_loop", true, null).Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.QueueElectrobank(null);
			smi.master.GetComponent<Operational>().SetActive(true, false);
		}).Exit(delegate(ElectrobankCharger.Instance smi)
		{
			smi.master.GetComponent<Operational>().SetActive(false, false);
		}).ToggleStatusItem(Db.Get().BuildingStatusItems.PowerBankChargerInProgress, null).Update(delegate(ElectrobankCharger.Instance smi, float dt)
		{
			smi.ChargeInternal(smi, dt);
		}, UpdateRate.SIM_EVERY_TICK, false).EventTransition(GameHashes.OperationalChanged, this.inoperational, (ElectrobankCharger.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).ParamTransition<float>(this.internalChargeAmount, this.full, (ElectrobankCharger.Instance smi, float dt) => this.internalChargeAmount.Get(smi) >= 120000f);
		this.full.PlayAnim("working_pst").Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.TransferChargeToElectrobank();
		}).OnAnimQueueComplete(this.noBattery);
	}

	// Token: 0x04001C11 RID: 7185
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State noBattery;

	// Token: 0x04001C12 RID: 7186
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State inoperational;

	// Token: 0x04001C13 RID: 7187
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State charging;

	// Token: 0x04001C14 RID: 7188
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State full;

	// Token: 0x04001C15 RID: 7189
	public StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.FloatParameter internalChargeAmount;

	// Token: 0x04001C16 RID: 7190
	public StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.BoolParameter hasElectrobank;

	// Token: 0x02001630 RID: 5680
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001631 RID: 5681
	public new class Instance : GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.GameInstance
	{
		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06009642 RID: 38466 RVA: 0x0037ED3A File Offset: 0x0037CF3A
		public Storage Storage
		{
			get
			{
				if (this.storage == null)
				{
					this.storage = base.GetComponent<Storage>();
				}
				return this.storage;
			}
		}

		// Token: 0x06009643 RID: 38467 RVA: 0x0037ED5C File Offset: 0x0037CF5C
		public Instance(IStateMachineTarget master, ElectrobankCharger.Def def) : base(master, def)
		{
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x06009644 RID: 38468 RVA: 0x0037ED89 File Offset: 0x0037CF89
		public void ChargeInternal(ElectrobankCharger.Instance smi, float dt)
		{
			smi.sm.internalChargeAmount.Delta(dt * 400f, smi);
			this.UpdateMeter();
		}

		// Token: 0x06009645 RID: 38469 RVA: 0x0037EDAA File Offset: 0x0037CFAA
		public void UpdateMeter()
		{
			this.meterController.SetPositionPercent(base.sm.internalChargeAmount.Get(base.smi) / 120000f);
		}

		// Token: 0x06009646 RID: 38470 RVA: 0x0037EDD3 File Offset: 0x0037CFD3
		public void TransferChargeToElectrobank()
		{
			this.targetElectrobank = Electrobank.ReplaceEmptyWithCharged(this.targetElectrobank, true);
			this.DequeueElectrobank();
		}

		// Token: 0x06009647 RID: 38471 RVA: 0x0037EDF0 File Offset: 0x0037CFF0
		public void DequeueElectrobank()
		{
			this.targetElectrobank = null;
			base.smi.sm.hasElectrobank.Set(false, base.smi, false);
			base.smi.sm.internalChargeAmount.Set(0f, base.smi, false);
			this.UpdateMeter();
		}

		// Token: 0x06009648 RID: 38472 RVA: 0x0037EE4C File Offset: 0x0037D04C
		public void QueueElectrobank(object data = null)
		{
			if (this.targetElectrobank == null)
			{
				for (int i = 0; i < this.Storage.items.Count; i++)
				{
					GameObject gameObject = this.Storage.items[i];
					if (gameObject != null && gameObject.HasTag(GameTags.EmptyPortableBattery))
					{
						this.targetElectrobank = gameObject;
						base.smi.sm.hasElectrobank.Set(true, base.smi, false);
						break;
					}
				}
			}
			this.UpdateMeter();
		}

		// Token: 0x04007406 RID: 29702
		private Storage storage;

		// Token: 0x04007407 RID: 29703
		public GameObject targetElectrobank;

		// Token: 0x04007408 RID: 29704
		private MeterController meterController;
	}
}
