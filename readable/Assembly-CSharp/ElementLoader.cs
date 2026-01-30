using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Klei;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x020009E2 RID: 2530
public class ElementLoader
{
	// Token: 0x06004997 RID: 18839 RVA: 0x001AA6DC File Offset: 0x001A88DC
	public static float GetMinMeltingPointAmongElements(IList<Tag> elements)
	{
		float num = float.MaxValue;
		for (int i = 0; i < elements.Count; i++)
		{
			Element element = ElementLoader.GetElement(elements[i]);
			if (element != null)
			{
				num = Mathf.Min(num, element.highTemp);
			}
		}
		return num;
	}

	// Token: 0x06004998 RID: 18840 RVA: 0x001AA720 File Offset: 0x001A8920
	public static List<ElementLoader.ElementEntry> CollectElementsFromYAML()
	{
		List<ElementLoader.ElementEntry> list = new List<ElementLoader.ElementEntry>();
		ListPool<FileHandle, ElementLoader>.PooledList pooledList = ListPool<FileHandle, ElementLoader>.Allocate();
		FileSystem.GetFiles(FileSystem.Normalize(ElementLoader.path), "*.yaml", pooledList);
		ListPool<YamlIO.Error, ElementLoader>.PooledList errors = ListPool<YamlIO.Error, ElementLoader>.Allocate();
		YamlIO.ErrorHandler <>9__0;
		foreach (FileHandle fileHandle in pooledList)
		{
			if (!Path.GetFileName(fileHandle.full_path).StartsWith("."))
			{
				string full_path = fileHandle.full_path;
				YamlIO.ErrorHandler handle_error;
				if ((handle_error = <>9__0) == null)
				{
					handle_error = (<>9__0 = delegate(YamlIO.Error error, bool force_log_as_warning)
					{
						errors.Add(error);
					});
				}
				ElementLoader.ElementEntryCollection elementEntryCollection = YamlIO.LoadFile<ElementLoader.ElementEntryCollection>(full_path, handle_error, null);
				if (elementEntryCollection != null)
				{
					list.AddRange(elementEntryCollection.elements);
				}
			}
		}
		pooledList.Recycle();
		if (Global.Instance != null && Global.Instance.modManager != null)
		{
			Global.Instance.modManager.HandleErrors(errors);
		}
		errors.Recycle();
		return list;
	}

	// Token: 0x06004999 RID: 18841 RVA: 0x001AA834 File Offset: 0x001A8A34
	public static void Load(ref Hashtable substanceList, Dictionary<string, SubstanceTable> substanceTablesByDlc)
	{
		ElementLoader.elements = new List<Element>();
		ElementLoader.elementTable = new Dictionary<int, Element>();
		ElementLoader.elementTagTable = new Dictionary<Tag, Element>();
		foreach (ElementLoader.ElementEntry elementEntry in ElementLoader.CollectElementsFromYAML())
		{
			int num = Hash.SDBMLower(elementEntry.elementId);
			if (!ElementLoader.elementTable.ContainsKey(num) && substanceTablesByDlc.ContainsKey(elementEntry.dlcId))
			{
				Element element = new Element();
				element.id = (SimHashes)num;
				element.name = Strings.Get(elementEntry.localizationID);
				element.nameUpperCase = element.name.ToUpper();
				element.description = Strings.Get(elementEntry.description);
				element.tag = TagManager.Create(elementEntry.elementId, element.name);
				ElementLoader.CopyEntryToElement(elementEntry, element);
				ElementLoader.elements.Add(element);
				ElementLoader.elementTable[num] = element;
				ElementLoader.elementTagTable[element.tag] = element;
				if (!ElementLoader.ManifestSubstanceForElement(element, ref substanceList, substanceTablesByDlc[elementEntry.dlcId]))
				{
					global::Debug.LogWarning("Missing substance for element: " + element.id.ToString());
				}
			}
		}
		ElementLoader.FinaliseElementsTable(ref substanceList);
		WorldGen.SetupDefaultElements();
	}

