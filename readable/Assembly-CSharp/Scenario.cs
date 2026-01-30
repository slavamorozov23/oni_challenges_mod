using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000B2A RID: 2858
[AddComponentMenu("KMonoBehaviour/scripts/Scenario")]
public class Scenario : KMonoBehaviour
{
	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x060053D4 RID: 21460 RVA: 0x001E9787 File Offset: 0x001E7987
	// (set) Token: 0x060053D5 RID: 21461 RVA: 0x001E978F File Offset: 0x001E798F
	public bool[] ReplaceElementMask { get; set; }

	// Token: 0x060053D6 RID: 21462 RVA: 0x001E9798 File Offset: 0x001E7998
	public static void DestroyInstance()
	{
		Scenario.Instance = null;
	}

	// Token: 0x060053D7 RID: 21463 RVA: 0x001E97A0 File Offset: 0x001E79A0
	protected override void OnPrefabInit()
	{
		Scenario.Instance = this;
		SaveLoader instance = SaveLoader.Instance;
		instance.OnWorldGenComplete = (Action<Cluster>)Delegate.Combine(instance.OnWorldGenComplete, new Action<Cluster>(this.OnWorldGenComplete));
	}

	// Token: 0x060053D8 RID: 21464 RVA: 0x001E97CE File Offset: 0x001E79CE
	private void OnWorldGenComplete(Cluster clusterLayout)
	{
		this.Init();
	}

	// Token: 0x060053D9 RID: 21465 RVA: 0x001E97D8 File Offset: 0x001E79D8
	private void Init()
	{
		this.Bot = Grid.HeightInCells / 4;
		this.Left = 150;
		this.RootCell = Grid.OffsetCell(0, this.Left, this.Bot);
		this.ReplaceElementMask = new bool[Grid.CellCount];
	}

