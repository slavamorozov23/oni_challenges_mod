using System;

// Token: 0x020000D3 RID: 211
public class AnimInterruptStates : GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>
{
	// Token: 0x060003BF RID: 959 RVA: 0x00020229 File Offset: 0x0001E429
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.play_anim;
		this.play_anim.Enter(new StateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.State.Callback(this.PlayAnim)).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.PlayInterruptAnim, false);
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x00020268 File Offset: 0x0001E468
	private void PlayAnim(AnimInterruptStates.Instance smi)
	{
		KBatchedAnimController kbatchedAnimController = smi.Get<KBatchedAnimController>();
		HashedString[] anims = smi.GetSMI<AnimInterruptMonitor.Instance>().anims;
		kbatchedAnimController.Play(anims[0], KAnim.PlayMode.Once, 1f, 0f);
		for (int i = 1; i < anims.Length; i++)
		{
			kbatchedAnimController.Queue(anims[i], KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x040002EA RID: 746
	public GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.State play_anim;

	// Token: 0x040002EB RID: 747
	public GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.State behaviourcomplete;

	// Token: 0x020010CD RID: 4301
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020010CE RID: 4302
	public new class Instance : GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.GameInstance
	{
		// Token: 0x0600830B RID: 33547 RVA: 0x00342BA4 File Offset: 0x00340DA4
		public Instance(Chore<AnimInterruptStates.Instance> chore, AnimInterruptStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.PlayInterruptAnim);
		}
	}
}
