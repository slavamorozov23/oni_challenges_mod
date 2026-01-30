using System;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;

// Token: 0x020004E5 RID: 1253
public class BalloonArtist : GameStateMachine<BalloonArtist, BalloonArtist.Instance>
{
	// Token: 0x06001B17 RID: 6935 RVA: 0x00094B3C File Offset: 0x00092D3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<int>(this.balloonsGivenOut, this.overjoyed.exitEarly, (BalloonArtist.Instance smi, int p) => p >= TRAITS.JOY_REACTIONS.BALLOON_ARTIST.NUM_BALLOONS_TO_GIVE).Exit(delegate(BalloonArtist.Instance smi)
		{
			smi.numBalloonsGiven = 0;
			this.balloonsGivenOut.Set(0, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(BalloonArtist.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.balloon_stand);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistPlanning, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.balloon_stand, (BalloonArtist.Instance smi) => smi.IsRecTime());
		this.overjoyed.balloon_stand.ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistHandingOut, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.idle, (BalloonArtist.Instance smi) => !smi.IsRecTime()).ToggleChore((BalloonArtist.Instance smi) => new BalloonArtistChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(BalloonArtist.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x04000F94 RID: 3988
	public StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.IntParameter balloonsGivenOut;

	// Token: 0x04000F95 RID: 3989
	public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000F96 RID: 3990
	public BalloonArtist.OverjoyedStates overjoyed;

	// Token: 0x0200136D RID: 4973
	public class OverjoyedStates : GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006B1E RID: 27422
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04006B1F RID: 27423
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State balloon_stand;

		// Token: 0x04006B20 RID: 27424
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x0200136E RID: 4974
	public new class Instance : GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008BCF RID: 35791 RVA: 0x0035FE78 File Offset: 0x0035E078
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008BD0 RID: 35792 RVA: 0x0035FE81 File Offset: 0x0035E081
		[OnDeserialized]
		private void OnDeserialized()
		{
			base.smi.sm.balloonsGivenOut.Set(this.numBalloonsGiven, base.smi, false);
		}

		// Token: 0x06008BD1 RID: 35793 RVA: 0x0035FEA8 File Offset: 0x0035E0A8
		public void Internal_InitBalloons()
		{
			JoyResponseOutfitTarget joyResponseOutfitTarget = JoyResponseOutfitTarget.FromMinion(base.master.gameObject);
			if (!this.balloonSymbolIter.IsNullOrDestroyed())
			{
				if (this.balloonSymbolIter.facade.AndThen<string>((BalloonArtistFacadeResource f) => f.Id) == joyResponseOutfitTarget.ReadFacadeId())
				{
					return;
				}
			}
			this.balloonSymbolIter = joyResponseOutfitTarget.ReadFacadeId().AndThen<BalloonArtistFacadeResource>((string id) => Db.Get().Permits.BalloonArtistFacades.Get(id)).AndThen<BalloonOverrideSymbolIter>((BalloonArtistFacadeResource permit) => permit.GetSymbolIter()).UnwrapOr(new BalloonOverrideSymbolIter(Option.None), null);
			this.SetBalloonSymbolOverride(this.balloonSymbolIter.Current());
		}

		// Token: 0x06008BD2 RID: 35794 RVA: 0x0035FF95 File Offset: 0x0035E195
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06008BD3 RID: 35795 RVA: 0x0035FFB8 File Offset: 0x0035E1B8
		public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverrideSymbol)
		{
			if (balloonOverrideSymbol.animFile.IsNone())
			{
				base.master.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", Assets.GetAnim("balloon_anim_kanim").GetData().build.GetSymbol("body"), 0);
				return;
			}
			base.master.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", balloonOverrideSymbol.symbol.Unwrap(), 0);
		}

		// Token: 0x06008BD4 RID: 35796 RVA: 0x00360040 File Offset: 0x0035E240
		public BalloonOverrideSymbol GetCurrentBalloonSymbolOverride()
		{
			return this.balloonSymbolIter.Current();
		}

		// Token: 0x06008BD5 RID: 35797 RVA: 0x0036004D File Offset: 0x0035E24D
		public void ApplyNextBalloonSymbolOverride()
		{
			this.SetBalloonSymbolOverride(this.balloonSymbolIter.Next());
		}

		// Token: 0x06008BD6 RID: 35798 RVA: 0x00360060 File Offset: 0x0035E260
		public void GiveBalloon()
		{
			this.numBalloonsGiven++;
			base.smi.sm.balloonsGivenOut.Set(this.numBalloonsGiven, base.smi, false);
		}

		// Token: 0x06008BD7 RID: 35799 RVA: 0x00360094 File Offset: 0x0035E294
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}

		// Token: 0x04006B21 RID: 27425
		[Serialize]
		public int numBalloonsGiven;

		// Token: 0x04006B22 RID: 27426
		[NonSerialized]
		private BalloonOverrideSymbolIter balloonSymbolIter;

		// Token: 0x04006B23 RID: 27427
		private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

		// Token: 0x04006B24 RID: 27428
		private const int TARGET_OVERRIDE_PRIORITY = 0;
	}
}
