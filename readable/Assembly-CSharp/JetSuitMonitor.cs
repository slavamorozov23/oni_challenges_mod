using System;
using UnityEngine;

// Token: 0x020009D2 RID: 2514
public class JetSuitMonitor : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance>
{
	// Token: 0x060048FE RID: 18686 RVA: 0x001A6774 File Offset: 0x001A4974
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.Target(this.owner);
		this.off.EventTransition(GameHashes.PathAdvanced, this.flying, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStartFlying));
		this.flying.Enter(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StartFlying)).Exit(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StopFlying)).EventTransition(GameHashes.PathAdvanced, this.off, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStopFlying)).Update(new Action<JetSuitMonitor.Instance, float>(JetSuitMonitor.Emit), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x060048FF RID: 18687 RVA: 0x001A6810 File Offset: 0x001A4A10
	public static bool ShouldStartFlying(JetSuitMonitor.Instance smi)
	{
		return smi.navigator && smi.navigator.CurrentNavType == NavType.Hover;
	}

	// Token: 0x06004900 RID: 18688 RVA: 0x001A682F File Offset: 0x001A4A2F
	public static bool ShouldStopFlying(JetSuitMonitor.Instance smi)
	{
		return !smi.navigator || smi.navigator.CurrentNavType != NavType.Hover;
	}

	// Token: 0x06004901 RID: 18689 RVA: 0x001A6851 File Offset: 0x001A4A51
	public static void StartFlying(JetSuitMonitor.Instance smi)
	{
	}

	// Token: 0x06004902 RID: 18690 RVA: 0x001A6853 File Offset: 0x001A4A53
	public static void StopFlying(JetSuitMonitor.Instance smi)
	{
	}

	// Token: 0x06004903 RID: 18691 RVA: 0x001A6858 File Offset: 0x001A4A58
	public static void Emit(JetSuitMonitor.Instance smi, float dt)
	{
		if (!smi.navigator)
		{
			return;
		}
		GameObject gameObject = smi.sm.owner.Get(smi);
		if (!gameObject)
		{
			return;
		}
		Grid.PosToCell(gameObject.transform.GetPosition());
		float num = 0.2f * dt;
		num = Mathf.Min(num, smi.jet_suit_tank.amount);
		smi.jet_suit_tank.amount -= num;
		float num2 = num * 0.25f;
		if (num2 > 1E-45f)
		{
			Vector3 vector2;
			Vector3 vector;
			Vector3 pos = vector = (vector2 = gameObject.transform.position);
			Vector3 down = Vector3.down;
			if (smi.helmetController.jet_anim != null)
			{
				KBatchedAnimController jet_anim = smi.helmetController.jet_anim;
				bool flag;
				Matrix4x4 symbolTransform = jet_anim.GetSymbolTransform("left_fire", out flag);
				Matrix4x4 symbolTransform2 = jet_anim.GetSymbolTransform("right_fire", out flag);
				vector2 = symbolTransform.GetColumn(3);
				vector = symbolTransform2.GetColumn(3);
				float f = Quaternion.LookRotation(symbolTransform.GetColumn(2), symbolTransform.GetColumn(1)).eulerAngles.z * 0.017453292f;
				down = new Vector3(-Mathf.Sin(f), Mathf.Cos(f));
				vector2 += down.normalized * 0.6f;
				vector += down.normalized * 0.6f;
			}
			float mass = num2 / 2f;
			float d = 0.5f;
			int co2Cell = Grid.PosToCell(pos);
			CO2Manager.instance.SpawnExhaust(vector2, down.normalized * d, co2Cell, mass, 373.15f);
			CO2Manager.instance.SpawnExhaust(vector, down.normalized * d, co2Cell, mass, 373.15f);
		}
		if (smi.jet_suit_tank.amount == 0f)
		{
			smi.navigator.AddTag(GameTags.JetSuitOutOfFuel);
			smi.navigator.SetCurrentNavType(NavType.Floor);
		}
	}

	// Token: 0x0400308A RID: 12426
	public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x0400308B RID: 12427
	public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State flying;

	// Token: 0x0400308C RID: 12428
	public StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.TargetParameter owner;

	// Token: 0x02001A2C RID: 6700
	public new class Instance : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A46F RID: 42095 RVA: 0x003B4698 File Offset: 0x003B2898
		public Instance(IStateMachineTarget master, GameObject owner) : base(master)
		{
			base.sm.owner.Set(owner, base.smi, false);
			this.helmetController = master.GetComponent<HelmetController>();
			this.navigator = owner.GetComponent<Navigator>();
			this.jet_suit_tank = master.GetComponent<JetSuitTank>();
		}

		// Token: 0x040080AB RID: 32939
		public HelmetController helmetController;

		// Token: 0x040080AC RID: 32940
		public Navigator navigator;

		// Token: 0x040080AD RID: 32941
		public JetSuitTank jet_suit_tank;
	}
}
