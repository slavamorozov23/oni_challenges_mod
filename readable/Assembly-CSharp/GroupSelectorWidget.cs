using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ECD RID: 3789
public class GroupSelectorWidget : MonoBehaviour
{
	// Token: 0x06007947 RID: 31047 RVA: 0x002E9A87 File Offset: 0x002E7C87
	public void Initialize(object widget_id, IList<GroupSelectorWidget.ItemData> options, GroupSelectorWidget.ItemCallbacks item_callbacks)
	{
		this.widgetID = widget_id;
		this.options = options;
		this.itemCallbacks = item_callbacks;
		this.addItemButton.onClick += this.OnAddItemClicked;
	}

	// Token: 0x06007948 RID: 31048 RVA: 0x002E9AB8 File Offset: 0x002E7CB8
	public void Reconfigure(IList<int> selected_option_indices)
	{
		this.selectedOptionIndices.Clear();
		this.selectedOptionIndices.AddRange(selected_option_indices);
		this.selectedOptionIndices.Sort();
		this.addItemButton.isInteractable = (this.selectedOptionIndices.Count < this.options.Count);
		this.RebuildSelectedVisualizers();
	}

	// Token: 0x06007949 RID: 31049 RVA: 0x002E9B10 File Offset: 0x002E7D10
	private void OnAddItemClicked()
	{
		if (!this.IsSubPanelOpen())
		{
			if (this.RebuildSubPanelOptions() > 0)
			{
				this.unselectedItemsPanel.GetComponent<GridLayoutGroup>().constraintCount = Mathf.Min(this.numExpectedPanelColumns, this.unselectedItemsPanel.childCount);
				this.unselectedItemsPanel.gameObject.SetActive(true);
				this.unselectedItemsPanel.GetComponent<Selectable>().Select();
				return;
			}
		}
		else
		{
			this.CloseSubPanel();
		}
	}

	// Token: 0x0600794A RID: 31050 RVA: 0x002E9B7C File Offset: 0x002E7D7C
	private void OnItemAdded(int option_idx)
	{
		if (this.itemCallbacks.onItemAdded != null)
		{
			this.itemCallbacks.onItemAdded(this.widgetID, this.options[option_idx].userData);
			this.RebuildSubPanelOptions();
		}
	}

	// Token: 0x0600794B RID: 31051 RVA: 0x002E9BB9 File Offset: 0x002E7DB9
	private void OnItemRemoved(int option_idx)
	{
		if (this.itemCallbacks.onItemRemoved != null)
		{
			this.itemCallbacks.onItemRemoved(this.widgetID, this.options[option_idx].userData);
		}
	}

	// Token: 0x0600794C RID: 31052 RVA: 0x002E9BF0 File Offset: 0x002E7DF0
	private void RebuildSelectedVisualizers()
	{
		foreach (GameObject original in this.selectedVisualizers)
		{
			Util.KDestroyGameObject(original);
		}
		this.selectedVisualizers.Clear();
		foreach (int idx in this.selectedOptionIndices)
		{
			GameObject item = this.CreateItem(idx, new Action<int>(this.OnItemRemoved), this.selectedItemsPanel.gameObject, true);
			this.selectedVisualizers.Add(item);
		}
	}

	// Token: 0x0600794D RID: 31053 RVA: 0x002E9CB4 File Offset: 0x002E7EB4
	private GameObject CreateItem(int idx, Action<int> on_click, GameObject parent, bool is_selected_item)
	{
		GameObject gameObject = Util.KInstantiateUI(this.itemTemplate, parent, true);
		KButton component = gameObject.GetComponent<KButton>();
		component.onClick += delegate()
		{
			on_click(idx);
		};
		component.fgImage.sprite = this.options[idx].sprite;
		if (parent == this.selectedItemsPanel.gameObject)
		{
			HierarchyReferences component2 = component.GetComponent<HierarchyReferences>();
			if (component2 != null)
			{
				Component reference = component2.GetReference("CancelImg");
				if (reference != null)
				{
					reference.gameObject.SetActive(true);
				}
			}
		}
		gameObject.GetComponent<ToolTip>().OnToolTip = (() => this.itemCallbacks.getItemHoverText(this.widgetID, this.options[idx].userData, is_selected_item));
		return gameObject;
	}

