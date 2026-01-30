using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000B0C RID: 2828
public class RetiredColonyData
{
	// Token: 0x06005257 RID: 21079 RVA: 0x001DE942 File Offset: 0x001DCB42
	public RetiredColonyData()
	{
	}

	// Token: 0x06005258 RID: 21080 RVA: 0x001DE94C File Offset: 0x001DCB4C
	public RetiredColonyData(string colonyName, int cycleCount, string date, string[] achievements, MinionAssignablesProxy[] minions, BuildingComplete[] buildingCompletes, string startWorld, Dictionary<string, string> worldIdentities)
	{
		this.colonyName = colonyName;
		this.cycleCount = cycleCount;
		this.achievements = achievements;
		this.date = date;
		this.Duplicants = new RetiredColonyData.RetiredDuplicantData[(minions == null) ? 0 : minions.Length];
		int i = 0;
		while (i < this.Duplicants.Length)
		{
			this.Duplicants[i] = new RetiredColonyData.RetiredDuplicantData();
			this.Duplicants[i].name = minions[i].GetProperName();
			this.Duplicants[i].age = (int)Mathf.Floor((float)GameClock.Instance.GetCycle() - minions[i].GetArrivalTime());
			this.Duplicants[i].skillPointsGained = minions[i].GetTotalSkillpoints();
			this.Duplicants[i].accessories = new Dictionary<string, string>();
			if (minions[i].GetTargetGameObject().GetComponent<Accessorizer>() != null)
			{
				using (List<ResourceRef<Accessory>>.Enumerator enumerator = minions[i].GetTargetGameObject().GetComponent<Accessorizer>().GetAccessories().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ResourceRef<Accessory> resourceRef = enumerator.Current;
						if (resourceRef.Get() != null)
						{
							this.Duplicants[i].accessories.Add(resourceRef.Get().slot.Id, resourceRef.Get().Id);
						}
					}
					goto IL_3AF;
				}
				goto IL_14E;
			}
			goto IL_14E;
			IL_3AF:
			i++;
			continue;
			IL_14E:
			StoredMinionIdentity component = minions[i].GetTargetGameObject().GetComponent<StoredMinionIdentity>();
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Eyes.Id, Db.Get().Accessories.Get(component.bodyData.eyes).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Arm.Id, Db.Get().Accessories.Get(component.bodyData.arms).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.ArmLower.Id, Db.Get().Accessories.Get(component.bodyData.armslower).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Body.Id, Db.Get().Accessories.Get(component.bodyData.body).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Hair.Id, Db.Get().Accessories.Get(component.bodyData.hair).Id);
			if (component.bodyData.hat != HashedString.Invalid)
			{
				this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Hat.Id, Db.Get().Accessories.Get(component.bodyData.hat).Id);
			}
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.HeadShape.Id, Db.Get().Accessories.Get(component.bodyData.headShape).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Mouth.Id, Db.Get().Accessories.Get(component.bodyData.mouth).Id);
			goto IL_3AF;
		}
		Dictionary<Tag, int> dictionary = new Dictionary<Tag, int>();
		if (buildingCompletes != null)
		{
			foreach (BuildingComplete cmp in buildingCompletes)
			{
				if (!dictionary.ContainsKey(cmp.PrefabID()))
				{
					dictionary[cmp.PrefabID()] = 0;
				}
				Dictionary<Tag, int> dictionary2 = dictionary;
				Tag key = cmp.PrefabID();
				int num = dictionary2[key];
				dictionary2[key] = num + 1;
			}
		}
		this.buildings = new List<global::Tuple<string, int>>();
		foreach (KeyValuePair<Tag, int> keyValuePair in dictionary)
		{
			this.buildings.Add(new global::Tuple<string, int>(keyValuePair.Key.ToString(), keyValuePair.Value));
		}
		this.Stats = null;
		if (ReportManager.Instance != null)
		{
			global::Tuple<float, float>[] array = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[k].day, ReportManager.Instance.reports[k].GetEntry(ReportManager.ReportType.OxygenCreated).accPositive);
			}
			global::Tuple<float, float>[] array2 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int l = 0; l < array2.Length; l++)
			{
				array2[l] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[l].day, ReportManager.Instance.reports[l].GetEntry(ReportManager.ReportType.OxygenCreated).accNegative * -1f);
			}
			global::Tuple<float, float>[] array3 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int m = 0; m < array3.Length; m++)
			{
				array3[m] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[m].day, ReportManager.Instance.reports[m].GetEntry(ReportManager.ReportType.CaloriesCreated).accPositive * 0.001f);
			}
			global::Tuple<float, float>[] array4 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int n = 0; n < array4.Length; n++)
			{
				array4[n] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[n].day, ReportManager.Instance.reports[n].GetEntry(ReportManager.ReportType.CaloriesCreated).accNegative * 0.001f * -1f);
			}
			global::Tuple<float, float>[] array5 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num2 = 0; num2 < array5.Length; num2++)
			{
				array5[num2] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num2].day, ReportManager.Instance.reports[num2].GetEntry(ReportManager.ReportType.EnergyCreated).accPositive * 0.001f);
			}
			global::Tuple<float, float>[] array6 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num3 = 0; num3 < array6.Length; num3++)
			{
				array6[num3] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num3].day, ReportManager.Instance.reports[num3].GetEntry(ReportManager.ReportType.EnergyWasted).accNegative * -1f * 0.001f);
			}
			global::Tuple<float, float>[] array7 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num4 = 0; num4 < array7.Length; num4++)
			{
				array7[num4] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num4].day, ReportManager.Instance.reports[num4].GetEntry(ReportManager.ReportType.WorkTime).accPositive);
			}
			global::Tuple<float, float>[] array8 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num5 = 0; num5 < array7.Length; num5++)
			{
				int num6 = 0;
				float num7 = 0f;
				ReportManager.ReportEntry entry = ReportManager.Instance.reports[num5].GetEntry(ReportManager.ReportType.WorkTime);
				for (int num8 = 0; num8 < entry.contextEntries.Count; num8++)
				{
					num6++;
					num7 += entry.contextEntries[num8].accPositive;
				}
				num7 /= (float)num6;
				num7 /= 600f;
				num7 *= 100f;
				array8[num5] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num5].day, num7);
			}
			global::Tuple<float, float>[] array9 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num9 = 0; num9 < array9.Length; num9++)
			{
				array9[num9] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num9].day, ReportManager.Instance.reports[num9].GetEntry(ReportManager.ReportType.TravelTime).accPositive);
			}
			global::Tuple<float, float>[] array10 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num10 = 0; num10 < array9.Length; num10++)
			{
				int num11 = 0;
				float num12 = 0f;
				ReportManager.ReportEntry entry2 = ReportManager.Instance.reports[num10].GetEntry(ReportManager.ReportType.TravelTime);
				for (int num13 = 0; num13 < entry2.contextEntries.Count; num13++)
				{
					num11++;
					num12 += entry2.contextEntries[num13].accPositive;
				}
				num12 /= (float)num11;
				num12 /= 600f;
				num12 *= 100f;
				array10[num10] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num10].day, num12);
			}
			global::Tuple<float, float>[] array11 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num14 = 0; num14 < array7.Length; num14++)
			{
				array11[num14] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num14].day, (float)ReportManager.Instance.reports[num14].GetEntry(ReportManager.ReportType.WorkTime).contextEntries.Count);
			}
			global::Tuple<float, float>[] array12 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num15 = 0; num15 < array12.Length; num15++)
			{
				int num16 = 0;
				float num17 = 0f;
				ReportManager.ReportEntry entry3 = ReportManager.Instance.reports[num15].GetEntry(ReportManager.ReportType.StressDelta);
				for (int num18 = 0; num18 < entry3.contextEntries.Count; num18++)
				{
					num16++;
					num17 += entry3.contextEntries[num18].accPositive;
				}
				array12[num15] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num15].day, num17 / (float)num16);
			}
			global::Tuple<float, float>[] array13 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num19 = 0; num19 < array13.Length; num19++)
			{
				int num20 = 0;
				float num21 = 0f;
				ReportManager.ReportEntry entry4 = ReportManager.Instance.reports[num19].GetEntry(ReportManager.ReportType.StressDelta);
				for (int num22 = 0; num22 < entry4.contextEntries.Count; num22++)
				{
					num20++;
					num21 += entry4.contextEntries[num22].accNegative;
				}
				num21 *= -1f;
				array13[num19] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num19].day, num21 / (float)num20);
			}
			global::Tuple<float, float>[] array14 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num23 = 0; num23 < array14.Length; num23++)
			{
				array14[num23] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num23].day, ReportManager.Instance.reports[num23].GetEntry(ReportManager.ReportType.DomesticatedCritters).accPositive);
			}
			global::Tuple<float, float>[] array15 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num24 = 0; num24 < array15.Length; num24++)
			{
				array15[num24] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num24].day, ReportManager.Instance.reports[num24].GetEntry(ReportManager.ReportType.WildCritters).accPositive);
			}
			global::Tuple<float, float>[] array16 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num25 = 0; num25 < array16.Length; num25++)
			{
				array16[num25] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num25].day, ReportManager.Instance.reports[num25].GetEntry(ReportManager.ReportType.RocketsInFlight).accPositive);
			}
			this.Stats = new RetiredColonyData.RetiredColonyStatistic[]
			{
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.OxygenProduced, array, UI.RETIRED_COLONY_INFO_SCREEN.STATS.OXYGEN_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.MASS.KILOGRAM),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.OxygenConsumed, array2, UI.RETIRED_COLONY_INFO_SCREEN.STATS.OXYGEN_CONSUMED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.MASS.KILOGRAM),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.CaloriesProduced, array3, UI.RETIRED_COLONY_INFO_SCREEN.STATS.CALORIES_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CALORIES.KILOCALORIE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.CaloriesRemoved, array4, UI.RETIRED_COLONY_INFO_SCREEN.STATS.CALORIES_CONSUMED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CALORIES.KILOCALORIE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.PowerProduced, array5, UI.RETIRED_COLONY_INFO_SCREEN.STATS.POWER_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.PowerWasted, array6, UI.RETIRED_COLONY_INFO_SCREEN.STATS.POWER_WASTED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.WorkTime, array7, UI.RETIRED_COLONY_INFO_SCREEN.STATS.WORK_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.SECONDS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageWorkTime, array8, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_WORK_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.TravelTime, array9, UI.RETIRED_COLONY_INFO_SCREEN.STATS.TRAVEL_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.SECONDS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageTravelTime, array10, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_TRAVEL_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.LiveDuplicants, array11, UI.RETIRED_COLONY_INFO_SCREEN.STATS.LIVE_DUPLICANTS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.DUPLICANTS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.RocketsInFlight, array16, UI.RETIRED_COLONY_INFO_SCREEN.STATS.ROCKET_MISSIONS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ROCKET_MISSIONS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageStressCreated, array12, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_STRESS_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageStressRemoved, array13, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_STRESS_REMOVED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.DomesticatedCritters, array14, UI.RETIRED_COLONY_INFO_SCREEN.STATS.NUMBER_DOMESTICATED_CRITTERS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CRITTERS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.WildCritters, array15, UI.RETIRED_COLONY_INFO_SCREEN.STATS.NUMBER_WILD_CRITTERS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CRITTERS)
			};
			this.startWorld = startWorld;
			this.worldIdentities = worldIdentities;
		}
	}

	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x06005259 RID: 21081 RVA: 0x001DF934 File Offset: 0x001DDB34
	// (set) Token: 0x0600525A RID: 21082 RVA: 0x001DF93C File Offset: 0x001DDB3C
	public string colonyName { get; set; }

	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x0600525B RID: 21083 RVA: 0x001DF945 File Offset: 0x001DDB45
	// (set) Token: 0x0600525C RID: 21084 RVA: 0x001DF94D File Offset: 0x001DDB4D
	public int cycleCount { get; set; }

	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x0600525D RID: 21085 RVA: 0x001DF956 File Offset: 0x001DDB56
	// (set) Token: 0x0600525E RID: 21086 RVA: 0x001DF95E File Offset: 0x001DDB5E
	public string date { get; set; }

	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x0600525F RID: 21087 RVA: 0x001DF967 File Offset: 0x001DDB67
	// (set) Token: 0x06005260 RID: 21088 RVA: 0x001DF96F File Offset: 0x001DDB6F
	public string[] achievements { get; set; }

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x06005261 RID: 21089 RVA: 0x001DF978 File Offset: 0x001DDB78
	// (set) Token: 0x06005262 RID: 21090 RVA: 0x001DF980 File Offset: 0x001DDB80
	public RetiredColonyData.RetiredDuplicantData[] Duplicants { get; set; }

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x06005263 RID: 21091 RVA: 0x001DF989 File Offset: 0x001DDB89
	// (set) Token: 0x06005264 RID: 21092 RVA: 0x001DF991 File Offset: 0x001DDB91
	public List<global::Tuple<string, int>> buildings { get; set; }

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x06005265 RID: 21093 RVA: 0x001DF99A File Offset: 0x001DDB9A
	// (set) Token: 0x06005266 RID: 21094 RVA: 0x001DF9A2 File Offset: 0x001DDBA2
	public RetiredColonyData.RetiredColonyStatistic[] Stats { get; set; }

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x06005267 RID: 21095 RVA: 0x001DF9AB File Offset: 0x001DDBAB
	// (set) Token: 0x06005268 RID: 21096 RVA: 0x001DF9B3 File Offset: 0x001DDBB3
	public Dictionary<string, string> worldIdentities { get; set; }

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x06005269 RID: 21097 RVA: 0x001DF9BC File Offset: 0x001DDBBC
	// (set) Token: 0x0600526A RID: 21098 RVA: 0x001DF9C4 File Offset: 0x001DDBC4
	public string startWorld { get; set; }

	// Token: 0x02001C3E RID: 7230
	public static class DataIDs
	{
		// Token: 0x04008758 RID: 34648
		public static string OxygenProduced = "oxygenProduced";

		// Token: 0x04008759 RID: 34649
		public static string OxygenConsumed = "oxygenConsumed";

		// Token: 0x0400875A RID: 34650
		public static string CaloriesProduced = "caloriesProduced";

		// Token: 0x0400875B RID: 34651
		public static string CaloriesRemoved = "caloriesRemoved";

		// Token: 0x0400875C RID: 34652
		public static string PowerProduced = "powerProduced";

		// Token: 0x0400875D RID: 34653
		public static string PowerWasted = "powerWasted";

		// Token: 0x0400875E RID: 34654
		public static string WorkTime = "workTime";

		// Token: 0x0400875F RID: 34655
		public static string TravelTime = "travelTime";

		// Token: 0x04008760 RID: 34656
		public static string AverageWorkTime = "averageWorkTime";

		// Token: 0x04008761 RID: 34657
		public static string AverageTravelTime = "averageTravelTime";

		// Token: 0x04008762 RID: 34658
		public static string LiveDuplicants = "liveDuplicants";

		// Token: 0x04008763 RID: 34659
		public static string AverageStressCreated = "averageStressCreated";

		// Token: 0x04008764 RID: 34660
		public static string AverageStressRemoved = "averageStressRemoved";

		// Token: 0x04008765 RID: 34661
		public static string DomesticatedCritters = "domesticatedCritters";

		// Token: 0x04008766 RID: 34662
		public static string WildCritters = "wildCritters";

		// Token: 0x04008767 RID: 34663
		public static string AverageGerms = "averageGerms";

		// Token: 0x04008768 RID: 34664
		public static string RocketsInFlight = "rocketsInFlight";
	}

	// Token: 0x02001C3F RID: 7231
	public class RetiredColonyStatistic
	{
		// Token: 0x0600ACE0 RID: 44256 RVA: 0x003CE07B File Offset: 0x003CC27B
		public RetiredColonyStatistic()
		{
		}

		// Token: 0x0600ACE1 RID: 44257 RVA: 0x003CE083 File Offset: 0x003CC283
		public RetiredColonyStatistic(string id, global::Tuple<float, float>[] data, string name, string axisNameX, string axisNameY)
		{
			this.id = id;
			this.value = data;
			this.name = name;
			this.nameX = axisNameX;
			this.nameY = axisNameY;
		}

		// Token: 0x0600ACE2 RID: 44258 RVA: 0x003CE0B0 File Offset: 0x003CC2B0
		public global::Tuple<float, float> GetByMaxValue()
		{
			if (this.value.Length == 0)
			{
				return new global::Tuple<float, float>(0f, 0f);
			}
			int num = -1;
			float num2 = -1f;
			for (int i = 0; i < this.value.Length; i++)
			{
				if (this.value[i].second > num2)
				{
					num2 = this.value[i].second;
					num = i;
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			return this.value[num];
		}

		// Token: 0x0600ACE3 RID: 44259 RVA: 0x003CE120 File Offset: 0x003CC320
		public global::Tuple<float, float> GetByMaxKey()
		{
			if (this.value.Length == 0)
			{
				return new global::Tuple<float, float>(0f, 0f);
			}
			int num = -1;
			float num2 = -1f;
			for (int i = 0; i < this.value.Length; i++)
			{
				if (this.value[i].first > num2)
				{
					num2 = this.value[i].first;
					num = i;
				}
			}
			return this.value[num];
		}

		// Token: 0x04008769 RID: 34665
		public string id;

		// Token: 0x0400876A RID: 34666
		public global::Tuple<float, float>[] value;

		// Token: 0x0400876B RID: 34667
		public string name;

		// Token: 0x0400876C RID: 34668
		public string nameX;

		// Token: 0x0400876D RID: 34669
		public string nameY;
	}

	// Token: 0x02001C40 RID: 7232
	public class RetiredDuplicantData
	{
		// Token: 0x0400876E RID: 34670
		public string name;

		// Token: 0x0400876F RID: 34671
		public int age;

		// Token: 0x04008770 RID: 34672
		public int skillPointsGained;

		// Token: 0x04008771 RID: 34673
		public Dictionary<string, string> accessories;
	}
}
