using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000684 RID: 1668
public class DevQuickActionCategoryNode : DevQuickActionNode
{
	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06002916 RID: 10518 RVA: 0x000EA66A File Offset: 0x000E886A
	private bool IsExpanded
	{
		get
		{
			return this.toggle.isOn;
		}
	}

	// Token: 0x06002917 RID: 10519 RVA: 0x000EA678 File Offset: 0x000E8878
	protected void Awake()
	{
		this.toggle = base.GetComponent<Toggle>();
		this.originalColorBlock = this.toggle.colors;
		this.pressedColorBlock = this.toggle.colors;
		this.pressedColorBlock.normalColor = this.originalColorBlock.pressedColor;
		this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
		this.RefreshVisuals();
	}

	// Token: 0x06002918 RID: 10520 RVA: 0x000EA6EB File Offset: 0x000E88EB
	public void Setup(string name, DevQuickActionNode parentNode)
	{
		this.label.SetText(name);
		this.parentNode = parentNode;
	}

	// Token: 0x06002919 RID: 10521 RVA: 0x000EA700 File Offset: 0x000E8900
	private void RefreshVisuals()
	{
		(this.toggle.targetGraphic as Image).sprite = (this.IsExpanded ? this.pressedSprite : this.notPressedSprite);
		this.toggle.colors = (this.IsExpanded ? this.pressedColorBlock : this.originalColorBlock);
	}

	// Token: 0x0600291A RID: 10522 RVA: 0x000EA759 File Offset: 0x000E8959
	private void OnToggleValueChanged(bool value)
	{
		this.RefreshVisuals();
		if (this.IsExpanded)
		{
			this.OnExpand();
		}
		else
		{
			this.OnCollapsed();
		}
		System.Action onNodeInteractedWith = this.OnNodeInteractedWith;
		if (onNodeInteractedWith == null)
		{
			return;
		}
		onNodeInteractedWith();
	}

	// Token: 0x0600291B RID: 10523 RVA: 0x000EA787 File Offset: 0x000E8987
	public virtual void Expand()
	{
		this.toggle.isOn = true;
	}

	// Token: 0x0600291C RID: 10524 RVA: 0x000EA795 File Offset: 0x000E8995
	public void Collapse()
	{
		this.toggle.isOn = false;
	}

	// Token: 0x0600291D RID: 10525 RVA: 0x000EA7A4 File Offset: 0x000E89A4
	private void OnExpand()
	{
		Vector2 v = Vector2.up;
		if (this.parentNode != null)
		{
			v = base.transform.anchoredPosition - this.parentNode.transform.anchoredPosition;
		}
		int count = this.childrenNodes.Count;
		float num = 180f / (float)(count + 1);
		for (int i = 0; i < count; i++)
		{
			DevQuickActionNode devQuickActionNode = this.childrenNodes[i];
			Vector2 vector = this.RotateVector2Clockwise(v, num * (float)i);
			Vector2 anchoredPosition = base.transform.anchoredPosition + vector.normalized * this.space;
			devQuickActionNode.transform.anchoredPosition = anchoredPosition;
			devQuickActionNode.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600291E RID: 10526 RVA: 0x000EA860 File Offset: 0x000E8A60
	private void OnCollapsed()
	{
		foreach (DevQuickActionNode devQuickActionNode in this.childrenNodes)
		{
			if (devQuickActionNode is DevQuickActionCategoryNode)
			{
				(devQuickActionNode as DevQuickActionCategoryNode).Collapse();
			}
			devQuickActionNode.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x000EA8CC File Offset: 0x000E8ACC
	public void AddChildren(DevQuickActionNode node)
	{
		if (!this.childrenNodes.Contains(node))
		{
			this.childrenNodes.Add(node);
		}
	}

	// Token: 0x06002920 RID: 10528 RVA: 0x000EA8E8 File Offset: 0x000E8AE8
	private Vector2 RotateVector2Clockwise(Vector2 v, float angleDegrees)
	{
		float f = angleDegrees * 0.017453292f;
		float num = Mathf.Cos(f);
		float num2 = Mathf.Sin(f);
		return new Vector2(v.x * num + v.y * num2, -v.x * num2 + v.y * num);
	}

	// Token: 0x06002921 RID: 10529 RVA: 0x000EA934 File Offset: 0x000E8B34
	public override void Recycle()
	{
		foreach (DevQuickActionNode devQuickActionNode in this.childrenNodes)
		{
			devQuickActionNode.Recycle();
		}
		this.toggle.SetIsOnWithoutNotify(false);
		this.RefreshVisuals();
		base.Recycle();
		this.childrenNodes.Clear();
	}

	// Token: 0x04001842 RID: 6210
	public Sprite pressedSprite;

	// Token: 0x04001843 RID: 6211
	public Sprite notPressedSprite;

	// Token: 0x04001844 RID: 6212
	private Toggle toggle;

	// Token: 0x04001845 RID: 6213
	protected List<DevQuickActionNode> childrenNodes = new List<DevQuickActionNode>();

	// Token: 0x04001846 RID: 6214
	private ColorBlock originalColorBlock;

	// Token: 0x04001847 RID: 6215
	private ColorBlock pressedColorBlock;
}