	// Token: 0x0600499A RID: 18842 RVA: 0x001AA9AC File Offset: 0x001A8BAC
	private static void CopyEntryToElement(ElementLoader.ElementEntry entry, Element elem)
	{
		Hash.SDBMLower(entry.elementId);
		elem.tag = TagManager.Create(entry.elementId.ToString());
		elem.specificHeatCapacity = entry.specificHeatCapacity;
		elem.thermalConductivity = entry.thermalConductivity;
		elem.molarMass = entry.molarMass;
		elem.strength = entry.strength;
		elem.disabled = entry.isDisabled;
		elem.dlcId = entry.dlcId;
		elem.flow = entry.flow;
		elem.maxMass = entry.maxMass;
		elem.maxCompression = entry.liquidCompression;
		elem.viscosity = entry.speed;
		elem.minHorizontalFlow = entry.minHorizontalFlow;
		elem.minVerticalFlow = entry.minVerticalFlow;
		elem.solidSurfaceAreaMultiplier = entry.solidSurfaceAreaMultiplier;
		elem.liquidSurfaceAreaMultiplier = entry.liquidSurfaceAreaMultiplier;
		elem.gasSurfaceAreaMultiplier = entry.gasSurfaceAreaMultiplier;
		elem.state = entry.state;
		elem.hardness = entry.hardness;
		elem.lowTemp = entry.lowTemp;
		elem.lowTempTransitionTarget = (SimHashes)Hash.SDBMLower(entry.lowTempTransitionTarget);
		elem.highTemp = entry.highTemp;
		elem.highTempTransitionTarget = (SimHashes)Hash.SDBMLower(entry.highTempTransitionTarget);
		elem.highTempTransitionOreID = (SimHashes)Hash.SDBMLower(entry.highTempTransitionOreId);
		elem.highTempTransitionOreMassConversion = entry.highTempTransitionOreMassConversion;
		elem.lowTempTransitionOreID = (SimHashes)Hash.SDBMLower(entry.lowTempTransitionOreId);
		elem.lowTempTransitionOreMassConversion = entry.lowTempTransitionOreMassConversion;
		elem.refinedMetalTarget = (SimHashes)Hash.SDBMLower(entry.refinedMetalTarget);
		elem.sublimateId = (SimHashes)Hash.SDBMLower(entry.sublimateId);
		elem.convertId = (SimHashes)Hash.SDBMLower(entry.convertId);
		elem.sublimateFX = (SpawnFXHashes)Hash.SDBMLower(entry.sublimateFx);
		elem.sublimateRate = entry.sublimateRate;
		elem.sublimateEfficiency = entry.sublimateEfficiency;
		elem.sublimateProbability = entry.sublimateProbability;
		elem.offGasPercentage = entry.offGasPercentage;
		elem.lightAbsorptionFactor = entry.lightAbsorptionFactor;
		elem.radiationAbsorptionFactor = entry.radiationAbsorptionFactor;
		elem.radiationPer1000Mass = entry.radiationPer1000Mass;
		elem.toxicity = entry.toxicity;
		elem.elementComposition = entry.composition;
		Tag phaseTag = TagManager.Create(entry.state.ToString());
		elem.materialCategory = ElementLoader.CreateMaterialCategoryTag(elem.id, phaseTag, entry.materialCategory);
		elem.oreTags = ElementLoader.CreateOreTags(elem.materialCategory, phaseTag, entry.tags);
		elem.buildMenuSort = entry.buildMenuSort;
		Sim.PhysicsData defaultValues = default(Sim.PhysicsData);
		defaultValues.temperature = entry.defaultTemperature;
		defaultValues.mass = entry.defaultMass;
		defaultValues.pressure = entry.defaultPressure;
		switch (entry.state)
		{
		case Element.State.Gas:
			GameTags.GasElements.Add(elem.tag);
			defaultValues.mass = 1f;
			elem.maxMass = 1.8f;
			break;
		case Element.State.Liquid:
			GameTags.LiquidElements.Add(elem.tag);
			break;
		case Element.State.Solid:
			GameTags.SolidElements.Add(elem.tag);
			break;
		}
		elem.defaultValues = defaultValues;
	}

