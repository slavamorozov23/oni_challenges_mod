using System;

// Token: 0x020008AC RID: 2220
public class InvalidPortReporter : KMonoBehaviour
{
	// Token: 0x06003D3C RID: 15676 RVA: 0x00155C45 File Offset: 0x00153E45
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnTagsChanged(null);
		base.Subscribe<InvalidPortReporter>(-1582839653, InvalidPortReporter.OnTagsChangedDelegate);
	}

	// Token: 0x06003D3D RID: 15677 RVA: 0x00155C65 File Offset: 0x00153E65
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003D3E RID: 15678 RVA: 0x00155C70 File Offset: 0x00153E70
	private void OnTagsChanged(object _)
	{
		bool flag = base.gameObject.HasTag(GameTags.HasInvalidPorts);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(InvalidPortReporter.portsNotOverlapping, !flag);
		}
		KSelectable component2 = base.GetComponent<KSelectable>();
		if (component2 != null)
		{
			component2.ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidPortOverlap, flag, base.gameObject);
		}
	}

	// Token: 0x040025C8 RID: 9672
	public static readonly Operational.Flag portsNotOverlapping = new Operational.Flag("ports_not_overlapping", Operational.Flag.Type.Functional);

	// Token: 0x040025C9 RID: 9673
	private static readonly EventSystem.IntraObjectHandler<InvalidPortReporter> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<InvalidPortReporter>(delegate(InvalidPortReporter component, object data)
	{
		component.OnTagsChanged(data);
	});
}
