using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000929 RID: 2345
[AddComponentMenu("KMonoBehaviour/scripts/EntombedItemVisualizer")]
public class EntombedItemVisualizer : KMonoBehaviour
{
	// Token: 0x0600419A RID: 16794 RVA: 0x001727F4 File Offset: 0x001709F4
	public void Clear()
	{
		this.cellEntombedCounts.Clear();
	}

	// Token: 0x0600419B RID: 16795 RVA: 0x00172801 File Offset: 0x00170A01
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.entombedItemPool = new GameObjectPool(new Func<GameObject>(this.InstantiateEntombedObject), delegate(GameObject _)
		{
		}, 32);
	}

	// Token: 0x0600419C RID: 16796 RVA: 0x00172844 File Offset: 0x00170A44
	public bool AddItem(int cell)
	{
		bool result = false;
		if (Grid.Objects[cell, 9] == null)
		{
			result = true;
			EntombedItemVisualizer.Data data;
			this.cellEntombedCounts.TryGetValue(cell, out data);
			if (data.refCount == 0)
			{
				GameObject instance = this.entombedItemPool.GetInstance();
				instance.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront));
				instance.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.value * 360f);
				KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
				int num = UnityEngine.Random.Range(0, EntombedItemVisualizer.EntombedVisualizerAnims.Length);
				string text = EntombedItemVisualizer.EntombedVisualizerAnims[num];
				component.initialAnim = text;
				instance.SetActive(true);
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				data.controller = component;
			}
			data.refCount++;
			this.cellEntombedCounts[cell] = data;
		}
		return result;
	}

	// Token: 0x0600419D RID: 16797 RVA: 0x00172934 File Offset: 0x00170B34
	public void RemoveItem(int cell)
	{
		EntombedItemVisualizer.Data data;
		if (this.cellEntombedCounts.TryGetValue(cell, out data))
		{
			data.refCount--;
			if (data.refCount == 0)
			{
				this.ReleaseVisualizer(cell, data);
				return;
			}
			this.cellEntombedCounts[cell] = data;
		}
	}

	// Token: 0x0600419E RID: 16798 RVA: 0x0017297C File Offset: 0x00170B7C
	public void ForceClear(int cell)
	{
		EntombedItemVisualizer.Data data;
		if (this.cellEntombedCounts.TryGetValue(cell, out data))
		{
			this.ReleaseVisualizer(cell, data);
		}
	}

	// Token: 0x0600419F RID: 16799 RVA: 0x001729A4 File Offset: 0x00170BA4
	private void ReleaseVisualizer(int cell, EntombedItemVisualizer.Data data)
	{
		if (data.controller != null)
		{
			data.controller.gameObject.SetActive(false);
			this.entombedItemPool.ReleaseInstance(data.controller.gameObject);
		}
		this.cellEntombedCounts.Remove(cell);
	}

	// Token: 0x060041A0 RID: 16800 RVA: 0x001729F3 File Offset: 0x00170BF3
	public bool IsEntombedItem(int cell)
	{
		return this.cellEntombedCounts.ContainsKey(cell) && this.cellEntombedCounts[cell].refCount > 0;
	}

	// Token: 0x060041A1 RID: 16801 RVA: 0x00172A19 File Offset: 0x00170C19
	private GameObject InstantiateEntombedObject()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.entombedItemPrefab, Grid.SceneLayer.FXFront, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x040028F5 RID: 10485
	[SerializeField]
	private GameObject entombedItemPrefab;

	// Token: 0x040028F6 RID: 10486
	private static readonly string[] EntombedVisualizerAnims = new string[]
	{
		"idle1",
		"idle2",
		"idle3",
		"idle4"
	};

	// Token: 0x040028F7 RID: 10487
	private GameObjectPool entombedItemPool;

	// Token: 0x040028F8 RID: 10488
	private Dictionary<int, EntombedItemVisualizer.Data> cellEntombedCounts = new Dictionary<int, EntombedItemVisualizer.Data>();

	// Token: 0x02001926 RID: 6438
	private struct Data
	{
		// Token: 0x04007D14 RID: 32020
		public int refCount;

		// Token: 0x04007D15 RID: 32021
		public KBatchedAnimController controller;
	}
}
