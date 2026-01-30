using System;

// Token: 0x020005E4 RID: 1508
public class FossilSculptureLightMonitor : GameStateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>
{
	// Token: 0x060022ED RID: 8941 RVA: 0x000CB558 File Offset: 0x000C9758
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noLit;
		this.noLit.TagTransition(GameTags.Operational, this.lit, false).EventHandler(GameHashes.WorkableCompleteWork, new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.HideLitEffect)).EventHandler(GameHashes.ArtableStateChanged, new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.HideLitEffect)).Enter(new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.HideLitEffect));
		this.lit.TagTransition(GameTags.Operational, this.noLit, true).EventHandler(GameHashes.WorkableCompleteWork, new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.ShowLitEffect)).EventHandler(GameHashes.ArtableStateChanged, new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.ShowLitEffect)).Enter(new StateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State.Callback(FossilSculptureLightMonitor.ShowLitEffect));
	}

	// Token: 0x060022EE RID: 8942 RVA: 0x000CB61E File Offset: 0x000C981E
	public static void ShowLitEffect(FossilSculptureLightMonitor.Instance smi)
	{
		smi.SetAnimLitState(true);
	}

	// Token: 0x060022EF RID: 8943 RVA: 0x000CB627 File Offset: 0x000C9827
	public static void HideLitEffect(FossilSculptureLightMonitor.Instance smi)
	{
		smi.SetAnimLitState(false);
	}

	// Token: 0x04001479 RID: 5241
	public const string LIT_LIGHT_BLOOM_SYMBOL_NAME = "statue_light_bloom";

	// Token: 0x0400147A RID: 5242
	public const string LIT_SHADING_SYMBOL_NAME = "shading_with_light";

	// Token: 0x0400147B RID: 5243
	public const string UNLIT_SHADING_SYMBOL_NAME = "shading_no_light";

	// Token: 0x0400147C RID: 5244
	public GameStateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State noLit;

	// Token: 0x0400147D RID: 5245
	public GameStateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.State lit;

	// Token: 0x020014C6 RID: 5318
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006F92 RID: 28562
		public bool usingBloom = true;
	}

	// Token: 0x020014C7 RID: 5319
	public new class Instance : GameStateMachine<FossilSculptureLightMonitor, FossilSculptureLightMonitor.Instance, IStateMachineTarget, FossilSculptureLightMonitor.Def>.GameInstance
	{
		// Token: 0x06009112 RID: 37138 RVA: 0x00370327 File Offset: 0x0036E527
		public Instance(IStateMachineTarget master, FossilSculptureLightMonitor.Def def) : base(master, def)
		{
			this.SetAnimLitState(false);
		}

		// Token: 0x06009113 RID: 37139 RVA: 0x00370338 File Offset: 0x0036E538
		public void SetAnimLitState(bool lit)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.SetSymbolVisiblity("statue_light_bloom", base.def.usingBloom && lit);
			component.SetSymbolVisiblity("shading_with_light", lit);
			component.SetSymbolVisiblity("shading_no_light", !lit);
		}
	}
}
