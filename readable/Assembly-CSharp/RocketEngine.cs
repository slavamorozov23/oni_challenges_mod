using System;
using KSerialization;
using STRINGS;

// Token: 0x02000BA1 RID: 2977
[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngine : StateMachineComponent<RocketEngine.StatesInstance>
{
	// Token: 0x060058F3 RID: 22771 RVA: 0x002045DC File Offset: 0x002027DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.mainEngine)
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new RequireAttachedComponent(base.gameObject.GetComponent<AttachableBuilding>(), typeof(FuelTank), UI.STARMAP.COMPONENT.FUEL_TANK));
		}
	}

	// Token: 0x04003BAB RID: 15275
	public float exhaustEmitRate = 50f;

	// Token: 0x04003BAC RID: 15276
	public float exhaustTemperature = 1500f;

	// Token: 0x04003BAD RID: 15277
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x04003BAE RID: 15278
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x04003BAF RID: 15279
	public Tag fuelTag;

	// Token: 0x04003BB0 RID: 15280
	public float efficiency = 1f;

	// Token: 0x04003BB1 RID: 15281
	public bool requireOxidizer = true;

	// Token: 0x04003BB2 RID: 15282
	public bool mainEngine = true;

	// Token: 0x02001D2C RID: 7468
	public class StatesInstance : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.GameInstance
	{
		// Token: 0x0600B063 RID: 45155 RVA: 0x003DA82C File Offset: 0x003D8A2C
		public StatesInstance(RocketEngine smi) : base(smi)
		{
		}
	}

	// Token: 0x02001D2D RID: 7469
	public class States : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine>
	{
		// Token: 0x0600B064 RID: 45156 RVA: 0x003DA838 File Offset: 0x003D8A38
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, null);
			this.burning.EventTransition(GameHashes.RocketLanded, this.burnComplete, null).PlayAnim("launch_pre").QueueAnim("launch_loop", true, null).Update(delegate(RocketEngine.StatesInstance smi, float dt)
			{
				int num = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
				if (Grid.IsValidCell(num))
				{
					SimMessages.EmitMass(num, ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, 0, 0, -1);
				}
				int num2 = 10;
				for (int i = 1; i < num2; i++)
				{
					int num3 = Grid.OffsetCell(num, -1, -i);
					int num4 = Grid.OffsetCell(num, 0, -i);
					int num5 = Grid.OffsetCell(num, 1, -i);
					if (Grid.IsValidCell(num3))
					{
						SimMessages.ModifyEnergy(num3, smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
					}
					if (Grid.IsValidCell(num4))
					{
						SimMessages.ModifyEnergy(num4, smi.master.exhaustTemperature / (float)i, 3200f, SimMessages.EnergySourceID.Burner);
					}
					if (Grid.IsValidCell(num5))
					{
						SimMessages.ModifyEnergy(num5, smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
					}
				}
			}, UpdateRate.SIM_200ms, false);
			this.burnComplete.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, null);
		}

		// Token: 0x04008A89 RID: 35465
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State idle;

		// Token: 0x04008A8A RID: 35466
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burning;

		// Token: 0x04008A8B RID: 35467
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burnComplete;
	}
}
