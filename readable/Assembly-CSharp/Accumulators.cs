using System;
using System.Collections.Generic;

// Token: 0x020006C7 RID: 1735
public class Accumulators
{
	// Token: 0x06002A86 RID: 10886 RVA: 0x000F9341 File Offset: 0x000F7541
	public Accumulators()
	{
		this.elapsedTime = 0f;
		this.accumulated = new KCompactedVector<float>(0);
		this.average = new KCompactedVector<float>(0);
	}

	// Token: 0x06002A87 RID: 10887 RVA: 0x000F936C File Offset: 0x000F756C
	public HandleVector<int>.Handle Add(string name, KMonoBehaviour owner)
	{
		HandleVector<int>.Handle result = this.accumulated.Allocate(0f);
		this.average.Allocate(0f);
		return result;
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x000F938F File Offset: 0x000F758F
	public HandleVector<int>.Handle Remove(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return HandleVector<int>.InvalidHandle;
		}
		this.accumulated.Free(handle);
		this.average.Free(handle);
		return HandleVector<int>.InvalidHandle;
	}

	// Token: 0x06002A89 RID: 10889 RVA: 0x000F93C0 File Offset: 0x000F75C0
	public void Sim200ms(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime < 3f)
		{
			return;
		}
		this.elapsedTime -= 3f;
		List<float> dataList = this.accumulated.GetDataList();
		List<float> dataList2 = this.average.GetDataList();
		int count = dataList.Count;
		float num = 0.33333334f;
		for (int i = 0; i < count; i++)
		{
			dataList2[i] = dataList[i] * num;
			dataList[i] = 0f;
		}
	}

	// Token: 0x06002A8A RID: 10890 RVA: 0x000F944F File Offset: 0x000F764F
	public float GetAverageRate(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return 0f;
		}
		return this.average.GetData(handle);
	}

	// Token: 0x06002A8B RID: 10891 RVA: 0x000F946C File Offset: 0x000F766C
	public void Accumulate(HandleVector<int>.Handle handle, float amount)
	{
		float data = this.accumulated.GetData(handle);
		this.accumulated.SetData(handle, data + amount);
	}

	// Token: 0x0400194C RID: 6476
	private const float TIME_WINDOW = 3f;

	// Token: 0x0400194D RID: 6477
	private float elapsedTime;

	// Token: 0x0400194E RID: 6478
	private KCompactedVector<float> accumulated;

	// Token: 0x0400194F RID: 6479
	private KCompactedVector<float> average;
}
