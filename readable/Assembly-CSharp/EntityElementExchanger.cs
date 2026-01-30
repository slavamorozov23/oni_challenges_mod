using System;
using UnityEngine;

// Token: 0x02000A5C RID: 2652
public class EntityElementExchanger : StateMachineComponent<EntityElementExchanger.StatesInstance>
{
	// Token: 0x06004D16 RID: 19734 RVA: 0x001C0A9C File Offset: 0x001BEC9C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06004D17 RID: 19735 RVA: 0x001C0AA4 File Offset: 0x001BECA4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004D18 RID: 19736 RVA: 0x001C0AB7 File Offset: 0x001BECB7
	public void SetConsumptionRate(float consumptionRate)
	{
		this.consumeRate = consumptionRate;
	}

	// Token: 0x06004D19 RID: 19737 RVA: 0x001C0AC0 File Offset: 0x001BECC0
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		EntityElementExchanger entityElementExchanger = (EntityElementExchanger)data;
		if (entityElementExchanger != null)
		{
			entityElementExchanger.OnSimConsume(mass_cb_info);
		}
	}

	// Token: 0x06004D1A RID: 19738 RVA: 0x001C0AE4 File Offset: 0x001BECE4
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		float num = mass_cb_info.mass * base.smi.master.exchangeRatio;
		if (this.reportExchange && base.smi.master.emittedElement == SimHashes.Oxygen)
		{
			string text = base.gameObject.GetProperName();
			ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
			if (component != null && component.GetReceptacle() != null)
			{
				text = text + " (" + component.GetReceptacle().gameObject.GetProperName() + ")";
			}
			ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, num, text, null);
		}
		SimMessages.EmitMass(Grid.PosToCell(base.smi.master.transform.GetPosition() + this.outputOffset), ElementLoader.FindElementByHash(base.smi.master.emittedElement).idx, num, ElementLoader.FindElementByHash(base.smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0, -1);
	}

	// Token: 0x04003376 RID: 13174
	public Vector3 outputOffset = Vector3.zero;

	// Token: 0x04003377 RID: 13175
	public bool reportExchange;

	// Token: 0x04003378 RID: 13176
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003379 RID: 13177
	public SimHashes consumedElement;

	// Token: 0x0400337A RID: 13178
	public SimHashes emittedElement;

	// Token: 0x0400337B RID: 13179
	public float consumeRate;

	// Token: 0x0400337C RID: 13180
	public float exchangeRatio;

	// Token: 0x02001B70 RID: 7024
	public class StatesInstance : GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.GameInstance
	{
		// Token: 0x0600AA06 RID: 43526 RVA: 0x003C30FA File Offset: 0x003C12FA
		public StatesInstance(EntityElementExchanger master) : base(master)
		{
		}
	}

	// Token: 0x02001B71 RID: 7025
	public class States : GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger>
	{
		// Token: 0x0600AA07 RID: 43527 RVA: 0x003C3104 File Offset: 0x003C1304
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.exchanging;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.exchanging.Enter(delegate(EntityElementExchanger.StatesInstance smi)
			{
				WiltCondition component = smi.master.gameObject.GetComponent<WiltCondition>();
				if (component != null && component.IsWilting())
				{
					smi.GoTo(smi.sm.paused);
				}
			}).EventTransition(GameHashes.Wilt, this.paused, null).ToggleStatusItem(Db.Get().CreatureStatusItems.ExchangingElementConsume, null).ToggleStatusItem(Db.Get().CreatureStatusItems.ExchangingElementOutput, null).Update("EntityElementExchanger", delegate(EntityElementExchanger.StatesInstance smi, float dt)
			{
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(EntityElementExchanger.OnSimConsumeCallback), smi.master, "EntityElementExchanger");
				SimMessages.ConsumeMass(Grid.PosToCell(smi.master.gameObject), smi.master.consumedElement, smi.master.consumeRate * dt, 3, handle.index);
			}, UpdateRate.SIM_1000ms, false);
			this.paused.EventTransition(GameHashes.WiltRecover, this.exchanging, null);
		}

		// Token: 0x040084FE RID: 34046
		public GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.State exchanging;

		// Token: 0x040084FF RID: 34047
		public GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.State paused;
	}
}
