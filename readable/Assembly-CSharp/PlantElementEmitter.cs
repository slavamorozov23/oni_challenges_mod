using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A95 RID: 2709
public class PlantElementEmitter : StateMachineComponent<PlantElementEmitter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06004EA2 RID: 20130 RVA: 0x001C999B File Offset: 0x001C7B9B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004EA3 RID: 20131 RVA: 0x001C99AE File Offset: 0x001C7BAE
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x04003475 RID: 13429
	[MyCmpGet]
	private WiltCondition wiltCondition;

	// Token: 0x04003476 RID: 13430
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003477 RID: 13431
	public SimHashes emittedElement;

	// Token: 0x04003478 RID: 13432
	public float emitRate;

	// Token: 0x02001BAF RID: 7087
	public class StatesInstance : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.GameInstance
	{
		// Token: 0x0600AABB RID: 43707 RVA: 0x003C516B File Offset: 0x003C336B
		public StatesInstance(PlantElementEmitter master) : base(master)
		{
		}

		// Token: 0x0600AABC RID: 43708 RVA: 0x003C5174 File Offset: 0x003C3374
		public bool IsWilting()
		{
			return !(base.master.wiltCondition == null) && base.master.wiltCondition != null && base.master.wiltCondition.IsWilting();
		}
	}

	// Token: 0x02001BB0 RID: 7088
	public class States : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter>
	{
		// Token: 0x0600AABD RID: 43709 RVA: 0x003C51B0 File Offset: 0x003C33B0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.healthy;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.healthy.EventTransition(GameHashes.Wilt, this.wilted, (PlantElementEmitter.StatesInstance smi) => smi.IsWilting()).Update("PlantEmit", delegate(PlantElementEmitter.StatesInstance smi, float dt)
			{
				SimMessages.EmitMass(Grid.PosToCell(smi.master.gameObject), ElementLoader.FindElementByHash(smi.master.emittedElement).idx, smi.master.emitRate * dt, ElementLoader.FindElementByHash(smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0, -1);
			}, UpdateRate.SIM_4000ms, false);
			this.wilted.EventTransition(GameHashes.WiltRecover, this.healthy, null);
		}

		// Token: 0x0400857F RID: 34175
		public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State wilted;

		// Token: 0x04008580 RID: 34176
		public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State healthy;
	}
}
