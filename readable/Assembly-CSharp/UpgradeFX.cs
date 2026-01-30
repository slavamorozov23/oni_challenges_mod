using System;
using UnityEngine;

// Token: 0x020006C6 RID: 1734
public class UpgradeFX : GameStateMachine<UpgradeFX, UpgradeFX.Instance>
{
	// Token: 0x06002A84 RID: 10884 RVA: 0x000F92B4 File Offset: 0x000F74B4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("upgrade").OnAnimQueueComplete(null).ToggleReactable((UpgradeFX.Instance smi) => smi.CreateReactable()).Exit("DestroyFX", delegate(UpgradeFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x0400194B RID: 6475
	public StateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x0200158A RID: 5514
	public new class Instance : GameStateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060093AB RID: 37803 RVA: 0x00376668 File Offset: 0x00374868
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("upgrade_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x060093AC RID: 37804 RVA: 0x003766CA File Offset: 0x003748CA
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x060093AD RID: 37805 RVA: 0x003766E8 File Offset: 0x003748E8
		public Reactable CreateReactable()
		{
			return new EmoteReactable(base.master.gameObject, "UpgradeFX", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 20f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Cheer);
		}
	}
}
