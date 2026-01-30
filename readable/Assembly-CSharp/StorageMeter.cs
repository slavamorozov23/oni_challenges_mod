using System;

// Token: 0x02000808 RID: 2056
public class StorageMeter : KMonoBehaviour
{
	// Token: 0x06003762 RID: 14178 RVA: 0x00137696 File Offset: 0x00135896
	public void SetInterpolateFunction(Func<float, int, float> func)
	{
		this.interpolateFunction = func;
		if (this.meter != null)
		{
			this.meter.interpolateFunction = this.interpolateFunction;
		}
	}

	// Token: 0x06003763 RID: 14179 RVA: 0x001376B8 File Offset: 0x001358B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003764 RID: 14180 RVA: 0x001376C0 File Offset: 0x001358C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_frame",
			"meter_level"
		});
		this.meter.interpolateFunction = this.interpolateFunction;
		this.UpdateMeter(null);
		base.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
	}

	// Token: 0x06003765 RID: 14181 RVA: 0x0013773F File Offset: 0x0013593F
	private void UpdateMeter(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
	}

	// Token: 0x040021B9 RID: 8633
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040021BA RID: 8634
	private MeterController meter;

	// Token: 0x040021BB RID: 8635
	private Func<float, int, float> interpolateFunction = new Func<float, int, float>(MeterController.MinMaxStepLerp);
}
