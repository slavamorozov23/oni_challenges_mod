using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200102F RID: 4143
	[DebuggerDisplay("{base.Id}")]
	public abstract class Sickness : Resource
	{
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06008090 RID: 32912 RVA: 0x0033A2F9 File Offset: 0x003384F9
		public new string Name
		{
			get
			{
				return Strings.Get(this.name);
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06008091 RID: 32913 RVA: 0x0033A30B File Offset: 0x0033850B
		public float SicknessDuration
		{
			get
			{
				return this.sicknessDuration;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06008092 RID: 32914 RVA: 0x0033A313 File Offset: 0x00338513
		public StringKey DescriptiveSymptoms
		{
			get
			{
				return this.descriptiveSymptoms;
			}
		}

		// Token: 0x06008093 RID: 32915 RVA: 0x0033A31C File Offset: 0x0033851C
		public Sickness(string id, Sickness.SicknessType type, Sickness.Severity severity, float immune_attack_strength, List<Sickness.InfectionVector> infection_vectors, float sickness_duration, string recovery_effect = null) : base(id, null, null)
		{
			this.name = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".NAME");
			this.id = id;
			this.sicknessType = type;
			this.severity = severity;
			this.infectionVectors = infection_vectors;
			this.sicknessDuration = sickness_duration;
			this.recoveryEffect = recovery_effect;
			this.descriptiveSymptoms = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".DESCRIPTIVE_SYMPTOMS");
			this.cureSpeedBase = new Attribute(id + "CureSpeed", false, Attribute.Display.Normal, false, 0f, null, null, null, null);
			this.cureSpeedBase.BaseValue = 1f;
			this.cureSpeedBase.SetFormatter(new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
			Db.Get().Attributes.Add(this.cureSpeedBase);
		}

		// Token: 0x06008094 RID: 32916 RVA: 0x0033A418 File Offset: 0x00338618
		public object[] Infect(GameObject go, SicknessInstance diseaseInstance, SicknessExposureInfo exposure_info)
		{
			object[] array = new object[this.components.Count];
			for (int i = 0; i < this.components.Count; i++)
			{
				array[i] = this.components[i].OnInfect(go, diseaseInstance);
			}
			return array;
		}

		// Token: 0x06008095 RID: 32917 RVA: 0x0033A464 File Offset: 0x00338664
		public void Cure(GameObject go, object[] componentData)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				this.components[i].OnCure(go, componentData[i]);
			}
		}

		// Token: 0x06008096 RID: 32918 RVA: 0x0033A49C File Offset: 0x0033869C
		public List<Descriptor> GetSymptoms()
		{
			return this.GetSymptoms(null);
		}

		// Token: 0x06008097 RID: 32919 RVA: 0x0033A4A8 File Offset: 0x003386A8
		public List<Descriptor> GetSymptoms(GameObject victim)
		{
			List<Descriptor> list = new List<Descriptor>();
			for (int i = 0; i < this.components.Count; i++)
			{
				List<Descriptor> symptoms = this.components[i].GetSymptoms(victim);
				if (symptoms != null && symptoms.Count > 0)
				{
					list.AddRange(symptoms);
				}
			}
			if (this.fatalityDuration > 0f)
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.DEATH_SYMPTOM, GameUtil.GetFormattedCycles(this.fatalityDuration, "F1", false)), string.Format(DUPLICANTS.DISEASES.DEATH_SYMPTOM_TOOLTIP, GameUtil.GetFormattedCycles(this.fatalityDuration, "F1", false)), Descriptor.DescriptorType.SymptomAidable, false));
			}
			return list;
		}

		// Token: 0x06008098 RID: 32920 RVA: 0x0033A552 File Offset: 0x00338752
		protected void AddSicknessComponent(Sickness.SicknessComponent cmp)
		{
			this.components.Add(cmp);
		}

		// Token: 0x06008099 RID: 32921 RVA: 0x0033A560 File Offset: 0x00338760
		public T GetSicknessComponent<T>() where T : Sickness.SicknessComponent
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i] is T)
				{
					return this.components[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x0600809A RID: 32922 RVA: 0x0033A5B6 File Offset: 0x003387B6
		public virtual List<Descriptor> GetSicknessSourceDescriptors()
		{
			return new List<Descriptor>();
		}

		// Token: 0x0600809B RID: 32923 RVA: 0x0033A5C0 File Offset: 0x003387C0
		public List<Descriptor> GetQualitativeDescriptors()
		{
			List<Descriptor> list = new List<Descriptor>();
			using (List<Sickness.InfectionVector>.Enumerator enumerator = this.infectionVectors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case Sickness.InfectionVector.Contact:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Digestion:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Inhalation:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Exposure:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					}
				}
			}
			list.Add(new Descriptor(Strings.Get(this.descriptiveSymptoms), "", Descriptor.DescriptorType.Information, false));
			return list;
		}

		// Token: 0x04006168 RID: 24936
		private StringKey name;

		// Token: 0x04006169 RID: 24937
		private StringKey descriptiveSymptoms;

		// Token: 0x0400616A RID: 24938
		private float sicknessDuration = 600f;

		// Token: 0x0400616B RID: 24939
		public float fatalityDuration;

		// Token: 0x0400616C RID: 24940
		public HashedString id;

		// Token: 0x0400616D RID: 24941
		public Sickness.SicknessType sicknessType;

		// Token: 0x0400616E RID: 24942
		public Sickness.Severity severity;

		// Token: 0x0400616F RID: 24943
		public string recoveryEffect;

		// Token: 0x04006170 RID: 24944
		public List<Sickness.InfectionVector> infectionVectors;

		// Token: 0x04006171 RID: 24945
		private List<Sickness.SicknessComponent> components = new List<Sickness.SicknessComponent>();

		// Token: 0x04006172 RID: 24946
		public Amount amount;

		// Token: 0x04006173 RID: 24947
		public Attribute amountDeltaAttribute;

		// Token: 0x04006174 RID: 24948
		public Attribute cureSpeedBase;

		// Token: 0x02002727 RID: 10023
		public abstract class SicknessComponent
		{
			// Token: 0x0600C807 RID: 51207
			public abstract object OnInfect(GameObject go, SicknessInstance diseaseInstance);

			// Token: 0x0600C808 RID: 51208
			public abstract void OnCure(GameObject go, object instance_data);

			// Token: 0x0600C809 RID: 51209 RVA: 0x004250A3 File Offset: 0x004232A3
			public virtual List<Descriptor> GetSymptoms()
			{
				return null;
			}

			// Token: 0x0600C80A RID: 51210 RVA: 0x004250A6 File Offset: 0x004232A6
			public virtual List<Descriptor> GetSymptoms(GameObject victim)
			{
				return this.GetSymptoms();
			}
		}

		// Token: 0x02002728 RID: 10024
		public enum InfectionVector
		{
			// Token: 0x0400AE6C RID: 44652
			Contact,
			// Token: 0x0400AE6D RID: 44653
			Digestion,
			// Token: 0x0400AE6E RID: 44654
			Inhalation,
			// Token: 0x0400AE6F RID: 44655
			Exposure
		}

		// Token: 0x02002729 RID: 10025
		public enum SicknessType
		{
			// Token: 0x0400AE71 RID: 44657
			Pathogen,
			// Token: 0x0400AE72 RID: 44658
			Ailment,
			// Token: 0x0400AE73 RID: 44659
			Injury
		}

		// Token: 0x0200272A RID: 10026
		public enum Severity
		{
			// Token: 0x0400AE75 RID: 44661
			Benign,
			// Token: 0x0400AE76 RID: 44662
			Minor,
			// Token: 0x0400AE77 RID: 44663
			Major,
			// Token: 0x0400AE78 RID: 44664
			Critical
		}
	}
}
