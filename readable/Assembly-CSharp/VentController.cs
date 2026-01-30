using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class VentController : GameStateMachine<VentController, VentController.Instance>
{
	// Token: 0x060001C9 RID: 457 RVA: 0x0000C8C8 File Offset: 0x0000AAC8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventHandler(GameHashes.VentAnimatingChanged, new GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.GameEvent.Callback(VentController.UpdateMeterColor)).EventTransition(GameHashes.VentClosed, this.closed, (VentController.Instance smi) => smi.GetComponent<Vent>().Closed()).EventTransition(GameHashes.VentOpen, this.off, (VentController.Instance smi) => !smi.GetComponent<Vent>().Closed());
		this.off.PlayAnim("off").EventTransition(GameHashes.VentAnimatingChanged, this.working_pre, new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(VentController.IsAnimating));
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Enter(new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State.Callback(VentController.PlayOutputMeterAnim)).EventTransition(GameHashes.VentAnimatingChanged, this.working_pst, GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Not(new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(VentController.IsAnimating)));
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
		this.closed.PlayAnim("closed").EventTransition(GameHashes.VentAnimatingChanged, this.working_pre, new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(VentController.IsAnimating));
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0000CA2E File Offset: 0x0000AC2E
	public static void PlayOutputMeterAnim(VentController.Instance smi)
	{
		smi.PlayMeterAnim();
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000CA36 File Offset: 0x0000AC36
	public static bool IsAnimating(VentController.Instance smi)
	{
		return smi.exhaust.IsAnimating();
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0000CA44 File Offset: 0x0000AC44
	public static void UpdateMeterColor(VentController.Instance smi, object data)
	{
		if (data != null)
		{
			Color32 value = ((Boxed<Color32>)data).value;
			value.a = byte.MaxValue;
			smi.SetMeterOutputColor(value);
		}
	}

	// Token: 0x04000124 RID: 292
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04000125 RID: 293
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x04000126 RID: 294
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x04000127 RID: 295
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x04000128 RID: 296
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State closed;

	// Token: 0x04000129 RID: 297
	public StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.BoolParameter isAnimating;

	// Token: 0x0200109F RID: 4255
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040062DE RID: 25310
		public bool usingDynamicColor;

		// Token: 0x040062DF RID: 25311
		public string outputSubstanceAnimName;
	}

	// Token: 0x020010A0 RID: 4256
	public new class Instance : GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600828A RID: 33418 RVA: 0x00341FE4 File Offset: 0x003401E4
		public Instance(IStateMachineTarget master, VentController.Def def) : base(master, def)
		{
			if (def.usingDynamicColor)
			{
				this.outputSubstanceMeter = new MeterController(this.anim, "meter_target", def.outputSubstanceAnimName, Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
			}
		}

		// Token: 0x0600828B RID: 33419 RVA: 0x0034201A File Offset: 0x0034021A
		public void PlayMeterAnim()
		{
			if (this.outputSubstanceMeter != null)
			{
				this.outputSubstanceMeter.meterController.Play(this.outputSubstanceMeter.meterController.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x0600828C RID: 33420 RVA: 0x00342054 File Offset: 0x00340254
		public void SetMeterOutputColor(Color32 color)
		{
			if (this.outputSubstanceMeter != null)
			{
				this.outputSubstanceMeter.meterController.TintColour = color;
			}
		}

		// Token: 0x040062E0 RID: 25312
		[MyCmpGet]
		private KBatchedAnimController anim;

		// Token: 0x040062E1 RID: 25313
		[MyCmpGet]
		public Exhaust exhaust;

		// Token: 0x040062E2 RID: 25314
		private MeterController outputSubstanceMeter;
	}
}
