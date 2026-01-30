using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using KSerialization;
using ProcGen;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000BAB RID: 2987
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SpacecraftManager")]
public class SpacecraftManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06005961 RID: 22881 RVA: 0x00207178 File Offset: 0x00205378
	public static void DestroyInstance()
	{
		SpacecraftManager.instance = null;
	}

	// Token: 0x06005962 RID: 22882 RVA: 0x00207180 File Offset: 0x00205380
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SpacecraftManager.instance = this;
		if (this.savedSpacecraftDestinations == null)
		{
			this.savedSpacecraftDestinations = new Dictionary<int, int>();
		}
	}

	// Token: 0x06005963 RID: 22883 RVA: 0x002071A4 File Offset: 0x002053A4
	private void GenerateFixedDestinations()
	{
		SpaceDestinationTypes spaceDestinationTypes = Db.Get().SpaceDestinationTypes;
		if (this.destinations != null)
		{
			return;
		}
		this.destinations = new List<SpaceDestination>
		{
			new SpaceDestination(0, spaceDestinationTypes.CarbonaceousAsteroid.Id, 0),
			new SpaceDestination(1, spaceDestinationTypes.CarbonaceousAsteroid.Id, 0),
			new SpaceDestination(2, spaceDestinationTypes.MetallicAsteroid.Id, 1),
			new SpaceDestination(3, spaceDestinationTypes.RockyAsteroid.Id, 2),
			new SpaceDestination(4, spaceDestinationTypes.IcyDwarf.Id, 3),
			new SpaceDestination(5, spaceDestinationTypes.OrganicDwarf.Id, 4)
		};
	}

	// Token: 0x06005964 RID: 22884 RVA: 0x00207260 File Offset: 0x00205460
	private void GenerateRandomDestinations()
	{
		KRandom krandom = new KRandom(SaveLoader.Instance.clusterDetailSave.globalWorldSeed);
		SpaceDestinationTypes spaceDestinationTypes = Db.Get().SpaceDestinationTypes;
		List<List<string>> list = new List<List<string>>
		{
			new List<string>(),
			new List<string>
			{
				spaceDestinationTypes.OilyAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.Satellite.Id
			},
			new List<string>
			{
				spaceDestinationTypes.Satellite.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.CarbonaceousAsteroid.Id,
				spaceDestinationTypes.ForestPlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.CarbonaceousAsteroid.Id,
				spaceDestinationTypes.SaltDwarf.Id
			},
			new List<string>
			{
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.CarbonaceousAsteroid.Id,
				spaceDestinationTypes.IcyDwarf.Id,
				spaceDestinationTypes.OrganicDwarf.Id
			},
			new List<string>
			{
				spaceDestinationTypes.IcyDwarf.Id,
				spaceDestinationTypes.OrganicDwarf.Id,
				spaceDestinationTypes.DustyMoon.Id,
				spaceDestinationTypes.ChlorinePlanet.Id,
				spaceDestinationTypes.RedDwarf.Id
			},
			new List<string>
			{
				spaceDestinationTypes.DustyMoon.Id,
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.VolcanoPlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.GasGiant.Id,
				spaceDestinationTypes.IceGiant.Id,
				spaceDestinationTypes.RustPlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.GasGiant.Id,
				spaceDestinationTypes.IceGiant.Id,
				spaceDestinationTypes.HydrogenGiant.Id
			},
			new List<string>
			{
				spaceDestinationTypes.RustPlanet.Id,
				spaceDestinationTypes.VolcanoPlanet.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.MetallicAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.ShinyPlanet.Id,
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.RockyAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.GoldAsteroid.Id,
				spaceDestinationTypes.OrganicDwarf.Id,
				spaceDestinationTypes.ForestPlanet.Id,
				spaceDestinationTypes.ChlorinePlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.IcyDwarf.Id,
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.DustyMoon.Id,
				spaceDestinationTypes.VolcanoPlanet.Id,
				spaceDestinationTypes.IceGiant.Id
			},
			new List<string>
			{
				spaceDestinationTypes.ShinyPlanet.Id,
				spaceDestinationTypes.RedDwarf.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.GasGiant.Id
			},
			new List<string>
			{
				spaceDestinationTypes.HydrogenGiant.Id,
				spaceDestinationTypes.ForestPlanet.Id,
				spaceDestinationTypes.OilyAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.GoldAsteroid.Id,
				spaceDestinationTypes.SaltDwarf.Id,
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.VolcanoPlanet.Id
			}
		};
		List<int> list2 = new List<int>();
		int num = 3;
		int minValue = 15;
		int maxValue = 25;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Count != 0)
			{
				for (int j = 0; j < num; j++)
				{
					list2.Add(i);
				}
			}
		}
		SpacecraftManager.<>c__DisplayClass12_0 CS$<>8__locals1;
		CS$<>8__locals1.nextId = this.destinations.Count;
		int num2 = krandom.Next(minValue, maxValue);
		List<SpaceDestination> list3 = new List<SpaceDestination>();
		for (int k = 0; k < num2; k++)
		{
			int index = krandom.Next(0, list2.Count - 1);
			int num3 = list2[index];
			list2.RemoveAt(index);
			List<string> list4 = list[num3];
			string type = list4[krandom.Next(0, list4.Count)];
			SpaceDestination item = new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), type, num3);
			list3.Add(item);
		}
		list2.ShuffleSeeded(krandom);
		List<SpaceDestination> list5 = new List<SpaceDestination>();
		foreach (string name in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
		{
			DlcMixingSettings cachedDlcMixingSettings = SettingsCache.GetCachedDlcMixingSettings(name);
			if (cachedDlcMixingSettings != null)
			{
				foreach (DlcMixingSettings.SpaceDestinationMix spaceDestinationMix in cachedDlcMixingSettings.spaceDesinations)
				{
					bool flag = false;
					if (list2.Count > 0)
					{
						for (int l = 0; l < list2.Count; l++)
						{
							int num4 = list2[l];
							if (num4 >= spaceDestinationMix.minTier && num4 <= spaceDestinationMix.maxTier)
							{
								SpaceDestination item2 = new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), spaceDestinationMix.type, num4);
								list5.Add(item2);
								list2.RemoveAt(l);
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						for (int m = 0; m < list3.Count; m++)
						{
							SpaceDestination spaceDestination = list3[m];
							if (spaceDestination.distance >= spaceDestinationMix.minTier && spaceDestination.distance <= spaceDestinationMix.maxTier)
							{
								list3[m] = new SpaceDestination(spaceDestination.id, spaceDestinationMix.type, spaceDestination.distance);
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						KCrashReporter.ReportDevNotification("Base game failed to mix a space destination", Environment.StackTrace, "", false, null);
						UnityEngine.Debug.LogWarning("Mixing: Unable to place destination '" + spaceDestinationMix.type + "'");
					}
				}
			}
		}
		this.destinations.AddRange(list3);
		this.destinations.Add(new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), Db.Get().SpaceDestinationTypes.Earth.Id, 4));
		this.destinations.Add(new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), Db.Get().SpaceDestinationTypes.Wormhole.Id, list.Count));
		this.destinations.AddRange(list5);
	}

	// Token: 0x06005965 RID: 22885 RVA: 0x00207A60 File Offset: 0x00205C60
	public bool AddDestination(string id, SpacecraftManager.DestinationLocationSelectionType selection, int minRandomDistance = 0, int maxRandomDistance = 2147483647, int maxPerDistance = 3)
	{
		List<int> list = new List<int>();
		int num = 0;
		foreach (SpaceDestination spaceDestination in this.destinations)
		{
			num = Math.Max(num, spaceDestination.distance);
		}
		num = Math.Min(num, maxRandomDistance);
		for (int i = minRandomDistance; i <= num; i++)
		{
			int num2 = 0;
			using (List<SpaceDestination>.Enumerator enumerator = this.destinations.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.distance == i)
					{
						num2++;
					}
				}
			}
			if (num2 < maxPerDistance)
			{
				list.Add(i);
			}
		}
		if (list.Count == 0)
		{
			global::Debug.LogWarning("Failed to find location to spawn new destination " + id);
			return false;
		}
		int num3 = list[0];
		if (selection != SpacecraftManager.DestinationLocationSelectionType.Nearest)
		{
			if (selection == SpacecraftManager.DestinationLocationSelectionType.Random)
			{
				num3 = list[UnityEngine.Random.Range(0, list.Count)];
			}
		}
		else
		{
			num3 = list[0];
		}
		List<float> list2 = new List<float>();
		for (float num4 = 0f; num4 < 0.999f; num4 += 0.1f)
		{
			list2.Add(num4);
		}
		foreach (SpaceDestination spaceDestination2 in this.destinations)
		{
			if (spaceDestination2.distance == num3)
			{
				list2.Remove(spaceDestination2.startingOrbitPercentage);
			}
		}
		SpaceDestination spaceDestination3 = new SpaceDestination(this.destinations.Count, id, num3);
		spaceDestination3.startingOrbitPercentage = list2[UnityEngine.Random.Range(0, list2.Count)];
		this.destinations.Add(spaceDestination3);
		base.Trigger(611818744, spaceDestination3);
		return true;
	}

	// Token: 0x06005966 RID: 22886 RVA: 0x00207C48 File Offset: 0x00205E48
	private void RestoreDestinations()
	{
		if (this.destinationsGenerated)
		{
			return;
		}
		this.GenerateFixedDestinations();
		this.GenerateRandomDestinations();
		this.destinations.Sort((SpaceDestination a, SpaceDestination b) => a.distance.CompareTo(b.distance));
		List<float> list = new List<float>();
		for (int i = 0; i < 10; i++)
		{
			list.Add((float)i / 10f);
		}
		for (int j = 0; j < 20; j++)
		{
			list.Shuffle<float>();
			int num = 0;
			foreach (SpaceDestination spaceDestination in this.destinations)
			{
				if (spaceDestination.distance == j)
				{
					num++;
					spaceDestination.startingOrbitPercentage = list[num];
				}
			}
		}
		this.destinationsGenerated = true;
	}

	// Token: 0x06005967 RID: 22887 RVA: 0x00207D30 File Offset: 0x00205F30
	public SpaceDestination GetSpacecraftDestination(LaunchConditionManager lcm)
	{
		Spacecraft spacecraftFromLaunchConditionManager = this.GetSpacecraftFromLaunchConditionManager(lcm);
		return this.GetSpacecraftDestination(spacecraftFromLaunchConditionManager.id);
	}

	// Token: 0x06005968 RID: 22888 RVA: 0x00207D51 File Offset: 0x00205F51
	public SpaceDestination GetSpacecraftDestination(int spacecraftID)
	{
		this.CleanSavedSpacecraftDestinations();
		if (this.savedSpacecraftDestinations.ContainsKey(spacecraftID))
		{
			return this.GetDestination(this.savedSpacecraftDestinations[spacecraftID]);
		}
		return null;
	}

	// Token: 0x06005969 RID: 22889 RVA: 0x00207D7C File Offset: 0x00205F7C
	public List<int> GetSpacecraftsForDestination(SpaceDestination destination)
	{
		this.CleanSavedSpacecraftDestinations();
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, int> keyValuePair in this.savedSpacecraftDestinations)
		{
			if (keyValuePair.Value == destination.id)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x0600596A RID: 22890 RVA: 0x00207DF4 File Offset: 0x00205FF4
	private void CleanSavedSpacecraftDestinations()
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, int> keyValuePair in this.savedSpacecraftDestinations)
		{
			bool flag = false;
			using (List<Spacecraft>.Enumerator enumerator2 = this.spacecraft.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.id == keyValuePair.Key)
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2 = false;
			using (List<SpaceDestination>.Enumerator enumerator3 = this.destinations.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					if (enumerator3.Current.id == keyValuePair.Value)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag || !flag2)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (int key in list)
		{
			this.savedSpacecraftDestinations.Remove(key);
		}
	}

	// Token: 0x0600596B RID: 22891 RVA: 0x00207F48 File Offset: 0x00206148
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.spacecraftManager = this;
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			global::Debug.Assert(this.spacecraft == null || this.spacecraft.Count == 0);
			return;
		}
		this.RestoreDestinations();
	}

	// Token: 0x0600596C RID: 22892 RVA: 0x00207F88 File Offset: 0x00206188
	public void SetSpacecraftDestination(LaunchConditionManager lcm, SpaceDestination destination)
	{
		Spacecraft spacecraftFromLaunchConditionManager = this.GetSpacecraftFromLaunchConditionManager(lcm);
		this.savedSpacecraftDestinations[spacecraftFromLaunchConditionManager.id] = destination.id;
		lcm.Trigger(929158128, destination);
	}

	// Token: 0x0600596D RID: 22893 RVA: 0x00207FC0 File Offset: 0x002061C0
	public int GetSpacecraftID(ILaunchableRocket rocket)
	{
		foreach (Spacecraft spacecraft in this.spacecraft)
		{
			if (spacecraft.launchConditions.gameObject == rocket.LaunchableGameObject)
			{
				return spacecraft.id;
			}
		}
		return -1;
	}

	// Token: 0x0600596E RID: 22894 RVA: 0x00208030 File Offset: 0x00206230
	public SpaceDestination GetDestination(int destinationID)
	{
		foreach (SpaceDestination spaceDestination in this.destinations)
		{
			if (spaceDestination.id == destinationID)
			{
				return spaceDestination;
			}
		}
		global::Debug.LogErrorFormat("No space destination with ID {0}", new object[]
		{
			destinationID
		});
		return null;
	}

	// Token: 0x0600596F RID: 22895 RVA: 0x002080A8 File Offset: 0x002062A8
	public void RegisterSpacecraft(Spacecraft craft)
	{
		if (this.spacecraft.Contains(craft))
		{
			return;
		}
		if (craft.HasInvalidID())
		{
			craft.SetID(this.nextSpacecraftID);
			this.nextSpacecraftID++;
		}
		this.spacecraft.Add(craft);
	}

	// Token: 0x06005970 RID: 22896 RVA: 0x002080E8 File Offset: 0x002062E8
	public void UnregisterSpacecraft(LaunchConditionManager conditionManager)
	{
		Spacecraft spacecraftFromLaunchConditionManager = this.GetSpacecraftFromLaunchConditionManager(conditionManager);
		spacecraftFromLaunchConditionManager.SetState(Spacecraft.MissionState.Destroyed);
		this.spacecraft.Remove(spacecraftFromLaunchConditionManager);
	}

	// Token: 0x06005971 RID: 22897 RVA: 0x00208111 File Offset: 0x00206311
	public List<Spacecraft> GetSpacecraft()
	{
		return this.spacecraft;
	}

	// Token: 0x06005972 RID: 22898 RVA: 0x0020811C File Offset: 0x0020631C
	public Spacecraft GetSpacecraftFromLaunchConditionManager(LaunchConditionManager lcm)
	{
		foreach (Spacecraft spacecraft in this.spacecraft)
		{
			if (spacecraft.launchConditions == lcm)
			{
				return spacecraft;
			}
		}
		return null;
	}

	// Token: 0x06005973 RID: 22899 RVA: 0x00208180 File Offset: 0x00206380
	public void Sim1000ms(float dt)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return;
		}
		foreach (Spacecraft spacecraft in this.spacecraft)
		{
			spacecraft.ProgressMission(dt);
		}
		foreach (SpaceDestination spaceDestination in this.destinations)
		{
			spaceDestination.Replenish(dt);
		}
	}

	// Token: 0x06005974 RID: 22900 RVA: 0x0020821C File Offset: 0x0020641C
	public void PushReadyToLandNotification(Spacecraft spacecraft)
	{
		Notification notification = new Notification(BUILDING.STATUSITEMS.SPACECRAFTREADYTOLAND.NOTIFICATION, NotificationType.Good, delegate(List<Notification> notificationList, object data)
		{
			string text = BUILDING.STATUSITEMS.SPACECRAFTREADYTOLAND.NOTIFICATION_TOOLTIP;
			foreach (Notification notification2 in notificationList)
			{
				text = text + "\n" + (string)notification2.tooltipData;
			}
			return text;
		}, "• " + spacecraft.rocketName, true, 0f, null, null, null, true, false, false);
		spacecraft.launchConditions.gameObject.AddOrGet<Notifier>().Add(notification, "");
	}

	// Token: 0x06005975 RID: 22901 RVA: 0x00208290 File Offset: 0x00206490
	private void SpawnMissionResults(Dictionary<SimHashes, float> results)
	{
		foreach (KeyValuePair<SimHashes, float> keyValuePair in results)
		{
			ElementLoader.FindElementByHash(keyValuePair.Key).substance.SpawnResource(PlayerController.GetCursorPos(KInputManager.GetMousePos()), keyValuePair.Value, 300f, 0, 0, false, false, false);
		}
	}

	// Token: 0x06005976 RID: 22902 RVA: 0x0020830C File Offset: 0x0020650C
	public float GetDestinationAnalysisScore(SpaceDestination destination)
	{
		return this.GetDestinationAnalysisScore(destination.id);
	}

	// Token: 0x06005977 RID: 22903 RVA: 0x0020831A File Offset: 0x0020651A
	public float GetDestinationAnalysisScore(int destinationID)
	{
		if (this.destinationAnalysisScores.ContainsKey(destinationID))
		{
			return this.destinationAnalysisScores[destinationID];
		}
		return 0f;
	}

	// Token: 0x06005978 RID: 22904 RVA: 0x0020833C File Offset: 0x0020653C
	public void EarnDestinationAnalysisPoints(int destinationID, float points)
	{
		if (!this.destinationAnalysisScores.ContainsKey(destinationID))
		{
			this.destinationAnalysisScores.Add(destinationID, 0f);
		}
		SpaceDestination destination = this.GetDestination(destinationID);
		SpacecraftManager.DestinationAnalysisState destinationAnalysisState = this.GetDestinationAnalysisState(destination);
		Dictionary<int, float> dictionary = this.destinationAnalysisScores;
		dictionary[destinationID] += points;
		SpacecraftManager.DestinationAnalysisState destinationAnalysisState2 = this.GetDestinationAnalysisState(destination);
		if (destinationAnalysisState != destinationAnalysisState2)
		{
			int starmapAnalysisDestinationID = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
			if (starmapAnalysisDestinationID == destinationID)
			{
				if (destinationAnalysisState2 == SpacecraftManager.DestinationAnalysisState.Complete)
				{
					if (SpacecraftManager.instance.GetDestination(starmapAnalysisDestinationID).type == Db.Get().SpaceDestinationTypes.Earth.Id)
					{
						Game.Instance.unlocks.Unlock("earth", true);
					}
					if (SpacecraftManager.instance.GetDestination(starmapAnalysisDestinationID).type == Db.Get().SpaceDestinationTypes.Wormhole.Id)
					{
						Game.Instance.unlocks.Unlock("wormhole", true);
					}
					SpacecraftManager.instance.SetStarmapAnalysisDestinationID(-1);
				}
				base.Trigger(532901469, null);
			}
		}
	}

	// Token: 0x06005979 RID: 22905 RVA: 0x00208454 File Offset: 0x00206654
	public SpacecraftManager.DestinationAnalysisState GetDestinationAnalysisState(SpaceDestination destination)
	{
		if (destination.startAnalyzed)
		{
			return SpacecraftManager.DestinationAnalysisState.Complete;
		}
		float destinationAnalysisScore = this.GetDestinationAnalysisScore(destination);
		if (destinationAnalysisScore >= (float)ROCKETRY.DESTINATION_ANALYSIS.COMPLETE)
		{
			return SpacecraftManager.DestinationAnalysisState.Complete;
		}
		if (destinationAnalysisScore >= (float)ROCKETRY.DESTINATION_ANALYSIS.DISCOVERED)
		{
			return SpacecraftManager.DestinationAnalysisState.Discovered;
		}
		return SpacecraftManager.DestinationAnalysisState.Hidden;
	}

	// Token: 0x0600597A RID: 22906 RVA: 0x0020848C File Offset: 0x0020668C
	public bool AreAllDestinationsAnalyzed()
	{
		foreach (SpaceDestination destination in this.destinations)
		{
			if (this.GetDestinationAnalysisState(destination) != SpacecraftManager.DestinationAnalysisState.Complete)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600597B RID: 22907 RVA: 0x002084EC File Offset: 0x002066EC
	public void DEBUG_RevealStarmap()
	{
		foreach (SpaceDestination spaceDestination in this.destinations)
		{
			this.EarnDestinationAnalysisPoints(spaceDestination.id, (float)ROCKETRY.DESTINATION_ANALYSIS.COMPLETE);
		}
	}

	// Token: 0x0600597C RID: 22908 RVA: 0x0020854C File Offset: 0x0020674C
	public void SetStarmapAnalysisDestinationID(int id)
	{
		this.analyzeDestinationID = id;
		base.BoxingTrigger<int>(532901469, id);
	}

	// Token: 0x0600597D RID: 22909 RVA: 0x00208561 File Offset: 0x00206761
	public int GetStarmapAnalysisDestinationID()
	{
		return this.analyzeDestinationID;
	}

	// Token: 0x0600597E RID: 22910 RVA: 0x00208569 File Offset: 0x00206769
	public bool HasAnalysisTarget()
	{
		return this.analyzeDestinationID != -1;
	}

	// Token: 0x06005980 RID: 22912 RVA: 0x0020859C File Offset: 0x0020679C
	[CompilerGenerated]
	internal static int <GenerateRandomDestinations>g__GetNextID|12_0(ref SpacecraftManager.<>c__DisplayClass12_0 A_0)
	{
		int nextId = A_0.nextId;
		A_0.nextId = nextId + 1;
		return nextId;
	}

	// Token: 0x04003BF8 RID: 15352
	public static SpacecraftManager instance;

	// Token: 0x04003BF9 RID: 15353
	[Serialize]
	private List<Spacecraft> spacecraft = new List<Spacecraft>();

	// Token: 0x04003BFA RID: 15354
	[Serialize]
	private int nextSpacecraftID;

	// Token: 0x04003BFB RID: 15355
	public const int INVALID_DESTINATION_ID = -1;

	// Token: 0x04003BFC RID: 15356
	[Serialize]
	private int analyzeDestinationID = -1;

	// Token: 0x04003BFD RID: 15357
	[Serialize]
	public bool hasVisitedWormHole;

	// Token: 0x04003BFE RID: 15358
	[Serialize]
	public List<SpaceDestination> destinations;

	// Token: 0x04003BFF RID: 15359
	[Serialize]
	public Dictionary<int, int> savedSpacecraftDestinations;

	// Token: 0x04003C00 RID: 15360
	[Serialize]
	public bool destinationsGenerated;

	// Token: 0x04003C01 RID: 15361
	[Serialize]
	public Dictionary<int, float> destinationAnalysisScores = new Dictionary<int, float>();

	// Token: 0x02001D3D RID: 7485
	public enum DestinationLocationSelectionType
	{
		// Token: 0x04008AC0 RID: 35520
		Nearest,
		// Token: 0x04008AC1 RID: 35521
		Random
	}

	// Token: 0x02001D3E RID: 7486
	public enum DestinationAnalysisState
	{
		// Token: 0x04008AC3 RID: 35523
		Hidden,
		// Token: 0x04008AC4 RID: 35524
		Discovered,
		// Token: 0x04008AC5 RID: 35525
		Complete
	}
}
