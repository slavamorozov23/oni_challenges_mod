using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E69 RID: 3689
public class RelatedEntitiesSideScreen : SideScreenContent, ISim1000ms
{
	// Token: 0x06007537 RID: 30007 RVA: 0x002CBDA3 File Offset: 0x002C9FA3
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.rowPrefab.SetActive(false);
		if (show)
		{
			this.RefreshOptions(null);
		}
	}

	// Token: 0x06007538 RID: 30008 RVA: 0x002CBDC2 File Offset: 0x002C9FC2
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IRelatedEntities>() != null;
	}

	// Token: 0x06007539 RID: 30009 RVA: 0x002CBDCD File Offset: 0x002C9FCD
	public override void SetTarget(GameObject target)
	{
		this.target = target;
		this.targetRelatedEntitiesComponent = target.GetComponent<IRelatedEntities>();
		this.RefreshOptions(null);
		this.uiRefreshSubHandle = Game.Instance.Subscribe(1980521255, new Action<object>(this.RefreshOptions));
	}

	// Token: 0x0600753A RID: 30010 RVA: 0x002CBE0A File Offset: 0x002CA00A
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.targetRelatedEntitiesComponent != null)
		{
			Game.Instance.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
	}

	// Token: 0x0600753B RID: 30011 RVA: 0x002CBE34 File Offset: 0x002CA034
	private void RefreshOptions(object data = null)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.ClearRows();
		foreach (KSelectable entity in this.targetRelatedEntitiesComponent.GetRelatedEntities())
		{
			this.AddRow(entity);
		}
	}

	// Token: 0x0600753C RID: 30012 RVA: 0x002CBEA0 File Offset: 0x002CA0A0
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x0600753D RID: 30013 RVA: 0x002CBEE4 File Offset: 0x002CA0E4
	private void AddRow(KSelectable entity)
	{
		GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			SelectTool.Instance.SelectAndFocus(entity.transform.position, entity);
		};
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("label").SetText((SelectTool.Instance.selected == entity) ? ("<b>" + entity.GetProperName() + "</b>") : entity.GetProperName());
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(entity.gameObject, "ui", false).first;
		this.rows.Add(entity, gameObject);
		this.RefreshMainStatus(entity);
	}

	// Token: 0x0600753E RID: 30014 RVA: 0x002CBFCC File Offset: 0x002CA1CC
	private void RefreshMainStatus(KSelectable entity)
	{
		if (entity.IsNullOrDestroyed())
		{
			return;
		}
		if (!this.rows.ContainsKey(entity))
		{
			return;
		}
		HierarchyReferences component = this.rows[entity].GetComponent<HierarchyReferences>();
		StatusItemGroup.Entry statusItem = entity.GetStatusItem(Db.Get().StatusItemCategories.Main);
		LocText reference = component.GetReference<LocText>("status");
		if (statusItem.data != null)
		{
			reference.gameObject.SetActive(true);
			reference.SetText(statusItem.item.GetName(statusItem.data));
			return;
		}
		reference.gameObject.SetActive(false);
		reference.SetText("");
	}

	// Token: 0x0600753F RID: 30015 RVA: 0x002CC068 File Offset: 0x002CA268
	public void Sim1000ms(float dt)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		foreach (KeyValuePair<KSelectable, GameObject> keyValuePair in this.rows)
		{
			this.RefreshMainStatus(keyValuePair.Key);
		}
	}

	// Token: 0x0400511C RID: 20764
	private GameObject target;

	// Token: 0x0400511D RID: 20765
	private IRelatedEntities targetRelatedEntitiesComponent;

	// Token: 0x0400511E RID: 20766
	public GameObject rowPrefab;

	// Token: 0x0400511F RID: 20767
	public RectTransform rowContainer;

	// Token: 0x04005120 RID: 20768
	public Dictionary<KSelectable, GameObject> rows = new Dictionary<KSelectable, GameObject>();

	// Token: 0x04005121 RID: 20769
	private int uiRefreshSubHandle = -1;
}
