using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B53 RID: 2899
public abstract class SlicedUpdaterSim1000ms<T> : KMonoBehaviour, ISim200ms where T : KMonoBehaviour, ISlicedSim1000ms
{
	// Token: 0x06005598 RID: 21912 RVA: 0x001F3B17 File Offset: 0x001F1D17
	protected override void OnPrefabInit()
	{
		this.InitializeSlices();
		base.OnPrefabInit();
		SlicedUpdaterSim1000ms<T>.instance = this;
	}

	// Token: 0x06005599 RID: 21913 RVA: 0x001F3B2B File Offset: 0x001F1D2B
	protected override void OnForcedCleanUp()
	{
		SlicedUpdaterSim1000ms<T>.instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600559A RID: 21914 RVA: 0x001F3B3C File Offset: 0x001F1D3C
	private void InitializeSlices()
	{
		int num = SlicedUpdaterSim1000ms<T>.NUM_200MS_BUCKETS * this.numSlicesPer200ms;
		this.m_slices = new List<SlicedUpdaterSim1000ms<T>.Slice>();
		for (int i = 0; i < num; i++)
		{
			this.m_slices.Add(new SlicedUpdaterSim1000ms<T>.Slice());
		}
		this.m_nextSliceIdx = 0;
	}

	// Token: 0x0600559B RID: 21915 RVA: 0x001F3B84 File Offset: 0x001F1D84
	private int GetSliceIdx(T toBeUpdated)
	{
		return Mathf.Abs(toBeUpdated.GetComponent<KPrefabID>().InstanceID) % this.m_slices.Count;
	}

	// Token: 0x0600559C RID: 21916 RVA: 0x001F3BA8 File Offset: 0x001F1DA8
	public void RegisterUpdate1000ms(T toBeUpdated)
	{
		SlicedUpdaterSim1000ms<T>.Slice slice = this.m_slices[this.GetSliceIdx(toBeUpdated)];
		slice.Register(toBeUpdated);
		DebugUtil.DevAssert(slice.Count < this.maxUpdatesPer200ms, string.Format("The SlicedUpdaterSim1000ms for {0} wants to update no more than {1} instances per 200ms tick, but a slice has grown more than the SlicedUpdaterSim1000ms can support.", typeof(T).Name, this.maxUpdatesPer200ms), null);
	}

	// Token: 0x0600559D RID: 21917 RVA: 0x001F3C05 File Offset: 0x001F1E05
	public void UnregisterUpdate1000ms(T toBeUpdated)
	{
		this.m_slices[this.GetSliceIdx(toBeUpdated)].Unregister(toBeUpdated);
	}

	// Token: 0x0600559E RID: 21918 RVA: 0x001F3C20 File Offset: 0x001F1E20
	public void Sim200ms(float dt)
	{
		foreach (SlicedUpdaterSim1000ms<T>.Slice slice in this.m_slices)
		{
			slice.IncrementDt(dt);
		}
		int num = 0;
		int i = 0;
		while (i < this.numSlicesPer200ms)
		{
			SlicedUpdaterSim1000ms<T>.Slice slice2 = this.m_slices[this.m_nextSliceIdx];
			num += slice2.Count;
			if (num > this.maxUpdatesPer200ms && i > 0)
			{
				break;
			}
			slice2.Update();
			i++;
			this.m_nextSliceIdx = (this.m_nextSliceIdx + 1) % this.m_slices.Count;
		}
	}

	// Token: 0x040039C9 RID: 14793
	private static int NUM_200MS_BUCKETS = 5;

	// Token: 0x040039CA RID: 14794
	public static SlicedUpdaterSim1000ms<T> instance;

	// Token: 0x040039CB RID: 14795
	[Serialize]
	public int maxUpdatesPer200ms = 300;

	// Token: 0x040039CC RID: 14796
	[Serialize]
	public int numSlicesPer200ms = 3;

	// Token: 0x040039CD RID: 14797
	private List<SlicedUpdaterSim1000ms<T>.Slice> m_slices;

	// Token: 0x040039CE RID: 14798
	private int m_nextSliceIdx;

	// Token: 0x02001CB9 RID: 7353
	private class Slice
	{
		// Token: 0x0600AE5E RID: 44638 RVA: 0x003D3312 File Offset: 0x003D1512
		public void Register(T toBeUpdated)
		{
			if (this.m_timeSinceLastUpdate == 0f)
			{
				this.m_updateList.Add(toBeUpdated);
				return;
			}
			this.m_recentlyAdded[toBeUpdated] = 0f;
		}

		// Token: 0x0600AE5F RID: 44639 RVA: 0x003D333F File Offset: 0x003D153F
		public void Unregister(T toBeUpdated)
		{
			if (!this.m_updateList.Remove(toBeUpdated))
			{
				this.m_recentlyAdded.Remove(toBeUpdated);
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x0600AE60 RID: 44640 RVA: 0x003D335C File Offset: 0x003D155C
		public int Count
		{
			get
			{
				return this.m_updateList.Count + this.m_recentlyAdded.Count;
			}
		}

		// Token: 0x0600AE61 RID: 44641 RVA: 0x003D3375 File Offset: 0x003D1575
		public List<T> GetUpdateList()
		{
			List<T> list = new List<T>();
			list.AddRange(this.m_updateList);
			list.AddRange(this.m_recentlyAdded.Keys);
			return list;
		}

		// Token: 0x0600AE62 RID: 44642 RVA: 0x003D339C File Offset: 0x003D159C
		public void Update()
		{
			foreach (T t in this.m_updateList)
			{
				t.SlicedSim1000ms(this.m_timeSinceLastUpdate);
			}
			foreach (KeyValuePair<T, float> keyValuePair in this.m_recentlyAdded)
			{
				keyValuePair.Key.SlicedSim1000ms(keyValuePair.Value);
				this.m_updateList.Add(keyValuePair.Key);
			}
			this.m_recentlyAdded.Clear();
			this.m_timeSinceLastUpdate = 0f;
		}

		// Token: 0x0600AE63 RID: 44643 RVA: 0x003D3474 File Offset: 0x003D1674
		public void IncrementDt(float dt)
		{
			this.m_timeSinceLastUpdate += dt;
			if (this.m_recentlyAdded.Count > 0)
			{
				foreach (T t in new List<T>(this.m_recentlyAdded.Keys))
				{
					Dictionary<T, float> recentlyAdded = this.m_recentlyAdded;
					T key = t;
					recentlyAdded[key] += dt;
				}
			}
		}

		// Token: 0x04008908 RID: 35080
		private float m_timeSinceLastUpdate;

		// Token: 0x04008909 RID: 35081
		private List<T> m_updateList = new List<T>();

		// Token: 0x0400890A RID: 35082
		private Dictionary<T, float> m_recentlyAdded = new Dictionary<T, float>();
	}
}
