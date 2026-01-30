using System;
using UnityEngine;

// Token: 0x02000977 RID: 2423
public struct GravityComponent
{
	// Token: 0x06004522 RID: 17698 RVA: 0x00190958 File Offset: 0x0018EB58
	public GravityComponent(Transform transform, Action<Transform> on_landed, Vector2 initial_velocity, bool land_on_fake_floors, bool mayLeaveWorld)
	{
		this.transform = transform;
		this.elapsedTime = 0f;
		this.velocity = initial_velocity;
		this.onLanded = on_landed;
		this.landOnFakeFloors = land_on_fake_floors;
		this.mayLeaveWorld = mayLeaveWorld;
		this.collider2D = transform.GetComponent<KCollider2D>();
		this.extents = GravityComponent.GetExtents(this.collider2D);
	}

	// Token: 0x06004523 RID: 17699 RVA: 0x001909B4 File Offset: 0x0018EBB4
	public static float GetGroundOffset(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.bounds.extents.y - collider.offset.y;
		}
		return 0f;
	}

	// Token: 0x06004524 RID: 17700 RVA: 0x001909EF File Offset: 0x0018EBEF
	public static float GetGroundOffset(GravityComponent gravityComponent)
	{
		if (gravityComponent.collider2D != null)
		{
			return gravityComponent.extents.y - gravityComponent.collider2D.offset.y;
		}
		return 0f;
	}

	// Token: 0x06004525 RID: 17701 RVA: 0x00190A24 File Offset: 0x0018EC24
	public static Vector2 GetExtents(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.bounds.extents;
		}
		return Vector2.zero;
	}

	// Token: 0x06004526 RID: 17702 RVA: 0x00190A53 File Offset: 0x0018EC53
	public static Vector2 GetOffset(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.offset;
		}
		return Vector2.zero;
	}

	// Token: 0x04002E5A RID: 11866
	public Transform transform;

	// Token: 0x04002E5B RID: 11867
	public Vector2 velocity;

	// Token: 0x04002E5C RID: 11868
	public float elapsedTime;

	// Token: 0x04002E5D RID: 11869
	public Action<Transform> onLanded;

	// Token: 0x04002E5E RID: 11870
	public bool landOnFakeFloors;

	// Token: 0x04002E5F RID: 11871
	public bool mayLeaveWorld;

	// Token: 0x04002E60 RID: 11872
	public Vector2 extents;

	// Token: 0x04002E61 RID: 11873
	public KCollider2D collider2D;
}
