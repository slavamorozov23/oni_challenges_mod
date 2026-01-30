using System;
using TUNING;
using UnityEngine;

// Token: 0x02000958 RID: 2392
[SkipSaveFileSerialization]
public class Flatulence : StateMachineComponent<Flatulence.StatesInstance>
{
	// Token: 0x060042DC RID: 17116 RVA: 0x00179FF9 File Offset: 0x001781F9
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x060042DD RID: 17117 RVA: 0x0017A008 File Offset: 0x00178208
	private void Emit(object data)
	{
		GameObject gameObject = (GameObject)data;
		float value = Db.Get().Amounts.Temperature.Lookup(this).value;
		Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
		if (equippable != null)
		{
			equippable.GetComponent<Storage>().AddGasChunk(SimHashes.Methane, 0.1f, value, byte.MaxValue, 0, false, true);
		}
		else
		{
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
						minionIdentity.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.PutridOdour);
					}
				}
			}
			SimMessages.AddRemoveSubstance(Grid.PosToCell(gameObject.transform.GetPosition()), SimHashes.Methane, CellEventLogger.Instance.ElementConsumerSimUpdate, 0.1f, value, byte.MaxValue, 0, true, -1);
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("odor_fx_kanim", gameObject.transform.GetPosition(), gameObject.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play(Flatulence.WorkLoopAnims, KAnim.PlayMode.Once);
			kbatchedAnimController.destroyOnAnimComplete = true;
		}
		GameObject gameObject2 = gameObject;
		bool flag = SoundEvent.ObjectIsSelectedAndVisible(gameObject2);
		Vector3 vector = gameObject2.transform.GetPosition();
		vector.z = 0f;
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

	// Token: 0x04002A07 RID: 10759
	private const float EmitMass = 0.1f;

	// Token: 0x04002A08 RID: 10760
	private const SimHashes EmitElement = SimHashes.Methane;

	// Token: 0x04002A09 RID: 10761
	private const float EmissionRadius = 1.5f;

	// Token: 0x04002A0A RID: 10762
	private const float MaxDistanceSq = 2.25f;

	// Token: 0x04002A0B RID: 10763
	private static readonly HashedString[] WorkLoopAnims = new HashedString[]
	{
		"working_pre",
		"working_loop",
		"working_pst"
	};

	// Token: 0x02001948 RID: 6472
	public class StatesInstance : GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.GameInstance
	{
		// Token: 0x0600A1D1 RID: 41425 RVA: 0x003ACE26 File Offset: 0x003AB026
		public StatesInstance(Flatulence master) : base(master)
		{
		}
	}

	// Token: 0x02001949 RID: 6473
	public class States : GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence>
	{
		// Token: 0x0600A1D2 RID: 41426 RVA: 0x003ACE30 File Offset: 0x003AB030
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Enter("ScheduleNextFart", delegate(Flatulence.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(), this.emit);
			});
			this.emit.Enter("Fart", delegate(Flatulence.StatesInstance smi)
			{
				smi.master.Emit(smi.master.gameObject);
			}).ToggleExpression(Db.Get().Expressions.Relief, null).ScheduleGoTo(3f, this.idle);
		}

		// Token: 0x0600A1D3 RID: 41427 RVA: 0x003ACECA File Offset: 0x003AB0CA
		private float GetNewInterval()
		{
			return Mathf.Min(Mathf.Max(Util.GaussianRandom(TRAITS.FLATULENCE_EMIT_INTERVAL_MAX - TRAITS.FLATULENCE_EMIT_INTERVAL_MIN, 1f), TRAITS.FLATULENCE_EMIT_INTERVAL_MIN), TRAITS.FLATULENCE_EMIT_INTERVAL_MAX);
		}

		// Token: 0x04007D63 RID: 32099
		public GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State idle;

		// Token: 0x04007D64 RID: 32100
		public GameStateMachine<Flatulence.States, Flatulence.StatesInstance, Flatulence, object>.State emit;
	}
}
