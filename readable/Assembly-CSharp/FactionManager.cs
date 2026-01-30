using System;
using UnityEngine;

// Token: 0x02000864 RID: 2148
[AddComponentMenu("KMonoBehaviour/scripts/FactionManager")]
public class FactionManager : KMonoBehaviour
{
	// Token: 0x06003AF8 RID: 15096 RVA: 0x00148E24 File Offset: 0x00147024
	public static void DestroyInstance()
	{
		FactionManager.Instance = null;
	}

	// Token: 0x06003AF9 RID: 15097 RVA: 0x00148E2C File Offset: 0x0014702C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		FactionManager.Instance = this;
	}

	// Token: 0x06003AFA RID: 15098 RVA: 0x00148E3A File Offset: 0x0014703A
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003AFB RID: 15099 RVA: 0x00148E44 File Offset: 0x00147044
	public Faction GetFaction(FactionManager.FactionID faction)
	{
		switch (faction)
		{
		case FactionManager.FactionID.Duplicant:
			return this.Duplicant;
		case FactionManager.FactionID.Friendly:
			return this.Friendly;
		case FactionManager.FactionID.Hostile:
			return this.Hostile;
		case FactionManager.FactionID.Prey:
			return this.Prey;
		case FactionManager.FactionID.Predator:
			return this.Predator;
		case FactionManager.FactionID.Pest:
			return this.Pest;
		default:
			return null;
		}
	}

	// Token: 0x06003AFC RID: 15100 RVA: 0x00148E9C File Offset: 0x0014709C
	public FactionManager.Disposition GetDisposition(FactionManager.FactionID of_faction, FactionManager.FactionID to_faction)
	{
		if (FactionManager.Instance.GetFaction(of_faction).Dispositions.ContainsKey(to_faction))
		{
			return FactionManager.Instance.GetFaction(of_faction).Dispositions[to_faction];
		}
		return FactionManager.Disposition.Neutral;
	}

	// Token: 0x040023E2 RID: 9186
	public static FactionManager Instance;

	// Token: 0x040023E3 RID: 9187
	public Faction Duplicant = new Faction(FactionManager.FactionID.Duplicant);

	// Token: 0x040023E4 RID: 9188
	public Faction Friendly = new Faction(FactionManager.FactionID.Friendly);

	// Token: 0x040023E5 RID: 9189
	public Faction Hostile = new Faction(FactionManager.FactionID.Hostile);

	// Token: 0x040023E6 RID: 9190
	public Faction Predator = new Faction(FactionManager.FactionID.Predator);

	// Token: 0x040023E7 RID: 9191
	public Faction Prey = new Faction(FactionManager.FactionID.Prey);

	// Token: 0x040023E8 RID: 9192
	public Faction Pest = new Faction(FactionManager.FactionID.Pest);

	// Token: 0x02001821 RID: 6177
	public enum FactionID
	{
		// Token: 0x040079E0 RID: 31200
		Duplicant,
		// Token: 0x040079E1 RID: 31201
		Friendly,
		// Token: 0x040079E2 RID: 31202
		Hostile,
		// Token: 0x040079E3 RID: 31203
		Prey,
		// Token: 0x040079E4 RID: 31204
		Predator,
		// Token: 0x040079E5 RID: 31205
		Pest,
		// Token: 0x040079E6 RID: 31206
		NumberOfFactions
	}

	// Token: 0x02001822 RID: 6178
	public enum Disposition
	{
		// Token: 0x040079E8 RID: 31208
		Assist,
		// Token: 0x040079E9 RID: 31209
		Neutral,
		// Token: 0x040079EA RID: 31210
		Attack
	}
}
