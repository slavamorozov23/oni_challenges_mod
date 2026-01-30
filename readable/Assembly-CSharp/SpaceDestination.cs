using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000BA9 RID: 2985
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{id}: {type} at distance {distance}")]
public class SpaceDestination
{
	// Token: 0x06005938 RID: 22840 RVA: 0x002062B8 File Offset: 0x002044B8
	private static global::Tuple<SimHashes, MathUtil.MinMax> GetRareElement(SimHashes id)
	{
		foreach (global::Tuple<SimHashes, MathUtil.MinMax> tuple in SpaceDestination.RARE_ELEMENTS)
		{
			if (tuple.first == id)
			{
				return tuple;
			}
		}
		return null;
	}

	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x06005939 RID: 22841 RVA: 0x00206314 File Offset: 0x00204514
	public int OneBasedDistance
	{
		get
		{
			return this.distance + 1;
		}
	}

	// Token: 0x1700067F RID: 1663
	// (get) Token: 0x0600593A RID: 22842 RVA: 0x0020631E File Offset: 0x0020451E
	public float CurrentMass
	{
		get
		{
			return (float)this.GetDestinationType().minimumMass + this.availableMass;
		}
	}

	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x0600593B RID: 22843 RVA: 0x00206333 File Offset: 0x00204533
	public float AvailableMass
	{
		get
		{
			return this.availableMass;
		}
	}

	// Token: 0x0600593C RID: 22844 RVA: 0x0020633C File Offset: 0x0020453C
	public SpaceDestination(int id, string type, int distance)
	{
		this.id = id;
		this.type = type;
		this.distance = distance;
		SpaceDestinationType destinationType = this.GetDestinationType();
		this.availableMass = (float)(destinationType.maxiumMass - destinationType.minimumMass);
		this.GenerateSurfaceElements();
		this.GenerateResearchOpportunities();
	}

