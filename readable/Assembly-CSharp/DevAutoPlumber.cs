using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200073F RID: 1855
public class DevAutoPlumber
{
	// Token: 0x06002EB5 RID: 11957 RVA: 0x0010DB9B File Offset: 0x0010BD9B
	public static void AutoPlumbBuilding(Building building)
	{
		DevAutoPlumber.DoElectricalPlumbing(building);
		DevAutoPlumber.DoLiquidAndGasPlumbing(building);
		DevAutoPlumber.SetupSolidOreDelivery(building);
	}

	// Token: 0x06002EB6 RID: 11958 RVA: 0x0010DBB0 File Offset: 0x0010BDB0
	public static void DoElectricalPlumbing(Building building)
	{
		if (!building.Def.RequiresPowerInput)
		{
			return;
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(building), building.Def.PowerInputOffset);
		GameObject gameObject = Grid.Objects[cell, 26];
		if (gameObject != null)
		{
			gameObject.Trigger(-790448070, null);
		}
		DevAutoPlumber.PlaceSourceAndUtilityConduit(building, Assets.GetBuildingDef("DevGenerator"), Assets.GetBuildingDef("WireRefined"), Game.Instance.electricalConduitSystem, new int[]
		{
			26,
			29
		}, DevAutoPlumber.PortSelection.PowerInput);
	}

	// Token: 0x06002EB7 RID: 11959 RVA: 0x0010DC3B File Offset: 0x0010BE3B
	public static void DoLiquidAndGasPlumbing(Building building)
	{
		DevAutoPlumber.SetupPlumbingInput(building);
		DevAutoPlumber.SetupPlumbingOutput(building);
	}

