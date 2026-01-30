using System;
using UnityEngine;

// Token: 0x0200094E RID: 2382
public struct FallerComponent
{
	// Token: 0x06004255 RID: 16981 RVA: 0x0017607C File Offset: 0x0017427C
	public FallerComponent(Transform transform, Vector2 initial_velocity)
	{
		this.transform = transform;
		this.transformInstanceId = transform.GetInstanceID();
		this.isFalling = false;
		this.initialVelocity = initial_velocity;
		this.partitionerEntry = default(HandleVector<int>.Handle);
		this.solidChangedCB = null;
		this.cellChangedCB = null;
		this.cellChangedHandlerID = 0UL;
		KCircleCollider2D component = transform.GetComponent<KCircleCollider2D>();
		if (component != null)
		{
			this.offset = component.radius;
			return;
		}
		KCollider2D component2 = transform.GetComponent<KCollider2D>();
		if (component2 != null)
		{
			this.offset = transform.GetPosition().y - component2.bounds.min.y;
			return;
		}
		this.offset = 0f;
	}

	// Token: 0x040029AA RID: 10666
	public Transform transform;

	// Token: 0x040029AB RID: 10667
	public int transformInstanceId;

	// Token: 0x040029AC RID: 10668
	public bool isFalling;

	// Token: 0x040029AD RID: 10669
	public float offset;

	// Token: 0x040029AE RID: 10670
	public Vector2 initialVelocity;

	// Token: 0x040029AF RID: 10671
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040029B0 RID: 10672
	public Action<object> solidChangedCB;

	// Token: 0x040029B1 RID: 10673
	public Action<object> cellChangedCB;

	// Token: 0x040029B2 RID: 10674
	public ulong cellChangedHandlerID;
}
