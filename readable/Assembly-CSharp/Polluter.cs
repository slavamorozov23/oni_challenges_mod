using System;
using UnityEngine;

// Token: 0x02000A6F RID: 2671
public class Polluter : IPolluter
{
	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06004D91 RID: 19857 RVA: 0x001C30E4 File Offset: 0x001C12E4
	// (set) Token: 0x06004D92 RID: 19858 RVA: 0x001C30EC File Offset: 0x001C12EC
	public int radius
	{
		get
		{
			return this._radius;
		}
		private set
		{
			this._radius = value;
			if (this._radius == 0)
			{
				global::Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", new object[]
				{
					this.GetName()
				});
				return;
			}
		}
	}

	// Token: 0x06004D93 RID: 19859 RVA: 0x001C3117 File Offset: 0x001C1317
	public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
	{
		this.position = pos;
		this.sourceName = name;
		this.decibels = dB;
		this.gameObject = go;
	}

	// Token: 0x06004D94 RID: 19860 RVA: 0x001C3136 File Offset: 0x001C1336
	public string GetName()
	{
		return this.sourceName;
	}

	// Token: 0x06004D95 RID: 19861 RVA: 0x001C313E File Offset: 0x001C133E
	public int GetRadius()
	{
		return this.radius;
	}

	// Token: 0x06004D96 RID: 19862 RVA: 0x001C3146 File Offset: 0x001C1346
	public int GetNoise()
	{
		return this.decibels;
	}

	// Token: 0x06004D97 RID: 19863 RVA: 0x001C314E File Offset: 0x001C134E
	public GameObject GetGameObject()
	{
		return this.gameObject;
	}

	// Token: 0x06004D98 RID: 19864 RVA: 0x001C3156 File Offset: 0x001C1356
	public Polluter(int radius)
	{
		this.radius = radius;
	}

	// Token: 0x06004D99 RID: 19865 RVA: 0x001C3165 File Offset: 0x001C1365
	public void SetSplat(NoiseSplat new_splat)
	{
		if (new_splat == null && this.splat != null)
		{
			this.Clear();
		}
		this.splat = new_splat;
		if (this.splat != null)
		{
			AudioEventManager.Get().AddSplat(this.splat);
		}
	}

	// Token: 0x06004D9A RID: 19866 RVA: 0x001C3197 File Offset: 0x001C1397
	public void Clear()
	{
		if (this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
			this.splat = null;
		}
	}

	// Token: 0x06004D9B RID: 19867 RVA: 0x001C31C3 File Offset: 0x001C13C3
	public Vector2 GetPosition()
	{
		return this.position;
	}

	// Token: 0x040033B0 RID: 13232
	private int _radius;

	// Token: 0x040033B1 RID: 13233
	private int decibels;

	// Token: 0x040033B2 RID: 13234
	private Vector2 position;

	// Token: 0x040033B3 RID: 13235
	private string sourceName;

	// Token: 0x040033B4 RID: 13236
	private GameObject gameObject;

	// Token: 0x040033B5 RID: 13237
	private NoiseSplat splat;
}
