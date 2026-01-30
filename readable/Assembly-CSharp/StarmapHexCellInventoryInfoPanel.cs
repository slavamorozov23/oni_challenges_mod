using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000EA2 RID: 3746
public class StarmapHexCellInventoryInfoPanel : SimpleInfoPanel
{
	// Token: 0x060077D1 RID: 30673 RVA: 0x002DEFF3 File Offset: 0x002DD1F3
	public StarmapHexCellInventoryInfoPanel(SimpleInfoScreen simpleInfoScreen) : base(simpleInfoScreen)
	{
	}

	// Token: 0x060077D2 RID: 30674 RVA: 0x002DF008 File Offset: 0x002DD208
	public override void Refresh(CollapsibleDetailContentPanel panel, GameObject selectedTarget)
	{
		StarmapHexCellInventory hexCellInventory;
		if (!this.IsValidTarget(selectedTarget, out hexCellInventory))
		{
			panel.gameObject.SetActive(false);
			return;
		}
		panel.SetTitle(UI.CLUSTERMAP.HEXCELL_INVENTORY.UI_PANEL.TITLE);
		this.RefreshElements(panel, hexCellInventory);
		panel.gameObject.SetActive(true);
	}

	// Token: 0x060077D3 RID: 30675 RVA: 0x002DF054 File Offset: 0x002DD254
	private void RefreshElements(CollapsibleDetailContentPanel panel, StarmapHexCellInventory hexCellInventory)
	{
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.itemRows)
		{
			if (keyValuePair.Value != null)
			{
				keyValuePair.Value.SetActive(false);
			}
		}
		if (hexCellInventory == null)
		{
			return;
		}
		List<StarmapHexCellInventory.SerializedItem> list = new List<StarmapHexCellInventory.SerializedItem>(hexCellInventory.Items);
		list.Sort((StarmapHexCellInventory.SerializedItem a, StarmapHexCellInventory.SerializedItem b) => b.Mass.CompareTo(a.Mass));
		foreach (StarmapHexCellInventory.SerializedItem serializedItem in list)
		{
			Tag id = serializedItem.ID;
			GameObject gameObject;
			if (!this.itemRows.TryGetValue(id, out gameObject))
			{
				gameObject = Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, panel.Content.gameObject, true);
				this.itemRows.Add(id, gameObject);
			}
			gameObject.SetActive(true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(id, "ui", false);
			component.GetReference<Image>("Icon").sprite = uisprite.first;
			component.GetReference<Image>("Icon").color = uisprite.second;
			component.GetReference<LocText>("NameLabel").text = (serializedItem.IsEntity ? serializedItem.ID.ProperName() : ElementLoader.GetElement(id).name);
			component.GetReference<LocText>("ValueLabel").text = (serializedItem.IsEntity ? GameUtil.GetFormattedUnits(serializedItem.Mass, GameUtil.TimeSlice.None, true, "") : GameUtil.GetFormattedMass(serializedItem.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			component.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
		}
	}

	// Token: 0x060077D4 RID: 30676 RVA: 0x002DF264 File Offset: 0x002DD464
	public bool IsValidTarget(GameObject go, out StarmapHexCellInventory hexCellInventory)
	{
		hexCellInventory = null;
		if (go == null)
		{
			return false;
		}
		hexCellInventory = go.GetComponent<StarmapHexCellInventory>();
		return hexCellInventory != null;
	}

	// Token: 0x0400534E RID: 21326
	private Dictionary<Tag, GameObject> itemRows = new Dictionary<Tag, GameObject>();
}
