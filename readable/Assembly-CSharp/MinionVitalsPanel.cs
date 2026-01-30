using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DB0 RID: 3504
[AddComponentMenu("KMonoBehaviour/scripts/MinionVitalsPanel")]
public class MinionVitalsPanel : CollapsibleDetailContentPanel
{
	// Token: 0x06006D1F RID: 27935 RVA: 0x00294C0C File Offset: 0x00292E0C
	public void Init()
	{
		this.AddAmountLine(Db.Get().Amounts.HitPoints, null);
		this.AddAmountLine(Db.Get().Amounts.BionicInternalBattery, null);
		this.AddAmountLine(Db.Get().Amounts.BionicOil, null);
		this.AddAmountLine(Db.Get().Amounts.BionicGunk, null);
		this.AddAttributeLine(Db.Get().CritterAttributes.Happiness, null);
		this.AddAmountLine(Db.Get().Amounts.Wildness, null);
		this.AddAmountLine(Db.Get().Amounts.Incubation, null);
		this.AddAmountLine(Db.Get().Amounts.Viability, null);
		this.AddAmountLine(Db.Get().Amounts.PowerCharge, null);
		this.AddAmountLine(Db.Get().Amounts.Fertility, null);
		this.AddAmountLine(Db.Get().Amounts.Beckoning, null);
		this.AddAmountLine(Db.Get().Amounts.Age, null);
		this.AddAmountLine(Db.Get().Amounts.Stress, null);
		this.AddAttributeLine(Db.Get().Attributes.QualityOfLife, null);
		this.AddAmountLine(Db.Get().Amounts.Bladder, null);
		this.AddAmountLine(Db.Get().Amounts.Breath, null);
		this.AddAmountLine(Db.Get().Amounts.BionicOxygenTank, null);
		this.AddAmountLine(Db.Get().Amounts.Stamina, null);
		this.AddAttributeLine(Db.Get().CritterAttributes.Metabolism, null);
		this.AddAmountLine(Db.Get().Amounts.Calories, null);
		this.AddAmountLine(Db.Get().Amounts.ScaleGrowth, null);
		this.AddAmountLine(Db.Get().Amounts.MilkProduction, null);
		this.AddAmountLine(Db.Get().Amounts.ElementGrowth, null);
		this.AddAmountLine(Db.Get().Amounts.Temperature, null);
		this.AddAmountLine(Db.Get().Amounts.CritterTemperature, null);
		this.AddAmountLine(Db.Get().Amounts.Decor, null);
		this.AddAmountLine(Db.Get().Amounts.InternalBattery, null);
		this.AddAmountLine(Db.Get().Amounts.InternalChemicalBattery, null);
		this.AddAmountLine(Db.Get().Amounts.InternalBioBattery, null);
		this.AddAmountLine(Db.Get().Amounts.InternalElectroBank, null);
		if (DlcManager.FeatureRadiationEnabled())
		{
			this.AddAmountLine(Db.Get().Amounts.RadiationBalance, null);
		}
		this.AddCheckboxLine(Db.Get().Amounts.AirPressure, this.conditionsContainerNormal, (GameObject go) => this.GetAirPressureLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<PressureVulnerable>() != null && go.GetComponent<PressureVulnerable>().pressure_sensitive)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_pressure(go), (GameObject go) => this.GetAirPressureTooltip(go));
		this.AddCheckboxLine(null, this.conditionsContainerNormal, (GameObject go) => this.GetAtmosphereLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<PressureVulnerable>() != null && go.GetComponent<PressureVulnerable>().safe_atmospheres.Count > 0)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_atmosphere(go), (GameObject go) => this.GetAtmosphereTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Temperature, this.conditionsContainerNormal, (GameObject go) => this.GetInternalTemperatureLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<TemperatureVulnerable>() != null)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_temperature(go), (GameObject go) => this.GetInternalTemperatureTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Fertilization, this.conditionsContainerAdditional, (GameObject go) => this.GetFertilizationLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<ReceptacleMonitor>() == null)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
			}
			if (go.GetComponent<ReceptacleMonitor>().Replanted)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Diminished;
		}, (GameObject go) => this.check_fertilizer(go), (GameObject go) => this.GetFertilizationTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Irrigation, this.conditionsContainerAdditional, (GameObject go) => this.GetIrrigationLabel(go), delegate(GameObject go)
		{
			ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
			if (!(component != null) || !component.Replanted)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Diminished;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
		}, (GameObject go) => this.check_irrigation(go), (GameObject go) => this.GetIrrigationTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Illumination, this.conditionsContainerNormal, (GameObject go) => this.GetIlluminationLabel(go), (GameObject go) => MinionVitalsPanel.CheckboxLineDisplayType.Normal, (GameObject go) => this.check_illumination(go), (GameObject go) => this.GetIlluminationTooltip(go));
		this.AddCheckboxLine(null, this.conditionsContainerNormal, (GameObject go) => this.GetRadiationLabel(go), delegate(GameObject go)
		{
			AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
			if (attributeInstance != null && attributeInstance.GetTotalValue() > 0f)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_radiation(go), (GameObject go) => this.GetRadiationTooltip(go));
		this.AddCheckboxLine(null, this.conditionsContainerNormal, (GameObject go) => this.GetEntityConsumptionLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<IPlantConsumeEntities>() != null)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_entity_consumed(go), (GameObject go) => this.GetEntityConsumedTooltip(go));
		this.AddCheckboxLine(null, this.conditionsContainerNormal, (GameObject go) => this.GetPollinationLabel(go), delegate(GameObject go)
		{
			if (go.GetSMI<PollinationMonitor.StatesInstance>() == null)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
		}, (GameObject go) => go.GetComponent<WiltCondition>().IsConditionSatisifed(WiltCondition.Condition.Pollination), (GameObject go) => this.GetPollinationTooltip(go));
	}

	// Token: 0x170007BA RID: 1978
	// (get) Token: 0x06006D20 RID: 27936 RVA: 0x002951F4 File Offset: 0x002933F4
	public string UnpollinatedTooltip
	{
		get
		{
			if (string.IsNullOrEmpty(this.unpollinatedTooltip))
			{
				StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
				foreach (GameObject gameObject in Assets.GetPrefabsWithTag(GameTags.Creatures.Pollinator))
				{
					KPrefabID component = gameObject.GetComponent<KPrefabID>();
					if (!(component == null) && Game.IsCorrectDlcActiveForCurrentSave(component))
					{
						stringBuilder.AppendFormat("\n{0}{1}", "    • ", gameObject.GetProperName());
					}
				}
				this.unpollinatedTooltip = string.Format(UI.TOOLTIPS.VITALS_CHECKBOX_UNPOLLINATED, GlobalStringBuilderPool.ReturnAndFree(stringBuilder));
			}
			return this.unpollinatedTooltip;
		}
	}

	// Token: 0x06006D21 RID: 27937 RVA: 0x002952AC File Offset: 0x002934AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Init();
	}

	// Token: 0x06006D22 RID: 27938 RVA: 0x002952BA File Offset: 0x002934BA
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x06006D23 RID: 27939 RVA: 0x002952CE File Offset: 0x002934CE
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06006D24 RID: 27940 RVA: 0x002952E4 File Offset: 0x002934E4
	private void AddAmountLine(Amount amount, Func<AmountInstance, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, this.Content.gameObject, false);
		gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite(amount.uiSprite);
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.AmountLine item = default(MinionVitalsPanel.AmountLine);
		item.amount = amount;
		item.go = gameObject;
		item.locText = gameObject.GetComponentInChildren<LocText>();
		item.toolTip = gameObject.GetComponentInChildren<ToolTip>();
		item.imageToggle = gameObject.GetComponentInChildren<ValueTrendImageToggle>();
		item.toolTipFunc = ((tooltip_func != null) ? tooltip_func : new Func<AmountInstance, string>(amount.GetTooltip));
		this.amountsLines.Add(item);
	}

	// Token: 0x06006D25 RID: 27941 RVA: 0x0029539C File Offset: 0x0029359C
	private void AddAttributeLine(Klei.AI.Attribute attribute, Func<AttributeInstance, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, this.Content.gameObject, false);
		gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite(attribute.uiSprite);
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.AttributeLine item = default(MinionVitalsPanel.AttributeLine);
		item.attribute = attribute;
		item.go = gameObject;
		item.locText = gameObject.GetComponentInChildren<LocText>();
		item.toolTip = gameObject.GetComponentInChildren<ToolTip>();
		gameObject.GetComponentInChildren<ValueTrendImageToggle>().gameObject.SetActive(false);
		item.toolTipFunc = ((tooltip_func != null) ? tooltip_func : new Func<AttributeInstance, string>(attribute.GetTooltip));
		this.attributesLines.Add(item);
	}

	// Token: 0x06006D26 RID: 27942 RVA: 0x00295458 File Offset: 0x00293658
	private void AddCheckboxLine(Amount amount, Transform parentContainer, Func<GameObject, string> label_text_func, Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition, Func<GameObject, bool> checkbox_value_func, Func<GameObject, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.CheckboxLinePrefab, this.Content.gameObject, false);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.CheckboxLine checkboxLine = default(MinionVitalsPanel.CheckboxLine);
		checkboxLine.go = gameObject;
		checkboxLine.parentContainer = parentContainer;
		checkboxLine.amount = amount;
		checkboxLine.locText = (component.GetReference("Label") as LocText);
		checkboxLine.get_value = checkbox_value_func;
		checkboxLine.display_condition = display_condition;
		checkboxLine.label_text_func = label_text_func;
		checkboxLine.go.name = "Checkbox_";
		if (amount != null)
		{
			GameObject go = checkboxLine.go;
			go.name += amount.Name;
		}
		else
		{
			GameObject go2 = checkboxLine.go;
			go2.name += "Unnamed";
		}
		if (tooltip_func != null)
		{
			checkboxLine.tooltip = tooltip_func;
			ToolTip tt = checkboxLine.go.GetComponent<ToolTip>();
			tt.refreshWhileHovering = true;
			tt.OnToolTip = delegate()
			{
				tt.ClearMultiStringTooltip();
				tt.AddMultiStringTooltip(tooltip_func(this.lastSelectedEntity), null);
				return "";
			};
		}
		this.checkboxLines.Add(checkboxLine);
	}

	// Token: 0x06006D27 RID: 27943 RVA: 0x0029559E File Offset: 0x0029379E
	private void ShouldShowVitalsPanel(GameObject selectedEntity)
	{
	}

	// Token: 0x06006D28 RID: 27944 RVA: 0x002955A0 File Offset: 0x002937A0
	public void Refresh(GameObject selectedEntity)
	{
		if (selectedEntity == null)
		{
			return;
		}
		if (selectedEntity.gameObject == null)
		{
			return;
		}
		this.lastSelectedEntity = selectedEntity;
		WiltCondition component = selectedEntity.GetComponent<WiltCondition>();
		MinionIdentity component2 = selectedEntity.GetComponent<MinionIdentity>();
		CreatureBrain component3 = selectedEntity.GetComponent<CreatureBrain>();
		IncubationMonitor.Instance smi = selectedEntity.GetSMI<IncubationMonitor.Instance>();
		object[] array = new object[]
		{
			component,
			component2,
			component3,
			smi
		};
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			base.SetActive(false);
			return;
		}
		base.SetActive(true);
		base.SetTitle((component == null) ? UI.DETAILTABS.SIMPLEINFO.GROUPNAME_CONDITION : UI.DETAILTABS.SIMPLEINFO.GROUPNAME_REQUIREMENTS);
		Amounts amounts = selectedEntity.GetAmounts();
		Attributes attributes = selectedEntity.GetAttributes();
		if (amounts == null || attributes == null)
		{
			return;
		}
		if (component == null)
		{
			this.conditionsContainerNormal.gameObject.SetActive(false);
			this.conditionsContainerAdditional.gameObject.SetActive(false);
			foreach (MinionVitalsPanel.AmountLine amountLine in this.amountsLines)
			{
				bool flag2 = amountLine.TryUpdate(amounts);
				if (amountLine.go.activeSelf != flag2)
				{
					amountLine.go.SetActive(flag2);
				}
			}
			foreach (MinionVitalsPanel.AttributeLine attributeLine in this.attributesLines)
			{
				bool flag3 = attributeLine.TryUpdate(attributes);
				if (attributeLine.go.activeSelf != flag3)
				{
					attributeLine.go.SetActive(flag3);
				}
			}
		}
		bool flag4 = false;
		for (int j = 0; j < this.checkboxLines.Count; j++)
		{
			MinionVitalsPanel.CheckboxLine checkboxLine = this.checkboxLines[j];
			MinionVitalsPanel.CheckboxLineDisplayType checkboxLineDisplayType = MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
			if (this.checkboxLines[j].amount != null)
			{
				for (int k = 0; k < amounts.Count; k++)
				{
					AmountInstance amountInstance = amounts[k];
					if (checkboxLine.amount == amountInstance.amount)
					{
						checkboxLineDisplayType = checkboxLine.display_condition(selectedEntity.gameObject);
						break;
					}
				}
			}
			else
			{
				checkboxLineDisplayType = checkboxLine.display_condition(selectedEntity.gameObject);
			}
			if (checkboxLineDisplayType != MinionVitalsPanel.CheckboxLineDisplayType.Hidden)
			{
				checkboxLine.locText.SetText(checkboxLine.label_text_func(selectedEntity.gameObject));
				if (!checkboxLine.go.activeSelf)
				{
					checkboxLine.go.SetActive(true);
				}
				GameObject gameObject = checkboxLine.go.GetComponent<HierarchyReferences>().GetReference("Check").gameObject;
				gameObject.SetActive(checkboxLine.get_value(selectedEntity.gameObject));
				if (checkboxLine.go.transform.parent != checkboxLine.parentContainer)
				{
					checkboxLine.go.transform.SetParent(checkboxLine.parentContainer);
					checkboxLine.go.transform.localScale = Vector3.one;
				}
				if (checkboxLine.parentContainer == this.conditionsContainerAdditional)
				{
					flag4 = true;
				}
				if (checkboxLineDisplayType == MinionVitalsPanel.CheckboxLineDisplayType.Normal)
				{
					if (checkboxLine.get_value(selectedEntity.gameObject))
					{
						checkboxLine.locText.color = Color.black;
						gameObject.transform.parent.GetComponent<Image>().color = Color.black;
					}
					else
					{
						Color color = new Color(0.99215686f, 0f, 0.101960786f);
						checkboxLine.locText.color = color;
						gameObject.transform.parent.GetComponent<Image>().color = color;
					}
				}
				else
				{
					checkboxLine.locText.color = Color.grey;
					gameObject.transform.parent.GetComponent<Image>().color = Color.grey;
				}
			}
			else if (checkboxLine.go.activeSelf)
			{
				checkboxLine.go.SetActive(false);
			}
		}
		if (component != null)
		{
			IManageGrowingStates manageGrowingStates = component.GetComponent<IManageGrowingStates>();
			manageGrowingStates = ((manageGrowingStates != null) ? manageGrowingStates : component.GetSMI<IManageGrowingStates>());
			bool flag5 = component.HasTag(GameTags.Decoration);
			this.conditionsContainerNormal.gameObject.SetActive(true);
			this.conditionsContainerAdditional.gameObject.SetActive(!flag5);
			if (manageGrowingStates == null)
			{
				float num = 1f;
				LocText reference = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference.text = "";
				reference.text = (flag5 ? string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_DECOR.BASE, Array.Empty<object>()) : string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.BASE, Util.FormatTwoDecimalPlace(num * 0.25f * 100f)));
				reference.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.TOOLTIP, Array.Empty<object>()));
				LocText reference2 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				ReceptacleMonitor component4 = selectedEntity.GetComponent<ReceptacleMonitor>();
				reference2.color = ((component4 == null || component4.Replanted) ? Color.black : Color.grey);
				reference2.text = string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.BASE, Util.FormatTwoDecimalPlace(num * 100f));
				reference2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.TOOLTIP, Array.Empty<object>()));
			}
			else
			{
				LocText reference3 = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference3.text = "";
				reference3.text = string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.BASE, GameUtil.GetFormattedCycles(manageGrowingStates.WildGrowthTime(), "F1", false));
				reference3.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.TOOLTIP, GameUtil.GetFormattedCycles(manageGrowingStates.WildGrowthTime(), "F1", false)));
				LocText reference4 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference4.color = (manageGrowingStates.IsWildPlanted() ? Color.grey : Color.black);
				reference4.text = "";
				reference4.text = (flag4 ? string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.BASE, GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime(), "F1", false)) : string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.DOMESTIC.BASE, GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime(), "F1", false)));
				reference4.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.TOOLTIP, GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime(), "F1", false)));
			}
			foreach (MinionVitalsPanel.AmountLine amountLine2 in this.amountsLines)
			{
				amountLine2.go.SetActive(false);
			}
			foreach (MinionVitalsPanel.AttributeLine attributeLine2 in this.attributesLines)
			{
				attributeLine2.go.SetActive(false);
			}
		}
	}

	// Token: 0x06006D29 RID: 27945 RVA: 0x00295CE0 File Offset: 0x00293EE0
	private string GetAirPressureTooltip(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		if (component == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_PRESSURE.text.Replace("{pressure}", GameUtil.GetFormattedMass(component.GetExternalPressure(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x06006D2A RID: 27946 RVA: 0x00295D2C File Offset: 0x00293F2C
	private string GetInternalTemperatureTooltip(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		if (component == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_TEMPERATURE.text.Replace("{temperature}", GameUtil.GetFormattedTemperature(component.InternalTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	}

	// Token: 0x06006D2B RID: 27947 RVA: 0x00295D74 File Offset: 0x00293F74
	private string GetFertilizationTooltip(GameObject go)
	{
		FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
		if (smi == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_FERTILIZER.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x06006D2C RID: 27948 RVA: 0x00295DB8 File Offset: 0x00293FB8
	private string GetIrrigationTooltip(GameObject go)
	{
		IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
		if (smi == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_IRRIGATION.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x06006D2D RID: 27949 RVA: 0x00295DFC File Offset: 0x00293FFC
	private string GetIlluminationTooltip(GameObject go)
	{
		IIlluminationTracker illuminationTracker = go.GetComponent<IIlluminationTracker>();
		if (illuminationTracker == null)
		{
			illuminationTracker = go.GetSMI<IIlluminationTracker>();
		}
		if (illuminationTracker == null)
		{
			return "";
		}
		return illuminationTracker.GetIlluminationUITooltip();
	}

	// Token: 0x06006D2E RID: 27950 RVA: 0x00295E2C File Offset: 0x0029402C
	private string GetRadiationTooltip(GameObject go)
	{
		int num = Grid.PosToCell(go);
		float rads = Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f;
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		AttributeInstance attributeInstance2 = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
		MutantPlant component = go.GetComponent<MutantPlant>();
		bool flag = component != null && component.IsOriginal;
		string text;
		if (attributeInstance.GetTotalValue() == 0f)
		{
			text = UI.TOOLTIPS.VITALS_CHECKBOX_RADIATION_NO_MIN.Replace("{rads}", GameUtil.GetFormattedRads(rads, GameUtil.TimeSlice.None)).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		else
		{
			text = UI.TOOLTIPS.VITALS_CHECKBOX_RADIATION.Replace("{rads}", GameUtil.GetFormattedRads(rads, GameUtil.TimeSlice.None)).Replace("{minRads}", attributeInstance.GetFormattedValue()).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		if (flag)
		{
			text += UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_SEED_TOOLTIP;
		}
		return text;
	}

	// Token: 0x06006D2F RID: 27951 RVA: 0x00295F34 File Offset: 0x00294134
	private string GetReceptacleTooltip(GameObject go)
	{
		ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
		if (component == null)
		{
			return "";
		}
		if (component.HasOperationalReceptacle())
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_OPERATIONAL;
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_INOPERATIONAL;
	}

	// Token: 0x06006D30 RID: 27952 RVA: 0x00295F74 File Offset: 0x00294174
	private string GetEntityConsumedTooltip(GameObject go)
	{
		IPlantConsumeEntities component = go.GetComponent<IPlantConsumeEntities>();
		component.AreEntitiesConsumptionRequirementsSatisfied();
		return GameUtil.SafeStringFormat(UI.TOOLTIPS.VITALS_CHECKBOX_ENTITY_CONSUMER_REQUIREMENTS, new object[]
		{
			component.GetConsumableEntitiesCategoryName()
		});
	}

	// Token: 0x06006D31 RID: 27953 RVA: 0x00295FAD File Offset: 0x002941AD
	private string GetPollinationTooltip(GameObject go)
	{
		if (!go.GetComponent<WiltCondition>().IsConditionSatisifed(WiltCondition.Condition.Pollination))
		{
			return this.UnpollinatedTooltip;
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_POLLINATED;
	}

	// Token: 0x06006D32 RID: 27954 RVA: 0x00295FD0 File Offset: 0x002941D0
	private string GetAtmosphereTooltip(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		if (component != null && component.currentAtmoElement != null)
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE.text.Replace("{element}", component.currentAtmoElement.name);
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE;
	}

	// Token: 0x06006D33 RID: 27955 RVA: 0x00296020 File Offset: 0x00294220
	private string GetAirPressureLabel(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return string.Concat(new string[]
		{
			Db.Get().Amounts.AirPressure.Name,
			"\n    • ",
			GameUtil.GetFormattedMass(component.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Gram, false, "{0:0.#}"),
			" - ",
			GameUtil.GetFormattedMass(component.pressureWarning_High, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}")
		});
	}

	// Token: 0x06006D34 RID: 27956 RVA: 0x00296094 File Offset: 0x00294294
	private string GetInternalTemperatureLabel(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		return string.Concat(new string[]
		{
			Db.Get().Amounts.Temperature.Name,
			"\n    • ",
			GameUtil.GetFormattedTemperature(component.TemperatureWarningLow, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false),
			" - ",
			GameUtil.GetFormattedTemperature(component.TemperatureWarningHigh, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)
		});
	}

	// Token: 0x06006D35 RID: 27957 RVA: 0x00296100 File Offset: 0x00294300
	private string GetFertilizationLabel(GameObject go)
	{
		StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.GenericInstance smi = go.GetSMI<FertilizationMonitor.Instance>();
		string text = Db.Get().Amounts.Fertilization.Name;
		float totalValue = go.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
		foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in smi.def.consumedElements)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n    • ",
				ElementLoader.GetElement(consumeInfo.tag).name,
				" ",
				GameUtil.GetFormattedMass(consumeInfo.massConsumptionRate * totalValue, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
			});
		}
		return text;
	}

	// Token: 0x06006D36 RID: 27958 RVA: 0x002961B8 File Offset: 0x002943B8
	private string GetIrrigationLabel(GameObject go)
	{
		StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.GenericInstance smi = go.GetSMI<IrrigationMonitor.Instance>();
		string text = Db.Get().Amounts.Irrigation.Name;
		float totalValue = go.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
		foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in smi.def.consumedElements)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n    • ",
				ElementLoader.GetElement(consumeInfo.tag).name,
				": ",
				GameUtil.GetFormattedMass(consumeInfo.massConsumptionRate * totalValue, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
			});
		}
		return text;
	}

	// Token: 0x06006D37 RID: 27959 RVA: 0x00296270 File Offset: 0x00294470
	private string GetIlluminationLabel(GameObject go)
	{
		IIlluminationTracker illuminationTracker = go.GetComponent<IIlluminationTracker>();
		if (illuminationTracker == null)
		{
			illuminationTracker = go.GetSMI<IIlluminationTracker>();
		}
		return illuminationTracker.GetIlluminationUILabel();
	}

	// Token: 0x06006D38 RID: 27960 RVA: 0x00296294 File Offset: 0x00294494
	private string GetEntityConsumptionLabel(GameObject go)
	{
		IPlantConsumeEntities component = go.GetComponent<IPlantConsumeEntities>();
		return component.GetRequirementText() + "\n    • " + (this.check_entity_consumed(go) ? GameUtil.SafeStringFormat(UI.TOOLTIPS.VITALS_CHECKBOX_ENTITY_CONSUMER_SATISFIED, new object[]
		{
			component.GetConsumedEntityName()
		}) : UI.TOOLTIPS.VITALS_CHECKBOX_ENTITY_CONSUMER_UNSATISFIED);
	}

	// Token: 0x06006D39 RID: 27961 RVA: 0x002962EB File Offset: 0x002944EB
	private string GetPollinationLabel(GameObject go)
	{
		return UI.VITALSSCREEN.POLLINATION;
	}

	// Token: 0x06006D3A RID: 27962 RVA: 0x002962F8 File Offset: 0x002944F8
	private string GetAtmosphereLabel(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		string text = UI.VITALSSCREEN.ATMOSPHERE_CONDITION;
		foreach (Element element in component.safe_atmospheres)
		{
			text = text + "\n    • " + element.name;
		}
		return text;
	}

	// Token: 0x06006D3B RID: 27963 RVA: 0x00296368 File Offset: 0x00294568
	private string GetRadiationLabel(GameObject go)
	{
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		AttributeInstance attributeInstance2 = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
		if (attributeInstance.GetTotalValue() == 0f)
		{
			return UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION + "\n    • " + UI.GAMEOBJECTEFFECTS.AMBIENT_NO_MIN_RADIATION_FMT.Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		return UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION + "\n    • " + UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION_FMT.Replace("{minRads}", attributeInstance.GetFormattedValue()).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
	}

	// Token: 0x06006D3C RID: 27964 RVA: 0x0029641C File Offset: 0x0029461C
	private bool check_pressure(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return !(component != null) || component.ExternalPressureState == PressureVulnerable.PressureState.Normal;
	}

	// Token: 0x06006D3D RID: 27965 RVA: 0x00296444 File Offset: 0x00294644
	private bool check_temperature(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		return !(component != null) || component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.Normal;
	}

	// Token: 0x06006D3E RID: 27966 RVA: 0x0029646C File Offset: 0x0029466C
	private bool check_irrigation(GameObject go)
	{
		IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
		return smi == null || (!smi.IsInsideState(smi.sm.replanted.starved) && !smi.IsInsideState(smi.sm.wild));
	}

	// Token: 0x06006D3F RID: 27967 RVA: 0x002964B4 File Offset: 0x002946B4
	private bool check_illumination(GameObject go)
	{
		IIlluminationTracker illuminationTracker = go.GetComponent<IIlluminationTracker>();
		if (illuminationTracker == null)
		{
			illuminationTracker = go.GetSMI<IIlluminationTracker>();
		}
		return illuminationTracker == null || illuminationTracker.ShouldIlluminationUICheckboxBeChecked();
	}

	// Token: 0x06006D40 RID: 27968 RVA: 0x002964E0 File Offset: 0x002946E0
	private bool check_radiation(GameObject go)
	{
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		if (attributeInstance != null && attributeInstance.GetTotalValue() != 0f)
		{
			int num = Grid.PosToCell(go);
			return (Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f) >= attributeInstance.GetTotalValue();
		}
		return true;
	}

	// Token: 0x06006D41 RID: 27969 RVA: 0x00296548 File Offset: 0x00294748
	private bool check_receptacle(GameObject go)
	{
		ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
		return !(component == null) && component.HasOperationalReceptacle();
	}

	// Token: 0x06006D42 RID: 27970 RVA: 0x00296570 File Offset: 0x00294770
	private bool check_fertilizer(GameObject go)
	{
		FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
		return smi == null || smi.sm.hasCorrectFertilizer.Get(smi);
	}

	// Token: 0x06006D43 RID: 27971 RVA: 0x0029659C File Offset: 0x0029479C
	private bool check_atmosphere(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return !(component != null) || component.testAreaElementSafe;
	}

	// Token: 0x06006D44 RID: 27972 RVA: 0x002965C1 File Offset: 0x002947C1
	private bool check_entity_consumed(GameObject go)
	{
		return go.GetComponent<IPlantConsumeEntities>().AreEntitiesConsumptionRequirementsSatisfied();
	}

	// Token: 0x04004AA9 RID: 19113
	public GameObject LineItemPrefab;

	// Token: 0x04004AAA RID: 19114
	public GameObject CheckboxLinePrefab;

	// Token: 0x04004AAB RID: 19115
	private GameObject lastSelectedEntity;

	// Token: 0x04004AAC RID: 19116
	public List<MinionVitalsPanel.AmountLine> amountsLines = new List<MinionVitalsPanel.AmountLine>();

	// Token: 0x04004AAD RID: 19117
	public List<MinionVitalsPanel.AttributeLine> attributesLines = new List<MinionVitalsPanel.AttributeLine>();

	// Token: 0x04004AAE RID: 19118
	public List<MinionVitalsPanel.CheckboxLine> checkboxLines = new List<MinionVitalsPanel.CheckboxLine>();

	// Token: 0x04004AAF RID: 19119
	public Transform conditionsContainerNormal;

	// Token: 0x04004AB0 RID: 19120
	public Transform conditionsContainerAdditional;

	// Token: 0x04004AB1 RID: 19121
	private string unpollinatedTooltip;

	// Token: 0x02001FF7 RID: 8183
	[DebuggerDisplay("{amount.Name}")]
	public struct AmountLine
	{
		// Token: 0x0600B7E3 RID: 47075 RVA: 0x003F3884 File Offset: 0x003F1A84
		public bool TryUpdate(Amounts amounts)
		{
			foreach (AmountInstance amountInstance in amounts)
			{
				if (this.amount == amountInstance.amount && !amountInstance.hide)
				{
					this.locText.SetText(this.amount.GetDescription(amountInstance));
					this.toolTip.toolTip = this.toolTipFunc(amountInstance);
					this.imageToggle.SetValue(amountInstance);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04009444 RID: 37956
		public Amount amount;

		// Token: 0x04009445 RID: 37957
		public GameObject go;

		// Token: 0x04009446 RID: 37958
		public ValueTrendImageToggle imageToggle;

		// Token: 0x04009447 RID: 37959
		public LocText locText;

		// Token: 0x04009448 RID: 37960
		public ToolTip toolTip;

		// Token: 0x04009449 RID: 37961
		public Func<AmountInstance, string> toolTipFunc;
	}

	// Token: 0x02001FF8 RID: 8184
	[DebuggerDisplay("{attribute.Name}")]
	public struct AttributeLine
	{
		// Token: 0x0600B7E4 RID: 47076 RVA: 0x003F391C File Offset: 0x003F1B1C
		public bool TryUpdate(Attributes attributes)
		{
			foreach (AttributeInstance attributeInstance in attributes)
			{
				if (this.attribute == attributeInstance.modifier && !attributeInstance.hide)
				{
					this.locText.SetText(this.attribute.GetDescription(attributeInstance));
					this.toolTip.toolTip = this.toolTipFunc(attributeInstance);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400944A RID: 37962
		public Klei.AI.Attribute attribute;

		// Token: 0x0400944B RID: 37963
		public GameObject go;

		// Token: 0x0400944C RID: 37964
		public LocText locText;

		// Token: 0x0400944D RID: 37965
		public ToolTip toolTip;

		// Token: 0x0400944E RID: 37966
		public Func<AttributeInstance, string> toolTipFunc;
	}

	// Token: 0x02001FF9 RID: 8185
	public struct CheckboxLine
	{
		// Token: 0x0400944F RID: 37967
		public Amount amount;

		// Token: 0x04009450 RID: 37968
		public GameObject go;

		// Token: 0x04009451 RID: 37969
		public LocText locText;

		// Token: 0x04009452 RID: 37970
		public Func<GameObject, string> tooltip;

		// Token: 0x04009453 RID: 37971
		public Func<GameObject, bool> get_value;

		// Token: 0x04009454 RID: 37972
		public Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition;

		// Token: 0x04009455 RID: 37973
		public Func<GameObject, string> label_text_func;

		// Token: 0x04009456 RID: 37974
		public Transform parentContainer;
	}

	// Token: 0x02001FFA RID: 8186
	public enum CheckboxLineDisplayType
	{
		// Token: 0x04009458 RID: 37976
		Normal,
		// Token: 0x04009459 RID: 37977
		Diminished,
		// Token: 0x0400945A RID: 37978
		Hidden
	}
}
