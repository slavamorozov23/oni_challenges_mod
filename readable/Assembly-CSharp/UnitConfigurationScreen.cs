using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000EBF RID: 3775
[Serializable]
public class UnitConfigurationScreen
{
	// Token: 0x060078F1 RID: 30961 RVA: 0x002E7E90 File Offset: 0x002E6090
	public void Init()
	{
		this.celsiusToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.celsiusToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.CELSIUS_TOOLTIP;
		this.celsiusToggle.GetComponentInChildren<KButton>().onClick += this.OnCelsiusClicked;
		this.celsiusToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.CELSIUS;
		this.kelvinToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.kelvinToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.KELVIN_TOOLTIP;
		this.kelvinToggle.GetComponentInChildren<KButton>().onClick += this.OnKelvinClicked;
		this.kelvinToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.KELVIN;
		this.fahrenheitToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.fahrenheitToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.FAHRENHEIT_TOOLTIP;
		this.fahrenheitToggle.GetComponentInChildren<KButton>().onClick += this.OnFahrenheitClicked;
		this.fahrenheitToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.FAHRENHEIT;
		this.DisplayCurrentUnit();
	}

	// Token: 0x060078F2 RID: 30962 RVA: 0x002E7FDC File Offset: 0x002E61DC
	private void DisplayCurrentUnit()
	{
		GameUtil.TemperatureUnit @int = (GameUtil.TemperatureUnit)KPlayerPrefs.GetInt(UnitConfigurationScreen.TemperatureUnitKey, 0);
		if (@int == GameUtil.TemperatureUnit.Celsius)
		{
			this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
			this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			return;
		}
		if (@int != GameUtil.TemperatureUnit.Kelvin)
		{
			this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
			return;
		}
		this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
		this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
		this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
	}

	// Token: 0x060078F3 RID: 30963 RVA: 0x002E8124 File Offset: 0x002E6324
	private void OnCelsiusClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Celsius;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.BoxingTrigger<GameUtil.TemperatureUnit>(999382396, GameUtil.TemperatureUnit.Celsius);
		}
	}

	// Token: 0x060078F4 RID: 30964 RVA: 0x002E8174 File Offset: 0x002E6374
	private void OnKelvinClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Kelvin;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.BoxingTrigger<GameUtil.TemperatureUnit>(999382396, GameUtil.TemperatureUnit.Kelvin);
		}
	}

	// Token: 0x060078F5 RID: 30965 RVA: 0x002E81C4 File Offset: 0x002E63C4
	private void OnFahrenheitClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Fahrenheit;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.BoxingTrigger<GameUtil.TemperatureUnit>(999382396, GameUtil.TemperatureUnit.Fahrenheit);
		}
	}

	// Token: 0x04005447 RID: 21575
	[SerializeField]
	private GameObject toggleUnitPrefab;

	// Token: 0x04005448 RID: 21576
	[SerializeField]
	private GameObject toggleGroup;

	// Token: 0x04005449 RID: 21577
	private GameObject celsiusToggle;

	// Token: 0x0400544A RID: 21578
	private GameObject kelvinToggle;

	// Token: 0x0400544B RID: 21579
	private GameObject fahrenheitToggle;

	// Token: 0x0400544C RID: 21580
	public static readonly string TemperatureUnitKey = "TemperatureUnit";

	// Token: 0x0400544D RID: 21581
	public static readonly string MassUnitKey = "MassUnit";
}
