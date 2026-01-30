using System;
using UnityEngine;

// Token: 0x020005F1 RID: 1521
public class KBoxCollider2D : KCollider2D
{
	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06002333 RID: 9011 RVA: 0x000CC27B File Offset: 0x000CA47B
	// (set) Token: 0x06002334 RID: 9012 RVA: 0x000CC283 File Offset: 0x000CA483
	public Vector2 size
	{
		get
		{
			return this._size;
		}
		set
		{
			this._size = value;
			base.MarkDirty(false);
		}
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x000CC294 File Offset: 0x000CA494
	public override Extents GetExtents()
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = this.size * 0.9999f;
		Vector2 vector3 = new Vector2(vector.x - vector2.x * 0.5f, vector.y - vector2.y * 0.5f);
		Vector2 vector4 = new Vector2(vector.x + vector2.x * 0.5f, vector.y + vector2.y * 0.5f);
		Vector2I vector2I = new Vector2I((int)vector3.x, (int)vector3.y);
		Vector2I vector2I2 = new Vector2I((int)vector4.x, (int)vector4.y);
		int width = vector2I2.x - vector2I.x + 1;
		int height = vector2I2.y - vector2I.y + 1;
		return new Extents(vector2I.x, vector2I.y, width, height);
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x000CC3A0 File Offset: 0x000CA5A0
	public override bool Intersects(Vector2 intersect_pos)
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = new Vector2(vector.x - this.size.x * 0.5f, vector.y - this.size.y * 0.5f);
		Vector2 vector3 = new Vector2(vector.x + this.size.x * 0.5f, vector.y + this.size.y * 0.5f);
		return intersect_pos.x >= vector2.x && intersect_pos.x <= vector3.x && intersect_pos.y >= vector2.y && intersect_pos.y <= vector3.y;
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06002337 RID: 9015 RVA: 0x000CC48C File Offset: 0x000CA68C
	public override Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f), new Vector3(this._size.x, this._size.y, 0f));
		}
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x000CC4F0 File Offset: 0x000CA6F0
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(this.bounds.center, new Vector3(this._size.x, this._size.y, 0f));
	}

	// Token: 0x04001498 RID: 5272
	[SerializeField]
	private Vector2 _size;
}
