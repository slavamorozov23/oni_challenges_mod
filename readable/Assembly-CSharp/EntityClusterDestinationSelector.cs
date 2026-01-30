using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B7A RID: 2938
public class EntityClusterDestinationSelector : ClusterDestinationSelector
{
	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x06005794 RID: 22420 RVA: 0x001FE281 File Offset: 0x001FC481
	private ClusterGridEntity DestinationEntity
	{
		get
		{
			if (this.m_DestinationEntity == null)
			{
				return null;
			}
			return this.m_DestinationEntity.Get();
		}
	}

	// Token: 0x06005795 RID: 22421 RVA: 0x001FE298 File Offset: 0x001FC498
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(this.requiredEntityLayer != EntityLayer.None, "EnityClusterDestinationSelector must specify an EntityLayer");
	}

	// Token: 0x06005796 RID: 22422 RVA: 0x001FE2B6 File Offset: 0x001FC4B6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
	}

	// Token: 0x06005797 RID: 22423 RVA: 0x001FE2D8 File Offset: 0x001FC4D8
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			EntityClusterDestinationSelector component = gameObject.GetComponent<EntityClusterDestinationSelector>();
			if (component != null && component.DestinationEntity != null)
			{
				this.m_DestinationEntity = new Ref<ClusterGridEntity>(component.DestinationEntity);
				this.SetDestination(this.m_DestinationEntity.Get().Location);
			}
		}
	}

	// Token: 0x06005798 RID: 22424 RVA: 0x001FE33A File Offset: 0x001FC53A
	public override ClusterGridEntity GetClusterEntityTarget()
	{
		return this.DestinationEntity;
	}

	// Token: 0x06005799 RID: 22425 RVA: 0x001FE342 File Offset: 0x001FC542
	public override AxialI GetDestination()
	{
		if (this.DestinationEntity != null)
		{
			return this.DestinationEntity.Location;
		}
		return base.GetDestination();
	}

	// Token: 0x0600579A RID: 22426 RVA: 0x001FE364 File Offset: 0x001FC564
	public override void SetDestination(AxialI location)
	{
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, this.requiredEntityLayer);
		this.m_DestinationEntity.Set(visibleEntityOfLayerAtCell);
		base.SetDestination(location);
	}

	// Token: 0x04003AC6 RID: 15046
	[Serialize]
	protected Ref<ClusterGridEntity> m_DestinationEntity = new Ref<ClusterGridEntity>();
}
