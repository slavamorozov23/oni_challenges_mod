using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200057C RID: 1404
public class NoiseSplat : IUniformGridObject
{
	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06001F31 RID: 7985 RVA: 0x000AA1EE File Offset: 0x000A83EE
	// (set) Token: 0x06001F32 RID: 7986 RVA: 0x000AA1F6 File Offset: 0x000A83F6
	public int dB { get; private set; }

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06001F33 RID: 7987 RVA: 0x000AA1FF File Offset: 0x000A83FF
	// (set) Token: 0x06001F34 RID: 7988 RVA: 0x000AA207 File Offset: 0x000A8407
	public float deathTime { get; private set; }

	// Token: 0x06001F35 RID: 7989 RVA: 0x000AA210 File Offset: 0x000A8410
	public string GetName()
	{
		return this.provider.GetName();
	}

	// Token: 0x06001F36 RID: 7990 RVA: 0x000AA21D File Offset: 0x000A841D
	public IPolluter GetProvider()
	{
		return this.provider;
	}

	// Token: 0x06001F37 RID: 7991 RVA: 0x000AA225 File Offset: 0x000A8425
	public Vector2 PosMin()
	{
		return new Vector2(this.position.x - (float)this.radius, this.position.y - (float)this.radius);
	}

	// Token: 0x06001F38 RID: 7992 RVA: 0x000AA252 File Offset: 0x000A8452
	public Vector2 PosMax()
	{
		return new Vector2(this.position.x + (float)this.radius, this.position.y + (float)this.radius);
	}

