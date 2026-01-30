using System;
using System.Diagnostics;
using Klei.AI;
using UnityEngine;

namespace Klei
{
	// Token: 0x0200100F RID: 4111
	public static class SimUtil
	{
		// Token: 0x06007F8C RID: 32652 RVA: 0x00335399 File Offset: 0x00333599
		public static float CalculateEnergyFlow(float source_temp, float source_thermal_conductivity, float dest_temp, float dest_thermal_conductivity, float surface_area = 1f, float thickness = 1f)
		{
			return (source_temp - dest_temp) * Math.Min(source_thermal_conductivity, dest_thermal_conductivity) * (surface_area / thickness);
		}

		// Token: 0x06007F8D RID: 32653 RVA: 0x003353AC File Offset: 0x003335AC
		public static float CalculateEnergyFlow(int cell, float dest_temp, float dest_specific_heat_capacity, float dest_thermal_conductivity, float surface_area = 1f, float thickness = 1f)
		{
			if (Grid.Mass[cell] <= 0f)
			{
				return 0f;
			}
			Element element = Grid.Element[cell];
			if (element.IsVacuum)
			{
				return 0f;
			}
			float source_temp = Grid.Temperature[cell];
			float thermalConductivity = element.thermalConductivity;
			return SimUtil.CalculateEnergyFlow(source_temp, thermalConductivity, dest_temp, dest_thermal_conductivity, surface_area, thickness) * 0.001f;
		}

		// Token: 0x06007F8E RID: 32654 RVA: 0x0033540B File Offset: 0x0033360B
		public static float ClampEnergyTransfer(float dt, float source_temp, float source_mass, float source_specific_heat_capacity, float dest_temp, float dest_mass, float dest_specific_heat_capacity, float max_watts_transferred)
		{
			return SimUtil.ClampEnergyTransfer(dt, source_temp, source_mass * source_specific_heat_capacity, dest_temp, dest_mass * dest_specific_heat_capacity, max_watts_transferred);
		}

		// Token: 0x06007F8F RID: 32655 RVA: 0x00335420 File Offset: 0x00333620
		public static float ClampEnergyTransfer(float dt, float source_temp, float source_heat_capacity, float dest_temp, float dest_heat_capacity, float max_watts_transferred)
		{
			float num = max_watts_transferred * dt / 1000f;
			SimUtil.CheckValidValue(num);
			float min = Math.Min(source_temp, dest_temp);
			float max = Math.Max(source_temp, dest_temp);
			float num2 = source_temp - num / source_heat_capacity;
			float value = dest_temp + num / dest_heat_capacity;
			SimUtil.CheckValidValue(num2);
			SimUtil.CheckValidValue(value);
			num2 = Mathf.Clamp(num2, min, max);
			float num3 = Mathf.Clamp(value, min, max);
			float num4 = Math.Abs(num2 - source_temp);
			float num5 = Math.Abs(num3 - dest_temp);
			float val = num4 * source_heat_capacity;
			float val2 = num5 * dest_heat_capacity;
			float num6 = (max_watts_transferred < 0f) ? -1f : 1f;
			float num7 = Math.Min(val, val2) * num6;
			SimUtil.CheckValidValue(num7);
			return num7;
		}

		// Token: 0x06007F90 RID: 32656 RVA: 0x003354BB File Offset: 0x003336BB
		private static float GetMassAreaScale(Element element)
		{
			if (!element.IsGas)
			{
				return 0.01f;
			}
			return 10f;
		}

		// Token: 0x06007F91 RID: 32657 RVA: 0x003354D0 File Offset: 0x003336D0
		public static float CalculateEnergyFlowCreatures(int cell, float creature_temperature, float creature_shc, float creature_thermal_conductivity, float creature_surface_area = 1f, float creature_surface_thickness = 1f)
		{
			return SimUtil.CalculateEnergyFlow(cell, creature_temperature, creature_shc, creature_thermal_conductivity, creature_surface_area, creature_surface_thickness);
		}

		// Token: 0x06007F92 RID: 32658 RVA: 0x003354DF File Offset: 0x003336DF
		public static float EnergyFlowToTemperatureDelta(float kilojoules, float specific_heat_capacity, float mass)
		{
			if (kilojoules * specific_heat_capacity * mass == 0f)
			{
				return 0f;
			}
			return kilojoules / (specific_heat_capacity * mass);
		}

		// Token: 0x06007F93 RID: 32659 RVA: 0x003354F8 File Offset: 0x003336F8
		public static float CalculateFinalTemperature(float mass1, float temp1, float mass2, float temp2)
		{
			float num = mass1 + mass2;
			if (num == 0f)
			{
				return 0f;
			}
			float num2 = mass1 * temp1;
			float num3 = mass2 * temp2;
			float val = (num2 + num3) / num;
			float val2;
			float val3;
			if (temp1 > temp2)
			{
				val2 = temp2;
				val3 = temp1;
			}
			else
			{
				val2 = temp1;
				val3 = temp2;
			}
			return Math.Max(val2, Math.Min(val3, val));
		}

