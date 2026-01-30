using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200056A RID: 1386
[AddComponentMenu("KMonoBehaviour/scripts/AudioEventManager")]
public class AudioEventManager : KMonoBehaviour
{
	// Token: 0x06001EE3 RID: 7907 RVA: 0x000A7FE0 File Offset: 0x000A61E0
	public static AudioEventManager Get()
	{
		if (AudioEventManager.instance == null)
		{
			if (App.IsExiting)
			{
				return null;
			}
			GameObject gameObject = GameObject.Find("/AudioEventManager");
			if (gameObject == null)
			{
				gameObject = new GameObject();
				gameObject.name = "AudioEventManager";
			}
			AudioEventManager.instance = gameObject.GetComponent<AudioEventManager>();
			if (AudioEventManager.instance == null)
			{
				AudioEventManager.instance = gameObject.AddComponent<AudioEventManager>();
			}
		}
		return AudioEventManager.instance;
	}

	// Token: 0x06001EE4 RID: 7908 RVA: 0x000A8050 File Offset: 0x000A6250
	protected override void OnSpawn()
	{
		base.OnPrefabInit();
		this.spatialSplats.Reset(Grid.WidthInCells, Grid.HeightInCells, 16, 16);
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x000A8071 File Offset: 0x000A6271
	public static float LoudnessToDB(float loudness)
	{
		if (loudness <= 0f)
		{
			return 0f;
		}
		return 10f * Mathf.Log10(loudness);
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x000A808D File Offset: 0x000A628D
	public static float DBToLoudness(float src_db)
	{
		return Mathf.Pow(10f, src_db / 10f);
	}

	// Token: 0x06001EE7 RID: 7911 RVA: 0x000A80A0 File Offset: 0x000A62A0
	public float GetDecibelsAtCell(int cell)
	{
		return Mathf.Round(AudioEventManager.LoudnessToDB(Grid.Loudness[cell]) * 2f) / 2f;
	}

	// Token: 0x06001EE8 RID: 7912 RVA: 0x000A80C0 File Offset: 0x000A62C0
	public static string GetLoudestNoisePolluterAtCell(int cell)
	{
		float negativeInfinity = float.NegativeInfinity;
		string result = null;
		AudioEventManager audioEventManager = AudioEventManager.Get();
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2 pos = new Vector2((float)vector2I.x, (float)vector2I.y);
		foreach (object obj in audioEventManager.spatialSplats.GetAllIntersecting(pos))
		{
			NoiseSplat noiseSplat = (NoiseSplat)obj;
			if (noiseSplat.GetLoudness(cell) > negativeInfinity)
			{
				result = noiseSplat.GetProvider().GetName();
			}
		}
		return result;
	}

	// Token: 0x06001EE9 RID: 7913 RVA: 0x000A8164 File Offset: 0x000A6364
	public void ClearNoiseSplat(NoiseSplat splat)
	{
		if (this.splats.Contains(splat))
		{
			this.splats.Remove(splat);
			this.spatialSplats.Remove(splat);
		}
	}

	// Token: 0x06001EEA RID: 7914 RVA: 0x000A818D File Offset: 0x000A638D
	public void AddSplat(NoiseSplat splat)
	{
		this.splats.Add(splat);
		this.spatialSplats.Add(splat);
	}

	// Token: 0x06001EEB RID: 7915 RVA: 0x000A81A8 File Offset: 0x000A63A8
	public NoiseSplat CreateNoiseSplat(Vector2 pos, int dB, int radius, string name, GameObject go)
	{
		Polluter polluter = this.GetPolluter(radius);
		polluter.SetAttributes(pos, dB, go, name);
		NoiseSplat noiseSplat = new NoiseSplat(polluter, 0f);
		polluter.SetSplat(noiseSplat);
		return noiseSplat;
	}

	// Token: 0x06001EEC RID: 7916 RVA: 0x000A81DC File Offset: 0x000A63DC
	public List<AudioEventManager.PolluterDisplay> GetPollutersForCell(int cell)
	{
		this.polluters.Clear();
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2 pos = new Vector2((float)vector2I.x, (float)vector2I.y);
		foreach (object obj in this.spatialSplats.GetAllIntersecting(pos))
		{
			NoiseSplat noiseSplat = (NoiseSplat)obj;
			float loudness = noiseSplat.GetLoudness(cell);
			if (loudness > 0f)
			{
				AudioEventManager.PolluterDisplay item = default(AudioEventManager.PolluterDisplay);
				item.name = noiseSplat.GetName();
				item.value = AudioEventManager.LoudnessToDB(loudness);
				item.provider = noiseSplat.GetProvider();
				this.polluters.Add(item);
			}
		}
		return this.polluters;
	}

	// Token: 0x06001EED RID: 7917 RVA: 0x000A82B4 File Offset: 0x000A64B4
	private void RemoveExpiredSplats()
	{
		if (this.removeTime.Count > 1)
		{
			this.removeTime.Sort((Pair<float, NoiseSplat> a, Pair<float, NoiseSplat> b) => a.first.CompareTo(b.first));
		}
		int num = -1;
		int num2 = 0;
		while (num2 < this.removeTime.Count && this.removeTime[num2].first <= Time.time)
		{
			NoiseSplat second = this.removeTime[num2].second;
			if (second != null)
			{
				IPolluter provider = second.GetProvider();
				this.FreePolluter(provider as Polluter);
			}
			num = num2;
			num2++;
		}
		for (int i = num; i >= 0; i--)
		{
			this.removeTime.RemoveAt(i);
		}
	}

	// Token: 0x06001EEE RID: 7918 RVA: 0x000A8370 File Offset: 0x000A6570
	private void Update()
	{
		this.RemoveExpiredSplats();
	}

	// Token: 0x06001EEF RID: 7919 RVA: 0x000A8378 File Offset: 0x000A6578
	private Polluter GetPolluter(int radius)
	{
		if (!this.freePool.ContainsKey(radius))
		{
			this.freePool.Add(radius, new List<Polluter>());
		}
		Polluter polluter;
		if (this.freePool[radius].Count > 0)
		{
			polluter = this.freePool[radius][0];
			this.freePool[radius].RemoveAt(0);
		}
		else
		{
			polluter = new Polluter(radius);
		}
		if (!this.inusePool.ContainsKey(radius))
		{
			this.inusePool.Add(radius, new List<Polluter>());
		}
		this.inusePool[radius].Add(polluter);
		return polluter;
	}

	// Token: 0x06001EF0 RID: 7920 RVA: 0x000A841C File Offset: 0x000A661C
	private void FreePolluter(Polluter pol)
	{
		if (pol != null)
		{
			pol.Clear();
			global::Debug.Assert(this.inusePool[pol.radius].Contains(pol));
			this.inusePool[pol.radius].Remove(pol);
			this.freePool[pol.radius].Add(pol);
		}
	}

	// Token: 0x06001EF1 RID: 7921 RVA: 0x000A8480 File Offset: 0x000A6680
	public void PlayTimedOnceOff(Vector2 pos, int dB, int radius, string name, GameObject go, float time = 1f)
	{
		if (dB > 0 && radius > 0 && time > 0f)
		{
			Polluter polluter = this.GetPolluter(radius);
			polluter.SetAttributes(pos, dB, go, name);
			this.AddTimedInstance(polluter, time);
		}
	}

	// Token: 0x06001EF2 RID: 7922 RVA: 0x000A84BC File Offset: 0x000A66BC
	private void AddTimedInstance(Polluter p, float time)
	{
		NoiseSplat noiseSplat = new NoiseSplat(p, time + Time.time);
		p.SetSplat(noiseSplat);
		this.removeTime.Add(new Pair<float, NoiseSplat>(time + Time.time, noiseSplat));
	}

	// Token: 0x06001EF3 RID: 7923 RVA: 0x000A84F6 File Offset: 0x000A66F6
	private static void SoundLog(long itemId, string message)
	{
		global::Debug.Log(" [" + itemId.ToString() + "] \t" + message);
	}

	// Token: 0x04001203 RID: 4611
	public const float NO_NOISE_EFFECTORS = 0f;

	// Token: 0x04001204 RID: 4612
	public const float MIN_LOUDNESS_THRESHOLD = 1f;

	// Token: 0x04001205 RID: 4613
	private static AudioEventManager instance;

	// Token: 0x04001206 RID: 4614
	private List<Pair<float, NoiseSplat>> removeTime = new List<Pair<float, NoiseSplat>>();

	// Token: 0x04001207 RID: 4615
	private Dictionary<int, List<Polluter>> freePool = new Dictionary<int, List<Polluter>>();

	// Token: 0x04001208 RID: 4616
	private Dictionary<int, List<Polluter>> inusePool = new Dictionary<int, List<Polluter>>();

	// Token: 0x04001209 RID: 4617
	private HashSet<NoiseSplat> splats = new HashSet<NoiseSplat>();

	// Token: 0x0400120A RID: 4618
	private UniformGrid<NoiseSplat> spatialSplats = new UniformGrid<NoiseSplat>();

	// Token: 0x0400120B RID: 4619
	private List<AudioEventManager.PolluterDisplay> polluters = new List<AudioEventManager.PolluterDisplay>();

	// Token: 0x02001400 RID: 5120
	public enum NoiseEffect
	{
		// Token: 0x04006D0F RID: 27919
		Peaceful,
		// Token: 0x04006D10 RID: 27920
		Quiet = 36,
		// Token: 0x04006D11 RID: 27921
		TossAndTurn = 45,
		// Token: 0x04006D12 RID: 27922
		WakeUp = 60,
		// Token: 0x04006D13 RID: 27923
		Passive = 80,
		// Token: 0x04006D14 RID: 27924
		Active = 106
	}

	// Token: 0x02001401 RID: 5121
	public struct PolluterDisplay
	{
		// Token: 0x04006D15 RID: 27925
		public string name;

		// Token: 0x04006D16 RID: 27926
		public float value;

		// Token: 0x04006D17 RID: 27927
		public IPolluter provider;
	}
}
