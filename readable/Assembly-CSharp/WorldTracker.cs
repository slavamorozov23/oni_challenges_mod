using System;

// Token: 0x02000661 RID: 1633
public abstract class WorldTracker : Tracker
{
	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06002793 RID: 10131 RVA: 0x000E318D File Offset: 0x000E138D
	// (set) Token: 0x06002794 RID: 10132 RVA: 0x000E3195 File Offset: 0x000E1395
	public int WorldID { get; private set; }

	// Token: 0x06002795 RID: 10133 RVA: 0x000E319E File Offset: 0x000E139E
	public WorldTracker(int worldID)
	{
		this.WorldID = worldID;
	}
}
