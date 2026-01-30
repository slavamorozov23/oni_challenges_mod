using System;

// Token: 0x020004FA RID: 1274
public abstract class PathFinderAbilities
{
	// Token: 0x06001B89 RID: 7049 RVA: 0x00098A2D File Offset: 0x00096C2D
	public PathFinderAbilities(Navigator navigator)
	{
		this.navigator = navigator;
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x00098A3C File Offset: 0x00096C3C
	public virtual string KPROFILER_getName()
	{
		return null;
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x00098A3F File Offset: 0x00096C3F
	public void Refresh()
	{
		this.prefabInstanceID = this.navigator.gameObject.GetComponent<KPrefabID>().InstanceID;
		this.navigator.cachedCell = Grid.PosToCell(this.navigator);
		this.Refresh(this.navigator);
	}

	// Token: 0x06001B8C RID: 7052
	protected abstract void Refresh(Navigator navigator);

	// Token: 0x06001B8D RID: 7053
	public abstract PathFinderAbilities Clone();

	// Token: 0x06001B8E RID: 7054
	public abstract void RecycleClone();

	// Token: 0x06001B8F RID: 7055
	public abstract bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged);

	// Token: 0x06001B90 RID: 7056 RVA: 0x00098A7E File Offset: 0x00096C7E
	public virtual int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
	{
		return 0;
	}

	// Token: 0x0400100F RID: 4111
	protected Navigator navigator;

	// Token: 0x04001010 RID: 4112
	protected int prefabInstanceID;
}
