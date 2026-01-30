using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200096F RID: 2415
public class Geyser : StateMachineComponent<Geyser.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x060044B1 RID: 17585 RVA: 0x0018BFA6 File Offset: 0x0018A1A6
	// (set) Token: 0x060044B0 RID: 17584 RVA: 0x0018BF9D File Offset: 0x0018A19D
	public float timeShift { get; private set; }

	// Token: 0x060044B2 RID: 17586 RVA: 0x0018BFAE File Offset: 0x0018A1AE
	public float GetCurrentLifeTime()
	{
		return GameClock.Instance.GetTime() + this.timeShift;
	}

	// Token: 0x060044B3 RID: 17587 RVA: 0x0018BFC4 File Offset: 0x0018A1C4
	public void AlterTime(float timeOffset, bool shouldSurviveSaveLoad = false)
	{
		this.timeShift = Mathf.Max(timeOffset, -GameClock.Instance.GetTime());
		if (shouldSurviveSaveLoad)
		{
			this.serializedTimeShift = this.timeShift;
		}
		float num = this.RemainingEruptTime();
		float num2 = this.RemainingNonEruptTime();
		float num3 = this.RemainingActiveTime();
		float num4 = this.RemainingDormantTime();
		this.configuration.GetYearLength();
		if (num2 == 0f)
		{
			if ((num4 == 0f && this.configuration.GetYearOnDuration() - num3 < this.configuration.GetOnDuration() - num) | (num3 == 0f && this.configuration.GetYearOffDuration() - num4 >= this.configuration.GetOnDuration() - num))
			{
				base.smi.GoTo(base.smi.sm.dormant);
				return;
			}
			base.smi.GoTo(base.smi.sm.erupt);
			return;
		}
		else
		{
			bool flag = (num4 == 0f && this.configuration.GetYearOnDuration() - num3 < this.configuration.GetIterationLength() - num2) | (num3 == 0f && this.configuration.GetYearOffDuration() - num4 >= this.configuration.GetIterationLength() - num2);
			float num5 = this.RemainingEruptPreTime();
			if (flag && num5 <= 0f)
			{
				base.smi.GoTo(base.smi.sm.dormant);
				return;
			}
			if (num5 <= 0f)
			{
				base.smi.GoTo(base.smi.sm.idle);
				return;
			}
			float num6 = this.PreDuration() - num5;
			if ((num3 == 0f) ? (this.configuration.GetYearOffDuration() - num4 > num6) : (num6 > this.configuration.GetYearOnDuration() - num3))
			{
				base.smi.GoTo(base.smi.sm.dormant);
				return;
			}
			base.smi.GoTo(base.smi.sm.pre_erupt);
			return;
		}
	}

	// Token: 0x060044B4 RID: 17588 RVA: 0x0018C1D0 File Offset: 0x0018A3D0
	public void ShiftTimeTo(Geyser.TimeShiftStep step, bool shouldSurviveSaveLoad = false)
	{
		float num = this.RemainingEruptTime();
		float num2 = this.RemainingNonEruptTime();
		float num3 = this.RemainingActiveTime();
		float num4 = this.RemainingDormantTime();
		float yearLength = this.configuration.GetYearLength();
		switch (step)
		{
		case Geyser.TimeShiftStep.ActiveState:
		{
			float num5 = (num3 > 0f) ? (this.configuration.GetYearOnDuration() - num3) : (yearLength - num4);
			this.AlterTime(this.timeShift - num5, shouldSurviveSaveLoad);
			return;
		}
		case Geyser.TimeShiftStep.DormantState:
		{
			float num6 = (num3 > 0f) ? num3 : (-(this.configuration.GetYearOffDuration() - num4));
			this.AlterTime(this.timeShift + num6, shouldSurviveSaveLoad);
			return;
		}
		case Geyser.TimeShiftStep.NextIteration:
		{
			float num7 = (num > 0f) ? (num + this.configuration.GetOffDuration()) : num2;
			this.AlterTime(this.timeShift + num7, shouldSurviveSaveLoad);
			return;
		}
		case Geyser.TimeShiftStep.PreviousIteration:
		{
			float num8 = (num > 0f) ? (-(this.configuration.GetOnDuration() - num)) : (-(this.configuration.GetIterationLength() - num2));
			if (num > 0f && Mathf.Abs(num8) < this.configuration.GetOnDuration() * 0.05f)
			{
				num8 -= this.configuration.GetIterationLength();
			}
			this.AlterTime(this.timeShift + num8, shouldSurviveSaveLoad);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x060044B5 RID: 17589 RVA: 0x0018C30C File Offset: 0x0018A50C
	public void AddModification(Geyser.GeyserModification modification)
	{
		this.modifications.Add(modification);
		this.UpdateModifier();
	}

	// Token: 0x060044B6 RID: 17590 RVA: 0x0018C320 File Offset: 0x0018A520
	public void RemoveModification(Geyser.GeyserModification modification)
	{
		this.modifications.Remove(modification);
		this.UpdateModifier();
	}

	// Token: 0x060044B7 RID: 17591 RVA: 0x0018C338 File Offset: 0x0018A538
	private void UpdateModifier()
	{
		this.modifier.Clear();
		foreach (Geyser.GeyserModification modification in this.modifications)
		{
			this.modifier.AddValues(modification);
		}
		this.configuration.SetModifier(this.modifier);
		this.ApplyConfigurationEmissionValues(this.configuration);
		this.RefreshGeotunerFeedback();
	}

	// Token: 0x060044B8 RID: 17592 RVA: 0x0018C3C0 File Offset: 0x0018A5C0
	public void RefreshGeotunerFeedback()
	{
		this.RefreshGeotunerStatusItem();
		this.RefreshStudiedMeter();
	}

	// Token: 0x060044B9 RID: 17593 RVA: 0x0018C3D0 File Offset: 0x0018A5D0
	private void RefreshGeotunerStatusItem()
	{
		KSelectable component = base.gameObject.GetComponent<KSelectable>();
		if (this.GetAmountOfGeotunersPointingThisGeyser() > 0)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.GeyserGeotuned, this);
			return;
		}
		component.RemoveStatusItem(Db.Get().BuildingStatusItems.GeyserGeotuned, this);
	}

	// Token: 0x060044BA RID: 17594 RVA: 0x0018C428 File Offset: 0x0018A628
	private void RefreshStudiedMeter()
	{
		if (this.studyable.Studied)
		{
			bool flag = this.GetAmountOfGeotunersPointingThisGeyser() > 0;
			GeyserConfig.TrackerMeterAnimNames trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.tracker;
			if (flag)
			{
				trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker;
				int amountOfGeotunersAffectingThisGeyser = this.GetAmountOfGeotunersAffectingThisGeyser();
				if (amountOfGeotunersAffectingThisGeyser > 0)
				{
					trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker_minor;
				}
				if (amountOfGeotunersAffectingThisGeyser >= 5)
				{
					trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker_major;
				}
			}
			this.studyable.studiedIndicator.meterController.Play(trackerMeterAnimNames.ToString(), KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x060044BB RID: 17595 RVA: 0x0018C494 File Offset: 0x0018A694
	public int GetAmountOfGeotunersPointingThisGeyser()
	{
		return Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this);
	}

	// Token: 0x060044BC RID: 17596 RVA: 0x0018C4BC File Offset: 0x0018A6BC
	public int GetAmountOfGeotunersPointingOrWillPointAtThisGeyser()
	{
		return Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this || x.GetFutureGeyser() == this);
	}

	// Token: 0x060044BD RID: 17597 RVA: 0x0018C4E4 File Offset: 0x0018A6E4
	public int GetAmountOfGeotunersAffectingThisGeyser()
	{
		int num = 0;
		for (int i = 0; i < this.modifications.Count; i++)
		{
			if (this.modifications[i].originID.Contains("GeoTuner"))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060044BE RID: 17598 RVA: 0x0018C52B File Offset: 0x0018A72B
	private void OnGeotunerChanged(object o)
	{
		this.RefreshGeotunerFeedback();
	}

	// Token: 0x060044BF RID: 17599 RVA: 0x0018C534 File Offset: 0x0018A734
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		if (this.configuration == null || this.configuration.typeId == HashedString.Invalid)
		{
			this.configuration = base.GetComponent<GeyserConfigurator>().MakeConfiguration();
		}
		else
		{
			PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
			if (this.configuration.geyserType.geyserTemperature - component.Temperature != 0f)
			{
				SimTemperatureTransfer component2 = base.gameObject.GetComponent<SimTemperatureTransfer>();
				component2.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Combine(component2.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnSimRegistered));
			}
		}
		this.ApplyConfigurationEmissionValues(this.configuration);
		base.GetComponent<CodexEntryRedirector>().CodexID = "GEYSERGENERIC" + this.configuration.geyserType.id.ToUpper();
		this.GenerateName();
		this.timeShift = this.serializedTimeShift;
		base.smi.StartSM();
		Workable component3 = base.GetComponent<Studyable>();
		if (component3 != null)
		{
			component3.alwaysShowProgressBar = true;
		}
		Components.Geysers.Add(base.gameObject.GetMyWorldId(), this);
		base.gameObject.Subscribe(1763323737, new Action<object>(this.OnGeotunerChanged));
		this.RefreshStudiedMeter();
		this.UpdateModifier();
	}

	// Token: 0x060044C0 RID: 17600 RVA: 0x0018C684 File Offset: 0x0018A884
	private void GenerateName()
	{
		StringKey key = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + this.configuration.geyserType.id.ToUpper() + ".NAME");
		if (this.nameable.savedName == Strings.Get(key))
		{
			int cell = Grid.PosToCell(base.gameObject);
			Quadrant[] quadrantOfCell = base.gameObject.GetMyWorld().GetQuadrantOfCell(cell, 2);
			int num = (int)quadrantOfCell[0];
			string str = num.ToString();
			num = (int)quadrantOfCell[1];
			string text = str + num.ToString();
			string[] array = NAMEGEN.GEYSER_IDS.IDs.ToString().Split('\n', StringSplitOptions.None);
			string text2 = array[UnityEngine.Random.Range(0, array.Length)];
			string name = string.Concat(new string[]
			{
				UI.StripLinkFormatting(base.gameObject.GetProperName()),
				" ",
				text2,
				text,
				"‑",
				UnityEngine.Random.Range(0, 10).ToString()
			});
			this.nameable.SetName(name);
		}
	}

	// Token: 0x060044C1 RID: 17601 RVA: 0x0018C798 File Offset: 0x0018A998
	public void ApplyConfigurationEmissionValues(GeyserConfigurator.GeyserInstanceConfiguration config)
	{
		this.emitter.emitRange = 2;
		this.emitter.maxPressure = config.GetMaxPressure();
		this.emitter.outputElement = new ElementConverter.OutputElement(config.GetEmitRate(), config.GetElement(), config.GetTemperature(), false, false, (float)this.outputOffset.x, (float)this.outputOffset.y, 1f, config.GetDiseaseIdx(), Mathf.RoundToInt((float)config.GetDiseaseCount() * config.GetEmitRate()), true);
		if (this.emitter.IsSimActive)
		{
			this.emitter.SetSimActive(true);
		}
	}

	// Token: 0x060044C2 RID: 17602 RVA: 0x0018C838 File Offset: 0x0018AA38
	public void Unentomb()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		int cell = Grid.PosToCell(this);
		foreach (CellOffset offset in component.OccupiedCellsOffsets)
		{
			int num = Grid.OffsetCell(cell, offset);
			if (Grid.IsSolidCell(num) && Grid.Element[num].id != SimHashes.Unobtanium)
			{
				SimMessages.Dig(num, -1, false);
			}
		}
	}

	// Token: 0x060044C3 RID: 17603 RVA: 0x0018C89D File Offset: 0x0018AA9D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.gameObject.Unsubscribe(1763323737, new Action<object>(this.OnGeotunerChanged));
		Components.Geysers.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x060044C4 RID: 17604 RVA: 0x0018C8D8 File Offset: 0x0018AAD8
	private void OnSimRegistered(SimTemperatureTransfer stt)
	{
		PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
		if (this.configuration.geyserType.geyserTemperature - component.Temperature != 0f)
		{
			component.Temperature = this.configuration.geyserType.geyserTemperature;
		}
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnSimRegistered));
	}

	// Token: 0x060044C5 RID: 17605 RVA: 0x0018C948 File Offset: 0x0018AB48
	public float RemainingPhaseTimeFrom2(float onDuration, float offDuration, float time, Geyser.Phase expectedPhase)
	{
		float num = onDuration + offDuration;
		float num2 = time % num;
		float result;
		Geyser.Phase phase;
		if (num2 < onDuration)
		{
			result = Mathf.Max(onDuration - num2, 0f);
			phase = Geyser.Phase.On;
		}
		else
		{
			result = Mathf.Max(onDuration + offDuration - num2, 0f);
			phase = Geyser.Phase.Off;
		}
		if (expectedPhase != Geyser.Phase.Any && phase != expectedPhase)
		{
			return 0f;
		}
		return result;
	}

	// Token: 0x060044C6 RID: 17606 RVA: 0x0018C998 File Offset: 0x0018AB98
	public float RemainingPhaseTimeFrom4(float onDuration, float pstDuration, float offDuration, float preDuration, float time, Geyser.Phase expectedPhase)
	{
		float num = onDuration + pstDuration + offDuration + preDuration;
		float num2 = time % num;
		float result;
		Geyser.Phase phase;
		if (num2 < onDuration)
		{
			result = onDuration - num2;
			phase = Geyser.Phase.On;
		}
		else if (num2 < onDuration + pstDuration)
		{
			result = onDuration + pstDuration - num2;
			phase = Geyser.Phase.Pst;
		}
		else if (num2 < onDuration + pstDuration + offDuration)
		{
			result = onDuration + pstDuration + offDuration - num2;
			phase = Geyser.Phase.Off;
		}
		else
		{
			result = onDuration + pstDuration + offDuration + preDuration - num2;
			phase = Geyser.Phase.Pre;
		}
		if (expectedPhase != Geyser.Phase.Any && phase != expectedPhase)
		{
			return 0f;
		}
		return result;
	}

	// Token: 0x060044C7 RID: 17607 RVA: 0x0018CA01 File Offset: 0x0018AC01
	private float IdleDuration()
	{
		return this.configuration.GetOffDuration() * 0.84999996f;
	}

	// Token: 0x060044C8 RID: 17608 RVA: 0x0018CA14 File Offset: 0x0018AC14
	private float PreDuration()
	{
		return this.configuration.GetOffDuration() * 0.1f;
	}

	// Token: 0x060044C9 RID: 17609 RVA: 0x0018CA27 File Offset: 0x0018AC27
	private float PostDuration()
	{
		return this.configuration.GetOffDuration() * 0.05f;
	}

	// Token: 0x060044CA RID: 17610 RVA: 0x0018CA3A File Offset: 0x0018AC3A
	private float EruptDuration()
	{
		return this.configuration.GetOnDuration();
	}

	// Token: 0x060044CB RID: 17611 RVA: 0x0018CA47 File Offset: 0x0018AC47
	public bool ShouldGoDormant()
	{
		return this.RemainingActiveTime() <= 0f;
	}

	// Token: 0x060044CC RID: 17612 RVA: 0x0018CA59 File Offset: 0x0018AC59
	public float RemainingIdleTime()
	{
		return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Off);
	}

	// Token: 0x060044CD RID: 17613 RVA: 0x0018CA80 File Offset: 0x0018AC80
	public float RemainingEruptPreTime()
	{
		return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Pre);
	}

	// Token: 0x060044CE RID: 17614 RVA: 0x0018CAA7 File Offset: 0x0018ACA7
	public float RemainingEruptTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetOnDuration(), this.configuration.GetOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.On);
	}

	// Token: 0x060044CF RID: 17615 RVA: 0x0018CACC File Offset: 0x0018ACCC
	public float RemainingEruptPostTime()
	{
		return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Pst);
	}

	// Token: 0x060044D0 RID: 17616 RVA: 0x0018CAF3 File Offset: 0x0018ACF3
	public float RemainingNonEruptTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetOnDuration(), this.configuration.GetOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Off);
	}

	// Token: 0x060044D1 RID: 17617 RVA: 0x0018CB18 File Offset: 0x0018AD18
	public float RemainingDormantTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetYearOnDuration(), this.configuration.GetYearOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Off);
	}

	// Token: 0x060044D2 RID: 17618 RVA: 0x0018CB3D File Offset: 0x0018AD3D
	public float RemainingActiveTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetYearOnDuration(), this.configuration.GetYearOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.On);
	}

	// Token: 0x060044D3 RID: 17619 RVA: 0x0018CB64 File Offset: 0x0018AD64
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.configuration.GetElement()).tag.ProperName();
		List<GeoTuner.Instance> items = Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId());
		GeoTuner.Instance instance = items.Find((GeoTuner.Instance g) => g.GetAssignedGeyser() == this);
		int num = items.Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this);
		bool flag = num > 0;
		string text = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION, ElementLoader.FindElementByHash(this.configuration.GetElement()).name, GameUtil.GetFormattedMass(this.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		if (flag)
		{
			Func<float, float> func = delegate(float emissionPerCycleModifier)
			{
				float num8 = 600f / this.configuration.GetIterationLength();
				return emissionPerCycleModifier / num8 / this.configuration.GetOnDuration();
			};
			int amountOfGeotunersAffectingThisGeyser = this.GetAmountOfGeotunersAffectingThisGeyser();
			float num2 = (Geyser.temperatureModificationMethod == Geyser.ModificationMethod.Percentages) ? (instance.currentGeyserModification.temperatureModifier * this.configuration.geyserType.temperature) : instance.currentGeyserModification.temperatureModifier;
			float num3 = func((Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages) ? (instance.currentGeyserModification.massPerCycleModifier * this.configuration.scaledRate) : instance.currentGeyserModification.massPerCycleModifier);
			float num4 = (float)amountOfGeotunersAffectingThisGeyser * num2;
			float num5 = (float)amountOfGeotunersAffectingThisGeyser * num3;
			string arg2 = ((num4 > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num4, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
			string arg3 = ((num5 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num5, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}");
			string str = ((num2 > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
			string str2 = ((num3 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num3, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}");
			text = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED, ElementLoader.FindElementByHash(this.configuration.GetElement()).name, GameUtil.GetFormattedMass(this.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			text += "\n";
			text = text + "\n" + string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED_COUNT, amountOfGeotunersAffectingThisGeyser.ToString(), num.ToString());
			text += "\n";
			text = text + "\n" + string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED_TOTAL, arg3, arg2);
			for (int i = 0; i < amountOfGeotunersAffectingThisGeyser; i++)
			{
				string text2 = "\n    • " + UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_GEOTUNER_MODIFIER_ROW_TITLE.ToString();
				text2 = text2 + str2 + " " + str;
				text += text2;
			}
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_PRODUCTION, arg, GameUtil.GetFormattedMass(this.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), text, Descriptor.DescriptorType.Effect, false));
		if (this.configuration.GetDiseaseIdx() != 255)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_DISEASE, GameUtil.GetFormattedDiseaseName(this.configuration.GetDiseaseIdx(), false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_DISEASE, GameUtil.GetFormattedDiseaseName(this.configuration.GetDiseaseIdx(), false)), Descriptor.DescriptorType.Effect, false));
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_PERIOD, GameUtil.GetFormattedTime(this.configuration.GetOnDuration(), "F0"), GameUtil.GetFormattedTime(this.configuration.GetIterationLength(), "F0")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PERIOD, GameUtil.GetFormattedTime(this.configuration.GetOnDuration(), "F0"), GameUtil.GetFormattedTime(this.configuration.GetIterationLength(), "F0")), Descriptor.DescriptorType.Effect, false));
		Studyable component = base.GetComponent<Studyable>();
		if (component && !component.Studied)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_UNSTUDIED, Array.Empty<object>()), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_UNSTUDIED, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED, Array.Empty<object>()), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
		}
		else
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_PERIOD, GameUtil.GetFormattedCycles(this.configuration.GetYearOnDuration(), "F1", false), GameUtil.GetFormattedCycles(this.configuration.GetYearLength(), "F1", false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_PERIOD, GameUtil.GetFormattedCycles(this.configuration.GetYearOnDuration(), "F1", false), GameUtil.GetFormattedCycles(this.configuration.GetYearLength(), "F1", false)), Descriptor.DescriptorType.Effect, false));
			if (base.smi.IsInsideState(base.smi.sm.dormant))
			{
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_ACTIVE, GameUtil.GetFormattedCycles(this.RemainingDormantTime(), "F1", false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_ACTIVE, GameUtil.GetFormattedCycles(this.RemainingDormantTime(), "F1", false)), Descriptor.DescriptorType.Effect, false));
			}
			else
			{
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_DORMANT, GameUtil.GetFormattedCycles(this.RemainingActiveTime(), "F1", false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_DORMANT, GameUtil.GetFormattedCycles(this.RemainingActiveTime(), "F1", false)), Descriptor.DescriptorType.Effect, false));
			}
			string text3 = UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT.Replace("{average}", GameUtil.GetFormattedMass(this.configuration.GetAverageEmission(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{element}", this.configuration.geyserType.element.CreateTag().ProperName());
			if (flag)
			{
				text3 += "\n";
				text3 = text3 + "\n" + UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_TITLE;
				int amountOfGeotunersAffectingThisGeyser2 = this.GetAmountOfGeotunersAffectingThisGeyser();
				float num6 = (Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages) ? (instance.currentGeyserModification.massPerCycleModifier * 100f) : (instance.currentGeyserModification.massPerCycleModifier * 100f / this.configuration.scaledRate);
				float num7 = num6 * (float)amountOfGeotunersAffectingThisGeyser2;
				text3 = text3 + GameUtil.AddPositiveSign(num7.ToString("0.0"), num7 > 0f) + "%";
				for (int j = 0; j < amountOfGeotunersAffectingThisGeyser2; j++)
				{
					string text4 = "\n    • " + UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_ROW.ToString();
					text4 = text4 + GameUtil.AddPositiveSign(num6.ToString("0.0"), num6 > 0f) + "%";
					text3 += text4;
				}
			}
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_AVR_OUTPUT, GameUtil.GetFormattedMass(this.configuration.GetAverageEmission(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), text3, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x04002E21 RID: 11809
	public static Geyser.ModificationMethod massModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002E22 RID: 11810
	public static Geyser.ModificationMethod temperatureModificationMethod = Geyser.ModificationMethod.Values;

	// Token: 0x04002E23 RID: 11811
	public static Geyser.ModificationMethod IterationDurationModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002E24 RID: 11812
	public static Geyser.ModificationMethod IterationPercentageModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002E25 RID: 11813
	public static Geyser.ModificationMethod yearDurationModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002E26 RID: 11814
	public static Geyser.ModificationMethod yearPercentageModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002E27 RID: 11815
	public static Geyser.ModificationMethod maxPressureModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04002E28 RID: 11816
	[MyCmpAdd]
	private ElementEmitter emitter;

	// Token: 0x04002E29 RID: 11817
	[MyCmpAdd]
	private UserNameable nameable;

	// Token: 0x04002E2A RID: 11818
	[MyCmpGet]
	private Studyable studyable;

	// Token: 0x04002E2B RID: 11819
	[Serialize]
	public GeyserConfigurator.GeyserInstanceConfiguration configuration;

	// Token: 0x04002E2C RID: 11820
	public Vector2I outputOffset;

	// Token: 0x04002E2D RID: 11821
	public List<Geyser.GeyserModification> modifications = new List<Geyser.GeyserModification>();

	// Token: 0x04002E2E RID: 11822
	private Geyser.GeyserModification modifier;

	// Token: 0x04002E2F RID: 11823
	[Serialize]
	private float serializedTimeShift;

	// Token: 0x04002E31 RID: 11825
	private const float PRE_PCT = 0.1f;

	// Token: 0x04002E32 RID: 11826
	private const float POST_PCT = 0.05f;

	// Token: 0x020019A3 RID: 6563
	public enum ModificationMethod
	{
		// Token: 0x04007ECD RID: 32461
		Values,
		// Token: 0x04007ECE RID: 32462
		Percentages
	}

	// Token: 0x020019A4 RID: 6564
	public struct GeyserModification
	{
		// Token: 0x0600A2B2 RID: 41650 RVA: 0x003AFFDC File Offset: 0x003AE1DC
		public void Clear()
		{
			this.massPerCycleModifier = 0f;
			this.temperatureModifier = 0f;
			this.iterationDurationModifier = 0f;
			this.iterationPercentageModifier = 0f;
			this.yearDurationModifier = 0f;
			this.yearPercentageModifier = 0f;
			this.maxPressureModifier = 0f;
			this.modifyElement = false;
			this.newElement = (SimHashes)0;
		}

		// Token: 0x0600A2B3 RID: 41651 RVA: 0x003B0044 File Offset: 0x003AE244
		public void AddValues(Geyser.GeyserModification modification)
		{
			this.massPerCycleModifier += modification.massPerCycleModifier;
			this.temperatureModifier += modification.temperatureModifier;
			this.iterationDurationModifier += modification.iterationDurationModifier;
			this.iterationPercentageModifier += modification.iterationPercentageModifier;
			this.yearDurationModifier += modification.yearDurationModifier;
			this.yearPercentageModifier += modification.yearPercentageModifier;
			this.maxPressureModifier += modification.maxPressureModifier;
			this.modifyElement |= modification.modifyElement;
			this.newElement = ((modification.newElement == (SimHashes)0) ? this.newElement : modification.newElement);
		}

		// Token: 0x0600A2B4 RID: 41652 RVA: 0x003B0105 File Offset: 0x003AE305
		public bool IsNewElementInUse()
		{
			return this.modifyElement && this.newElement > (SimHashes)0;
		}

		// Token: 0x04007ECF RID: 32463
		public string originID;

		// Token: 0x04007ED0 RID: 32464
		public float massPerCycleModifier;

		// Token: 0x04007ED1 RID: 32465
		public float temperatureModifier;

		// Token: 0x04007ED2 RID: 32466
		public float iterationDurationModifier;

		// Token: 0x04007ED3 RID: 32467
		public float iterationPercentageModifier;

		// Token: 0x04007ED4 RID: 32468
		public float yearDurationModifier;

		// Token: 0x04007ED5 RID: 32469
		public float yearPercentageModifier;

		// Token: 0x04007ED6 RID: 32470
		public float maxPressureModifier;

		// Token: 0x04007ED7 RID: 32471
		public bool modifyElement;

		// Token: 0x04007ED8 RID: 32472
		public SimHashes newElement;
	}

	// Token: 0x020019A5 RID: 6565
	public class StatesInstance : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.GameInstance
	{
		// Token: 0x0600A2B5 RID: 41653 RVA: 0x003B011A File Offset: 0x003AE31A
		public StatesInstance(Geyser smi) : base(smi)
		{
		}
	}

	// Token: 0x020019A6 RID: 6566
	public class States : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser>
	{
		// Token: 0x0600A2B6 RID: 41654 RVA: 0x003B0124 File Offset: 0x003AE324
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.DefaultState(this.idle).Enter(delegate(Geyser.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(false);
			});
			this.dormant.PlayAnim("inactive", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutDormant, null).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingDormantTime(), this.pre_erupt);
			this.idle.PlayAnim("inactive", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle, null).Enter(delegate(Geyser.StatesInstance smi)
			{
				if (smi.master.ShouldGoDormant())
				{
					smi.GoTo(this.dormant);
				}
			}).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingIdleTime(), this.pre_erupt);
			this.pre_erupt.PlayAnim("shake", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutPressureBuilding, null).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingEruptPreTime(), this.erupt);
			this.erupt.TriggerOnEnter(GameHashes.GeyserEruption, (Geyser.StatesInstance smi) => true).TriggerOnExit(GameHashes.GeyserEruption, (Geyser.StatesInstance smi) => false).DefaultState(this.erupt.erupting).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingEruptTime(), this.post_erupt).Enter(delegate(Geyser.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(true);
			}).Exit(delegate(Geyser.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(false);
			});
			this.erupt.erupting.EventTransition(GameHashes.EmitterBlocked, this.erupt.overpressure, (Geyser.StatesInstance smi) => smi.GetComponent<ElementEmitter>().isEmitterBlocked).PlayAnim("erupt", KAnim.PlayMode.Loop);
			this.erupt.overpressure.EventTransition(GameHashes.EmitterUnblocked, this.erupt.erupting, (Geyser.StatesInstance smi) => !smi.GetComponent<ElementEmitter>().isEmitterBlocked).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure, null).PlayAnim("inactive", KAnim.PlayMode.Loop);
			this.post_erupt.PlayAnim("shake", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle, null).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingEruptPostTime(), this.idle);
		}

		// Token: 0x04007ED9 RID: 32473
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State dormant;

		// Token: 0x04007EDA RID: 32474
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State idle;

		// Token: 0x04007EDB RID: 32475
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State pre_erupt;

		// Token: 0x04007EDC RID: 32476
		public Geyser.States.EruptState erupt;

		// Token: 0x04007EDD RID: 32477
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State post_erupt;

		// Token: 0x020029B7 RID: 10679
		public class EruptState : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State
		{
			// Token: 0x0400B875 RID: 47221
			public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State erupting;

			// Token: 0x0400B876 RID: 47222
			public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State overpressure;
		}
	}

	// Token: 0x020019A7 RID: 6567
	public enum TimeShiftStep
	{
		// Token: 0x04007EDF RID: 32479
		ActiveState,
		// Token: 0x04007EE0 RID: 32480
		DormantState,
		// Token: 0x04007EE1 RID: 32481
		NextIteration,
		// Token: 0x04007EE2 RID: 32482
		PreviousIteration
	}

	// Token: 0x020019A8 RID: 6568
	public enum Phase
	{
		// Token: 0x04007EE4 RID: 32484
		Pre,
		// Token: 0x04007EE5 RID: 32485
		On,
		// Token: 0x04007EE6 RID: 32486
		Pst,
		// Token: 0x04007EE7 RID: 32487
		Off,
		// Token: 0x04007EE8 RID: 32488
		Any
	}
}
