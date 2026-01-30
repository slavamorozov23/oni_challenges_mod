using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000793 RID: 1939
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicDiseaseSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x060031D1 RID: 12753 RVA: 0x0011F0C9 File Offset: 0x0011D2C9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicDiseaseSensor>(-905833192, LogicDiseaseSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060031D2 RID: 12754 RVA: 0x0011F0E4 File Offset: 0x0011D2E4
	private void OnCopySettings(object data)
	{
		LogicDiseaseSensor component = ((GameObject)data).GetComponent<LogicDiseaseSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060031D3 RID: 12755 RVA: 0x0011F11E File Offset: 0x0011D31E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060031D4 RID: 12756 RVA: 0x0011F160 File Offset: 0x0011D360
	public void Sim200ms(float dt)
	{
		if (this.sampleIdx < 8)
		{
			int i = Grid.PosToCell(this);
			if (Grid.Mass[i] > 0f)
			{
				this.samples[this.sampleIdx] = Grid.DiseaseCount[i];
				this.sampleIdx++;
			}
			return;
		}
		this.sampleIdx = 0;
		float currentValue = this.CurrentValue;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
		this.animController.SetSymbolVisiblity(LogicDiseaseSensor.TINT_SYMBOL, currentValue > 0f);
	}

	// Token: 0x060031D5 RID: 12757 RVA: 0x0011F23B File Offset: 0x0011D43B
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x060031D6 RID: 12758 RVA: 0x0011F24A File Offset: 0x0011D44A
	// (set) Token: 0x060031D7 RID: 12759 RVA: 0x0011F252 File Offset: 0x0011D452
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
		}
	}

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x060031D8 RID: 12760 RVA: 0x0011F25B File Offset: 0x0011D45B
	// (set) Token: 0x060031D9 RID: 12761 RVA: 0x0011F263 File Offset: 0x0011D463
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
		}
	}

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x060031DA RID: 12762 RVA: 0x0011F26C File Offset: 0x0011D46C
	public float CurrentValue
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += (float)this.samples[i];
			}
			return num / 8f;
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x060031DB RID: 12763 RVA: 0x0011F29E File Offset: 0x0011D49E
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x060031DC RID: 12764 RVA: 0x0011F2A5 File Offset: 0x0011D4A5
	public float RangeMax
	{
		get
		{
			return 100000f;
		}
	}

	// Token: 0x060031DD RID: 12765 RVA: 0x0011F2AC File Offset: 0x0011D4AC
	public float GetRangeMinInputField()
	{
		return 0f;
	}

	// Token: 0x060031DE RID: 12766 RVA: 0x0011F2B3 File Offset: 0x0011D4B3
	public float GetRangeMaxInputField()
	{
		return 100000f;
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x060031DF RID: 12767 RVA: 0x0011F2BA File Offset: 0x0011D4BA
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE;
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x060031E0 RID: 12768 RVA: 0x0011F2C1 File Offset: 0x0011D4C1
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x060031E1 RID: 12769 RVA: 0x0011F2CD File Offset: 0x0011D4CD
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060031E2 RID: 12770 RVA: 0x0011F2D9 File Offset: 0x0011D4D9
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedInt((float)((int)value), GameUtil.TimeSlice.None);
	}

	// Token: 0x060031E3 RID: 12771 RVA: 0x0011F2E4 File Offset: 0x0011D4E4
	public float ProcessedSliderValue(float input)
	{
		return input;
	}

	// Token: 0x060031E4 RID: 12772 RVA: 0x0011F2E7 File Offset: 0x0011D4E7
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x060031E5 RID: 12773 RVA: 0x0011F2EA File Offset: 0x0011D4EA
	public LocString ThresholdValueUnits()
	{
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_UNITS;
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x060031E6 RID: 12774 RVA: 0x0011F2F1 File Offset: 0x0011D4F1
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x060031E7 RID: 12775 RVA: 0x0011F2F4 File Offset: 0x0011D4F4
	public int IncrementScale
	{
		get
		{
			return 100;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x060031E8 RID: 12776 RVA: 0x0011F2F8 File Offset: 0x0011D4F8
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x060031E9 RID: 12777 RVA: 0x0011F305 File Offset: 0x0011D505
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060031EA RID: 12778 RVA: 0x0011F324 File Offset: 0x0011D524
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(LogicDiseaseSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				int i = Grid.PosToCell(this);
				byte b = Grid.DiseaseIdx[i];
				Color32 c = Color.white;
				if (b != 255)
				{
					Disease disease = Db.Get().Diseases[(int)b];
					c = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
				}
				this.animController.SetSymbolTint(LogicDiseaseSensor.TINT_SYMBOL, c);
				return;
			}
			this.animController.Play(LogicDiseaseSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x060031EB RID: 12779 RVA: 0x0011F3E8 File Offset: 0x0011D5E8
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x060031EC RID: 12780 RVA: 0x0011F43B File Offset: 0x0011D63B
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
		}
	}

	// Token: 0x04001DFB RID: 7675
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04001DFC RID: 7676
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04001DFD RID: 7677
	private KBatchedAnimController animController;

	// Token: 0x04001DFE RID: 7678
	private bool wasOn;

	// Token: 0x04001DFF RID: 7679
	private const float rangeMin = 0f;

	// Token: 0x04001E00 RID: 7680
	private const float rangeMax = 100000f;

	// Token: 0x04001E01 RID: 7681
	private const int WINDOW_SIZE = 8;

	// Token: 0x04001E02 RID: 7682
	private int[] samples = new int[8];

	// Token: 0x04001E03 RID: 7683
	private int sampleIdx;

	// Token: 0x04001E04 RID: 7684
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001E05 RID: 7685
	private static readonly EventSystem.IntraObjectHandler<LogicDiseaseSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicDiseaseSensor>(delegate(LogicDiseaseSensor component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001E06 RID: 7686
	private static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on_loop"
	};

	// Token: 0x04001E07 RID: 7687
	private static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};

	// Token: 0x04001E08 RID: 7688
	private static readonly HashedString TINT_SYMBOL = "germs";
}
