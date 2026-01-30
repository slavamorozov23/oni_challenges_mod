using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E34 RID: 3636
public class DispenserSideScreen : SideScreenContent
{
	// Token: 0x0600736A RID: 29546 RVA: 0x002C1406 File Offset: 0x002BF606
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IDispenser>() != null;
	}

	// Token: 0x0600736B RID: 29547 RVA: 0x002C1411 File Offset: 0x002BF611
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetDispenser = target.GetComponent<IDispenser>();
		this.Refresh();
	}

	// Token: 0x0600736C RID: 29548 RVA: 0x002C142C File Offset: 0x002BF62C
	private void Refresh()
	{
		this.dispenseButton.ClearOnClick();
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value);
		}
		this.rows.Clear();
		foreach (Tag tag in this.targetDispenser.DispensedItems())
		{
			GameObject gameObject = Util.KInstantiateUI(this.itemRowPrefab, this.itemRowContainer.gameObject, true);
			this.rows.Add(tag, gameObject);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<Image>("Icon").sprite = Def.GetUISprite(tag, "ui", false).first;
			component.GetReference<LocText>("Label").text = Assets.GetPrefab(tag).GetProperName();
			gameObject.GetComponent<MultiToggle>().ChangeState((tag == this.targetDispenser.SelectedItem()) ? 0 : 1);
		}
		if (this.targetDispenser.HasOpenChore())
		{
			this.dispenseButton.onClick += delegate()
			{
				this.targetDispenser.OnCancelDispense();
				this.Refresh();
			};
			this.dispenseButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_CANCEL;
		}
		else
		{
			this.dispenseButton.onClick += delegate()
			{
				this.targetDispenser.OnOrderDispense();
				this.Refresh();
			};
			this.dispenseButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_DISPENSE;
		}
		this.targetDispenser.OnStopWorkEvent -= this.Refresh;
		this.targetDispenser.OnStopWorkEvent += this.Refresh;
	}

	// Token: 0x0600736D RID: 29549 RVA: 0x002C1610 File Offset: 0x002BF810
	private void SelectTag(Tag tag)
	{
		this.targetDispenser.SelectItem(tag);
		this.Refresh();
	}

	// Token: 0x04004FD2 RID: 20434
	[SerializeField]
	private KButton dispenseButton;

	// Token: 0x04004FD3 RID: 20435
	[SerializeField]
	private RectTransform itemRowContainer;

	// Token: 0x04004FD4 RID: 20436
	[SerializeField]
	private GameObject itemRowPrefab;

	// Token: 0x04004FD5 RID: 20437
	private IDispenser targetDispenser;

	// Token: 0x04004FD6 RID: 20438
	private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
}