	// Token: 0x06001F39 RID: 7993 RVA: 0x000AA280 File Offset: 0x000A8480
	public NoiseSplat(NoisePolluter setProvider, float death_time = 0f)
	{
		this.deathTime = death_time;
		this.dB = 0;
		this.radius = 5;
		if (setProvider.dB != null)
		{
			this.dB = (int)setProvider.dB.GetTotalValue();
		}
		int cell = Grid.PosToCell(setProvider.gameObject);
		if (!NoisePolluter.IsNoiseableCell(cell))
		{
			this.dB = 0;
		}
		if (this.dB == 0)
		{
			return;
		}
		setProvider.Clear();
		OccupyArea occupyArea = setProvider.occupyArea;
		this.baseExtents = occupyArea.GetExtents();
		this.provider = setProvider;
		this.position = setProvider.transform.GetPosition();
		if (setProvider.dBRadius != null)
		{
			this.radius = (int)setProvider.dBRadius.GetTotalValue();
		}
		if (this.radius == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int widthInCells = occupyArea.GetWidthInCells();
		int heightInCells = occupyArea.GetHeightInCells();
		Vector2I vector2I = new Vector2I(num - this.radius, num2 - this.radius);
		Vector2I vector2I2 = vector2I + new Vector2I(this.radius * 2 + widthInCells, this.radius * 2 + heightInCells);
		vector2I = Vector2I.Max(vector2I, Vector2I.zero);
		vector2I2 = Vector2I.Min(vector2I2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
		this.effectExtents = new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatCollectNoisePolluters", setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.noisePolluterLayer, setProvider.onCollectNoisePollutersCallback);
		this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatSolidCheck", setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.solidChangedLayer, setProvider.refreshPartionerCallback);
	}

	// Token: 0x06001F3A RID: 7994 RVA: 0x000AA468 File Offset: 0x000A8668
	public NoiseSplat(IPolluter setProvider, float death_time = 0f)
	{
		this.deathTime = death_time;
		this.provider = setProvider;
		this.provider.Clear();
		this.position = this.provider.GetPosition();
		this.dB = this.provider.GetNoise();
		int cell = Grid.PosToCell(this.position);
		if (!NoisePolluter.IsNoiseableCell(cell))
		{
			this.dB = 0;
		}
		if (this.dB == 0)
		{
			return;
		}
		this.radius = this.provider.GetRadius();
		if (this.radius == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		Vector2I vector2I = new Vector2I(num - this.radius, num2 - this.radius);
		Vector2I vector2I2 = vector2I + new Vector2I(this.radius * 2, this.radius * 2);
		vector2I = Vector2I.Max(vector2I, Vector2I.zero);
		vector2I2 = Vector2I.Min(vector2I2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
		this.effectExtents = new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
		this.baseExtents = new Extents(num, num2, 1, 1);
		this.AddNoise();
	}

	// Token: 0x06001F3B RID: 7995 RVA: 0x000AA5B1 File Offset: 0x000A87B1
	public void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
		this.RemoveNoise();
	}

	// Token: 0x06001F3C RID: 7996 RVA: 0x000AA5DC File Offset: 0x000A87DC
	private void AddNoise()
	{
		int cell = Grid.PosToCell(this.position);
		int num = this.effectExtents.x + this.effectExtents.width;
		int num2 = this.effectExtents.y + this.effectExtents.height;
		int num3 = this.effectExtents.x;
		int num4 = this.effectExtents.y;
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		num = Math.Min(num, Grid.WidthInCells);
		num2 = Math.Min(num2, Grid.HeightInCells);
		num3 = Math.Max(0, num3);
		num4 = Math.Max(0, num4);
		for (int i = num4; i < num2; i++)
		{
			for (int j = num3; j < num; j++)
			{
				if (Grid.VisibilityTest(x, y, j, i, false))
				{
					int num5 = Grid.XYToCell(j, i);
					float dbforCell = this.GetDBForCell(num5);
					if (dbforCell > 0f)
					{
						float num6 = AudioEventManager.DBToLoudness(dbforCell);
						Grid.Loudness[num5] += num6;
						Pair<int, float> item = new Pair<int, float>(num5, num6);
						this.decibels.Add(item);
					}
				}
			}
		}
	}

	// Token: 0x06001F3D RID: 7997 RVA: 0x000AA6F4 File Offset: 0x000A88F4
	public float GetDBForCell(int cell)
	{
		Vector2 vector = Grid.CellToPos2D(cell);
		float num = Mathf.Floor(Vector2.Distance(this.position, vector));
		if (vector.x >= (float)this.baseExtents.x && vector.x < (float)(this.baseExtents.x + this.baseExtents.width) && vector.y >= (float)this.baseExtents.y && vector.y < (float)(this.baseExtents.y + this.baseExtents.height))
		{
			num = 0f;
		}
		return Mathf.Round((float)this.dB - (float)this.dB * num * 0.05f);
	}

	// Token: 0x06001F3E RID: 7998 RVA: 0x000AA7AC File Offset: 0x000A89AC
	private void RemoveNoise()
	{
		for (int i = 0; i < this.decibels.Count; i++)
		{
			Pair<int, float> pair = this.decibels[i];
			float num = Math.Max(0f, Grid.Loudness[pair.first] - pair.second);
			Grid.Loudness[pair.first] = ((num < 1f) ? 0f : num);
		}
		this.decibels.Clear();
	}

	// Token: 0x06001F3F RID: 7999 RVA: 0x000AA824 File Offset: 0x000A8A24
	public float GetLoudness(int cell)
	{
		float result = 0f;
		for (int i = 0; i < this.decibels.Count; i++)
		{
			Pair<int, float> pair = this.decibels[i];
			if (pair.first == cell)
			{
				result = pair.second;
				break;
			}
		}
		return result;
	}

	// Token: 0x04001231 RID: 4657
	public const float noiseFalloff = 0.05f;

	// Token: 0x04001234 RID: 4660
	private IPolluter provider;

	// Token: 0x04001235 RID: 4661
	private Vector2 position;

	// Token: 0x04001236 RID: 4662
	private int radius;

	// Token: 0x04001237 RID: 4663
	private Extents effectExtents;

	// Token: 0x04001238 RID: 4664
	private Extents baseExtents;

	// Token: 0x04001239 RID: 4665
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400123A RID: 4666
	private HandleVector<int>.Handle solidChangedPartitionerEntry;

	// Token: 0x0400123B RID: 4667
	private List<Pair<int, float>> decibels = new List<Pair<int, float>>();
}
