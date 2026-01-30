using System;
using Klei.AI;

// Token: 0x0200048B RID: 1163
public class RobotPathFinderAbilities : PathFinderAbilities
{
	// Token: 0x060018AC RID: 6316 RVA: 0x00088F9C File Offset: 0x0008719C
	public RobotPathFinderAbilities(Navigator navigator) : base(navigator)
	{
		KPrefabID component = navigator.GetComponent<KPrefabID>();
		this.prefabTag = component.PrefabTag;
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x00088FC4 File Offset: 0x000871C4
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

	// Token: 0x060018AE RID: 6318 RVA: 0x00089006 File Offset: 0x00087206
	public override bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged)
	{
		return (!submerged || this.canTraverseSubmered) && RobotPathFinderAbilities.IsAccessPermitted(this.prefabTag, path.cell, from_cell, from_nav_type);
	}

	// Token: 0x060018AF RID: 6319 RVA: 0x00089030 File Offset: 0x00087230
	private static bool IsAccessPermitted(Tag prefabTag, int cell, int from_cell, NavType from_nav_type)
	{
		int tagId = GridRestrictionSerializer.Instance.GetTagId(prefabTag);
		int tagId2 = GridRestrictionSerializer.Instance.GetTagId(GameTags.Robot);
		return Grid.HasPermission(cell, tagId, tagId2, from_cell, from_nav_type);
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x00089063 File Offset: 0x00087263
	public override PathFinderAbilities Clone()
	{
		return new RobotPathFinderAbilities(this.navigator)
		{
			prefabInstanceID = this.prefabInstanceID,
			canTraverseSubmered = this.canTraverseSubmered,
			prefabTag = this.prefabTag
		};
	}

	// Token: 0x060018B1 RID: 6321 RVA: 0x00089094 File Offset: 0x00087294
	public override void RecycleClone()
	{
	}

	// Token: 0x04000E49 RID: 3657
	public bool canTraverseSubmered;

	// Token: 0x04000E4A RID: 3658
	private Tag prefabTag;
}
