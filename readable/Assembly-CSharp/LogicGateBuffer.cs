using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200079A RID: 1946
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGateBuffer : LogicGate, ISingleSliderControl, ISliderControl
{
	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x0600324E RID: 12878 RVA: 0x001225D7 File Offset: 0x001207D7
	// (set) Token: 0x0600324F RID: 12879 RVA: 0x001225E0 File Offset: 0x001207E0
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

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06003250 RID: 12880 RVA: 0x0012260B File Offset: 0x0012080B
	private int DelayAmountTicks
	{
		get
		{
			return Mathf.RoundToInt(this.delayAmount / LogicCircuitManager.ClockTickInterval);
		}
	}

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06003251 RID: 12881 RVA: 0x0012261E File Offset: 0x0012081E
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06003252 RID: 12882 RVA: 0x00122625 File Offset: 0x00120825
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.SECOND;
		}
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x00122631 File Offset: 0x00120831
	public int SliderDecimalPlaces(int index)
	{
		return 1;
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x00122634 File Offset: 0x00120834
	public float GetSliderMin(int index)
	{
		return 0.1f;
	}

	// Token: 0x06003255 RID: 12885 RVA: 0x0012263B File Offset: 0x0012083B
	public float GetSliderMax(int index)
	{
		return 200f;
	}

	// Token: 0x06003256 RID: 12886 RVA: 0x00122642 File Offset: 0x00120842
	public float GetSliderValue(int index)
	{
		return this.DelayAmount;
	}

	// Token: 0x06003257 RID: 12887 RVA: 0x0012264A File Offset: 0x0012084A
	public void SetSliderValue(float value, int index)
	{
		this.DelayAmount = value;
	}

	// Token: 0x06003258 RID: 12888 RVA: 0x00122653 File Offset: 0x00120853
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x06003259 RID: 12889 RVA: 0x0012265A File Offset: 0x0012085A
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.LOGIC_BUFFER_SIDE_SCREEN.TOOLTIP"), this.DelayAmount);
	}

	// Token: 0x0600325A RID: 12890 RVA: 0x0012267B File Offset: 0x0012087B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicGateBuffer>(-905833192, LogicGateBuffer.OnCopySettingsDelegate);
	}

	// Token: 0x0600325B RID: 12891 RVA: 0x00122694 File Offset: 0x00120894
	private void OnCopySettings(object data)
	{
		LogicGateBuffer component = ((GameObject)data).GetComponent<LogicGateBuffer>();
		if (component != null)
		{
			this.DelayAmount = component.DelayAmount;
		}
	}

	// Token: 0x0600325C RID: 12892 RVA: 0x001226C4 File Offset: 0x001208C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.meter.SetPositionPercent(1f);
	}

	// Token: 0x0600325D RID: 12893 RVA: 0x00122710 File Offset: 0x00120910
	private void Update()
	{
		float positionPercent;
		if (this.input_was_previously_positive)
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

	// Token: 0x0600325E RID: 12894 RVA: 0x00122767 File Offset: 0x00120967
	public override void LogicTick()
	{
		if (!this.input_was_previously_positive && this.delayTicksRemaining > 0)
		{
			this.delayTicksRemaining--;
			if (this.delayTicksRemaining <= 0)
			{
				this.OnDelay();
			}
		}
	}

	// Token: 0x0600325F RID: 12895 RVA: 0x00122798 File Offset: 0x00120998
	protected override int GetCustomValue(int val1, int val2)
	{
		if (val1 != 0)
		{
			this.input_was_previously_positive = true;
			this.delayTicksRemaining = 0;
			this.meter.SetPositionPercent(0f);
		}
		else if (this.delayTicksRemaining <= 0)
		{
			if (this.input_was_previously_positive)
			{
				this.delayTicksRemaining = this.DelayAmountTicks;
			}
			this.input_was_previously_positive = false;
		}
		if (val1 == 0 && this.delayTicksRemaining <= 0)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x06003260 RID: 12896 RVA: 0x001227FC File Offset: 0x001209FC
	private void OnDelay()
	{
		if (this.cleaningUp)
		{
			return;
		}
		this.delayTicksRemaining = 0;
		this.meter.SetPositionPercent(1f);
		if (this.outputValueOne == 0)
		{
			return;
		}
		int outputCellOne = base.OutputCellOne;
		if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
		{
			return;
		}
		this.outputValueOne = 0;
		base.RefreshAnimation();
	}

	// Token: 0x04001E90 RID: 7824
	[Serialize]
	private bool input_was_previously_positive;

	// Token: 0x04001E91 RID: 7825
	[Serialize]
	private float delayAmount = 5f;

	// Token: 0x04001E92 RID: 7826
	[Serialize]
	private int delayTicksRemaining;

	// Token: 0x04001E93 RID: 7827
	private MeterController meter;

	// Token: 0x04001E94 RID: 7828
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001E95 RID: 7829
	private static readonly EventSystem.IntraObjectHandler<LogicGateBuffer> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicGateBuffer>(delegate(LogicGateBuffer component, object data)
	{
		component.OnCopySettings(data);
	});
}