	// Token: 0x060053DA RID: 21466 RVA: 0x001E9828 File Offset: 0x001E7A28
	private void DigHole(int x, int y, int width, int height)
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y + j), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 200f, -1f, byte.MaxValue, 0, -1);
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y + j), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + width, y + j), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			}
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y - 1), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y + height), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
		}
	}

	// Token: 0x060053DB RID: 21467 RVA: 0x001E9967 File Offset: 0x001E7B67
	private void Fill(int x, int y, SimHashes id = SimHashes.Ice)
	{
		SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y), id, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
	}

	// Token: 0x060053DC RID: 21468 RVA: 0x001E9998 File Offset: 0x001E7B98
	private void PlaceColumn(int x, int y, int height)
	{
		for (int i = 0; i < height; i++)
		{
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y + i), SimHashes.Ice, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
		}
	}

	// Token: 0x060053DD RID: 21469 RVA: 0x001E99E8 File Offset: 0x001E7BE8
	private void PlaceTileX(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(left + i, bot, "Tile", SimHashes.Cuprite);
		}
	}

	// Token: 0x060053DE RID: 21470 RVA: 0x001E9A18 File Offset: 0x001E7C18
	private void PlaceTileY(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(left, bot + i, "Tile", SimHashes.Cuprite);
		}
	}

	// Token: 0x060053DF RID: 21471 RVA: 0x001E9A46 File Offset: 0x001E7C46
	private void Clear(int x, int y)
	{
		SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
	}

	// Token: 0x060053E0 RID: 21472 RVA: 0x001E9A7C File Offset: 0x001E7C7C
	private void PlacerLadder(int x, int y, int amount)
	{
		int num = 1;
		if (amount < 0)
		{
			amount = -amount;
			num = -1;
		}
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(x, y + i * num, "Ladder", SimHashes.Cuprite);
		}
	}

	// Token: 0x060053E1 RID: 21473 RVA: 0x001E9AB8 File Offset: 0x001E7CB8
	private void PlaceBuildings(int left, int bot)
	{
		this.PlaceBuilding(++left, bot, "ManualGenerator", SimHashes.Iron);
		this.PlaceBuilding(left += 2, bot, "OxygenMachine", SimHashes.Steel);
		this.PlaceBuilding(left += 2, bot, "SpaceHeater", SimHashes.Steel);
		this.PlaceBuilding(++left, bot, "Electrolyzer", SimHashes.Steel);
		this.PlaceBuilding(++left, bot, "Smelter", SimHashes.Steel);
		this.SpawnOre(left, bot + 1, SimHashes.Ice);
	}

	// Token: 0x060053E2 RID: 21474 RVA: 0x001E9B4C File Offset: 0x001E7D4C
	private IEnumerator TurnOn(GameObject go)
	{
		yield return null;
		yield return null;
		go.GetComponent<BuildingEnabledButton>().IsEnabled = true;
		yield break;
	}

	// Token: 0x060053E3 RID: 21475 RVA: 0x001E9B5C File Offset: 0x001E7D5C
	private void SetupPlacerTest(Scenario.Builder b, Element element)
	{
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (buildingDef.Name != "Excavator")
			{
				b.Placer(buildingDef.PrefabID, element);
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053E4 RID: 21476 RVA: 0x001E9BD8 File Offset: 0x001E7DD8
	private void SetupBuildingTest(Scenario.RowLayout row_layout, bool is_powered, bool break_building)
	{
		Scenario.Builder builder = null;
		int num = 0;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (builder == null)
			{
				builder = row_layout.NextRow();
				num = this.Left;
				if (is_powered)
				{
					builder.Minion(null);
					builder.Minion(null);
				}
			}
			if (buildingDef.Name != "Excavator")
			{
				GameObject gameObject = builder.Building(buildingDef.PrefabID);
				if (break_building)
				{
					BuildingHP component = gameObject.GetComponent<BuildingHP>();
					if (component != null)
					{
						component.DoDamage(int.MaxValue);
					}
				}
			}
			if (builder.Left > num + 100)
			{
				builder.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
				builder = null;
			}
		}
		builder.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053E5 RID: 21477 RVA: 0x001E9CC4 File Offset: 0x001E7EC4
	private IEnumerator RunAfterNextUpdateRoutine(System.Action action)
	{
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		action();
		yield break;
	}

	// Token: 0x060053E6 RID: 21478 RVA: 0x001E9CD3 File Offset: 0x001E7ED3
	private void RunAfterNextUpdate(System.Action action)
	{
		base.StartCoroutine(this.RunAfterNextUpdateRoutine(action));
	}

	// Token: 0x060053E7 RID: 21479 RVA: 0x001E9CE4 File Offset: 0x001E7EE4
	public void SetupFabricatorTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("ManualGenerator");
		b.Ore(3, SimHashes.Cuprite);
		b.Minion(null);
		b.Building("Masonry");
		b.InAndOuts();
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053E8 RID: 21480 RVA: 0x001E9D3A File Offset: 0x001E7F3A
	public void SetupDoorTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(1, 0);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053E9 RID: 21481 RVA: 0x001E9D74 File Offset: 0x001E7F74
	public void SetupHatchTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053EA RID: 21482 RVA: 0x001E9DA6 File Offset: 0x001E7FA6
	public void SetupPropaneGeneratorTest(Scenario.Builder b)
	{
		b.Building("PropaneGenerator");
		b.Building("OxygenMachine");
		b.FinalizeRoom(SimHashes.Propane, SimHashes.Steel);
	}

	// Token: 0x060053EB RID: 21483 RVA: 0x001E9DD4 File Offset: 0x001E7FD4
	public void SetupAirLockTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(1, 0);
		b.Minion(null);
		b.Jump(1, 0);
		b.Building("PoweredAirlock");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053EC RID: 21484 RVA: 0x001E9E28 File Offset: 0x001E8028
	public void SetupBedTest(Scenario.Builder b)
	{
		b.Minion(delegate(GameObject go)
		{
			go.GetAmounts().SetValue("Stamina", 10f);
		});
		b.Building("ManualGenerator");
		b.Minion(delegate(GameObject go)
		{
			go.GetAmounts().SetValue("Stamina", 10f);
		});
		b.Building("ComfyBed");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053ED RID: 21485 RVA: 0x001E9EA8 File Offset: 0x001E80A8
	public void SetupHexapedTest(Scenario.Builder b)
	{
		b.Fill(4, 4, SimHashes.Oxygen);
		b.Prefab("Hexaped", null);
		b.Jump(2, 0);
		b.Ore(1, SimHashes.Iron);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053EE RID: 21486 RVA: 0x001E9EE8 File Offset: 0x001E80E8
	public void SetupElectrolyzerTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("ManualGenerator");
		b.Ore(3, SimHashes.Ice);
		b.Minion(null);
		b.Building("Electrolyzer");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053EF RID: 21487 RVA: 0x001E9F38 File Offset: 0x001E8138
	public void SetupOrePerformanceTest(Scenario.Builder b)
	{
		int num = 20;
		int num2 = 20;
		int left = b.Left;
		int bot = b.Bot;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j += 2)
			{
				b.Jump(i, j);
				b.Ore(1, SimHashes.Cuprite);
				b.JumpTo(left, bot);
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053F0 RID: 21488 RVA: 0x001E9FA8 File Offset: 0x001E81A8
	public void SetupFeedingTest(Scenario.Builder b)
	{
		b.FillOffsets(SimHashes.IgneousRock, new int[]
		{
			1,
			0,
			3,
			0,
			3,
			1,
			5,
			0,
			5,
			1,
			5,
			2,
			7,
			0,
			7,
			1,
			7,
			2,
			9,
			0,
			9,
			1,
			11,
			0
		});
		b.PrefabOffsets("Hexaped", new int[]
		{
			0,
			0,
			2,
			0,
			4,
			0,
			7,
			3,
			9,
			2,
			11,
			1
		});
		b.OreOffsets(1, SimHashes.IronOre, new int[]
		{
			1,
			1,
			3,
			2,
			5,
			3,
			8,
			0,
			10,
			0,
			12,
			0
		});
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053F1 RID: 21489 RVA: 0x001EA020 File Offset: 0x001E8220
	public void SetupLiquifierTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Minion(null);
		b.Ore(2, SimHashes.Ice);
		b.Building("ManualGenerator");
		b.Building("Liquifier");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053F2 RID: 21490 RVA: 0x001EA070 File Offset: 0x001E8270
	public void SetupFallTest(Scenario.Builder b)
	{
		b.Jump(0, 5);
		b.Minion(null);
		b.Jump(0, -1);
		b.Building("Tile");
		b.Building("Tile");
		b.Building("Tile");
		b.Jump(-1, 1);
		b.Minion(null);
		b.Jump(2, 0);
		b.Minion(null);
		b.Jump(0, -1);
		b.Building("Tile");
		b.Jump(2, 1);
		b.Minion(null);
		b.Building("Ladder");
		b.Jump(-1, -1);
		b.Building("Tile");
		b.Jump(-1, -3);
		b.Building("Ladder");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053F3 RID: 21491 RVA: 0x001EA140 File Offset: 0x001E8340
	public void SetupClimbTest(int left, int bot)
	{
		this.DigHole(left, bot, 13, 5);
		this.SpawnPrefab(left + 1, bot + 1, "Minion", Grid.SceneLayer.Ore);
		int num = left + 2;
		this.Clear(num++, bot - 1);
		num++;
		this.Fill(num++, bot, SimHashes.Ice);
		num++;
		this.Clear(num, bot - 1);
		this.Clear(num++, bot - 2);
		this.Fill(num++, bot, SimHashes.Ice);
		this.Clear(num, bot - 1);
		this.Clear(num++, bot - 2);
		num++;
		this.Fill(num, bot, SimHashes.Ice);
		this.Fill(num, bot + 1, SimHashes.Ice);
	}

	// Token: 0x060053F4 RID: 21492 RVA: 0x001EA1F8 File Offset: 0x001E83F8
	private void SetupSuitRechargeTest(Scenario.Builder b)
	{
		b.Prefab("PressureSuit", delegate(GameObject go)
		{
			go.GetComponent<SuitTank>().Empty();
		});
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("SuitRecharger");
		b.Minion(null);
		b.Building("GasVent");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053F5 RID: 21493 RVA: 0x001EA274 File Offset: 0x001E8474
	private void SetupSuitTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Prefab("PressureSuit", null);
		b.Jump(1, 2);
		b.Building("Tile");
		b.Jump(-1, -2);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053F6 RID: 21494 RVA: 0x001EA2DC File Offset: 0x001E84DC
	private void SetupTwoKelvinsOneSuitTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(2, 0);
		b.Building("Door");
		b.Jump(2, 0);
		b.Minion(null);
		b.Prefab("PressureSuit", null);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053F7 RID: 21495 RVA: 0x001EA330 File Offset: 0x001E8530
	public void Clear()
	{
		foreach (Brain brain in Components.Brains.Items)
		{
			UnityEngine.Object.Destroy(brain.gameObject);
		}
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			UnityEngine.Object.Destroy(pickupable.gameObject);
		}
		foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
		{
			UnityEngine.Object.Destroy(buildingComplete.gameObject);
		}
	}

	// Token: 0x060053F8 RID: 21496 RVA: 0x001EA41C File Offset: 0x001E861C
	public void SetupGameplayTest()
	{
		this.Init();
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		if (this.ClearExistingScene)
		{
			this.Clear();
		}
		Scenario.RowLayout rowLayout = new Scenario.RowLayout(0, 0);
		if (this.CementMixerTest)
		{
			this.SetupCementMixerTest(rowLayout.NextRow());
		}
		if (this.RockCrusherTest)
		{
			this.SetupRockCrusherTest(rowLayout.NextRow());
		}
		if (this.PropaneGeneratorTest)
		{
			this.SetupPropaneGeneratorTest(rowLayout.NextRow());
		}
		if (this.DoorTest)
		{
			this.SetupDoorTest(rowLayout.NextRow());
		}
		if (this.HatchTest)
		{
			this.SetupHatchTest(rowLayout.NextRow());
		}
		if (this.AirLockTest)
		{
			this.SetupAirLockTest(rowLayout.NextRow());
		}
		if (this.BedTest)
		{
			this.SetupBedTest(rowLayout.NextRow());
		}
		if (this.LiquifierTest)
		{
			this.SetupLiquifierTest(rowLayout.NextRow());
		}
		if (this.SuitTest)
		{
			this.SetupSuitTest(rowLayout.NextRow());
		}
		if (this.SuitRechargeTest)
		{
			this.SetupSuitRechargeTest(rowLayout.NextRow());
		}
		if (this.TwoKelvinsOneSuitTest)
		{
			this.SetupTwoKelvinsOneSuitTest(rowLayout.NextRow());
		}
		if (this.FabricatorTest)
		{
			this.SetupFabricatorTest(rowLayout.NextRow());
		}
		if (this.ElectrolyzerTest)
		{
			this.SetupElectrolyzerTest(rowLayout.NextRow());
		}
		if (this.HexapedTest)
		{
			this.SetupHexapedTest(rowLayout.NextRow());
		}
		if (this.FallTest)
		{
			this.SetupFallTest(rowLayout.NextRow());
		}
		if (this.FeedingTest)
		{
			this.SetupFeedingTest(rowLayout.NextRow());
		}
		if (this.OrePerformanceTest)
		{
			this.SetupOrePerformanceTest(rowLayout.NextRow());
		}
		if (this.KilnTest)
		{
			this.SetupKilnTest(rowLayout.NextRow());
		}
	}

	// Token: 0x060053F9 RID: 21497 RVA: 0x001EA5C5 File Offset: 0x001E87C5
	private GameObject SpawnMinion(int x, int y)
	{
		return this.SpawnPrefab(x, y, "Minion", Grid.SceneLayer.Move);
	}

	// Token: 0x060053FA RID: 21498 RVA: 0x001EA5D8 File Offset: 0x001E87D8
	private void SetupLadderTest(int left, int bot)
	{
		int num = 5;
		this.DigHole(left, bot, 13, num);
		this.SpawnMinion(left + 1, bot);
		int x = left + 1;
		this.PlacerLadder(x++, bot, num);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x, bot);
		this.PlacerLadder(x++, bot + 1, num - 1);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot, num);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot + 1, num - 1);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot - 1, -num);
	}

	// Token: 0x060053FB RID: 21499 RVA: 0x001EA6B4 File Offset: 0x001E88B4
	public void PlaceUtilitiesX(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceUtilities(left + i, bot);
		}
	}

	// Token: 0x060053FC RID: 21500 RVA: 0x001EA6D7 File Offset: 0x001E88D7
	public void PlaceUtilities(int left, int bot)
	{
		this.PlaceBuilding(left, bot, "Wire", SimHashes.Cuprite);
		this.PlaceBuilding(left, bot, "GasConduit", SimHashes.Cuprite);
	}

	// Token: 0x060053FD RID: 21501 RVA: 0x001EA700 File Offset: 0x001E8900
	public void SetupVisualTest()
	{
		this.Init();
		Scenario.RowLayout row_layout = new Scenario.RowLayout(this.Left, this.Bot);
		this.SetupBuildingTest(row_layout, false, false);
	}

	// Token: 0x060053FE RID: 21502 RVA: 0x001EA730 File Offset: 0x001E8930
	private void SpawnMaterialTest(Scenario.Builder b)
	{
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid)
			{
				b.Element = element.id;
				b.Building("Generator");
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x060053FF RID: 21503 RVA: 0x001EA7AC File Offset: 0x001E89AC
	public GameObject PlaceBuilding(int x, int y, string prefab_id, SimHashes element = SimHashes.Cuprite)
	{
		return Scenario.PlaceBuilding(this.RootCell, x, y, prefab_id, element);
	}

	// Token: 0x06005400 RID: 21504 RVA: 0x001EA7C0 File Offset: 0x001E89C0
	public static GameObject PlaceBuilding(int root_cell, int x, int y, string prefab_id, SimHashes element = SimHashes.Cuprite)
	{
		int cell = Grid.OffsetCell(root_cell, x, y);
		BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
		if (buildingDef == null || buildingDef.PlacementOffsets == null)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Missing def for",
				prefab_id
			});
		}
		Element element2 = ElementLoader.FindElementByHash(element);
		global::Debug.Assert(element2 != null, string.Concat(new string[]
		{
			"Missing primary element '",
			Enum.GetName(typeof(SimHashes), element),
			"' in '",
			prefab_id,
			"'"
		}));
		GameObject gameObject = buildingDef.Build(buildingDef.GetBuildingCell(cell), Orientation.Neutral, null, new Tag[]
		{
			element2.tag,
			ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag
		}, 293.15f, false, -1f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.InternalTemperature = 300f;
		component.Temperature = 300f;
		return gameObject;
	}

	// Token: 0x06005401 RID: 21505 RVA: 0x001EA8B4 File Offset: 0x001E8AB4
	private void SpawnOre(int x, int y, SimHashes element = SimHashes.Cuprite)
	{
		this.RunAfterNextUpdate(delegate
		{
			Vector3 position = Grid.CellToPosCCC(Grid.OffsetCell(this.RootCell, x, y), Grid.SceneLayer.Ore);
			position.x += UnityEngine.Random.Range(-0.1f, 0.1f);
			ElementLoader.FindElementByHash(element).substance.SpawnResource(position, 4000f, 293f, byte.MaxValue, 0, false, false, false);
		});
	}

	// Token: 0x06005402 RID: 21506 RVA: 0x001EA8F5 File Offset: 0x001E8AF5
	public GameObject SpawnPrefab(int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		return Scenario.SpawnPrefab(this.RootCell, x, y, name, scene_layer);
	}

	// Token: 0x06005403 RID: 21507 RVA: 0x001EA908 File Offset: 0x001E8B08
	public void SpawnPrefabLate(int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		this.RunAfterNextUpdate(delegate
		{
			Scenario.SpawnPrefab(this.RootCell, x, y, name, scene_layer);
		});
	}

	// Token: 0x06005404 RID: 21508 RVA: 0x001EA954 File Offset: 0x001E8B54
	public static GameObject SpawnPrefab(int RootCell, int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		int cell = Grid.OffsetCell(RootCell, x, y);
		GameObject prefab = Assets.GetPrefab(TagManager.Create(name));
		if (prefab == null)
		{
			return null;
		}
		return GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(cell, scene_layer), scene_layer, null, 0);
	}

	// Token: 0x06005405 RID: 21509 RVA: 0x001EA994 File Offset: 0x001E8B94
	public void SetupElementTest()
	{
		this.Init();
		PropertyTextures.FogOfWarScale = 1f;
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		this.Clear();
		Scenario.Builder builder = new Scenario.RowLayout(0, 0).NextRow();
		HashSet<Element> elements = new HashSet<Element>();
		int bot = builder.Bot;
		foreach (Element element5 in (from element in ElementLoader.elements
		where element.IsSolid
		orderby element.highTempTransitionTarget
		select element).ToList<Element>())
		{
			if (element5.IsSolid)
			{
				Element element2 = element5;
				int left = builder.Left;
				bool hasTransitionUp;
				do
				{
					elements.Add(element2);
					builder.Hole(2, 3);
					builder.Fill(2, 2, element2.id);
					builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
					builder = new Scenario.Builder(left, builder.Bot + 4, SimHashes.Copper);
					hasTransitionUp = element2.HasTransitionUp;
					if (hasTransitionUp)
					{
						element2 = element2.highTempTransition;
					}
				}
				while (hasTransitionUp);
				builder = new Scenario.Builder(left + 3, bot, SimHashes.Copper);
			}
		}
		foreach (Element element3 in (from element in ElementLoader.elements
		where element.IsLiquid && !elements.Contains(element)
		orderby element.highTempTransitionTarget
		select element).ToList<Element>())
		{
			int left2 = builder.Left;
			bool hasTransitionUp2;
			do
			{
				elements.Add(element3);
				builder.Hole(2, 3);
				builder.Fill(2, 2, element3.id);
				builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
				builder = new Scenario.Builder(left2, builder.Bot + 4, SimHashes.Copper);
				hasTransitionUp2 = element3.HasTransitionUp;
				if (hasTransitionUp2)
				{
					element3 = element3.highTempTransition;
				}
			}
			while (hasTransitionUp2);
			builder = new Scenario.Builder(left2 + 3, bot, SimHashes.Copper);
		}
		foreach (Element element4 in (from element in ElementLoader.elements
		where element.state == Element.State.Gas && !elements.Contains(element)
		select element).ToList<Element>())
		{
			int left3 = builder.Left;
			builder.Hole(2, 3);
			builder.Fill(2, 2, element4.id);
			builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
			builder = new Scenario.Builder(left3, builder.Bot + 4, SimHashes.Copper);
			builder = new Scenario.Builder(left3 + 3, bot, SimHashes.Copper);
		}
	}

	// Token: 0x06005406 RID: 21510 RVA: 0x001EACB8 File Offset: 0x001E8EB8
	private void InitDebugScenario()
	{
		this.Init();
		PropertyTextures.FogOfWarScale = 1f;
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		this.Clear();
	}

	// Token: 0x06005407 RID: 21511 RVA: 0x001EACF4 File Offset: 0x001E8EF4
	public void SetupTileTest()
	{
		this.InitDebugScenario();
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			for (int j = 0; j < Grid.WidthInCells; j++)
			{
				SimMessages.ReplaceElement(Grid.XYToCell(j, i), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 100f, -1f, byte.MaxValue, 0, -1);
			}
		}
		Scenario.Builder builder = new Scenario.RowLayout(0, 0).NextRow();
		for (int k = 0; k < 16; k++)
		{
			builder.Jump(0, 0);
			builder.Fill(1, 1, ((k & 1) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(1, 0);
			builder.Fill(1, 1, ((k & 2) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(-1, 1);
			builder.Fill(1, 1, ((k & 4) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(1, 0);
			builder.Fill(1, 1, ((k & 8) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(2, -1);
		}
	}

	// Token: 0x06005408 RID: 21512 RVA: 0x001EAE00 File Offset: 0x001E9000
	public void SetupRiverTest()
	{
		this.InitDebugScenario();
		int num = Mathf.Min(64, Grid.WidthInCells);
		int num2 = Mathf.Min(64, Grid.HeightInCells);
		List<Element> list = new List<Element>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsLiquid)
			{
				list.Add(element);
			}
		}
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				SimHashes new_element = (i == 0) ? SimHashes.Unobtanium : SimHashes.Oxygen;
				SimMessages.ReplaceElement(Grid.XYToCell(j, i), new_element, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			}
		}
	}

	// Token: 0x06005409 RID: 21513 RVA: 0x001EAEE0 File Offset: 0x001E90E0
	public void SetupRockCrusherTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("Crusher");
		b.Minion(null);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x0600540A RID: 21514 RVA: 0x001EAF20 File Offset: 0x001E9120
	public void SetupCementMixerTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("Generator");
		b.Minion(null);
		b.Building("Crusher");
		b.Minion(null);
		b.Minion(null);
		b.Building("Mixer");
		b.Ore(20, SimHashes.SedimentaryRock);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x0600540B RID: 21515 RVA: 0x001EAF8C File Offset: 0x001E918C
	public void SetupKilnTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("Kiln");
		b.Minion(null);
		b.Ore(20, SimHashes.SandCement);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x040038AD RID: 14509
	private int Bot;

	// Token: 0x040038AE RID: 14510
	private int Left;

	// Token: 0x040038AF RID: 14511
	public int RootCell;

	// Token: 0x040038B1 RID: 14513
	public static Scenario Instance;

	// Token: 0x040038B2 RID: 14514
	public bool PropaneGeneratorTest = true;

	// Token: 0x040038B3 RID: 14515
	public bool HatchTest = true;

	// Token: 0x040038B4 RID: 14516
	public bool DoorTest = true;

	// Token: 0x040038B5 RID: 14517
	public bool AirLockTest = true;

	// Token: 0x040038B6 RID: 14518
	public bool BedTest = true;

	// Token: 0x040038B7 RID: 14519
	public bool SuitTest = true;

	// Token: 0x040038B8 RID: 14520
	public bool SuitRechargeTest = true;

	// Token: 0x040038B9 RID: 14521
	public bool FabricatorTest = true;

	// Token: 0x040038BA RID: 14522
	public bool ElectrolyzerTest = true;

	// Token: 0x040038BB RID: 14523
	public bool HexapedTest = true;

	// Token: 0x040038BC RID: 14524
	public bool FallTest = true;

	// Token: 0x040038BD RID: 14525
	public bool FeedingTest = true;

	// Token: 0x040038BE RID: 14526
	public bool OrePerformanceTest = true;

	// Token: 0x040038BF RID: 14527
	public bool TwoKelvinsOneSuitTest = true;

	// Token: 0x040038C0 RID: 14528
	public bool LiquifierTest = true;

	// Token: 0x040038C1 RID: 14529
	public bool RockCrusherTest = true;

	// Token: 0x040038C2 RID: 14530
	public bool CementMixerTest = true;

	// Token: 0x040038C3 RID: 14531
	public bool KilnTest = true;

	// Token: 0x040038C4 RID: 14532
	public bool ClearExistingScene = true;

	// Token: 0x02001C8D RID: 7309
	public class RowLayout
	{
		// Token: 0x0600ADE3 RID: 44515 RVA: 0x003D198F File Offset: 0x003CFB8F
		public RowLayout(int left, int bot)
		{
			this.Left = left;
			this.Bot = bot;
		}

		// Token: 0x0600ADE4 RID: 44516 RVA: 0x003D19A8 File Offset: 0x003CFBA8
		public Scenario.Builder NextRow()
		{
			if (this.Builder != null)
			{
				this.Bot = this.Builder.Max.y + 1;
			}
			this.Builder = new Scenario.Builder(this.Left, this.Bot, SimHashes.Copper);
			return this.Builder;
		}

		// Token: 0x04008870 RID: 34928
		public int Left;

		// Token: 0x04008871 RID: 34929
		public int Bot;

		// Token: 0x04008872 RID: 34930
		public Scenario.Builder Builder;
	}

	// Token: 0x02001C8E RID: 7310
	public class Builder
	{
		// Token: 0x0600ADE5 RID: 44517 RVA: 0x003D19F8 File Offset: 0x003CFBF8
		public Builder(int left, int bot, SimHashes element = SimHashes.Copper)
		{
			this.Left = left;
			this.Bot = bot;
			this.Element = element;
			this.Scenario = Scenario.Instance;
			this.PlaceUtilities = true;
			this.Min = new Vector2I(left, bot);
			this.Max = new Vector2I(left, bot);
		}

		// Token: 0x0600ADE6 RID: 44518 RVA: 0x003D1A4C File Offset: 0x003CFC4C
		private void UpdateMinMax(int x, int y)
		{
			this.Min.x = Math.Min(x, this.Min.x);
			this.Min.y = Math.Min(y, this.Min.y);
			this.Max.x = Math.Max(x + 1, this.Max.x);
			this.Max.y = Math.Max(y + 1, this.Max.y);
		}

		// Token: 0x0600ADE7 RID: 44519 RVA: 0x003D1AD0 File Offset: 0x003CFCD0
		public void Utilities(int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.Scenario.PlaceUtilities(this.Left, this.Bot);
				this.Left++;
			}
		}

		// Token: 0x0600ADE8 RID: 44520 RVA: 0x003D1B10 File Offset: 0x003CFD10
		public void BuildingOffsets(string prefab_id, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Building(prefab_id);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x0600ADE9 RID: 44521 RVA: 0x003D1B60 File Offset: 0x003CFD60
		public void Placer(string prefab_id, Element element)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
			int buildingCell = buildingDef.GetBuildingCell(Grid.OffsetCell(Scenario.Instance.RootCell, this.Left, this.Bot));
			Vector3 pos = Grid.CellToPosCBC(buildingCell, Grid.SceneLayer.Building);
			this.UpdateMinMax(this.Left, this.Bot);
			this.UpdateMinMax(this.Left + buildingDef.WidthInCells - 1, this.Bot + buildingDef.HeightInCells - 1);
			this.Left += buildingDef.WidthInCells;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				Assets.GetBuildingDef(prefab_id).TryPlace(null, pos, Orientation.Neutral, new Tag[]
				{
					element.tag,
					ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag
				}, 0);
			});
		}

		// Token: 0x0600ADEA RID: 44522 RVA: 0x003D1C20 File Offset: 0x003CFE20
		public GameObject Building(string prefab_id)
		{
			GameObject result = this.Scenario.PlaceBuilding(this.Left, this.Bot, prefab_id, this.Element);
			BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
			this.UpdateMinMax(this.Left, this.Bot);
			this.UpdateMinMax(this.Left + buildingDef.WidthInCells - 1, this.Bot + buildingDef.HeightInCells - 1);
			if (this.PlaceUtilities)
			{
				for (int i = 0; i < buildingDef.WidthInCells; i++)
				{
					this.UpdateMinMax(this.Left + i, this.Bot);
					this.Scenario.PlaceUtilities(this.Left + i, this.Bot);
				}
			}
			this.Left += buildingDef.WidthInCells;
			return result;
		}

		// Token: 0x0600ADEB RID: 44523 RVA: 0x003D1CE4 File Offset: 0x003CFEE4
		public void Minion(Action<GameObject> on_spawn = null)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			int left = this.Left;
			int bot = this.Bot;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				GameObject obj = this.Scenario.SpawnMinion(left, bot);
				if (on_spawn != null)
				{
					on_spawn(obj);
				}
			});
		}

		// Token: 0x0600ADEC RID: 44524 RVA: 0x003D1D46 File Offset: 0x003CFF46
		private GameObject Hexaped()
		{
			return this.Scenario.SpawnPrefab(this.Left, this.Bot, "Hexaped", Grid.SceneLayer.Front);
		}

		// Token: 0x0600ADED RID: 44525 RVA: 0x003D1D68 File Offset: 0x003CFF68
		public void OreOffsets(int count, SimHashes element, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Ore(count, element);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x0600ADEE RID: 44526 RVA: 0x003D1DB8 File Offset: 0x003CFFB8
		public void Ore(int count = 1, SimHashes element = SimHashes.Cuprite)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			for (int i = 0; i < count; i++)
			{
				this.Scenario.SpawnOre(this.Left, this.Bot, element);
			}
		}

		// Token: 0x0600ADEF RID: 44527 RVA: 0x003D1DFC File Offset: 0x003CFFFC
		public void PrefabOffsets(string prefab_id, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Prefab(prefab_id, null);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x0600ADF0 RID: 44528 RVA: 0x003D1E4C File Offset: 0x003D004C
		public void Prefab(string prefab_id, Action<GameObject> on_spawn = null)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			int left = this.Left;
			int bot = this.Bot;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				GameObject obj = this.Scenario.SpawnPrefab(left, bot, prefab_id, Grid.SceneLayer.Ore);
				if (on_spawn != null)
				{
					on_spawn(obj);
				}
			});
		}

		// Token: 0x0600ADF1 RID: 44529 RVA: 0x003D1EB8 File Offset: 0x003D00B8
		public void Wall(int height)
		{
			for (int i = 0; i < height; i++)
			{
				this.Scenario.PlaceBuilding(this.Left, this.Bot + i, "Tile", SimHashes.Cuprite);
				this.UpdateMinMax(this.Left, this.Bot + i);
				if (this.PlaceUtilities)
				{
					this.Scenario.PlaceUtilities(this.Left, this.Bot + i);
				}
			}
			this.Left++;
		}

		// Token: 0x0600ADF2 RID: 44530 RVA: 0x003D1F38 File Offset: 0x003D0138
		public void Jump(int x = 0, int y = 0)
		{
			this.Left += x;
			this.Bot += y;
		}

		// Token: 0x0600ADF3 RID: 44531 RVA: 0x003D1F56 File Offset: 0x003D0156
		public void JumpTo(int left, int bot)
		{
			this.Left = left;
			this.Bot = bot;
		}

		// Token: 0x0600ADF4 RID: 44532 RVA: 0x003D1F68 File Offset: 0x003D0168
		public void Hole(int width, int height)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int num = Grid.OffsetCell(this.Scenario.RootCell, this.Left + i, this.Bot + j);
					this.UpdateMinMax(this.Left + i, this.Bot + j);
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.Scenario, 0f, -1f, byte.MaxValue, 0, -1);
					this.Scenario.ReplaceElementMask[num] = true;
				}
			}
		}

		// Token: 0x0600ADF5 RID: 44533 RVA: 0x003D1FF8 File Offset: 0x003D01F8
		public void FillOffsets(SimHashes element, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Fill(1, 1, element);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x0600ADF6 RID: 44534 RVA: 0x003D2048 File Offset: 0x003D0248
		public void Fill(int width, int height, SimHashes element)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int num = Grid.OffsetCell(this.Scenario.RootCell, this.Left + i, this.Bot + j);
					this.UpdateMinMax(this.Left + i, this.Bot + j);
					SimMessages.ReplaceElement(num, element, CellEventLogger.Instance.Scenario, 5000f, -1f, byte.MaxValue, 0, -1);
					this.Scenario.ReplaceElementMask[num] = true;
				}
			}
		}

		// Token: 0x0600ADF7 RID: 44535 RVA: 0x003D20D4 File Offset: 0x003D02D4
		public void InAndOuts()
		{
			this.Wall(3);
			this.Building("GasVent");
			this.Hole(3, 3);
			this.Utilities(2);
			this.Wall(3);
			this.Building("LiquidVent");
			this.Hole(3, 3);
			this.Utilities(2);
			this.Wall(3);
			this.Fill(3, 3, SimHashes.Water);
			this.Utilities(2);
			GameObject pump = this.Building("Pump");
			this.Scenario.RunAfterNextUpdate(delegate
			{
				pump.GetComponent<BuildingEnabledButton>().IsEnabled = true;
			});
		}

		// Token: 0x0600ADF8 RID: 44536 RVA: 0x003D2170 File Offset: 0x003D0370
		public Scenario.Builder FinalizeRoom(SimHashes element = SimHashes.Oxygen, SimHashes tileElement = SimHashes.Steel)
		{
			for (int i = this.Min.x - 1; i <= this.Max.x; i++)
			{
				if (i == this.Min.x - 1 || i == this.Max.x)
				{
					for (int j = this.Min.y - 1; j <= this.Max.y; j++)
					{
						this.Scenario.PlaceBuilding(i, j, "Tile", tileElement);
					}
				}
				else
				{
					int num = 500;
					if (element == SimHashes.Void)
					{
						num = 0;
					}
					for (int k = this.Min.y; k < this.Max.y; k++)
					{
						int num2 = Grid.OffsetCell(this.Scenario.RootCell, i, k);
						if (!this.Scenario.ReplaceElementMask[num2])
						{
							SimMessages.ReplaceElement(num2, element, CellEventLogger.Instance.Scenario, (float)num, -1f, byte.MaxValue, 0, -1);
						}
					}
				}
				this.Scenario.PlaceBuilding(i, this.Min.y - 1, "Tile", tileElement);
				this.Scenario.PlaceBuilding(i, this.Max.y, "Tile", tileElement);
			}
			return new Scenario.Builder(this.Max.x + 1, this.Min.y, SimHashes.Copper);
		}

		// Token: 0x04008873 RID: 34931
		public bool PlaceUtilities;

		// Token: 0x04008874 RID: 34932
		public int Left;

		// Token: 0x04008875 RID: 34933
		public int Bot;

		// Token: 0x04008876 RID: 34934
		public Vector2I Min;

		// Token: 0x04008877 RID: 34935
		public Vector2I Max;

		// Token: 0x04008878 RID: 34936
		public SimHashes Element;

		// Token: 0x04008879 RID: 34937
		private Scenario Scenario;
	}
}
