using System;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class BeeSleepMonitor : GameStateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>
{
	// Token: 0x06000409 RID: 1033 RVA: 0x00021DE6 File Offset: 0x0001FFE6
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<BeeSleepMonitor.Instance, float>(this.UpdateCO2Exposure), UpdateRate.SIM_1000ms, false).ToggleBehaviour(GameTags.Creatures.BeeWantsToSleep, new StateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>.Transition.ConditionCallback(this.ShouldSleep), null);
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x00021E21 File Offset: 0x00020021
	public bool ShouldSleep(BeeSleepMonitor.Instance smi)
	{
		return smi.CO2Exposure >= 5f;
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00021E34 File Offset: 0x00020034
	public void UpdateCO2Exposure(BeeSleepMonitor.Instance smi, float dt)
	{
		if (this.IsInCO2(smi))
		{
			smi.CO2Exposure += 1f;
		}
		else
		{
			smi.CO2Exposure -= 0.5f;
		}
		smi.CO2Exposure = Mathf.Clamp(smi.CO2Exposure, 0f, 10f);
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x00021E8C File Offset: 0x0002008C
	public bool IsInCO2(BeeSleepMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi.gameObject);
		return Grid.IsValidCell(num) && Grid.Element[num].id == SimHashes.CarbonDioxide;
	}

	// Token: 0x020010F3 RID: 4339
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020010F4 RID: 4340
	public new class Instance : GameStateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>.GameInstance
	{
		// Token: 0x0600835B RID: 33627 RVA: 0x003434A9 File Offset: 0x003416A9
		public Instance(IStateMachineTarget master, BeeSleepMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x040063A2 RID: 25506
		public float CO2Exposure;
	}
}
