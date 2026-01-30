using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200079B RID: 1947
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGateFilter : LogicGate, ISingleSliderControl, ISliderControl
{
	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06003263 RID: 12899 RVA: 0x0012288D File Offset: 0x00120A8D
	// (set) Token: 0x06003264 RID: 12900 RVA: 0x00122898 File Offset: 0x00120A98
	public float DelayAmount
	{
		get
		{
			return this.delayAmount;
		}
		set
		{
			this.delayAmount = value;
			int delayAmountTicks = this.DelayAmountTicks;
			if (this.delayTicksRemaining > delayAmountTicks)
			{
				this.delayTicksRemaining = delayAmountTicks;
			}
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06003265 RID: 12901 RVA: 0x001228C3 File Offset: 0x00120AC3
	private int DelayAmountTicks
	{
		get
		{
			return Mathf.RoundToInt(this.delayAmount / LogicCircuitManager.ClockTickInterval);
		}
	}

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06003266 RID: 12902 RVA: 0x001228D6 File Offset: 0x00120AD6
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06003267 RID: 12903 RVA: 0x001228DD File Offset: 0x00120ADD
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.SECOND;
		}
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x001228E9 File Offset: 0x00120AE9
	public int SliderDecimalPlaces(int index)
	{
		return 1;
	}

	// Token: 0x06003269 RID: 12905 RVA: 0x001228EC File Offset: 0x00120AEC
	public float GetSliderMin(int index)
	{
		return 0.1f;
	}

	// Token: 0x0600326A RID: 12906 RVA: 0x001228F3 File Offset: 0x00120AF3
	public float GetSliderMax(int index)
	{
		return 200f;
	}

	// Token: 0x0600326B RID: 12907 RVA: 0x001228FA File Offset: 0x00120AFA
	public float GetSliderValue(int index)
	{
		return this.DelayAmount;
	}

	// Token: 0x0600326C RID: 12908 RVA: 0x00122902 File Offset: 0x00120B02
	public void SetSliderValue(float value, int index)
	{
		this.DelayAmount = value;
	}

	// Token: 0x0600326D RID: 12909 RVA: 0x0012290B File Offset: 0x00120B0B
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x0600326E RID: 12910 RVA: 0x00122912 File Offset: 0x00120B12
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.LOGIC_FILTER_SIDE_SCREEN.TOOLTIP"), this.DelayAmount);
	}

	// Token: 0x0600326F RID: 12911 RVA: 0x00122933 File Offset: 0x00120B33
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicGateFilter>(-905833192, LogicGateFilter.OnCopySettingsDelegate);
	}

	// Token: 0x06003270 RID: 12912 RVA: 0x0012294C File Offset: 0x00120B4C
	private void OnCopySettings(object data)
	{
		LogicGateFilter component = ((GameObject)data).GetComponent<LogicGateFilter>();
		if (component != null)
		{
			this.DelayAmount = component.DelayAmount;
		}
	}

	// Token: 0x06003271 RID: 12913 RVA: 0x0012297C File Offset: 0x00120B7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.meter.SetPositionPercent(0f);
	}

	// Token: 0x06003272 RID: 12914 RVA: 0x001229C8 File Offset: 0x00120BC8
	private void Update()
	{
		float positionPercent;
		if (this.input_was_previously_negative)
		{
			positionPercent = 0f;
		}
		else if (this.delayTicksRemaining > 0)
		{
			positionPercent = (float)(this.DelayAmountTicks - this.delayTicksRemaining) / (float)this.DelayAmountTicks;
		}
		else
		{
			positionPercent = 1f;
		}
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06003273 RID: 12915 RVA: 0x00122A1F File Offset: 0x00120C1F
	public override void LogicTick()
	{
		if (!this.input_was_previously_negative && this.delayTicksRemaining > 0)
		{
			this.delayTicksRemaining--;
			if (this.delayTicksRemaining <= 0)
			{
				this.OnDelay();
			}
		}
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x00122A50 File Offset: 0x00120C50
	protected override int GetCustomValue(int val1, int val2)
	{
		if (val1 == 0)
		{
			this.input_was_previously_negative = true;
			this.delayTicksRemaining = 0;
			this.meter.SetPositionPercent(1f);
		}
		else if (this.delayTicksRemaining <= 0)
		{
			if (this.input_was_previously_negative)
			{
				this.delayTicksRemaining = this.DelayAmountTicks;
			}
			this.input_was_previously_negative = false;
		}
		if (val1 != 0 && this.delayTicksRemaining <= 0)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x00122AB4 File Offset: 0x00120CB4
	private void OnDelay()
	{
		if (this.cleaningUp)
		{
			return;
		}
		this.delayTicksRemaining = 0;
		this.meter.SetPositionPercent(0f);
		if (this.outputValueOne == 1)
		{
			return;
		}
		int outputCellOne = base.OutputCellOne;
		if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
		{
			return;
		}
		this.outputValueOne = 1;
		base.RefreshAnimation();
	}

	// Token: 0x04001E96 RID: 7830
	[Serialize]
	private bool input_was_previously_negative;

	// Token: 0x04001E97 RID: 7831
	[Serialize]
	private float delayAmount = 5f;

	// Token: 0x04001E98 RID: 7832
	[Serialize]
	private int delayTicksRemaining;

	// Token: 0x04001E99 RID: 7833
	private MeterController meter;

	// Token: 0x04001E9A RID: 7834
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001E9B RID: 7835
	private static readonly EventSystem.IntraObjectHandler<LogicGateFilter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateFilter>(delegate(LogicGateFilter component, object data)
	{
		component.OnCopySettings(data);
	});
}