	// Token: 0x0600794E RID: 31054 RVA: 0x002E9D86 File Offset: 0x002E7F86
	public bool IsSubPanelOpen()
	{
		return this.unselectedItemsPanel.gameObject.activeSelf;
	}

	// Token: 0x0600794F RID: 31055 RVA: 0x002E9D98 File Offset: 0x002E7F98
	public void CloseSubPanel()
	{
		this.ClearSubPanelOptions();
		this.unselectedItemsPanel.gameObject.SetActive(false);
	}

	// Token: 0x06007950 RID: 31056 RVA: 0x002E9DB4 File Offset: 0x002E7FB4
	private void ClearSubPanelOptions()
	{
		foreach (object obj in this.unselectedItemsPanel.transform)
		{
			Util.KDestroyGameObject(((Transform)obj).gameObject);
		}
	}

	// Token: 0x06007951 RID: 31057 RVA: 0x002E9E14 File Offset: 0x002E8014
	private int RebuildSubPanelOptions()
	{
		IList<int> list = this.itemCallbacks.getSubPanelDisplayIndices(this.widgetID);
		if (list.Count > 0)
		{
			this.ClearSubPanelOptions();
			using (IEnumerator<int> enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num = enumerator.Current;
					if (!this.selectedOptionIndices.Contains(num))
					{
						this.CreateItem(num, new Action<int>(this.OnItemAdded), this.unselectedItemsPanel.gameObject, false);
					}
				}
				goto IL_7E;
			}
		}
		this.CloseSubPanel();
		IL_7E:
		return list.Count;
	}

	// Token: 0x040054A8 RID: 21672
	[SerializeField]
	private GameObject itemTemplate;

	// Token: 0x040054A9 RID: 21673
	[SerializeField]
	private RectTransform selectedItemsPanel;

	// Token: 0x040054AA RID: 21674
	[SerializeField]
	private RectTransform unselectedItemsPanel;

	// Token: 0x040054AB RID: 21675
	[SerializeField]
	private KButton addItemButton;

	// Token: 0x040054AC RID: 21676
	[SerializeField]
	private int numExpectedPanelColumns = 3;

	// Token: 0x040054AD RID: 21677
	private object widgetID;

	// Token: 0x040054AE RID: 21678
	private GroupSelectorWidget.ItemCallbacks itemCallbacks;

	// Token: 0x040054AF RID: 21679
	private IList<GroupSelectorWidget.ItemData> options;

	// Token: 0x040054B0 RID: 21680
	private List<int> selectedOptionIndices = new List<int>();

	// Token: 0x040054B1 RID: 21681
	private List<GameObject> selectedVisualizers = new List<GameObject>();

	// Token: 0x0200212E RID: 8494
	[Serializable]
	public struct ItemData
	{
		// Token: 0x0600BBA6 RID: 48038 RVA: 0x003FDD7D File Offset: 0x003FBF7D
		public ItemData(Sprite sprite, object user_data)
		{
			this.sprite = sprite;
			this.userData = user_data;
		}

		// Token: 0x04009883 RID: 39043
		public Sprite sprite;

		// Token: 0x04009884 RID: 39044
		public object userData;
	}

	// Token: 0x0200212F RID: 8495
	public struct ItemCallbacks
	{
		// Token: 0x04009885 RID: 39045
		public Func<object, IList<int>> getSubPanelDisplayIndices;

		// Token: 0x04009886 RID: 39046
		public Action<object, object> onItemAdded;

		// Token: 0x04009887 RID: 39047
		public Action<object, object> onItemRemoved;

		// Token: 0x04009888 RID: 39048
		public Func<object, object, bool, string> getItemHoverText;
	}
}
