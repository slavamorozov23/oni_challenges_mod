using System;
using UnityEngine;

// Token: 0x020005F2 RID: 1522
public class KCircleCollider2D : KCollider2D
{
	// Token: 0x1700016A RID: 362
	// (get) Token: 0x0600233A RID: 9018 RVA: 0x000CC542 File Offset: 0x000CA742
	// (set) Token: 0x0600233B RID: 9019 RVA: 0x000CC54A File Offset: 0x000CA74A
	public float radius
	{
		get
		{
			return this._radius;
		}
		set
		{
			this._radius = value;
			base.MarkDirty(false);
		}
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x000CC55C File Offset: 0x000CA75C
	public override Extents GetExtents()
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = new Vector2(vector.x - this.radius, vector.y - this.radius);
		Vector2 vector3 = new Vector2(vector.x + this.radius, vector.y + this.radius);
		int width = (int)vector3.x - (int)vector2.x + 1;
		int height = (int)vector3.y - (int)vector2.y + 1;
		return new Extents((int)(vector.x - this._radius), (int)(vector.y - this._radius), width, height);
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x0600233D RID: 9021 RVA: 0x000CC620 File Offset: 0x000CA820
	public override Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f), new Vector3(this._radius * 2f, this._radius * 2f, 0f));
		}
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x000CC684 File Offset: 0x000CA884
	public override bool Intersects(Vector2 pos)
	{
		Vector3 position = base.transform.GetPosition();
		Vector2 b = new Vector2(position.x, position.y) + base.offset;
		return (pos - b).sqrMagnitude <= this._radius * this._radius;
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x000CC6DC File Offset: 0x000CA8DC
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.bounds.center, this.radius);
	}

	// Token: 0x04001499 RID: 5273
	[SerializeField]
	private float _radius;
}
