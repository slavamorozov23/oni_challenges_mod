using System;
using Database;
using UnityEngine;

// Token: 0x020006BD RID: 1725
public class BalloonFX : GameStateMachine<BalloonFX, BalloonFX.Instance>
{
	// Token: 0x06002A6C RID: 10860 RVA: 0x000F8420 File Offset: 0x000F6620
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.Exit("DestroyFX", delegate(BalloonFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x04001926 RID: 6438
	public StateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x04001927 RID: 6439
	public KAnimFile defaultAnim = Assets.GetAnim("balloon_anim_kanim");

	// Token: 0x04001928 RID: 6440
	private KAnimFile defaultBalloon = Assets.GetAnim("balloon_basic_red_kanim");

	// Token: 0x04001929 RID: 6441
	private const string defaultAnimName = "balloon_anim_kanim";

	// Token: 0x0400192A RID: 6442
	private const string balloonAnimName = "balloon_basic_red_kanim";

	// Token: 0x0400192B RID: 6443
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x0400192C RID: 6444
	private const int TARGET_OVERRIDE_PRIORITY = 0;

	// Token: 0x02001578 RID: 5496
	public new class Instance : GameStateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009369 RID: 37737 RVA: 0x00375B10 File Offset: 0x00373D10
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.balloonAnimController = FXHelpers.CreateEffectOverride(new string[]
			{
				"balloon_anim_kanim",
				"balloon_basic_red_kanim"
			}, master.gameObject.transform.GetPosition() + new Vector3(0f, 0.3f, 1f), master.transform, true, Grid.SceneLayer.Creatures, false);
			base.sm.fx.Set(this.balloonAnimController.gameObject, base.smi, false);
			this.balloonAnimController.defaultAnim = "idle_default";
			master.GetComponent<KBatchedAnimController>().GetSynchronizer().Add(this.balloonAnimController.GetComponent<KBatchedAnimController>());
		}

		// Token: 0x0600936A RID: 37738 RVA: 0x00375BC8 File Offset: 0x00373DC8
		public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverride)
		{
			KAnimFile kanimFile = balloonOverride.animFile.IsSome() ? balloonOverride.animFile.Unwrap() : base.smi.sm.defaultBalloon;
			this.balloonAnimController.SwapAnims(new KAnimFile[]
			{
				base.smi.sm.defaultAnim,
				kanimFile
			});
			SymbolOverrideController component = this.balloonAnimController.GetComponent<SymbolOverrideController>();
			if (this.currentBodyOverrideSymbol.IsSome())
			{
				component.RemoveSymbolOverride("body", 0);
			}
			if (balloonOverride.symbol.IsNone())
			{
				if (this.currentBodyOverrideSymbol.IsSome())
				{
					component.AddSymbolOverride("body", base.smi.sm.defaultAnim.GetData().build.GetSymbol("body"), 0);
				}
				this.balloonAnimController.SetBatchGroupOverride(HashedString.Invalid);
			}
			else
			{
				component.AddSymbolOverride("body", balloonOverride.symbol.Unwrap(), 0);
				this.balloonAnimController.SetBatchGroupOverride(kanimFile.batchTag);
			}
			this.currentBodyOverrideSymbol = balloonOverride;
		}

		// Token: 0x0600936B RID: 37739 RVA: 0x00375CF8 File Offset: 0x00373EF8
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x040071DB RID: 29147
		private KBatchedAnimController balloonAnimController;

		// Token: 0x040071DC RID: 29148
		private Option<BalloonOverrideSymbol> currentBodyOverrideSymbol;
	}
}
