using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005AB RID: 1451
public class ColonyDiagnosticUtility : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06002110 RID: 8464 RVA: 0x000BF408 File Offset: 0x000BD608
	public static void DestroyInstance()
	{
		ColonyDiagnosticUtility.Instance = null;
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x000BF410 File Offset: 0x000BD610
	public ColonyDiagnostic.DiagnosticResult.Opinion GetWorldDiagnosticResult(int worldID)
	{
		ColonyDiagnostic.DiagnosticResult.Opinion opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Good;
		foreach (ColonyDiagnostic colonyDiagnostic in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else
				{
					opinion = (ColonyDiagnostic.DiagnosticResult.Opinion)Math.Min((int)opinion, (int)colonyDiagnostic.LatestResult.opinion);
				}
			}
		}
		return opinion;
	}

	// Token: 0x06002112 RID: 8466 RVA: 0x000BF4CC File Offset: 0x000BD6CC
	public string GetWorldDiagnosticResultStatus(int worldID)
	{
		ColonyDiagnostic colonyDiagnostic = null;
		foreach (ColonyDiagnostic colonyDiagnostic2 in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic2.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic2.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic2.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else if (colonyDiagnostic == null || colonyDiagnostic2.LatestResult.opinion < colonyDiagnostic.LatestResult.opinion)
				{
					colonyDiagnostic = colonyDiagnostic2;
				}
			}
		}
		if (colonyDiagnostic == null || colonyDiagnostic.LatestResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			return "";
		}
		return colonyDiagnostic.name;
	}

	// Token: 0x06002113 RID: 8467 RVA: 0x000BF5AC File Offset: 0x000BD7AC
	public string GetWorldDiagnosticResultTooltip(int worldID)
	{
		string text = "";
		foreach (ColonyDiagnostic colonyDiagnostic in this.worldDiagnostics[worldID])
		{
			if (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[worldID][colonyDiagnostic.id] != ColonyDiagnosticUtility.DisplaySetting.Never && !ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(colonyDiagnostic.id))
			{
				ColonyDiagnosticUtility.DisplaySetting displaySetting = this.diagnosticDisplaySettings[worldID][colonyDiagnostic.id];
				if (displaySetting > ColonyDiagnosticUtility.DisplaySetting.AlertOnly)
				{
					if (displaySetting != ColonyDiagnosticUtility.DisplaySetting.Never)
					{
					}
				}
				else if (colonyDiagnostic.LatestResult.opinion < ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
				{
					text = text + "\n" + colonyDiagnostic.LatestResult.GetFormattedMessage();
				}
			}
		}
		return text;
	}

	// Token: 0x06002114 RID: 8468 RVA: 0x000BF688 File Offset: 0x000BD888
	public bool IsDiagnosticTutorialDisabled(string id)
	{
		return ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus.ContainsKey(id) && GameClock.Instance.GetTime() < ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus[id];
	}

	// Token: 0x06002115 RID: 8469 RVA: 0x000BF6BB File Offset: 0x000BD8BB
	public void ClearDiagnosticTutorialSetting(string id)
	{
		if (ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus.ContainsKey(id))
		{
			ColonyDiagnosticUtility.Instance.diagnosticTutorialStatus[id] = -1f;
		}
	}

	// Token: 0x06002116 RID: 8470 RVA: 0x000BF6E4 File Offset: 0x000BD8E4
	public bool IsCriteriaEnabled(int worldID, string diagnosticID, string criteriaID)
	{
		Dictionary<string, List<string>> dictionary = this.diagnosticCriteriaDisabled[worldID];
		return dictionary.ContainsKey(diagnosticID) && !dictionary[diagnosticID].Contains(criteriaID);
	}

	// Token: 0x06002117 RID: 8471 RVA: 0x000BF71C File Offset: 0x000BD91C
	public void SetCriteriaEnabled(int worldID, string diagnosticID, string criteriaID, bool enabled)
	{
		Dictionary<string, List<string>> dictionary = this.diagnosticCriteriaDisabled[worldID];
		global::Debug.Assert(dictionary.ContainsKey(diagnosticID), string.Format("Trying to set criteria on World {0} lacks diagnostic {1} that criteria {2} relates to", worldID, diagnosticID, criteriaID));
		List<string> list = dictionary[diagnosticID];
		if (enabled && list.Contains(criteriaID))
		{
			list.Remove(criteriaID);
		}
		if (!enabled && !list.Contains(criteriaID))
		{
			list.Add(criteriaID);
		}
	}

	// Token: 0x06002118 RID: 8472 RVA: 0x000BF783 File Offset: 0x000BD983
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ColonyDiagnosticUtility.Instance = this;
	}

	// Token: 0x06002119 RID: 8473 RVA: 0x000BF794 File Offset: 0x000BD994
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 33))
		{
			string key = "IdleDiagnostic";
			foreach (int num in this.diagnosticDisplaySettings.Keys)
			{
				WorldContainer world = ClusterManager.Instance.GetWorld(num);
				if (this.diagnosticDisplaySettings[num].ContainsKey(key) && this.diagnosticDisplaySettings[num][key] != ColonyDiagnosticUtility.DisplaySetting.Always)
				{
					this.diagnosticDisplaySettings[num][key] = (world.IsModuleInterior ? ColonyDiagnosticUtility.DisplaySetting.Never : ColonyDiagnosticUtility.DisplaySetting.AlertOnly);
				}
			}
		}
		foreach (int worldID in ClusterManager.Instance.GetWorldIDsSorted())
		{
			this.AddWorld(worldID);
		}
		ClusterManager.Instance.Subscribe(-1280433810, new Action<object>(this.Refresh));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorld));
	}

	// Token: 0x0600211A RID: 8474 RVA: 0x000BF8E0 File Offset: 0x000BDAE0
	private void Refresh(object data)
	{
		int value = ((Boxed<int>)data).value;
		this.AddWorld(value);
	}

	// Token: 0x0600211B RID: 8475 RVA: 0x000BF900 File Offset: 0x000BDB00
	private void RemoveWorld(object data)
	{
		int value = ((Boxed<int>)data).value;
		if (this.diagnosticDisplaySettings.Remove(value))
		{
			List<ColonyDiagnostic> list;
			if (this.worldDiagnostics.TryGetValue(value, out list))
			{
				foreach (ColonyDiagnostic colonyDiagnostic in list)
				{
					colonyDiagnostic.OnCleanUp();
				}
			}
			this.worldDiagnostics.Remove(value);
		}
	}

	// Token: 0x0600211C RID: 8476 RVA: 0x000BF984 File Offset: 0x000BDB84
	public ColonyDiagnostic GetDiagnostic(string id, int worldID)
	{
		return this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match.id == id);
	}

	// Token: 0x0600211D RID: 8477 RVA: 0x000BF9BB File Offset: 0x000BDBBB
	public T GetDiagnostic<T>(int worldID) where T : ColonyDiagnostic
	{
		return (T)((object)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is T));
	}

	// Token: 0x0600211E RID: 8478 RVA: 0x000BF9F4 File Offset: 0x000BDBF4
	public string GetDiagnosticName(string id)
	{
		foreach (KeyValuePair<int, List<ColonyDiagnostic>> keyValuePair in this.worldDiagnostics)
		{
			foreach (ColonyDiagnostic colonyDiagnostic in keyValuePair.Value)
			{
				if (colonyDiagnostic.id == id)
				{
					return colonyDiagnostic.name;
				}
			}
		}
		global::Debug.LogWarning("Cannot locate name of diagnostic " + id + " because no worlds have a diagnostic with that id ");
		return "";
	}

	// Token: 0x0600211F RID: 8479 RVA: 0x000BFAB4 File Offset: 0x000BDCB4
	public ChoreGroupDiagnostic GetChoreGroupDiagnostic(int worldID, ChoreGroup choreGroup)
	{
		return (ChoreGroupDiagnostic)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is ChoreGroupDiagnostic && ((ChoreGroupDiagnostic)match).choreGroup == choreGroup);
	}

	// Token: 0x06002120 RID: 8480 RVA: 0x000BFAF0 File Offset: 0x000BDCF0
	public WorkTimeDiagnostic GetWorkTimeDiagnostic(int worldID, ChoreGroup choreGroup)
	{
		return (WorkTimeDiagnostic)this.worldDiagnostics[worldID].Find((ColonyDiagnostic match) => match is WorkTimeDiagnostic && ((WorkTimeDiagnostic)match).choreGroup == choreGroup);
	}

	// Token: 0x06002121 RID: 8481 RVA: 0x000BFB2C File Offset: 0x000BDD2C
	private void TryAddDiagnosticToWorldCollection(ref List<ColonyDiagnostic> newWorldDiagnostics, ColonyDiagnostic newDiagnostic)
	{
		if (!Game.IsCorrectDlcActiveForCurrentSave(newDiagnostic))
		{
			newDiagnostic.OnCleanUp();
			return;
		}
		newWorldDiagnostics.Add(newDiagnostic);
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x000BFB48 File Offset: 0x000BDD48
	public void AddWorld(int worldID)
	{
		bool flag = false;
		if (!this.diagnosticDisplaySettings.ContainsKey(worldID))
		{
			this.diagnosticDisplaySettings.Add(worldID, new Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>());
			flag = true;
		}
		if (!this.diagnosticCriteriaDisabled.ContainsKey(worldID))
		{
			this.diagnosticCriteriaDisabled.Add(worldID, new Dictionary<string, List<string>>());
		}
		List<ColonyDiagnostic> list = new List<ColonyDiagnostic>();
		this.TryAddDiagnosticToWorldCollection(ref list, new IdleDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new BreathabilityDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new FoodDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new StressDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new RadiationDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new ReactorDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new SelfChargingElectrobankDiagnostic(worldID));
		this.TryAddDiagnosticToWorldCollection(ref list, new BionicBatteryDiagnostic(worldID));
		if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
		{
			this.TryAddDiagnosticToWorldCollection(ref list, new FloatingRocketDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketFuelDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketOxidizerDiagnostic(worldID));
		}
		else
		{
			this.TryAddDiagnosticToWorldCollection(ref list, new BedDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new ToiletDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new PowerUseDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new BatteryDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new TrappedDuplicantDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new FarmDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new EntombedDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new RocketsInOrbitDiagnostic(worldID));
			this.TryAddDiagnosticToWorldCollection(ref list, new MeteorDiagnostic(worldID));
		}
		this.worldDiagnostics.Add(worldID, list);
		foreach (ColonyDiagnostic colonyDiagnostic in list)
		{
			if (!this.diagnosticDisplaySettings[worldID].ContainsKey(colonyDiagnostic.id))
			{
				this.diagnosticDisplaySettings[worldID].Add(colonyDiagnostic.id, ColonyDiagnosticUtility.DisplaySetting.AlertOnly);
			}
			if (!this.diagnosticCriteriaDisabled[worldID].ContainsKey(colonyDiagnostic.id))
			{
				this.diagnosticCriteriaDisabled[worldID].Add(colonyDiagnostic.id, new List<string>());
			}
		}
		if (flag)
		{
			this.diagnosticDisplaySettings[worldID]["BreathabilityDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
			this.diagnosticDisplaySettings[worldID]["FoodDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
			this.diagnosticDisplaySettings[worldID]["StressDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
			if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
			{
				this.diagnosticDisplaySettings[worldID]["FloatingRocketDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
				this.diagnosticDisplaySettings[worldID]["RocketFuelDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
				this.diagnosticDisplaySettings[worldID]["RocketOxidizerDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Always;
				this.diagnosticDisplaySettings[worldID]["IdleDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.Never;
				return;
			}
			this.diagnosticDisplaySettings[worldID]["IdleDiagnostic"] = ColonyDiagnosticUtility.DisplaySetting.AlertOnly;
		}
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000BFE48 File Offset: 0x000BE048
	public void Sim1000ms(float dt)
	{
		if (ColonyDiagnosticUtility.IgnoreFirstUpdate)
		{
			ColonyDiagnosticUtility.IgnoreFirstUpdate = false;
		}
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x000BFE58 File Offset: 0x000BE058
	public static bool PastNewBuildingGracePeriod(Transform building)
	{
		BuildingComplete component = building.GetComponent<BuildingComplete>();
		return !(component != null) || GameClock.Instance.GetTime() - component.creationTime >= 600f;
	}

	// Token: 0x06002125 RID: 8485 RVA: 0x000BFE90 File Offset: 0x000BE090
	public static bool IgnoreRocketsWithNoCrewRequested(int worldID, out ColonyDiagnostic.DiagnosticResult result)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(worldID);
		string message = world.IsModuleInterior ? UI.COLONY_DIAGNOSTICS.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.NO_MINIONS_PLANETOID;
		result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, message, null);
		if (world.IsModuleInterior)
		{
			for (int i = 0; i < Components.Clustercrafts.Count; i++)
			{
				WorldContainer interiorWorld = Components.Clustercrafts[i].ModuleInterface.GetInteriorWorld();
				if (!(interiorWorld == null) && interiorWorld.id == worldID)
				{
					PassengerRocketModule passengerModule = Components.Clustercrafts[i].ModuleInterface.GetPassengerModule();
					if (passengerModule != null && !passengerModule.ShouldCrewGetIn())
					{
						result = default(ColonyDiagnostic.DiagnosticResult);
						result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
						result.Message = UI.COLONY_DIAGNOSTICS.NO_MINIONS_REQUESTED;
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x04001345 RID: 4933
	public static ColonyDiagnosticUtility Instance;

	// Token: 0x04001346 RID: 4934
	private Dictionary<int, List<ColonyDiagnostic>> worldDiagnostics = new Dictionary<int, List<ColonyDiagnostic>>();

	// Token: 0x04001347 RID: 4935
	[Serialize]
	public Dictionary<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>> diagnosticDisplaySettings = new Dictionary<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>>();

	// Token: 0x04001348 RID: 4936
	[Serialize]
	public Dictionary<int, Dictionary<string, List<string>>> diagnosticCriteriaDisabled = new Dictionary<int, Dictionary<string, List<string>>>();

	// Token: 0x04001349 RID: 4937
	[Serialize]
	private Dictionary<string, float> diagnosticTutorialStatus = new Dictionary<string, float>
	{
		{
			"ToiletDiagnostic",
			450f
		},
		{
			"BedDiagnostic",
			900f
		},
		{
			"BreathabilityDiagnostic",
			1800f
		},
		{
			"FoodDiagnostic",
			3000f
		},
		{
			"FarmDiagnostic",
			6000f
		},
		{
			"StressDiagnostic",
			9000f
		},
		{
			"PowerUseDiagnostic",
			12000f
		},
		{
			"BatteryDiagnostic",
			12000f
		},
		{
			"IdleDiagnostic",
			600f
		}
	};

	// Token: 0x0400134A RID: 4938
	public static bool IgnoreFirstUpdate = true;

	// Token: 0x0400134B RID: 4939
	public static ColonyDiagnostic.DiagnosticResult NoDataResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.NO_DATA, null);

	// Token: 0x0200142D RID: 5165
	public enum DisplaySetting
	{
		// Token: 0x04006DCA RID: 28106
		Always,
		// Token: 0x04006DCB RID: 28107
		AlertOnly,
		// Token: 0x04006DCC RID: 28108
		Never,
		// Token: 0x04006DCD RID: 28109
		LENGTH
	}
}
