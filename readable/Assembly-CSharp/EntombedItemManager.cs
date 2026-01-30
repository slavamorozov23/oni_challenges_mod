using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000928 RID: 2344
[AddComponentMenu("KMonoBehaviour/scripts/EntombedItemManager")]
public class EntombedItemManager : KMonoBehaviour, ISim33ms
{
	// Token: 0x0600418E RID: 16782 RVA: 0x001720CB File Offset: 0x001702CB
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.SpawnUncoveredObjects();
		this.AddMassToWorldIfPossible();
		this.PopulateEntombedItemVisualizers();
	}

	// Token: 0x0600418F RID: 16783 RVA: 0x001720E0 File Offset: 0x001702E0
	public static bool CanEntomb(Pickupable pickupable)
	{
		if (pickupable == null)
		{
			return false;
		}
		if (pickupable.storage != null)
		{
			return false;
		}
		int num = Grid.PosToCell(pickupable);
		return Grid.IsValidCell(num) && Grid.Solid[num] && !(Grid.Objects[num, 9] != null) && (pickupable.PrimaryElement.Element.IsSolid && pickupable.GetComponent<ElementChunk>() != null);
	}

	// Token: 0x06004190 RID: 16784 RVA: 0x00172162 File Offset: 0x00170362
	public void Add(Pickupable pickupable)
	{
		this.pickupables.Add(pickupable);
	}

	// Token: 0x06004191 RID: 16785 RVA: 0x00172170 File Offset: 0x00170370
	public void Sim33ms(float dt)
	{
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		HashSetPool<Pickupable, EntombedItemManager>.PooledHashSet pooledHashSet = HashSetPool<Pickupable, EntombedItemManager>.Allocate();
		foreach (Pickupable pickupable in this.pickupables)
		{
			if (EntombedItemManager.CanEntomb(pickupable))
			{
				pooledHashSet.Add(pickupable);
			}
		}
		this.pickupables.Clear();
		foreach (Pickupable pickupable2 in pooledHashSet)
		{
			int num = Grid.PosToCell(pickupable2);
			PrimaryElement primaryElement = pickupable2.PrimaryElement;
			SimHashes elementID = primaryElement.ElementID;
			float mass = primaryElement.Mass;
			float temperature = primaryElement.Temperature;
			byte diseaseIdx = primaryElement.DiseaseIdx;
			int diseaseCount = primaryElement.DiseaseCount;
			Element element = Grid.Element[num];
			if (elementID == element.id && mass > 0.010000001f && Grid.Mass[num] + mass < element.maxMass)
			{
				SimMessages.AddRemoveSubstance(num, ElementLoader.FindElementByHash(elementID).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, diseaseIdx, diseaseCount, true, -1);
			}
			else
			{
				component.AddItem(num);
				this.cells.Add(num);
				this.elementIds.Add((int)elementID);
				this.masses.Add(mass);
				this.temperatures.Add(temperature);
				this.diseaseIndices.Add(diseaseIdx);
				this.diseaseCounts.Add(diseaseCount);
			}
			Util.KDestroyGameObject(pickupable2.gameObject);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x06004192 RID: 16786 RVA: 0x00172338 File Offset: 0x00170538
	public void OnSolidChanged(List<int> solid_changed_cells)
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		foreach (int num in solid_changed_cells)
		{
			if (!Grid.Solid[num])
			{
				pooledList.Add(num);
			}
		}
		ListPool<int, EntombedItemManager>.PooledList pooledList2 = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int num2 = this.cells[i];
			foreach (int num3 in pooledList)
			{
				if (num2 == num3)
				{
					pooledList2.Add(i);
					break;
				}
			}
		}
		pooledList.Recycle();
		this.SpawnObjects(pooledList2);
		pooledList2.Recycle();
	}

	// Token: 0x06004193 RID: 16787 RVA: 0x00172424 File Offset: 0x00170624
	private void SpawnUncoveredObjects()
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int i2 = this.cells[i];
			if (!Grid.Solid[i2])
			{
				pooledList.Add(i);
			}
		}
		this.SpawnObjects(pooledList);
		pooledList.Recycle();
	}

	// Token: 0x06004194 RID: 16788 RVA: 0x0017247C File Offset: 0x0017067C
	private void AddMassToWorldIfPossible()
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int num = this.cells[i];
			if (Grid.Solid[num] && Grid.Element[num].id == (SimHashes)this.elementIds[i])
			{
				pooledList.Add(i);
			}
		}
		pooledList.Sort();
		pooledList.Reverse();
		foreach (int item_idx in pooledList)
		{
			EntombedItemManager.Item item = this.GetItem(item_idx);
			this.RemoveItem(item_idx);
			if (item.mass > 1E-45f)
			{
				SimMessages.AddRemoveSubstance(item.cell, ElementLoader.FindElementByHash((SimHashes)item.elementId).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, item.mass, item.temperature, item.diseaseIdx, item.diseaseCount, false, -1);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06004195 RID: 16789 RVA: 0x00172594 File Offset: 0x00170794
	private void RemoveItem(int item_idx)
	{
		this.cells.RemoveAt(item_idx);
		this.elementIds.RemoveAt(item_idx);
		this.masses.RemoveAt(item_idx);
		this.temperatures.RemoveAt(item_idx);
		this.diseaseIndices.RemoveAt(item_idx);
		this.diseaseCounts.RemoveAt(item_idx);
	}

	// Token: 0x06004196 RID: 16790 RVA: 0x001725EC File Offset: 0x001707EC
	private EntombedItemManager.Item GetItem(int item_idx)
	{
		return new EntombedItemManager.Item
		{
			cell = this.cells[item_idx],
			elementId = this.elementIds[item_idx],
			mass = this.masses[item_idx],
			temperature = this.temperatures[item_idx],
			diseaseIdx = this.diseaseIndices[item_idx],
			diseaseCount = this.diseaseCounts[item_idx]
		};
	}

	// Token: 0x06004197 RID: 16791 RVA: 0x00172674 File Offset: 0x00170874
	private void SpawnObjects(List<int> uncovered_item_indices)
	{
		uncovered_item_indices.Sort();
		uncovered_item_indices.Reverse();
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		foreach (int item_idx in uncovered_item_indices)
		{
			EntombedItemManager.Item item = this.GetItem(item_idx);
			component.RemoveItem(item.cell);
			this.RemoveItem(item_idx);
			Element element = ElementLoader.FindElementByHash((SimHashes)item.elementId);
			if (element != null)
			{
				element.substance.SpawnResource(Grid.CellToPosCCC(item.cell, Grid.SceneLayer.Ore), item.mass, item.temperature, item.diseaseIdx, item.diseaseCount, false, false, false);
			}
		}
	}

	// Token: 0x06004198 RID: 16792 RVA: 0x00172734 File Offset: 0x00170934
	private void PopulateEntombedItemVisualizers()
	{
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		foreach (int cell in this.cells)
		{
			component.AddItem(cell);
		}
	}

	// Token: 0x040028EE RID: 10478
	[Serialize]
	private List<int> cells = new List<int>();

	// Token: 0x040028EF RID: 10479
	[Serialize]
	private List<int> elementIds = new List<int>();

	// Token: 0x040028F0 RID: 10480
	[Serialize]
	private List<float> masses = new List<float>();

	// Token: 0x040028F1 RID: 10481
	[Serialize]
	private List<float> temperatures = new List<float>();

	// Token: 0x040028F2 RID: 10482
	[Serialize]
	private List<byte> diseaseIndices = new List<byte>();

	// Token: 0x040028F3 RID: 10483
	[Serialize]
	private List<int> diseaseCounts = new List<int>();

	// Token: 0x040028F4 RID: 10484
	private List<Pickupable> pickupables = new List<Pickupable>();

	// Token: 0x02001925 RID: 6437
	private struct Item
	{
		// Token: 0x04007D0E RID: 32014
		public int cell;

		// Token: 0x04007D0F RID: 32015
		public int elementId;

		// Token: 0x04007D10 RID: 32016
		public float mass;

		// Token: 0x04007D11 RID: 32017
		public float temperature;

		// Token: 0x04007D12 RID: 32018
		public byte diseaseIdx;

		// Token: 0x04007D13 RID: 32019
		public int diseaseCount;
	}
}
