using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000E32 RID: 3634
public class CounterSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x06007353 RID: 29523 RVA: 0x002C0FF7 File Offset: 0x002BF1F7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06007354 RID: 29524 RVA: 0x002C1000 File Offset: 0x002BF200
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetButton.onClick += this.ResetCounter;
		this.incrementMaxButton.onClick += this.IncrementMaxCount;
		this.decrementMaxButton.onClick += this.DecrementMaxCount;
		this.incrementModeButton.onClick += this.ToggleMode;
		this.advancedModeToggle.onClick += this.ToggleAdvanced;
		this.maxCountInput.onEndEdit += delegate()
		{
			this.UpdateMaxCountFromTextInput(this.maxCountInput.currentValue);
		};
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
	}

	// Token: 0x06007355 RID: 29525 RVA: 0x002C10AE File Offset: 0x002BF2AE
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicCounter>() != null;
	}

	// Token: 0x06007356 RID: 29526 RVA: 0x002C10BC File Offset: 0x002BF2BC
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.maxCountInput.minValue = 1f;
		this.maxCountInput.maxValue = 10f;
		this.targetLogicCounter = target.GetComponent<LogicCounter>();
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
		this.UpdateMaxCountLabel(this.targetLogicCounter.maxCount);
		this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
	}

	// Token: 0x06007357 RID: 29527 RVA: 0x002C1134 File Offset: 0x002BF334
	public void Render200ms(float dt)
	{
		if (this.targetLogicCounter == null)
		{
			return;
		}
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
	}

	// Token: 0x06007358 RID: 29528 RVA: 0x002C1158 File Offset: 0x002BF358
	private void UpdateCurrentCountLabel(int value)
	{
		string text = value.ToString();
		if (value == this.targetLogicCounter.maxCount)
		{
			text = UI.FormatAsAutomationState(text, UI.AutomationState.Active);
		}
		else
		{
			text = UI.FormatAsAutomationState(text, UI.AutomationState.Standby);
		}
		this.currentCount.text = (this.targetLogicCounter.advancedMode ? string.Format(UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_ADVANCED, text) : string.Format(UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_SIMPLE, text));
	}

	// Token: 0x06007359 RID: 29529 RVA: 0x002C11C7 File Offset: 0x002BF3C7
	private void UpdateMaxCountLabel(int value)
	{
		this.maxCountInput.SetAmount((float)value);
	}

	// Token: 0x0600735A RID: 29530 RVA: 0x002C11D6 File Offset: 0x002BF3D6
	private void UpdateMaxCountFromTextInput(float newValue)
	{
		this.SetMaxCount((int)newValue);
	}

	// Token: 0x0600735B RID: 29531 RVA: 0x002C11E0 File Offset: 0x002BF3E0
	private void IncrementMaxCount()
	{
		this.SetMaxCount(this.targetLogicCounter.maxCount + 1);
	}

	// Token: 0x0600735C RID: 29532 RVA: 0x002C11F5 File Offset: 0x002BF3F5
	private void DecrementMaxCount()
	{
		this.SetMaxCount(this.targetLogicCounter.maxCount - 1);
	}

	// Token: 0x0600735D RID: 29533 RVA: 0x002C120C File Offset: 0x002BF40C
	private void SetMaxCount(int newValue)
	{
		if (newValue > 10)
		{
			newValue = 1;
		}
		if (newValue < 1)
		{
			newValue = 10;
		}
		if (newValue < this.targetLogicCounter.currentCount)
		{
			this.targetLogicCounter.currentCount = newValue;
		}
		this.targetLogicCounter.maxCount = newValue;
		this.UpdateCounterStates();
		this.UpdateMaxCountLabel(newValue);
	}

	// Token: 0x0600735E RID: 29534 RVA: 0x002C125C File Offset: 0x002BF45C
	private void ResetCounter()
	{
		this.targetLogicCounter.ResetCounter();
	}

	// Token: 0x0600735F RID: 29535 RVA: 0x002C1269 File Offset: 0x002BF469
	private void UpdateCounterStates()
	{
		this.targetLogicCounter.SetCounterState();
		this.targetLogicCounter.UpdateLogicCircuit();
		this.targetLogicCounter.UpdateVisualState(true);
		this.targetLogicCounter.UpdateMeter();
	}

	// Token: 0x06007360 RID: 29536 RVA: 0x002C1298 File Offset: 0x002BF498
	private void ToggleMode()
	{
	}

	// Token: 0x06007361 RID: 29537 RVA: 0x002C129C File Offset: 0x002BF49C
	private void ToggleAdvanced()
	{
		this.targetLogicCounter.advancedMode = !this.targetLogicCounter.advancedMode;
		this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
		this.UpdateCounterStates();
	}

	// Token: 0x04004FC4 RID: 20420
	public LogicCounter targetLogicCounter;

	// Token: 0x04004FC5 RID: 20421
	public KButton resetButton;

	// Token: 0x04004FC6 RID: 20422
	public KButton incrementMaxButton;

	// Token: 0x04004FC7 RID: 20423
	public KButton decrementMaxButton;

	// Token: 0x04004FC8 RID: 20424
	public KButton incrementModeButton;

	// Token: 0x04004FC9 RID: 20425
	public KToggle advancedModeToggle;

	// Token: 0x04004FCA RID: 20426
	public KImage advancedModeCheckmark;

	// Token: 0x04004FCB RID: 20427
	public LocText currentCount;

	// Token: 0x04004FCC RID: 20428
	[SerializeField]
	private KNumberInputField maxCountInput;
}
