using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000957 RID: 2391
[AddComponentMenu("KMonoBehaviour/scripts/FishOvercrowingManager")]
public class FishOvercrowingManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x060042D4 RID: 17108 RVA: 0x00179BD2 File Offset: 0x00177DD2
	public static void DestroyInstance()
	{
		FishOvercrowingManager.Instance = null;
	}

	// Token: 0x060042D5 RID: 17109 RVA: 0x00179BDA File Offset: 0x00177DDA
	protected override void OnPrefabInit()
	{
		FishOvercrowingManager.Instance = this;
		this.cells = new FishOvercrowingManager.Cell[Grid.CellCount];
	}

	// Token: 0x060042D6 RID: 17110 RVA: 0x00179BF2 File Offset: 0x00177DF2
	public void Add(KPrefabID aquaticEntity)
	{
		this.allAquaticEntities.Add(aquaticEntity);
	}

	// Token: 0x060042D7 RID: 17111 RVA: 0x00179C00 File Offset: 0x00177E00
	public void Remove(KPrefabID aquaticEntity)
	{
		if (aquaticEntity.IsNullOrDestroyed())
		{
			return;
		}
		for (int i = this.allAquaticEntities.Count - 1; i >= 0; i--)
		{
			KPrefabID kprefabID = this.allAquaticEntities[i];
			if (!kprefabID.IsNullOrDestroyed() && kprefabID.InstanceID == aquaticEntity.InstanceID)
			{
				this.allAquaticEntities.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x060042D8 RID: 17112 RVA: 0x00179C60 File Offset: 0x00177E60
	public void Sim1000ms(float dt)
	{
		int num = this.versionCounter;
		this.versionCounter = num + 1;
		int num2 = num;
		for (int num3 = 0; num3 != this.ponds.Count; num3++)
		{
			FishOvercrowingManager.Pond pond = this.ponds[num3];
			pond.fishes.Clear();
			pond.eggs.Clear();
			pond.cellCount = 0;
			pond.occupancy.dirty = true;
		}
		int num4 = (this.ponds.Count == 0) ? -1 : 0;
		QueuePool<int, FishOvercrowingManager>.PooledQueue pooledQueue = QueuePool<int, FishOvercrowingManager>.Allocate();
		foreach (KPrefabID kprefabID in this.allAquaticEntities)
		{
			if (!kprefabID.IsNullOrDestroyed())
			{
				int num5 = Grid.PosToCell(kprefabID);
				if (Grid.IsValidCell(num5))
				{
					pooledQueue.Clear();
					pooledQueue.Enqueue(num5);
					FishOvercrowingManager.Cell cell = this.cells[num5];
					int num6;
					if (cell.Version == num2)
					{
						num6 = cell.PondIndex;
					}
					else if (num4 != -1 && num4 < this.ponds.Count)
					{
						num6 = num4;
						num4++;
						if (num4 == this.ponds.Count)
						{
							num4 = -1;
						}
					}
					else
					{
						FishOvercrowingManager.Pond item = new FishOvercrowingManager.Pond
						{
							fishes = new List<KPrefabID>(),
							eggs = new List<KPrefabID>()
						};
						this.ponds.Add(item);
						num6 = this.ponds.Count - 1;
					}
					FishOvercrowingManager.Pond pond2 = this.ponds[num6];
					if (kprefabID.HasTag(GameTags.Egg))
					{
						pond2.eggs.Add(kprefabID);
					}
					else
					{
						pond2.fishes.Add(kprefabID);
					}
					int num7;
					while (pooledQueue.TryDequeue(out num7))
					{
						if (Grid.IsValidCell(num7) && this.cells[num7].Version != num2 && Grid.IsNavigatableLiquid(num7))
						{
							this.cells[num7] = new FishOvercrowingManager.Cell(num2, num6);
							pond2.cellCount++;
							pooledQueue.Enqueue(Grid.CellLeft(num7));
							pooledQueue.Enqueue(Grid.CellRight(num7));
							pooledQueue.Enqueue(Grid.CellAbove(num7));
							pooledQueue.Enqueue(Grid.CellBelow(num7));
						}
					}
				}
			}
		}
		pooledQueue.Recycle();
		if (num4 != -1)
		{
			int num8 = this.ponds.Count - num4;
			if (num8 > 0)
			{
				this.ponds.RemoveRange(num4, num8);
			}
		}
		this.allAquaticEntities.RemoveAll(new Predicate<KPrefabID>(Util.IsNullOrDestroyed));
	}

	// Token: 0x060042D9 RID: 17113 RVA: 0x00179F00 File Offset: 0x00178100
	public FishOvercrowingManager.Pond GetPond(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		FishOvercrowingManager.Cell cell2 = this.cells[cell];
		if (cell2.Version != this.versionCounter - 1)
		{
			return null;
		}
		return this.ponds[cell2.PondIndex];
	}

	// Token: 0x060042DA RID: 17114 RVA: 0x00179F4C File Offset: 0x0017814C
	public int GetFishInPondCount(int cell, HashSet<Tag> accepted_tags)
	{
		int num = 0;
		FishOvercrowingManager.Pond pond = this.GetPond(cell);
		if (pond == null)
		{
			return 0;
		}
		foreach (KPrefabID kprefabID in pond.fishes)
		{
			if (!kprefabID.HasTag(GameTags.Creatures.Bagged) && !kprefabID.HasTag(GameTags.Trapped) && accepted_tags.Contains(kprefabID.PrefabTag))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x04002A02 RID: 10754
	public static FishOvercrowingManager Instance;

	// Token: 0x04002A03 RID: 10755
	private readonly List<KPrefabID> allAquaticEntities = new List<KPrefabID>();

	// Token: 0x04002A04 RID: 10756
	private readonly List<FishOvercrowingManager.Pond> ponds = new List<FishOvercrowingManager.Pond>();

	// Token: 0x04002A05 RID: 10757
	private FishOvercrowingManager.Cell[] cells;

	// Token: 0x04002A06 RID: 10758
	private int versionCounter = 2;

	// Token: 0x02001946 RID: 6470
	private readonly struct Cell
	{
		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x0600A1CB RID: 41419 RVA: 0x003ACDD9 File Offset: 0x003AAFD9
		public int Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x0600A1CC RID: 41420 RVA: 0x003ACDE1 File Offset: 0x003AAFE1
		public int PondIndex
		{
			get
			{
				return this.pondIndex;
			}
		}

		// Token: 0x0600A1CD RID: 41421 RVA: 0x003ACDE9 File Offset: 0x003AAFE9
		public Cell(int version, int pondIndex)
		{
			this.version = version;
			this.pondIndex = pondIndex;
		}

		// Token: 0x04007D5D RID: 32093
		private readonly int version;

		// Token: 0x04007D5E RID: 32094
		private readonly int pondIndex;
	}

	// Token: 0x02001947 RID: 6471
	public class Pond
	{
		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x0600A1CE RID: 41422 RVA: 0x003ACDF9 File Offset: 0x003AAFF9
		public int FishCount
		{
			get
			{
				return this.fishes.Count;
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x0600A1CF RID: 41423 RVA: 0x003ACE06 File Offset: 0x003AB006
		public int EggCount
		{
			get
			{
				return this.eggs.Count;
			}
		}

		// Token: 0x04007D5F RID: 32095
		public List<KPrefabID> fishes;

		// Token: 0x04007D60 RID: 32096
		public List<KPrefabID> eggs;

		// Token: 0x04007D61 RID: 32097
		public int cellCount;

		// Token: 0x04007D62 RID: 32098
		public OvercrowdingMonitor.Occupancy occupancy = new OvercrowdingMonitor.Occupancy();
	}
}
