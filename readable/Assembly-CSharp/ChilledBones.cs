using System;
using Klei.AI;

// Token: 0x02000840 RID: 2112
public class ChilledBones : GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>
{
	// Token: 0x06003996 RID: 14742 RVA: 0x00141AE0 File Offset: 0x0013FCE0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.normal;
		this.normal.UpdateTransition(this.chilled, new Func<ChilledBones.Instance, float, bool>(this.IsChilling), UpdateRate.SIM_200ms, false);
		this.chilled.ToggleEffect("ChilledBones").UpdateTransition(this.normal, new Func<ChilledBones.Instance, float, bool>(this.IsNotChilling), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06003997 RID: 14743 RVA: 0x00141B46 File Offset: 0x0013FD46
	public bool IsNotChilling(ChilledBones.Instance smi, float dt)
	{
		return !this.IsChilling(smi, dt);
	}

	// Token: 0x06003998 RID: 14744 RVA: 0x00141B53 File Offset: 0x0013FD53
	public bool IsChilling(ChilledBones.Instance smi, float dt)
	{
		return smi.IsChilled;
	}

	// Token: 0x04002345 RID: 9029
	public const string EFFECT_NAME = "ChilledBones";

	// Token: 0x04002346 RID: 9030
	public GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.State normal;

	// Token: 0x04002347 RID: 9031
	public GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.State chilled;

	// Token: 0x020017E6 RID: 6118
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400790F RID: 30991
		public float THRESHOLD = -1f;
	}

	// Token: 0x020017E7 RID: 6119
	public new class Instance : GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.GameInstance
	{
		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06009CED RID: 40173 RVA: 0x0039B9B7 File Offset: 0x00399BB7
		public float TemperatureTransferAttribute
		{
			get
			{
				return this.minionModifiers.GetAttributes().GetValue(this.bodyTemperatureTransferAttribute.Id) * 600f;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06009CEE RID: 40174 RVA: 0x0039B9DA File Offset: 0x00399BDA
		public bool IsChilled
		{
			get
			{
				return this.TemperatureTransferAttribute < base.def.THRESHOLD;
			}
		}

		// Token: 0x06009CEF RID: 40175 RVA: 0x0039B9EF File Offset: 0x00399BEF
		public Instance(IStateMachineTarget master, ChilledBones.Def def) : base(master, def)
		{
			this.bodyTemperatureTransferAttribute = Db.Get().Attributes.TryGet("TemperatureDelta");
		}

		// Token: 0x04007910 RID: 30992
		[MyCmpGet]
		public MinionModifiers minionModifiers;

		// Token: 0x04007911 RID: 30993
		public Klei.AI.Attribute bodyTemperatureTransferAttribute;
	}
}
