using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000D81 RID: 3457
public class VitalsTableScreen : TableScreen
{
	// Token: 0x06006B50 RID: 27472 RVA: 0x0028B370 File Offset: 0x00289570
	protected override void OnActivate()
	{
		this.has_default_duplicant_row = false;
		this.title = UI.VITALS;
		base.OnActivate();
		base.AddPortraitColumn("Portrait", new Action<IAssignableIdentity, GameObject>(base.on_load_portrait), null, true);
		base.AddButtonLabelColumn("Names", new Action<IAssignableIdentity, GameObject>(base.on_load_name_label), new Func<IAssignableIdentity, GameObject, string>(base.get_value_name_label), delegate(GameObject widget_go)
		{
			base.GetWidgetRow(widget_go).SelectMinion();
		}, delegate(GameObject widget_go)
		{
			base.GetWidgetRow(widget_go).SelectAndFocusMinion();
		}, new Comparison<IAssignableIdentity>(base.compare_rows_alphabetical), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_name), new Action<IAssignableIdentity, GameObject, ToolTip>(base.on_tooltip_sort_alphabetically), false);
		base.AddLabelColumn("Stress", new Action<IAssignableIdentity, GameObject>(this.on_load_stress), new Func<IAssignableIdentity, GameObject, string>(this.get_value_stress_label), new Comparison<IAssignableIdentity>(this.compare_rows_stress), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_stress), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_stress), 64, true);
		base.AddLabelColumn("QOLExpectations", new Action<IAssignableIdentity, GameObject>(this.on_load_qualityoflife_expectations), new Func<IAssignableIdentity, GameObject, string>(this.get_value_qualityoflife_expectations_label), new Comparison<IAssignableIdentity>(this.compare_rows_qualityoflife_expectations), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_qualityoflife_expectations), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_qualityoflife_expectations), 64, true);
		if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
		{
			base.AddLabelColumn("PowerBanks", new Action<IAssignableIdentity, GameObject>(this.on_load_power_banks), new Func<IAssignableIdentity, GameObject, string>(this.get_value_power_banks_label), new Comparison<IAssignableIdentity>(this.compare_rows_power_banks), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_power_banks), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_power_banks), 64, true);
		}
		base.AddLabelColumn("Fullness", new Action<IAssignableIdentity, GameObject>(this.on_load_fullness), new Func<IAssignableIdentity, GameObject, string>(this.get_value_fullness_label), new Comparison<IAssignableIdentity>(this.compare_rows_fullness), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_fullness), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_fullness), 96, true);
		base.AddLabelColumn("Health", new Action<IAssignableIdentity, GameObject>(this.on_load_health), new Func<IAssignableIdentity, GameObject, string>(this.get_value_health_label), new Comparison<IAssignableIdentity>(this.compare_rows_health), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_health), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_health), 64, true);
		base.AddLabelColumn("Immunity", new Action<IAssignableIdentity, GameObject>(this.on_load_sickness), new Func<IAssignableIdentity, GameObject, string>(this.get_value_sickness_label), new Comparison<IAssignableIdentity>(this.compare_rows_sicknesses), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sicknesses), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_sicknesses), 192, true);
	}

	// Token: 0x06006B51 RID: 27473 RVA: 0x0028B5E8 File Offset: 0x002897E8
	private void on_load_stress(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN.STRESS.ToString());
	}

	// Token: 0x06006B52 RID: 27474 RVA: 0x0028B648 File Offset: 0x00289848
	private string get_value_stress_label(IAssignableIdentity identity, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				return Db.Get().Amounts.Stress.Lookup(minionIdentity).GetValueString();
			}
		}
		else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x06006B53 RID: 27475 RVA: 0x0028B6AC File Offset: 0x002898AC
	private int compare_rows_stress(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float value = Db.Get().Amounts.Stress.Lookup(minionIdentity).value;
		float value2 = Db.Get().Amounts.Stress.Lookup(minionIdentity2).value;
		return value2.CompareTo(value);
	}

	// Token: 0x06006B54 RID: 27476 RVA: 0x0028B730 File Offset: 0x00289930
	protected void on_tooltip_stress(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null)
			{
				tooltip.AddMultiStringTooltip(Db.Get().Amounts.Stress.Lookup(minionIdentity).GetTooltip(), null);
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B55 RID: 27477 RVA: 0x0028B7A4 File Offset: 0x002899A4
	protected void on_tooltip_sort_stress(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_STRESS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B56 RID: 27478 RVA: 0x0028B7EC File Offset: 0x002899EC
	private void on_load_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN.QUALITYOFLIFE_EXPECTATIONS.ToString());
	}

	// Token: 0x06006B57 RID: 27479 RVA: 0x0028B84C File Offset: 0x00289A4C
	private string get_value_qualityoflife_expectations_label(IAssignableIdentity identity, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				return Db.Get().Attributes.QualityOfLife.Lookup(minionIdentity).GetFormattedValue();
			}
		}
		else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x06006B58 RID: 27480 RVA: 0x0028B8B0 File Offset: 0x00289AB0
	private int compare_rows_qualityoflife_expectations(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float totalValue = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(minionIdentity).GetTotalValue();
		float totalValue2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(minionIdentity2).GetTotalValue();
		return totalValue.CompareTo(totalValue2);
	}

	// Token: 0x06006B59 RID: 27481 RVA: 0x0028B934 File Offset: 0x00289B34
	protected void on_tooltip_qualityoflife_expectations(IAssignableIdentity identity, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				tooltip.AddMultiStringTooltip(Db.Get().Attributes.QualityOfLife.Lookup(minionIdentity).GetAttributeValueTooltip(), null);
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(identity, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B5A RID: 27482 RVA: 0x0028B9A8 File Offset: 0x00289BA8
	protected void on_tooltip_sort_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_EXPECTATIONS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B5B RID: 27483 RVA: 0x0028B9F0 File Offset: 0x00289BF0
	private void on_load_health(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : (componentInChildren.text = UI.VITALSSCREEN_HEALTH.ToString()));
	}

	// Token: 0x06006B5C RID: 27484 RVA: 0x0028BA58 File Offset: 0x00289C58
	private string get_value_health_label(IAssignableIdentity minion, GameObject widget_go)
	{
		if (minion != null)
		{
			TableRow widgetRow = base.GetWidgetRow(widget_go);
			if (widgetRow.rowType == TableRow.RowType.Minion && minion as MinionIdentity != null)
			{
				return Db.Get().Amounts.HitPoints.Lookup(minion as MinionIdentity).GetValueString();
			}
			if (widgetRow.rowType == TableRow.RowType.StoredMinon)
			{
				return UI.TABLESCREENS.NA;
			}
		}
		return "";
	}

	// Token: 0x06006B5D RID: 27485 RVA: 0x0028BAC0 File Offset: 0x00289CC0
	private int compare_rows_health(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float value = Db.Get().Amounts.HitPoints.Lookup(minionIdentity).value;
		float value2 = Db.Get().Amounts.HitPoints.Lookup(minionIdentity2).value;
		return value2.CompareTo(value);
	}

	// Token: 0x06006B5E RID: 27486 RVA: 0x0028BB44 File Offset: 0x00289D44
	protected void on_tooltip_health(IAssignableIdentity identity, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				tooltip.AddMultiStringTooltip(Db.Get().Amounts.HitPoints.Lookup(minionIdentity).GetTooltip(), null);
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(identity, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B5F RID: 27487 RVA: 0x0028BBB8 File Offset: 0x00289DB8
	protected void on_tooltip_sort_health(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_HITPOINTS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B60 RID: 27488 RVA: 0x0028BC00 File Offset: 0x00289E00
	private void on_load_sickness(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN_SICKNESS.ToString());
	}

	// Token: 0x06006B61 RID: 27489 RVA: 0x0028BC60 File Offset: 0x00289E60
	private string get_value_sickness_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null)
			{
				List<KeyValuePair<string, float>> list = new List<KeyValuePair<string, float>>();
				foreach (SicknessInstance sicknessInstance in minionIdentity.GetComponent<MinionModifiers>().sicknesses)
				{
					list.Add(new KeyValuePair<string, float>(sicknessInstance.modifier.Name, sicknessInstance.GetInfectedTimeRemaining()));
				}
				if (DlcManager.FeatureRadiationEnabled())
				{
					RadiationMonitor.Instance smi = minionIdentity.GetSMI<RadiationMonitor.Instance>();
					if (smi != null && smi.sm.isSick.Get(smi))
					{
						Effects component = minionIdentity.GetComponent<Effects>();
						string key;
						if (component.HasEffect(RadiationMonitor.minorSicknessEffect) || component.HasEffect(RadiationMonitor.bionic_minorSicknessEffect))
						{
							key = Db.Get().effects.Get(RadiationMonitor.minorSicknessEffect).Name;
						}
						else if (component.HasEffect(RadiationMonitor.majorSicknessEffect) || component.HasEffect(RadiationMonitor.bionic_majorSicknessEffect))
						{
							key = Db.Get().effects.Get(RadiationMonitor.majorSicknessEffect).Name;
						}
						else if (component.HasEffect(RadiationMonitor.extremeSicknessEffect) || component.HasEffect(RadiationMonitor.bionic_extremeSicknessEffect))
						{
							key = Db.Get().effects.Get(RadiationMonitor.extremeSicknessEffect).Name;
						}
						else
						{
							key = DUPLICANTS.MODIFIERS.RADIATIONEXPOSUREDEADLY.NAME;
						}
						list.Add(new KeyValuePair<string, float>(key, smi.SicknessSecondsRemaining()));
					}
				}
				if (list.Count > 0)
				{
					string text = "";
					if (list.Count > 1)
					{
						float seconds = 0f;
						foreach (KeyValuePair<string, float> keyValuePair in list)
						{
							seconds = Mathf.Min(new float[]
							{
								keyValuePair.Value
							});
						}
						text += string.Format(UI.VITALSSCREEN.MULTIPLE_SICKNESSES, GameUtil.GetFormattedCycles(seconds, "F1", false));
					}
					else
					{
						foreach (KeyValuePair<string, float> keyValuePair2 in list)
						{
							if (!string.IsNullOrEmpty(text))
							{
								text += "\n";
							}
							text += string.Format(UI.VITALSSCREEN.SICKNESS_REMAINING, keyValuePair2.Key, GameUtil.GetFormattedCycles(keyValuePair2.Value, "F1", false));
						}
					}
					return text;
				}
				return UI.VITALSSCREEN.NO_SICKNESSES;
			}
		}
		else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x06006B62 RID: 27490 RVA: 0x0028BF44 File Offset: 0x0028A144
	private int compare_rows_sicknesses(IAssignableIdentity a, IAssignableIdentity b)
	{
		float value = 0f;
		return 0f.CompareTo(value);
	}

	// Token: 0x06006B63 RID: 27491 RVA: 0x0028BF68 File Offset: 0x0028A168
	protected void on_tooltip_sicknesses(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null)
			{
				bool flag = false;
				new List<KeyValuePair<string, float>>();
				if (DlcManager.FeatureRadiationEnabled())
				{
					RadiationMonitor.Instance smi = minionIdentity.GetSMI<RadiationMonitor.Instance>();
					if (smi != null && smi.sm.isSick.Get(smi))
					{
						tooltip.AddMultiStringTooltip(smi.GetEffectStatusTooltip(), null);
						flag = true;
					}
				}
				Sicknesses sicknesses = minionIdentity.GetComponent<MinionModifiers>().sicknesses;
				if (sicknesses.IsInfected())
				{
					flag = true;
					foreach (SicknessInstance sicknessInstance in sicknesses)
					{
						tooltip.AddMultiStringTooltip(UI.HORIZONTAL_RULE, null);
						tooltip.AddMultiStringTooltip(sicknessInstance.modifier.Name, null);
						StatusItem statusItem = sicknessInstance.GetStatusItem();
						tooltip.AddMultiStringTooltip(statusItem.GetTooltip(sicknessInstance.ExposureInfo), null);
					}
				}
				if (!flag)
				{
					tooltip.AddMultiStringTooltip(UI.VITALSSCREEN.NO_SICKNESSES, null);
					return;
				}
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B64 RID: 27492 RVA: 0x0028C0A4 File Offset: 0x0028A2A4
	protected void on_tooltip_sort_sicknesses(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_SICKNESSES, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B65 RID: 27493 RVA: 0x0028C0EC File Offset: 0x0028A2EC
	private void on_load_fullness(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN_CALORIES.ToString());
	}

	// Token: 0x06006B66 RID: 27494 RVA: 0x0028C14C File Offset: 0x0028A34C
	private string get_value_fullness_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion && minion as MinionIdentity != null)
		{
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(minion as MinionIdentity);
			if (amountInstance != null)
			{
				return amountInstance.GetValueString();
			}
			return UI.TABLESCREENS.NA;
		}
		else
		{
			if (widgetRow.rowType == TableRow.RowType.StoredMinon)
			{
				return UI.TABLESCREENS.NA;
			}
			return "";
		}
	}

	// Token: 0x06006B67 RID: 27495 RVA: 0x0028C1C4 File Offset: 0x0028A3C4
	private int compare_rows_fullness(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(minionIdentity);
		AmountInstance amountInstance2 = Db.Get().Amounts.Calories.Lookup(minionIdentity2);
		if (amountInstance == null && amountInstance2 == null)
		{
			return 0;
		}
		if (amountInstance == null)
		{
			return -1;
		}
		if (amountInstance2 == null)
		{
			return 1;
		}
		float value = amountInstance.value;
		float value2 = amountInstance2.value;
		return value2.CompareTo(value);
	}

	// Token: 0x06006B68 RID: 27496 RVA: 0x0028C260 File Offset: 0x0028A460
	protected void on_tooltip_fullness(IAssignableIdentity identity, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(minionIdentity);
				if (amountInstance != null)
				{
					tooltip.AddMultiStringTooltip(amountInstance.GetTooltip(), null);
					tooltip.AddMultiStringTooltip("\n" + string.Format(UI.VITALSSCREEN.EATEN_TODAY_TOOLTIP, GameUtil.GetFormattedCalories(VitalsTableScreen.RationsEatenToday(minionIdentity), GameUtil.TimeSlice.None, true)), null);
					return;
				}
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(identity, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B69 RID: 27497 RVA: 0x0028C304 File Offset: 0x0028A504
	protected void on_tooltip_sort_fullness(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_FULLNESS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B6A RID: 27498 RVA: 0x0028C34C File Offset: 0x0028A54C
	protected void on_tooltip_name(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
		case TableRow.RowType.StoredMinon:
			break;
		case TableRow.RowType.Minion:
			if (minion != null)
			{
				tooltip.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.GOTO_DUPLICANT_BUTTON, minion.GetProperName()), null);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B6B RID: 27499 RVA: 0x0028C3A4 File Offset: 0x0028A5A4
	private void on_load_eaten_today(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN_EATENTODAY.ToString());
	}

	// Token: 0x06006B6C RID: 27500 RVA: 0x0028C404 File Offset: 0x0028A604
	private static float RationsEatenToday(MinionIdentity minion)
	{
		float result = 0f;
		if (minion != null)
		{
			RationMonitor.Instance smi = minion.GetSMI<RationMonitor.Instance>();
			if (smi != null)
			{
				result = smi.GetRationsAteToday();
			}
		}
		return result;
	}

	// Token: 0x06006B6D RID: 27501 RVA: 0x0028C434 File Offset: 0x0028A634
	private string get_value_eaten_today_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			return GameUtil.GetFormattedCalories(VitalsTableScreen.RationsEatenToday(minion as MinionIdentity), GameUtil.TimeSlice.None, true);
		}
		if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x06006B6E RID: 27502 RVA: 0x0028C480 File Offset: 0x0028A680
	private int compare_rows_eaten_today(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float value = VitalsTableScreen.RationsEatenToday(minionIdentity);
		return VitalsTableScreen.RationsEatenToday(minionIdentity2).CompareTo(value);
	}

	// Token: 0x06006B6F RID: 27503 RVA: 0x0028C4DC File Offset: 0x0028A6DC
	protected void on_tooltip_eaten_today(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
			if (minion != null)
			{
				float calories = VitalsTableScreen.RationsEatenToday(minion as MinionIdentity);
				tooltip.AddMultiStringTooltip(string.Format(UI.VITALSSCREEN.EATEN_TODAY_TOOLTIP, GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true)), null);
				return;
			}
			break;
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B70 RID: 27504 RVA: 0x0028C54C File Offset: 0x0028A74C
	protected void on_tooltip_sort_eaten_today(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_EATEN_TODAY, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B71 RID: 27505 RVA: 0x0028C594 File Offset: 0x0028A794
	private void on_load_power_banks(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN_POWERBANKS.ToString());
	}

	// Token: 0x06006B72 RID: 27506 RVA: 0x0028C5F4 File Offset: 0x0028A7F4
	private string get_value_power_banks_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null && minionIdentity.HasTag(GameTags.Minions.Models.Bionic))
			{
				return GameUtil.GetFormattedJoules(minionIdentity.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).value, "F1", GameUtil.TimeSlice.None);
			}
			return UI.TABLESCREENS.NA;
		}
		else
		{
			if (widgetRow.rowType == TableRow.RowType.StoredMinon)
			{
				return UI.TABLESCREENS.NA;
			}
			return "";
		}
	}

	// Token: 0x06006B73 RID: 27507 RVA: 0x0028C680 File Offset: 0x0028A880
	private int compare_rows_power_banks(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		float value;
		if (minionIdentity != null && minionIdentity.HasTag(GameTags.Minions.Models.Bionic))
		{
			value = minionIdentity.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).value;
		}
		else
		{
			value = -1f;
		}
		float num;
		if (minionIdentity2 != null && minionIdentity2.HasTag(GameTags.Minions.Models.Bionic))
		{
			num = minionIdentity2.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).value;
		}
		else
		{
			num = -1f;
		}
		return num.CompareTo(value);
	}

	// Token: 0x06006B74 RID: 27508 RVA: 0x0028C72C File Offset: 0x0028A92C
	protected void on_tooltip_power_banks(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null && minionIdentity != null && minionIdentity.HasTag(GameTags.Minions.Models.Bionic))
			{
				tooltip.SetSimpleTooltip(minionIdentity.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).GetDescription());
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B75 RID: 27509 RVA: 0x0028C7B8 File Offset: 0x0028A9B8
	protected void on_tooltip_sort_power_banks(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_POWERBANKS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06006B76 RID: 27510 RVA: 0x0028C800 File Offset: 0x0028AA00
	private void StoredMinionTooltip(IAssignableIdentity minion, ToolTip tooltip)
	{
		if (minion != null && minion as StoredMinionIdentity != null)
		{
			tooltip.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (minion as StoredMinionIdentity).GetStorageReason(), minion.GetProperName()), null);
		}
	}
}