	// Token: 0x0600499B RID: 18843 RVA: 0x001AACC0 File Offset: 0x001A8EC0
	private static bool ManifestSubstanceForElement(Element elem, ref Hashtable substanceList, SubstanceTable substanceTable)
	{
		elem.substance = null;
		if (substanceList.ContainsKey(elem.id))
		{
			elem.substance = (substanceList[elem.id] as Substance);
			return false;
		}
		if (substanceTable != null)
		{
			elem.substance = substanceTable.GetSubstance(elem.id);
		}
		if (elem.substance == null)
		{
			elem.substance = new Substance();
			substanceTable.GetList().Add(elem.substance);
		}
		elem.substance.elementID = elem.id;
		elem.substance.renderedByWorld = elem.IsSolid;
		elem.substance.idx = substanceList.Count;
		if (elem.substance.uiColour == ElementLoader.noColour)
		{
			int count = ElementLoader.elements.Count;
			int idx = elem.substance.idx;
			elem.substance.uiColour = Color.HSVToRGB((float)idx / (float)count, 1f, 1f);
		}
		string name = UI.StripLinkFormatting(elem.name);
		elem.substance.name = name;
		elem.substance.nameTag = elem.tag;
		elem.substance.audioConfig = ElementsAudio.Instance.GetConfigForElement(elem.id);
		substanceList.Add(elem.id, elem.substance);
		return true;
	}

	// Token: 0x0600499C RID: 18844 RVA: 0x001AAE2E File Offset: 0x001A902E
	public static Element FindElementByName(string name)
	{
		return ElementLoader.FindElementByHash((SimHashes)Hash.SDBMLower(name));
	}

	// Token: 0x0600499D RID: 18845 RVA: 0x001AAE3B File Offset: 0x001A903B
	public static Element FindElementByTag(Tag tag)
	{
		return ElementLoader.GetElement(tag);
	}

	// Token: 0x0600499E RID: 18846 RVA: 0x001AAE44 File Offset: 0x001A9044
	public static List<Element> FindElements(Func<Element, bool> filter)
	{
		List<Element> list = new List<Element>();
		foreach (int key in ElementLoader.elementTable.Keys)
		{
			Element element = ElementLoader.elementTable[key];
			if (filter(element))
			{
				list.Add(element);
			}
		}
		return list;
	}

	// Token: 0x0600499F RID: 18847 RVA: 0x001AAEB8 File Offset: 0x001A90B8
	public static Element FindElementByHash(SimHashes hash)
	{
		Element result = null;
		ElementLoader.elementTable.TryGetValue((int)hash, out result);
		return result;
	}

	// Token: 0x060049A0 RID: 18848 RVA: 0x001AAED8 File Offset: 0x001A90D8
	public static ushort GetElementIndex(SimHashes hash)
	{
		Element element = null;
		ElementLoader.elementTable.TryGetValue((int)hash, out element);
		if (element != null)
		{
			return element.idx;
		}
		return ushort.MaxValue;
	}

