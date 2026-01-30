using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Database;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Pool;

// Token: 0x02000967 RID: 2407
public static class GameUtil
{
	// Token: 0x06004387 RID: 17287 RVA: 0x00184E4C File Offset: 0x0018304C
	public static CellOffset[] Expand(this CellOffset[] original)
	{
		List<CellOffset> list = new List<CellOffset>(original);
		Vector4 vector = new Vector2(float.MaxValue, float.MinValue);
		Vector4 vector2 = new Vector2(float.MaxValue, float.MinValue);
		foreach (CellOffset cellOffset in original)
		{
			if ((float)cellOffset.x < vector.x)
			{
				vector.x = (float)cellOffset.x;
			}
			if ((float)cellOffset.x > vector.y)
			{
				vector.y = (float)cellOffset.x;
			}
			if ((float)cellOffset.y < vector2.x)
			{
				vector2.x = (float)cellOffset.y;
			}
			if ((float)cellOffset.y > vector2.y)
			{
				vector2.y = (float)cellOffset.y;
			}
		}
		foreach (CellOffset cellOffset2 in original)
		{
			Vector2Int zero = Vector2Int.zero;
			if ((float)cellOffset2.x == vector.x)
			{
				list.Add(new CellOffset(cellOffset2.x - 1, cellOffset2.y));
				zero.x = -1;
			}
			if ((float)cellOffset2.x == vector.y)
			{
				list.Add(new CellOffset(cellOffset2.x + 1, cellOffset2.y));
				zero.x = 1;
			}
			if ((float)cellOffset2.y == vector2.x)
			{
				list.Add(new CellOffset(cellOffset2.x, cellOffset2.y - 1));
				zero.y = -1;
			}
			if ((float)cellOffset2.y == vector2.y)
			{
				list.Add(new CellOffset(cellOffset2.x, cellOffset2.y + 1));
				zero.y = 1;
			}
			if (zero.x != 0 && zero.y != 0)
			{
				list.Add(new CellOffset((int)((zero.x < 0) ? vector.x : vector.y) + zero.x, (int)((zero.y < 0) ? vector2.x : vector2.y) + zero.y));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06004388 RID: 17288 RVA: 0x00185080 File Offset: 0x00183280
	public static string GetTemperatureUnitSuffix()
	{
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		string result;
		if (temperatureUnit != GameUtil.TemperatureUnit.Celsius)
		{
			if (temperatureUnit != GameUtil.TemperatureUnit.Fahrenheit)
			{
				result = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
			}
			else
			{
				result = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
			}
		}
		else
		{
			result = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
		}
		return result;
	}

	// Token: 0x06004389 RID: 17289 RVA: 0x001850C2 File Offset: 0x001832C2
	private static string AddTemperatureUnitSuffix(string text)
	{
		return text + GameUtil.GetTemperatureUnitSuffix();
	}

	// Token: 0x0600438A RID: 17290 RVA: 0x001850CF File Offset: 0x001832CF
	public static float GetTemperatureConvertedFromKelvin(float temperature, GameUtil.TemperatureUnit targetUnit)
	{
		if (targetUnit == GameUtil.TemperatureUnit.Celsius)
		{
			return temperature - 273.15f;
		}
		if (targetUnit != GameUtil.TemperatureUnit.Fahrenheit)
		{
			return temperature;
		}
		return temperature * 1.8f - 459.67f;
	}

	// Token: 0x0600438B RID: 17291 RVA: 0x001850F4 File Offset: 0x001832F4
	public static float GetConvertedTemperature(float temperature, bool roundOutput = false)
	{
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		if (temperatureUnit != GameUtil.TemperatureUnit.Celsius)
		{
			if (temperatureUnit != GameUtil.TemperatureUnit.Fahrenheit)
			{
				if (!roundOutput)
				{
					return temperature;
				}
				return Mathf.Round(temperature);
			}
			else
			{
				float num = temperature * 1.8f - 459.67f;
				if (!roundOutput)
				{
					return num;
				}
				return Mathf.Round(num);
			}
		}
		else
		{
			float num = temperature - 273.15f;
			if (!roundOutput)
			{
				return num;
			}
			return Mathf.Round(num);
		}
	}

	// Token: 0x0600438C RID: 17292 RVA: 0x0018514F File Offset: 0x0018334F
	public static float GetTemperatureConvertedToKelvin(float temperature, GameUtil.TemperatureUnit fromUnit)
	{
		if (fromUnit == GameUtil.TemperatureUnit.Celsius)
		{
			return temperature + 273.15f;
		}
		if (fromUnit != GameUtil.TemperatureUnit.Fahrenheit)
		{
			return temperature;
		}
		return (temperature + 459.67f) * 5f / 9f;
	}

	// Token: 0x0600438D RID: 17293 RVA: 0x00185178 File Offset: 0x00183378
	public static float GetTemperatureConvertedToKelvin(float temperature)
	{
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		if (temperatureUnit == GameUtil.TemperatureUnit.Celsius)
		{
			return temperature + 273.15f;
		}
		if (temperatureUnit != GameUtil.TemperatureUnit.Fahrenheit)
		{
			return temperature;
		}
		return (temperature + 459.67f) * 5f / 9f;
	}

	// Token: 0x0600438E RID: 17294 RVA: 0x001851B4 File Offset: 0x001833B4
	private static float GetConvertedTemperatureDelta(float kelvin_delta)
	{
		switch (GameUtil.temperatureUnit)
		{
		case GameUtil.TemperatureUnit.Celsius:
			return kelvin_delta;
		case GameUtil.TemperatureUnit.Fahrenheit:
			return kelvin_delta * 1.8f;
		case GameUtil.TemperatureUnit.Kelvin:
			return kelvin_delta;
		default:
			return kelvin_delta;
		}
	}

	// Token: 0x0600438F RID: 17295 RVA: 0x001851E8 File Offset: 0x001833E8
	public static float ApplyTimeSlice(float val, GameUtil.TimeSlice timeSlice)
	{
		if (timeSlice == GameUtil.TimeSlice.PerCycle)
		{
			return val * 600f;
		}
		return val;
	}

	// Token: 0x06004390 RID: 17296 RVA: 0x001851F7 File Offset: 0x001833F7
	public static float ApplyTimeSlice(int val, GameUtil.TimeSlice timeSlice)
	{
		if (timeSlice == GameUtil.TimeSlice.PerCycle)
		{
			return (float)val * 600f;
		}
		return (float)val;
	}

	// Token: 0x06004391 RID: 17297 RVA: 0x00185208 File Offset: 0x00183408
	public static string AddTimeSliceText(string text, GameUtil.TimeSlice timeSlice)
	{
		switch (timeSlice)
		{
		case GameUtil.TimeSlice.PerSecond:
			return text + UI.UNITSUFFIXES.PERSECOND;
		case GameUtil.TimeSlice.PerCycle:
			return text + UI.UNITSUFFIXES.PERCYCLE;
		}
		return text;
	}

	// Token: 0x06004392 RID: 17298 RVA: 0x00185245 File Offset: 0x00183445
	public static void AddTimeSliceText(StringBuilder builder, GameUtil.TimeSlice timeSlice)
	{
		switch (timeSlice)
		{
		case GameUtil.TimeSlice.None:
		case GameUtil.TimeSlice.ModifyOnly:
			break;
		case GameUtil.TimeSlice.PerSecond:
			builder.Append(UI.UNITSUFFIXES.PERSECOND);
			return;
		case GameUtil.TimeSlice.PerCycle:
			builder.Append(UI.UNITSUFFIXES.PERCYCLE);
			break;
		default:
			return;
		}
	}

	// Token: 0x06004393 RID: 17299 RVA: 0x00185281 File Offset: 0x00183481
	public static string AddPositiveSign(string text, bool positive)
	{
		if (positive)
		{
			return string.Format(UI.POSITIVE_FORMAT, text);
		}
		return text;
	}

	// Token: 0x06004394 RID: 17300 RVA: 0x00185298 File Offset: 0x00183498
	public static float AttributeSkillToAlpha(AttributeInstance attributeInstance)
	{
		return Mathf.Min(attributeInstance.GetTotalValue() / 10f, 1f);
	}

	// Token: 0x06004395 RID: 17301 RVA: 0x001852B0 File Offset: 0x001834B0
	public static float AttributeSkillToAlpha(float attributeSkill)
	{
		return Mathf.Min(attributeSkill / 10f, 1f);
	}

	// Token: 0x06004396 RID: 17302 RVA: 0x001852C3 File Offset: 0x001834C3
	public static float AptitudeToAlpha(float aptitude)
	{
		return Mathf.Min(aptitude / 10f, 1f);
	}

	// Token: 0x06004397 RID: 17303 RVA: 0x001852D6 File Offset: 0x001834D6
	public static float GetThermalEnergy(PrimaryElement pe)
	{
		return pe.Temperature * pe.Mass * pe.Element.specificHeatCapacity;
	}

	// Token: 0x06004398 RID: 17304 RVA: 0x001852F1 File Offset: 0x001834F1
	public static float CalculateTemperatureChange(float shc, float mass, float kilowatts)
	{
		return kilowatts / (shc * mass);
	}

	// Token: 0x06004399 RID: 17305 RVA: 0x001852F8 File Offset: 0x001834F8
	public static void DeltaThermalEnergy(PrimaryElement pe, float kilowatts, float targetTemperature)
	{
		float num = GameUtil.CalculateTemperatureChange(pe.Element.specificHeatCapacity, pe.Mass, kilowatts);
		float num2 = pe.Temperature + num;
		if (targetTemperature > pe.Temperature)
		{
			num2 = Mathf.Clamp(num2, pe.Temperature, targetTemperature);
		}
		else
		{
			num2 = Mathf.Clamp(num2, targetTemperature, pe.Temperature);
		}
		pe.Temperature = num2;
	}

	// Token: 0x0600439A RID: 17306 RVA: 0x00185354 File Offset: 0x00183554
	public static BindingEntry ActionToBinding(global::Action action)
	{
		foreach (BindingEntry bindingEntry in GameInputMapping.KeyBindings)
		{
			if (bindingEntry.mAction == action)
			{
				return bindingEntry;
			}
		}
		throw new ArgumentException(action.ToString() + " is not bound in GameInputBindings");
	}

	// Token: 0x0600439B RID: 17307 RVA: 0x001853A4 File Offset: 0x001835A4
	public static string GetIdentityDescriptor(GameObject go, GameUtil.IdentityDescriptorTense tense = GameUtil.IdentityDescriptorTense.Normal)
	{
		if (go.GetComponent<MinionIdentity>())
		{
			switch (tense)
			{
			case GameUtil.IdentityDescriptorTense.Normal:
				return DUPLICANTS.STATS.SUBJECTS.DUPLICANT;
			case GameUtil.IdentityDescriptorTense.Possessive:
				return DUPLICANTS.STATS.SUBJECTS.DUPLICANT_POSSESSIVE;
			case GameUtil.IdentityDescriptorTense.Plural:
				return DUPLICANTS.STATS.SUBJECTS.DUPLICANT_PLURAL;
			}
		}
		else if (go.GetComponent<CreatureBrain>())
		{
			switch (tense)
			{
			case GameUtil.IdentityDescriptorTense.Normal:
				return DUPLICANTS.STATS.SUBJECTS.CREATURE;
			case GameUtil.IdentityDescriptorTense.Possessive:
				return DUPLICANTS.STATS.SUBJECTS.CREATURE_POSSESSIVE;
			case GameUtil.IdentityDescriptorTense.Plural:
				return DUPLICANTS.STATS.SUBJECTS.CREATURE_PLURAL;
			}
		}
		else
		{
			switch (tense)
			{
			case GameUtil.IdentityDescriptorTense.Normal:
				return DUPLICANTS.STATS.SUBJECTS.PLANT;
			case GameUtil.IdentityDescriptorTense.Possessive:
				return DUPLICANTS.STATS.SUBJECTS.PLANT_POSESSIVE;
			case GameUtil.IdentityDescriptorTense.Plural:
				return DUPLICANTS.STATS.SUBJECTS.PLANT_PLURAL;
			}
		}
		return "";
	}

	// Token: 0x0600439C RID: 17308 RVA: 0x00185472 File Offset: 0x00183672
	public static float GetEnergyInPrimaryElement(PrimaryElement element)
	{
		return 0.001f * (element.Temperature * (element.Mass * 1000f * element.Element.specificHeatCapacity));
	}

	// Token: 0x0600439D RID: 17309 RVA: 0x0018549C File Offset: 0x0018369C
	public static float EnergyToTemperatureDelta(float kilojoules, PrimaryElement element)
	{
		global::Debug.Assert(element.Mass > 0f);
		float num = Mathf.Max(GameUtil.GetEnergyInPrimaryElement(element) - kilojoules, 1f);
		float temperature = element.Temperature;
		return num / (0.001f * (element.Mass * (element.Element.specificHeatCapacity * 1000f))) - temperature;
	}

	// Token: 0x0600439E RID: 17310 RVA: 0x001854F5 File Offset: 0x001836F5
	public static float CalculateEnergyDeltaForElement(PrimaryElement element, float startTemp, float endTemp)
	{
		return GameUtil.CalculateEnergyDeltaForElementChange(element.Mass, element.Element.specificHeatCapacity, startTemp, endTemp);
	}

	// Token: 0x0600439F RID: 17311 RVA: 0x0018550F File Offset: 0x0018370F
	public static float CalculateEnergyDeltaForElementChange(float mass, float shc, float startTemp, float endTemp)
	{
		return (endTemp - startTemp) * mass * shc;
	}

	// Token: 0x060043A0 RID: 17312 RVA: 0x00185518 File Offset: 0x00183718
	public static float GetFinalTemperature(float t1, float m1, float t2, float m2)
	{
		float num = m1 + m2;
		float num2 = (t1 * m1 + t2 * m2) / num;
		float num3 = Mathf.Min(t1, t2);
		float num4 = Mathf.Max(t1, t2);
		num2 = Mathf.Clamp(num2, num3, num4);
		if (float.IsNaN(num2) || float.IsInfinity(num2))
		{
			global::Debug.LogError(string.Format("Calculated an invalid temperature: t1={0}, m1={1}, t2={2}, m2={3}, min_temp={4}, max_temp={5}", new object[]
			{
				t1,
				m1,
				t2,
				m2,
				num3,
				num4
			}));
		}
		return num2;
	}

	// Token: 0x060043A1 RID: 17313 RVA: 0x001855A8 File Offset: 0x001837A8
	public static void ForceConduction(PrimaryElement a, PrimaryElement b, float dt)
	{
		float num = a.Temperature * a.Element.specificHeatCapacity * a.Mass;
		float num2 = b.Temperature * b.Element.specificHeatCapacity * b.Mass;
		float num3 = Math.Min(a.Element.thermalConductivity, b.Element.thermalConductivity);
		float num4 = Math.Min(a.Mass, b.Mass);
		float num5 = (b.Temperature - a.Temperature) * (num3 * num4) * dt;
		float num6 = (num + num2) / (a.Element.specificHeatCapacity * a.Mass + b.Element.specificHeatCapacity * b.Mass);
		float val = Math.Abs((num6 - a.Temperature) * a.Element.specificHeatCapacity * a.Mass);
		float val2 = Math.Abs((num6 - b.Temperature) * b.Element.specificHeatCapacity * b.Mass);
		float num7 = Math.Min(val, val2);
		num5 = Math.Min(num5, num7);
		num5 = Math.Max(num5, -num7);
		a.Temperature = (num + num5) / a.Element.specificHeatCapacity / a.Mass;
		b.Temperature = (num2 - num5) / b.Element.specificHeatCapacity / b.Mass;
	}

	// Token: 0x060043A2 RID: 17314 RVA: 0x001856F4 File Offset: 0x001838F4
	public static string FloatToString(float f, string format = null)
	{
		if (float.IsPositiveInfinity(f))
		{
			return UI.POS_INFINITY;
		}
		if (float.IsNegativeInfinity(f))
		{
			return UI.NEG_INFINITY;
		}
		return f.ToString(format);
	}

	// Token: 0x060043A3 RID: 17315 RVA: 0x00185724 File Offset: 0x00183924
	public unsafe static void AppendFloatToString(StringBuilder builder, float f, string format = null)
	{
		if (float.IsPositiveInfinity(f))
		{
			builder.Append(UI.POS_INFINITY);
			return;
		}
		if (float.IsNegativeInfinity(f))
		{
			builder.Append(UI.NEG_INFINITY);
			return;
		}
		if (format != null)
		{
			Span<char> destination = new Span<char>(stackalloc byte[(UIntPtr)128], 64);
			int length;
			f.TryFormat(destination, out length, format, null);
			builder.Append(destination.Slice(0, length));
			return;
		}
		builder.Append(f);
	}

	// Token: 0x060043A4 RID: 17316 RVA: 0x001857A8 File Offset: 0x001839A8
	public static string GetFloatWithDecimalPoint(float f)
	{
		string format;
		if (f == 0f)
		{
			format = "0";
		}
		else if (Mathf.Abs(f) < 1f)
		{
			format = "#,##0.#";
		}
		else
		{
			format = "#,###.#";
		}
		return GameUtil.FloatToString(f, format);
	}

	// Token: 0x060043A5 RID: 17317 RVA: 0x001857F0 File Offset: 0x001839F0
	public static void AppendFloatWithDecimalPoint(StringBuilder builder, float f)
	{
		if (f == 0f)
		{
			builder.AppendFormat("{0:0}", f);
			return;
		}
		if (Mathf.Abs(f) < 1f)
		{
			builder.AppendFormat("{0:#,##0.#}", f);
			return;
		}
		builder.AppendFormat("{0:#,###.#}", f);
	}

	// Token: 0x060043A6 RID: 17318 RVA: 0x0018584C File Offset: 0x00183A4C
	public static string GetStandardFloat(float f)
	{
		string format;
		if (f == 0f)
		{
			format = "0";
		}
		else if (Mathf.Abs(f) < 1f)
		{
			format = "#,##0.#";
		}
		else if (Mathf.Abs(f) < 10f)
		{
			format = "#,###.#";
		}
		else
		{
			format = "#,###";
		}
		return GameUtil.FloatToString(f, format);
	}

	// Token: 0x060043A7 RID: 17319 RVA: 0x001858A8 File Offset: 0x00183AA8
	public static void AppendStandardFloat(StringBuilder builder, float f)
	{
		if (float.IsPositiveInfinity(f))
		{
			builder.Append(UI.POS_INFINITY);
			return;
		}
		if (float.IsNegativeInfinity(f))
		{
			builder.Append(UI.NEG_INFINITY);
			return;
		}
		if (f == 0f)
		{
			builder.AppendFormat("{0:0}", f);
			return;
		}
		if (Math.Abs(f) < 1f)
		{
			builder.AppendFormat("{0:#,##0.##}", f);
			return;
		}
		if (Math.Abs(f) < 10f)
		{
			builder.AppendFormat("{0:#,##0.##}", f);
			return;
		}
		builder.AppendFormat("{0:#,###}", f);
	}

	// Token: 0x060043A8 RID: 17320 RVA: 0x00185958 File Offset: 0x00183B58
	public static string GetStandardPercentageFloat(float f, bool allowHundredths = false)
	{
		string format;
		if (Mathf.Abs(f) == 0f)
		{
			format = "0";
		}
		else if (Mathf.Abs(f) < 0.1f && allowHundredths)
		{
			format = "##0.##";
		}
		else if (Mathf.Abs(f) < 1f)
		{
			format = "##0.#";
		}
		else
		{
			format = "##0";
		}
		return GameUtil.FloatToString(f, format);
	}

	// Token: 0x060043A9 RID: 17321 RVA: 0x001859BC File Offset: 0x00183BBC
	public static void AppendStandardPercentageFloat(StringBuilder builder, float f, bool allowHundredths = false)
	{
		if (Mathf.Abs(f) == 0f)
		{
			builder.AppendFormat("{0:0}", f);
			return;
		}
		if (Mathf.Abs(f) < 0.1f && allowHundredths)
		{
			builder.AppendFormat("{0:##0.##}", f);
			return;
		}
		if (Mathf.Abs(f) < 1f)
		{
			builder.AppendFormat("{0:##0.#}", f);
			return;
		}
		builder.AppendFormat("{0:##0}", f);
	}

	// Token: 0x060043AA RID: 17322 RVA: 0x00185A40 File Offset: 0x00183C40
	public static string GetUnitFormattedName(GameObject go, bool upperName = false)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		if (component != null && Assets.IsTagCountable(component.PrefabTag))
		{
			PrimaryElement component2 = go.GetComponent<PrimaryElement>();
			return GameUtil.GetUnitFormattedName(go.GetProperName(), component2.Units, upperName);
		}
		if (!upperName)
		{
			return go.GetProperName();
		}
		return StringFormatter.ToUpper(go.GetProperName());
	}

	// Token: 0x060043AB RID: 17323 RVA: 0x00185A99 File Offset: 0x00183C99
	public static string GetUnitFormattedName(string name, float count, bool upperName = false)
	{
		if (upperName)
		{
			name = name.ToUpper();
		}
		return StringFormatter.Replace(UI.NAME_WITH_UNITS, "{0}", name).Replace("{1}", string.Format("{0:0.##}", count));
	}

	// Token: 0x060043AC RID: 17324 RVA: 0x00185AD8 File Offset: 0x00183CD8
	public static void AppendFormattedUnits(StringBuilder builder, float units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool displaySuffix = true, string floatFormatOverride = "")
	{
		units = GameUtil.ApplyTimeSlice(units, timeSlice);
		if (!floatFormatOverride.IsNullOrWhiteSpace())
		{
			builder.AppendFormat(floatFormatOverride, units);
		}
		else
		{
			GameUtil.AppendStandardFloat(builder, units);
		}
		if (displaySuffix)
		{
			builder.Append((units == 1f) ? UI.UNITSUFFIXES.UNIT : UI.UNITSUFFIXES.UNITS);
		}
		GameUtil.AddTimeSliceText(builder, timeSlice);
	}

	// Token: 0x060043AD RID: 17325 RVA: 0x00185B39 File Offset: 0x00183D39
	public static string GetFormattedUnits(float units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool displaySuffix = true, string floatFormatOverride = "")
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedUnits(stringBuilder, units, timeSlice, displaySuffix, floatFormatOverride);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043AE RID: 17326 RVA: 0x00185B4F File Offset: 0x00183D4F
	public static void AppendFormattedRocketRangePerCycle(StringBuilder builder, float range, bool displaySuffix = true)
	{
		if (displaySuffix)
		{
			builder.AppendFormat("{0:N1} {1}", range, UI.CLUSTERMAP.TILES_PER_CYCLE);
			return;
		}
		builder.AppendFormat("{0:N1}", range);
	}

	// Token: 0x060043AF RID: 17327 RVA: 0x00185B7E File Offset: 0x00183D7E
	public static string GetFormattedRocketRangePerCycle(float range, bool displaySuffix = true)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedRocketRangePerCycle(stringBuilder, range, displaySuffix);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043B0 RID: 17328 RVA: 0x00185B92 File Offset: 0x00183D92
	public static void AppendFormattedRocketRange(StringBuilder builder, int rangeInTiles, bool displaySuffix = true)
	{
		builder.Append(rangeInTiles);
		if (displaySuffix)
		{
			builder.Append(" ");
			builder.Append(UI.CLUSTERMAP.TILES);
		}
	}

	// Token: 0x060043B1 RID: 17329 RVA: 0x00185BBC File Offset: 0x00183DBC
	public static string GetFormattedRocketRange(int rangeInTiles, bool displaySuffix = true)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedRocketRange(stringBuilder, rangeInTiles, displaySuffix);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043B2 RID: 17330 RVA: 0x00185BD0 File Offset: 0x00183DD0
	public static string ApplyBoldString(string source)
	{
		return "<b>" + source + "</b>";
	}

	// Token: 0x060043B3 RID: 17331 RVA: 0x00185BE2 File Offset: 0x00183DE2
	public static void AppendBoldString(StringBuilder builder, string source)
	{
		builder.AppendFormat("<b>{0}</b>", source);
	}

	// Token: 0x060043B4 RID: 17332 RVA: 0x00185BF4 File Offset: 0x00183DF4
	public static float GetRoundedTemperatureInKelvin(float kelvin)
	{
		float result = 0f;
		switch (GameUtil.temperatureUnit)
		{
		case GameUtil.TemperatureUnit.Celsius:
			result = GameUtil.GetTemperatureConvertedToKelvin(Mathf.Round(GameUtil.GetConvertedTemperature(Mathf.Round(kelvin), true)));
			break;
		case GameUtil.TemperatureUnit.Fahrenheit:
			result = GameUtil.GetTemperatureConvertedToKelvin((float)Mathf.RoundToInt(GameUtil.GetTemperatureConvertedFromKelvin(kelvin, GameUtil.TemperatureUnit.Fahrenheit)), GameUtil.TemperatureUnit.Fahrenheit);
			break;
		case GameUtil.TemperatureUnit.Kelvin:
			result = (float)Mathf.RoundToInt(kelvin);
			break;
		}
		return result;
	}

	// Token: 0x060043B5 RID: 17333 RVA: 0x00185C5C File Offset: 0x00183E5C
	public static void AppendFormattedTemperature(StringBuilder builder, float temp, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation interpretation = GameUtil.TemperatureInterpretation.Absolute, bool displayUnits = true, bool roundInDestinationFormat = false)
	{
		if (interpretation != GameUtil.TemperatureInterpretation.Absolute)
		{
			if (interpretation != GameUtil.TemperatureInterpretation.Relative)
			{
			}
			temp = GameUtil.GetConvertedTemperatureDelta(temp);
		}
		else
		{
			temp = GameUtil.GetConvertedTemperature(temp, roundInDestinationFormat);
		}
		temp = GameUtil.ApplyTimeSlice(temp, timeSlice);
		if (Mathf.Abs(temp) < 0.1f)
		{
			builder.AppendFormat("{0:##0.####}", temp);
		}
		else
		{
			builder.AppendFormat("{0:##0.#}", temp);
		}
		if (displayUnits)
		{
			builder.Append(GameUtil.GetTemperatureUnitSuffix());
		}
		GameUtil.AddTimeSliceText(builder, timeSlice);
	}

	// Token: 0x060043B6 RID: 17334 RVA: 0x00185CD9 File Offset: 0x00183ED9
	public static string GetFormattedTemperature(float temp, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation interpretation = GameUtil.TemperatureInterpretation.Absolute, bool displayUnits = true, bool roundInDestinationFormat = false)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedTemperature(stringBuilder, temp, timeSlice, interpretation, displayUnits, roundInDestinationFormat);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043B7 RID: 17335 RVA: 0x00185CF4 File Offset: 0x00183EF4
	public static void AppendFormattedCaloriesForItem(StringBuilder builder, Tag tag, float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool forceKcal = true)
	{
		EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(tag.Name);
		GameUtil.AppendFormattedCalories(builder, (foodInfo != null) ? (foodInfo.CaloriesPerUnit * amount) : -1f, timeSlice, forceKcal);
	}

	// Token: 0x060043B8 RID: 17336 RVA: 0x00185D29 File Offset: 0x00183F29
	public static string GetFormattedCaloriesForItem(Tag tag, float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool forceKcal = true)
	{
		return GameUtil.GetFormattedCaloriesForItem(tag, amount, true, timeSlice, forceKcal);
	}

	// Token: 0x060043B9 RID: 17337 RVA: 0x00185D38 File Offset: 0x00183F38
	public static string GetFormattedCaloriesForItem(Tag tag, float amount, bool showSuffix, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool forceKcal = true)
	{
		EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(tag.Name);
		return GameUtil.GetFormattedCalories((foodInfo != null) ? (foodInfo.CaloriesPerUnit * amount) : -1f, showSuffix, timeSlice, forceKcal);
	}

	// Token: 0x060043BA RID: 17338 RVA: 0x00185D6D File Offset: 0x00183F6D
	public static void AppendFormattedCalories(StringBuilder builder, float calories, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool forceKcal = true)
	{
		GameUtil.AppendFormattedCalories(builder, calories, true, timeSlice, forceKcal);
	}

	// Token: 0x060043BB RID: 17339 RVA: 0x00185D7C File Offset: 0x00183F7C
	public static void AppendFormattedCalories(StringBuilder builder, float calories, bool showSuffix, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool forceKcal = true)
	{
		string value = UI.UNITSUFFIXES.CALORIES.CALORIE;
		if (Mathf.Abs(calories) >= 1000f || forceKcal)
		{
			calories /= 1000f;
			value = UI.UNITSUFFIXES.CALORIES.KILOCALORIE;
		}
		calories = GameUtil.ApplyTimeSlice(calories, timeSlice);
		GameUtil.AppendStandardFloat(builder, calories);
		if (showSuffix)
		{
			builder.Append(value);
			GameUtil.AddTimeSliceText(builder, timeSlice);
		}
	}

	// Token: 0x060043BC RID: 17340 RVA: 0x00185DDF File Offset: 0x00183FDF
	public static string GetFormattedCalories(float calories, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool forceKcal = true)
	{
		return GameUtil.GetFormattedCalories(calories, true, timeSlice, forceKcal);
	}

	// Token: 0x060043BD RID: 17341 RVA: 0x00185DEA File Offset: 0x00183FEA
	public static string GetFormattedCalories(float calories, bool showSuffix, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool forceKcal = true)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedCalories(stringBuilder, calories, showSuffix, timeSlice, forceKcal);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043BE RID: 17342 RVA: 0x00185E00 File Offset: 0x00184000
	public static string GetFormattedPreyConsumptionValuePerCycle(Tag preyTag, float crittersPerSecond, bool perCycle = true)
	{
		Assets.GetPrefab(preyTag).GetComponent<PrimaryElement>();
		return GameUtil.GetFormattedUnits(crittersPerSecond, GameUtil.TimeSlice.PerCycle, true, "");
	}

	// Token: 0x060043BF RID: 17343 RVA: 0x00185E1C File Offset: 0x0018401C
	public static string GetFormattedDirectPlantConsumptionValuePerCycle(Tag plantTag, float consumer_caloriesLossPerCaloriesPerKG, bool perCycle = true)
	{
		IPlantConsumptionInstructions[] plantConsumptionInstructions = GameUtil.GetPlantConsumptionInstructions(Assets.GetPrefab(plantTag));
		if (plantConsumptionInstructions == null || plantConsumptionInstructions.Length == 0)
		{
			return "Error";
		}
		foreach (IPlantConsumptionInstructions plantConsumptionInstructions2 in plantConsumptionInstructions)
		{
			if (plantConsumptionInstructions2.GetDietFoodType() == Diet.Info.FoodType.EatPlantDirectly)
			{
				return plantConsumptionInstructions2.GetFormattedConsumptionPerCycle(consumer_caloriesLossPerCaloriesPerKG);
			}
		}
		return "Error";
	}

	// Token: 0x060043C0 RID: 17344 RVA: 0x00185E6C File Offset: 0x0018406C
	public static string GetFormattedBranchGrowerPlantProductionValuePerCycle(Tag productTag, float outputAmountPerBranch, int branchCount, bool perCycle = true)
	{
		return GameUtil.SafeStringFormat(UI.BUILDINGEFFECTS.TOOLTIPS.BRANCH_GROWER_PLANT_POTENTIAL_OUTPUT, new object[]
		{
			GameUtil.GetFormattedByTag(productTag, outputAmountPerBranch, false, GameUtil.TimeSlice.PerCycle),
			GameUtil.GetFormattedByTag(productTag, outputAmountPerBranch * (float)branchCount, GameUtil.TimeSlice.PerCycle)
		});
	}

	// Token: 0x060043C1 RID: 17345 RVA: 0x00185EA0 File Offset: 0x001840A0
	public static string GetFormattedBranchGrowerPlantPlantFiberProductionValuePerCycle(Tag productTag, float outputAmountPerBranch, int branchCount, bool perCycle = true)
	{
		return GameUtil.SafeStringFormat(UI.BUILDINGEFFECTS.TOOLTIPS.BRANCH_GROWER_PLANT_POTENTIAL_OUTPUT, new object[]
		{
			GameUtil.GetFormattedMass(GameUtil.ApplyTimeSlice(outputAmountPerBranch, GameUtil.TimeSlice.PerCycle), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"),
			GameUtil.GetFormattedMass(outputAmountPerBranch * (float)branchCount, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")
		});
	}

	// Token: 0x060043C2 RID: 17346 RVA: 0x00185EF0 File Offset: 0x001840F0
	public static string GetFormattedPlantStorageConsumptionValuePerCycle(Tag plantTag, float consumer_caloriesLossPerCaloriesPerKG, bool perCycle = true)
	{
		IPlantConsumptionInstructions[] plantConsumptionInstructions = GameUtil.GetPlantConsumptionInstructions(Assets.GetPrefab(plantTag));
		if (plantConsumptionInstructions == null || plantConsumptionInstructions.Length == 0)
		{
			return "Error";
		}
		foreach (IPlantConsumptionInstructions plantConsumptionInstructions2 in plantConsumptionInstructions)
		{
			if (plantConsumptionInstructions2.GetDietFoodType() == Diet.Info.FoodType.EatPlantStorage)
			{
				return plantConsumptionInstructions2.GetFormattedConsumptionPerCycle(consumer_caloriesLossPerCaloriesPerKG);
			}
		}
		return "Error";
	}

	// Token: 0x060043C3 RID: 17347 RVA: 0x00185F40 File Offset: 0x00184140
	public static IPlantConsumptionInstructions[] GetPlantConsumptionInstructions(GameObject prefab)
	{
		IPlantConsumptionInstructions[] components = prefab.GetComponents<IPlantConsumptionInstructions>();
		List<IPlantConsumptionInstructions> allSMI = prefab.GetAllSMI<IPlantConsumptionInstructions>();
		List<IPlantConsumptionInstructions> list = new List<IPlantConsumptionInstructions>();
		if (components != null)
		{
			list.AddRange(components);
		}
		if (allSMI != null)
		{
			list.AddRange(allSMI);
		}
		return list.ToArray();
	}

	// Token: 0x060043C4 RID: 17348 RVA: 0x00185F7C File Offset: 0x0018417C
	public static void AppendFormattedPlantGrowth(StringBuilder builder, float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		percent = GameUtil.ApplyTimeSlice(percent, timeSlice);
		GameUtil.AppendStandardPercentageFloat(builder, percent, true);
		builder.Append(UI.UNITSUFFIXES.PERCENT);
		builder.Append(" ");
		builder.Append(UI.UNITSUFFIXES.GROWTH);
		GameUtil.AddTimeSliceText(builder, timeSlice);
	}

	// Token: 0x060043C5 RID: 17349 RVA: 0x00185FCF File Offset: 0x001841CF
	public static string GetFormattedPlantGrowth(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedPlantGrowth(stringBuilder, percent, timeSlice);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043C6 RID: 17350 RVA: 0x00185FE3 File Offset: 0x001841E3
	public static void AppendFormattedPercent(StringBuilder builder, float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		GameUtil.AppendStandardPercentageFloat(builder, GameUtil.ApplyTimeSlice(percent, timeSlice), false);
		builder.Append(UI.UNITSUFFIXES.PERCENT);
		GameUtil.AddTimeSliceText(builder, timeSlice);
	}

	// Token: 0x060043C7 RID: 17351 RVA: 0x0018600B File Offset: 0x0018420B
	public static string GetFormattedPercent(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedPercent(stringBuilder, percent, timeSlice);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043C8 RID: 17352 RVA: 0x00186020 File Offset: 0x00184220
	public static void AppendFormattedRoundedJoules(StringBuilder builder, float joules)
	{
		if (Mathf.Abs(joules) > 1000f)
		{
			builder.AppendFormat("{0:F1}", joules / 1000f);
			builder.Append(UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE);
			return;
		}
		builder.AppendFormat("{0:F1}", joules);
		builder.Append(UI.UNITSUFFIXES.ELECTRICAL.JOULE);
	}

	// Token: 0x060043C9 RID: 17353 RVA: 0x00186087 File Offset: 0x00184287
	public static string GetFormattedRoundedJoules(float joules)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedRoundedJoules(stringBuilder, joules);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043CA RID: 17354 RVA: 0x0018609C File Offset: 0x0018429C
	public static string GetFormattedJoules(float joules, string floatFormat = "F1", GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		if (timeSlice == GameUtil.TimeSlice.PerSecond)
		{
			return GameUtil.GetFormattedWattage(joules, GameUtil.WattageFormatterUnit.Automatic, true);
		}
		joules = GameUtil.ApplyTimeSlice(joules, timeSlice);
		string text;
		if (Math.Abs(joules) > 1000000f)
		{
			text = GameUtil.FloatToString(joules / 1000000f, floatFormat) + UI.UNITSUFFIXES.ELECTRICAL.MEGAJOULE;
		}
		else if (Mathf.Abs(joules) > 1000f)
		{
			text = GameUtil.FloatToString(joules / 1000f, floatFormat) + UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE;
		}
		else
		{
			text = GameUtil.FloatToString(joules, floatFormat) + UI.UNITSUFFIXES.ELECTRICAL.JOULE;
		}
		return GameUtil.AddTimeSliceText(text, timeSlice);
	}

	// Token: 0x060043CB RID: 17355 RVA: 0x00186135 File Offset: 0x00184335
	public static void AppendFormattedRads(StringBuilder builder, float rads, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		rads = GameUtil.ApplyTimeSlice(rads, timeSlice);
		GameUtil.AppendStandardFloat(builder, rads);
		builder.Append(UI.UNITSUFFIXES.RADIATION.RADS);
		GameUtil.AddTimeSliceText(builder, timeSlice);
	}

	// Token: 0x060043CC RID: 17356 RVA: 0x0018615F File Offset: 0x0018435F
	public static string GetFormattedRads(float rads, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedRads(stringBuilder, rads, timeSlice);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043CD RID: 17357 RVA: 0x00186173 File Offset: 0x00184373
	public static void AppendFormattedHighEnergyParticles(StringBuilder builder, float units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool displayUnits = true)
	{
		GameUtil.AppendFloatWithDecimalPoint(builder, units);
		if (displayUnits)
		{
			builder.Append((units == 1f) ? UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLE : UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES);
		}
		GameUtil.AddTimeSliceText(builder, timeSlice);
	}

	// Token: 0x060043CE RID: 17358 RVA: 0x001861A6 File Offset: 0x001843A6
	public static string GetFormattedHighEnergyParticles(float units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, bool displayUnits = true)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedHighEnergyParticles(stringBuilder, units, timeSlice, displayUnits);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043CF RID: 17359 RVA: 0x001861BC File Offset: 0x001843BC
	public static void AppendFormattedWattage(StringBuilder builder, float watts, GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Automatic, bool displayUnits = true)
	{
		string text = null;
		switch (unit)
		{
		case GameUtil.WattageFormatterUnit.Watts:
			text = UI.UNITSUFFIXES.ELECTRICAL.WATT;
			break;
		case GameUtil.WattageFormatterUnit.Kilowatts:
			watts /= 1000f;
			text = UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
			break;
		case GameUtil.WattageFormatterUnit.Automatic:
			if (Mathf.Abs(watts) > 1000f)
			{
				watts /= 1000f;
				text = UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
			}
			else
			{
				text = UI.UNITSUFFIXES.ELECTRICAL.WATT;
			}
			break;
		}
		GameUtil.AppendFloatToString(builder, watts, "###0.##");
		if (displayUnits && text != null)
		{
			builder.Append(text);
		}
	}

	// Token: 0x060043D0 RID: 17360 RVA: 0x0018624A File Offset: 0x0018444A
	public static string GetFormattedWattage(float watts, GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Automatic, bool displayUnits = true)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedWattage(stringBuilder, watts, unit, displayUnits);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043D1 RID: 17361 RVA: 0x00186260 File Offset: 0x00184460
	public static void AppendFormattedHeatEnergy(StringBuilder builder, float dtu, GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
	{
		string value;
		string format;
		switch (unit)
		{
		case GameUtil.HeatEnergyFormatterUnit.DTU_S:
			value = UI.UNITSUFFIXES.HEAT.DTU;
			format = "###0.";
			break;
		case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
			dtu /= 1000f;
			value = UI.UNITSUFFIXES.HEAT.KDTU;
			format = "###0.##";
			break;
		default:
			if (Mathf.Abs(dtu) > 1000f)
			{
				dtu /= 1000f;
				value = UI.UNITSUFFIXES.HEAT.KDTU;
				format = "###0.##";
			}
			else
			{
				value = UI.UNITSUFFIXES.HEAT.DTU;
				format = "###0.";
			}
			break;
		}
		GameUtil.AppendFloatToString(builder, dtu, format);
		builder.Append(value);
	}

	// Token: 0x060043D2 RID: 17362 RVA: 0x001862FA File Offset: 0x001844FA
	public static string GetFormattedHeatEnergy(float dtu, GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedHeatEnergy(stringBuilder, dtu, unit);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043D3 RID: 17363 RVA: 0x00186310 File Offset: 0x00184510
	public static void AppendFormattedHeatEnergyRate(StringBuilder builder, float dtu_s, GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
	{
		string text = null;
		switch (unit)
		{
		case GameUtil.HeatEnergyFormatterUnit.DTU_S:
			text = UI.UNITSUFFIXES.HEAT.DTU_S;
			break;
		case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
			dtu_s /= 1000f;
			text = UI.UNITSUFFIXES.HEAT.KDTU_S;
			break;
		case GameUtil.HeatEnergyFormatterUnit.Automatic:
			if (Mathf.Abs(dtu_s) > 1000f)
			{
				dtu_s /= 1000f;
				text = UI.UNITSUFFIXES.HEAT.KDTU_S;
			}
			else
			{
				text = UI.UNITSUFFIXES.HEAT.DTU_S;
			}
			break;
		}
		GameUtil.AppendFloatToString(builder, dtu_s, null);
		if (text != null)
		{
			builder.Append(text);
		}
	}

	// Token: 0x060043D4 RID: 17364 RVA: 0x00186397 File Offset: 0x00184597
	public static string GetFormattedHeatEnergyRate(float dtu_s, GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedHeatEnergyRate(stringBuilder, dtu_s, unit);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043D5 RID: 17365 RVA: 0x001863AB File Offset: 0x001845AB
	public static string GetFormattedInt(float num, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		num = GameUtil.ApplyTimeSlice(num, timeSlice);
		return GameUtil.AddTimeSliceText(GameUtil.FloatToString(num, "F0"), timeSlice);
	}

	// Token: 0x060043D6 RID: 17366 RVA: 0x001863C8 File Offset: 0x001845C8
	public static string GetSpeciesNameFromGameObject(GameObject critterGameObject)
	{
		CreatureBrain component = critterGameObject.GetComponent<CreatureBrain>();
		if (component != null)
		{
			return GameUtil.GetNameForSpecies(component.species);
		}
		return "UNKNOWN SPECIES";
	}

	// Token: 0x060043D7 RID: 17367 RVA: 0x001863F8 File Offset: 0x001845F8
	public static string GetNameForSpecies(Tag species)
	{
		Option<string> option = Option.None;
		if (species == GameTags.Creatures.Species.HatchSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.HATCHSPECIES);
		}
		else if (species == GameTags.Creatures.Species.LightBugSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.LIGHTBUGSPECIES);
		}
		else if (species == GameTags.Creatures.Species.OilFloaterSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.OILFLOATERSPECIES);
		}
		else if (species == GameTags.Creatures.Species.DreckoSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.DRECKOSPECIES);
		}
		else if (species == GameTags.Creatures.Species.GlomSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.GLOMSPECIES);
		}
		else if (species == GameTags.Creatures.Species.PuftSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.PUFTSPECIES);
		}
		else if (species == GameTags.Creatures.Species.PacuSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.PACUSPECIES);
		}
		else if (species == GameTags.Creatures.Species.MooSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.MOOSPECIES);
		}
		else if (species == GameTags.Creatures.Species.MoleSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.MOLESPECIES);
		}
		else if (species == GameTags.Creatures.Species.SquirrelSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.SQUIRRELSPECIES);
		}
		else if (species == GameTags.Creatures.Species.CrabSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.CRABSPECIES);
		}
		else if (species == GameTags.Creatures.Species.DivergentSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.DIVERGENTSPECIES);
		}
		else if (species == GameTags.Creatures.Species.StaterpillarSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.STATERPILLARSPECIES);
		}
		else if (species == GameTags.Creatures.Species.BeetaSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.BEETASPECIES);
		}
		else if (species == GameTags.Creatures.Species.BellySpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.BELLYSPECIES);
		}
		else if (species == GameTags.Creatures.Species.SealSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.SEALSPECIES);
		}
		else if (species == GameTags.Creatures.Species.DeerSpecies)
		{
			option = Option.Some<string>(STRINGS.CREATURES.FAMILY_PLURAL.DEERSPECIES);
		}
		else
		{
			option = Option.None;
		}
		return option.Value;
	}

	// Token: 0x060043D8 RID: 17368 RVA: 0x00186658 File Offset: 0x00184858
	public static void AppendFormattedSimple(StringBuilder builder, float num, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, string formatString = null)
	{
		num = GameUtil.ApplyTimeSlice(num, timeSlice);
		if (formatString != null)
		{
			GameUtil.AppendFloatToString(builder, num, formatString);
		}
		else if (num == 0f)
		{
			builder.Append("0");
		}
		else if (Mathf.Abs(num) < 1f)
		{
			GameUtil.AppendFloatToString(builder, num, "#,##0.##");
		}
		else if (Mathf.Abs(num) < 10f)
		{
			GameUtil.AppendFloatToString(builder, num, "#,###.##");
		}
		else
		{
			GameUtil.AppendFloatToString(builder, num, "#,###.##");
		}
		GameUtil.AddTimeSliceText(builder, timeSlice);
	}

	// Token: 0x060043D9 RID: 17369 RVA: 0x001866DA File Offset: 0x001848DA
	public static string GetFormattedSimple(float num, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, string formatString = null)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedSimple(stringBuilder, num, timeSlice, formatString);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043DA RID: 17370 RVA: 0x001866EF File Offset: 0x001848EF
	public static void AppendFormattedLux(StringBuilder builder, int lux)
	{
		builder.Append(lux);
		builder.Append(UI.UNITSUFFIXES.LIGHT.LUX);
	}

	// Token: 0x060043DB RID: 17371 RVA: 0x0018670A File Offset: 0x0018490A
	public static string GetFormattedLux(int lux)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedLux(stringBuilder, lux);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043DC RID: 17372 RVA: 0x00186720 File Offset: 0x00184920
	public static string GetLightDescription(int lux)
	{
		if (lux == 0)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.NO_LIGHT;
		}
		if (lux < DUPLICANTSTATS.STANDARD.Light.LOW_LIGHT)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.VERY_LOW_LIGHT;
		}
		if (lux < DUPLICANTSTATS.STANDARD.Light.MEDIUM_LIGHT)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.LOW_LIGHT;
		}
		if (lux < DUPLICANTSTATS.STANDARD.Light.HIGH_LIGHT)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.MEDIUM_LIGHT;
		}
		if (lux < DUPLICANTSTATS.STANDARD.Light.VERY_HIGH_LIGHT)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.HIGH_LIGHT;
		}
		if (lux < DUPLICANTSTATS.STANDARD.Light.MAX_LIGHT)
		{
			return UI.OVERLAYS.LIGHTING.RANGES.VERY_HIGH_LIGHT;
		}
		return UI.OVERLAYS.LIGHTING.RANGES.MAX_LIGHT;
	}

	// Token: 0x060043DD RID: 17373 RVA: 0x001867D8 File Offset: 0x001849D8
	public static string GetRadiationDescription(float radsPerCycle)
	{
		if (radsPerCycle == 0f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.NONE;
		}
		if (radsPerCycle < 100f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.VERY_LOW;
		}
		if (radsPerCycle < 200f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.LOW;
		}
		if (radsPerCycle < 400f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.MEDIUM;
		}
		if (radsPerCycle < 2000f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.HIGH;
		}
		if (radsPerCycle < 4000f)
		{
			return UI.OVERLAYS.RADIATION.RANGES.VERY_HIGH;
		}
		return UI.OVERLAYS.RADIATION.RANGES.MAX;
	}

	// Token: 0x060043DE RID: 17374 RVA: 0x00186864 File Offset: 0x00184A64
	public static void AppendFormattedByTag(StringBuilder builder, Tag tag, float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		if (GameTags.DisplayAsCalories.Contains(tag))
		{
			GameUtil.AppendFormattedCaloriesForItem(builder, tag, amount, timeSlice, true);
			return;
		}
		if (GameTags.DisplayAsUnits.Contains(tag))
		{
			GameUtil.AppendFormattedUnits(builder, amount, timeSlice, true, "");
			return;
		}
		GameUtil.AppendFormattedMass(builder, amount, timeSlice, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}

	// Token: 0x060043DF RID: 17375 RVA: 0x001868B4 File Offset: 0x00184AB4
	public static string GetFormattedByTag(Tag tag, float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		return GameUtil.GetFormattedByTag(tag, amount, true, timeSlice);
	}

	// Token: 0x060043E0 RID: 17376 RVA: 0x001868C0 File Offset: 0x00184AC0
	public static string GetFormattedByTag(Tag tag, float amount, bool showSuffix, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		if (GameTags.DisplayAsCalories.Contains(tag))
		{
			return GameUtil.GetFormattedCaloriesForItem(tag, amount, showSuffix, timeSlice, true);
		}
		if (GameTags.DisplayAsUnits.Contains(tag))
		{
			return GameUtil.GetFormattedUnits(amount, timeSlice, showSuffix, "");
		}
		return GameUtil.GetFormattedMass(amount, timeSlice, GameUtil.MetricMassFormat.UseThreshold, showSuffix, "{0:0.#}");
	}

	// Token: 0x060043E1 RID: 17377 RVA: 0x00186910 File Offset: 0x00184B10
	public static string GetFormattedFoodQuality(int quality)
	{
		if (GameUtil.adjectives == null)
		{
			GameUtil.adjectives = LocString.GetStrings(typeof(DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVES));
		}
		LocString loc_string = (quality >= 0) ? DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_POSITIVE : DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_NEGATIVE;
		int num = quality - DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_INDEX_OFFSET;
		num = Mathf.Clamp(num, 0, GameUtil.adjectives.Length);
		return string.Format(loc_string, GameUtil.adjectives[num], GameUtil.AddPositiveSign(quality.ToString(), quality > 0));
	}

	// Token: 0x060043E2 RID: 17378 RVA: 0x00186980 File Offset: 0x00184B80
	public static string GetFormattedBytes(ulong amount)
	{
		string[] array = new string[]
		{
			UI.UNITSUFFIXES.INFORMATION.BYTE,
			UI.UNITSUFFIXES.INFORMATION.KILOBYTE,
			UI.UNITSUFFIXES.INFORMATION.MEGABYTE,
			UI.UNITSUFFIXES.INFORMATION.GIGABYTE,
			UI.UNITSUFFIXES.INFORMATION.TERABYTE
		};
		int num = (amount == 0UL) ? 0 : ((int)Math.Floor(Math.Floor(Math.Log(amount)) / Math.Log(1024.0)));
		double num2 = amount / Math.Pow(1024.0, (double)num);
		global::Debug.Assert(num >= 0 && num < array.Length);
		return string.Format("{0:F} {1}", num2, array[num]);
	}

	// Token: 0x060043E3 RID: 17379 RVA: 0x00186A38 File Offset: 0x00184C38
	public static string GetFormattedInfomation(float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		amount = GameUtil.ApplyTimeSlice(amount, timeSlice);
		string str = "";
		if (amount < 1024f)
		{
			str = UI.UNITSUFFIXES.INFORMATION.KILOBYTE;
		}
		else if (amount < 1048576f)
		{
			amount /= 1000f;
			str = UI.UNITSUFFIXES.INFORMATION.MEGABYTE;
		}
		else if (amount < 1.0737418E+09f)
		{
			amount /= 1048576f;
			str = UI.UNITSUFFIXES.INFORMATION.GIGABYTE;
		}
		return GameUtil.AddTimeSliceText(amount.ToString() + str, timeSlice);
	}

	// Token: 0x060043E4 RID: 17380 RVA: 0x00186AB8 File Offset: 0x00184CB8
	public static LocString GetCurrentMassUnit(bool useSmallUnit = false)
	{
		LocString result = null;
		GameUtil.MassUnit massUnit = GameUtil.massUnit;
		if (massUnit != GameUtil.MassUnit.Kilograms)
		{
			if (massUnit == GameUtil.MassUnit.Pounds)
			{
				result = UI.UNITSUFFIXES.MASS.POUND;
			}
		}
		else if (useSmallUnit)
		{
			result = UI.UNITSUFFIXES.MASS.GRAM;
		}
		else
		{
			result = UI.UNITSUFFIXES.MASS.KILOGRAM;
		}
		return result;
	}

	// Token: 0x060043E5 RID: 17381 RVA: 0x00186AF0 File Offset: 0x00184CF0
	public static void AppendFormattedMass(StringBuilder builder, float mass, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.UseThreshold, bool includeSuffix = true, string floatFormat = "{0:0.#}")
	{
		if (mass == -3.4028235E+38f)
		{
			builder.Append(UI.CALCULATING);
			return;
		}
		if (float.IsPositiveInfinity(mass))
		{
			builder.Append(UI.POS_INFINITY);
			builder.Append(UI.UNITSUFFIXES.MASS.TONNE);
			return;
		}
		if (float.IsNegativeInfinity(mass))
		{
			builder.Append(UI.NEG_INFINITY);
			builder.Append(UI.UNITSUFFIXES.MASS.TONNE);
			return;
		}
		mass = GameUtil.ApplyTimeSlice(mass, timeSlice);
		string value;
		if (GameUtil.massUnit == GameUtil.MassUnit.Kilograms)
		{
			value = UI.UNITSUFFIXES.MASS.TONNE;
			if (massFormat == GameUtil.MetricMassFormat.UseThreshold)
			{
				float num = Mathf.Abs(mass);
				if (0f < num)
				{
					if (num < 5E-06f)
					{
						value = UI.UNITSUFFIXES.MASS.MICROGRAM;
						mass = Mathf.Floor(mass * 1E+09f);
					}
					else if (num < 0.005f)
					{
						mass *= 1000000f;
						value = UI.UNITSUFFIXES.MASS.MILLIGRAM;
					}
					else if (Mathf.Abs(mass) < 5f)
					{
						mass *= 1000f;
						value = UI.UNITSUFFIXES.MASS.GRAM;
					}
					else if (Mathf.Abs(mass) < 5000f)
					{
						value = UI.UNITSUFFIXES.MASS.KILOGRAM;
					}
					else
					{
						mass /= 1000f;
						value = UI.UNITSUFFIXES.MASS.TONNE;
					}
				}
				else
				{
					value = UI.UNITSUFFIXES.MASS.KILOGRAM;
				}
			}
			else if (massFormat == GameUtil.MetricMassFormat.Kilogram)
			{
				value = UI.UNITSUFFIXES.MASS.KILOGRAM;
			}
			else if (massFormat == GameUtil.MetricMassFormat.Gram)
			{
				mass *= 1000f;
				value = UI.UNITSUFFIXES.MASS.GRAM;
			}
			else if (massFormat == GameUtil.MetricMassFormat.Tonne)
			{
				mass /= 1000f;
				value = UI.UNITSUFFIXES.MASS.TONNE;
			}
		}
		else
		{
			mass /= 2.2f;
			value = UI.UNITSUFFIXES.MASS.POUND;
			if (massFormat == GameUtil.MetricMassFormat.UseThreshold)
			{
				float num2 = Mathf.Abs(mass);
				if (num2 < 5f && num2 > 0.001f)
				{
					mass *= 256f;
					value = UI.UNITSUFFIXES.MASS.DRACHMA;
				}
				else
				{
					mass *= 7000f;
					value = UI.UNITSUFFIXES.MASS.GRAIN;
				}
			}
		}
		builder.AppendFormat(floatFormat, mass);
		if (includeSuffix)
		{
			builder.Append(value);
			GameUtil.AddTimeSliceText(builder, timeSlice);
		}
	}

	// Token: 0x060043E6 RID: 17382 RVA: 0x00186D18 File Offset: 0x00184F18
	public static string GetFormattedMass(float mass, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None, GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.UseThreshold, bool includeSuffix = true, string floatFormat = "{0:0.#}")
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedMass(stringBuilder, mass, timeSlice, massFormat, includeSuffix, floatFormat);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043E7 RID: 17383 RVA: 0x00186D30 File Offset: 0x00184F30
	public static void AppendFormattedTime(StringBuilder builder, float seconds)
	{
		builder.AppendFormat(UI.FORMATSECONDS, (int)seconds);
	}

	// Token: 0x060043E8 RID: 17384 RVA: 0x00186D4A File Offset: 0x00184F4A
	public static string GetFormattedTime(float seconds, string floatFormat = "F0")
	{
		return string.Format(UI.FORMATSECONDS, seconds.ToString(floatFormat));
	}

	// Token: 0x060043E9 RID: 17385 RVA: 0x00186D63 File Offset: 0x00184F63
	public static void AppendFormattedEngineEfficiency(StringBuilder builder, float amount)
	{
		builder.Append(amount);
		builder.Append(" km /");
		builder.Append(UI.UNITSUFFIXES.MASS.KILOGRAM);
	}

	// Token: 0x060043EA RID: 17386 RVA: 0x00186D8A File Offset: 0x00184F8A
	public static string GetFormattedEngineEfficiency(float amount)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedEngineEfficiency(stringBuilder, amount);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043EB RID: 17387 RVA: 0x00186DA0 File Offset: 0x00184FA0
	public static void AppendFormattedDistance(StringBuilder builder, float meters)
	{
		if (Mathf.Abs(meters) < 1f)
		{
			builder.AppendFormat("{0:0.0} cm", Math.Abs(meters * 100f));
			return;
		}
		if (meters < 1000f)
		{
			builder.Append(meters);
			builder.Append(" m");
			return;
		}
		builder.AppendFormat("{0:0.0} km", meters / 1000f);
	}

	// Token: 0x060043EC RID: 17388 RVA: 0x00186E0D File Offset: 0x0018500D
	public static string GetFormattedDistance(float meters)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedDistance(stringBuilder, meters);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043ED RID: 17389 RVA: 0x00186E20 File Offset: 0x00185020
	public static void AppendFormattedCycles(StringBuilder builder, float seconds, bool forceCycles = false)
	{
		if (forceCycles || Math.Abs(seconds) > 100f)
		{
			builder.AppendFormat(UI.FORMATDAY, seconds / 600f);
			return;
		}
		GameUtil.AppendFormattedTime(builder, seconds);
	}

	// Token: 0x060043EE RID: 17390 RVA: 0x00186E57 File Offset: 0x00185057
	public static string GetFormattedCycles(float seconds, string formatString = "F1", bool forceCycles = false)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendFormattedCycles(stringBuilder, seconds, forceCycles);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x060043EF RID: 17391 RVA: 0x00186E6B File Offset: 0x0018506B
	public static float GetDisplaySHC(float shc)
	{
		if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
		{
			shc /= 1.8f;
		}
		return shc;
	}

	// Token: 0x060043F0 RID: 17392 RVA: 0x00186E7F File Offset: 0x0018507F
	public static string GetSHCSuffix()
	{
		return string.Format("(DTU/g)/{0}", GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x060043F1 RID: 17393 RVA: 0x00186E90 File Offset: 0x00185090
	public static string GetFormattedSHC(float shc)
	{
		shc = GameUtil.GetDisplaySHC(shc);
		return string.Format("{0} (DTU/g)/{1}", shc.ToString("0.000"), GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x060043F2 RID: 17394 RVA: 0x00186EB5 File Offset: 0x001850B5
	public static float GetDisplayThermalConductivity(float tc)
	{
		if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
		{
			tc /= 1.8f;
		}
		return tc;
	}

	// Token: 0x060043F3 RID: 17395 RVA: 0x00186EC9 File Offset: 0x001850C9
	public static string GetThermalConductivitySuffix()
	{
		return string.Format("(DTU/(m*s))/{0}", GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x060043F4 RID: 17396 RVA: 0x00186EDA File Offset: 0x001850DA
	public static string GetFormattedThermalConductivity(float tc)
	{
		tc = GameUtil.GetDisplayThermalConductivity(tc);
		return string.Format("{0} (DTU/(m*s))/{1}", tc.ToString("0.000"), GameUtil.GetTemperatureUnitSuffix());
	}

	// Token: 0x060043F5 RID: 17397 RVA: 0x00186EFF File Offset: 0x001850FF
	public static string GetElementNameByElementHash(SimHashes elementHash)
	{
		return ElementLoader.FindElementByHash(elementHash).tag.ProperName();
	}

	// Token: 0x060043F6 RID: 17398 RVA: 0x00186F14 File Offset: 0x00185114
	public static string SafeStringFormat(string source, params object[] args)
	{
		for (int i = 0; i < args.Length; i++)
		{
			string text = "{" + i.ToString() + "}";
			if (!source.Contains(text))
			{
				KCrashReporter.ReportDevNotification(string.Format("Format error in string: \"{0}\". Source is missing the {{{1}}} format marker for argument \"{2}\" insertion.", source, i, args[i]), Environment.StackTrace, "", false, null);
			}
			else
			{
				source = source.Replace(text, args[i].ToString());
			}
		}
		return source;
	}

	// Token: 0x060043F7 RID: 17399 RVA: 0x00186F88 File Offset: 0x00185188
	public static bool HasTrait(GameObject go, string traitName)
	{
		Traits component = go.GetComponent<Traits>();
		return !(component == null) && component.HasTrait(traitName);
	}

	// Token: 0x060043F8 RID: 17400 RVA: 0x00186FB0 File Offset: 0x001851B0
	public static HashSet<int> GetFloodFillCavity(int startCell, bool allowLiquid)
	{
		HashSet<int> result = new HashSet<int>();
		if (allowLiquid)
		{
			result = GameUtil.FloodCollectCells(startCell, (int cell) => !Grid.Solid[cell], 300, null, true);
		}
		else
		{
			result = GameUtil.FloodCollectCells(startCell, (int cell) => Grid.Element[cell].IsVacuum || Grid.Element[cell].IsGas, 300, null, true);
		}
		return result;
	}

	// Token: 0x060043F9 RID: 17401 RVA: 0x00187024 File Offset: 0x00185224
	public static float GetRadiationAbsorptionPercentage(int cell)
	{
		if (Grid.IsValidCell(cell))
		{
			return GameUtil.GetRadiationAbsorptionPercentage(Grid.Element[cell], Grid.Mass[cell], Grid.IsSolidCell(cell) && (Grid.Properties[cell] & 128) == 128);
		}
		return 0f;
	}

	// Token: 0x060043FA RID: 17402 RVA: 0x0018707C File Offset: 0x0018527C
	public static float GetRadiationAbsorptionPercentage(Element elem, float mass, bool isConstructed)
	{
		float num = 2000f;
		float num2 = 0.3f;
		float num3 = 0.7f;
		float num4 = 0.8f;
		float value;
		if (isConstructed)
		{
			value = elem.radiationAbsorptionFactor * num4;
		}
		else
		{
			value = elem.radiationAbsorptionFactor * num2 + mass / num * elem.radiationAbsorptionFactor * num3;
		}
		return Mathf.Clamp(value, 0f, 1f);
	}

	// Token: 0x060043FB RID: 17403 RVA: 0x001870E0 File Offset: 0x001852E0
	public static HashSet<int> CollectCellsBreadthFirst(int start_cell, Func<int, bool> test_func, int max_depth = 10)
	{
		HashSet<int> hashSet = new HashSet<int>();
		HashSet<int> hashSet2 = new HashSet<int>();
		HashSet<int> hashSet3 = new HashSet<int>();
		hashSet3.Add(start_cell);
		Vector2Int[] array = new Vector2Int[]
		{
			new Vector2Int(1, 0),
			new Vector2Int(-1, 0),
			new Vector2Int(0, 1),
			new Vector2Int(0, -1)
		};
		for (int i = 0; i < max_depth; i++)
		{
			List<int> list = new List<int>();
			foreach (int cell in hashSet3)
			{
				foreach (Vector2Int vector2Int in array)
				{
					int num = Grid.OffsetCell(cell, vector2Int.x, vector2Int.y);
					if (!hashSet2.Contains(num) && !hashSet.Contains(num))
					{
						if (Grid.IsValidCell(num) && test_func(num))
						{
							hashSet.Add(num);
							list.Add(num);
						}
						else
						{
							hashSet2.Add(num);
						}
					}
				}
			}
			hashSet3.Clear();
			foreach (int item in list)
			{
				hashSet3.Add(item);
			}
			list.Clear();
			if (hashSet3.Count == 0)
			{
				break;
			}
		}
		return hashSet;
	}

	// Token: 0x060043FC RID: 17404 RVA: 0x0018727C File Offset: 0x0018547C
	public static HashSet<int> FloodCollectCells(int start_cell, Func<int, bool> is_valid, int maxSize = 300, HashSet<int> AddInvalidCellsToSet = null, bool clearOversizedResults = true)
	{
		HashSet<int> hashSet = new HashSet<int>();
		HashSet<int> hashSet2 = new HashSet<int>();
		GameUtil.probeFromCell(start_cell, is_valid, hashSet, hashSet2, maxSize);
		if (AddInvalidCellsToSet != null)
		{
			AddInvalidCellsToSet.UnionWith(hashSet2);
			if (hashSet.Count > maxSize)
			{
				AddInvalidCellsToSet.UnionWith(hashSet);
			}
		}
		if (hashSet.Count > maxSize && clearOversizedResults)
		{
			hashSet.Clear();
		}
		return hashSet;
	}

	// Token: 0x060043FD RID: 17405 RVA: 0x001872D0 File Offset: 0x001854D0
	public static HashSet<int> FloodCollectCells(HashSet<int> results, int start_cell, Func<int, bool> is_valid, int maxSize = 300, HashSet<int> AddInvalidCellsToSet = null, bool clearOversizedResults = true)
	{
		HashSet<int> hashSet = new HashSet<int>();
		GameUtil.probeFromCell(start_cell, is_valid, results, hashSet, maxSize);
		if (AddInvalidCellsToSet != null)
		{
			AddInvalidCellsToSet.UnionWith(hashSet);
			if (results.Count > maxSize)
			{
				AddInvalidCellsToSet.UnionWith(results);
			}
		}
		if (results.Count > maxSize && clearOversizedResults)
		{
			results.Clear();
		}
		return results;
	}

	// Token: 0x060043FE RID: 17406 RVA: 0x00187320 File Offset: 0x00185520
	private static void probeFromCell(int start_cell, Func<int, bool> is_valid, HashSet<int> cells, HashSet<int> invalidCells, int maxSize = 300)
	{
		if (cells.Count > maxSize || !Grid.IsValidCell(start_cell) || invalidCells.Contains(start_cell) || cells.Contains(start_cell) || !is_valid(start_cell))
		{
			invalidCells.Add(start_cell);
			return;
		}
		cells.Add(start_cell);
		GameUtil.probeFromCell(Grid.CellLeft(start_cell), is_valid, cells, invalidCells, maxSize);
		GameUtil.probeFromCell(Grid.CellRight(start_cell), is_valid, cells, invalidCells, maxSize);
		GameUtil.probeFromCell(Grid.CellAbove(start_cell), is_valid, cells, invalidCells, maxSize);
		GameUtil.probeFromCell(Grid.CellBelow(start_cell), is_valid, cells, invalidCells, maxSize);
	}

	// Token: 0x060043FF RID: 17407 RVA: 0x001873AB File Offset: 0x001855AB
	public static bool FloodFillCheck<ArgType>(Func<int, ArgType, bool> fn, ArgType arg, int start_cell, int max_depth, bool stop_at_solid, bool stop_at_liquid)
	{
		return GameUtil.FloodFillFind<ArgType>(fn, arg, start_cell, max_depth, stop_at_solid, stop_at_liquid) != -1;
	}

	// Token: 0x06004400 RID: 17408 RVA: 0x001873C0 File Offset: 0x001855C0
	private static void FillThreadLocalNeighbors(int cell)
	{
		GameUtil.FloodFillNeighbors.Value[0] = Grid.CellLeft(cell);
		GameUtil.FloodFillNeighbors.Value[1] = Grid.CellAbove(cell);
		GameUtil.FloodFillNeighbors.Value[2] = Grid.CellRight(cell);
		GameUtil.FloodFillNeighbors.Value[3] = Grid.CellBelow(cell);
	}

	// Token: 0x06004401 RID: 17409 RVA: 0x00187428 File Offset: 0x00185628
	private static bool CellCheck(int cell, bool stop_at_solid, bool stop_at_liquid)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		Element element = Grid.Element[cell];
		return (!stop_at_solid || !element.IsSolid) && (!stop_at_liquid || !element.IsLiquid) && !GameUtil.FloodFillVisited.Value.Contains(cell);
	}

	// Token: 0x06004402 RID: 17410 RVA: 0x00187478 File Offset: 0x00185678
	public static int FloodFillFind<ArgType>(Func<int, ArgType, bool> fn, ArgType arg, int start_cell, int max_depth, bool stop_at_solid, bool stop_at_liquid)
	{
		if (GameUtil.CellCheck(start_cell, stop_at_solid, stop_at_liquid))
		{
			GameUtil.FloodFillNext.Value.Enqueue(new GameUtil.FloodFillInfo
			{
				cell = start_cell,
				depth = 0
			});
		}
		int result = -1;
		while (GameUtil.FloodFillNext.Value.Count > 0)
		{
			GameUtil.FloodFillInfo floodFillInfo = GameUtil.FloodFillNext.Value.Dequeue();
			if (!GameUtil.FloodFillVisited.Value.Contains(floodFillInfo.cell))
			{
				GameUtil.FloodFillVisited.Value.Add(floodFillInfo.cell);
				if (fn(floodFillInfo.cell, arg))
				{
					result = floodFillInfo.cell;
					break;
				}
				if (floodFillInfo.depth < max_depth)
				{
					GameUtil.FillThreadLocalNeighbors(floodFillInfo.cell);
					foreach (int cell in GameUtil.FloodFillNeighbors.Value)
					{
						if (GameUtil.CellCheck(cell, stop_at_solid, stop_at_liquid))
						{
							GameUtil.FloodFillNext.Value.Enqueue(new GameUtil.FloodFillInfo
							{
								cell = cell,
								depth = floodFillInfo.depth + 1
							});
						}
					}
				}
			}
		}
		GameUtil.FloodFillVisited.Value.Clear();
		GameUtil.FloodFillNext.Value.Clear();
		return result;
	}

	// Token: 0x06004403 RID: 17411 RVA: 0x001875E4 File Offset: 0x001857E4
	public static int FloodFillFindBest<ArgType>(Func<int, ArgType, float> rateCell, ArgType arg, Func<int, ArgType, bool> validCheck, int startCell, int maxCellEvaluations = -1)
	{
		if (!validCheck(startCell, arg))
		{
			return Grid.InvalidCell;
		}
		float num = rateCell(startCell, arg);
		int result = startCell;
		if (validCheck(startCell, arg))
		{
			GameUtil.FloodFillNext.Value.Enqueue(new GameUtil.FloodFillInfo
			{
				cell = startCell,
				depth = 0
			});
		}
		GameUtil.FloodFillVisited.Value.Add(Grid.InvalidCell);
		GameUtil.FloodFillVisited.Value.Add(startCell);
		while (GameUtil.FloodFillNext.Value.Count > 0 && maxCellEvaluations != 0)
		{
			GameUtil.FloodFillInfo floodFillInfo = GameUtil.FloodFillNext.Value.Dequeue();
			float num2 = rateCell(floodFillInfo.cell, arg);
			if (num2 > num)
			{
				num = num2;
				result = floodFillInfo.cell;
			}
			GameUtil.FillThreadLocalNeighbors(floodFillInfo.cell);
			foreach (int num3 in GameUtil.FloodFillNeighbors.Value)
			{
				if (!GameUtil.FloodFillVisited.Value.Contains(num3) && validCheck(num3, arg))
				{
					GameUtil.FloodFillNext.Value.Enqueue(new GameUtil.FloodFillInfo
					{
						cell = num3,
						depth = floodFillInfo.depth + 1
					});
					GameUtil.FloodFillVisited.Value.Add(num3);
				}
			}
			if (maxCellEvaluations > 0)
			{
				maxCellEvaluations--;
			}
		}
		GameUtil.FloodFillNext.Value.Clear();
		GameUtil.FloodFillVisited.Value.Clear();
		return result;
	}

	// Token: 0x06004404 RID: 17412 RVA: 0x00187790 File Offset: 0x00185990
	public static void FloodFillConditional(int start_cell, Func<int, bool> condition, HashSet<int> visited_cells, List<int> valid_cells = null)
	{
		GameUtil.FloodFillNext.Value.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = start_cell,
			depth = 0
		});
		GameUtil.FloodFillConditional(GameUtil.FloodFillNext.Value, condition, visited_cells, valid_cells, 10000);
	}

	// Token: 0x06004405 RID: 17413 RVA: 0x001877DC File Offset: 0x001859DC
	public static void FloodFillConditional(Queue<GameUtil.FloodFillInfo> queue, Func<int, bool> condition, HashSet<int> visited_cells, List<int> valid_cells = null, int max_depth = 10000)
	{
		while (queue.Count > 0)
		{
			GameUtil.FloodFillInfo floodFillInfo = queue.Dequeue();
			if (floodFillInfo.depth < max_depth && Grid.IsValidCell(floodFillInfo.cell) && visited_cells.Add(floodFillInfo.cell) && condition(floodFillInfo.cell))
			{
				if (valid_cells != null)
				{
					valid_cells.Add(floodFillInfo.cell);
				}
				int depth = floodFillInfo.depth + 1;
				queue.Enqueue(new GameUtil.FloodFillInfo
				{
					cell = Grid.CellLeft(floodFillInfo.cell),
					depth = depth
				});
				queue.Enqueue(new GameUtil.FloodFillInfo
				{
					cell = Grid.CellRight(floodFillInfo.cell),
					depth = depth
				});
				queue.Enqueue(new GameUtil.FloodFillInfo
				{
					cell = Grid.CellAbove(floodFillInfo.cell),
					depth = depth
				});
				queue.Enqueue(new GameUtil.FloodFillInfo
				{
					cell = Grid.CellBelow(floodFillInfo.cell),
					depth = depth
				});
			}
		}
		queue.Clear();
	}

	// Token: 0x06004406 RID: 17414 RVA: 0x00187904 File Offset: 0x00185B04
	public static void AppendHardnessString(StringBuilder builder, Element element, bool addColor = true)
	{
		if (!element.IsSolid)
		{
			builder.Append(ELEMENTS.HARDNESS.NA);
			return;
		}
		Color c = GameUtil.Hardness.firmColor;
		string format;
		if (element.hardness >= 255)
		{
			c = GameUtil.Hardness.ImpenetrableColor;
			format = ELEMENTS.HARDNESS.IMPENETRABLE;
		}
		else if (element.hardness >= 150)
		{
			c = GameUtil.Hardness.nearlyImpenetrableColor;
			format = ELEMENTS.HARDNESS.NEARLYIMPENETRABLE;
		}
		else if (element.hardness >= 50)
		{
			c = GameUtil.Hardness.veryFirmColor;
			format = ELEMENTS.HARDNESS.VERYFIRM;
		}
		else if (element.hardness >= 25)
		{
			c = GameUtil.Hardness.firmColor;
			format = ELEMENTS.HARDNESS.FIRM;
		}
		else if (element.hardness >= 10)
		{
			c = GameUtil.Hardness.softColor;
			format = ELEMENTS.HARDNESS.SOFT;
		}
		else
		{
			c = GameUtil.Hardness.verySoftColor;
			format = ELEMENTS.HARDNESS.VERYSOFT;
		}
		if (addColor)
		{
			builder.AppendFormat("<color=#{0}>", c.ToHexString());
		}
		builder.AppendFormat(format, element.hardness);
		if (addColor)
		{
			builder.Append("</color>");
		}
	}

	// Token: 0x06004407 RID: 17415 RVA: 0x00187A15 File Offset: 0x00185C15
	public static string GetHardnessString(Element element, bool addColor = true)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		GameUtil.AppendHardnessString(stringBuilder, element, addColor);
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}

	// Token: 0x06004408 RID: 17416 RVA: 0x00187A2C File Offset: 0x00185C2C
	public static string GetGermResistanceModifierString(float modifier, bool addColor = true)
	{
		Color c = Color.black;
		string text = "";
		if (modifier > 0f)
		{
			if (modifier >= 5f)
			{
				c = GameUtil.GermResistanceValues.PositiveLargeColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_LARGE, modifier);
			}
			else if (modifier >= 2f)
			{
				c = GameUtil.GermResistanceValues.PositiveMediumColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_MEDIUM, modifier);
			}
			else if (modifier > 0f)
			{
				c = GameUtil.GermResistanceValues.PositiveSmallColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_SMALL, modifier);
			}
		}
		else if (modifier < 0f)
		{
			if (modifier <= -5f)
			{
				c = GameUtil.GermResistanceValues.NegativeLargeColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_LARGE, modifier);
			}
			else if (modifier <= -2f)
			{
				c = GameUtil.GermResistanceValues.NegativeMediumColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_MEDIUM, modifier);
			}
			else if (modifier < 0f)
			{
				c = GameUtil.GermResistanceValues.NegativeSmallColor;
				text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_SMALL, modifier);
			}
		}
		else
		{
			addColor = false;
			text = string.Format(DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NONE, modifier);
		}
		if (addColor)
		{
			text = string.Format("<color=#{0}>{1}</color>", c.ToHexString(), text);
		}
		return text;
	}

	// Token: 0x06004409 RID: 17417 RVA: 0x00187B74 File Offset: 0x00185D74
	public static string GetThermalConductivityString(Element element, bool addColor = true, bool addValue = true)
	{
		Color c = GameUtil.ThermalConductivityValues.mediumConductivityColor;
		string text;
		if (element.thermalConductivity >= 50f)
		{
			c = GameUtil.ThermalConductivityValues.veryHighConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_HIGH_CONDUCTIVITY;
		}
		else if (element.thermalConductivity >= 10f)
		{
			c = GameUtil.ThermalConductivityValues.highConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.HIGH_CONDUCTIVITY;
		}
		else if (element.thermalConductivity >= 2f)
		{
			c = GameUtil.ThermalConductivityValues.mediumConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.MEDIUM_CONDUCTIVITY;
		}
		else if (element.thermalConductivity >= 1f)
		{
			c = GameUtil.ThermalConductivityValues.lowConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.LOW_CONDUCTIVITY;
		}
		else
		{
			c = GameUtil.ThermalConductivityValues.veryLowConductivityColor;
			text = UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_LOW_CONDUCTIVITY;
		}
		if (addColor)
		{
			text = string.Format("<color=#{0}>{1}</color>", c.ToHexString(), text);
		}
		if (addValue)
		{
			text = string.Format(UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VALUE_WITH_ADJECTIVE, element.thermalConductivity.ToString(), text);
		}
		return text;
	}

	// Token: 0x0600440A RID: 17418 RVA: 0x00187C54 File Offset: 0x00185E54
	public static string GetBreathableString(Element element, float Mass)
	{
		if (!element.IsGas && !element.IsVacuum)
		{
			return "";
		}
		Color c = GameUtil.BreathableValues.positiveColor;
		SimHashes id = element.id;
		LocString arg;
		if (id != SimHashes.Oxygen)
		{
			if (id != SimHashes.ContaminatedOxygen)
			{
				c = GameUtil.BreathableValues.negativeColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND4;
			}
			else if (Mass >= SimDebugView.optimallyBreathable)
			{
				c = GameUtil.BreathableValues.positiveColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND1;
			}
			else if (Mass >= SimDebugView.minimumBreathable + (SimDebugView.optimallyBreathable - SimDebugView.minimumBreathable) / 2f)
			{
				c = GameUtil.BreathableValues.positiveColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND2;
			}
			else if (Mass >= SimDebugView.minimumBreathable)
			{
				c = GameUtil.BreathableValues.warningColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND3;
			}
			else
			{
				c = GameUtil.BreathableValues.negativeColor;
				arg = UI.OVERLAYS.OXYGEN.LEGEND4;
			}
		}
		else if (Mass >= SimDebugView.optimallyBreathable)
		{
			c = GameUtil.BreathableValues.positiveColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND1;
		}
		else if (Mass >= SimDebugView.minimumBreathable + (SimDebugView.optimallyBreathable - SimDebugView.minimumBreathable) / 2f)
		{
			c = GameUtil.BreathableValues.positiveColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND2;
		}
		else if (Mass >= SimDebugView.minimumBreathable)
		{
			c = GameUtil.BreathableValues.warningColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND3;
		}
		else
		{
			c = GameUtil.BreathableValues.negativeColor;
			arg = UI.OVERLAYS.OXYGEN.LEGEND4;
		}
		return string.Format(ELEMENTS.BREATHABLEDESC, c.ToHexString(), arg);
	}

	// Token: 0x0600440B RID: 17419 RVA: 0x00187D88 File Offset: 0x00185F88
	public static string GetWireLoadColor(float load, float maxLoad, float potentialLoad)
	{
		Color c;
		if (load > maxLoad + POWER.FLOAT_FUDGE_FACTOR)
		{
			c = GameUtil.WireLoadValues.negativeColor;
		}
		else if (potentialLoad > maxLoad && load / maxLoad >= 0.75f)
		{
			c = GameUtil.WireLoadValues.warningColor;
		}
		else
		{
			c = Color.white;
		}
		return c.ToHexString();
	}

	// Token: 0x0600440C RID: 17420 RVA: 0x00187DC9 File Offset: 0x00185FC9
	public static string GetHotkeyString(global::Action action)
	{
		if (KInputManager.currentControllerIsGamepad)
		{
			return UI.FormatAsHotkey(GameUtil.GetActionString(action));
		}
		return UI.FormatAsHotkey("[" + GameUtil.GetActionString(action) + "]");
	}

	// Token: 0x0600440D RID: 17421 RVA: 0x00187DF8 File Offset: 0x00185FF8
	public static string ReplaceHotkeyString(string template, global::Action action)
	{
		return template.Replace("{Hotkey}", GameUtil.GetHotkeyString(action));
	}

	// Token: 0x0600440E RID: 17422 RVA: 0x00187E0B File Offset: 0x0018600B
	public static string ReplaceHotkeyString(string template, global::Action action1, global::Action action2)
	{
		return template.Replace("{Hotkey}", GameUtil.GetHotkeyString(action1) + GameUtil.GetHotkeyString(action2));
	}

	// Token: 0x0600440F RID: 17423 RVA: 0x00187E2C File Offset: 0x0018602C
	public static string GetKeycodeLocalized(KKeyCode key_code)
	{
		string result = key_code.ToString();
		if (key_code <= KKeyCode.Slash)
		{
			if (key_code <= KKeyCode.Tab)
			{
				if (key_code == KKeyCode.None)
				{
					return result;
				}
				if (key_code == KKeyCode.Backspace)
				{
					return INPUT.BACKSPACE;
				}
				if (key_code == KKeyCode.Tab)
				{
					return INPUT.TAB;
				}
			}
			else if (key_code <= KKeyCode.Escape)
			{
				if (key_code == KKeyCode.Return)
				{
					return INPUT.ENTER;
				}
				if (key_code == KKeyCode.Escape)
				{
					return INPUT.ESCAPE;
				}
			}
			else
			{
				if (key_code == KKeyCode.Space)
				{
					return INPUT.SPACE;
				}
				switch (key_code)
				{
				case KKeyCode.Plus:
					return "+";
				case KKeyCode.Comma:
					return ",";
				case KKeyCode.Minus:
					return "-";
				case KKeyCode.Period:
					return INPUT.PERIOD;
				case KKeyCode.Slash:
					return "/";
				}
			}
		}
		else if (key_code <= KKeyCode.Insert)
		{
			switch (key_code)
			{
			case KKeyCode.Colon:
				return ":";
			case KKeyCode.Semicolon:
				return ";";
			case KKeyCode.Less:
				break;
			case KKeyCode.Equals:
				return "=";
			default:
				switch (key_code)
				{
				case KKeyCode.LeftBracket:
					return "[";
				case KKeyCode.Backslash:
					return "\\";
				case KKeyCode.RightBracket:
					return "]";
				case KKeyCode.Caret:
				case KKeyCode.Underscore:
					break;
				case KKeyCode.BackQuote:
					return INPUT.BACKQUOTE;
				default:
					switch (key_code)
					{
					case KKeyCode.Keypad0:
						return INPUT.NUM + " 0";
					case KKeyCode.Keypad1:
						return INPUT.NUM + " 1";
					case KKeyCode.Keypad2:
						return INPUT.NUM + " 2";
					case KKeyCode.Keypad3:
						return INPUT.NUM + " 3";
					case KKeyCode.Keypad4:
						return INPUT.NUM + " 4";
					case KKeyCode.Keypad5:
						return INPUT.NUM + " 5";
					case KKeyCode.Keypad6:
						return INPUT.NUM + " 6";
					case KKeyCode.Keypad7:
						return INPUT.NUM + " 7";
					case KKeyCode.Keypad8:
						return INPUT.NUM + " 8";
					case KKeyCode.Keypad9:
						return INPUT.NUM + " 9";
					case KKeyCode.KeypadPeriod:
						return INPUT.NUM + " " + INPUT.PERIOD;
					case KKeyCode.KeypadDivide:
						return INPUT.NUM + " /";
					case KKeyCode.KeypadMultiply:
						return INPUT.NUM + " *";
					case KKeyCode.KeypadMinus:
						return INPUT.NUM + " -";
					case KKeyCode.KeypadPlus:
						return INPUT.NUM + " +";
					case KKeyCode.KeypadEnter:
						return INPUT.NUM + " " + INPUT.ENTER;
					case KKeyCode.Insert:
						return INPUT.INSERT;
					}
					break;
				}
				break;
			}
		}
		else if (key_code <= KKeyCode.Mouse6)
		{
			switch (key_code)
			{
			case KKeyCode.RightShift:
				return INPUT.RIGHT_SHIFT;
			case KKeyCode.LeftShift:
				return INPUT.LEFT_SHIFT;
			case KKeyCode.RightControl:
				return INPUT.RIGHT_CTRL;
			case KKeyCode.LeftControl:
				return INPUT.LEFT_CTRL;
			case KKeyCode.RightAlt:
				return INPUT.RIGHT_ALT;
			case KKeyCode.LeftAlt:
				return INPUT.LEFT_ALT;
			default:
				switch (key_code)
				{
				case KKeyCode.Mouse0:
					return INPUT.MOUSE + " 0";
				case KKeyCode.Mouse1:
					return INPUT.MOUSE + " 1";
				case KKeyCode.Mouse2:
					return INPUT.MOUSE + " 2";
				case KKeyCode.Mouse3:
					return INPUT.MOUSE + " 3";
				case KKeyCode.Mouse4:
					return INPUT.MOUSE + " 4";
				case KKeyCode.Mouse5:
					return INPUT.MOUSE + " 5";
				case KKeyCode.Mouse6:
					return INPUT.MOUSE + " 6";
				}
				break;
			}
		}
		else
		{
			if (key_code == KKeyCode.MouseScrollDown)
			{
				return INPUT.MOUSE_SCROLL_DOWN;
			}
			if (key_code == KKeyCode.MouseScrollUp)
			{
				return INPUT.MOUSE_SCROLL_UP;
			}
		}
		if (KKeyCode.A <= key_code && key_code <= KKeyCode.Z)
		{
			result = ((char)(65 + (key_code - KKeyCode.A))).ToString();
		}
		else if (KKeyCode.Alpha0 <= key_code && key_code <= KKeyCode.Alpha9)
		{
			result = ((char)(48 + (key_code - KKeyCode.Alpha0))).ToString();
		}
		else if (KKeyCode.F1 <= key_code && key_code <= KKeyCode.F12)
		{
			result = "F" + (key_code - KKeyCode.F1 + 1).ToString();
		}
		else
		{
			global::Debug.LogWarning("Unable to find proper string for KKeyCode: " + key_code.ToString() + " using key_code.ToString()");
		}
		return result;
	}

	// Token: 0x06004410 RID: 17424 RVA: 0x00188434 File Offset: 0x00186634
	public static string GetActionString(global::Action action)
	{
		string result = "";
		if (action == global::Action.NumActions)
		{
			return result;
		}
		BindingEntry bindingEntry = GameUtil.ActionToBinding(action);
		KKeyCode mKeyCode = bindingEntry.mKeyCode;
		if (KInputManager.currentControllerIsGamepad)
		{
			return KInputManager.steamInputInterpreter.GetActionGlyph(action);
		}
		if (bindingEntry.mModifier == global::Modifier.None)
		{
			return GameUtil.GetKeycodeLocalized(mKeyCode).ToUpper();
		}
		string str = "";
		global::Modifier mModifier = bindingEntry.mModifier;
		switch (mModifier)
		{
		case global::Modifier.Alt:
			str = INPUT.ALT.ToString();
			break;
		case global::Modifier.Ctrl:
			str = INPUT.CTRL.ToString();
			break;
		case (global::Modifier)3:
			break;
		case global::Modifier.Shift:
			str = INPUT.SHIFT.ToString();
			break;
		default:
			if (mModifier != global::Modifier.CapsLock)
			{
				if (mModifier == global::Modifier.Backtick)
				{
					str = GameUtil.GetKeycodeLocalized(KKeyCode.BackQuote);
				}
			}
			else
			{
				str = GameUtil.GetKeycodeLocalized(KKeyCode.CapsLock);
			}
			break;
		}
		return (str + " + " + GameUtil.GetKeycodeLocalized(mKeyCode)).ToUpper();
	}

	// Token: 0x06004411 RID: 17425 RVA: 0x0018850C File Offset: 0x0018670C
	public static void CreateExplosion(Vector3 explosion_pos)
	{
		Vector2 b = new Vector2(explosion_pos.x, explosion_pos.y);
		float num = 5f;
		float num2 = num * num;
		foreach (Health health in Components.Health.Items)
		{
			Vector3 position = health.transform.GetPosition();
			float sqrMagnitude = (new Vector2(position.x, position.y) - b).sqrMagnitude;
			if (num2 >= sqrMagnitude && health != null)
			{
				health.Damage(health.maxHitPoints);
			}
		}
	}

	// Token: 0x06004412 RID: 17426 RVA: 0x001885C4 File Offset: 0x001867C4
	private static void GetNonSolidCells(int x, int y, List<int> cells, int min_x, int min_y, int max_x, int max_y)
	{
		int num = Grid.XYToCell(x, y);
		if (Grid.IsValidCell(num) && !Grid.Solid[num] && !Grid.DupePassable[num] && x >= min_x && x <= max_x && y >= min_y && y <= max_y && !cells.Contains(num))
		{
			cells.Add(num);
			GameUtil.GetNonSolidCells(x + 1, y, cells, min_x, min_y, max_x, max_y);
			GameUtil.GetNonSolidCells(x - 1, y, cells, min_x, min_y, max_x, max_y);
			GameUtil.GetNonSolidCells(x, y + 1, cells, min_x, min_y, max_x, max_y);
			GameUtil.GetNonSolidCells(x, y - 1, cells, min_x, min_y, max_x, max_y);
		}
	}

	// Token: 0x06004413 RID: 17427 RVA: 0x00188668 File Offset: 0x00186868
	public static void GetNonSolidCells(int cell, int radius, List<int> cells)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		GameUtil.GetNonSolidCells(num, num2, cells, num - radius, num2 - radius, num + radius, num2 + radius);
	}

	// Token: 0x06004414 RID: 17428 RVA: 0x00188698 File Offset: 0x00186898
	public static float GetMaxStressInActiveWorld()
	{
		if (Components.LiveMinionIdentities.Count <= 0)
		{
			return 0f;
		}
		float num = 0f;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Stress.Lookup(minionIdentity);
				if (amountInstance != null)
				{
					num = Mathf.Max(num, amountInstance.value);
				}
			}
		}
		return num;
	}

	// Token: 0x06004415 RID: 17429 RVA: 0x00188744 File Offset: 0x00186944
	public static float GetAverageStressInActiveWorld()
	{
		if (Components.LiveMinionIdentities.Count <= 0)
		{
			return 0f;
		}
		float num = 0f;
		int num2 = 0;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
			{
				num += Db.Get().Amounts.Stress.Lookup(minionIdentity).value;
				num2++;
			}
		}
		return num / (float)num2;
	}

	// Token: 0x06004416 RID: 17430 RVA: 0x001887F0 File Offset: 0x001869F0
	public static string MigrateFMOD(FMODAsset asset)
	{
		if (asset == null)
		{
			return null;
		}
		if (asset.path == null)
		{
			return asset.name;
		}
		return asset.path;
	}

	// Token: 0x06004417 RID: 17431 RVA: 0x00188812 File Offset: 0x00186A12
	private static void SortGameObjectDescriptors(List<IGameObjectEffectDescriptor> descriptorList)
	{
		descriptorList.Sort(delegate(IGameObjectEffectDescriptor e1, IGameObjectEffectDescriptor e2)
		{
			int num = TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e1.GetType());
			int value = TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e2.GetType());
			return num.CompareTo(value);
		});
	}

	// Token: 0x06004418 RID: 17432 RVA: 0x0018883C File Offset: 0x00186A3C
	public static void IndentListOfDescriptors(List<Descriptor> list, int indentCount = 1)
	{
		for (int i = 0; i < list.Count; i++)
		{
			Descriptor value = list[i];
			for (int j = 0; j < indentCount; j++)
			{
				value.IncreaseIndent();
			}
			list[i] = value;
		}
	}

	// Token: 0x06004419 RID: 17433 RVA: 0x00188880 File Offset: 0x00186A80
	public static List<Descriptor> GetAllDescriptors(GameObject go, bool simpleInfoScreen = false)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<IGameObjectEffectDescriptor> list2 = new List<IGameObjectEffectDescriptor>(go.GetComponents<IGameObjectEffectDescriptor>());
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			list2.AddRange(component.GetDescriptors());
		}
		GameUtil.SortGameObjectDescriptors(list2);
		foreach (IGameObjectEffectDescriptor gameObjectEffectDescriptor in list2)
		{
			List<Descriptor> descriptors = gameObjectEffectDescriptor.GetDescriptors(go);
			if (descriptors != null)
			{
				foreach (Descriptor descriptor in descriptors)
				{
					if (!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen)
					{
						list.Add(descriptor);
					}
				}
			}
		}
		KPrefabID component2 = go.GetComponent<KPrefabID>();
		if (component2 != null && component2.AdditionalRequirements != null)
		{
			foreach (Descriptor descriptor2 in component2.AdditionalRequirements)
			{
				if (!descriptor2.onlyForSimpleInfoScreen || simpleInfoScreen)
				{
					list.Add(descriptor2);
				}
			}
		}
		if (component2 != null && component2.AdditionalEffects != null)
		{
			foreach (Descriptor descriptor3 in component2.AdditionalEffects)
			{
				if (!descriptor3.onlyForSimpleInfoScreen || simpleInfoScreen)
				{
					list.Add(descriptor3);
				}
			}
		}
		return list;
	}

	// Token: 0x0600441A RID: 17434 RVA: 0x00188A20 File Offset: 0x00186C20
	public static List<Descriptor> GetDetailDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Detail)
			{
				list.Add(descriptor);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x0600441B RID: 17435 RVA: 0x00188A88 File Offset: 0x00186C88
	public static List<Descriptor> GetRequirementDescriptors(List<Descriptor> descriptors)
	{
		return GameUtil.GetRequirementDescriptors(descriptors, true);
	}

	// Token: 0x0600441C RID: 17436 RVA: 0x00188A94 File Offset: 0x00186C94
	public static List<Descriptor> GetRequirementDescriptors(List<Descriptor> descriptors, bool indent)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Requirement)
			{
				list.Add(descriptor);
			}
		}
		if (indent)
		{
			GameUtil.IndentListOfDescriptors(list, 1);
		}
		return list;
	}

	// Token: 0x0600441D RID: 17437 RVA: 0x00188AFC File Offset: 0x00186CFC
	public static List<Descriptor> GetEffectDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource)
			{
				list.Add(descriptor);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x0600441E RID: 17438 RVA: 0x00188B6C File Offset: 0x00186D6C
	public static List<Descriptor> GetInformationDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Lifecycle)
			{
				list.Add(descriptor);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x0600441F RID: 17439 RVA: 0x00188BD4 File Offset: 0x00186DD4
	public static List<Descriptor> GetCropOptimumConditionDescriptors(List<Descriptor> descriptors)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor descriptor in descriptors)
		{
			if (descriptor.type == Descriptor.DescriptorType.Lifecycle)
			{
				Descriptor descriptor2 = descriptor;
				descriptor2.text = "• " + descriptor2.text;
				list.Add(descriptor2);
			}
		}
		GameUtil.IndentListOfDescriptors(list, 1);
		return list;
	}

	// Token: 0x06004420 RID: 17440 RVA: 0x00188C54 File Offset: 0x00186E54
	public static List<Descriptor> GetGameObjectRequirements(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<IGameObjectEffectDescriptor> list2 = new List<IGameObjectEffectDescriptor>(go.GetComponents<IGameObjectEffectDescriptor>());
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			list2.AddRange(component.GetDescriptors());
		}
		GameUtil.SortGameObjectDescriptors(list2);
		foreach (IGameObjectEffectDescriptor gameObjectEffectDescriptor in list2)
		{
			List<Descriptor> descriptors = gameObjectEffectDescriptor.GetDescriptors(go);
			if (descriptors != null)
			{
				foreach (Descriptor descriptor in descriptors)
				{
					if (descriptor.type == Descriptor.DescriptorType.Requirement)
					{
						list.Add(descriptor);
					}
				}
			}
		}
		KPrefabID component2 = go.GetComponent<KPrefabID>();
		if (component2.AdditionalRequirements != null)
		{
			list.AddRange(component2.AdditionalRequirements);
		}
		return list;
	}

	// Token: 0x06004421 RID: 17441 RVA: 0x00188D44 File Offset: 0x00186F44
	public static List<Descriptor> GetGameObjectEffects(GameObject go, bool simpleInfoScreen = false)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<IGameObjectEffectDescriptor> list2 = new List<IGameObjectEffectDescriptor>(go.GetComponents<IGameObjectEffectDescriptor>());
		StateMachineController component = go.GetComponent<StateMachineController>();
		if (component != null)
		{
			list2.AddRange(component.GetDescriptors());
		}
		GameUtil.SortGameObjectDescriptors(list2);
		foreach (IGameObjectEffectDescriptor gameObjectEffectDescriptor in list2)
		{
			List<Descriptor> descriptors = gameObjectEffectDescriptor.GetDescriptors(go);
			if (descriptors != null)
			{
				foreach (Descriptor descriptor in descriptors)
				{
					if ((!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen) && (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource))
					{
						list.Add(descriptor);
					}
				}
			}
		}
		KPrefabID component2 = go.GetComponent<KPrefabID>();
		if (component2 != null && component2.AdditionalEffects != null)
		{
			foreach (Descriptor descriptor2 in component2.AdditionalEffects)
			{
				if (!descriptor2.onlyForSimpleInfoScreen || simpleInfoScreen)
				{
					list.Add(descriptor2);
				}
			}
		}
		return list;
	}

	// Token: 0x06004422 RID: 17442 RVA: 0x00188E98 File Offset: 0x00187098
	public static List<Descriptor> GetPlantRequirementDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(GameUtil.GetAllDescriptors(go, false));
		if (requirementDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTREQUIREMENTS, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTREQUIREMENTS, Descriptor.DescriptorType.Requirement);
			list.Add(item);
			list.AddRange(requirementDescriptors);
		}
		return list;
	}

	// Token: 0x06004423 RID: 17443 RVA: 0x00188EF4 File Offset: 0x001870F4
	public static List<Descriptor> GetPlantLifeCycleDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		List<Descriptor> informationDescriptors = GameUtil.GetInformationDescriptors(GameUtil.GetAllDescriptors(go, false));
		if (informationDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.LIFECYCLE, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTLIFECYCLE, Descriptor.DescriptorType.Lifecycle);
			list.Add(item);
			list.AddRange(informationDescriptors);
		}
		return list;
	}

	// Token: 0x06004424 RID: 17444 RVA: 0x00188F50 File Offset: 0x00187150
	public static List<Descriptor> GetPlantEffectDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (go.GetComponent<Growing>() == null)
		{
			return list;
		}
		List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(go, false);
		List<Descriptor> list2 = new List<Descriptor>();
		list2.AddRange(GameUtil.GetEffectDescriptors(allDescriptors));
		if (list2.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTEFFECTS, UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTEFFECTS, Descriptor.DescriptorType.Effect);
			list.Add(item);
			list.AddRange(list2);
		}
		return list;
	}

	// Token: 0x06004425 RID: 17445 RVA: 0x00188FCC File Offset: 0x001871CC
	public static string GetGameObjectEffectsTooltipString(GameObject go)
	{
		string text = "";
		List<Descriptor> gameObjectEffects = GameUtil.GetGameObjectEffects(go, false);
		if (gameObjectEffects.Count > 0)
		{
			text = text + UI.BUILDINGEFFECTS.OPERATIONEFFECTS + "\n";
		}
		foreach (Descriptor descriptor in gameObjectEffects)
		{
			text = text + descriptor.IndentedText() + "\n";
		}
		return text;
	}

	// Token: 0x06004426 RID: 17446 RVA: 0x00189054 File Offset: 0x00187254
	public static List<Descriptor> GetEquipmentEffects(EquipmentDef def)
	{
		global::Debug.Assert(def != null);
		List<Descriptor> list = new List<Descriptor>();
		List<AttributeModifier> attributeModifiers = def.AttributeModifiers;
		if (attributeModifiers != null)
		{
			foreach (AttributeModifier attributeModifier in attributeModifiers)
			{
				string name = Db.Get().Attributes.Get(attributeModifier.AttributeId).Name;
				string formattedString = attributeModifier.GetFormattedString();
				string newValue = (attributeModifier.Value >= 0f) ? "produced" : "consumed";
				string text = UI.GAMEOBJECTEFFECTS.EQUIPMENT_MODS.text.Replace("{Attribute}", name).Replace("{Style}", newValue).Replace("{Value}", formattedString);
				list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
			}
		}
		return list;
	}

	// Token: 0x06004427 RID: 17447 RVA: 0x00189144 File Offset: 0x00187344
	public static string GetRecipeDescription(Recipe recipe)
	{
		string text = null;
		if (recipe != null)
		{
			text = recipe.recipeDescription;
		}
		if (text == null)
		{
			text = RESEARCH.TYPES.MISSINGRECIPEDESC;
			global::Debug.LogWarning("Missing recipeDescription");
		}
		return text;
	}

	// Token: 0x06004428 RID: 17448 RVA: 0x00189176 File Offset: 0x00187376
	public static int GetCurrentCycle()
	{
		return GameClock.Instance.GetCycle() + 1;
	}

	// Token: 0x06004429 RID: 17449 RVA: 0x00189184 File Offset: 0x00187384
	public static float GetCurrentTimeInCycles()
	{
		return GameClock.Instance.GetTimeInCycles() + 1f;
	}

	// Token: 0x0600442A RID: 17450 RVA: 0x00189198 File Offset: 0x00187398
	public static GameObject GetActiveTelepad()
	{
		GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.activeWorldId);
		if (telepad == null)
		{
			telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
		}
		return telepad;
	}

	// Token: 0x0600442B RID: 17451 RVA: 0x001891D4 File Offset: 0x001873D4
	public static GameObject GetTelepad(int worldId)
	{
		if (Components.Telepads.Count > 0)
		{
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				if (Components.Telepads[i].GetMyWorldId() == worldId)
				{
					return Components.Telepads[i].gameObject;
				}
			}
		}
		return null;
	}

	// Token: 0x0600442C RID: 17452 RVA: 0x00189228 File Offset: 0x00187428
	public static GameObject KInstantiate(GameObject original, Vector3 position, Grid.SceneLayer sceneLayer, string name = null, int gameLayer = 0)
	{
		return GameUtil.KInstantiate(original, position, sceneLayer, null, name, gameLayer);
	}

	// Token: 0x0600442D RID: 17453 RVA: 0x00189236 File Offset: 0x00187436
	public static GameObject KInstantiate(GameObject original, Vector3 position, Grid.SceneLayer sceneLayer, GameObject parent, string name = null, int gameLayer = 0)
	{
		position.z = Grid.GetLayerZ(sceneLayer);
		return Util.KInstantiate(original, position, Quaternion.identity, parent, name, true, gameLayer);
	}

	// Token: 0x0600442E RID: 17454 RVA: 0x00189257 File Offset: 0x00187457
	public static GameObject KInstantiate(GameObject original, Grid.SceneLayer sceneLayer, string name = null, int gameLayer = 0)
	{
		return GameUtil.KInstantiate(original, Vector3.zero, sceneLayer, name, gameLayer);
	}

	// Token: 0x0600442F RID: 17455 RVA: 0x00189267 File Offset: 0x00187467
	public static GameObject KInstantiate(Component original, Grid.SceneLayer sceneLayer, string name = null, int gameLayer = 0)
	{
		return GameUtil.KInstantiate(original.gameObject, Vector3.zero, sceneLayer, name, gameLayer);
	}

	// Token: 0x06004430 RID: 17456 RVA: 0x0018927C File Offset: 0x0018747C
	public unsafe static void IsEmissionBlocked(int cell, out bool all_not_gaseous, out bool all_over_pressure)
	{
		int* ptr = stackalloc int[(UIntPtr)16];
		*ptr = Grid.CellBelow(cell);
		ptr[1] = Grid.CellLeft(cell);
		ptr[2] = Grid.CellRight(cell);
		ptr[3] = Grid.CellAbove(cell);
		all_not_gaseous = true;
		all_over_pressure = true;
		for (int i = 0; i < 4; i++)
		{
			int num = ptr[i];
			if (Grid.IsValidCell(num))
			{
				Element element = Grid.Element[num];
				all_not_gaseous = (all_not_gaseous && !element.IsGas && !element.IsVacuum);
				all_over_pressure = (all_over_pressure && ((!element.IsGas && !element.IsVacuum) || Grid.Mass[num] >= 1.8f));
			}
		}
	}

	// Token: 0x06004431 RID: 17457 RVA: 0x00189333 File Offset: 0x00187533
	public static float GetDecorAtCell(int cell)
	{
		return GameUtil.GetDecorAtCell(cell, true);
	}

	// Token: 0x06004432 RID: 17458 RVA: 0x0018933C File Offset: 0x0018753C
	public static float GetDecorAtCell(int cell, bool includeLightDecor)
	{
		float num = 0f;
		if (!Grid.Solid[cell])
		{
			num = Grid.Decor[cell];
			if (includeLightDecor)
			{
				num += (float)DecorProvider.GetLightDecorBonus(cell);
			}
		}
		return num;
	}

	// Token: 0x06004433 RID: 17459 RVA: 0x00189374 File Offset: 0x00187574
	public static string GetUnitTypeMassOrUnit(GameObject go)
	{
		string result = UI.UNITSUFFIXES.UNITS;
		KPrefabID component = go.GetComponent<KPrefabID>();
		if (component != null)
		{
			result = (component.Tags.Contains(GameTags.Seed) ? UI.UNITSUFFIXES.UNITS : UI.UNITSUFFIXES.MASS.KILOGRAM);
		}
		return result;
	}

	// Token: 0x06004434 RID: 17460 RVA: 0x001893C4 File Offset: 0x001875C4
	public static string GetKeywordStyle(Tag tag)
	{
		Element element = ElementLoader.GetElement(tag);
		string result;
		if (element != null)
		{
			result = GameUtil.GetKeywordStyle(element);
		}
		else if (GameUtil.foodTags.Contains(tag))
		{
			result = "food";
		}
		else if (GameUtil.solidTags.Contains(tag))
		{
			result = "solid";
		}
		else
		{
			result = null;
		}
		return result;
	}

	// Token: 0x06004435 RID: 17461 RVA: 0x00189414 File Offset: 0x00187614
	public static string GetKeywordStyle(SimHashes hash)
	{
		Element element = ElementLoader.FindElementByHash(hash);
		if (element != null)
		{
			return GameUtil.GetKeywordStyle(element);
		}
		return null;
	}

	// Token: 0x06004436 RID: 17462 RVA: 0x00189434 File Offset: 0x00187634
	public static string GetKeywordStyle(Element element)
	{
		if (element.id == SimHashes.Oxygen)
		{
			return "oxygen";
		}
		if (element.IsSolid)
		{
			return "solid";
		}
		if (element.IsLiquid)
		{
			return "liquid";
		}
		if (element.IsGas)
		{
			return "gas";
		}
		if (element.IsVacuum)
		{
			return "vacuum";
		}
		return null;
	}

	// Token: 0x06004437 RID: 17463 RVA: 0x00189490 File Offset: 0x00187690
	public static string GetKeywordStyle(GameObject go)
	{
		string result = "";
		UnityEngine.Object component = go.GetComponent<Edible>();
		Equippable component2 = go.GetComponent<Equippable>();
		MedicinalPill component3 = go.GetComponent<MedicinalPill>();
		ResearchPointObject component4 = go.GetComponent<ResearchPointObject>();
		if (component != null)
		{
			result = "food";
		}
		else if (component2 != null)
		{
			result = "equipment";
		}
		else if (component3 != null)
		{
			result = "medicine";
		}
		else if (component4 != null)
		{
			result = "research";
		}
		return result;
	}

	// Token: 0x06004438 RID: 17464 RVA: 0x00189500 File Offset: 0x00187700
	public static Sprite GetBiomeSprite(string id)
	{
		string text = "biomeIcon" + char.ToUpper(id[0]).ToString() + id.Substring(1).ToLower();
		Sprite sprite = Assets.GetSprite(text);
		if (sprite != null)
		{
			return new global::Tuple<Sprite, Color>(sprite, Color.white).first;
		}
		global::Debug.LogWarning("Missing codex biome icon: " + text);
		return null;
	}

	// Token: 0x06004439 RID: 17465 RVA: 0x00189570 File Offset: 0x00187770
	public static string GenerateRandomDuplicantName()
	{
		string text = "";
		string text2 = "";
		bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
		List<string> list = new List<string>(LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.NAME.NB)));
		list.AddRange(flag ? LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.NAME.MALE)) : LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.NAME.FEMALE)));
		string random = list.GetRandom<string>();
		if (UnityEngine.Random.Range(0f, 1f) > 0.7f)
		{
			List<string> list2 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.PREFIX.NB)));
			list2.AddRange(flag ? LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.PREFIX.MALE)) : LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.PREFIX.FEMALE)));
			text = list2.GetRandom<string>();
		}
		if (!string.IsNullOrEmpty(text))
		{
			text += " ";
		}
		if (UnityEngine.Random.Range(0f, 1f) >= 0.9f)
		{
			List<string> list3 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.SUFFIX.NB)));
			list3.AddRange(flag ? LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.SUFFIX.MALE)) : LocString.GetStrings(typeof(NAMEGEN.DUPLICANT.SUFFIX.FEMALE)));
			text2 = list3.GetRandom<string>();
		}
		if (!string.IsNullOrEmpty(text2))
		{
			text2 = " " + text2;
		}
		return text + random + text2;
	}

	// Token: 0x0600443A RID: 17466 RVA: 0x001896D8 File Offset: 0x001878D8
	public static string GenerateRandomLaunchPadName()
	{
		return NAMEGEN.LAUNCHPAD.FORMAT.Replace("{Name}", UnityEngine.Random.Range(1, 1000).ToString());
	}

	// Token: 0x0600443B RID: 17467 RVA: 0x00189708 File Offset: 0x00187908
	public static string GenerateRandomRocketName()
	{
		string newValue = "";
		string newValue2 = "";
		string newValue3 = "";
		int num = 1;
		int num2 = 2;
		int num3 = 4;
		string random = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.NOUN))).GetRandom<string>();
		int num4 = 0;
		if (UnityEngine.Random.value > 0.7f)
		{
			newValue = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.PREFIX))).GetRandom<string>();
			num4 |= num;
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			newValue2 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.ADJECTIVE))).GetRandom<string>();
			num4 |= num2;
		}
		if (UnityEngine.Random.value > 0.1f)
		{
			newValue3 = new List<string>(LocString.GetStrings(typeof(NAMEGEN.ROCKET.SUFFIX))).GetRandom<string>();
			num4 |= num3;
		}
		string text;
		if (num4 == (num | num2 | num3))
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN_SUFFIX;
		}
		else if (num4 == (num2 | num3))
		{
			text = NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN_SUFFIX;
		}
		else if (num4 == (num | num3))
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_NOUN_SUFFIX;
		}
		else if (num4 == num3)
		{
			text = NAMEGEN.ROCKET.FMT_NOUN_SUFFIX;
		}
		else if (num4 == (num | num2))
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN;
		}
		else if (num4 == num)
		{
			text = NAMEGEN.ROCKET.FMT_PREFIX_NOUN;
		}
		else if (num4 == num2)
		{
			text = NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN;
		}
		else
		{
			text = NAMEGEN.ROCKET.FMT_NOUN;
		}
		DebugUtil.LogArgs(new object[]
		{
			"Rocket name bits:",
			Convert.ToString(num4, 2)
		});
		return text.Replace("{Prefix}", newValue).Replace("{Adjective}", newValue2).Replace("{Noun}", random).Replace("{Suffix}", newValue3);
	}

	// Token: 0x0600443C RID: 17468 RVA: 0x001898D0 File Offset: 0x00187AD0
	public static string GenerateRandomWorldName(string[] nameTables)
	{
		if (nameTables == null)
		{
			global::Debug.LogWarning("No name tables provided to generate world name. Using GENERIC");
			nameTables = new string[]
			{
				"GENERIC"
			};
		}
		string text = "";
		foreach (string text2 in nameTables)
		{
			text += Strings.Get("STRINGS.NAMEGEN.WORLD.ROOTS." + text2.ToUpper());
		}
		string text3 = GameUtil.RandomValueFromSeparatedString(text, "\n");
		if (string.IsNullOrEmpty(text3))
		{
			text3 = GameUtil.RandomValueFromSeparatedString(Strings.Get(NAMEGEN.WORLD.ROOTS.GENERIC), "\n");
		}
		string str = GameUtil.RandomValueFromSeparatedString(NAMEGEN.WORLD.SUFFIXES.GENERICLIST, "\n");
		return text3 + str;
	}

	// Token: 0x0600443D RID: 17469 RVA: 0x0018998C File Offset: 0x00187B8C
	public static float GetThermalComfort(Tag duplicantType, int cell, float tolerance)
	{
		DUPLICANTSTATS statsFor = DUPLICANTSTATS.GetStatsFor(duplicantType);
		float num = 0f;
		Element element = ElementLoader.FindElementByHash(SimHashes.Creature);
		if (Grid.Element[cell].thermalConductivity != 0f)
		{
			num = SimUtil.CalculateEnergyFlowCreatures(cell, statsFor.Temperature.Internal.IDEAL, element.specificHeatCapacity, element.thermalConductivity, statsFor.Temperature.SURFACE_AREA, statsFor.Temperature.SKIN_THICKNESS + 0.0025f);
		}
		num -= tolerance;
		return num * 1000f;
	}

	// Token: 0x0600443E RID: 17470 RVA: 0x00189A14 File Offset: 0x00187C14
	public static void FocusCamera(Transform target, bool select = true, bool show_back_button = true)
	{
		GameUtil.FocusCamera(target.GetPosition(), 2f, true, show_back_button);
		if (select)
		{
			KSelectable component = target.GetComponent<KSelectable>();
			SelectTool.Instance.Select(component, false);
		}
	}

	// Token: 0x0600443F RID: 17471 RVA: 0x00189A49 File Offset: 0x00187C49
	public static void FocusCameraOnWorld(int worldID, Vector3 pos, float forceOrthgraphicSize = 10f, System.Action callback = null, bool show_back_button = true)
	{
		CameraController.Instance.ActiveWorldStarWipe(worldID, pos, forceOrthgraphicSize, callback);
		if (show_back_button && NotificationScreen_TemporaryActions.Instance != null)
		{
			NotificationScreen_TemporaryActions.Instance.CreateCameraReturnActionButton(CameraController.Instance.transform.position);
		}
	}

	// Token: 0x06004440 RID: 17472 RVA: 0x00189A83 File Offset: 0x00187C83
	public static void FocusCamera(int cell, bool show_back_button = true)
	{
		GameUtil.FocusCamera(Grid.CellToPos(cell), 2f, true, show_back_button);
	}

	// Token: 0x06004441 RID: 17473 RVA: 0x00189A97 File Offset: 0x00187C97
	public static void FocusCamera(Vector3 position, float speed = 2f, bool playSound = true, bool show_back_button = true)
	{
		CameraController.Instance.CameraGoTo(position, speed, playSound);
		if (show_back_button && NotificationScreen_TemporaryActions.Instance != null)
		{
			NotificationScreen_TemporaryActions.Instance.CreateCameraReturnActionButton(CameraController.Instance.transform.position);
		}
	}

	// Token: 0x06004442 RID: 17474 RVA: 0x00189AD0 File Offset: 0x00187CD0
	public static string RandomValueFromSeparatedString(string source, string separator = "\n")
	{
		int num = 0;
		int num2 = 0;
		for (;;)
		{
			num = source.IndexOf(separator, num);
			if (num == -1)
			{
				break;
			}
			num += separator.Length;
			num2++;
		}
		if (num2 == 0)
		{
			return "";
		}
		int num3 = UnityEngine.Random.Range(0, num2);
		num = 0;
		for (int i = 0; i < num3; i++)
		{
			num = source.IndexOf(separator, num) + separator.Length;
		}
		int num4 = source.IndexOf(separator, num);
		return source.Substring(num, (num4 == -1) ? (source.Length - num) : (num4 - num));
	}

	// Token: 0x06004443 RID: 17475 RVA: 0x00189B54 File Offset: 0x00187D54
	public static string GetFormattedDiseaseName(byte idx, bool color = false)
	{
		Disease disease = Db.Get().Diseases[(int)idx];
		if (color)
		{
			return string.Format(UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT, disease.Name, GameUtil.ColourToHex(GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName)));
		}
		return string.Format(UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT_NO_COLOR, disease.Name);
	}

	// Token: 0x06004444 RID: 17476 RVA: 0x00189BBC File Offset: 0x00187DBC
	public static string GetFormattedDisease(byte idx, int units, bool color = false)
	{
		if (idx == 255 || units <= 0)
		{
			return UI.OVERLAYS.DISEASE.NO_DISEASE;
		}
		Disease disease = Db.Get().Diseases[(int)idx];
		if (color)
		{
			return string.Format(UI.OVERLAYS.DISEASE.DISEASE_FORMAT, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None), GameUtil.ColourToHex(GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName)));
		}
		return string.Format(UI.OVERLAYS.DISEASE.DISEASE_FORMAT_NO_COLOR, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None));
	}

	// Token: 0x06004445 RID: 17477 RVA: 0x00189C47 File Offset: 0x00187E47
	public static string GetFormattedDiseaseAmount(int units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		GameUtil.ApplyTimeSlice(units, timeSlice);
		return GameUtil.AddTimeSliceText(units.ToString("#,##0") + UI.UNITSUFFIXES.DISEASE.UNITS, timeSlice);
	}

	// Token: 0x06004446 RID: 17478 RVA: 0x00189C72 File Offset: 0x00187E72
	public static string GetFormattedDiseaseAmount(long units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		GameUtil.ApplyTimeSlice((float)units, timeSlice);
		return GameUtil.AddTimeSliceText(units.ToString("#,##0") + UI.UNITSUFFIXES.DISEASE.UNITS, timeSlice);
	}

	// Token: 0x06004447 RID: 17479 RVA: 0x00189C9E File Offset: 0x00187E9E
	public static string ColourizeString(Color32 colour, string str)
	{
		return string.Format("<color=#{0}>{1}</color>", GameUtil.ColourToHex(colour), str);
	}

	// Token: 0x06004448 RID: 17480 RVA: 0x00189CB4 File Offset: 0x00187EB4
	public static string ColourToHex(Color32 colour)
	{
		return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[]
		{
			colour.r,
			colour.g,
			colour.b,
			colour.a
		});
	}

	// Token: 0x06004449 RID: 17481 RVA: 0x00189D0C File Offset: 0x00187F0C
	public static string GetFormattedDecor(float value, bool enforce_max = false)
	{
		string arg = "";
		LocString loc_string = (value > DecorMonitor.MAXIMUM_DECOR_VALUE && enforce_max) ? UI.OVERLAYS.DECOR.MAXIMUM_DECOR : UI.OVERLAYS.DECOR.VALUE;
		if (enforce_max)
		{
			value = Math.Min(value, DecorMonitor.MAXIMUM_DECOR_VALUE);
		}
		if (value > 0f)
		{
			arg = "+";
		}
		else if (value >= 0f)
		{
			loc_string = UI.OVERLAYS.DECOR.VALUE_ZERO;
		}
		return string.Format(loc_string, arg, value);
	}

	// Token: 0x0600444A RID: 17482 RVA: 0x00189D78 File Offset: 0x00187F78
	public static Color GetDecorColourFromValue(int decor)
	{
		Color result = Color.black;
		float num = (float)decor / 100f;
		if (num > 0f)
		{
			result = Color.Lerp(new Color(0.15f, 0f, 0f), new Color(0f, 1f, 0f), Mathf.Abs(num));
		}
		else
		{
			result = Color.Lerp(new Color(0.15f, 0f, 0f), new Color(1f, 0f, 0f), Mathf.Abs(num));
		}
		return result;
	}

	// Token: 0x0600444B RID: 17483 RVA: 0x00189E08 File Offset: 0x00188008
	public static List<Descriptor> GetMaterialDescriptors(Element element)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (element.attributeModifiers.Count > 0)
		{
			foreach (AttributeModifier attributeModifier in element.attributeModifiers)
			{
				string txt = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
				string tooltip = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
				Descriptor item = default(Descriptor);
				item.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
				item.IncreaseIndent();
				list.Add(item);
			}
		}
		list.AddRange(GameUtil.GetSignificantMaterialPropertyDescriptors(element));
		return list;
	}

	// Token: 0x0600444C RID: 17484 RVA: 0x00189F04 File Offset: 0x00188104
	public static string GetMaterialTooltips(Element element)
	{
		string text = element.tag.ProperName();
		foreach (AttributeModifier attributeModifier in element.attributeModifiers)
		{
			string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
			string formattedString = attributeModifier.GetFormattedString();
			text = text + "\n    • " + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name, formattedString);
		}
		text += GameUtil.GetSignificantMaterialPropertyTooltips(element);
		return text;
	}

	// Token: 0x0600444D RID: 17485 RVA: 0x00189FAC File Offset: 0x001881AC
	public static string GetSignificantMaterialPropertyTooltips(Element element)
	{
		string text = "";
		List<Descriptor> significantMaterialPropertyDescriptors = GameUtil.GetSignificantMaterialPropertyDescriptors(element);
		if (significantMaterialPropertyDescriptors.Count > 0)
		{
			text += "\n";
			for (int i = 0; i < significantMaterialPropertyDescriptors.Count; i++)
			{
				text = text + "    • " + Util.StripTextFormatting(significantMaterialPropertyDescriptors[i].text) + "\n";
			}
		}
		return text;
	}

	// Token: 0x0600444E RID: 17486 RVA: 0x0018A010 File Offset: 0x00188210
	public static List<Descriptor> GetSignificantMaterialPropertyDescriptors(Element element)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (element.thermalConductivity > 10f)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(ELEMENTS.MATERIAL_MODIFIERS.HIGH_THERMAL_CONDUCTIVITY, GameUtil.GetThermalConductivityString(element, false, false)), string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_THERMAL_CONDUCTIVITY, element.name, element.thermalConductivity.ToString("0.#####")), Descriptor.DescriptorType.Effect);
			item.IncreaseIndent();
			list.Add(item);
		}
		if (element.thermalConductivity < 1f)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(string.Format(ELEMENTS.MATERIAL_MODIFIERS.LOW_THERMAL_CONDUCTIVITY, GameUtil.GetThermalConductivityString(element, false, false)), string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_THERMAL_CONDUCTIVITY, element.name, element.thermalConductivity.ToString("0.#####")), Descriptor.DescriptorType.Effect);
			item2.IncreaseIndent();
			list.Add(item2);
		}
		if (element.specificHeatCapacity <= 0.2f)
		{
			Descriptor item3 = default(Descriptor);
			item3.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.LOW_SPECIFIC_HEAT_CAPACITY, string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_SPECIFIC_HEAT_CAPACITY, element.name, element.specificHeatCapacity * 1f), Descriptor.DescriptorType.Effect);
			item3.IncreaseIndent();
			list.Add(item3);
		}
		if (element.specificHeatCapacity >= 1f)
		{
			Descriptor item4 = default(Descriptor);
			item4.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.HIGH_SPECIFIC_HEAT_CAPACITY, string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_SPECIFIC_HEAT_CAPACITY, element.name, element.specificHeatCapacity * 1f), Descriptor.DescriptorType.Effect);
			item4.IncreaseIndent();
			list.Add(item4);
		}
		if (Sim.IsRadiationEnabled() && element.radiationAbsorptionFactor >= 0.8f)
		{
			Descriptor item5 = default(Descriptor);
			item5.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EXCELLENT_RADIATION_SHIELD, string.Format(ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EXCELLENT_RADIATION_SHIELD, element.name, element.radiationAbsorptionFactor), Descriptor.DescriptorType.Effect);
			item5.IncreaseIndent();
			list.Add(item5);
		}
		return list;
	}

	// Token: 0x0600444F RID: 17487 RVA: 0x0018A20B File Offset: 0x0018840B
	public static int NaturalBuildingCell(this KMonoBehaviour cmp)
	{
		return Grid.PosToCell(cmp.transform.GetPosition());
	}

	// Token: 0x06004450 RID: 17488 RVA: 0x0018A220 File Offset: 0x00188420
	public static List<Descriptor> GetMaterialDescriptors(Tag tag)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.GetElement(tag);
		if (element != null)
		{
			if (element.attributeModifiers.Count > 0)
			{
				foreach (AttributeModifier attributeModifier in element.attributeModifiers)
				{
					string txt = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
					string tooltip = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), attributeModifier.GetFormattedString());
					Descriptor item = default(Descriptor);
					item.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
					item.IncreaseIndent();
					list.Add(item);
				}
			}
			list.AddRange(GameUtil.GetSignificantMaterialPropertyDescriptors(element));
		}
		else
		{
			GameObject gameObject = Assets.TryGetPrefab(tag);
			if (gameObject != null)
			{
				PrefabAttributeModifiers component = gameObject.GetComponent<PrefabAttributeModifiers>();
				if (component != null)
				{
					foreach (AttributeModifier attributeModifier2 in component.descriptors)
					{
						string txt2 = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier2.AttributeId.ToUpper())), attributeModifier2.GetFormattedString());
						string tooltip2 = string.Format(Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier2.AttributeId.ToUpper())), attributeModifier2.GetFormattedString());
						Descriptor item2 = default(Descriptor);
						item2.SetupDescriptor(txt2, tooltip2, Descriptor.DescriptorType.Effect);
						item2.IncreaseIndent();
						list.Add(item2);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06004451 RID: 17489 RVA: 0x0018A428 File Offset: 0x00188628
	public static string GetMaterialTooltips(Tag tag)
	{
		string text = tag.ProperName();
		Element element = ElementLoader.GetElement(tag);
		if (element != null)
		{
			foreach (AttributeModifier attributeModifier in element.attributeModifiers)
			{
				string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
				string formattedString = attributeModifier.GetFormattedString();
				text = text + "\n    • " + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name, formattedString);
			}
			text += GameUtil.GetSignificantMaterialPropertyTooltips(element);
		}
		else
		{
			GameObject gameObject = Assets.TryGetPrefab(tag);
			if (gameObject != null)
			{
				PrefabAttributeModifiers component = gameObject.GetComponent<PrefabAttributeModifiers>();
				if (component != null)
				{
					foreach (AttributeModifier attributeModifier2 in component.descriptors)
					{
						string name2 = Db.Get().BuildingAttributes.Get(attributeModifier2.AttributeId).Name;
						string formattedString2 = attributeModifier2.GetFormattedString();
						text = text + "\n    • " + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name2, formattedString2);
					}
				}
			}
		}
		return text;
	}

	// Token: 0x06004452 RID: 17490 RVA: 0x0018A588 File Offset: 0x00188788
	public static bool AreChoresUIMergeable(Chore.Precondition.Context choreA, Chore.Precondition.Context choreB)
	{
		if (choreA.chore.target.isNull || choreB.chore.target.isNull)
		{
			return false;
		}
		ChoreType choreType = choreB.chore.choreType;
		ChoreType choreType2 = choreA.chore.choreType;
		return (choreA.chore.choreType == choreB.chore.choreType && choreA.chore.target.GetComponent<KPrefabID>().PrefabTag == choreB.chore.target.GetComponent<KPrefabID>().PrefabTag) || (choreA.chore.choreType == Db.Get().ChoreTypes.Dig && choreB.chore.choreType == Db.Get().ChoreTypes.Dig) || (choreA.chore.choreType == Db.Get().ChoreTypes.Relax && choreB.chore.choreType == Db.Get().ChoreTypes.Relax) || ((choreType2 == Db.Get().ChoreTypes.ReturnSuitIdle || choreType2 == Db.Get().ChoreTypes.ReturnSuitUrgent) && (choreType == Db.Get().ChoreTypes.ReturnSuitIdle || choreType == Db.Get().ChoreTypes.ReturnSuitUrgent)) || (choreA.chore.target.gameObject == choreB.chore.target.gameObject && choreA.chore.choreType == choreB.chore.choreType);
	}

	// Token: 0x06004453 RID: 17491 RVA: 0x0018A720 File Offset: 0x00188920
	public static string GetChoreName(Chore chore, object choreData)
	{
		string result = "";
		if (chore.choreType == Db.Get().ChoreTypes.Fetch || chore.choreType == Db.Get().ChoreTypes.MachineFetch || chore.choreType == Db.Get().ChoreTypes.FabricateFetch || chore.choreType == Db.Get().ChoreTypes.FetchCritical || chore.choreType == Db.Get().ChoreTypes.PowerFetch)
		{
			result = chore.GetReportName(chore.gameObject.GetProperName());
		}
		else if (chore.choreType == Db.Get().ChoreTypes.StorageFetch || chore.choreType == Db.Get().ChoreTypes.FoodFetch)
		{
			FetchChore fetchChore = chore as FetchChore;
			FetchAreaChore fetchAreaChore = chore as FetchAreaChore;
			if (fetchAreaChore != null)
			{
				GameObject getFetchTarget = fetchAreaChore.GetFetchTarget;
				KMonoBehaviour kmonoBehaviour = choreData as KMonoBehaviour;
				if (getFetchTarget != null)
				{
					result = chore.GetReportName(getFetchTarget.GetProperName());
				}
				else if (kmonoBehaviour != null)
				{
					result = chore.GetReportName(kmonoBehaviour.GetProperName());
				}
				else
				{
					result = chore.GetReportName(null);
				}
			}
			else if (fetchChore != null)
			{
				Pickupable fetchTarget = fetchChore.fetchTarget;
				KMonoBehaviour kmonoBehaviour2 = choreData as KMonoBehaviour;
				if (fetchTarget != null)
				{
					result = chore.GetReportName(fetchTarget.GetProperName());
				}
				else if (kmonoBehaviour2 != null)
				{
					result = chore.GetReportName(kmonoBehaviour2.GetProperName());
				}
				else
				{
					result = chore.GetReportName(null);
				}
			}
		}
		else
		{
			result = chore.GetReportName(null);
		}
		return result;
	}

	// Token: 0x06004454 RID: 17492 RVA: 0x0018A8A4 File Offset: 0x00188AA4
	public static string ChoreGroupsForChoreType(ChoreType choreType)
	{
		if (choreType.groups == null || choreType.groups.Length == 0)
		{
			return null;
		}
		string text = "";
		for (int i = 0; i < choreType.groups.Length; i++)
		{
			if (i != 0)
			{
				text += UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CHORE_GROUP_SEPARATOR;
			}
			text += choreType.groups[i].Name;
		}
		return text;
	}

	// Token: 0x06004455 RID: 17493 RVA: 0x0018A908 File Offset: 0x00188B08
	public static List<BuildingDef> GetBuildingsRequiringSkillPerk(string perkID)
	{
		return (from building in Assets.BuildingDefs
		where building.RequiredSkillPerkID == perkID
		select building).ToList<BuildingDef>();
	}

	// Token: 0x06004456 RID: 17494 RVA: 0x0018A940 File Offset: 0x00188B40
	public static string NamesOfBuildingsRequiringSkillPerk(string perkID)
	{
		List<string> list = (from building in GameUtil.GetBuildingsRequiringSkillPerk(perkID)
		select GameUtil.SafeStringFormat(UI.ROLES_SCREEN.PERKS.CAN_USE_BUILDING.DESCRIPTION, new object[]
		{
			building.Name
		})).ToList<string>();
		if (list == null || list.Count == 0)
		{
			return null;
		}
		return string.Join("\n", list);
	}

	// Token: 0x06004457 RID: 17495 RVA: 0x0018A998 File Offset: 0x00188B98
	public static string NamesOfBoostersWithSkillPerk(string perkID)
	{
		List<string> values = (from tag in BionicUpgradeComponentConfig.GetBoostersWithSkillPerk(perkID)
		select Strings.Get(string.Format("STRINGS.ITEMS.BIONIC_BOOSTERS.{0}.NAME", tag.ToString().ToUpper())).String).ToList<string>();
		return string.Join("\n", values);
	}

	// Token: 0x06004458 RID: 17496 RVA: 0x0018A9E0 File Offset: 0x00188BE0
	public static string NamesOfSkillsWithSkillPerk(string perkID)
	{
		List<string> list = (from match in Db.Get().Skills.resources
		where !match.deprecated && match.GivesPerk(perkID)
		select match.Name).ToList<string>();
		return string.Join("\n", list.ToArray());
	}

	// Token: 0x06004459 RID: 17497 RVA: 0x0018AA54 File Offset: 0x00188C54
	public static bool IsCapturingTimeLapse()
	{
		return Game.Instance != null && Game.Instance.timelapser != null && Game.Instance.timelapser.CapturingTimelapseScreenshot;
	}

	// Token: 0x0600445A RID: 17498 RVA: 0x0018AA88 File Offset: 0x00188C88
	public static ExposureType GetExposureTypeForDisease(Disease disease)
	{
		for (int i = 0; i < GERM_EXPOSURE.TYPES.Length; i++)
		{
			if (disease.id == GERM_EXPOSURE.TYPES[i].germ_id)
			{
				return GERM_EXPOSURE.TYPES[i];
			}
		}
		return null;
	}

	// Token: 0x0600445B RID: 17499 RVA: 0x0018AAD0 File Offset: 0x00188CD0
	public static Sickness GetSicknessForDisease(Disease disease)
	{
		int i = 0;
		while (i < GERM_EXPOSURE.TYPES.Length)
		{
			if (disease.id == GERM_EXPOSURE.TYPES[i].germ_id)
			{
				if (GERM_EXPOSURE.TYPES[i].sickness_id == null)
				{
					return null;
				}
				return Db.Get().Sicknesses.Get(GERM_EXPOSURE.TYPES[i].sickness_id);
			}
			else
			{
				i++;
			}
		}
		return null;
	}

	// Token: 0x0600445C RID: 17500 RVA: 0x0018AB3C File Offset: 0x00188D3C
	public static void SubscribeToTags<T>(T target, EventSystem.IntraObjectHandler<T> handler, bool triggerImmediately) where T : KMonoBehaviour
	{
		if (triggerImmediately)
		{
			Boxed<TagChangedEventData> boxed = Boxed<TagChangedEventData>.Get(new TagChangedEventData(Tag.Invalid, false));
			handler.Trigger(target.gameObject, boxed);
			Boxed<TagChangedEventData>.Release(boxed);
		}
		target.Subscribe<T>(-1582839653, handler);
	}

	// Token: 0x0600445D RID: 17501 RVA: 0x0018AB87 File Offset: 0x00188D87
	public static void UnsubscribeToTags<T>(T target, EventSystem.IntraObjectHandler<T> handler) where T : KMonoBehaviour
	{
		target.Unsubscribe<T>(-1582839653, handler, false);
	}

	// Token: 0x0600445E RID: 17502 RVA: 0x0018AB9B File Offset: 0x00188D9B
	public static EventSystem.IntraObjectHandler<T> CreateHasTagHandler<T>(Tag tag, Action<T, object> callback) where T : KMonoBehaviour
	{
		return new EventSystem.IntraObjectHandler<T>(delegate(T component, object data)
		{
			TagChangedEventData value = ((Boxed<TagChangedEventData>)data).value;
			if (value.tag == Tag.Invalid)
			{
				KPrefabID component2 = component.GetComponent<KPrefabID>();
				value = new TagChangedEventData(tag, component2.HasTag(tag));
			}
			if (value.tag == tag && value.added)
			{
				callback(component, data);
			}
		});
	}

	// Token: 0x0600445F RID: 17503 RVA: 0x0018ABC0 File Offset: 0x00188DC0
	public static void DestroyCell(int cell, CellElementEvent eventSource, bool deleteMinions = true)
	{
		List<GameObject> list;
		using (CollectionPool<List<GameObject>, GameObject>.Get(out list))
		{
			list.Add(Grid.Objects[cell, 2]);
			list.Add(Grid.Objects[cell, 1]);
			list.Add(Grid.Objects[cell, 9]);
			list.Add(Grid.Objects[cell, 5]);
			list.Add(Grid.Objects[cell, 12]);
			list.Add(Grid.Objects[cell, 15]);
			list.Add(Grid.Objects[cell, 16]);
			list.Add(Grid.Objects[cell, 19]);
			list.Add(Grid.Objects[cell, 20]);
			list.Add(Grid.Objects[cell, 23]);
			list.Add(Grid.Objects[cell, 26]);
			list.Add(Grid.Objects[cell, 29]);
			list.Add(Grid.Objects[cell, 27]);
			list.Add(Grid.Objects[cell, 31]);
			list.Add(Grid.Objects[cell, 30]);
			foreach (Comet comet in Components.Meteors.GetItems((int)Grid.WorldIdx[cell]))
			{
				if (!comet.IsNullOrDestroyed() && Grid.PosToCell(comet) == cell)
				{
					list.Add(comet.gameObject);
				}
			}
			foreach (GameObject gameObject in list)
			{
				if (gameObject != null)
				{
					Util.KDestroyGameObject(gameObject);
				}
			}
			GameUtil.ClearCell(cell, deleteMinions);
			FallingWater.instance.ClearParticles(cell);
			if (ElementLoader.elements[(int)Grid.ElementIdx[cell]].id == SimHashes.Void)
			{
				SimMessages.ReplaceElement(cell, SimHashes.Void, eventSource, 0f, 0f, byte.MaxValue, 0, -1);
			}
			else
			{
				SimMessages.ReplaceElement(cell, SimHashes.Vacuum, eventSource, 0f, 0f, byte.MaxValue, 0, -1);
			}
		}
	}

	// Token: 0x06004460 RID: 17504 RVA: 0x0018AE50 File Offset: 0x00189050
	public static void ClearCell(int cell, bool deleteMinions = true)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		List<ScenePartitionerEntry> list;
		using (CollectionPool<List<ScenePartitionerEntry>, ScenePartitionerEntry>.Get(out list))
		{
			GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, list);
			for (int i = 0; i < list.Count; i++)
			{
				Pickupable pickupable = list[i].obj as Pickupable;
				if (!(pickupable == null))
				{
					bool flag = pickupable.KPrefabID.HasTag(GameTags.BaseMinion);
					if (deleteMinions || !flag)
					{
						Util.KDestroyGameObject(pickupable.gameObject);
					}
				}
			}
		}
	}

	// Token: 0x04002DF3 RID: 11763
	public static GameUtil.TemperatureUnit temperatureUnit;

	// Token: 0x04002DF4 RID: 11764
	public static GameUtil.MassUnit massUnit;

	// Token: 0x04002DF5 RID: 11765
	private static string[] adjectives;

	// Token: 0x04002DF6 RID: 11766
	public static ThreadLocal<Queue<GameUtil.FloodFillInfo>> FloodFillNext = new ThreadLocal<Queue<GameUtil.FloodFillInfo>>(() => new Queue<GameUtil.FloodFillInfo>());

	// Token: 0x04002DF7 RID: 11767
	public static ThreadLocal<HashSet<int>> FloodFillVisited = new ThreadLocal<HashSet<int>>(() => new HashSet<int>());

	// Token: 0x04002DF8 RID: 11768
	public static ThreadLocal<List<int>> FloodFillNeighbors = new ThreadLocal<List<int>>(() => new List<int>(4)
	{
		-1,
		-1,
		-1,
		-1
	});

	// Token: 0x04002DF9 RID: 11769
	public static TagSet foodTags = new TagSet(new string[]
	{
		"BasicPlantFood",
		"MushBar",
		"ColdWheatSeed",
		"ColdWheatSeed",
		"SpiceNut",
		"PrickleFruit",
		"Meat",
		"Mushroom",
		"ColdWheat",
		GameTags.Compostable.Name
	});

	// Token: 0x04002DFA RID: 11770
	public static TagSet solidTags = new TagSet(new string[]
	{
		"Filter",
		"Coal",
		"BasicFabric",
		"SwampLilyFlower",
		"RefinedMetal"
	});

	// Token: 0x02001981 RID: 6529
	public enum UnitClass
	{
		// Token: 0x04007E51 RID: 32337
		SimpleFloat,
		// Token: 0x04007E52 RID: 32338
		SimpleInteger,
		// Token: 0x04007E53 RID: 32339
		Temperature,
		// Token: 0x04007E54 RID: 32340
		Mass,
		// Token: 0x04007E55 RID: 32341
		Calories,
		// Token: 0x04007E56 RID: 32342
		Percent,
		// Token: 0x04007E57 RID: 32343
		Distance,
		// Token: 0x04007E58 RID: 32344
		Disease,
		// Token: 0x04007E59 RID: 32345
		Radiation,
		// Token: 0x04007E5A RID: 32346
		Energy,
		// Token: 0x04007E5B RID: 32347
		Power,
		// Token: 0x04007E5C RID: 32348
		Lux,
		// Token: 0x04007E5D RID: 32349
		Time,
		// Token: 0x04007E5E RID: 32350
		Seconds,
		// Token: 0x04007E5F RID: 32351
		Cycles
	}

	// Token: 0x02001982 RID: 6530
	public enum TemperatureUnit
	{
		// Token: 0x04007E61 RID: 32353
		Celsius,
		// Token: 0x04007E62 RID: 32354
		Fahrenheit,
		// Token: 0x04007E63 RID: 32355
		Kelvin
	}

	// Token: 0x02001983 RID: 6531
	public enum MassUnit
	{
		// Token: 0x04007E65 RID: 32357
		Kilograms,
		// Token: 0x04007E66 RID: 32358
		Pounds
	}

	// Token: 0x02001984 RID: 6532
	public enum MetricMassFormat
	{
		// Token: 0x04007E68 RID: 32360
		UseThreshold,
		// Token: 0x04007E69 RID: 32361
		Kilogram,
		// Token: 0x04007E6A RID: 32362
		Gram,
		// Token: 0x04007E6B RID: 32363
		Tonne
	}

	// Token: 0x02001985 RID: 6533
	public enum TemperatureInterpretation
	{
		// Token: 0x04007E6D RID: 32365
		Absolute,
		// Token: 0x04007E6E RID: 32366
		Relative
	}

	// Token: 0x02001986 RID: 6534
	public enum TimeSlice
	{
		// Token: 0x04007E70 RID: 32368
		None,
		// Token: 0x04007E71 RID: 32369
		ModifyOnly,
		// Token: 0x04007E72 RID: 32370
		PerSecond,
		// Token: 0x04007E73 RID: 32371
		PerCycle
	}

	// Token: 0x02001987 RID: 6535
	public enum MeasureUnit
	{
		// Token: 0x04007E75 RID: 32373
		mass,
		// Token: 0x04007E76 RID: 32374
		kcal,
		// Token: 0x04007E77 RID: 32375
		quantity
	}

	// Token: 0x02001988 RID: 6536
	public enum IdentityDescriptorTense
	{
		// Token: 0x04007E79 RID: 32377
		Normal,
		// Token: 0x04007E7A RID: 32378
		Possessive,
		// Token: 0x04007E7B RID: 32379
		Plural
	}

	// Token: 0x02001989 RID: 6537
	public enum WattageFormatterUnit
	{
		// Token: 0x04007E7D RID: 32381
		Watts,
		// Token: 0x04007E7E RID: 32382
		Kilowatts,
		// Token: 0x04007E7F RID: 32383
		Automatic
	}

	// Token: 0x0200198A RID: 6538
	public enum HeatEnergyFormatterUnit
	{
		// Token: 0x04007E81 RID: 32385
		DTU_S,
		// Token: 0x04007E82 RID: 32386
		KDTU_S,
		// Token: 0x04007E83 RID: 32387
		Automatic
	}

	// Token: 0x0200198B RID: 6539
	public struct FloodFillInfo
	{
		// Token: 0x04007E84 RID: 32388
		public int cell;

		// Token: 0x04007E85 RID: 32389
		public int depth;
	}

	// Token: 0x0200198C RID: 6540
	public static class Hardness
	{
		// Token: 0x04007E86 RID: 32390
		public const int VERY_SOFT = 0;

		// Token: 0x04007E87 RID: 32391
		public const int SOFT = 10;

		// Token: 0x04007E88 RID: 32392
		public const int FIRM = 25;

		// Token: 0x04007E89 RID: 32393
		public const int VERY_FIRM = 50;

		// Token: 0x04007E8A RID: 32394
		public const int NEARLY_IMPENETRABLE = 150;

		// Token: 0x04007E8B RID: 32395
		public const int SUPER_DUPER_HARD = 200;

		// Token: 0x04007E8C RID: 32396
		public const int RADIOACTIVE_MATERIALS = 251;

		// Token: 0x04007E8D RID: 32397
		public const int IMPENETRABLE = 255;

		// Token: 0x04007E8E RID: 32398
		public static Color ImpenetrableColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);

		// Token: 0x04007E8F RID: 32399
		public static Color nearlyImpenetrableColor = new Color(0.7411765f, 0.34901962f, 0.49803922f);

		// Token: 0x04007E90 RID: 32400
		public static Color veryFirmColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x04007E91 RID: 32401
		public static Color firmColor = new Color(0.5254902f, 0.41960785f, 0.64705884f);

		// Token: 0x04007E92 RID: 32402
		public static Color softColor = new Color(0.42745098f, 0.48235294f, 0.75686276f);

		// Token: 0x04007E93 RID: 32403
		public static Color verySoftColor = new Color(0.44313726f, 0.67058825f, 0.8117647f);
	}

	// Token: 0x0200198D RID: 6541
	public static class GermResistanceValues
	{
		// Token: 0x04007E94 RID: 32404
		public const float MEDIUM = 2f;

		// Token: 0x04007E95 RID: 32405
		public const float LARGE = 5f;

		// Token: 0x04007E96 RID: 32406
		public static Color NegativeLargeColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);

		// Token: 0x04007E97 RID: 32407
		public static Color NegativeMediumColor = new Color(0.7411765f, 0.34901962f, 0.49803922f);

		// Token: 0x04007E98 RID: 32408
		public static Color NegativeSmallColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x04007E99 RID: 32409
		public static Color PositiveSmallColor = new Color(0.5254902f, 0.41960785f, 0.64705884f);

		// Token: 0x04007E9A RID: 32410
		public static Color PositiveMediumColor = new Color(0.42745098f, 0.48235294f, 0.75686276f);

		// Token: 0x04007E9B RID: 32411
		public static Color PositiveLargeColor = new Color(0.44313726f, 0.67058825f, 0.8117647f);
	}

	// Token: 0x0200198E RID: 6542
	public static class ThermalConductivityValues
	{
		// Token: 0x04007E9C RID: 32412
		public const float VERY_HIGH = 50f;

		// Token: 0x04007E9D RID: 32413
		public const float HIGH = 10f;

		// Token: 0x04007E9E RID: 32414
		public const float MEDIUM = 2f;

		// Token: 0x04007E9F RID: 32415
		public const float LOW = 1f;

		// Token: 0x04007EA0 RID: 32416
		public static Color veryLowConductivityColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);

		// Token: 0x04007EA1 RID: 32417
		public static Color lowConductivityColor = new Color(0.7411765f, 0.34901962f, 0.49803922f);

		// Token: 0x04007EA2 RID: 32418
		public static Color mediumConductivityColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x04007EA3 RID: 32419
		public static Color highConductivityColor = new Color(0.5254902f, 0.41960785f, 0.64705884f);

		// Token: 0x04007EA4 RID: 32420
		public static Color veryHighConductivityColor = new Color(0.42745098f, 0.48235294f, 0.75686276f);
	}

	// Token: 0x0200198F RID: 6543
	public static class BreathableValues
	{
		// Token: 0x04007EA5 RID: 32421
		public static Color positiveColor = new Color(0.44313726f, 0.67058825f, 0.8117647f);

		// Token: 0x04007EA6 RID: 32422
		public static Color warningColor = new Color(0.6392157f, 0.39215687f, 0.6039216f);

		// Token: 0x04007EA7 RID: 32423
		public static Color negativeColor = new Color(0.83137256f, 0.28627452f, 0.28235295f);
	}

	// Token: 0x02001990 RID: 6544
	public static class WireLoadValues
	{
		// Token: 0x04007EA8 RID: 32424
		public static Color warningColor = new Color(0.9843137f, 0.6901961f, 0.23137255f);

		// Token: 0x04007EA9 RID: 32425
		public static Color negativeColor = new Color(1f, 0.19215687f, 0.19215687f);
	}
}
