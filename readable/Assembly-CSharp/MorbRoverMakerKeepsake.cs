using System;
using UnityEngine;

// Token: 0x02000362 RID: 866
public class MorbRoverMakerKeepsake : GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>
{
	// Token: 0x06001216 RID: 4630 RVA: 0x00069940 File Offset: 0x00067B40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.silent;
		this.silent.PlayAnim("silent").Enter(new StateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State.Callback(MorbRoverMakerKeepsake.CalculateNextActivationTime)).Update(new Action<MorbRoverMakerKeepsake.Instance, float>(MorbRoverMakerKeepsake.TimerUpdate), UpdateRate.SIM_200ms, false);
		this.talking.PlayAnim("idle").OnAnimQueueComplete(this.silent);
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x000699AD File Offset: 0x00067BAD
	public static void CalculateNextActivationTime(MorbRoverMakerKeepsake.Instance smi)
	{
		smi.CalculateNextActivationTime();
	}

	// Token: 0x06001218 RID: 4632 RVA: 0x000699B5 File Offset: 0x00067BB5
	public static void TimerUpdate(MorbRoverMakerKeepsake.Instance smi, float dt)
	{
		if (GameClock.Instance.GetTime() > smi.NextActivationTime)
		{
			smi.GoTo(smi.sm.talking);
		}
	}

	// Token: 0x04000B64 RID: 2916
	public const string SILENT_ANIMATION_NAME = "silent";

	// Token: 0x04000B65 RID: 2917
	public const string TALKING_ANIMATION_NAME = "idle";

	// Token: 0x04000B66 RID: 2918
	public GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State silent;

	// Token: 0x04000B67 RID: 2919
	public GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State talking;

	// Token: 0x02001249 RID: 4681
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006778 RID: 26488
		public Vector2 OperationalRandomnessRange = new Vector2(120f, 600f);
	}

	// Token: 0x0200124A RID: 4682
	public new class Instance : GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.GameInstance
	{
		// Token: 0x06008798 RID: 34712 RVA: 0x0034BF11 File Offset: 0x0034A111
		public Instance(IStateMachineTarget master, MorbRoverMakerKeepsake.Def def) : base(master, def)
		{
		}

		// Token: 0x06008799 RID: 34713 RVA: 0x0034BF28 File Offset: 0x0034A128
		public void CalculateNextActivationTime()
		{
			float time = GameClock.Instance.GetTime();
			float minInclusive = time + base.def.OperationalRandomnessRange.x;
			float maxInclusive = time + base.def.OperationalRandomnessRange.y;
			this.NextActivationTime = UnityEngine.Random.Range(minInclusive, maxInclusive);
		}

		// Token: 0x04006779 RID: 26489
		public float NextActivationTime = -1f;
	}
}
