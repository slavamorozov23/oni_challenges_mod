using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001030 RID: 4144
	[SerializationConfig(MemberSerialization.OptIn)]
	public class SicknessInstance : ModifierInstance<Sickness>, ISaveLoadable
	{
		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x0600809C RID: 32924 RVA: 0x0033A6E0 File Offset: 0x003388E0
		public Sickness Sickness
		{
			get
			{
				return this.modifier;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x0600809D RID: 32925 RVA: 0x0033A6E8 File Offset: 0x003388E8
		public float TotalCureSpeedMultiplier
		{
			get
			{
				AttributeInstance attributeInstance = Db.Get().Attributes.DiseaseCureSpeed.Lookup(this.smi.master.gameObject);
				AttributeInstance attributeInstance2 = this.modifier.cureSpeedBase.Lookup(this.smi.master.gameObject);
				float num = 1f;
				if (attributeInstance != null)
				{
					num *= attributeInstance.GetTotalValue();
				}
				if (attributeInstance2 != null)
				{
					num *= attributeInstance2.GetTotalValue();
				}
				return num;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x0600809E RID: 32926 RVA: 0x0033A75C File Offset: 0x0033895C
		public bool IsDoctored
		{
			get
			{
				if (base.gameObject == null)
				{
					return false;
				}
				AttributeInstance attributeInstance = Db.Get().Attributes.DoctoredLevel.Lookup(base.gameObject);
				return attributeInstance != null && attributeInstance.GetTotalValue() > 0f;
			}
		}

		// Token: 0x0600809F RID: 32927 RVA: 0x0033A7A7 File Offset: 0x003389A7
		public SicknessInstance(GameObject game_object, Sickness disease) : base(game_object, disease)
		{
		}

		// Token: 0x060080A0 RID: 32928 RVA: 0x0033A7B1 File Offset: 0x003389B1
		[OnDeserialized]
		private void OnDeserialized()
		{
			this.InitializeAndStart();
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x060080A1 RID: 32929 RVA: 0x0033A7B9 File Offset: 0x003389B9
		// (set) Token: 0x060080A2 RID: 32930 RVA: 0x0033A7C1 File Offset: 0x003389C1
		public SicknessExposureInfo ExposureInfo
		{
			get
			{
				return this.exposureInfo;
			}
			set
			{
				this.exposureInfo = value;
				this.InitializeAndStart();
			}
		}

		// Token: 0x060080A3 RID: 32931 RVA: 0x0033A7D0 File Offset: 0x003389D0
		private void InitializeAndStart()
		{
			Sickness disease = this.modifier;
			Func<List<Notification>, object, string> tooltip = delegate(List<Notification> notificationList, object data)
			{
				string text = "";
				for (int i = 0; i < notificationList.Count; i++)
				{
					Notification notification = notificationList[i];
					string arg = (string)notification.tooltipData;
					text += string.Format(DUPLICANTS.DISEASES.NOTIFICATION_TOOLTIP, notification.NotifierName, disease.Name, arg);
					if (i < notificationList.Count - 1)
					{
						text += "\n";
					}
				}
				return text;
			};
			string name = disease.Name;
			string title = name;
			NotificationType type = (disease.severity <= Sickness.Severity.Minor) ? NotificationType.BadMinor : NotificationType.Bad;
			object sourceInfo = this.exposureInfo.sourceInfo;
			this.notification = new Notification(title, type, tooltip, sourceInfo, true, 0f, null, null, null, true, false, false);
			this.statusItem = new StatusItem(disease.Id, disease.Name, DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.TEMPLATE, "", (disease.severity <= Sickness.Severity.Minor) ? StatusItem.IconType.Info : StatusItem.IconType.Exclamation, (disease.severity <= Sickness.Severity.Minor) ? NotificationType.BadMinor : NotificationType.Bad, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItem.resolveTooltipCallback = new Func<string, object, string>(this.ResolveString);
			if (this.smi != null)
			{
				this.smi.StopSM("refresh");
			}
			this.smi = new SicknessInstance.StatesInstance(this);
			this.smi.StartSM();
		}

		// Token: 0x060080A4 RID: 32932 RVA: 0x0033A8E8 File Offset: 0x00338AE8
		private string ResolveString(string str, object data)
		{
			if (this.smi == null)
			{
				global::Debug.LogWarning("Attempting to resolve string when smi is null");
				return str;
			}
			KSelectable component = base.gameObject.GetComponent<KSelectable>();
			str = str.Replace("{Descriptor}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DESCRIPTOR, Strings.Get("STRINGS.DUPLICANTS.DISEASES.SEVERITY." + this.modifier.severity.ToString().ToUpper()), Strings.Get("STRINGS.DUPLICANTS.DISEASES.TYPE." + this.modifier.sicknessType.ToString().ToUpper())));
			str = str.Replace("{Infectee}", component.GetProperName());
			str = str.Replace("{InfectionSource}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.INFECTION_SOURCE, this.exposureInfo.sourceInfo));
			if (this.modifier.severity <= Sickness.Severity.Minor)
			{
				str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
			}
			else if (this.modifier.severity == Sickness.Severity.Major)
			{
				str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
				if (!this.IsDoctored)
				{
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.BEDREST);
				}
				else
				{
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTORED);
				}
			}
			else if (this.modifier.severity >= Sickness.Severity.Critical)
			{
				if (!this.IsDoctored)
				{
					str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.FATALITY, GameUtil.GetFormattedCycles(this.GetFatalityTimeRemaining(), "F1", false)));
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTOR_REQUIRED);
				}
				else
				{
					str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTORED);
				}
			}
			List<Descriptor> symptoms = this.modifier.GetSymptoms(this.smi.gameObject);
			string text = "";
			foreach (Descriptor descriptor in symptoms)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text = text + "    • " + descriptor.text;
			}
			str = str.Replace("{Symptoms}", text);
			str = Regex.Replace(str, "{[^}]*}", "");
			return str;
		}

		// Token: 0x060080A5 RID: 32933 RVA: 0x0033ABBC File Offset: 0x00338DBC
		public float GetInfectedTimeRemaining()
		{
			return this.modifier.SicknessDuration * (1f - this.smi.sm.percentRecovered.Get(this.smi)) / this.TotalCureSpeedMultiplier;
		}

		// Token: 0x060080A6 RID: 32934 RVA: 0x0033ABF2 File Offset: 0x00338DF2
		public float GetFatalityTimeRemaining()
		{
			return this.modifier.fatalityDuration * (1f - this.smi.sm.percentDied.Get(this.smi));
		}

		// Token: 0x060080A7 RID: 32935 RVA: 0x0033AC21 File Offset: 0x00338E21
		public float GetPercentCured()
		{
			if (this.smi == null)
			{
				return 0f;
			}
			return this.smi.sm.percentRecovered.Get(this.smi);
		}

		// Token: 0x060080A8 RID: 32936 RVA: 0x0033AC4C File Offset: 0x00338E4C
		public void SetPercentCured(float pct)
		{
			this.smi.sm.percentRecovered.Set(pct, this.smi, false);
		}

		// Token: 0x060080A9 RID: 32937 RVA: 0x0033AC6C File Offset: 0x00338E6C
		public void Cure()
		{
			this.smi.Cure();
		}

		// Token: 0x060080AA RID: 32938 RVA: 0x0033AC79 File Offset: 0x00338E79
		public override void OnCleanUp()
		{
			if (this.smi != null)
			{
				this.smi.StopSM("DiseaseInstance.OnCleanUp");
				this.smi = null;
			}
		}

		// Token: 0x060080AB RID: 32939 RVA: 0x0033AC9A File Offset: 0x00338E9A
		public StatusItem GetStatusItem()
		{
			return this.statusItem;
		}

		// Token: 0x060080AC RID: 32940 RVA: 0x0033ACA2 File Offset: 0x00338EA2
		public List<Descriptor> GetDescriptors()
		{
			return this.modifier.GetSicknessSourceDescriptors();
		}

		// Token: 0x04006175 RID: 24949
		[Serialize]
		private SicknessExposureInfo exposureInfo;

		// Token: 0x04006176 RID: 24950
		private SicknessInstance.StatesInstance smi;

		// Token: 0x04006177 RID: 24951
		private StatusItem statusItem;

		// Token: 0x04006178 RID: 24952
		private Notification notification;

		// Token: 0x0200272B RID: 10027
		private struct CureInfo
		{
			// Token: 0x0400AE79 RID: 44665
			public string name;

			// Token: 0x0400AE7A RID: 44666
			public float multiplier;
		}

		// Token: 0x0200272C RID: 10028
		public class StatesInstance : GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.GameInstance
		{
			// Token: 0x0600C80C RID: 51212 RVA: 0x004250B6 File Offset: 0x004232B6
			public StatesInstance(SicknessInstance master) : base(master)
			{
			}

			// Token: 0x0600C80D RID: 51213 RVA: 0x004250C0 File Offset: 0x004232C0
			public void UpdateProgress(float dt)
			{
				float delta_value = dt * base.master.TotalCureSpeedMultiplier / base.master.modifier.SicknessDuration;
				base.sm.percentRecovered.Delta(delta_value, base.smi);
				if (base.master.modifier.fatalityDuration > 0f)
				{
					if (!base.master.IsDoctored)
					{
						float delta_value2 = dt / base.master.modifier.fatalityDuration;
						base.sm.percentDied.Delta(delta_value2, base.smi);
						return;
					}
					base.sm.percentDied.Set(0f, base.smi, false);
				}
			}

			// Token: 0x0600C80E RID: 51214 RVA: 0x00425174 File Offset: 0x00423374
			public void Infect()
			{
				Sickness modifier = base.master.modifier;
				this.componentData = modifier.Infect(base.gameObject, base.master, base.master.exposureInfo);
				if (PopFXManager.Instance != null)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, string.Format(DUPLICANTS.DISEASES.INFECTED_POPUP, modifier.Name), base.gameObject.transform, 1.5f, true);
				}
			}

			// Token: 0x0600C80F RID: 51215 RVA: 0x004251F8 File Offset: 0x004233F8
			public void Cure()
			{
				Sickness modifier = base.master.modifier;
				base.gameObject.GetComponent<Modifiers>().sicknesses.Cure(modifier);
				modifier.Cure(base.gameObject, this.componentData);
				if (PopFXManager.Instance != null)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, string.Format(DUPLICANTS.DISEASES.CURED_POPUP, modifier.Name), base.gameObject.transform, 1.5f, true);
				}
				if (!string.IsNullOrEmpty(modifier.recoveryEffect))
				{
					Effects component = base.gameObject.GetComponent<Effects>();
					if (component)
					{
						component.Add(modifier.recoveryEffect, true);
					}
				}
			}

			// Token: 0x0600C810 RID: 51216 RVA: 0x004252B1 File Offset: 0x004234B1
			public SicknessExposureInfo GetExposureInfo()
			{
				return base.master.ExposureInfo;
			}

			// Token: 0x0400AE7B RID: 44667
			private object[] componentData;
		}

		// Token: 0x0200272D RID: 10029
		public class States : GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance>
		{
			// Token: 0x0600C811 RID: 51217 RVA: 0x004252C0 File Offset: 0x004234C0
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.infected;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.infected.Enter("Infect", delegate(SicknessInstance.StatesInstance smi)
				{
					smi.Infect();
				}).DoNotification((SicknessInstance.StatesInstance smi) => smi.master.notification).Update("UpdateProgress", delegate(SicknessInstance.StatesInstance smi, float dt)
				{
					smi.UpdateProgress(dt);
				}, UpdateRate.SIM_200ms, false).ToggleStatusItem((SicknessInstance.StatesInstance smi) => smi.master.GetStatusItem(), (SicknessInstance.StatesInstance smi) => smi).ParamTransition<float>(this.percentRecovered, this.cured, GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.IsGTOne).ParamTransition<float>(this.percentDied, this.fatality_pre, GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.IsGTOne);
				this.cured.Enter("Cure", delegate(SicknessInstance.StatesInstance smi)
				{
					smi.master.Cure();
				});
				this.fatality_pre.Update("DeathByDisease", delegate(SicknessInstance.StatesInstance smi, float dt)
				{
					DeathMonitor.Instance smi2 = smi.master.gameObject.GetSMI<DeathMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.Kill(Db.Get().Deaths.FatalDisease);
						smi.GoTo(this.fatality);
					}
				}, UpdateRate.SIM_200ms, false);
				this.fatality.DoNothing();
			}

			// Token: 0x0400AE7C RID: 44668
			public StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.FloatParameter percentRecovered;

			// Token: 0x0400AE7D RID: 44669
			public StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.FloatParameter percentDied;

			// Token: 0x0400AE7E RID: 44670
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State infected;

			// Token: 0x0400AE7F RID: 44671
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State cured;

			// Token: 0x0400AE80 RID: 44672
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State fatality_pre;

			// Token: 0x0400AE81 RID: 44673
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State fatality;
		}
	}
}
