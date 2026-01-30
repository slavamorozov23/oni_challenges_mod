using System;

// Token: 0x0200048C RID: 1164
public class MinionPathFinderAbilities : PathFinderAbilities
{
	// Token: 0x060018B2 RID: 6322 RVA: 0x00089098 File Offset: 0x00087298
	public MinionPathFinderAbilities(Navigator navigator) : base(navigator)
	{
		this.transitionVoidOffsets = new CellOffset[navigator.NavGrid.transitions.Length][];
		for (int i = 0; i < this.transitionVoidOffsets.Length; i++)
		{
			this.transitionVoidOffsets[i] = navigator.NavGrid.transitions[i].voidOffsets;
		}
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x000890F8 File Offset: 0x000872F8
	protected override void Refresh(Navigator navigator)
	{
		MinionAssignablesProxy minionAssignablesProxy = navigator.GetComponent<MinionIdentity>().assignableProxy.Get();
		this.proxyID = minionAssignablesProxy.GetComponent<KPrefabID>().InstanceID;
		this.accessControlDefaultKey = GridRestrictionSerializer.Instance.GetTagId(minionAssignablesProxy.GetMinionModel());
		this.out_of_fuel = navigator.HasTag(GameTags.JetSuitOutOfFuel);
	}

	// Token: 0x060018B4 RID: 6324 RVA: 0x0008914E File Offset: 0x0008734E
	public void SetIdleNavMaskEnabled(bool enabled)
	{
		this.idleNavMaskEnabled = enabled;
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x00089157 File Offset: 0x00087357
	private static bool IsAccessPermitted(int proxyID, int proxyTag, int cell, int from_cell, NavType from_nav_type)
	{
		return Grid.HasPermission(cell, proxyID, proxyTag, from_cell, from_nav_type);
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x00089164 File Offset: 0x00087364
	public override int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
	{
		if (!path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasLeadSuit))
		{
			return (int)(link.cost * 2);
		}
		return 0;
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x0008917C File Offset: 0x0008737C
	public override bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged)
	{
		if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, this.accessControlDefaultKey, path.cell, from_cell, from_nav_type))
		{
			return false;
		}
		foreach (CellOffset offset in this.transitionVoidOffsets[transition_id])
		{
			int cell = Grid.OffsetCell(from_cell, offset);
			if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, this.accessControlDefaultKey, cell, from_cell, from_nav_type))
			{
				return false;
			}
		}
		if (path.navType == NavType.Tube && from_nav_type == NavType.Floor && !Grid.HasUsableTubeEntrance(from_cell, this.prefabInstanceID))
		{
			return false;
		}
		if (path.navType == NavType.Hover && (this.out_of_fuel || !path.HasFlag(PathFinder.PotentialPath.Flags.HasJetPack)))
		{
			return false;
		}
		Grid.SuitMarker.Flags flags = (Grid.SuitMarker.Flags)0;
		PathFinder.PotentialPath.Flags flags2 = PathFinder.PotentialPath.Flags.None;
		bool flag = path.HasFlag(PathFinder.PotentialPath.Flags.PerformSuitChecks) && Grid.TryGetSuitMarkerFlags(from_cell, out flags, out flags2) && (flags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		bool flag2 = SuitMarker.DoesTraversalDirectionRequireSuit(from_cell, path.cell, flags);
		bool flag3 = path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
		if (flag)
		{
			bool flag4 = path.HasFlag(flags2);
			if (flag2)
			{
				if (!flag3 && !Grid.HasSuit(from_cell, this.prefabInstanceID))
				{
					return false;
				}
			}
			else if (flag3 && (flags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) != (Grid.SuitMarker.Flags)0 && (!flag4 || !Grid.HasEmptyLocker(from_cell, this.prefabInstanceID)))
			{
				return false;
			}
		}
		if (this.idleNavMaskEnabled && (Grid.PreventIdleTraversal[path.cell] || Grid.PreventIdleTraversal[from_cell]))
		{
			return false;
		}
		if (flag)
		{
			if (flag2)
			{
				if (!flag3)
				{
					path.SetFlags(flags2);
				}
			}
			else
			{
				path.ClearFlags(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
			}
		}
		return true;
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x000892EC File Offset: 0x000874EC
	public override PathFinderAbilities Clone()
	{
		return new MinionPathFinderAbilities(this.navigator)
		{
			prefabInstanceID = this.prefabInstanceID,
			proxyID = this.proxyID,
			accessControlDefaultKey = this.accessControlDefaultKey,
			out_of_fuel = this.out_of_fuel,
			idleNavMaskEnabled = this.idleNavMaskEnabled
		};
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x00089340 File Offset: 0x00087540
	public override void RecycleClone()
	{
	}

	// Token: 0x04000E4B RID: 3659
	private CellOffset[][] transitionVoidOffsets;

	// Token: 0x04000E4C RID: 3660
	private int proxyID;

	// Token: 0x04000E4D RID: 3661
	private int accessControlDefaultKey;

	// Token: 0x04000E4E RID: 3662
	private bool out_of_fuel;

	// Token: 0x04000E4F RID: 3663
	private bool idleNavMaskEnabled;
}
