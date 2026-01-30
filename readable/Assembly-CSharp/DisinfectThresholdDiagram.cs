using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C55 RID: 3157
public class DisinfectThresholdDiagram : MonoBehaviour
{
	// Token: 0x06005FE4 RID: 24548 RVA: 0x00232F4C File Offset: 0x0023114C
	private void Start()
	{
		this.inputField.minValue = 0f;
		this.inputField.maxValue = (float)DisinfectThresholdDiagram.MAX_VALUE;
		this.inputField.currentValue = (float)SaveGame.Instance.minGermCountForDisinfect;
		this.inputField.SetDisplayValue(SaveGame.Instance.minGermCountForDisinfect.ToString());
		this.inputField.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.inputField.currentValue);
		};
		this.inputField.decimalPlaces = 1;
		this.inputField.Activate();
		this.slider.minValue = 0f;
		this.slider.maxValue = (float)(DisinfectThresholdDiagram.MAX_VALUE / DisinfectThresholdDiagram.SLIDER_CONVERSION);
		this.slider.wholeNumbers = true;
		this.slider.value = (float)(SaveGame.Instance.minGermCountForDisinfect / DisinfectThresholdDiagram.SLIDER_CONVERSION);
		this.slider.onReleaseHandle += this.OnReleaseHandle;
		this.slider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
			this.OnReleaseHandle();
		};
		this.unitsLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.UNITS);
		this.minLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.MIN_LABEL);
		this.maxLabel.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.MAX_LABEL);
		this.thresholdPrefix.SetText(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.THRESHOLD_PREFIX);
		this.toolTip.OnToolTip = delegate()
		{
			this.toolTip.ClearMultiStringTooltip();
			if (SaveGame.Instance.enableAutoDisinfect)
			{
				this.toolTip.AddMultiStringTooltip(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.TOOLTIP.ToString().Replace("{NumberOfGerms}", SaveGame.Instance.minGermCountForDisinfect.ToString()), null);
			}
			else
			{
				this.toolTip.AddMultiStringTooltip(UI.OVERLAYS.DISEASE.DISINFECT_THRESHOLD_DIAGRAM.TOOLTIP_DISABLED.ToString(), null);
			}
			return "";
		};
		this.disabledImage.gameObject.SetActive(!SaveGame.Instance.enableAutoDisinfect);
		this.toggle.isOn = SaveGame.Instance.enableAutoDisinfect;
		this.toggle.onValueChanged += this.OnClickToggle;
	}

	// Token: 0x06005FE5 RID: 24549 RVA: 0x00233138 File Offset: 0x00231338
	private void OnReleaseHandle()
	{
		float num = (float)((int)this.slider.value * DisinfectThresholdDiagram.SLIDER_CONVERSION);
		SaveGame.Instance.minGermCountForDisinfect = (int)num;
		this.inputField.SetDisplayValue(num.ToString());
	}

	// Token: 0x06005FE6 RID: 24550 RVA: 0x00233178 File Offset: 0x00231378
	private void ReceiveValueFromSlider(float new_value)
	{
		SaveGame.Instance.minGermCountForDisinfect = (int)new_value * DisinfectThresholdDiagram.SLIDER_CONVERSION;
		this.inputField.SetDisplayValue((new_value * (float)DisinfectThresholdDiagram.SLIDER_CONVERSION).ToString());
	}

	// Token: 0x06005FE7 RID: 24551 RVA: 0x002331B2 File Offset: 0x002313B2
	private void ReceiveValueFromInput(float new_value)
	{
		this.slider.value = new_value / (float)DisinfectThresholdDiagram.SLIDER_CONVERSION;
		SaveGame.Instance.minGermCountForDisinfect = (int)new_value;
	}

	// Token: 0x06005FE8 RID: 24552 RVA: 0x002331D3 File Offset: 0x002313D3
	private void OnClickToggle(bool new_value)
	{
		SaveGame.Instance.enableAutoDisinfect = new_value;
		this.disabledImage.gameObject.SetActive(!SaveGame.Instance.enableAutoDisinfect);
	}

	// Token: 0x04004010 RID: 16400
	[SerializeField]
	private KNumberInputField inputField;

	// Token: 0x04004011 RID: 16401
	[SerializeField]
	private KSlider slider;

	// Token: 0x04004012 RID: 16402
	[SerializeField]
	private LocText minLabel;

	// Token: 0x04004013 RID: 16403
	[SerializeField]
	private LocText maxLabel;

	// Token: 0x04004014 RID: 16404
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04004015 RID: 16405
	[SerializeField]
	private LocText thresholdPrefix;

	// Token: 0x04004016 RID: 16406
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x04004017 RID: 16407
	[SerializeField]
	private KToggle toggle;

	// Token: 0x04004018 RID: 16408
	[SerializeField]
	private Image disabledImage;

	// Token: 0x04004019 RID: 16409
	private static int MAX_VALUE = 1000000;

	// Token: 0x0400401A RID: 16410
	private static int SLIDER_CONVERSION = 1000;
}
