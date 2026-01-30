using System;
using System.Collections.Generic;

// Token: 0x02000865 RID: 2149
public class Faction
{
	// Token: 0x06003AFE RID: 15102 RVA: 0x00148F2C File Offset: 0x0014712C
	public HashSet<FactionAlignment> HostileTo()
	{
		HashSet<FactionAlignment> hashSet = new HashSet<FactionAlignment>();
		foreach (KeyValuePair<FactionManager.FactionID, FactionManager.Disposition> keyValuePair in this.Dispositions)
		{
			if (keyValuePair.Value == FactionManager.Disposition.Attack)
			{
				hashSet.UnionWith(FactionManager.Instance.GetFaction(keyValuePair.Key).Members);
			}
		}
		return hashSet;
	}

	// Token: 0x06003AFF RID: 15103 RVA: 0x00148FA8 File Offset: 0x001471A8
	public Faction(FactionManager.FactionID faction)
	{
		this.ID = faction;
		this.ConfigureAlignments(faction);
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06003B00 RID: 15104 RVA: 0x00148FED File Offset: 0x001471ED
	// (set) Token: 0x06003B01 RID: 15105 RVA: 0x00148FF5 File Offset: 0x001471F5
	public bool CanAttack { get; private set; }

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x06003B02 RID: 15106 RVA: 0x00148FFE File Offset: 0x001471FE
	// (set) Token: 0x06003B03 RID: 15107 RVA: 0x00149006 File Offset: 0x00147206
	public bool CanAssist { get; private set; }

	// Token: 0x06003B04 RID: 15108 RVA: 0x00149010 File Offset: 0x00147210
	private void ConfigureAlignments(FactionManager.FactionID faction)
	{
		switch (faction)
		{
		case FactionManager.FactionID.Duplicant:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Friendly:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Hostile:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Attack);
			break;
		case FactionManager.FactionID.Prey:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Predator:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Attack);
			break;
		case FactionManager.FactionID.Pest:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		}
		foreach (KeyValuePair<FactionManager.FactionID, FactionManager.Disposition> keyValuePair in this.Dispositions)
		{
			if (keyValuePair.Value == FactionManager.Disposition.Attack)
			{
				this.CanAttack = true;
			}
			if (keyValuePair.Value == FactionManager.Disposition.Assist)
			{
				this.CanAssist = true;
			}
		}
	}

	// Token: 0x040023E9 RID: 9193
	public HashSet<FactionAlignment> Members = new HashSet<FactionAlignment>();

	// Token: 0x040023EA RID: 9194
	public FactionManager.FactionID ID;

	// Token: 0x040023EB RID: 9195
	public Dictionary<FactionManager.FactionID, FactionManager.Disposition> Dispositions = new Dictionary<FactionManager.FactionID, FactionManager.Disposition>(default(Faction.FactionIDComparer));

	// Token: 0x02001823 RID: 6179
	public struct FactionIDComparer : IEqualityComparer<FactionManager.FactionID>
	{
		// Token: 0x06009DE1 RID: 40417 RVA: 0x003A12A6 File Offset: 0x0039F4A6
		public bool Equals(FactionManager.FactionID x, FactionManager.FactionID y)
		{
			return x == y;
		}

		// Token: 0x06009DE2 RID: 40418 RVA: 0x003A12AC File Offset: 0x0039F4AC
		public int GetHashCode(FactionManager.FactionID obj)
		{
			return (int)obj;
		}
	}
}
