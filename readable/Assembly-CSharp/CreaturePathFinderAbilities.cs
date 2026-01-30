using System;
using Klei.AI;

// Token: 0x0200048A RID: 1162
public class CreaturePathFinderAbilities : PathFinderAbilities
{
	// Token: 0x060018A7 RID: 6311 RVA: 0x00088F14 File Offset: 0x00087114
	public CreaturePathFinderAbilities(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x00088F20 File Offset: 0x00087120
	protected override void Refresh(Navigator navigator)
	{
		if (PathFinder.IsSubmerged(navigator.cachedCell))
		{
			this.canTraverseSubmered = true;
			return;
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.MaxUnderwaterTravelCost.Lookup(navigator);
		this.canTraverseSubmered = (attributeInstance == null);
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x00088F62 File Offset: 0x00087162
	public override bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged)
	{
		return !submerged || this.canTraverseSubmered;
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x00088F73 File Offset: 0x00087173
	public override PathFinderAbilities Clone()
	{
		return new CreaturePathFinderAbilities(this.navigator)
		{
			prefabInstanceID = this.prefabInstanceID,
			canTraverseSubmered = this.canTraverseSubmered
		};
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x00088F98 File Offset: 0x00087198
	public override void RecycleClone()
	{
	}

	// Token: 0x04000E48 RID: 3656
	public bool canTraverseSubmered;
}
