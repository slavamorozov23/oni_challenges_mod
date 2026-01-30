using System;

// Token: 0x020000FE RID: 254
public class HiveHarvestStates : GameStateMachine<HiveHarvestStates, HiveHarvestStates.Instance, IStateMachineTarget, HiveHarvestStates.Def>
{
	// Token: 0x060004AA RID: 1194 RVA: 0x000260C3 File Offset: 0x000242C3
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.DoNothing();
	}

	// Token: 0x02001159 RID: 4441
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200115A RID: 4442
	public new class Instance : GameStateMachine<HiveHarvestStates, HiveHarvestStates.Instance, IStateMachineTarget, HiveHarvestStates.Def>.GameInstance
	{
		// Token: 0x0600844E RID: 33870 RVA: 0x00344C5D File Offset: 0x00342E5D
		public Instance(Chore<HiveHarvestStates.Instance> chore, HiveHarvestStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.HarvestHiveBehaviour);
		}
	}
}
