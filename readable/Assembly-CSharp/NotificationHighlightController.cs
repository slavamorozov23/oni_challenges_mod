using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DC5 RID: 3525
public class NotificationHighlightController : KMonoBehaviour
{
	// Token: 0x06006E21 RID: 28193 RVA: 0x0029B57A File Offset: 0x0029977A
	protected override void OnSpawn()
	{
		this.highlightBox = Util.KInstantiateUI<RectTransform>(this.highlightBoxPrefab.gameObject, base.gameObject, false);
		this.HideBox();
	}

	// Token: 0x06006E22 RID: 28194 RVA: 0x0029B5A0 File Offset: 0x002997A0
	[ContextMenu("Force Update")]
	protected void LateUpdate()
	{
		bool flag = false;
		if (this.activeTargetNotification != null)
		{
			foreach (NotificationHighlightTarget notificationHighlightTarget in this.targets)
			{
				if (notificationHighlightTarget.targetKey == this.activeTargetNotification.highlightTarget)
				{
					this.SnapBoxToTarget(notificationHighlightTarget);
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			this.HideBox();
		}
	}

	// Token: 0x06006E23 RID: 28195 RVA: 0x0029B624 File Offset: 0x00299824
	public void AddTarget(NotificationHighlightTarget target)
	{
		this.targets.Add(target);
	}

	// Token: 0x06006E24 RID: 28196 RVA: 0x0029B632 File Offset: 0x00299832
	public void RemoveTarget(NotificationHighlightTarget target)
	{
		this.targets.Remove(target);
	}

	// Token: 0x06006E25 RID: 28197 RVA: 0x0029B641 File Offset: 0x00299841
	public void SetActiveTarget(ManagementMenuNotification notification)
	{
		this.activeTargetNotification = notification;
	}

	// Token: 0x06006E26 RID: 28198 RVA: 0x0029B64A File Offset: 0x0029984A
	public void ClearActiveTarget(ManagementMenuNotification checkNotification)
	{
		if (checkNotification == this.activeTargetNotification)
		{
			this.activeTargetNotification = null;
		}
	}

	// Token: 0x06006E27 RID: 28199 RVA: 0x0029B65C File Offset: 0x0029985C
	public void ClearActiveTarget()
	{
		this.activeTargetNotification = null;
	}

	// Token: 0x06006E28 RID: 28200 RVA: 0x0029B665 File Offset: 0x00299865
	public void TargetViewed(NotificationHighlightTarget target)
	{
		if (this.activeTargetNotification != null && this.activeTargetNotification.highlightTarget == target.targetKey)
		{
			this.activeTargetNotification.View();
		}
	}

	// Token: 0x06006E29 RID: 28201 RVA: 0x0029B694 File Offset: 0x00299894
	private void SnapBoxToTarget(NotificationHighlightTarget target)
	{
		RectTransform rectTransform = target.rectTransform();
		Vector3 position = rectTransform.GetPosition();
		this.highlightBox.sizeDelta = rectTransform.rect.size;
		this.highlightBox.SetPosition(position + new Vector3(rectTransform.rect.position.x, rectTransform.rect.position.y, 0f));
		RectMask2D componentInParent = rectTransform.GetComponentInParent<RectMask2D>();
		if (componentInParent != null)
		{
			RectTransform rectTransform2 = componentInParent.rectTransform();
			Vector3 a = rectTransform2.TransformPoint(rectTransform2.rect.min);
			Vector3 a2 = rectTransform2.TransformPoint(rectTransform2.rect.max);
			Vector3 b = this.highlightBox.TransformPoint(this.highlightBox.rect.min);
			Vector3 b2 = this.highlightBox.TransformPoint(this.highlightBox.rect.max);
			Vector3 vector = a - b;
			Vector3 vector2 = a2 - b2;
			if (vector.x > 0f)
			{
				this.highlightBox.anchoredPosition = this.highlightBox.anchoredPosition + new Vector2(vector.x, 0f);
				this.highlightBox.sizeDelta -= new Vector2(vector.x, 0f);
			}
			else if (vector.y > 0f)
			{
				this.highlightBox.anchoredPosition = this.highlightBox.anchoredPosition + new Vector2(0f, vector.y);
				this.highlightBox.sizeDelta -= new Vector2(0f, vector.y);
			}
			if (vector2.x < 0f)
			{
				this.highlightBox.sizeDelta += new Vector2(vector2.x, 0f);
			}
			if (vector2.y < 0f)
			{
				this.highlightBox.sizeDelta += new Vector2(0f, vector2.y);
			}
		}
		this.highlightBox.gameObject.SetActive(this.highlightBox.sizeDelta.x > 0f && this.highlightBox.sizeDelta.y > 0f);
	}

	// Token: 0x06006E2A RID: 28202 RVA: 0x0029B923 File Offset: 0x00299B23
	private void HideBox()
	{
		this.highlightBox.gameObject.SetActive(false);
	}

	// Token: 0x04004B43 RID: 19267
	public RectTransform highlightBoxPrefab;

	// Token: 0x04004B44 RID: 19268
	private RectTransform highlightBox;

	// Token: 0x04004B45 RID: 19269
	private List<NotificationHighlightTarget> targets = new List<NotificationHighlightTarget>();

	// Token: 0x04004B46 RID: 19270
	private ManagementMenuNotification activeTargetNotification;
}
