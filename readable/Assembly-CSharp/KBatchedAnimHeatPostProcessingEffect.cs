using System;
using UnityEngine;

// Token: 0x02000553 RID: 1363
public class KBatchedAnimHeatPostProcessingEffect : KMonoBehaviour
{
	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06001E41 RID: 7745 RVA: 0x000A4150 File Offset: 0x000A2350
	public float HeatProduction
	{
		get
		{
			return this.heatProduction;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06001E42 RID: 7746 RVA: 0x000A4158 File Offset: 0x000A2358
	public bool IsHeatProductionEnoughToShowEffect
	{
		get
		{
			return this.HeatProduction >= 1f;
		}
	}

	// Token: 0x06001E43 RID: 7747 RVA: 0x000A416A File Offset: 0x000A236A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.animController.postProcessingEffectsAllowed |= KAnimConverter.PostProcessingEffects.TemperatureOverlay;
	}

	// Token: 0x06001E44 RID: 7748 RVA: 0x000A4185 File Offset: 0x000A2385
	public void SetHeatBeingProducedValue(float heat)
	{
		this.heatProduction = heat;
		this.RefreshEffectVisualState();
	}

	// Token: 0x06001E45 RID: 7749 RVA: 0x000A4194 File Offset: 0x000A2394
	public void RefreshEffectVisualState()
	{
		if (base.enabled && this.IsHeatProductionEnoughToShowEffect)
		{
			this.SetParameterValue(1f);
			return;
		}
		this.SetParameterValue(0f);
	}

	// Token: 0x06001E46 RID: 7750 RVA: 0x000A41BD File Offset: 0x000A23BD
	private void SetParameterValue(float value)
	{
		if (this.animController != null)
		{
			this.animController.postProcessingParameters = value;
		}
	}

	// Token: 0x06001E47 RID: 7751 RVA: 0x000A41D9 File Offset: 0x000A23D9
	protected override void OnCmpEnable()
	{
		this.RefreshEffectVisualState();
	}

	// Token: 0x06001E48 RID: 7752 RVA: 0x000A41E1 File Offset: 0x000A23E1
	protected override void OnCmpDisable()
	{
		this.RefreshEffectVisualState();
	}

	// Token: 0x06001E49 RID: 7753 RVA: 0x000A41EC File Offset: 0x000A23EC
	private void Update()
	{
		int num = Mathf.FloorToInt(Time.timeSinceLevelLoad / 1f);
		if (num != this.loopsPlayed)
		{
			this.loopsPlayed = num;
			this.OnNewLoopReached();
		}
	}

	// Token: 0x06001E4A RID: 7754 RVA: 0x000A4220 File Offset: 0x000A2420
	private void OnNewLoopReached()
	{
		if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode == OverlayModes.Temperature.ID && this.IsHeatProductionEnoughToShowEffect)
		{
			Vector3 position = base.transform.GetPosition();
			string sound = GlobalAssets.GetSound("Temperature_Heat_Emission", false);
			position.z = 0f;
			SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, position, 1f, false));
		}
	}

	// Token: 0x040011A3 RID: 4515
	public const float SHOW_EFFECT_HEAT_TRESHOLD = 1f;

	// Token: 0x040011A4 RID: 4516
	private const float DISABLING_VALUE = 0f;

	// Token: 0x040011A5 RID: 4517
	private const float ENABLING_VALUE = 1f;

	// Token: 0x040011A6 RID: 4518
	private float heatProduction;

	// Token: 0x040011A7 RID: 4519
	public const float ANIM_DURATION = 1f;

	// Token: 0x040011A8 RID: 4520
	private int loopsPlayed;

	// Token: 0x040011A9 RID: 4521
	[MyCmpGet]
	private KBatchedAnimController animController;
}
