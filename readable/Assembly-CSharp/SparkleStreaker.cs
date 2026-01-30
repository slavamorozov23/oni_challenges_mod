using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020004E9 RID: 1257
public class SparkleStreaker : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance>
{
	// Token: 0x06001B28 RID: 6952 RVA: 0x00095320 File Offset: 0x00093520
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsSparkleStreaker").ToggleLoopingSound(this.soundPath, null, true, true, true).Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.sparkleStreakFX = Util.KInstantiate(EffectPrefabs.Instance.SparkleStreakFX, smi.master.transform.GetPosition() + this.offset);
			smi.sparkleStreakFX.transform.SetParent(smi.master.transform);
			smi.sparkleStreakFX.SetActive(true);
			smi.CreatePasserbyReactable();
		}).Exit(delegate(SparkleStreaker.Instance smi)
		{
			Util.KDestroyGameObject(smi.sparkleStreakFX);
			smi.ClearPasserbyReactable();
		});
		this.overjoyed.idle.Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.SetSparkleSoundParam(0f);
		}).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.moving, (SparkleStreaker.Instance smi) => smi.IsMoving());
		this.overjoyed.moving.Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.SetSparkleSoundParam(1f);
		}).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.idle, (SparkleStreaker.Instance smi) => !smi.IsMoving());
	}

	// Token: 0x04000FA3 RID: 4003
	private Vector3 offset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x04000FA4 RID: 4004
	public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000FA5 RID: 4005
	public SparkleStreaker.OverjoyedStates overjoyed;

	// Token: 0x04000FA6 RID: 4006
	public string soundPath = GlobalAssets.GetSound("SparkleStreaker_lp", false);

	// Token: 0x04000FA7 RID: 4007
	public HashedString SPARKLE_STREAKER_MOVING_PARAMETER = "sparkleStreaker_moving";

	// Token: 0x02001379 RID: 4985
	public class OverjoyedStates : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006B47 RID: 27463
		public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04006B48 RID: 27464
		public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State moving;
	}

	// Token: 0x0200137A RID: 4986
	public new class Instance : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C05 RID: 35845 RVA: 0x0036045B File Offset: 0x0035E65B
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008C06 RID: 35846 RVA: 0x00360464 File Offset: 0x0035E664
		public void CreatePasserbyReactable()
		{
			if (this.passerbyReactable == null)
			{
				EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 0f, 600f, float.PositiveInfinity, 0f);
				Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
				emoteReactable.SetEmote(clapCheer).SetThought(Db.Get().Thoughts.Happy).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
				emoteReactable.RegisterEmoteStepCallbacks("clapcheer_pre", new Action<GameObject>(this.AddReactionEffect), null);
				this.passerbyReactable = emoteReactable;
			}
		}

		// Token: 0x06008C07 RID: 35847 RVA: 0x0036051E File Offset: 0x0035E71E
		private void AddReactionEffect(GameObject reactor)
		{
			reactor.GetComponent<Effects>().Add("SawSparkleStreaker", true);
		}

		// Token: 0x06008C08 RID: 35848 RVA: 0x00360532 File Offset: 0x0035E732
		private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
		{
			return transition.end == NavType.Floor;
		}

		// Token: 0x06008C09 RID: 35849 RVA: 0x0036053D File Offset: 0x0035E73D
		public void ClearPasserbyReactable()
		{
			if (this.passerbyReactable != null)
			{
				this.passerbyReactable.Cleanup();
				this.passerbyReactable = null;
			}
		}

		// Token: 0x06008C0A RID: 35850 RVA: 0x00360559 File Offset: 0x0035E759
		public bool IsMoving()
		{
			return base.smi.master.GetComponent<Navigator>().IsMoving();
		}

		// Token: 0x06008C0B RID: 35851 RVA: 0x00360570 File Offset: 0x0035E770
		public void SetSparkleSoundParam(float val)
		{
			base.GetComponent<LoopingSounds>().SetParameter(GlobalAssets.GetSound("SparkleStreaker_lp", false), "sparkleStreaker_moving", val);
		}

		// Token: 0x04006B49 RID: 27465
		private Reactable passerbyReactable;

		// Token: 0x04006B4A RID: 27466
		public GameObject sparkleStreakFX;
	}
}
