using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000564 RID: 1380
[AddComponentMenu("KMonoBehaviour/scripts/SymbolOverrideController")]
public class SymbolOverrideController : KMonoBehaviour
{
	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06001EBC RID: 7868 RVA: 0x000A6FE0 File Offset: 0x000A51E0
	public SymbolOverrideController.SymbolEntry[] GetSymbolOverrides
	{
		get
		{
			return this.symbolOverrides.ToArray();
		}
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06001EBD RID: 7869 RVA: 0x000A6FED File Offset: 0x000A51ED
	// (set) Token: 0x06001EBE RID: 7870 RVA: 0x000A6FF5 File Offset: 0x000A51F5
	public int version { get; private set; }

	// Token: 0x06001EBF RID: 7871 RVA: 0x000A7000 File Offset: 0x000A5200
	protected override void OnPrefabInit()
	{
		this.animController = base.GetComponent<KBatchedAnimController>();
		DebugUtil.Assert(base.GetComponent<KBatchedAnimController>() != null, "SymbolOverrideController requires KBatchedAnimController");
		DebugUtil.Assert(base.GetComponent<KBatchedAnimController>().usingNewSymbolOverrideSystem, "SymbolOverrideController requires usingNewSymbolOverrideSystem to be set to true. Try adding the component by calling: SymbolOverrideControllerUtil.AddToPrefab");
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			symbolEntry.sourceSymbol = KAnimBatchManager.Instance().GetBatchGroupData(symbolEntry.sourceSymbolBatchTag).GetSymbol(symbolEntry.sourceSymbolId);
			this.symbolOverrides[i] = symbolEntry;
		}
		this.atlases = new KAnimBatch.AtlasList(0, KAnimBatchManager.MaxAtlasesByMaterialType[(int)this.animController.materialType]);
		this.faceGraph = base.GetComponent<FaceGraph>();
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x000A70C4 File Offset: 0x000A52C4
	public int AddSymbolOverride(HashedString target_symbol, KAnim.Build.Symbol source_symbol, int priority = 0)
	{
		if (source_symbol == null)
		{
			throw new Exception("NULL source symbol when overriding: " + target_symbol.ToString());
		}
		SymbolOverrideController.SymbolEntry symbolEntry = new SymbolOverrideController.SymbolEntry
		{
			targetSymbol = target_symbol,
			sourceSymbol = source_symbol,
			sourceSymbolId = new HashedString(source_symbol.hash.HashValue),
			sourceSymbolBatchTag = source_symbol.build.batchTag,
			priority = priority
		};
		int num = this.GetSymbolOverrideIdx(target_symbol, priority);
		if (num >= 0)
		{
			this.symbolOverrides[num] = symbolEntry;
		}
		else
		{
			num = this.symbolOverrides.Count;
			this.symbolOverrides.Add(symbolEntry);
		}
		this.MarkDirty();
		return num;
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x000A7178 File Offset: 0x000A5378
	public bool RemoveSymbolOverride(HashedString target_symbol, int priority = 0)
	{
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			if (symbolEntry.targetSymbol == target_symbol && symbolEntry.priority == priority)
			{
				this.symbolOverrides.RemoveAt(i);
				this.MarkDirty();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x000A71D4 File Offset: 0x000A53D4
	public void RemoveAllSymbolOverrides(int priority = 0)
	{
		this.symbolOverrides.RemoveAll((SymbolOverrideController.SymbolEntry x) => x.priority >= priority);
		this.MarkDirty();
	}

	// Token: 0x06001EC3 RID: 7875 RVA: 0x000A720C File Offset: 0x000A540C
	public int GetSymbolOverrideIdx(HashedString target_symbol, int priority = 0)
	{
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			if (symbolEntry.targetSymbol == target_symbol && symbolEntry.priority == priority)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x000A7256 File Offset: 0x000A5456
	public int GetAtlasIdx(Texture2D atlas)
	{
		return this.atlases.GetAtlasIdx(atlas);
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x000A7264 File Offset: 0x000A5464
	public void ApplyOverrides()
	{
		if (this.requiresSorting)
		{
			this.symbolOverrides.Sort((SymbolOverrideController.SymbolEntry x, SymbolOverrideController.SymbolEntry y) => x.priority - y.priority);
			this.requiresSorting = false;
		}
		KAnimBatch batch = this.animController.GetBatch();
		DebugUtil.Assert(batch != null);
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.animController.batchGroupID);
		int count = batch.atlases.Count;
		this.atlases.Clear(count);
		DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.PooledDictionary pooledDictionary = DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.Allocate();
		ListPool<SymbolOverrideController.SymbolEntry, SymbolOverrideController>.PooledList pooledList = ListPool<SymbolOverrideController.SymbolEntry, SymbolOverrideController>.Allocate();
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			Pair<int, int> pair;
			if (pooledDictionary.TryGetValue(symbolEntry.targetSymbol, out pair))
			{
				int first = pair.first;
				if (symbolEntry.priority > first)
				{
					int second = pair.second;
					pooledDictionary[symbolEntry.targetSymbol] = new Pair<int, int>(symbolEntry.priority, second);
					pooledList[second] = symbolEntry;
				}
			}
			else
			{
				pooledDictionary[symbolEntry.targetSymbol] = new Pair<int, int>(symbolEntry.priority, pooledList.Count);
				pooledList.Add(symbolEntry);
			}
		}
		DictionaryPool<KAnim.Build, SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.PooledDictionary pooledDictionary2 = DictionaryPool<KAnim.Build, SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.Allocate();
		for (int j = 0; j < pooledList.Count; j++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry2 = pooledList[j];
			SymbolOverrideController.BatchGroupInfo batchGroupInfo;
			if (!pooledDictionary2.TryGetValue(symbolEntry2.sourceSymbol.build, out batchGroupInfo))
			{
				batchGroupInfo = new SymbolOverrideController.BatchGroupInfo
				{
					build = symbolEntry2.sourceSymbol.build,
					data = KAnimBatchManager.Instance().GetBatchGroupData(symbolEntry2.sourceSymbol.build.batchTag)
				};
				Texture2D texture = symbolEntry2.sourceSymbol.build.GetTexture(0);
				int num = batch.atlases.GetAtlasIdx(texture);
				if (num < 0)
				{
					num = this.atlases.Add(texture);
				}
				batchGroupInfo.atlasIdx = num;
				pooledDictionary2[batchGroupInfo.build] = batchGroupInfo;
			}
			KAnim.Build.Symbol symbol = batchGroupData.GetSymbol(symbolEntry2.targetSymbol);
			if (symbol != null)
			{
				this.animController.SetSymbolOverrides(symbol.firstFrameIdx, symbol.numFrames, batchGroupInfo.atlasIdx, batchGroupInfo.data, symbolEntry2.sourceSymbol.firstFrameIdx, symbolEntry2.sourceSymbol.numFrames);
			}
		}
		pooledDictionary2.Recycle();
		pooledList.Recycle();
		pooledDictionary.Recycle();
		if (this.faceGraph != null)
		{
			this.faceGraph.ApplyShape();
		}
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x000A7508 File Offset: 0x000A5708
	public void ApplyAtlases()
	{
		KAnimBatch batch = this.animController.GetBatch();
		this.atlases.Apply(batch.matProperties);
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x000A7532 File Offset: 0x000A5732
	public KAnimBatch.AtlasList GetAtlasList()
	{
		return this.atlases;
	}

	// Token: 0x06001EC8 RID: 7880 RVA: 0x000A753C File Offset: 0x000A573C
	public void MarkDirty()
	{
		if (this.animController != null)
		{
			this.animController.SetDirty();
		}
		int version = this.version + 1;
		this.version = version;
		this.requiresSorting = true;
	}

	// Token: 0x040011EB RID: 4587
	public bool applySymbolOverridesEveryFrame;

	// Token: 0x040011EC RID: 4588
	[SerializeField]
	private List<SymbolOverrideController.SymbolEntry> symbolOverrides = new List<SymbolOverrideController.SymbolEntry>();

	// Token: 0x040011ED RID: 4589
	private KAnimBatch.AtlasList atlases;

	// Token: 0x040011EE RID: 4590
	private KBatchedAnimController animController;

	// Token: 0x040011EF RID: 4591
	private FaceGraph faceGraph;

	// Token: 0x040011F1 RID: 4593
	private bool requiresSorting;

	// Token: 0x020013F6 RID: 5110
	[Serializable]
	public struct SymbolEntry
	{
		// Token: 0x04006CCE RID: 27854
		public HashedString targetSymbol;

		// Token: 0x04006CCF RID: 27855
		[NonSerialized]
		public KAnim.Build.Symbol sourceSymbol;

		// Token: 0x04006CD0 RID: 27856
		public HashedString sourceSymbolId;

		// Token: 0x04006CD1 RID: 27857
		public HashedString sourceSymbolBatchTag;

		// Token: 0x04006CD2 RID: 27858
		public int priority;
	}

	// Token: 0x020013F7 RID: 5111
	private struct SymbolToOverride
	{
		// Token: 0x04006CD3 RID: 27859
		public KAnim.Build.Symbol sourceSymbol;

		// Token: 0x04006CD4 RID: 27860
		public HashedString targetSymbol;

		// Token: 0x04006CD5 RID: 27861
		public KBatchGroupData data;

		// Token: 0x04006CD6 RID: 27862
		public int atlasIdx;
	}

	// Token: 0x020013F8 RID: 5112
	private struct BatchGroupInfo
	{
		// Token: 0x04006CD7 RID: 27863
		public KAnim.Build build;

		// Token: 0x04006CD8 RID: 27864
		public int atlasIdx;

		// Token: 0x04006CD9 RID: 27865
		public KBatchGroupData data;
	}
}
