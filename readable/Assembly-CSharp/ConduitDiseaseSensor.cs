using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000733 RID: 1843
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitDiseaseSensor : ConduitThresholdSensor, IThresholdSwitch
{
	// Token: 0x06002E4F RID: 11855 RVA: 0x0010C26C File Offset: 0x0010A46C
	protected override void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(ConduitSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				int num;
				int num2;
				bool flag;
				this.GetContentsDisease(out num, out num2, out flag);
				Color32 c = Color.white;
				if (num != 255)
				{
					Disease disease = Db.Get().Diseases[num];
					c = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
				}
				this.animController.SetSymbolTint(ConduitDiseaseSensor.TINT_SYMBOL, c);
				return;
			}
			this.animController.Play(ConduitSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x06002E50 RID: 11856 RVA: 0x0010C32C File Offset: 0x0010A52C
	private void GetContentsDisease(out int diseaseIdx, out int diseaseCount, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			diseaseIdx = (int)contents.diseaseIdx;
			diseaseCount = contents.diseaseCount;
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		if (pickupable != null && pickupable.PrimaryElement.Mass > 0f)
		{
			diseaseIdx = (int)pickupable.PrimaryElement.DiseaseIdx;
			diseaseCount = pickupable.PrimaryElement.DiseaseCount;
			hasMass = true;
			return;
		}
		diseaseIdx = 0;
		diseaseCount = 0;
		hasMass = false;
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06002E51 RID: 11857 RVA: 0x0010C3E0 File Offset: 0x0010A5E0
	public override float CurrentValue
	{
		get
		{
			int num;
			int num2;
			bool flag;
			this.GetContentsDisease(out num, out num2, out flag);
			if (flag)
			{
				this.lastValue = (float)num2;
			}
			return this.lastValue;
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06002E52 RID: 11858 RVA: 0x0010C40A File Offset: 0x0010A60A
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06002E53 RID: 11859 RVA: 0x0010C411 File Offset: 0x0010A611
	public float RangeMax
	{
		get
		{
			return 100000f;
		}
	}

	// Token: 0x06002E54 RID: 11860 RVA: 0x0010C418 File Offset: 0x0010A618
	public float GetRangeMinInputField()
	{
		return 0f;
	}

	// Token: 0x06002E55 RID: 11861 RVA: 0x0010C41F File Offset: 0x0010A61F
	public float GetRangeMaxInputField()
	{
		return 100000f;
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06002E56 RID: 11862 RVA: 0x0010C426 File Offset: 0x0010A626
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06002E57 RID: 11863 RVA: 0x0010C42D File Offset: 0x0010A62D
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_DISEASE;
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06002E58 RID: 11864 RVA: 0x0010C434 File Offset: 0x0010A634
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06002E59 RID: 11865 RVA: 0x0010C440 File Offset: 0x0010A640
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002E5A RID: 11866 RVA: 0x0010C44C File Offset: 0x0010A64C
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedInt((float)((int)value), GameUtil.TimeSlice.None);
	}

	// Token: 0x06002E5B RID: 11867 RVA: 0x0010C457 File Offset: 0x0010A657
	public float ProcessedSliderValue(float input)
	{
		return input;
	}

	// Token: 0x06002E5C RID: 11868 RVA: 0x0010C45A File Offset: 0x0010A65A
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06002E5D RID: 11869 RVA: 0x0010C45D File Offset: 0x0010A65D
	public LocString ThresholdValueUnits()
	{
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_UNITS;
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06002E5E RID: 11870 RVA: 0x0010C464 File Offset: 0x0010A664
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06002E5F RID: 11871 RVA: 0x0010C467 File Offset: 0x0010A667
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06002E60 RID: 11872 RVA: 0x0010C46A File Offset: 0x0010A66A
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04001B81 RID: 7041
	private const float rangeMin = 0f;

	// Token: 0x04001B82 RID: 7042
	private const float rangeMax = 100000f;

	// Token: 0x04001B83 RID: 7043
	[Serialize]
	private float lastValue;

	// Token: 0x04001B84 RID: 7044
	private static readonly HashedString TINT_SYMBOL = "germs";
}
