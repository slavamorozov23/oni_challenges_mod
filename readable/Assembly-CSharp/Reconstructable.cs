using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000626 RID: 1574
public class Reconstructable : KMonoBehaviour
{
	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06002574 RID: 9588 RVA: 0x000D7078 File Offset: 0x000D5278
	public bool AllowReconstruct
	{
		get
		{
			return this.deconstructable.allowDeconstruction && (this.building.Def.ShowInBuildMenu || SelectModuleSideScreen.moduleButtonSortOrder.Contains(this.building.Def.PrefabID));
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06002575 RID: 9589 RVA: 0x000D70B7 File Offset: 0x000D52B7
	public Tag PrimarySelectedElementTag
	{
		get
		{
			return this.selectedElementsTags[0];
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06002576 RID: 9590 RVA: 0x000D70C5 File Offset: 0x000D52C5
	public bool ReconstructRequested
	{
		get
		{
			return this.reconstructRequested;
		}
	}

	// Token: 0x06002577 RID: 9591 RVA: 0x000D70CD File Offset: 0x000D52CD
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06002578 RID: 9592 RVA: 0x000D70D8 File Offset: 0x000D52D8
	public void RequestReconstruct(Tag newElement)
	{
		if (!this.deconstructable.allowDeconstruction)
		{
			return;
		}
		this.reconstructRequested = !this.reconstructRequested;
		if (this.reconstructRequested)
		{
			this.deconstructable.QueueDeconstruction(false);
			this.selectedElementsTags = new Tag[]
			{
				newElement
			};
		}
		else
		{
			this.deconstructable.CancelDeconstruction();
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06002579 RID: 9593 RVA: 0x000D714C File Offset: 0x000D534C
	public void CancelReconstructOrder()
	{
		this.reconstructRequested = false;
		this.deconstructable.CancelDeconstruction();
		base.Trigger(954267658, null);
	}

	// Token: 0x0600257A RID: 9594 RVA: 0x000D716C File Offset: 0x000D536C
	public void TryCommenceReconstruct()
	{
		if (!this.deconstructable.allowDeconstruction)
		{
			return;
		}
		if (!this.reconstructRequested)
		{
			return;
		}
		string facadeID = this.building.GetComponent<BuildingFacade>().CurrentFacade;
		Vector3 position = this.building.transform.position;
		Orientation orientation = this.building.Orientation;
		GameScheduler.Instance.ScheduleNextFrame("Reconstruct", delegate(object data)
		{
			this.building.Def.TryPlace(null, position, orientation, this.selectedElementsTags, facadeID, false, 0);
		}, null, null);
	}

	// Token: 0x040015F6 RID: 5622
	[MyCmpReq]
	private Deconstructable deconstructable;

	// Token: 0x040015F7 RID: 5623
	[MyCmpReq]
	private Building building;

	// Token: 0x040015F8 RID: 5624
	[Serialize]
	private Tag[] selectedElementsTags;

	// Token: 0x040015F9 RID: 5625
	[Serialize]
	private bool reconstructRequested;
}
