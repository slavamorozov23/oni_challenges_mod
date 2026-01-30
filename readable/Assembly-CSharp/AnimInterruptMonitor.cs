using System;

// Token: 0x02000885 RID: 2181
public class AnimInterruptMonitor : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>
{
	// Token: 0x06003C13 RID: 15379 RVA: 0x001507E8 File Offset: 0x0014E9E8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.PlayInterruptAnim, new StateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.Transition.ConditionCallback(AnimInterruptMonitor.ShoulPlayAnim), new Action<AnimInterruptMonitor.Instance>(AnimInterruptMonitor.ClearAnim));
	}

	// Token: 0x06003C14 RID: 15380 RVA: 0x0015081B File Offset: 0x0014EA1B
	private static bool ShoulPlayAnim(AnimInterruptMonitor.Instance smi)
	{
		return smi.anims != null;
	}

	// Token: 0x06003C15 RID: 15381 RVA: 0x00150826 File Offset: 0x0014EA26
	private static void ClearAnim(AnimInterruptMonitor.Instance smi)
	{
		smi.anims = null;
	}

	// Token: 0x0200184C RID: 6220
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200184D RID: 6221
	public new class Instance : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.GameInstance
	{
		// Token: 0x06009E7F RID: 40575 RVA: 0x003A3516 File Offset: 0x003A1716
		public Instance(IStateMachineTarget master, AnimInterruptMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009E80 RID: 40576 RVA: 0x003A3520 File Offset: 0x003A1720
		public void PlayAnim(HashedString anim)
		{
			this.PlayAnimSequence(new HashedString[]
			{
				anim
			});
		}

		// Token: 0x06009E81 RID: 40577 RVA: 0x003A3536 File Offset: 0x003A1736
		public void PlayAnimSequence(HashedString[] anims)
		{
			this.anims = anims;
			base.GetComponent<CreatureBrain>().UpdateBrain();
		}

		// Token: 0x04007A8C RID: 31372
		public HashedString[] anims;
	}
}