	// Token: 0x06002EB8 RID: 11960 RVA: 0x0010DC4C File Offset: 0x0010BE4C
	public static void SetupSolidOreDelivery(Building building)
	{
		ManualDeliveryKG component = building.GetComponent<ManualDeliveryKG>();
		if (component != null)
		{
			DevAutoPlumber.TrySpawnElementOreFromTag(component.RequestedItemTag, Grid.PosToCell(building), component.Capacity) == null;
			return;
		}
		foreach (ComplexRecipe complexRecipe in ComplexRecipeManager.Get().recipes)
		{
			using (List<Tag>.Enumerator enumerator2 = complexRecipe.fabricators.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == building.Def.PrefabID)
					{
						foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
						{
							DevAutoPlumber.TrySpawnElementOreFromTag(recipeElement.material, Grid.PosToCell(building), recipeElement.amount * 10f) == null;
						}
					}
				}
			}
		}
	}

	// Token: 0x06002EB9 RID: 11961 RVA: 0x0010DD6C File Offset: 0x0010BF6C
	private static GameObject TrySpawnElementOreFromTag(Tag t, int cell, float amount)
	{
		Element element = ElementLoader.GetElement(t);
		if (element == null)
		{
			element = ElementLoader.elements.Find((Element match) => match.HasTag(t));
		}
		if (element != null)
		{
			return element.substance.SpawnResource(Grid.CellToPos(cell), amount, element.defaultValues.temperature, byte.MaxValue, 0, false, false, false);
		}
		return null;
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x0010DDD8 File Offset: 0x0010BFD8
	private static void SetupPlumbingInput(Building building)
	{
		ConduitConsumer component = building.GetComponent<ConduitConsumer>();
		if (component == null)
		{
			return;
		}
		BuildingDef sourceDef = null;
		BuildingDef conduitDef = null;
		int[] conduitTypeLayers = null;
		UtilityNetworkManager<FlowUtilityNetwork, Vent> utlityNetworkManager = null;
		ConduitType conduitType = component.ConduitType;
		if (conduitType != ConduitType.Gas)
		{
			if (conduitType == ConduitType.Liquid)
			{
				conduitDef = Assets.GetBuildingDef("InsulatedLiquidConduit");
				sourceDef = Assets.GetBuildingDef("DevPumpLiquid");
				utlityNetworkManager = Game.Instance.liquidConduitSystem;
				conduitTypeLayers = new int[]
				{
					16,
					19
				};
			}
		}
		else
		{
			conduitDef = Assets.GetBuildingDef("InsulatedGasConduit");
			sourceDef = Assets.GetBuildingDef("DevPumpGas");
			utlityNetworkManager = Game.Instance.gasConduitSystem;
			conduitTypeLayers = new int[]
			{
				12,
				15
			};
		}
		GameObject gameObject = DevAutoPlumber.PlaceSourceAndUtilityConduit(building, sourceDef, conduitDef, utlityNetworkManager, conduitTypeLayers, DevAutoPlumber.PortSelection.UtilityInput);
		Element element = DevAutoPlumber.GuessMostRelevantElementForPump(building);
		if (element != null)
		{
			gameObject.GetComponent<DevPump>().SelectedTag = element.tag;
			return;
		}
		gameObject.GetComponent<DevPump>().SelectedTag = ElementLoader.FindElementByHash(SimHashes.Vacuum).tag;
	}

	// Token: 0x06002EBB RID: 11963 RVA: 0x0010DEC4 File Offset: 0x0010C0C4
	private static void SetupPlumbingOutput(Building building)
	{
		ConduitDispenser component = building.GetComponent<ConduitDispenser>();
		if (component == null)
		{
			return;
		}
		BuildingDef sourceDef = null;
		BuildingDef conduitDef = null;
		int[] conduitTypeLayers = null;
		UtilityNetworkManager<FlowUtilityNetwork, Vent> utlityNetworkManager = null;
		ConduitType conduitType = component.ConduitType;
		if (conduitType != ConduitType.Gas)
		{
			if (conduitType == ConduitType.Liquid)
			{
				conduitDef = Assets.GetBuildingDef("InsulatedLiquidConduit");
				sourceDef = Assets.GetBuildingDef("LiquidVent");
				utlityNetworkManager = Game.Instance.liquidConduitSystem;
				conduitTypeLayers = new int[]
				{
					16,
					19
				};
			}
		}
		else
		{
			conduitDef = Assets.GetBuildingDef("InsulatedGasConduit");
			sourceDef = Assets.GetBuildingDef("GasVent");
			utlityNetworkManager = Game.Instance.gasConduitSystem;
			conduitTypeLayers = new int[]
			{
				12,
				15
			};
		}
		DevAutoPlumber.PlaceSourceAndUtilityConduit(building, sourceDef, conduitDef, utlityNetworkManager, conduitTypeLayers, DevAutoPlumber.PortSelection.UtilityOutput);
	}

	// Token: 0x06002EBC RID: 11964 RVA: 0x0010DF74 File Offset: 0x0010C174
	private static Element GuessMostRelevantElementForPump(Building destinationBuilding)
	{
		ConduitConsumer consumer = destinationBuilding.GetComponent<ConduitConsumer>();
		Tag targetTag = destinationBuilding.GetComponent<ConduitConsumer>().capacityTag;
		ElementConverter elementConverter = destinationBuilding.GetComponent<ElementConverter>();
		ElementConsumer elementConsumer = destinationBuilding.GetComponent<ElementConsumer>();
		RocketEngineCluster rocketEngineCluster = destinationBuilding.GetComponent<RocketEngineCluster>();
		return ElementLoader.elements.Find(delegate(Element match)
		{
			if (elementConverter != null)
			{
				bool flag = false;
				for (int i = 0; i < elementConverter.consumedElements.Length; i++)
				{
					if (elementConverter.consumedElements[i].Tag == match.tag)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			else if (elementConsumer != null)
			{
				bool flag2 = false;
				if (ElementLoader.FindElementByHash(elementConsumer.elementToConsume).tag == match.tag)
				{
					flag2 = true;
				}
				if (!flag2)
				{
					return false;
				}
			}
			else if (rocketEngineCluster != null)
			{
				bool flag3 = false;
				if (rocketEngineCluster.fuelTag == match.tag)
				{
					flag3 = true;
				}
				if (!flag3)
				{
					return false;
				}
			}
			return (consumer.ConduitType != ConduitType.Liquid || match.IsLiquid) && (consumer.ConduitType != ConduitType.Gas || match.IsGas) && (match.HasTag(targetTag) || !(targetTag != GameTags.Any));
		});
	}

	// Token: 0x06002EBD RID: 11965 RVA: 0x0010DFE0 File Offset: 0x0010C1E0
	private static GameObject PlaceSourceAndUtilityConduit(Building destinationBuilding, BuildingDef sourceDef, BuildingDef conduitDef, IUtilityNetworkMgr utlityNetworkManager, int[] conduitTypeLayers, DevAutoPlumber.PortSelection portSelection)
	{
		Building building = null;
		List<int> list = new List<int>();
		int cell = DevAutoPlumber.FindClearPlacementLocation(Grid.PosToCell(destinationBuilding), new List<int>(conduitTypeLayers)
		{
			1
		}.ToArray(), list);
		bool flag = false;
		int num = 10;
		while (!flag)
		{
			num--;
			building = DevAutoPlumber.PlaceConduitSourceBuilding(cell, sourceDef);
			if (building == null)
			{
				return null;
			}
			List<int> list2 = DevAutoPlumber.GenerateClearConduitPath(building, destinationBuilding, conduitTypeLayers, portSelection);
			if (list2 == null)
			{
				list.Add(Grid.PosToCell(building));
				building.Trigger(-790448070, null);
			}
			else
			{
				flag = true;
				DevAutoPlumber.BuildConduits(list2, conduitDef, utlityNetworkManager);
			}
		}
		return building.gameObject;
	}

	// Token: 0x06002EBE RID: 11966 RVA: 0x0010E07C File Offset: 0x0010C27C
	private static int FindClearPlacementLocation(int nearStartingCell, int[] placementBlockingObjectLayers, List<int> rejectLocations)
	{
		Func<int, object, bool> fn = delegate(int test, object unusedData)
		{
			foreach (int num in new int[]
			{
				test,
				Grid.OffsetCell(test, 1, 0),
				Grid.OffsetCell(test, 1, -1),
				Grid.OffsetCell(test, 0, -1),
				Grid.OffsetCell(test, 0, 1),
				Grid.OffsetCell(test, 1, 1)
			})
			{
				if (!Grid.IsValidCell(num))
				{
					return false;
				}
				if (Grid.Solid[num])
				{
					return false;
				}
				if (Grid.ObjectLayers[1].ContainsKey(num))
				{
					return false;
				}
				foreach (int num2 in placementBlockingObjectLayers)
				{
					if (Grid.ObjectLayers[num2].ContainsKey(num))
					{
						return false;
					}
				}
				if (rejectLocations.Contains(test))
				{
					return false;
				}
			}
			return true;
		};
		int max_depth = 20;
		return GameUtil.FloodFillFind<object>(fn, null, nearStartingCell, max_depth, false, false);
	}

	// Token: 0x06002EBF RID: 11967 RVA: 0x0010E0B4 File Offset: 0x0010C2B4
	private static List<int> GenerateClearConduitPath(Building sourceBuilding, Building destinationBuilding, int[] conduitTypeLayers, DevAutoPlumber.PortSelection portSelection)
	{
		new List<int>();
		if (sourceBuilding == null)
		{
			return null;
		}
		int conduitStart = -1;
		int conduitEnd = -1;
		switch (portSelection)
		{
		case DevAutoPlumber.PortSelection.UtilityInput:
			conduitStart = Grid.OffsetCell(Grid.PosToCell(sourceBuilding), sourceBuilding.Def.UtilityOutputOffset);
			conduitEnd = Grid.OffsetCell(Grid.PosToCell(destinationBuilding), destinationBuilding.Def.UtilityInputOffset);
			break;
		case DevAutoPlumber.PortSelection.UtilityOutput:
			conduitStart = Grid.OffsetCell(Grid.PosToCell(destinationBuilding), destinationBuilding.Def.UtilityOutputOffset);
			conduitEnd = Grid.OffsetCell(Grid.PosToCell(sourceBuilding), sourceBuilding.Def.UtilityInputOffset);
			break;
		case DevAutoPlumber.PortSelection.PowerInput:
			conduitStart = Grid.OffsetCell(Grid.PosToCell(sourceBuilding), sourceBuilding.Def.PowerOutputOffset);
			conduitEnd = Grid.OffsetCell(Grid.PosToCell(destinationBuilding), destinationBuilding.Def.PowerInputOffset);
			break;
		}
		return DevAutoPlumber.GetGridPath(conduitStart, conduitEnd, delegate(int cell)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			foreach (int layer in conduitTypeLayers)
			{
				GameObject x = Grid.Objects[cell, layer];
				bool flag = x == sourceBuilding.gameObject || x == destinationBuilding.gameObject;
				bool flag2 = cell == conduitEnd || cell == conduitStart;
				if (x != null && (!flag || (flag && !flag2)))
				{
					return false;
				}
			}
			return true;
		}, 20);
	}

	// Token: 0x06002EC0 RID: 11968 RVA: 0x0010E224 File Offset: 0x0010C424
	private static Building PlaceConduitSourceBuilding(int cell, BuildingDef def)
	{
		List<Tag> selected_elements = new List<Tag>
		{
			SimHashes.Cuprite.CreateTag()
		};
		return def.Build(cell, Orientation.Neutral, null, selected_elements, 273.15f, true, GameClock.Instance.GetTime()).GetComponent<Building>();
	}

	// Token: 0x06002EC1 RID: 11969 RVA: 0x0010E268 File Offset: 0x0010C468
	private static void BuildConduits(List<int> path, BuildingDef conduitDef, object utilityNetwork)
	{
		List<Tag> selected_elements = new List<Tag>
		{
			SimHashes.Cuprite.CreateTag()
		};
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < path.Count; i++)
		{
			list.Add(conduitDef.Build(path[i], Orientation.Neutral, null, selected_elements, 273.15f, true, GameClock.Instance.GetTime()));
		}
		if (list.Count < 2)
		{
			return;
		}
		IUtilityNetworkMgr utilityNetworkMgr = (IUtilityNetworkMgr)utilityNetwork;
		for (int j = 1; j < list.Count; j++)
		{
			UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(Grid.PosToCell(list[j - 1]), Grid.PosToCell(list[j]));
			utilityNetworkMgr.AddConnection(utilityConnections, Grid.PosToCell(list[j - 1]), true);
			utilityNetworkMgr.AddConnection(utilityConnections.InverseDirection(), Grid.PosToCell(list[j]), true);
			IUtilityItem component = list[j].GetComponent<KAnimGraphTileVisualizer>();
			if (component != null)
			{
				component.UpdateConnections(utilityNetworkMgr.GetConnections(Grid.PosToCell(list[j]), true));
			}
		}
	}

	// Token: 0x06002EC2 RID: 11970 RVA: 0x0010E378 File Offset: 0x0010C578
	private static List<int> GetGridPath(int startCell, int endCell, Func<int, bool> testFunction, int maxDepth = 20)
	{
		DevAutoPlumber.<>c__DisplayClass14_0 CS$<>8__locals1;
		CS$<>8__locals1.testFunction = testFunction;
		CS$<>8__locals1.endCell = endCell;
		List<int> list = new List<int>();
		CS$<>8__locals1.frontier = new List<int>();
		CS$<>8__locals1.touched = new List<int>();
		CS$<>8__locals1.crumbs = new Dictionary<int, int>();
		CS$<>8__locals1.frontier.Add(startCell);
		CS$<>8__locals1.newFrontier = new List<int>();
		int num = 0;
		while (!CS$<>8__locals1.touched.Contains(CS$<>8__locals1.endCell))
		{
			num++;
			if (num > maxDepth || CS$<>8__locals1.frontier.Count == 0)
			{
				break;
			}
			foreach (int fromCell in CS$<>8__locals1.frontier)
			{
				DevAutoPlumber.<GetGridPath>g___ExpandFrontier|14_0(fromCell, ref CS$<>8__locals1);
			}
			CS$<>8__locals1.frontier.Clear();
			foreach (int item in CS$<>8__locals1.newFrontier)
			{
				CS$<>8__locals1.frontier.Add(item);
			}
			CS$<>8__locals1.newFrontier.Clear();
		}
		int num2 = CS$<>8__locals1.endCell;
		list.Add(num2);
		while (CS$<>8__locals1.crumbs.ContainsKey(num2))
		{
			num2 = CS$<>8__locals1.crumbs[num2];
			list.Add(num2);
		}
		list.Reverse();
		return list;
	}

	// Token: 0x06002EC4 RID: 11972 RVA: 0x0010E4F8 File Offset: 0x0010C6F8
	[CompilerGenerated]
	internal static void <GetGridPath>g___ExpandFrontier|14_0(int fromCell, ref DevAutoPlumber.<>c__DisplayClass14_0 A_1)
	{
		foreach (int num in new int[]
		{
			Grid.CellAbove(fromCell),
			Grid.CellBelow(fromCell),
			Grid.CellLeft(fromCell),
			Grid.CellRight(fromCell)
		})
		{
			if (!A_1.newFrontier.Contains(num) && !A_1.frontier.Contains(num) && !A_1.touched.Contains(num) && A_1.testFunction(num))
			{
				A_1.newFrontier.Add(num);
				A_1.crumbs.Add(num, fromCell);
			}
			A_1.touched.Add(num);
			if (num == A_1.endCell)
			{
				break;
			}
		}
		A_1.touched.Add(fromCell);
	}

	// Token: 0x02001617 RID: 5655
	private enum PortSelection
	{
		// Token: 0x040073B9 RID: 29625
		UtilityInput,
		// Token: 0x040073BA RID: 29626
		UtilityOutput,
		// Token: 0x040073BB RID: 29627
		PowerInput
	}
}
