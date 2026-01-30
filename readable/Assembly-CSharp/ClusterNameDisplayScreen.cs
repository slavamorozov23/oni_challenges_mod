using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CB9 RID: 3257
public class ClusterNameDisplayScreen : KScreen
{
	// Token: 0x060063E2 RID: 25570 RVA: 0x00253433 File Offset: 0x00251633
	public static void DestroyInstance()
	{
		ClusterNameDisplayScreen.Instance = null;
	}

	// Token: 0x060063E3 RID: 25571 RVA: 0x0025343B File Offset: 0x0025163B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClusterNameDisplayScreen.Instance = this;
	}

	// Token: 0x060063E4 RID: 25572 RVA: 0x00253449 File Offset: 0x00251649
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060063E5 RID: 25573 RVA: 0x00253454 File Offset: 0x00251654
	public void AddNewEntry(ClusterGridEntity representedObject)
	{
		if (this.GetEntry(representedObject) != null)
		{
			return;
		}
		ClusterNameDisplayScreen.Entry entry = new ClusterNameDisplayScreen.Entry();
		entry.grid_entity = representedObject;
		GameObject gameObject = Util.KInstantiateUI(this.nameAndBarsPrefab, base.gameObject, true);
		entry.display_go = gameObject;
		gameObject.name = representedObject.name + " cluster overlay";
		entry.Name = representedObject.name;
		entry.refs = gameObject.GetComponent<HierarchyReferences>();
		entry.bars_go = entry.refs.GetReference<RectTransform>("Bars").gameObject;
		this.m_entries.Add(entry);
		if (representedObject.GetComponent<KSelectable>() != null)
		{
			this.UpdateName(representedObject);
			this.UpdateBars(representedObject);
		}
	}

	// Token: 0x060063E6 RID: 25574 RVA: 0x00253504 File Offset: 0x00251704
	private void LateUpdate()
	{
		if (App.isLoading || App.IsExiting)
		{
			return;
		}
		int num = this.m_entries.Count;
		int i = 0;
		while (i < num)
		{
			if (this.m_entries[i].grid_entity != null && ClusterMapScreen.GetRevealLevel(this.m_entries[i].grid_entity) == ClusterRevealLevel.Visible)
			{
				Transform gridEntityNameTarget = ClusterMapScreen.Instance.GetGridEntityNameTarget(this.m_entries[i].grid_entity);
				if (gridEntityNameTarget != null)
				{
					Vector3 position = gridEntityNameTarget.GetPosition();
					this.m_entries[i].display_go.GetComponent<RectTransform>().SetPositionAndRotation(position, Quaternion.identity);
					this.m_entries[i].display_go.SetActive(this.m_entries[i].grid_entity.IsVisible && this.m_entries[i].grid_entity.ShowName());
				}
				else if (this.m_entries[i].display_go.activeSelf)
				{
					this.m_entries[i].display_go.SetActive(false);
				}
				this.UpdateBars(this.m_entries[i].grid_entity);
				if (this.m_entries[i].bars_go != null)
				{
					this.m_entries[i].bars_go.GetComponentsInChildren<KCollider2D>(false, this.workingList);
					foreach (KCollider2D kcollider2D in this.workingList)
					{
						kcollider2D.MarkDirty(false);
					}
				}
				i++;
			}
			else
			{
				UnityEngine.Object.Destroy(this.m_entries[i].display_go);
				num--;
				this.m_entries[i] = this.m_entries[num];
			}
		}
		this.m_entries.RemoveRange(num, this.m_entries.Count - num);
	}

	// Token: 0x060063E7 RID: 25575 RVA: 0x0025371C File Offset: 0x0025191C
	public void UpdateName(ClusterGridEntity representedObject)
	{
		ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		KSelectable component = representedObject.GetComponent<KSelectable>();
		entry.display_go.name = component.GetProperName() + " cluster overlay";
		LocText componentInChildren = entry.display_go.GetComponentInChildren<LocText>();
		if (componentInChildren != null)
		{
			componentInChildren.text = component.GetProperName();
		}
	}

	// Token: 0x060063E8 RID: 25576 RVA: 0x00253778 File Offset: 0x00251978
	private void UpdateBars(ClusterGridEntity representedObject)
	{
		ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		GenericUIProgressBar componentInChildren = entry.bars_go.GetComponentInChildren<GenericUIProgressBar>(true);
		if (entry.grid_entity.ShowProgressBar())
		{
			if (!componentInChildren.gameObject.activeSelf)
			{
				componentInChildren.gameObject.SetActive(true);
			}
			componentInChildren.SetFillPercentage(entry.grid_entity.GetProgress());
			return;
		}
		if (componentInChildren.gameObject.activeSelf)
		{
			componentInChildren.gameObject.SetActive(false);
		}
	}

	// Token: 0x060063E9 RID: 25577 RVA: 0x002537F0 File Offset: 0x002519F0
	private ClusterNameDisplayScreen.Entry GetEntry(ClusterGridEntity entity)
	{
		return this.m_entries.Find((ClusterNameDisplayScreen.Entry entry) => entry.grid_entity == entity);
	}

	// Token: 0x040043F7 RID: 17399
	public static ClusterNameDisplayScreen Instance;

	// Token: 0x040043F8 RID: 17400
	public GameObject nameAndBarsPrefab;

	// Token: 0x040043F9 RID: 17401
	[SerializeField]
	private Color selectedColor;

	// Token: 0x040043FA RID: 17402
	[SerializeField]
	private Color defaultColor;

	// Token: 0x040043FB RID: 17403
	private List<ClusterNameDisplayScreen.Entry> m_entries = new List<ClusterNameDisplayScreen.Entry>();

	// Token: 0x040043FC RID: 17404
	private List<KCollider2D> workingList = new List<KCollider2D>();

	// Token: 0x02001EE3 RID: 7907
	private class Entry
	{
		// Token: 0x040090F6 RID: 37110
		public string Name;

		// Token: 0x040090F7 RID: 37111
		public ClusterGridEntity grid_entity;

		// Token: 0x040090F8 RID: 37112
		public GameObject display_go;

		// Token: 0x040090F9 RID: 37113
		public GameObject bars_go;

		// Token: 0x040090FA RID: 37114
		public HierarchyReferences refs;
	}
}
