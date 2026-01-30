using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000BE0 RID: 3040
[SkipSaveFileSerialization]
public class Stinky : StateMachineComponent<Stinky.StatesInstance>
{
	// Token: 0x06005B02 RID: 23298 RVA: 0x0020F6F9 File Offset: 0x0020D8F9
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06005B03 RID: 23299 RVA: 0x0020F708 File Offset: 0x0020D908
	private void Emit(object data)
	{
		GameObject gameObject = (GameObject)data;
		Components.Cmps<MinionIdentity> liveMinionIdentities = Components.LiveMinionIdentities;
		Vector2 a = gameObject.transform.GetPosition();
		for (int i = 0; i < liveMinionIdentities.Count; i++)
		{
			MinionIdentity minionIdentity = liveMinionIdentities[i];
			if (minionIdentity.gameObject != gameObject.gameObject)
			{
				Vector2 b = minionIdentity.transform.GetPosition();
				if (Vector2.SqrMagnitude(a - b) <= 2.25f)
				{
					minionIdentity.Trigger(508119890, Strings.Get("STRINGS.DUPLICANTS.DISEASES.PUTRIDODOUR.CRINGE_EFFECT").String);
					minionIdentity.GetComponent<Effects>().Add("SmelledStinky", true);
					minionIdentity.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.PutridOdour);
				}
			}
		}
		int gameCell = Grid.PosToCell(gameObject.transform.GetPosition());
		float value = Db.Get().Amounts.Temperature.Lookup(this).value;
		SimMessages.AddRemoveSubstance(gameCell, SimHashes.ContaminatedOxygen, CellEventLogger.Instance.ElementConsumerSimUpdate, 0.0025000002f, value, byte.MaxValue, 0, true, -1);
		GameObject gameObject2 = gameObject;
		bool flag = SoundEvent.ObjectIsSelectedAndVisible(gameObject2);
		Vector3 vector = gameObject2.transform.GetPosition();
		float volume = 1f;
		if (flag)
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
			volume = SoundEvent.GetVolume(flag);
		}
		else
		{
			vector.z = 0f;
		}
		KFMOD.PlayOneShot(GlobalAssets.GetSound("Dupe_Flatulence", false), vector, volume);
	}

	// Token: 0x04003CAD RID: 15533
	private const float EmitMass = 0.0025000002f;

	// Token: 0x04003CAE RID: 15534
	private const SimHashes EmitElement = SimHashes.ContaminatedOxygen;

	// Token: 0x04003CAF RID: 15535
	private const float EmissionRadius = 1.5f;

	// Token: 0x04003CB0 RID: 15536
	private const float MaxDistanceSq = 2.25f;

	// Token: 0x04003CB1 RID: 15537
	private KBatchedAnimController stinkyController;

	// Token: 0x04003CB2 RID: 15538
	private static readonly HashedString[] WorkLoopAnims = new HashedString[]
	{
		"working_pre",
		"working_loop",
		"working_pst"
	};

	// Token: 0x02001D71 RID: 7537
	public class StatesInstance : GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.GameInstance
	{
		// Token: 0x0600B130 RID: 45360 RVA: 0x003DC8D3 File Offset: 0x003DAAD3
		public StatesInstance(Stinky master) : base(master)
		{
		}
	}

	// Token: 0x02001D72 RID: 7538
	public class States : GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky>
	{
		// Token: 0x0600B131 RID: 45361 RVA: 0x003DC8DC File Offset: 0x003DAADC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false).Enter(delegate(Stinky.StatesInstance smi)
			{
				KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("odor_fx_kanim", smi.master.gameObject.transform.GetPosition(), smi.master.gameObject.transform, true, Grid.SceneLayer.Front, false);
				kbatchedAnimController.Play(Stinky.WorkLoopAnims, KAnim.PlayMode.Once);
				smi.master.stinkyController = kbatchedAnimController;
			}).Update("StinkyFX", delegate(Stinky.StatesInstance smi, float dt)
			{
				if (smi.master.stinkyController != null)
				{
					smi.master.stinkyController.Play(Stinky.WorkLoopAnims, KAnim.PlayMode.Once);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.idle.Enter("ScheduleNextFart", delegate(Stinky.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(), this.emit);
			});
			this.emit.Enter("Fart", delegate(Stinky.StatesInstance smi)
			{
				smi.master.Emit(smi.master.gameObject);
			}).ToggleExpression(Db.Get().Expressions.Relief, null).ScheduleGoTo(3f, this.idle);
		}

		// Token: 0x0600B132 RID: 45362 RVA: 0x003DC9C5 File Offset: 0x003DABC5
		private float GetNewInterval()
		{
			return Mathf.Min(Mathf.Max(Util.GaussianRandom(TRAITS.STINKY_EMIT_INTERVAL_MAX - TRAITS.STINKY_EMIT_INTERVAL_MIN, 1f), TRAITS.STINKY_EMIT_INTERVAL_MIN), TRAITS.STINKY_EMIT_INTERVAL_MAX);
		}

		// Token: 0x04008B51 RID: 35665
		public GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State idle;

		// Token: 0x04008B52 RID: 35666
		public GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State emit;
	}
}
