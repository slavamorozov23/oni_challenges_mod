using System;
using STRINGS;

// Token: 0x02000745 RID: 1861
public class DevRadiationEmitter : KMonoBehaviour, ISingleSliderControl, ISliderControl
{
	// Token: 0x06002EE3 RID: 12003 RVA: 0x0010EC1D File Offset: 0x0010CE1D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.radiationEmitter != null)
		{
			this.radiationEmitter.SetEmitting(true);
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06002EE4 RID: 12004 RVA: 0x0010EC3F File Offset: 0x0010CE3F
	public string SliderTitleKey
	{
		get
		{
			return BUILDINGS.PREFABS.DEVRADIATIONGENERATOR.NAME;
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06002EE5 RID: 12005 RVA: 0x0010EC4B File Offset: 0x0010CE4B
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.RADIATION.RADS;
		}
	}

	// Token: 0x06002EE6 RID: 12006 RVA: 0x0010EC57 File Offset: 0x0010CE57
	public float GetSliderMax(int index)
	{
		return 5000f;
	}

	// Token: 0x06002EE7 RID: 12007 RVA: 0x0010EC5E File Offset: 0x0010CE5E
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06002EE8 RID: 12008 RVA: 0x0010EC65 File Offset: 0x0010CE65
	public string GetSliderTooltip(int index)
	{
		return "";
	}

	// Token: 0x06002EE9 RID: 12009 RVA: 0x0010EC6C File Offset: 0x0010CE6C
	public string GetSliderTooltipKey(int index)
	{
		return "";
	}

	// Token: 0x06002EEA RID: 12010 RVA: 0x0010EC73 File Offset: 0x0010CE73
	public float GetSliderValue(int index)
	{
		return this.radiationEmitter.emitRads;
	}

	// Token: 0x06002EEB RID: 12011 RVA: 0x0010EC80 File Offset: 0x0010CE80
	public void SetSliderValue(float value, int index)
	{
		this.radiationEmitter.emitRads = value;
		this.radiationEmitter.Refresh();
	}

	// Token: 0x06002EEC RID: 12012 RVA: 0x0010EC99 File Offset: 0x0010CE99
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x04001BC4 RID: 7108
	[MyCmpReq]
	private RadiationEmitter radiationEmitter;
}