	// Token: 0x0600593D RID: 22845 RVA: 0x002063B8 File Offset: 0x002045B8
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 9))
		{
			SpaceDestinationType destinationType = this.GetDestinationType();
			this.availableMass = (float)(destinationType.maxiumMass - destinationType.minimumMass);
		}
	}

	// Token: 0x0600593E RID: 22846 RVA: 0x002063F7 File Offset: 0x002045F7
	public SpaceDestinationType GetDestinationType()
	{
		return Db.Get().SpaceDestinationTypes.Get(this.type);
	}

	// Token: 0x0600593F RID: 22847 RVA: 0x00206410 File Offset: 0x00204610
	public SpaceDestination.ResearchOpportunity TryCompleteResearchOpportunity()
	{
		foreach (SpaceDestination.ResearchOpportunity researchOpportunity in this.researchOpportunities)
		{
			if (researchOpportunity.TryComplete(this))
			{
				return researchOpportunity;
			}
		}
		return null;
	}

	// Token: 0x06005940 RID: 22848 RVA: 0x0020646C File Offset: 0x0020466C
	public void GenerateSurfaceElements()
	{
		foreach (KeyValuePair<SimHashes, MathUtil.MinMax> keyValuePair in this.GetDestinationType().elementTable)
		{
			this.recoverableElements.Add(keyValuePair.Key, UnityEngine.Random.value);
		}
	}

	// Token: 0x06005941 RID: 22849 RVA: 0x002064D4 File Offset: 0x002046D4
	public SpacecraftManager.DestinationAnalysisState AnalysisState()
	{
		return SpacecraftManager.instance.GetDestinationAnalysisState(this);
	}

	// Token: 0x06005942 RID: 22850 RVA: 0x002064E4 File Offset: 0x002046E4
	public void GenerateResearchOpportunities()
	{
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.UPPERATMO, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.LOWERATMO, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.MAGNETICFIELD, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SURFACE, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SUBSURFACE, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		float num = 0f;
		foreach (global::Tuple<float, int> tuple in SpaceDestination.RARE_ELEMENT_CHANCES)
		{
			num += tuple.first;
		}
		float num2 = UnityEngine.Random.value * num;
		int num3 = 0;
		foreach (global::Tuple<float, int> tuple2 in SpaceDestination.RARE_ELEMENT_CHANCES)
		{
			num2 -= tuple2.first;
			if (num2 <= 0f)
			{
				num3 = tuple2.second;
			}
		}
		for (int i = 0; i < num3; i++)
		{
			this.researchOpportunities[UnityEngine.Random.Range(0, this.researchOpportunities.Count)].discoveredRareResource = SpaceDestination.RARE_ELEMENTS[UnityEngine.Random.Range(0, SpaceDestination.RARE_ELEMENTS.Count)].first;
		}
		if (UnityEngine.Random.value < 0.33f)
		{
			int index = UnityEngine.Random.Range(0, this.researchOpportunities.Count);
			this.researchOpportunities[index].discoveredRareItem = SpaceDestination.RARE_ITEMS[UnityEngine.Random.Range(0, SpaceDestination.RARE_ITEMS.Count)].first;
		}
	}

	// Token: 0x06005943 RID: 22851 RVA: 0x002066DC File Offset: 0x002048DC
	public float GetResourceValue(SimHashes resource, float roll)
	{
		if (this.GetDestinationType().elementTable.ContainsKey(resource))
		{
			return this.GetDestinationType().elementTable[resource].Lerp(roll);
		}
		if (SpaceDestinationTypes.extendedElementTable.ContainsKey(resource))
		{
			return SpaceDestinationTypes.extendedElementTable[resource].Lerp(roll);
		}
		return 0f;
	}

	// Token: 0x06005944 RID: 22852 RVA: 0x00206740 File Offset: 0x00204940
	public Dictionary<SimHashes, float> GetMissionResourceResult(float totalCargoSpace, float reservedMass, bool solids = true, bool liquids = true, bool gasses = true)
	{
		Dictionary<SimHashes, float> dictionary = new Dictionary<SimHashes, float>();
		float num = 0f;
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && gasses))
			{
				num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value);
			}
		}
		float num2 = Mathf.Min(this.CurrentMass + reservedMass - (float)this.GetDestinationType().minimumMass, totalCargoSpace);
		foreach (KeyValuePair<SimHashes, float> keyValuePair2 in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair2.Key).IsSolid && solids) || (ElementLoader.FindElementByHash(keyValuePair2.Key).IsLiquid && liquids) || (ElementLoader.FindElementByHash(keyValuePair2.Key).IsGas && gasses))
			{
				float value = num2 * (this.GetResourceValue(keyValuePair2.Key, keyValuePair2.Value) / num);
				dictionary.Add(keyValuePair2.Key, value);
			}
		}
		return dictionary;
	}

	// Token: 0x06005945 RID: 22853 RVA: 0x002068B4 File Offset: 0x00204AB4
	public Dictionary<Tag, int> GetRecoverableEntities()
	{
		Dictionary<Tag, int> dictionary = new Dictionary<Tag, int>();
		Dictionary<string, int> recoverableEntities = this.GetDestinationType().recoverableEntities;
		if (recoverableEntities != null)
		{
			foreach (KeyValuePair<string, int> keyValuePair in recoverableEntities)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		return dictionary;
	}

	// Token: 0x06005946 RID: 22854 RVA: 0x0020692C File Offset: 0x00204B2C
	public Dictionary<Tag, int> GetMissionEntityResult()
	{
		return this.GetRecoverableEntities();
	}

	// Token: 0x06005947 RID: 22855 RVA: 0x00206934 File Offset: 0x00204B34
	public float ReserveResources(CargoBay bay)
	{
		float num = 0f;
		if (bay != null)
		{
			Storage component = bay.GetComponent<Storage>();
			foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
			{
				if (this.HasElementType(bay.storageType))
				{
					num += component.capacityKg;
					this.availableMass = Mathf.Max(0f, this.availableMass - component.capacityKg);
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x06005948 RID: 22856 RVA: 0x002069D0 File Offset: 0x00204BD0
	public bool HasElementType(CargoBay.CargoType type)
	{
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && type == CargoBay.CargoType.Solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && type == CargoBay.CargoType.Liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && type == CargoBay.CargoType.Gasses))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005949 RID: 22857 RVA: 0x00206A68 File Offset: 0x00204C68
	public void Replenish(float dt)
	{
		SpaceDestinationType destinationType = this.GetDestinationType();
		if (this.CurrentMass < (float)destinationType.maxiumMass)
		{
			this.availableMass += destinationType.replishmentPerSim1000ms;
		}
	}

	// Token: 0x0600594A RID: 22858 RVA: 0x00206AA0 File Offset: 0x00204CA0
	public float GetAvailableResourcesPercentage(CargoBay.CargoType cargoType)
	{
		float num = 0f;
		float totalMass = this.GetTotalMass();
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && cargoType == CargoBay.CargoType.Solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && cargoType == CargoBay.CargoType.Liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && cargoType == CargoBay.CargoType.Gasses))
			{
				num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value) / totalMass;
			}
		}
		return num;
	}

	// Token: 0x0600594B RID: 22859 RVA: 0x00206B58 File Offset: 0x00204D58
	public float GetTotalMass()
	{
		float num = 0f;
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value);
		}
		return num;
	}

	// Token: 0x04003BE2 RID: 15330
	private const int MASS_TO_RECOVER_AMOUNT = 1000;

	// Token: 0x04003BE3 RID: 15331
	private static List<global::Tuple<float, int>> RARE_ELEMENT_CHANCES = new List<global::Tuple<float, int>>
	{
		new global::Tuple<float, int>(1f, 0),
		new global::Tuple<float, int>(0.33f, 1),
		new global::Tuple<float, int>(0.03f, 2)
	};

	// Token: 0x04003BE4 RID: 15332
	private static readonly List<global::Tuple<SimHashes, MathUtil.MinMax>> RARE_ELEMENTS = new List<global::Tuple<SimHashes, MathUtil.MinMax>>
	{
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Katairite, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Niobium, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Fullerene, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Isoresin, new MathUtil.MinMax(1f, 10f))
	};

	// Token: 0x04003BE5 RID: 15333
	private const float RARE_ITEM_CHANCE = 0.33f;

	// Token: 0x04003BE6 RID: 15334
	private static readonly List<global::Tuple<string, MathUtil.MinMax>> RARE_ITEMS = new List<global::Tuple<string, MathUtil.MinMax>>
	{
		new global::Tuple<string, MathUtil.MinMax>("GeneShufflerRecharge", new MathUtil.MinMax(1f, 2f))
	};

	// Token: 0x04003BE7 RID: 15335
	[Serialize]
	public int id;

	// Token: 0x04003BE8 RID: 15336
	[Serialize]
	public string type;

	// Token: 0x04003BE9 RID: 15337
	public bool startAnalyzed;

	// Token: 0x04003BEA RID: 15338
	[Serialize]
	public int distance;

	// Token: 0x04003BEB RID: 15339
	[Serialize]
	public float activePeriod = 20f;

	// Token: 0x04003BEC RID: 15340
	[Serialize]
	public float inactivePeriod = 10f;

	// Token: 0x04003BED RID: 15341
	[Serialize]
	public float startingOrbitPercentage;

	// Token: 0x04003BEE RID: 15342
	[Serialize]
	public Dictionary<SimHashes, float> recoverableElements = new Dictionary<SimHashes, float>();

	// Token: 0x04003BEF RID: 15343
	[Serialize]
	public List<SpaceDestination.ResearchOpportunity> researchOpportunities = new List<SpaceDestination.ResearchOpportunity>();

	// Token: 0x04003BF0 RID: 15344
	[Serialize]
	private float availableMass;

	// Token: 0x02001D3B RID: 7483
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ResearchOpportunity
	{
		// Token: 0x0600B0A0 RID: 45216 RVA: 0x003DB493 File Offset: 0x003D9693
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (this.discoveredRareResource == (SimHashes)0)
			{
				this.discoveredRareResource = SimHashes.Void;
			}
			if (this.dataValue > 50)
			{
				this.dataValue = 50;
			}
		}

		// Token: 0x0600B0A1 RID: 45217 RVA: 0x003DB4BA File Offset: 0x003D96BA
		public ResearchOpportunity(string description, int pointValue)
		{
			this.description = description;
			this.dataValue = pointValue;
		}

		// Token: 0x0600B0A2 RID: 45218 RVA: 0x003DB4DC File Offset: 0x003D96DC
		public bool TryComplete(SpaceDestination destination)
		{
			if (!this.completed)
			{
				this.completed = true;
				if (this.discoveredRareResource != SimHashes.Void && !destination.recoverableElements.ContainsKey(this.discoveredRareResource))
				{
					destination.recoverableElements.Add(this.discoveredRareResource, UnityEngine.Random.value);
				}
				return true;
			}
			return false;
		}

		// Token: 0x04008AB3 RID: 35507
		[Serialize]
		public string description;

		// Token: 0x04008AB4 RID: 35508
		[Serialize]
		public int dataValue;

		// Token: 0x04008AB5 RID: 35509
		[Serialize]
		public bool completed;

		// Token: 0x04008AB6 RID: 35510
		[Serialize]
		public SimHashes discoveredRareResource = SimHashes.Void;

		// Token: 0x04008AB7 RID: 35511
		[Serialize]
		public string discoveredRareItem;
	}
}
