using System;
using Klei;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;

// Token: 0x02000870 RID: 2160
public class ConduitDiseaseManager : KCompactedVector<ConduitDiseaseManager.Data>
{
	// Token: 0x06003B53 RID: 15187 RVA: 0x0014BAB4 File Offset: 0x00149CB4
	private static ElemGrowthInfo GetGrowthInfo(byte disease_idx, ushort elem_idx)
	{
		ElemGrowthInfo result;
		if (disease_idx != 255)
		{
			result = Db.Get().Diseases[(int)disease_idx].elemGrowthInfo[(int)elem_idx];
		}
		else
		{
			result = Disease.DEFAULT_GROWTH_INFO;
		}
		return result;
	}

	// Token: 0x06003B54 RID: 15188 RVA: 0x0014BAEE File Offset: 0x00149CEE
	public ConduitDiseaseManager(ConduitTemperatureManager temperature_manager) : base(0)
	{
		this.temperatureManager = temperature_manager;
	}

	// Token: 0x06003B55 RID: 15189 RVA: 0x0014BB00 File Offset: 0x00149D00
	public HandleVector<int>.Handle Allocate(HandleVector<int>.Handle temperature_handle, ref ConduitFlow.ConduitContents contents)
	{
		ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
		ConduitDiseaseManager.Data initial_data = new ConduitDiseaseManager.Data(temperature_handle, elementIndex, contents.mass, contents.diseaseIdx, contents.diseaseCount);
		return base.Allocate(initial_data);
	}

	// Token: 0x06003B56 RID: 15190 RVA: 0x0014BB3C File Offset: 0x00149D3C
	public void SetData(HandleVector<int>.Handle handle, ref ConduitFlow.ConduitContents contents)
	{
		ConduitDiseaseManager.Data data = base.GetData(handle);
		data.diseaseCount = contents.diseaseCount;
		if (contents.diseaseIdx != data.diseaseIdx)
		{
			data.diseaseIdx = contents.diseaseIdx;
			ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
			data.growthInfo = ConduitDiseaseManager.GetGrowthInfo(contents.diseaseIdx, elementIndex);
		}
		base.SetData(handle, data);
	}

	// Token: 0x06003B57 RID: 15191 RVA: 0x0014BBA0 File Offset: 0x00149DA0
	public void Sim200ms(float dt)
	{
		for (int i = 0; i < this.data.Count; i++)
		{
			ConduitDiseaseManager.Data data = this.data[i];
			if (data.diseaseIdx != 255)
			{
				float num = data.accumulatedError;
				num += data.growthInfo.CalculateDiseaseCountDelta(data.diseaseCount, data.mass, dt);
				Disease disease = Db.Get().Diseases[(int)data.diseaseIdx];
				float num2 = Disease.HalfLifeToGrowthRate(Disease.CalculateRangeHalfLife(this.temperatureManager.GetTemperature(data.temperatureHandle), ref disease.temperatureRange, ref disease.temperatureHalfLives), dt);
				num += (float)data.diseaseCount * num2 - (float)data.diseaseCount;
				int num3 = (int)num;
				data.accumulatedError = num - (float)num3;
				data.diseaseCount += num3;
				if (data.diseaseCount <= 0)
				{
					data.diseaseCount = 0;
					data.diseaseIdx = byte.MaxValue;
					data.accumulatedError = 0f;
				}
				this.data[i] = data;
			}
		}
	}

	// Token: 0x06003B58 RID: 15192 RVA: 0x0014BCB4 File Offset: 0x00149EB4
	public void ModifyDiseaseCount(HandleVector<int>.Handle h, int disease_count_delta)
	{
		ConduitDiseaseManager.Data data = base.GetData(h);
		data.diseaseCount = Math.Max(0, data.diseaseCount + disease_count_delta);
		if (data.diseaseCount == 0)
		{
			data.diseaseIdx = byte.MaxValue;
		}
		base.SetData(h, data);
	}

	// Token: 0x06003B59 RID: 15193 RVA: 0x0014BCFC File Offset: 0x00149EFC
	public void AddDisease(HandleVector<int>.Handle h, byte disease_idx, int disease_count)
	{
		ConduitDiseaseManager.Data data = base.GetData(h);
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, disease_count, data.diseaseIdx, data.diseaseCount);
		data.diseaseIdx = diseaseInfo.idx;
		data.diseaseCount = diseaseInfo.count;
		base.SetData(h, data);
	}

	// Token: 0x040024B5 RID: 9397
	private ConduitTemperatureManager temperatureManager;

	// Token: 0x0200182A RID: 6186
	public struct Data
	{
		// Token: 0x06009E0D RID: 40461 RVA: 0x003A190D File Offset: 0x0039FB0D
		public Data(HandleVector<int>.Handle temperature_handle, ushort elem_idx, float mass, byte disease_idx, int disease_count)
		{
			this.diseaseIdx = disease_idx;
			this.elemIdx = elem_idx;
			this.mass = mass;
			this.diseaseCount = disease_count;
			this.accumulatedError = 0f;
			this.temperatureHandle = temperature_handle;
			this.growthInfo = ConduitDiseaseManager.GetGrowthInfo(disease_idx, elem_idx);
		}

		// Token: 0x040079FA RID: 31226
		public byte diseaseIdx;

		// Token: 0x040079FB RID: 31227
		public ushort elemIdx;

		// Token: 0x040079FC RID: 31228
		public int diseaseCount;

		// Token: 0x040079FD RID: 31229
		public float accumulatedError;

		// Token: 0x040079FE RID: 31230
		public float mass;

		// Token: 0x040079FF RID: 31231
		public HandleVector<int>.Handle temperatureHandle;

		// Token: 0x04007A00 RID: 31232
		public ElemGrowthInfo growthInfo;
	}
}
