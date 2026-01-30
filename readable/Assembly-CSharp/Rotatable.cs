using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200062D RID: 1581
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Rotatable")]
public class Rotatable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x060025AD RID: 9645 RVA: 0x000D85CA File Offset: 0x000D67CA
	public Orientation Orientation
	{
		get
		{
			return this.orientation;
		}
	}

	// Token: 0x060025AE RID: 9646 RVA: 0x000D85D4 File Offset: 0x000D67D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			this.SetSize(component.Def.WidthInCells, component.Def.HeightInCells);
		}
		this.OrientVisualizer(this.orientation);
		this.OrientCollider(this.orientation);
	}

	// Token: 0x060025AF RID: 9647 RVA: 0x000D862C File Offset: 0x000D682C
	public void SetSize(int width, int height)
	{
		this.width = width;
		this.height = height;
		if (width % 2 == 0)
		{
			this.pivot = new Vector3(-0.5f, 0.5f, 0f);
			this.visualizerOffset = new Vector3(0.5f, 0f, 0f);
			return;
		}
		this.pivot = new Vector3(0f, 0.5f, 0f);
		this.visualizerOffset = Vector3.zero;
	}

	// Token: 0x060025B0 RID: 9648 RVA: 0x000D86AC File Offset: 0x000D68AC
	public Orientation Rotate()
	{
		switch (this.permittedRotations)
		{
		case PermittedRotations.R90:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.R90 : Orientation.Neutral);
			break;
		case PermittedRotations.R360:
			this.orientation = (this.orientation + 1) % Orientation.NumRotations;
			break;
		case PermittedRotations.FlipH:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.FlipH : Orientation.Neutral);
			break;
		case PermittedRotations.FlipV:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.FlipV : Orientation.Neutral);
			break;
		}
		this.OrientVisualizer(this.orientation);
		return this.orientation;
	}

	// Token: 0x060025B1 RID: 9649 RVA: 0x000D8738 File Offset: 0x000D6938
	public void SetOrientation(Orientation new_orientation)
	{
		this.orientation = new_orientation;
		this.OrientVisualizer(new_orientation);
		this.OrientCollider(new_orientation);
	}

	// Token: 0x060025B2 RID: 9650 RVA: 0x000D8750 File Offset: 0x000D6950
	public void Match(Rotatable other)
	{
		this.pivot = other.pivot;
		this.visualizerOffset = other.visualizerOffset;
		this.permittedRotations = other.permittedRotations;
		this.orientation = other.orientation;
		this.OrientVisualizer(this.orientation);
		this.OrientCollider(this.orientation);
	}

	// Token: 0x060025B3 RID: 9651 RVA: 0x000D87A8 File Offset: 0x000D69A8
	public float GetVisualizerRotation()
	{
		PermittedRotations permittedRotations = this.permittedRotations;
		if (permittedRotations - PermittedRotations.R90 <= 1)
		{
			return -90f * (float)this.orientation;
		}
		return 0f;
	}

	// Token: 0x060025B4 RID: 9652 RVA: 0x000D87D5 File Offset: 0x000D69D5
	public bool GetVisualizerFlipX()
	{
		return this.orientation == Orientation.FlipH;
	}

	// Token: 0x060025B5 RID: 9653 RVA: 0x000D87E0 File Offset: 0x000D69E0
	public bool GetVisualizerFlipY()
	{
		return this.orientation == Orientation.FlipV;
	}

	// Token: 0x060025B6 RID: 9654 RVA: 0x000D87EC File Offset: 0x000D69EC
	public Vector3 GetVisualizerPivot()
	{
		Vector3 result = this.pivot;
		Orientation orientation = this.orientation;
		if (orientation != Orientation.FlipH)
		{
			if (orientation != Orientation.FlipV)
			{
			}
		}
		else
		{
			result.x = -this.pivot.x;
		}
		return result;
	}

	// Token: 0x060025B7 RID: 9655 RVA: 0x000D8828 File Offset: 0x000D6A28
	private Vector3 GetVisualizerOffset()
	{
		Orientation orientation = this.orientation;
		Vector3 result;
		if (orientation != Orientation.FlipH)
		{
			if (orientation != Orientation.FlipV)
			{
				result = this.visualizerOffset;
			}
			else
			{
				result = new Vector3(this.visualizerOffset.x, 1f, this.visualizerOffset.z);
			}
		}
		else
		{
			result = new Vector3(-this.visualizerOffset.x, this.visualizerOffset.y, this.visualizerOffset.z);
		}
		return result;
	}

	// Token: 0x060025B8 RID: 9656 RVA: 0x000D889C File Offset: 0x000D6A9C
	private void OrientVisualizer(Orientation orientation)
	{
		float visualizerRotation = this.GetVisualizerRotation();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Pivot = this.GetVisualizerPivot();
		component.Rotation = visualizerRotation;
		component.Offset = this.GetVisualizerOffset();
		component.FlipX = this.GetVisualizerFlipX();
		component.FlipY = this.GetVisualizerFlipY();
		base.Trigger(-1643076535, this);
	}

	// Token: 0x060025B9 RID: 9657 RVA: 0x000D88F8 File Offset: 0x000D6AF8
	private void OrientCollider(Orientation orientation)
	{
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component == null)
		{
			return;
		}
		float num = 0.5f * (float)((this.width + 1) % 2);
		float num2 = 0f;
		switch (orientation)
		{
		case Orientation.R90:
			num2 = -90f;
			goto IL_11B;
		case Orientation.R180:
			num2 = -180f;
			goto IL_11B;
		case Orientation.R270:
			num2 = -270f;
			goto IL_11B;
		case Orientation.FlipH:
			component.offset = new Vector2(num + (float)(this.width % 2) - 1f, 0.5f * (float)this.height);
			component.size = new Vector2((float)this.width, (float)this.height);
			goto IL_11B;
		case Orientation.FlipV:
			component.offset = new Vector2(num, -0.5f * (float)(this.height - 2));
			component.size = new Vector2((float)this.width, (float)this.height);
			goto IL_11B;
		}
		component.offset = new Vector2(num, 0.5f * (float)this.height);
		component.size = new Vector2((float)this.width, (float)this.height);
		IL_11B:
		if (num2 != 0f)
		{
			Matrix2x3 n = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n2 = Matrix2x3.Rotate(num2 * 0.017453292f);
			Matrix2x3 matrix2x = Matrix2x3.Translate(this.pivot + new Vector3(num, 0f, 0f)) * n2 * n;
			Vector2 vector = new Vector2(-0.5f * (float)this.width, 0f);
			Vector2 vector2 = new Vector2(0.5f * (float)this.width, (float)this.height);
			Vector2 vector3 = new Vector2(0f, 0.5f * (float)this.height);
			vector = matrix2x.MultiplyPoint(vector);
			vector2 = matrix2x.MultiplyPoint(vector2);
			vector3 = matrix2x.MultiplyPoint(vector3);
			float num3 = Mathf.Min(vector.x, vector2.x);
			float num4 = Mathf.Max(vector.x, vector2.x);
			float num5 = Mathf.Min(vector.y, vector2.y);
			float num6 = Mathf.Max(vector.y, vector2.y);
			component.offset = vector3;
			component.size = new Vector2(num4 - num3, num6 - num5);
		}
	}

	// Token: 0x060025BA RID: 9658 RVA: 0x000D8B80 File Offset: 0x000D6D80
	public CellOffset GetRotatedCellOffset(CellOffset offset)
	{
		return Rotatable.GetRotatedCellOffset(offset, this.orientation);
	}

	// Token: 0x060025BB RID: 9659 RVA: 0x000D8B90 File Offset: 0x000D6D90
	public static CellOffset GetRotatedCellOffset(CellOffset offset, Orientation orientation)
	{
		switch (orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new CellOffset(offset.y, -offset.x);
		case Orientation.R180:
			return new CellOffset(-offset.x, -offset.y);
		case Orientation.R270:
			return new CellOffset(-offset.y, offset.x);
		case Orientation.FlipH:
			return new CellOffset(-offset.x, offset.y);
		case Orientation.FlipV:
			return new CellOffset(offset.x, -offset.y);
		}
	}

	// Token: 0x060025BC RID: 9660 RVA: 0x000D8C20 File Offset: 0x000D6E20
	public static CellOffset GetRotatedCellOffset(int x, int y, Orientation orientation)
	{
		return Rotatable.GetRotatedCellOffset(new CellOffset(x, y), orientation);
	}

	// Token: 0x060025BD RID: 9661 RVA: 0x000D8C2F File Offset: 0x000D6E2F
	public Vector3 GetRotatedOffset(Vector3 offset)
	{
		return Rotatable.GetRotatedOffset(offset, this.orientation);
	}

	// Token: 0x060025BE RID: 9662 RVA: 0x000D8C40 File Offset: 0x000D6E40
	public static Vector3 GetRotatedOffset(Vector3 offset, Orientation orientation)
	{
		switch (orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new Vector3(offset.y, -offset.x);
		case Orientation.R180:
			return new Vector3(-offset.x, -offset.y);
		case Orientation.R270:
			return new Vector3(-offset.y, offset.x);
		case Orientation.FlipH:
			return new Vector3(-offset.x, offset.y);
		case Orientation.FlipV:
			return new Vector3(offset.x, -offset.y);
		}
	}

	// Token: 0x060025BF RID: 9663 RVA: 0x000D8CD0 File Offset: 0x000D6ED0
	public Vector2I GetRotatedOffset(Vector2I offset)
	{
		switch (this.orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new Vector2I(offset.y, -offset.x);
		case Orientation.R180:
			return new Vector2I(-offset.x, -offset.y);
		case Orientation.R270:
			return new Vector2I(-offset.y, offset.x);
		case Orientation.FlipH:
			return new Vector2I(-offset.x, offset.y);
		case Orientation.FlipV:
			return new Vector2I(offset.x, -offset.y);
		}
	}

	// Token: 0x060025C0 RID: 9664 RVA: 0x000D8D67 File Offset: 0x000D6F67
	public Orientation GetOrientation()
	{
		return this.orientation;
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x060025C1 RID: 9665 RVA: 0x000D8D6F File Offset: 0x000D6F6F
	public bool IsRotated
	{
		get
		{
			return this.orientation > Orientation.Neutral;
		}
	}

	// Token: 0x04001620 RID: 5664
	[Serialize]
	[SerializeField]
	private Orientation orientation;

	// Token: 0x04001621 RID: 5665
	[SerializeField]
	private Vector3 pivot = Vector3.zero;

	// Token: 0x04001622 RID: 5666
	[SerializeField]
	private Vector3 visualizerOffset = Vector3.zero;

	// Token: 0x04001623 RID: 5667
	public PermittedRotations permittedRotations;

	// Token: 0x04001624 RID: 5668
	[SerializeField]
	private int width;

	// Token: 0x04001625 RID: 5669
	[SerializeField]
	private int height;
}