	// Token: 0x060049A1 RID: 18849 RVA: 0x001AAF04 File Offset: 0x001A9104
	public static Element GetElement(Tag tag)
	{
		Element result;
		ElementLoader.elementTagTable.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x060049A2 RID: 18850 RVA: 0x001AAF20 File Offset: 0x001A9120
	public static SimHashes GetElementID(Tag tag)
	{
		Element element;
		ElementLoader.elementTagTable.TryGetValue(tag, out element);
		if (element != null)
		{
			return element.id;
		}
		return SimHashes.Vacuum;
	}

	// Token: 0x060049A3 RID: 18851 RVA: 0x001AAF4C File Offset: 0x001A914C
	private static SimHashes GetID(int column, int row, string[,] grid, SimHashes defaultValue = SimHashes.Vacuum)
	{
		if (column >= grid.GetLength(0) || row > grid.GetLength(1))
		{
			global::Debug.LogError(string.Format("Could not find element at loc [{0},{1}] grid is only [{2},{3}]", new object[]
			{
				column,
				row,
				grid.GetLength(0),
				grid.GetLength(1)
			}));
			return defaultValue;
		}
		string text = grid[column, row];
		if (text == null || text == "")
		{
			return defaultValue;
		}
		object obj = null;
		try
		{
			obj = Enum.Parse(typeof(SimHashes), text);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(string.Format("Could not find element {0}: {1}", text, ex.ToString()));
			return defaultValue;
		}
		return (SimHashes)obj;
	}

	// Token: 0x060049A4 RID: 18852 RVA: 0x001AB018 File Offset: 0x001A9218
	private static SpawnFXHashes GetSpawnFX(int column, int row, string[,] grid)
	{
		if (column >= grid.GetLength(0) || row > grid.GetLength(1))
		{
			global::Debug.LogError(string.Format("Could not find SpawnFXHashes at loc [{0},{1}] grid is only [{2},{3}]", new object[]
			{
				column,
				row,
				grid.GetLength(0),
				grid.GetLength(1)
			}));
			return SpawnFXHashes.None;
		}
		string text = grid[column, row];
		if (text == null || text == "")
		{
			return SpawnFXHashes.None;
		}
		object obj = null;
		try
		{
			obj = Enum.Parse(typeof(SpawnFXHashes), text);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(string.Format("Could not find FX {0}: {1}", text, ex.ToString()));
			return SpawnFXHashes.None;
		}
		return (SpawnFXHashes)obj;
	}

	// Token: 0x060049A5 RID: 18853 RVA: 0x001AB0E4 File Offset: 0x001A92E4
	private static Tag CreateMaterialCategoryTag(SimHashes element_id, Tag phaseTag, string materialCategoryField)
	{
		if (!string.IsNullOrEmpty(materialCategoryField))
		{
			Tag tag = TagManager.Create(materialCategoryField);
			if (!GameTags.MaterialCategories.Contains(tag) && !GameTags.IgnoredMaterialCategories.Contains(tag))
			{
				global::Debug.LogWarningFormat("Element {0} has category {1}, but that isn't in GameTags.MaterialCategores!", new object[]
				{
					element_id,
					materialCategoryField
				});
			}
			return tag;
		}
		return phaseTag;
	}

	// Token: 0x060049A6 RID: 18854 RVA: 0x001AB13C File Offset: 0x001A933C
	private static Tag[] CreateOreTags(Tag materialCategory, Tag phaseTag, string[] ore_tags_split)
	{
		List<Tag> list = new List<Tag>();
		if (ore_tags_split != null)
		{
			foreach (string text in ore_tags_split)
			{
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(TagManager.Create(text));
				}
			}
		}
		list.Add(phaseTag);
		if (materialCategory.IsValid && !list.Contains(materialCategory))
		{
			list.Add(materialCategory);
		}
		return list.ToArray();
	}

	// Token: 0x060049A7 RID: 18855 RVA: 0x001AB1A0 File Offset: 0x001A93A0
	private static void FinaliseElementsTable(ref Hashtable substanceList)
	{
		foreach (Element element in ElementLoader.elements)
		{
			if (element != null)
			{
				if (element.substance == null)
				{
					global::Debug.LogWarning("Skipping finalise for missing element: " + element.id.ToString());
				}
				else
				{
					global::Debug.Assert(element.substance.nameTag.IsValid);
					if (element.thermalConductivity == 0f)
					{
						element.state |= Element.State.TemperatureInsulated;
					}
					if (element.strength == 0f)
					{
						element.state |= Element.State.Unbreakable;
					}
					if (element.IsSolid)
					{
						Element element2 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
						if (element2 != null)
						{
							element.highTempTransition = element2;
						}
					}
					else if (element.IsLiquid)
					{
						Element element3 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
						if (element3 != null)
						{
							element.highTempTransition = element3;
						}
						Element element4 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
						if (element4 != null)
						{
							element.lowTempTransition = element4;
						}
					}
					else if (element.IsGas)
					{
						Element element5 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
						if (element5 != null)
						{
							element.lowTempTransition = element5;
						}
					}
				}
			}
		}
		ElementLoader.elements = (from e in ElementLoader.elements
		orderby (int)(e.state & Element.State.Solid) descending, e.id
		select e).ToList<Element>();
		for (int i = 0; i < ElementLoader.elements.Count; i++)
		{
			if (ElementLoader.elements[i].substance != null)
			{
				ElementLoader.elements[i].substance.idx = i;
			}
			ElementLoader.elements[i].idx = (ushort)i;
		}
	}

