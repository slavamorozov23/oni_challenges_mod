using System;
using STRINGS;

// Token: 0x02000743 RID: 1859
public class DevLightGenerator : Light2D, IMultiSliderControl
{
	// Token: 0x06002EDB RID: 11995 RVA: 0x0010EA9F File Offset: 0x0010CC9F
	public DevLightGenerator()
	{
		this.sliderControls = new ISliderControl[]
		{
			new DevLightGenerator.LuxController(this),
			new DevLightGenerator.RangeController(this),
			new DevLightGenerator.FalloffController(this)
		};
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06002EDC RID: 11996 RVA: 0x0010EACE File Offset: 0x0010CCCE
	string IMultiSliderControl.SidescreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.NAME";
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06002EDD RID: 11997 RVA: 0x0010EAD5 File Offset: 0x0010CCD5
	ISliderControl[] IMultiSliderControl.sliderControls
	{
		get
		{
			return this.sliderControls;
		}
	}

	// Token: 0x06002EDE RID: 11998 RVA: 0x0010EADD File Offset: 0x0010CCDD
	bool IMultiSliderControl.SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x04001BC1 RID: 7105
	protected ISliderControl[] sliderControls;

	// Token: 0x02001620 RID: 5664
	protected class LuxController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x06009609 RID: 38409 RVA: 0x0037E4D0 File Offset: 0x0037C6D0
		public LuxController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x0600960A RID: 38410 RVA: 0x0037E4DF File Offset: 0x0037C6DF
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.BRIGHTNESS_LABEL";
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x0600960B RID: 38411 RVA: 0x0037E4E6 File Offset: 0x0037C6E6
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.LIGHT.LUX;
			}
		}

		// Token: 0x0600960C RID: 38412 RVA: 0x0037E4F2 File Offset: 0x0037C6F2
		public float GetSliderMax(int index)
		{
			return 100000f;
		}

		// Token: 0x0600960D RID: 38413 RVA: 0x0037E4F9 File Offset: 0x0037C6F9
		public float GetSliderMin(int index)
		{
			return 0f;
		}

		// Token: 0x0600960E RID: 38414 RVA: 0x0037E500 File Offset: 0x0037C700
		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, this.target.Lux);
		}

		// Token: 0x0600960F RID: 38415 RVA: 0x0037E521 File Offset: 0x0037C721
		public string GetSliderTooltipKey(int index)
		{
			return "<unused>";
		}

		// Token: 0x06009610 RID: 38416 RVA: 0x0037E528 File Offset: 0x0037C728
		public float GetSliderValue(int index)
		{
			return (float)this.target.Lux;
		}

		// Token: 0x06009611 RID: 38417 RVA: 0x0037E536 File Offset: 0x0037C736
		public void SetSliderValue(float value, int index)
		{
			this.target.Lux = (int)value;
			this.target.FullRefresh();
		}

		// Token: 0x06009612 RID: 38418 RVA: 0x0037E550 File Offset: 0x0037C750
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x040073D3 RID: 29651
		protected Light2D target;
	}

	// Token: 0x02001621 RID: 5665
	protected class RangeController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x06009613 RID: 38419 RVA: 0x0037E553 File Offset: 0x0037C753
		public RangeController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06009614 RID: 38420 RVA: 0x0037E562 File Offset: 0x0037C762
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.RANGE_LABEL";
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06009615 RID: 38421 RVA: 0x0037E569 File Offset: 0x0037C769
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.TILES;
			}
		}

		// Token: 0x06009616 RID: 38422 RVA: 0x0037E575 File Offset: 0x0037C775
		public float GetSliderMax(int index)
		{
			return 20f;
		}

		// Token: 0x06009617 RID: 38423 RVA: 0x0037E57C File Offset: 0x0037C77C
		public float GetSliderMin(int index)
		{
			return 1f;
		}

		// Token: 0x06009618 RID: 38424 RVA: 0x0037E583 File Offset: 0x0037C783
		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.target.Range);
		}

		// Token: 0x06009619 RID: 38425 RVA: 0x0037E5A4 File Offset: 0x0037C7A4
		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		// Token: 0x0600961A RID: 38426 RVA: 0x0037E5AB File Offset: 0x0037C7AB
		public float GetSliderValue(int index)
		{
			return this.target.Range;
		}

		// Token: 0x0600961B RID: 38427 RVA: 0x0037E5B9 File Offset: 0x0037C7B9
		public void SetSliderValue(float value, int index)
		{
			this.target.Range = (float)((int)value);
			this.target.FullRefresh();
		}

		// Token: 0x0600961C RID: 38428 RVA: 0x0037E5D4 File Offset: 0x0037C7D4
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x040073D4 RID: 29652
		protected Light2D target;
	}

	// Token: 0x02001622 RID: 5666
	protected class FalloffController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x0600961D RID: 38429 RVA: 0x0037E5D7 File Offset: 0x0037C7D7
		public FalloffController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x0600961E RID: 38430 RVA: 0x0037E5E6 File Offset: 0x0037C7E6
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.FALLOFF_LABEL";
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x0600961F RID: 38431 RVA: 0x0037E5ED File Offset: 0x0037C7ED
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.PERCENT;
			}
		}

		// Token: 0x06009620 RID: 38432 RVA: 0x0037E5F9 File Offset: 0x0037C7F9
		public float GetSliderMax(int index)
		{
			return 100f;
		}

		// Token: 0x06009621 RID: 38433 RVA: 0x0037E600 File Offset: 0x0037C800
		public float GetSliderMin(int index)
		{
			return 1f;
		}

		// Token: 0x06009622 RID: 38434 RVA: 0x0037E607 File Offset: 0x0037C807
		public string GetSliderTooltip(int index)
		{
			return string.Format("{0}", this.target.FalloffRate * 100f);
		}

		// Token: 0x06009623 RID: 38435 RVA: 0x0037E629 File Offset: 0x0037C829
		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		// Token: 0x06009624 RID: 38436 RVA: 0x0037E630 File Offset: 0x0037C830
		public float GetSliderValue(int index)
		{
			return this.target.FalloffRate * 100f;
		}

		// Token: 0x06009625 RID: 38437 RVA: 0x0037E644 File Offset: 0x0037C844
		public void SetSliderValue(float value, int index)
		{
			this.target.FalloffRate = value / 100f;
			this.target.FullRefresh();
		}

		// Token: 0x06009626 RID: 38438 RVA: 0x0037E663 File Offset: 0x0037C863
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x040073D5 RID: 29653
		protected Light2D target;
	}
}
