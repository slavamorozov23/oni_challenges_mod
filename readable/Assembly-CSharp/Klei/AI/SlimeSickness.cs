using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001033 RID: 4147
	public class SlimeSickness : Sickness
	{
		// Token: 0x060080B5 RID: 32949 RVA: 0x0033B250 File Offset: 0x00339450
		public SlimeSickness() : base("SlimeSickness", Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.00025f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Inhalation
		}, 2220f, "SlimeSicknessRecovery")
		{
			base.AddSicknessComponent(new CommonSickEffectSickness());
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier("Athletics", -3f, DUPLICANTS.DISEASES.SLIMESICKNESS.NAME, false, false, true)
			}));
			base.AddSicknessComponent(new AttributeModifierSickness(MinionConfig.ID, new AttributeModifier[]
			{
				new AttributeModifier("BreathDelta", DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE * -1.25f, DUPLICANTS.DISEASES.SLIMESICKNESS.NAME, false, false, true)
			}));
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_sick_kanim"
			}, Db.Get().Expressions.Sick));
			base.AddSicknessComponent(new PeriodicEmoteSickness(Db.Get().Emotes.Minion.Sick, 50f));
			base.AddSicknessComponent(new SlimeSickness.SlimeLungComponent());
		}

		// Token: 0x0400617C RID: 24956
		private const float COUGH_FREQUENCY = 20f;

		// Token: 0x0400617D RID: 24957
		private const float COUGH_MASS = 0.1f;

		// Token: 0x0400617E RID: 24958
		private const int DISEASE_AMOUNT = 1000;

		// Token: 0x0400617F RID: 24959
		public const string ID = "SlimeSickness";

		// Token: 0x04006180 RID: 24960
		public const string RECOVERY_ID = "SlimeSicknessRecovery";

		// Token: 0x0200272F RID: 10031
		public class SlimeLungComponent : Sickness.SicknessComponent
		{
			// Token: 0x0600C816 RID: 51222 RVA: 0x004254F4 File Offset: 0x004236F4
			public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
			{
				SlimeSickness.SlimeLungComponent.StatesInstance statesInstance = new SlimeSickness.SlimeLungComponent.StatesInstance(diseaseInstance);
				statesInstance.StartSM();
				return statesInstance;
			}

			// Token: 0x0600C817 RID: 51223 RVA: 0x00425502 File Offset: 0x00423702
			public override void OnCure(GameObject go, object instance_data)
			{
				((SlimeSickness.SlimeLungComponent.StatesInstance)instance_data).StopSM("Cured");
			}

			// Token: 0x0600C818 RID: 51224 RVA: 0x00425514 File Offset: 0x00423714
			public override List<Descriptor> GetSymptoms()
			{
				return new List<Descriptor>
				{
					new Descriptor(DUPLICANTS.DISEASES.SLIMESICKNESS.COUGH_SYMPTOM, DUPLICANTS.DISEASES.SLIMESICKNESS.COUGH_SYMPTOM_TOOLTIP, Descriptor.DescriptorType.SymptomAidable, false)
				};
			}

			// Token: 0x02003A1E RID: 14878
			public class StatesInstance : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.GameInstance
			{
				// Token: 0x0600F367 RID: 62311 RVA: 0x00494C36 File Offset: 0x00492E36
				public StatesInstance(SicknessInstance master) : base(master)
				{
				}

				// Token: 0x0600F368 RID: 62312 RVA: 0x00494C40 File Offset: 0x00492E40
				public Reactable GetReactable()
				{
					Emote cough = Db.Get().Emotes.Minion.Cough;
					SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "SlimeLungCough", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
					selfEmoteReactable.SetEmote(cough);
					selfEmoteReactable.RegisterEmoteStepCallbacks("react", null, new Action<GameObject>(this.FinishedCoughing));
					return selfEmoteReactable;
				}

				// Token: 0x0600F369 RID: 62313 RVA: 0x00494CC8 File Offset: 0x00492EC8
				private void ProduceSlime(GameObject cougher)
				{
					AmountInstance amountInstance = Db.Get().Amounts.Temperature.Lookup(cougher);
					int gameCell = Grid.PosToCell(cougher);
					string id = Db.Get().Diseases.SlimeGerms.Id;
					Equippable equippable = base.master.gameObject.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
					if (equippable != null)
					{
						equippable.GetComponent<Storage>().AddGasChunk(SimHashes.ContaminatedOxygen, 0.1f, amountInstance.value, Db.Get().Diseases.GetIndex(id), 1000, false, true);
					}
					else
					{
						SimMessages.AddRemoveSubstance(gameCell, SimHashes.ContaminatedOxygen, CellEventLogger.Instance.Cough, 0.1f, amountInstance.value, Db.Get().Diseases.GetIndex(id), 1000, true, -1);
					}
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, base.master.modifier.Name, 1000), cougher.transform, 1.5f, false);
				}

				// Token: 0x0600F36A RID: 62314 RVA: 0x00494DE6 File Offset: 0x00492FE6
				private void FinishedCoughing(GameObject cougher)
				{
					this.ProduceSlime(cougher);
					base.sm.coughFinished.Trigger(this);
				}

				// Token: 0x0400EB12 RID: 60178
				public float lastCoughTime;
			}

			// Token: 0x02003A1F RID: 14879
			public class States : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance>
			{
				// Token: 0x0600F36B RID: 62315 RVA: 0x00494E00 File Offset: 0x00493000
				public override void InitializeStates(out StateMachine.BaseState default_state)
				{
					default_state = this.breathing;
					this.breathing.DefaultState(this.breathing.normal).TagTransition(GameTags.NoOxygen, this.notbreathing, false);
					this.breathing.normal.Enter("SetCoughTime", delegate(SlimeSickness.SlimeLungComponent.StatesInstance smi)
					{
						if (smi.lastCoughTime < Time.time)
						{
							smi.lastCoughTime = Time.time;
						}
					}).Update("Cough", delegate(SlimeSickness.SlimeLungComponent.StatesInstance smi, float dt)
					{
						if (!smi.master.IsDoctored && Time.time - smi.lastCoughTime > 20f)
						{
							smi.GoTo(this.breathing.cough);
						}
					}, UpdateRate.SIM_4000ms, false);
					this.breathing.cough.ToggleReactable((SlimeSickness.SlimeLungComponent.StatesInstance smi) => smi.GetReactable()).OnSignal(this.coughFinished, this.breathing.normal);
					this.notbreathing.TagTransition(new Tag[]
					{
						GameTags.NoOxygen
					}, this.breathing, true);
				}

				// Token: 0x0400EB13 RID: 60179
				public StateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.Signal coughFinished;

				// Token: 0x0400EB14 RID: 60180
				public SlimeSickness.SlimeLungComponent.States.BreathingStates breathing;

				// Token: 0x0400EB15 RID: 60181
				public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State notbreathing;

				// Token: 0x02003DB5 RID: 15797
				public class BreathingStates : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State
				{
					// Token: 0x0400F435 RID: 62517
					public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State normal;

					// Token: 0x0400F436 RID: 62518
					public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State cough;
				}
			}
		}
	}
}
