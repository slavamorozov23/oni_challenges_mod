using System;
using System.Collections;
using STRINGS;
using UnityEngine;

// Token: 0x02000E8F RID: 3727
public class ValveSideScreen : SideScreenContent
{
	// Token: 0x060076F7 RID: 30455 RVA: 0x002D59D0 File Offset: 0x002D3BD0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = GameUtil.AddTimeSliceText(UI.UNITSUFFIXES.MASS.GRAM, GameUtil.TimeSlice.PerSecond);
		this.flowSlider.onReleaseHandle += this.OnReleaseHandle;
		this.flowSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
		};
		this.flowSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
		};
		this.flowSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
			this.OnReleaseHandle();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x060076F8 RID: 30456 RVA: 0x002D5A7D File Offset: 0x002D3C7D
	public void OnReleaseHandle()
	{
		this.targetValve.ChangeFlow(this.targetFlow);
	}

	// Token: 0x060076F9 RID: 30457 RVA: 0x002D5A90 File Offset: 0x002D3C90
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Valve>() != null;
	}

	// Token: 0x060076FA RID: 30458 RVA: 0x002D5AA0 File Offset: 0x002D3CA0
	public override void SetTarget(GameObject target)
	{
		this.targetValve = target.GetComponent<Valve>();
		if (this.targetValve == null)
		{
			global::Debug.LogError("The target object does not have a Valve component.");
			return;
		}
		this.flowSlider.minValue = 0f;
		this.flowSlider.maxValue = this.targetValve.MaxFlow;
		this.flowSlider.value = this.targetValve.DesiredFlow;
		this.minFlowLabel.text = GameUtil.GetFormattedMass(0f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
		this.maxFlowLabel.text = GameUtil.GetFormattedMass(this.targetValve.MaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
		this.numberInput.minValue = 0f;
		this.numberInput.maxValue = this.targetValve.MaxFlow * 1000f;
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0f, this.targetValve.DesiredFlow), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
		this.numberInput.Activate();
	}

	// Token: 0x060076FB RID: 30459 RVA: 0x002D5BB2 File Offset: 0x002D3DB2
	private void ReceiveValueFromSlider(float newValue)
	{
		newValue = Mathf.Round(newValue * 1000f) / 1000f;
		this.UpdateFlowValue(newValue);
	}

	// Token: 0x060076FC RID: 30460 RVA: 0x002D5BD0 File Offset: 0x002D3DD0
	private void ReceiveValueFromInput(float input)
	{
		float newValue = input / 1000f;
		this.UpdateFlowValue(newValue);
		this.targetValve.ChangeFlow(this.targetFlow);
	}

	// Token: 0x060076FD RID: 30461 RVA: 0x002D5BFD File Offset: 0x002D3DFD
	private void UpdateFlowValue(float newValue)
	{
		this.targetFlow = newValue;
		this.flowSlider.value = newValue;
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
	}

	// Token: 0x060076FE RID: 30462 RVA: 0x002D5C2B File Offset: 0x002D3E2B
	private IEnumerator SettingDelay(float delay)
	{
		float startTime = Time.realtimeSinceStartup;
		float currentTime = startTime;
		while (currentTime < startTime + delay)
		{
			currentTime += Time.unscaledDeltaTime;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.OnReleaseHandle();
		yield break;
	}

	// Token: 0x04005252 RID: 21074
	private Valve targetValve;

	// Token: 0x04005253 RID: 21075
	[Header("Slider")]
	[SerializeField]
	private KSlider flowSlider;

	// Token: 0x04005254 RID: 21076
	[SerializeField]
	private LocText minFlowLabel;

	// Token: 0x04005255 RID: 21077
	[SerializeField]
	private LocText maxFlowLabel;

	// Token: 0x04005256 RID: 21078
	[Header("Input Field")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04005257 RID: 21079
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04005258 RID: 21080
	private float targetFlow;
}