		// Token: 0x06007F94 RID: 32660 RVA: 0x00335543 File Offset: 0x00333743
		[Conditional("STRICT_CHECKING")]
		public static void CheckValidValue(float value)
		{
			if (!float.IsNaN(value))
			{
				float.IsInfinity(value);
			}
		}

		// Token: 0x06007F95 RID: 32661 RVA: 0x00335554 File Offset: 0x00333754
		public static SimUtil.DiseaseInfo CalculateFinalDiseaseInfo(SimUtil.DiseaseInfo a, SimUtil.DiseaseInfo b)
		{
			return SimUtil.CalculateFinalDiseaseInfo(a.idx, a.count, b.idx, b.count);
		}

		// Token: 0x06007F96 RID: 32662 RVA: 0x00335574 File Offset: 0x00333774
		public static SimUtil.DiseaseInfo CalculateFinalDiseaseInfo(byte src1_idx, int src1_count, byte src2_idx, int src2_count)
		{
			SimUtil.DiseaseInfo diseaseInfo = default(SimUtil.DiseaseInfo);
			if (src1_idx == src2_idx)
			{
				diseaseInfo.idx = src1_idx;
				diseaseInfo.count = src1_count + src2_count;
			}
			else if (src1_idx == 255)
			{
				diseaseInfo.idx = src2_idx;
				diseaseInfo.count = src2_count;
			}
			else if (src2_idx == 255)
			{
				diseaseInfo.idx = src1_idx;
				diseaseInfo.count = src1_count;
			}
			else
			{
				Disease disease = Db.Get().Diseases[(int)src1_idx];
				Disease disease2 = Db.Get().Diseases[(int)src2_idx];
				float num = disease.strength * (float)src1_count;
				float num2 = disease2.strength * (float)src2_count;
				if (num > num2)
				{
					int num3 = (int)((float)src2_count - num / num2 * (float)src1_count);
					if (num3 < 0)
					{
						diseaseInfo.idx = src1_idx;
						diseaseInfo.count = -num3;
					}
					else
					{
						diseaseInfo.idx = src2_idx;
						diseaseInfo.count = num3;
					}
				}
				else
				{
					int num4 = (int)((float)src1_count - num2 / num * (float)src2_count);
					if (num4 < 0)
					{
						diseaseInfo.idx = src2_idx;
						diseaseInfo.count = -num4;
					}
					else
					{
						diseaseInfo.idx = src1_idx;
						diseaseInfo.count = num4;
					}
				}
			}
			if (diseaseInfo.count <= 0)
			{
				diseaseInfo.count = 0;
				diseaseInfo.idx = byte.MaxValue;
			}
			return diseaseInfo;
		}

		// Token: 0x06007F97 RID: 32663 RVA: 0x003356A4 File Offset: 0x003338A4
		public static byte DiseaseCountToAlpha254(int count)
		{
			float num = Mathf.Log((float)count, 10f);
			num /= SimUtil.MAX_DISEASE_LOG_RANGE;
			num = Math.Max(0f, Math.Min(1f, num));
			num -= SimUtil.MIN_DISEASE_LOG_SUBTRACTION / SimUtil.MAX_DISEASE_LOG_RANGE;
			num = Math.Max(0f, num);
			num /= 1f - SimUtil.MIN_DISEASE_LOG_SUBTRACTION / SimUtil.MAX_DISEASE_LOG_RANGE;
			return (byte)(num * 254f);
		}

		// Token: 0x06007F98 RID: 32664 RVA: 0x00335712 File Offset: 0x00333912
		public static float DiseaseCountToAlpha(int count)
		{
			return (float)SimUtil.DiseaseCountToAlpha254(count) / 255f;
		}

		// Token: 0x06007F99 RID: 32665 RVA: 0x00335724 File Offset: 0x00333924
		public static SimUtil.DiseaseInfo GetPercentOfDisease(PrimaryElement pe, float percent)
		{
			return new SimUtil.DiseaseInfo
			{
				idx = pe.DiseaseIdx,
				count = (int)((float)pe.DiseaseCount * percent)
			};
		}

		// Token: 0x040060C3 RID: 24771
		private const int MAX_ALPHA_COUNT = 1000000;

		// Token: 0x040060C4 RID: 24772
		private static float MIN_DISEASE_LOG_SUBTRACTION = 2f;

		// Token: 0x040060C5 RID: 24773
		private static float MAX_DISEASE_LOG_RANGE = 6f;

		// Token: 0x0200271F RID: 10015
		public struct DiseaseInfo
		{
			// Token: 0x0400AE53 RID: 44627
			public byte idx;

			// Token: 0x0400AE54 RID: 44628
			public int count;

			// Token: 0x0400AE55 RID: 44629
			public static readonly SimUtil.DiseaseInfo Invalid = new SimUtil.DiseaseInfo
			{
				idx = byte.MaxValue,
				count = 0
			};
		}
	}
}