	// Token: 0x060049A8 RID: 18856 RVA: 0x001AB3A8 File Offset: 0x001A95A8
	private static void ValidateElements()
	{
		global::Debug.Log("------ Start Validating Elements ------");
		foreach (Element element in ElementLoader.elements)
		{
			string text = string.Format("{0} ({1})", element.tag.ProperNameStripLink(), element.state);
			if (element.IsLiquid && element.sublimateId != (SimHashes)0)
			{
				global::Debug.Assert(element.sublimateRate == 0f, text + ": Liquids don't use sublimateRate, use offGasPercentage instead.");
				global::Debug.Assert(element.offGasPercentage > 0f, text + ": Missing offGasPercentage");
			}
			if (element.IsSolid && element.sublimateId != (SimHashes)0)
			{
				global::Debug.Assert(element.offGasPercentage == 0f, text + ": Solids don't use offGasPercentage, use sublimateRate instead.");
				global::Debug.Assert(element.sublimateRate > 0f, text + ": Missing sublimationRate");
				global::Debug.Assert(element.sublimateRate * element.sublimateEfficiency > 0.001f, text + ": Sublimation rate and efficiency will result in gas that will be obliterated because its less than 1g. Increase these values and use sublimateProbability if you want a low amount of sublimation");
			}
			if (element.highTempTransition != null && element.highTempTransition.lowTempTransition == element)
			{
				global::Debug.Assert(element.highTemp >= element.highTempTransition.lowTemp, text + ": highTemp is higher than transition element's (" + element.highTempTransition.tag.ProperNameStripLink() + ") lowTemp");
			}
			global::Debug.Assert(element.defaultValues.mass <= element.maxMass, text + ": Default mass should be less than max mass");
			if (false)
			{
				if (element.IsSolid && element.highTempTransition != null && element.highTempTransition.IsLiquid && element.defaultValues.mass > element.highTempTransition.maxMass)
				{
					global::Debug.LogWarning(string.Format("{0} defaultMass {1} > {2}: maxMass {3}", new object[]
					{
						text,
						element.defaultValues.mass,
						element.highTempTransition.tag.ProperNameStripLink(),
						element.highTempTransition.maxMass
					}));
				}
				if (element.defaultValues.mass < element.maxMass && element.IsLiquid)
				{
					global::Debug.LogWarning(string.Format("{0} has defaultMass: {1} and maxMass {2}", element.tag.ProperNameStripLink(), element.defaultValues.mass, element.maxMass));
				}
			}
		}
		global::Debug.Log("------ End Validating Elements ------");
	}

	// Token: 0x040030FE RID: 12542
	public static List<Element> elements;

	// Token: 0x040030FF RID: 12543
	public static Dictionary<int, Element> elementTable;

	// Token: 0x04003100 RID: 12544
	public static Dictionary<Tag, Element> elementTagTable;

	// Token: 0x04003101 RID: 12545
	private static string path = Application.streamingAssetsPath + "/elements/";

	// Token: 0x04003102 RID: 12546
	private static readonly Color noColour = new Color(0f, 0f, 0f, 0f);

	// Token: 0x02001A43 RID: 6723
	public class ElementEntryCollection
	{
		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x0600A4B2 RID: 42162 RVA: 0x003B5AC0 File Offset: 0x003B3CC0
		// (set) Token: 0x0600A4B3 RID: 42163 RVA: 0x003B5AC8 File Offset: 0x003B3CC8
		public ElementLoader.ElementEntry[] elements { get; set; }
	}

	// Token: 0x02001A44 RID: 6724
	public class ElementComposition
	{
		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x0600A4B6 RID: 42166 RVA: 0x003B5AE1 File Offset: 0x003B3CE1
		// (set) Token: 0x0600A4B7 RID: 42167 RVA: 0x003B5AE9 File Offset: 0x003B3CE9
		public string elementID { get; set; }

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x0600A4B8 RID: 42168 RVA: 0x003B5AF2 File Offset: 0x003B3CF2
		// (set) Token: 0x0600A4B9 RID: 42169 RVA: 0x003B5AFA File Offset: 0x003B3CFA
		public float percentage { get; set; }
	}

