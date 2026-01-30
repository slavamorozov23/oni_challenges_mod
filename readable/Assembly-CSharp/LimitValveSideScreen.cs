using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E49 RID: 3657
public class LimitValveSideScreen : SideScreenContent
{
	// Token: 0x060073EB RID: 29675 RVA: 0x002C4318 File Offset: 0x002C2518
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetButton.onClick += this.ResetCounter;
		this.limitSlider.onReleaseHandle += this.OnReleaseHandle;
		this.limitSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.limitSlider.value);
		};
		this.limitSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.limitSlider.value);
		};
		this.limitSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.limitSlider.value);
			this.OnReleaseHandle();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 3;
	}

	// Token: 0x060073EC RID: 29676 RVA: 0x002C43C1 File Offset: 0x002C25C1
	public void OnReleaseHandle()
	{
		this.targetLimitValve.Limit = this.targetLimit;
	}

	// Token: 0x060073ED RID: 29677 RVA: 0x002C43D4 File Offset: 0x002C25D4
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LimitValve>() != null;
	}

	// Token: 0x060073EE RID: 29678 RVA: 0x002C43E4 File Offset: 0x002C25E4
	public override void SetTarget(GameObject target)
	{
		bool flag = true;
		if (this.targetLimitValve != null && this.targetLimitValve.gameObject == target)
		{
			flag = false;
		}
		this.targetLimitValve = target.GetComponent<LimitValve>();
		if (this.targetLimitValve == null)
		{
			global::Debug.LogError("The target object does not have a LimitValve component.");
			return;
		}
		if (this.targetLimitValveSubHandle != -1)
		{
			base.Unsubscribe(this.targetLimitValveSubHandle);
		}
		this.targetLimitValveSubHandle = this.targetLimitValve.Subscribe(-1722241721, new Action<object>(this.UpdateAmountLabel));
		this.limitSlider.minValue = 0f;
		this.limitSlider.maxValue = 100f;
		this.limitSlider.SetRanges(this.targetLimitValve.GetRanges());
		this.limitSlider.value = this.limitSlider.GetPercentageFromValue(this.targetLimitValve.Limit);
		this.numberInput.minValue = 0f;
		this.numberInput.maxValue = this.targetLimitValve.maxLimitKg;
		this.numberInput.Activate();
		if (this.targetLimitValve.displayUnitsInsteadOfMass)
		{
			this.minLimitLabel.text = GameUtil.GetFormattedUnits(0f, GameUtil.TimeSlice.None, true, "");
			this.maxLimitLabel.text = GameUtil.GetFormattedUnits(this.targetLimitValve.maxLimitKg, GameUtil.TimeSlice.None, true, "");
			if (flag)
			{
				this.numberInput.SetDisplayValue(GameUtil.GetFormattedUnits(Mathf.Max(0f, this.targetLimitValve.Limit), GameUtil.TimeSlice.None, false, LimitValveSideScreen.FLOAT_FORMAT));
			}
			this.unitsLabel.text = UI.UNITSUFFIXES.UNITS;
			this.toolTip.enabled = true;
			this.toolTip.SetSimpleTooltip(UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.SLIDER_TOOLTIP_UNITS);
		}
		else
		{
			this.minLimitLabel.text = GameUtil.GetFormattedMass(0f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}");
			this.maxLimitLabel.text = GameUtil.GetFormattedMass(this.targetLimitValve.maxLimitKg, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}");
			if (flag)
			{
				this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0f, this.targetLimitValve.Limit), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, false, LimitValveSideScreen.FLOAT_FORMAT));
			}
			this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
			this.toolTip.enabled = false;
		}
		this.UpdateAmountLabel(null);
	}

	// Token: 0x060073EF RID: 29679 RVA: 0x002C464C File Offset: 0x002C284C
	private void UpdateAmountLabel(object _ = null)
	{
		if (this.targetLimitValve.displayUnitsInsteadOfMass)
		{
			string formattedUnits = GameUtil.GetFormattedUnits(this.targetLimitValve.Amount, GameUtil.TimeSlice.None, true, LimitValveSideScreen.FLOAT_FORMAT);
			this.amountLabel.text = string.Format(UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.AMOUNT, formattedUnits);
			return;
		}
		string formattedMass = GameUtil.GetFormattedMass(this.targetLimitValve.Amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, LimitValveSideScreen.FLOAT_FORMAT);
		this.amountLabel.text = string.Format(UI.UISIDESCREENS.LIMIT_VALVE_SIDE_SCREEN.AMOUNT, formattedMass);
	}

	// Token: 0x060073F0 RID: 29680 RVA: 0x002C46CE File Offset: 0x002C28CE
	private void ResetCounter()
	{
		this.targetLimitValve.ResetAmount();
	}

	// Token: 0x060073F1 RID: 29681 RVA: 0x002C46DC File Offset: 0x002C28DC
	private void ReceiveValueFromSlider(float sliderPercentage)
	{
		float num = this.limitSlider.GetValueForPercentage(sliderPercentage);
		num = (float)Mathf.RoundToInt(num);
		this.UpdateLimitValue(num);
	}

	// Token: 0x060073F2 RID: 29682 RVA: 0x002C4705 File Offset: 0x002C2905
	private void ReceiveValueFromInput(float input)
	{
		this.UpdateLimitValue(input);
		this.targetLimitValve.Limit = this.targetLimit;
	}

	// Token: 0x060073F3 RID: 29683 RVA: 0x002C4720 File Offset: 0x002C2920
	private void UpdateLimitValue(float newValue)
	{
		this.targetLimit = newValue;
		this.limitSlider.value = this.limitSlider.GetPercentageFromValue(newValue);
		if (this.targetLimitValve.displayUnitsInsteadOfMass)
		{
			this.numberInput.SetDisplayValue(GameUtil.GetFormattedUnits(newValue, GameUtil.TimeSlice.None, false, LimitValveSideScreen.FLOAT_FORMAT));
			return;
		}
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, false, LimitValveSideScreen.FLOAT_FORMAT));
	}

	// Token: 0x0400502D RID: 20525
	public static readonly string FLOAT_FORMAT = "{0:0.#####}";

	// Token: 0x0400502E RID: 20526
	private LimitValve targetLimitValve;

	// Token: 0x0400502F RID: 20527
	[Header("State")]
	[SerializeField]
	private LocText amountLabel;

	// Token: 0x04005030 RID: 20528
	[SerializeField]
	private KButton resetButton;

	// Token: 0x04005031 RID: 20529
	[Header("Slider")]
	[SerializeField]
	private NonLinearSlider limitSlider;

	// Token: 0x04005032 RID: 20530
	[SerializeField]
	private LocText minLimitLabel;

	// Token: 0x04005033 RID: 20531
	[SerializeField]
	private LocText maxLimitLabel;

	// Token: 0x04005034 RID: 20532
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x04005035 RID: 20533
	[Header("Input Field")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04005036 RID: 20534
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04005037 RID: 20535
	private float targetLimit;

	// Token: 0x04005038 RID: 20536
	private int targetLimitValveSubHandle = -1;
}
