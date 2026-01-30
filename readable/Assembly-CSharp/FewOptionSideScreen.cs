using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E3A RID: 3642
public class FewOptionSideScreen : SideScreenContent
{
	// Token: 0x06007387 RID: 29575 RVA: 0x002C1CFB File Offset: 0x002BFEFB
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshOptions();
		}
	}

	// Token: 0x06007388 RID: 29576 RVA: 0x002C1D10 File Offset: 0x002BFF10
	private void RefreshOptions()
	{
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == this.targetFewOptions.GetSelectedOption()) ? 1 : 0);
		}
	}

	// Token: 0x06007389 RID: 29577 RVA: 0x002C1D8C File Offset: 0x002BFF8C
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x0600738A RID: 29578 RVA: 0x002C1DD0 File Offset: 0x002BFFD0
	private void SpawnRows()
	{
		FewOptionSideScreen.IFewOptionSideScreen.Option[] options = this.targetFewOptions.GetOptions();
		for (int i = 0; i < options.Length; i++)
		{
			FewOptionSideScreen.IFewOptionSideScreen.Option option = options[i];
			GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("label").SetText(option.labelText);
			component.GetReference<Image>("icon").sprite = option.iconSpriteColorTuple.first;
			component.GetReference<Image>("icon").color = option.iconSpriteColorTuple.second;
			gameObject.GetComponent<ToolTip>().toolTip = option.tooltipText;
			gameObject.GetComponent<MultiToggle>().onClick = delegate()
			{
				this.targetFewOptions.OnOptionSelected(option);
				this.RefreshOptions();
			};
			this.rows.Add(option.tag, gameObject);
		}
		this.RefreshOptions();
	}

	// Token: 0x0600738B RID: 29579 RVA: 0x002C1ED9 File Offset: 0x002C00D9
	public override void SetTarget(GameObject target)
	{
		this.ClearRows();
		this.targetFewOptions = target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>();
		this.SpawnRows();
	}

	// Token: 0x0600738C RID: 29580 RVA: 0x002C1EF3 File Offset: 0x002C00F3
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>() != null;
	}

	// Token: 0x04004FE0 RID: 20448
	public GameObject rowPrefab;

	// Token: 0x04004FE1 RID: 20449
	public RectTransform rowContainer;

	// Token: 0x04004FE2 RID: 20450
	public Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();

	// Token: 0x04004FE3 RID: 20451
	private FewOptionSideScreen.IFewOptionSideScreen targetFewOptions;

	// Token: 0x020020B7 RID: 8375
	public interface IFewOptionSideScreen
	{
		// Token: 0x0600BA23 RID: 47651
		FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions();

		// Token: 0x0600BA24 RID: 47652
		void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option);

		// Token: 0x0600BA25 RID: 47653
		Tag GetSelectedOption();

		// Token: 0x02002A83 RID: 10883
		public struct Option
		{
			// Token: 0x0600D4F7 RID: 54519 RVA: 0x0043DB7E File Offset: 0x0043BD7E
			public Option(Tag tag, string labelText, global::Tuple<Sprite, Color> iconSpriteColorTuple, string tooltipText = "")
			{
				this.tag = tag;
				this.labelText = labelText;
				this.iconSpriteColorTuple = iconSpriteColorTuple;
				this.tooltipText = tooltipText;
			}

			// Token: 0x0400BB8E RID: 48014
			public Tag tag;

			// Token: 0x0400BB8F RID: 48015
			public string labelText;

			// Token: 0x0400BB90 RID: 48016
			public string tooltipText;

			// Token: 0x0400BB91 RID: 48017
			public global::Tuple<Sprite, Color> iconSpriteColorTuple;
		}
	}
}
