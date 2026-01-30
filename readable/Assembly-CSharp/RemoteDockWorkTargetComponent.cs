using System;

// Token: 0x02000AD0 RID: 2768
public abstract class RemoteDockWorkTargetComponent : KMonoBehaviour, IRemoteDockWorkTarget
{
	// Token: 0x06005085 RID: 20613 RVA: 0x001D3785 File Offset: 0x001D1985
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RemoteDockWorkTargets.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06005086 RID: 20614 RVA: 0x001D37A3 File Offset: 0x001D19A3
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteDockWorkTargets.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06005087 RID: 20615
	public abstract Chore RemoteDockChore { get; }

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06005088 RID: 20616 RVA: 0x001D37C1 File Offset: 0x001D19C1
	public virtual IApproachable Approachable
	{
		get
		{
			return base.gameObject.GetComponent<IApproachable>();
		}
	}
}
