using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EC8 RID: 3784
[AddComponentMenu("KMonoBehaviour/scripts/BreakdownListRow")]
public class BreakdownListRow : KMonoBehaviour
{
	// Token: 0x0600792A RID: 31018 RVA: 0x002E90A0 File Offset: 0x002E72A0
	public void ShowData(string name, string value)
	{
		base.gameObject.transform.localScale = Vector3.one;
		this.nameLabel.text = name;
		this.valueLabel.text = value;
		this.dotOutlineImage.gameObject.SetActive(true);
		Vector2 vector = Vector2.one * 0.6f;
		this.dotOutlineImage.rectTransform.localScale.Set(vector.x, vector.y, 1f);
		this.dotInsideImage.gameObject.SetActive(true);
		this.dotInsideImage.color = BreakdownListRow.statusColour[0];
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
		this.SetHighlighted(false);
		this.SetImportant(false);
	}

	// Token: 0x0600792B RID: 31019 RVA: 0x002E917C File Offset: 0x002E737C
	public void ShowStatusData(string name, string value, BreakdownListRow.Status dotColor)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(true);
		this.dotInsideImage.gameObject.SetActive(true);
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
		this.SetStatusColor(dotColor);
	}

	// Token: 0x0600792C RID: 31020 RVA: 0x002E91DC File Offset: 0x002E73DC
	public void SetStatusColor(BreakdownListRow.Status dotColor)
	{
		this.checkmarkImage.gameObject.SetActive(dotColor > BreakdownListRow.Status.Default);
		this.checkmarkImage.color = BreakdownListRow.statusColour[(int)dotColor];
		switch (dotColor)
		{
		case BreakdownListRow.Status.Red:
			this.checkmarkImage.sprite = this.statusFailureIcon;
			return;
		case BreakdownListRow.Status.Green:
			this.checkmarkImage.sprite = this.statusSuccessIcon;
			return;
		case BreakdownListRow.Status.Yellow:
			this.checkmarkImage.sprite = this.statusWarningIcon;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600792D RID: 31021 RVA: 0x002E9260 File Offset: 0x002E7460
	public void ShowCheckmarkData(string name, string value, BreakdownListRow.Status status)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(true);
		this.dotOutlineImage.rectTransform.localScale = Vector3.one;
		this.dotInsideImage.gameObject.SetActive(true);
		this.iconImage.gameObject.SetActive(false);
		this.SetStatusColor(status);
	}

	// Token: 0x0600792E RID: 31022 RVA: 0x002E92C4 File Offset: 0x002E74C4
	public void ShowIconData(string name, string value, Sprite sprite)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(false);
		this.dotInsideImage.gameObject.SetActive(false);
		this.iconImage.gameObject.SetActive(true);
		this.checkmarkImage.gameObject.SetActive(false);
		this.iconImage.sprite = sprite;
		this.iconImage.color = Color.white;
	}

	// Token: 0x0600792F RID: 31023 RVA: 0x002E9339 File Offset: 0x002E7539
	public void ShowIconData(string name, string value, Sprite sprite, Color spriteColor)
	{
		this.ShowIconData(name, value, sprite);
		this.iconImage.color = spriteColor;
	}

	// Token: 0x06007930 RID: 31024 RVA: 0x002E9354 File Offset: 0x002E7554
	public void SetHighlighted(bool highlighted)
	{
		this.isHighlighted = highlighted;
		Vector2 vector = Vector2.one * 0.8f;
		this.dotOutlineImage.rectTransform.localScale.Set(vector.x, vector.y, 1f);
		this.nameLabel.alpha = (this.isHighlighted ? 0.9f : 0.5f);
		this.valueLabel.alpha = (this.isHighlighted ? 0.9f : 0.5f);
	}

	// Token: 0x06007931 RID: 31025 RVA: 0x002E93E0 File Offset: 0x002E75E0
	public void SetDisabled(bool disabled)
	{
		this.isDisabled = disabled;
		this.nameLabel.alpha = (this.isDisabled ? 0.4f : 0.5f);
		this.valueLabel.alpha = (this.isDisabled ? 0.4f : 0.5f);
	}

	// Token: 0x06007932 RID: 31026 RVA: 0x002E9434 File Offset: 0x002E7634
	public void SetImportant(bool important)
	{
		this.isImportant = important;
		this.dotOutlineImage.rectTransform.localScale = Vector3.one;
		this.nameLabel.alpha = (this.isImportant ? 1f : 0.5f);
		this.valueLabel.alpha = (this.isImportant ? 1f : 0.5f);
		this.nameLabel.fontStyle = (this.isImportant ? FontStyles.Bold : FontStyles.Normal);
		this.valueLabel.fontStyle = (this.isImportant ? FontStyles.Bold : FontStyles.Normal);
	}

	// Token: 0x06007933 RID: 31027 RVA: 0x002E94CC File Offset: 0x002E76CC
	public void HideIcon()
	{
		this.dotOutlineImage.gameObject.SetActive(false);
		this.dotInsideImage.gameObject.SetActive(false);
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
	}

	// Token: 0x06007934 RID: 31028 RVA: 0x002E951D File Offset: 0x002E771D
	public void AddTooltip(string tooltipText)
	{
		if (this.tooltip == null)
		{
			this.tooltip = base.gameObject.AddComponent<ToolTip>();
		}
		this.tooltip.SetSimpleTooltip(tooltipText);
	}

	// Token: 0x06007935 RID: 31029 RVA: 0x002E954A File Offset: 0x002E774A
	public void ClearTooltip()
	{
		if (this.tooltip != null)
		{
			this.tooltip.ClearMultiStringTooltip();
		}
	}

	// Token: 0x06007936 RID: 31030 RVA: 0x002E9565 File Offset: 0x002E7765
	public void SetValue(string value)
	{
		this.valueLabel.text = value;
	}

	// Token: 0x04005488 RID: 21640
	private static Color[] statusColour = new Color[]
	{
		new Color(0.34117648f, 0.36862746f, 0.45882353f, 1f),
		new Color(0.72156864f, 0.38431373f, 0f, 1f),
		new Color(0.38431373f, 0.72156864f, 0f, 1f),
		new Color(0.72156864f, 0.72156864f, 0f, 1f)
	};

	// Token: 0x04005489 RID: 21641
	public Image dotOutlineImage;

	// Token: 0x0400548A RID: 21642
	public Image dotInsideImage;

	// Token: 0x0400548B RID: 21643
	public Image iconImage;

	// Token: 0x0400548C RID: 21644
	public Image checkmarkImage;

	// Token: 0x0400548D RID: 21645
	public LocText nameLabel;

	// Token: 0x0400548E RID: 21646
	public LocText valueLabel;

	// Token: 0x0400548F RID: 21647
	private bool isHighlighted;

	// Token: 0x04005490 RID: 21648
	private bool isDisabled;

	// Token: 0x04005491 RID: 21649
	private bool isImportant;

	// Token: 0x04005492 RID: 21650
	private ToolTip tooltip;

	// Token: 0x04005493 RID: 21651
	[SerializeField]
	private Sprite statusSuccessIcon;

	// Token: 0x04005494 RID: 21652
	[SerializeField]
	private Sprite statusWarningIcon;

	// Token: 0x04005495 RID: 21653
	[SerializeField]
	private Sprite statusFailureIcon;

	// Token: 0x02002127 RID: 8487
	public enum Status
	{
		// Token: 0x04009863 RID: 39011
		Default,
		// Token: 0x04009864 RID: 39012
		Red,
		// Token: 0x04009865 RID: 39013
		Green,
		// Token: 0x04009866 RID: 39014
		Yellow
	}
}
