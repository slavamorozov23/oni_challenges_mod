using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A2A RID: 2602
public class GermExposureMonitor : GameStateMachine<GermExposureMonitor, GermExposureMonitor.Instance>
{
	// Token: 0x06004C0E RID: 19470 RVA: 0x001BA254 File Offset: 0x001B8454
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.Update(delegate(GermExposureMonitor.Instance smi, float dt)
		{
			smi.OnInhaleExposureTick(dt);
		}, UpdateRate.SIM_1000ms, true).EventHandler(GameHashes.EatCompleteEater, delegate(GermExposureMonitor.Instance smi, object obj)
		{
			smi.OnEatComplete(obj);
		}).EventHandler(GameHashes.SicknessAdded, delegate(GermExposureMonitor.Instance smi, object data)
		{
			smi.OnSicknessAdded(data);
		}).EventHandler(GameHashes.SicknessCured, delegate(GermExposureMonitor.Instance smi, object data)
		{
			smi.OnSicknessCured(data);
		}).EventHandler(GameHashes.SleepFinished, delegate(GermExposureMonitor.Instance smi)
		{
			smi.OnSleepFinished();
		});
	}

	// Token: 0x06004C0F RID: 19471 RVA: 0x001BA341 File Offset: 0x001B8541
	public static float GetContractionChance(float rating)
	{
		return 0.5f - 0.5f * (float)Math.Tanh(0.25 * (double)rating);
	}

	// Token: 0x02001AE9 RID: 6889
	public enum ExposureState
	{
		// Token: 0x0400832B RID: 33579
		None,
		// Token: 0x0400832C RID: 33580
		Contact,
		// Token: 0x0400832D RID: 33581
		Exposed,
		// Token: 0x0400832E RID: 33582
		Contracted,
		// Token: 0x0400832F RID: 33583
		Sick
	}

	// Token: 0x02001AEA RID: 6890
	public class ExposureStatusData
	{
		// Token: 0x04008330 RID: 33584
		public ExposureType exposure_type;

		// Token: 0x04008331 RID: 33585
		public GermExposureMonitor.Instance owner;
	}

	// Token: 0x02001AEB RID: 6891
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<GermExposureMonitor, GermExposureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A793 RID: 42899 RVA: 0x003BC968 File Offset: 0x003BAB68
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sicknesses = master.GetComponent<MinionModifiers>().sicknesses;
			this.primaryElement = master.GetComponent<PrimaryElement>();
			this.traits = master.GetComponent<Traits>();
			this.lastDiseaseSources = new Dictionary<HashedString, GermExposureMonitor.Instance.DiseaseSourceInfo>();
			this.lastExposureTime = new Dictionary<HashedString, float>();
			this.inhaleExposureTick = new Dictionary<HashedString, GermExposureMonitor.Instance.InhaleTickInfo>();
			GameClock.Instance.Subscribe(-722330267, new Action<object>(this.OnNightTime));
			base.gameObject.Subscribe(-1582839653, new Action<object>(this.OnBionicTagsChanged));
			this.inateImmunities = DUPLICANTSTATS.GetStatsFor(base.gameObject).DiseaseImmunities.IMMUNITIES;
			OxygenBreather component = base.GetComponent<OxygenBreather>();
			if (component != null)
			{
				OxygenBreather oxygenBreather = component;
				oxygenBreather.onBreathableGasConsumed = (Action<SimHashes, float, float, byte, int>)Delegate.Combine(oxygenBreather.onBreathableGasConsumed, new Action<SimHashes, float, float, byte, int>(this.OnAirConsumed));
			}
		}

		// Token: 0x0600A794 RID: 42900 RVA: 0x003BCA78 File Offset: 0x003BAC78
		protected override void OnCleanUp()
		{
			base.gameObject.Unsubscribe(-1582839653, new Action<object>(this.OnBionicTagsChanged));
			base.OnCleanUp();
		}

		// Token: 0x0600A795 RID: 42901 RVA: 0x003BCA9C File Offset: 0x003BAC9C
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshStatusItems();
		}

		// Token: 0x0600A796 RID: 42902 RVA: 0x003BCAAC File Offset: 0x003BACAC
		public override void StopSM(string reason)
		{
			GameClock.Instance.Unsubscribe(-722330267, new Action<object>(this.OnNightTime));
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				Guid guid;
				this.statusItemHandles.TryGetValue(exposureType.germ_id, out guid);
				guid = base.GetComponent<KSelectable>().RemoveStatusItem(guid, false);
			}
			base.StopSM(reason);
		}

		// Token: 0x0600A797 RID: 42903 RVA: 0x003BCB18 File Offset: 0x003BAD18
		public void OnEatComplete(object obj)
		{
			Edible edible = (Edible)obj;
			HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(edible.gameObject);
			if (handle != HandleVector<int>.InvalidHandle)
			{
				DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(handle);
				if (header.diseaseIdx != 255)
				{
					Disease disease = Db.Get().Diseases[(int)header.diseaseIdx];
					float num = edible.unitsConsumed / (edible.unitsConsumed + edible.Units);
					int num2 = Mathf.CeilToInt((float)header.diseaseCount * num);
					GameComps.DiseaseContainers.ModifyDiseaseCount(handle, -num2);
					KPrefabID component = edible.GetComponent<KPrefabID>();
					this.InjectDisease(disease, num2, component.PrefabID(), Sickness.InfectionVector.Digestion);
				}
			}
		}

		// Token: 0x0600A798 RID: 42904 RVA: 0x003BCBC8 File Offset: 0x003BADC8
		public void OnAirConsumed(SimHashes elementConsumed, float massConsumed, float temperature, byte disseaseIDX, int disseaseCount)
		{
			if (disseaseIDX != 255)
			{
				Disease disease = Db.Get().Diseases[(int)disseaseIDX];
				this.InjectDisease(disease, disseaseCount, ElementLoader.FindElementByHash(elementConsumed).tag, Sickness.InfectionVector.Inhalation);
			}
		}

		// Token: 0x0600A799 RID: 42905 RVA: 0x003BCC08 File Offset: 0x003BAE08
		public void OnInhaleExposureTick(float dt)
		{
			foreach (KeyValuePair<HashedString, GermExposureMonitor.Instance.InhaleTickInfo> keyValuePair in this.inhaleExposureTick)
			{
				if (keyValuePair.Value.inhaled)
				{
					keyValuePair.Value.inhaled = false;
					keyValuePair.Value.ticks++;
				}
				else
				{
					keyValuePair.Value.ticks = Mathf.Max(0, keyValuePair.Value.ticks - 1);
				}
			}
		}

		// Token: 0x0600A79A RID: 42906 RVA: 0x003BCCA8 File Offset: 0x003BAEA8
		public void TryInjectDisease(byte disease_idx, int count, Tag source, Sickness.InfectionVector vector)
		{
			if (disease_idx != 255)
			{
				Disease disease = Db.Get().Diseases[(int)disease_idx];
				this.InjectDisease(disease, count, source, vector);
			}
		}

		// Token: 0x0600A79B RID: 42907 RVA: 0x003BCCD9 File Offset: 0x003BAED9
		public float GetGermResistance()
		{
			return Db.Get().Attributes.GermResistance.Lookup(base.gameObject).GetTotalValue();
		}

		// Token: 0x0600A79C RID: 42908 RVA: 0x003BCCFC File Offset: 0x003BAEFC
		public float GetResistanceToExposureType(ExposureType exposureType, float overrideExposureTier = -1f)
		{
			float num = overrideExposureTier;
			if (num == -1f)
			{
				num = this.GetExposureTier(exposureType.germ_id);
			}
			num = Mathf.Clamp(num, 1f, 3f);
			float num2 = GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[(int)num - 1];
			float totalValue = Db.Get().Attributes.GermResistance.Lookup(base.gameObject).GetTotalValue();
			return (float)exposureType.base_resistance + totalValue + num2;
		}

		// Token: 0x0600A79D RID: 42909 RVA: 0x003BCD68 File Offset: 0x003BAF68
		public int AssessDigestedGerms(ExposureType exposure_type, int count)
		{
			int exposure_threshold = exposure_type.exposure_threshold;
			int val = count / exposure_threshold;
			return MathUtil.Clamp(1, 3, val);
		}

		// Token: 0x0600A79E RID: 42910 RVA: 0x003BCD88 File Offset: 0x003BAF88
		public bool AssessInhaledGerms(ExposureType exposure_type)
		{
			GermExposureMonitor.Instance.InhaleTickInfo inhaleTickInfo;
			this.inhaleExposureTick.TryGetValue(exposure_type.germ_id, out inhaleTickInfo);
			if (inhaleTickInfo == null)
			{
				inhaleTickInfo = new GermExposureMonitor.Instance.InhaleTickInfo();
				this.inhaleExposureTick[exposure_type.germ_id] = inhaleTickInfo;
			}
			if (!inhaleTickInfo.inhaled)
			{
				float exposureTier = this.GetExposureTier(exposure_type.germ_id);
				inhaleTickInfo.inhaled = true;
				return inhaleTickInfo.ticks >= GERM_EXPOSURE.INHALE_TICK_THRESHOLD[(int)exposureTier];
			}
			return false;
		}

		// Token: 0x0600A79F RID: 42911 RVA: 0x003BCE00 File Offset: 0x003BB000
		public bool IsImmuneToDisease(string sicknessID)
		{
			if (this.inateImmunities == null)
			{
				return false;
			}
			for (int i = 0; i < this.inateImmunities.Length; i++)
			{
				if (sicknessID == this.inateImmunities[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A7A0 RID: 42912 RVA: 0x003BCE40 File Offset: 0x003BB040
		public void InjectDisease(Disease disease, int count, Tag source, Sickness.InfectionVector vector)
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (disease.id == exposureType.germ_id && !this.IsImmuneToDisease(exposureType.sickness_id) && count > exposureType.exposure_threshold && this.HasMinExposurePeriodElapsed(exposureType.germ_id) && this.IsExposureValidForTraits(exposureType))
				{
					Sickness sickness = (exposureType.sickness_id != null) ? Db.Get().Sicknesses.Get(exposureType.sickness_id) : null;
					if (sickness == null || sickness.infectionVectors.Contains(vector))
					{
						GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
						float exposureTier = this.GetExposureTier(exposureType.germ_id);
						if (exposureState == GermExposureMonitor.ExposureState.None || exposureState == GermExposureMonitor.ExposureState.Contact)
						{
							float contractionChance = GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f));
							this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Contact);
							if (contractionChance > 0f)
							{
								this.lastDiseaseSources[disease.id] = new GermExposureMonitor.Instance.DiseaseSourceInfo(source, vector, contractionChance, base.transform.GetPosition());
								if (exposureType.infect_immediately)
								{
									this.InfectImmediately(exposureType);
								}
								else
								{
									bool flag = true;
									bool flag2 = vector == Sickness.InfectionVector.Inhalation;
									bool flag3 = vector == Sickness.InfectionVector.Digestion;
									int num = 1;
									if (flag2)
									{
										flag = this.AssessInhaledGerms(exposureType);
									}
									if (flag3)
									{
										num = this.AssessDigestedGerms(exposureType, count);
									}
									if (flag)
									{
										if (flag2)
										{
											this.inhaleExposureTick[exposureType.germ_id].ticks = 0;
										}
										this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Exposed);
										this.SetExposureTier(exposureType.germ_id, (float)num);
										float amount = Mathf.Clamp01(contractionChance);
										GermExposureTracker.Instance.AddExposure(exposureType, amount);
									}
								}
							}
						}
						else if (exposureState == GermExposureMonitor.ExposureState.Exposed && exposureTier < 3f)
						{
							float contractionChance2 = GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f));
							if (contractionChance2 > 0f)
							{
								this.lastDiseaseSources[disease.id] = new GermExposureMonitor.Instance.DiseaseSourceInfo(source, vector, contractionChance2, base.transform.GetPosition());
								if (!exposureType.infect_immediately)
								{
									bool flag4 = true;
									bool flag5 = vector == Sickness.InfectionVector.Inhalation;
									bool flag6 = vector == Sickness.InfectionVector.Digestion;
									int num2 = 1;
									if (flag5)
									{
										flag4 = this.AssessInhaledGerms(exposureType);
									}
									if (flag6)
									{
										num2 = this.AssessDigestedGerms(exposureType, count);
									}
									if (flag4)
									{
										if (flag5)
										{
											this.inhaleExposureTick[exposureType.germ_id].ticks = 0;
										}
										this.SetExposureTier(exposureType.germ_id, this.GetExposureTier(exposureType.germ_id) + (float)num2);
										float amount2 = Mathf.Clamp01(GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f)) - contractionChance2);
										GermExposureTracker.Instance.AddExposure(exposureType, amount2);
									}
								}
							}
						}
					}
				}
			}
			this.RefreshStatusItems();
		}

		// Token: 0x0600A7A1 RID: 42913 RVA: 0x003BD114 File Offset: 0x003BB314
		public GermExposureMonitor.ExposureState GetExposureState(string germ_id)
		{
			GermExposureMonitor.ExposureState result;
			this.exposureStates.TryGetValue(germ_id, out result);
			return result;
		}

		// Token: 0x0600A7A2 RID: 42914 RVA: 0x003BD134 File Offset: 0x003BB334
		public float GetExposureTier(string germ_id)
		{
			float value = 1f;
			this.exposureTiers.TryGetValue(germ_id, out value);
			return Mathf.Clamp(value, 1f, 3f);
		}

		// Token: 0x0600A7A3 RID: 42915 RVA: 0x003BD166 File Offset: 0x003BB366
		public void SetExposureState(string germ_id, GermExposureMonitor.ExposureState exposure_state)
		{
			this.exposureStates[germ_id] = exposure_state;
			this.RefreshStatusItems();
		}

		// Token: 0x0600A7A4 RID: 42916 RVA: 0x003BD17B File Offset: 0x003BB37B
		public void SetExposureTier(string germ_id, float tier)
		{
			tier = Mathf.Clamp(tier, 0f, 3f);
			this.exposureTiers[germ_id] = tier;
			this.RefreshStatusItems();
		}

		// Token: 0x0600A7A5 RID: 42917 RVA: 0x003BD1A2 File Offset: 0x003BB3A2
		public void ContractGerms(string germ_id)
		{
			DebugUtil.DevAssert(this.GetExposureState(germ_id) == GermExposureMonitor.ExposureState.Exposed, "Duplicant is contracting a sickness but was never exposed to it!", null);
			this.SetExposureState(germ_id, GermExposureMonitor.ExposureState.Contracted);
		}

		// Token: 0x0600A7A6 RID: 42918 RVA: 0x003BD1C4 File Offset: 0x003BB3C4
		public void OnSicknessAdded(object sickness_instance_data)
		{
			SicknessInstance sicknessInstance = (SicknessInstance)sickness_instance_data;
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (exposureType.sickness_id == sicknessInstance.Sickness.Id)
				{
					this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Sick);
				}
			}
		}

		// Token: 0x0600A7A7 RID: 42919 RVA: 0x003BD218 File Offset: 0x003BB418
		public void OnSicknessCured(object sickness_instance_data)
		{
			SicknessInstance sicknessInstance = (SicknessInstance)sickness_instance_data;
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (exposureType.sickness_id == sicknessInstance.Sickness.Id)
				{
					this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.None);
				}
			}
		}

		// Token: 0x0600A7A8 RID: 42920 RVA: 0x003BD26C File Offset: 0x003BB46C
		private bool IsExposureValidForTraits(ExposureType exposure_type)
		{
			if (exposure_type.required_traits != null && exposure_type.required_traits.Count > 0)
			{
				foreach (string trait_id in exposure_type.required_traits)
				{
					if (!this.traits.HasTrait(trait_id))
					{
						return false;
					}
				}
			}
			if (exposure_type.excluded_traits != null && exposure_type.excluded_traits.Count > 0)
			{
				foreach (string trait_id2 in exposure_type.excluded_traits)
				{
					if (this.traits.HasTrait(trait_id2))
					{
						return false;
					}
				}
			}
			if (exposure_type.excluded_effects != null && exposure_type.excluded_effects.Count > 0)
			{
				Effects component = base.master.GetComponent<Effects>();
				foreach (string effect_id in exposure_type.excluded_effects)
				{
					if (component.HasEffect(effect_id))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600A7A9 RID: 42921 RVA: 0x003BD3B8 File Offset: 0x003BB5B8
		private bool HasMinExposurePeriodElapsed(string germ_id)
		{
			float num;
			this.lastExposureTime.TryGetValue(germ_id, out num);
			return num == 0f || GameClock.Instance.GetTime() - num > 540f;
		}

		// Token: 0x0600A7AA RID: 42922 RVA: 0x003BD3F8 File Offset: 0x003BB5F8
		private void RefreshStatusItems()
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				Guid guid;
				this.contactStatusItemHandles.TryGetValue(exposureType.germ_id, out guid);
				Guid guid2;
				this.statusItemHandles.TryGetValue(exposureType.germ_id, out guid2);
				GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
				if (guid2 == Guid.Empty && (exposureState == GermExposureMonitor.ExposureState.Exposed || exposureState == GermExposureMonitor.ExposureState.Contracted) && !string.IsNullOrEmpty(exposureType.sickness_id))
				{
					guid2 = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExposedToGerms, new GermExposureMonitor.ExposureStatusData
					{
						exposure_type = exposureType,
						owner = this
					});
				}
				else if (guid2 != Guid.Empty && exposureState != GermExposureMonitor.ExposureState.Exposed && exposureState != GermExposureMonitor.ExposureState.Contracted)
				{
					guid2 = base.GetComponent<KSelectable>().RemoveStatusItem(guid2, false);
				}
				this.statusItemHandles[exposureType.germ_id] = guid2;
				if (guid == Guid.Empty && exposureState == GermExposureMonitor.ExposureState.Contact)
				{
					if (!string.IsNullOrEmpty(exposureType.sickness_id))
					{
						guid = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ContactWithGerms, new GermExposureMonitor.ExposureStatusData
						{
							exposure_type = exposureType,
							owner = this
						});
					}
				}
				else if (guid != Guid.Empty && exposureState != GermExposureMonitor.ExposureState.Contact)
				{
					guid = base.GetComponent<KSelectable>().RemoveStatusItem(guid, false);
				}
				this.contactStatusItemHandles[exposureType.germ_id] = guid;
			}
		}

		// Token: 0x0600A7AB RID: 42923 RVA: 0x003BD56B File Offset: 0x003BB76B
		private void OnNightTime(object data)
		{
			this.UpdateReports();
		}

		// Token: 0x0600A7AC RID: 42924 RVA: 0x003BD574 File Offset: 0x003BB774
		private void UpdateReports()
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseStatus, (float)this.primaryElement.DiseaseCount, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.GERMS, "{0}", base.master.name), base.master.gameObject.GetProperName());
		}

		// Token: 0x0600A7AD RID: 42925 RVA: 0x003BD5C8 File Offset: 0x003BB7C8
		public void InfectImmediately(ExposureType exposure_type)
		{
			if (exposure_type.infection_effect != null)
			{
				base.master.GetComponent<Effects>().Add(exposure_type.infection_effect, true);
			}
			if (exposure_type.sickness_id != null)
			{
				string lastDiseaseSource = this.GetLastDiseaseSource(exposure_type.germ_id);
				SicknessExposureInfo exposure_info = new SicknessExposureInfo(exposure_type.sickness_id, lastDiseaseSource);
				this.sicknesses.Infect(exposure_info);
			}
		}

		// Token: 0x0600A7AE RID: 42926 RVA: 0x003BD624 File Offset: 0x003BB824
		private void OnBionicTagsChanged(object o)
		{
			if (o == null)
			{
				return;
			}
			TagChangedEventData value = ((Boxed<TagChangedEventData>)o).value;
			if (value.tag == GameTags.BionicBedTime && !value.added)
			{
				this.OnSleepFinished();
			}
		}

		// Token: 0x0600A7AF RID: 42927 RVA: 0x003BD664 File Offset: 0x003BB864
		public void OnSleepFinished()
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (!exposureType.infect_immediately && exposureType.sickness_id != null)
				{
					GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
					if (exposureState == GermExposureMonitor.ExposureState.Exposed)
					{
						this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.None);
					}
					if (exposureState == GermExposureMonitor.ExposureState.Contracted)
					{
						this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Sick);
						string lastDiseaseSource = this.GetLastDiseaseSource(exposureType.germ_id);
						SicknessExposureInfo exposure_info = new SicknessExposureInfo(exposureType.sickness_id, lastDiseaseSource);
						this.sicknesses.Infect(exposure_info);
					}
					this.SetExposureTier(exposureType.germ_id, 0f);
				}
			}
		}

		// Token: 0x0600A7B0 RID: 42928 RVA: 0x003BD704 File Offset: 0x003BB904
		public string GetLastDiseaseSource(string id)
		{
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			string result;
			if (this.lastDiseaseSources.TryGetValue(id, out diseaseSourceInfo))
			{
				switch (diseaseSourceInfo.vector)
				{
				case Sickness.InfectionVector.Contact:
					result = DUPLICANTS.DISEASES.INFECTIONSOURCES.SKIN;
					break;
				case Sickness.InfectionVector.Digestion:
					result = string.Format(DUPLICANTS.DISEASES.INFECTIONSOURCES.FOOD, diseaseSourceInfo.sourceObject.ProperName());
					break;
				case Sickness.InfectionVector.Inhalation:
					result = string.Format(DUPLICANTS.DISEASES.INFECTIONSOURCES.AIR, diseaseSourceInfo.sourceObject.ProperName());
					break;
				default:
					result = DUPLICANTS.DISEASES.INFECTIONSOURCES.UNKNOWN;
					break;
				}
			}
			else
			{
				result = DUPLICANTS.DISEASES.INFECTIONSOURCES.UNKNOWN;
			}
			return result;
		}

		// Token: 0x0600A7B1 RID: 42929 RVA: 0x003BD7A0 File Offset: 0x003BB9A0
		public Vector3 GetLastExposurePosition(string germ_id)
		{
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			if (this.lastDiseaseSources.TryGetValue(germ_id, out diseaseSourceInfo))
			{
				return diseaseSourceInfo.position;
			}
			return base.transform.GetPosition();
		}

		// Token: 0x0600A7B2 RID: 42930 RVA: 0x003BD7D4 File Offset: 0x003BB9D4
		public float GetExposureWeight(string id)
		{
			float exposureTier = this.GetExposureTier(id);
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			if (this.lastDiseaseSources.TryGetValue(id, out diseaseSourceInfo))
			{
				return diseaseSourceInfo.factor * exposureTier;
			}
			return 0f;
		}

		// Token: 0x04008332 RID: 33586
		[Serialize]
		public Dictionary<HashedString, GermExposureMonitor.Instance.DiseaseSourceInfo> lastDiseaseSources;

		// Token: 0x04008333 RID: 33587
		[Serialize]
		public Dictionary<HashedString, float> lastExposureTime;

		// Token: 0x04008334 RID: 33588
		private Dictionary<HashedString, GermExposureMonitor.Instance.InhaleTickInfo> inhaleExposureTick;

		// Token: 0x04008335 RID: 33589
		private string[] inateImmunities;

		// Token: 0x04008336 RID: 33590
		private Sicknesses sicknesses;

		// Token: 0x04008337 RID: 33591
		private PrimaryElement primaryElement;

		// Token: 0x04008338 RID: 33592
		private Traits traits;

		// Token: 0x04008339 RID: 33593
		[Serialize]
		private Dictionary<string, GermExposureMonitor.ExposureState> exposureStates = new Dictionary<string, GermExposureMonitor.ExposureState>();

		// Token: 0x0400833A RID: 33594
		[Serialize]
		private Dictionary<string, float> exposureTiers = new Dictionary<string, float>();

		// Token: 0x0400833B RID: 33595
		private Dictionary<string, Guid> statusItemHandles = new Dictionary<string, Guid>();

		// Token: 0x0400833C RID: 33596
		private Dictionary<string, Guid> contactStatusItemHandles = new Dictionary<string, Guid>();

		// Token: 0x020029DB RID: 10715
		[Serializable]
		public class DiseaseSourceInfo
		{
			// Token: 0x0600D29A RID: 53914 RVA: 0x004393C6 File Offset: 0x004375C6
			public DiseaseSourceInfo(Tag sourceObject, Sickness.InfectionVector vector, float factor, Vector3 position)
			{
				this.sourceObject = sourceObject;
				this.vector = vector;
				this.factor = factor;
				this.position = position;
			}

			// Token: 0x0400B911 RID: 47377
			public Tag sourceObject;

			// Token: 0x0400B912 RID: 47378
			public Sickness.InfectionVector vector;

			// Token: 0x0400B913 RID: 47379
			public float factor;

			// Token: 0x0400B914 RID: 47380
			public Vector3 position;
		}

		// Token: 0x020029DC RID: 10716
		public class InhaleTickInfo
		{
			// Token: 0x0400B915 RID: 47381
			public bool inhaled;

			// Token: 0x0400B916 RID: 47382
			public int ticks;
		}
	}
}