	// Token: 0x02001A45 RID: 6725
	public class ElementEntry
	{
		// Token: 0x0600A4BA RID: 42170 RVA: 0x003B5B03 File Offset: 0x003B3D03
		public ElementEntry()
		{
			this.lowTemp = 0f;
			this.highTemp = 10000f;
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x0600A4BB RID: 42171 RVA: 0x003B5B21 File Offset: 0x003B3D21
		// (set) Token: 0x0600A4BC RID: 42172 RVA: 0x003B5B29 File Offset: 0x003B3D29
		public string elementId { get; set; }

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x0600A4BD RID: 42173 RVA: 0x003B5B32 File Offset: 0x003B3D32
		// (set) Token: 0x0600A4BE RID: 42174 RVA: 0x003B5B3A File Offset: 0x003B3D3A
		public float specificHeatCapacity { get; set; }

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x0600A4BF RID: 42175 RVA: 0x003B5B43 File Offset: 0x003B3D43
		// (set) Token: 0x0600A4C0 RID: 42176 RVA: 0x003B5B4B File Offset: 0x003B3D4B
		public float thermalConductivity { get; set; }

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x0600A4C1 RID: 42177 RVA: 0x003B5B54 File Offset: 0x003B3D54
		// (set) Token: 0x0600A4C2 RID: 42178 RVA: 0x003B5B5C File Offset: 0x003B3D5C
		public float solidSurfaceAreaMultiplier { get; set; }

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x0600A4C3 RID: 42179 RVA: 0x003B5B65 File Offset: 0x003B3D65
		// (set) Token: 0x0600A4C4 RID: 42180 RVA: 0x003B5B6D File Offset: 0x003B3D6D
		public float liquidSurfaceAreaMultiplier { get; set; }

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x0600A4C5 RID: 42181 RVA: 0x003B5B76 File Offset: 0x003B3D76
		// (set) Token: 0x0600A4C6 RID: 42182 RVA: 0x003B5B7E File Offset: 0x003B3D7E
		public float gasSurfaceAreaMultiplier { get; set; }

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x0600A4C7 RID: 42183 RVA: 0x003B5B87 File Offset: 0x003B3D87
		// (set) Token: 0x0600A4C8 RID: 42184 RVA: 0x003B5B8F File Offset: 0x003B3D8F
		public float defaultMass { get; set; }

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x0600A4C9 RID: 42185 RVA: 0x003B5B98 File Offset: 0x003B3D98
		// (set) Token: 0x0600A4CA RID: 42186 RVA: 0x003B5BA0 File Offset: 0x003B3DA0
		public float defaultTemperature { get; set; }

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600A4CB RID: 42187 RVA: 0x003B5BA9 File Offset: 0x003B3DA9
		// (set) Token: 0x0600A4CC RID: 42188 RVA: 0x003B5BB1 File Offset: 0x003B3DB1
		public float defaultPressure { get; set; }

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x0600A4CD RID: 42189 RVA: 0x003B5BBA File Offset: 0x003B3DBA
		// (set) Token: 0x0600A4CE RID: 42190 RVA: 0x003B5BC2 File Offset: 0x003B3DC2
		public float molarMass { get; set; }

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x0600A4CF RID: 42191 RVA: 0x003B5BCB File Offset: 0x003B3DCB
		// (set) Token: 0x0600A4D0 RID: 42192 RVA: 0x003B5BD3 File Offset: 0x003B3DD3
		public float lightAbsorptionFactor { get; set; }

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x0600A4D1 RID: 42193 RVA: 0x003B5BDC File Offset: 0x003B3DDC
		// (set) Token: 0x0600A4D2 RID: 42194 RVA: 0x003B5BE4 File Offset: 0x003B3DE4
		public float radiationAbsorptionFactor { get; set; }

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x0600A4D3 RID: 42195 RVA: 0x003B5BED File Offset: 0x003B3DED
		// (set) Token: 0x0600A4D4 RID: 42196 RVA: 0x003B5BF5 File Offset: 0x003B3DF5
		public float radiationPer1000Mass { get; set; }

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x0600A4D5 RID: 42197 RVA: 0x003B5BFE File Offset: 0x003B3DFE
		// (set) Token: 0x0600A4D6 RID: 42198 RVA: 0x003B5C06 File Offset: 0x003B3E06
		public string lowTempTransitionTarget { get; set; }

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x0600A4D7 RID: 42199 RVA: 0x003B5C0F File Offset: 0x003B3E0F
		// (set) Token: 0x0600A4D8 RID: 42200 RVA: 0x003B5C17 File Offset: 0x003B3E17
		public float lowTemp { get; set; }

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x0600A4D9 RID: 42201 RVA: 0x003B5C20 File Offset: 0x003B3E20
		// (set) Token: 0x0600A4DA RID: 42202 RVA: 0x003B5C28 File Offset: 0x003B3E28
		public string highTempTransitionTarget { get; set; }

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x0600A4DB RID: 42203 RVA: 0x003B5C31 File Offset: 0x003B3E31
		// (set) Token: 0x0600A4DC RID: 42204 RVA: 0x003B5C39 File Offset: 0x003B3E39
		public float highTemp { get; set; }

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x0600A4DD RID: 42205 RVA: 0x003B5C42 File Offset: 0x003B3E42
		// (set) Token: 0x0600A4DE RID: 42206 RVA: 0x003B5C4A File Offset: 0x003B3E4A
		public string lowTempTransitionOreId { get; set; }

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x0600A4DF RID: 42207 RVA: 0x003B5C53 File Offset: 0x003B3E53
		// (set) Token: 0x0600A4E0 RID: 42208 RVA: 0x003B5C5B File Offset: 0x003B3E5B
		public float lowTempTransitionOreMassConversion { get; set; }

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x0600A4E1 RID: 42209 RVA: 0x003B5C64 File Offset: 0x003B3E64
		// (set) Token: 0x0600A4E2 RID: 42210 RVA: 0x003B5C6C File Offset: 0x003B3E6C
		public string highTempTransitionOreId { get; set; }

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600A4E3 RID: 42211 RVA: 0x003B5C75 File Offset: 0x003B3E75
		// (set) Token: 0x0600A4E4 RID: 42212 RVA: 0x003B5C7D File Offset: 0x003B3E7D
		public float highTempTransitionOreMassConversion { get; set; }

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x0600A4E5 RID: 42213 RVA: 0x003B5C86 File Offset: 0x003B3E86
		// (set) Token: 0x0600A4E6 RID: 42214 RVA: 0x003B5C8E File Offset: 0x003B3E8E
		public string sublimateId { get; set; }

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x0600A4E7 RID: 42215 RVA: 0x003B5C97 File Offset: 0x003B3E97
		// (set) Token: 0x0600A4E8 RID: 42216 RVA: 0x003B5C9F File Offset: 0x003B3E9F
		public string sublimateFx { get; set; }

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x0600A4E9 RID: 42217 RVA: 0x003B5CA8 File Offset: 0x003B3EA8
		// (set) Token: 0x0600A4EA RID: 42218 RVA: 0x003B5CB0 File Offset: 0x003B3EB0
		public float sublimateRate { get; set; }

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x0600A4EB RID: 42219 RVA: 0x003B5CB9 File Offset: 0x003B3EB9
		// (set) Token: 0x0600A4EC RID: 42220 RVA: 0x003B5CC1 File Offset: 0x003B3EC1
		public float sublimateEfficiency { get; set; }

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x0600A4ED RID: 42221 RVA: 0x003B5CCA File Offset: 0x003B3ECA
		// (set) Token: 0x0600A4EE RID: 42222 RVA: 0x003B5CD2 File Offset: 0x003B3ED2
		public float sublimateProbability { get; set; }

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x0600A4EF RID: 42223 RVA: 0x003B5CDB File Offset: 0x003B3EDB
		// (set) Token: 0x0600A4F0 RID: 42224 RVA: 0x003B5CE3 File Offset: 0x003B3EE3
		public float offGasPercentage { get; set; }

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x0600A4F1 RID: 42225 RVA: 0x003B5CEC File Offset: 0x003B3EEC
		// (set) Token: 0x0600A4F2 RID: 42226 RVA: 0x003B5CF4 File Offset: 0x003B3EF4
		public string materialCategory { get; set; }

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x0600A4F3 RID: 42227 RVA: 0x003B5CFD File Offset: 0x003B3EFD
		// (set) Token: 0x0600A4F4 RID: 42228 RVA: 0x003B5D05 File Offset: 0x003B3F05
		public string[] tags { get; set; }

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x0600A4F5 RID: 42229 RVA: 0x003B5D0E File Offset: 0x003B3F0E
		// (set) Token: 0x0600A4F6 RID: 42230 RVA: 0x003B5D16 File Offset: 0x003B3F16
		public bool isDisabled { get; set; }

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x0600A4F7 RID: 42231 RVA: 0x003B5D1F File Offset: 0x003B3F1F
		// (set) Token: 0x0600A4F8 RID: 42232 RVA: 0x003B5D27 File Offset: 0x003B3F27
		public float strength { get; set; }

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x0600A4F9 RID: 42233 RVA: 0x003B5D30 File Offset: 0x003B3F30
		// (set) Token: 0x0600A4FA RID: 42234 RVA: 0x003B5D38 File Offset: 0x003B3F38
		public float maxMass { get; set; }

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x0600A4FB RID: 42235 RVA: 0x003B5D41 File Offset: 0x003B3F41
		// (set) Token: 0x0600A4FC RID: 42236 RVA: 0x003B5D49 File Offset: 0x003B3F49
		public byte hardness { get; set; }

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x0600A4FD RID: 42237 RVA: 0x003B5D52 File Offset: 0x003B3F52
		// (set) Token: 0x0600A4FE RID: 42238 RVA: 0x003B5D5A File Offset: 0x003B3F5A
		public float toxicity { get; set; }

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x0600A4FF RID: 42239 RVA: 0x003B5D63 File Offset: 0x003B3F63
		// (set) Token: 0x0600A500 RID: 42240 RVA: 0x003B5D6B File Offset: 0x003B3F6B
		public float liquidCompression { get; set; }

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x0600A501 RID: 42241 RVA: 0x003B5D74 File Offset: 0x003B3F74
		// (set) Token: 0x0600A502 RID: 42242 RVA: 0x003B5D7C File Offset: 0x003B3F7C
		public float speed { get; set; }

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x0600A503 RID: 42243 RVA: 0x003B5D85 File Offset: 0x003B3F85
		// (set) Token: 0x0600A504 RID: 42244 RVA: 0x003B5D8D File Offset: 0x003B3F8D
		public float minHorizontalFlow { get; set; }

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600A505 RID: 42245 RVA: 0x003B5D96 File Offset: 0x003B3F96
		// (set) Token: 0x0600A506 RID: 42246 RVA: 0x003B5D9E File Offset: 0x003B3F9E
		public float minVerticalFlow { get; set; }

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x0600A507 RID: 42247 RVA: 0x003B5DA7 File Offset: 0x003B3FA7
		// (set) Token: 0x0600A508 RID: 42248 RVA: 0x003B5DAF File Offset: 0x003B3FAF
		public string convertId { get; set; }

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x0600A509 RID: 42249 RVA: 0x003B5DB8 File Offset: 0x003B3FB8
		// (set) Token: 0x0600A50A RID: 42250 RVA: 0x003B5DC0 File Offset: 0x003B3FC0
		public float flow { get; set; }

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x0600A50B RID: 42251 RVA: 0x003B5DC9 File Offset: 0x003B3FC9
		// (set) Token: 0x0600A50C RID: 42252 RVA: 0x003B5DD1 File Offset: 0x003B3FD1
		public int buildMenuSort { get; set; }

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x0600A50D RID: 42253 RVA: 0x003B5DDA File Offset: 0x003B3FDA
		// (set) Token: 0x0600A50E RID: 42254 RVA: 0x003B5DE2 File Offset: 0x003B3FE2
		public Element.State state { get; set; }

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x0600A50F RID: 42255 RVA: 0x003B5DEB File Offset: 0x003B3FEB
		// (set) Token: 0x0600A510 RID: 42256 RVA: 0x003B5DF3 File Offset: 0x003B3FF3
		public string localizationID { get; set; }

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x0600A511 RID: 42257 RVA: 0x003B5DFC File Offset: 0x003B3FFC
		// (set) Token: 0x0600A512 RID: 42258 RVA: 0x003B5E04 File Offset: 0x003B4004
		public string dlcId { get; set; }

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x0600A513 RID: 42259 RVA: 0x003B5E0D File Offset: 0x003B400D
		// (set) Token: 0x0600A514 RID: 42260 RVA: 0x003B5E15 File Offset: 0x003B4015
		public string refinedMetalTarget { get; set; }

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x0600A515 RID: 42261 RVA: 0x003B5E1E File Offset: 0x003B401E
		// (set) Token: 0x0600A516 RID: 42262 RVA: 0x003B5E26 File Offset: 0x003B4026
		public ElementLoader.ElementComposition[] composition { get; set; }

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x0600A517 RID: 42263 RVA: 0x003B5E2F File Offset: 0x003B402F
		// (set) Token: 0x0600A518 RID: 42264 RVA: 0x003B5E5A File Offset: 0x003B405A
		public string description
		{
			get
			{
				return this.description_backing ?? ("STRINGS.ELEMENTS." + this.elementId.ToString().ToUpper() + ".DESC");
			}
			set
			{
				this.description_backing = value;
			}
		}

		// Token: 0x04008135 RID: 33077
		private string description_backing;
	}
}
