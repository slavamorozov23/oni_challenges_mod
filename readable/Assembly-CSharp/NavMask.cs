using System;

// Token: 0x020004F3 RID: 1267
public class NavMask
{
	// Token: 0x06001B6C RID: 7020 RVA: 0x00097EAA File Offset: 0x000960AA
	public virtual bool IsTraversable(PathFinder.PotentialPath path, int from_cell, int cost, int transition_id, PathFinderAbilities abilities)
	{
		return true;
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x00097EAD File Offset: 0x000960AD
	public virtual void ApplyTraversalToPath(ref PathFinder.PotentialPath path, int from_cell)
	{
	}
}
