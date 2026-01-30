using System;
using UnityEngine;

// Token: 0x02000366 RID: 870
public class MorbRoverMaker_Capsule : KMonoBehaviour
{
	// Token: 0x06001223 RID: 4643 RVA: 0x00069BCC File Offset: 0x00067DCC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.MorbDevelopment_Meter = new MeterController(this.buildingAnimCtr, "meter_morb_target", "meter_morb_1", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.GermMeter = new MeterController(this.buildingAnimCtr, "meter_germs_target", "meter_germs", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.MorbDevelopment_Capsule_Meter = new MeterController(this.buildingAnimCtr, "meter_capsule_target", "meter_capsule", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.MorbDevelopment_Capsule_Meter.meterController.onAnimComplete += this.OnGermAddedAnimationComplete;
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x00069C64 File Offset: 0x00067E64
	private void OnGermAddedAnimationComplete(HashedString animName)
	{
		if (animName == MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME)
		{
			this.MorbDevelopment_Capsule_Meter.meterController.Play("meter_capsule", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x00069C98 File Offset: 0x00067E98
	public void PlayPumpGermsAnimation()
	{
		if (this.MorbDevelopment_Capsule_Meter.meterController.currentAnim != MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME)
		{
			this.MorbDevelopment_Capsule_Meter.meterController.Play(MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME, KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x00069CD8 File Offset: 0x00067ED8
	public void SetMorbDevelopmentProgress(float morbDevelopmentProgress)
	{
		global::Debug.Assert(true, "MORB PHASES COUNT needs to be larger than 0");
		string s = "meter_morb_" + (1 + Mathf.FloorToInt(morbDevelopmentProgress * 4f)).ToString();
		if (this.MorbDevelopment_Meter.meterController.currentAnim != s)
		{
			this.MorbDevelopment_Meter.meterController.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x00069D4F File Offset: 0x00067F4F
	public void SetGermMeterProgress(float progress)
	{
		this.GermMeter.SetPositionPercent(progress);
	}

	// Token: 0x04000B6D RID: 2925
	public const byte MORB_PHASES_COUNT = 5;

	// Token: 0x04000B6E RID: 2926
	public const byte MORB_FIRST_PHASE_INDEX = 1;

	// Token: 0x04000B6F RID: 2927
	private const string GERM_METER_TARGET_NAME = "meter_germs_target";

	// Token: 0x04000B70 RID: 2928
	private const string GERM_METER_ANIMATION_NAME = "meter_germs";

	// Token: 0x04000B71 RID: 2929
	private const string MORB_METER_TARGET_NAME = "meter_morb_target";

	// Token: 0x04000B72 RID: 2930
	private const string MORB_METER_ANIMATION_NAME = "meter_morb";

	// Token: 0x04000B73 RID: 2931
	private const string MORB_CAPSULE_METER_TARGET_NAME = "meter_capsule_target";

	// Token: 0x04000B74 RID: 2932
	private const string MORB_CAPSULE_METER_ANIMATION_NAME = "meter_capsule";

	// Token: 0x04000B75 RID: 2933
	private static HashedString MORB_CAPSULE_METER_PUMP_ANIM_NAME = new HashedString("germ_pump");

	// Token: 0x04000B76 RID: 2934
	[MyCmpGet]
	private KBatchedAnimController buildingAnimCtr;

	// Token: 0x04000B77 RID: 2935
	private MeterController MorbDevelopment_Meter;

	// Token: 0x04000B78 RID: 2936
	private MeterController MorbDevelopment_Capsule_Meter;

	// Token: 0x04000B79 RID: 2937
	private MeterController GermMeter;
}
