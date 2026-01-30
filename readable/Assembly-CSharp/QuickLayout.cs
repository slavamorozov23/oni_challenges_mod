using System;
using UnityEngine;

// Token: 0x02000DE7 RID: 3559
public class QuickLayout : KMonoBehaviour
{
	// Token: 0x06006FFF RID: 28671 RVA: 0x002A8ACC File Offset: 0x002A6CCC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ForceUpdate();
	}

	// Token: 0x06007000 RID: 28672 RVA: 0x002A8ADA File Offset: 0x002A6CDA
	private void OnEnable()
	{
		this.ForceUpdate();
	}

	// Token: 0x06007001 RID: 28673 RVA: 0x002A8AE2 File Offset: 0x002A6CE2
	private void LateUpdate()
	{
		this.Run(false);
	}

	// Token: 0x06007002 RID: 28674 RVA: 0x002A8AEB File Offset: 0x002A6CEB
	public void ForceUpdate()
	{
		this.Run(true);
	}

	// Token: 0x06007003 RID: 28675 RVA: 0x002A8AF4 File Offset: 0x002A6CF4
	private void Run(bool forceUpdate = false)
	{
		forceUpdate = (forceUpdate || this._elementSize != this.elementSize);
		forceUpdate = (forceUpdate || this._spacing != this.spacing);
		forceUpdate = (forceUpdate || this._layoutDirection != this.layoutDirection);
		forceUpdate = (forceUpdate || this._offset != this.offset);
		if (forceUpdate)
		{
			this._elementSize = this.elementSize;
			this._spacing = this.spacing;
			this._layoutDirection = this.layoutDirection;
			this._offset = this.offset;
		}
		int num = 0;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				num++;
			}
		}
		if (num != this.oldActiveChildCount || forceUpdate)
		{
			this.Layout();
			this.oldActiveChildCount = num;
		}
	}

	// Token: 0x06007004 RID: 28676 RVA: 0x002A8BEC File Offset: 0x002A6DEC
	public void Layout()
	{
		Vector3 vector = this._offset;
		bool flag = false;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				flag = true;
				base.transform.GetChild(i).rectTransform().anchoredPosition = vector;
				vector += (float)(this._elementSize + this._spacing) * this.GetDirectionVector();
			}
		}
		if (this.driveParentRectSize != null)
		{
			if (!flag)
			{
				if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(this.driveParentRectSize.sizeDelta.x), 0f);
					return;
				}
				if (this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight || this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(0f, Mathf.Abs(this.driveParentRectSize.sizeDelta.y));
					return;
				}
			}
			else
			{
				if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(this.driveParentRectSize.sizeDelta.x, Mathf.Abs(vector.y));
					return;
				}
				if (this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight || this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(vector.x), this.driveParentRectSize.sizeDelta.y);
				}
			}
		}
	}

	// Token: 0x06007005 RID: 28677 RVA: 0x002A8D84 File Offset: 0x002A6F84
	private Vector2 GetDirectionVector()
	{
		Vector2 result = Vector3.zero;
		switch (this._layoutDirection)
		{
		case QuickLayout.LayoutDirection.TopToBottom:
			result = Vector2.down;
			break;
		case QuickLayout.LayoutDirection.BottomToTop:
			result = Vector2.up;
			break;
		case QuickLayout.LayoutDirection.LeftToRight:
			result = Vector2.right;
			break;
		case QuickLayout.LayoutDirection.RightToLeft:
			result = Vector2.left;
			break;
		}
		return result;
	}

	// Token: 0x04004CCD RID: 19661
	[Header("Configuration")]
	[SerializeField]
	private int elementSize;

	// Token: 0x04004CCE RID: 19662
	[SerializeField]
	private int spacing;

	// Token: 0x04004CCF RID: 19663
	[SerializeField]
	private QuickLayout.LayoutDirection layoutDirection;

	// Token: 0x04004CD0 RID: 19664
	[SerializeField]
	private Vector2 offset;

	// Token: 0x04004CD1 RID: 19665
	[SerializeField]
	private RectTransform driveParentRectSize;

	// Token: 0x04004CD2 RID: 19666
	private int _elementSize;

	// Token: 0x04004CD3 RID: 19667
	private int _spacing;

	// Token: 0x04004CD4 RID: 19668
	private QuickLayout.LayoutDirection _layoutDirection;

	// Token: 0x04004CD5 RID: 19669
	private Vector2 _offset;

	// Token: 0x04004CD6 RID: 19670
	private int oldActiveChildCount;

	// Token: 0x02002051 RID: 8273
	private enum LayoutDirection
	{
		// Token: 0x040095AF RID: 38319
		TopToBottom,
		// Token: 0x040095B0 RID: 38320
		BottomToTop,
		// Token: 0x040095B1 RID: 38321
		LeftToRight,
		// Token: 0x040095B2 RID: 38322
		RightToLeft
	}
}
