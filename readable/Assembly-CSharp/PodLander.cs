using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B99 RID: 2969
[SerializationConfig(MemberSerialization.OptIn)]
public class PodLander : StateMachineComponent<PodLander.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060058BC RID: 22716 RVA: 0x00203378 File Offset: 0x00201578
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060058BD RID: 22717 RVA: 0x0020338C File Offset: 0x0020158C
	public void ReleaseAstronaut()
	{
		if (this.releasingAstronaut)
		{
			return;
		}
		this.releasingAstronaut = true;
		MinionStorage component = base.GetComponent<MinionStorage>();
		List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
		for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
		{
			MinionStorage.Info info = storedMinionInfo[i];
			component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(base.smi.master.transform.GetPosition())));
		}
		this.releasingAstronaut = false;
	}

	// Token: 0x060058BE RID: 22718 RVA: 0x00203405 File Offset: 0x00201605
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x04003B84 RID: 15236
	[Serialize]
	private int landOffLocation;

	// Token: 0x04003B85 RID: 15237
	[Serialize]
	private float flightAnimOffset;

	// Token: 0x04003B86 RID: 15238
	private float rocketSpeed;

	// Token: 0x04003B87 RID: 15239
	public float exhaustEmitRate = 2f;

	// Token: 0x04003B88 RID: 15240
	public float exhaustTemperature = 1000f;

	// Token: 0x04003B89 RID: 15241
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x04003B8A RID: 15242
	private GameObject soundSpeakerObject;

	// Token: 0x04003B8B RID: 15243
	private bool releasingAstronaut;

	// Token: 0x02001D25 RID: 7461
	public class StatesInstance : GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.GameInstance
	{
		// Token: 0x0600B044 RID: 45124 RVA: 0x003DA0C5 File Offset: 0x003D82C5
		public StatesInstance(PodLander master) : base(master)
		{
		}
	}

	// Token: 0x02001D26 RID: 7462
	public class States : GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander>
	{
		// Token: 0x0600B045 RID: 45125 RVA: 0x003DA0D0 File Offset: 0x003D82D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.landing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.landing.PlayAnim("launch_loop", KAnim.PlayMode.Loop).Enter(delegate(PodLander.StatesInstance smi)
			{
				smi.master.flightAnimOffset = 50f;
			}).Update(delegate(PodLander.StatesInstance smi, float dt)
			{
				float num = 10f;
				smi.master.rocketSpeed = num - Mathf.Clamp(Mathf.Pow(smi.timeinstate / 3.5f, 4f), 0f, num - 2f);
				smi.master.flightAnimOffset -= dt * smi.master.rocketSpeed;
				KBatchedAnimController component = smi.master.GetComponent<KBatchedAnimController>();
				component.Offset = Vector3.up * smi.master.flightAnimOffset;
				Vector3 positionIncludingOffset = component.PositionIncludingOffset;
				int num2 = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
				if (Grid.IsValidCell(num2))
				{
					SimMessages.EmitMass(num2, ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, 0, 0, -1);
				}
				if (component.Offset.y <= 0f)
				{
					smi.GoTo(this.crashed);
				}
			}, UpdateRate.SIM_33ms, false);
			this.crashed.PlayAnim("grounded").Enter(delegate(PodLander.StatesInstance smi)
			{
				smi.master.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
				smi.master.rocketSpeed = 0f;
				smi.master.ReleaseAstronaut();
			});
		}

		// Token: 0x04008A74 RID: 35444
		public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State landing;

		// Token: 0x04008A75 RID: 35445
		public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State crashed;
	}
}
