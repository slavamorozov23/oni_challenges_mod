using System;

// Token: 0x02000ACF RID: 2767
public abstract class RemoteWorkable : Workable, IRemoteDockWorkTarget
{
	// Token: 0x06005080 RID: 20608 RVA: 0x001D373E File Offset: 0x001D193E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RemoteDockWorkTargets.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06005081 RID: 20609 RVA: 0x001D375C File Offset: 0x001D195C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteDockWorkTargets.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x06005082 RID: 20610
	public abstract Chore RemoteDockChore { get; }

	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x06005083 RID: 20611 RVA: 0x001D377A File Offset: 0x001D197A
	public virtual IApproachable Approachable
	{
		get
		{
			return this;
		}
	}
}
