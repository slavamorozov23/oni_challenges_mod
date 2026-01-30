using System;

// Token: 0x02000A1A RID: 2586
public class CreatureDecorMonitor : GameStateMachine<CreatureDecorMonitor, CreatureDecorMonitor.Instance, IStateMachineTarget, CreatureDecorMonitor.Def>
{
	// Token: 0x06004BCB RID: 19403 RVA: 0x001B871C File Offset: 0x001B691C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.lowDecor;
		this.lowDecor.UpdateTransition(this.highDecor, new Func<CreatureDecorMonitor.Instance, float, bool>(CreatureDecorMonitor.IsInHighDecor), UpdateRate.SIM_4000ms, false).Update(new Action<CreatureDecorMonitor.Instance, float>(CreatureDecorMonitor.TriggerLowDecorUpdate), UpdateRate.SIM_4000ms, false).TriggerOnEnter(GameHashes.CreatureLowDecor, null);
		this.highDecor.UpdateTransition(this.lowDecor, new Func<CreatureDecorMonitor.Instance, float, bool>(CreatureDecorMonitor.IsInLowDecor), UpdateRate.SIM_4000ms, false).Update(new Action<CreatureDecorMonitor.Instance, float>(CreatureDecorMonitor.TriggerHighDecorUpdate), UpdateRate.SIM_4000ms, false).TriggerOnEnter(GameHashes.CreatureHighDecor, null);
	}

	// Token: 0x06004BCC RID: 19404 RVA: 0x001B87B4 File Offset: 0x001B69B4
	private static void TriggerHighDecorUpdate(CreatureDecorMonitor.Instance smi, float dt)
	{
		Action<float> onHighDecorUpdate = smi.OnHighDecorUpdate;
		if (onHighDecorUpdate == null)
		{
			return;
		}
		onHighDecorUpdate(dt);
	}

	// Token: 0x06004BCD RID: 19405 RVA: 0x001B87C7 File Offset: 0x001B69C7
	private static void TriggerLowDecorUpdate(CreatureDecorMonitor.Instance smi, float dt)
	{
		Action<float> onLowDecorUpdate = smi.OnLowDecorUpdate;
		if (onLowDecorUpdate == null)
		{
			return;
		}
		onLowDecorUpdate(dt);
	}

	// Token: 0x06004BCE RID: 19406 RVA: 0x001B87DA File Offset: 0x001B69DA
	private static bool IsInHighDecor(CreatureDecorMonitor.Instance smi, float dt)
	{
		return Grid.Decor[Grid.PosToCell(smi)] >= smi.def.DecorValueTreshold;
	}

	// Token: 0x06004BCF RID: 19407 RVA: 0x001B87F8 File Offset: 0x001B69F8
	private static bool IsInLowDecor(CreatureDecorMonitor.Instance smi, float dt)
	{
		return !CreatureDecorMonitor.IsInHighDecor(smi, dt);
	}

	// Token: 0x0400323E RID: 12862
	private const UpdateRate UPDATE_RATE = UpdateRate.SIM_4000ms;

	// Token: 0x0400323F RID: 12863
	private GameStateMachine<CreatureDecorMonitor, CreatureDecorMonitor.Instance, IStateMachineTarget, CreatureDecorMonitor.Def>.State lowDecor;

	// Token: 0x04003240 RID: 12864
	private GameStateMachine<CreatureDecorMonitor, CreatureDecorMonitor.Instance, IStateMachineTarget, CreatureDecorMonitor.Def>.State highDecor;

	// Token: 0x02001ABE RID: 6846
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04008299 RID: 33433
		public float DecorValueTreshold;
	}

	// Token: 0x02001ABF RID: 6847
	public new class Instance : GameStateMachine<CreatureDecorMonitor, CreatureDecorMonitor.Instance, IStateMachineTarget, CreatureDecorMonitor.Def>.GameInstance
	{
		// Token: 0x0600A6F4 RID: 42740 RVA: 0x003BB0B1 File Offset: 0x003B92B1
		public Instance(IStateMachineTarget master, CreatureDecorMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0400829A RID: 33434
		public Action<float> OnHighDecorUpdate;

		// Token: 0x0400829B RID: 33435
		public Action<float> OnLowDecorUpdate;
	}
}
