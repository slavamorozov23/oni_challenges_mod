using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

// Token: 0x02000E25 RID: 3621
public class CheckboxListGroupSideScreen : SideScreenContent
{
	// Token: 0x060072D7 RID: 29399 RVA: 0x002BD4BF File Offset: 0x002BB6BF
	private CheckboxListGroupSideScreen.CheckboxContainer InstantiateCheckboxContainer()
	{
		return new CheckboxListGroupSideScreen.CheckboxContainer(Util.KInstantiateUI(this.checkboxGroupPrefab, this.groupParent.gameObject, true).GetComponent<HierarchyReferences>());
	}

	// Token: 0x060072D8 RID: 29400 RVA: 0x002BD4E2 File Offset: 0x002BB6E2
	private GameObject InstantiateCheckbox()
	{
		return Util.KInstantiateUI(this.checkboxPrefab, this.checkboxParent.gameObject, false);
	}

	// Token: 0x060072D9 RID: 29401 RVA: 0x002BD4FB File Offset: 0x002BB6FB
	protected override void OnSpawn()
	{
		this.checkboxPrefab.SetActive(false);
		this.checkboxGroupPrefab.SetActive(false);
		base.OnSpawn();
	}

	// Token: 0x060072DA RID: 29402 RVA: 0x002BD51C File Offset: 0x002BB71C
	public override bool IsValidForTarget(GameObject target)
	{
		ICheckboxListGroupControl[] components = target.GetComponents<ICheckboxListGroupControl>();
		if (components != null)
		{
			ICheckboxListGroupControl[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].SidescreenEnabled())
				{
					return true;
				}
			}
		}
		using (List<ICheckboxListGroupControl>.Enumerator enumerator = target.GetAllSMI<ICheckboxListGroupControl>().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.SidescreenEnabled())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060072DB RID: 29403 RVA: 0x002BD5A0 File Offset: 0x002BB7A0
	public override int GetSideScreenSortOrder()
	{
		if (this.targets == null)
		{
			return 20;
		}
		return this.targets[0].CheckboxSideScreenSortOrder();
	}

	// Token: 0x060072DC RID: 29404 RVA: 0x002BD5C0 File Offset: 0x002BB7C0
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targets = target.GetAllSMI<ICheckboxListGroupControl>();
		this.targets.AddRange(target.GetComponents<ICheckboxListGroupControl>());
		this.Rebuild(target);
		this.uiRefreshSubHandle = this.currentBuildTarget.Subscribe(1980521255, new Action<object>(this.Refresh));
	}

	// Token: 0x060072DD RID: 29405 RVA: 0x002BD628 File Offset: 0x002BB828
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.currentBuildTarget != null)
		{
			this.currentBuildTarget.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
		this.ReleaseContainers(this.activeChecklistGroups.Count);
	}

	// Token: 0x060072DE RID: 29406 RVA: 0x002BD675 File Offset: 0x002BB875
	public override string GetTitle()
	{
		if (this.targets != null && this.targets.Count > 0 && this.targets[0] != null)
		{
			return this.targets[0].Title;
		}
		return base.GetTitle();
	}

	// Token: 0x060072DF RID: 29407 RVA: 0x002BD6B4 File Offset: 0x002BB8B4
	private void Rebuild(GameObject buildTarget)
	{
		if (this.checkboxContainerPool == null)
		{
			this.checkboxContainerPool = new ObjectPool<CheckboxListGroupSideScreen.CheckboxContainer>(new Func<CheckboxListGroupSideScreen.CheckboxContainer>(this.InstantiateCheckboxContainer), null, null, null, false, 10, 10000);
			this.checkboxPool = new GameObjectPool(new Func<GameObject>(this.InstantiateCheckbox), delegate(GameObject _)
			{
			}, 0);
		}
		this.descriptionLabel.enabled = !this.targets[0].Description.IsNullOrWhiteSpace();
		if (!this.targets[0].Description.IsNullOrWhiteSpace())
		{
			this.descriptionLabel.SetText(this.targets[0].Description);
		}
		if (buildTarget == this.currentBuildTarget)
		{
			this.Refresh(null);
			return;
		}
		this.currentBuildTarget = buildTarget;
		foreach (ICheckboxListGroupControl checkboxListGroupControl in this.targets)
		{
			foreach (ICheckboxListGroupControl.ListGroup group in checkboxListGroupControl.GetData())
			{
				CheckboxListGroupSideScreen.CheckboxContainer groupUI = this.checkboxContainerPool.Get();
				this.InitContainer(checkboxListGroupControl, group, groupUI);
			}
		}
	}

	// Token: 0x060072E0 RID: 29408 RVA: 0x002BD80C File Offset: 0x002BBA0C
	[ContextMenu("Force refresh")]
	private void Test()
	{
		this.Refresh(null);
	}

	// Token: 0x060072E1 RID: 29409 RVA: 0x002BD818 File Offset: 0x002BBA18
	private void Refresh(object data = null)
	{
		int num = 0;
		foreach (ICheckboxListGroupControl checkboxListGroupControl in this.targets)
		{
			foreach (ICheckboxListGroupControl.ListGroup listGroup in checkboxListGroupControl.GetData())
			{
				if (++num > this.activeChecklistGroups.Count)
				{
					this.InitContainer(checkboxListGroupControl, listGroup, this.checkboxContainerPool.Get());
				}
				CheckboxListGroupSideScreen.CheckboxContainer checkboxContainer = this.activeChecklistGroups[num - 1];
				if (listGroup.resolveTitleCallback != null)
				{
					checkboxContainer.container.GetReference<LocText>("Text").SetText(listGroup.resolveTitleCallback(listGroup.title));
				}
				for (int j = 0; j < listGroup.checkboxItems.Length; j++)
				{
					ICheckboxListGroupControl.CheckboxItem data3 = listGroup.checkboxItems[j];
					if (checkboxContainer.checkboxUIItems.Count <= j)
					{
						this.CreateSingleCheckBoxForGroupUI(checkboxContainer);
					}
					HierarchyReferences checkboxUI = checkboxContainer.checkboxUIItems[j];
					this.SetCheckboxData(checkboxUI, data3, checkboxListGroupControl);
				}
				while (checkboxContainer.checkboxUIItems.Count > listGroup.checkboxItems.Length)
				{
					HierarchyReferences checkbox = checkboxContainer.checkboxUIItems[checkboxContainer.checkboxUIItems.Count - 1];
					this.RemoveSingleCheckboxFromContainer(checkbox, checkboxContainer);
				}
			}
		}
		this.ReleaseContainers(this.activeChecklistGroups.Count - num);
	}

	// Token: 0x060072E2 RID: 29410 RVA: 0x002BD9B8 File Offset: 0x002BBBB8
	private void ReleaseContainers(int count)
	{
		int count2 = this.activeChecklistGroups.Count;
		for (int i = 1; i <= count; i++)
		{
			int index = count2 - i;
			CheckboxListGroupSideScreen.CheckboxContainer checkboxContainer = this.activeChecklistGroups[index];
			this.activeChecklistGroups.RemoveAt(index);
			for (int j = checkboxContainer.checkboxUIItems.Count - 1; j >= 0; j--)
			{
				HierarchyReferences checkbox = checkboxContainer.checkboxUIItems[j];
				this.RemoveSingleCheckboxFromContainer(checkbox, checkboxContainer);
			}
			checkboxContainer.container.gameObject.SetActive(false);
			this.checkboxContainerPool.Release(checkboxContainer);
		}
	}

	// Token: 0x060072E3 RID: 29411 RVA: 0x002BDA4C File Offset: 0x002BBC4C
	private void InitContainer(ICheckboxListGroupControl target, ICheckboxListGroupControl.ListGroup group, CheckboxListGroupSideScreen.CheckboxContainer groupUI)
	{
		this.activeChecklistGroups.Add(groupUI);
		groupUI.container.gameObject.SetActive(true);
		string text = group.title;
		if (group.resolveTitleCallback != null)
		{
			text = group.resolveTitleCallback(text);
		}
		groupUI.container.GetReference<LocText>("Text").SetText(text);
		foreach (ICheckboxListGroupControl.CheckboxItem data in group.checkboxItems)
		{
			this.CreateSingleCheckBoxForGroupUI(data, target, groupUI);
		}
	}

	// Token: 0x060072E4 RID: 29412 RVA: 0x002BDACF File Offset: 0x002BBCCF
	public void RemoveSingleCheckboxFromContainer(HierarchyReferences checkbox, CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		container.checkboxUIItems.Remove(checkbox);
		checkbox.gameObject.SetActive(false);
		checkbox.transform.SetParent(this.checkboxParent);
		this.checkboxPool.ReleaseInstance(checkbox.gameObject);
	}

	// Token: 0x060072E5 RID: 29413 RVA: 0x002BDB0C File Offset: 0x002BBD0C
	public HierarchyReferences CreateSingleCheckBoxForGroupUI(CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		HierarchyReferences component = this.checkboxPool.GetInstance().GetComponent<HierarchyReferences>();
		component.gameObject.SetActive(true);
		container.checkboxUIItems.Add(component);
		component.transform.SetParent(container.container.transform);
		return component;
	}

	// Token: 0x060072E6 RID: 29414 RVA: 0x002BDB5C File Offset: 0x002BBD5C
	public HierarchyReferences CreateSingleCheckBoxForGroupUI(ICheckboxListGroupControl.CheckboxItem data, ICheckboxListGroupControl target, CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		HierarchyReferences hierarchyReferences = this.CreateSingleCheckBoxForGroupUI(container);
		this.SetCheckboxData(hierarchyReferences, data, target);
		return hierarchyReferences;
	}

	// Token: 0x060072E7 RID: 29415 RVA: 0x002BDB7C File Offset: 0x002BBD7C
	public void SetCheckboxData(HierarchyReferences checkboxUI, ICheckboxListGroupControl.CheckboxItem data, ICheckboxListGroupControl target)
	{
		LocText reference = checkboxUI.GetReference<LocText>("Text");
		reference.SetText(data.text);
		reference.SetLinkOverrideAction(data.overrideLinkActions);
		checkboxUI.GetReference<Image>("Check").enabled = data.isOn;
		ToolTip reference2 = checkboxUI.GetReference<ToolTip>("Tooltip");
		reference2.SetSimpleTooltip(data.tooltip);
		reference2.refreshWhileHovering = (data.resolveTooltipCallback != null);
		reference2.OnToolTip = delegate()
		{
			if (data.resolveTooltipCallback == null)
			{
				return data.tooltip;
			}
			return data.resolveTooltipCallback(data.tooltip, target);
		};
	}

	// Token: 0x04004F5A RID: 20314
	public const int DefaultCheckboxListSideScreenSortOrder = 20;

	// Token: 0x04004F5B RID: 20315
	private ObjectPool<CheckboxListGroupSideScreen.CheckboxContainer> checkboxContainerPool;

	// Token: 0x04004F5C RID: 20316
	private GameObjectPool checkboxPool;

	// Token: 0x04004F5D RID: 20317
	[SerializeField]
	private GameObject checkboxGroupPrefab;

	// Token: 0x04004F5E RID: 20318
	[SerializeField]
	private GameObject checkboxPrefab;

	// Token: 0x04004F5F RID: 20319
	[SerializeField]
	private RectTransform groupParent;

	// Token: 0x04004F60 RID: 20320
	[SerializeField]
	private RectTransform checkboxParent;

	// Token: 0x04004F61 RID: 20321
	[SerializeField]
	private LocText descriptionLabel;

	// Token: 0x04004F62 RID: 20322
	private List<ICheckboxListGroupControl> targets;

	// Token: 0x04004F63 RID: 20323
	private GameObject currentBuildTarget;

	// Token: 0x04004F64 RID: 20324
	private int uiRefreshSubHandle = -1;

	// Token: 0x04004F65 RID: 20325
	private List<CheckboxListGroupSideScreen.CheckboxContainer> activeChecklistGroups = new List<CheckboxListGroupSideScreen.CheckboxContainer>();

	// Token: 0x020020A6 RID: 8358
	public class CheckboxContainer
	{
		// Token: 0x0600B9F9 RID: 47609 RVA: 0x003FA23D File Offset: 0x003F843D
		public CheckboxContainer(HierarchyReferences container)
		{
			this.container = container;
		}

		// Token: 0x040096D0 RID: 38608
		public HierarchyReferences container;

		// Token: 0x040096D1 RID: 38609
		public List<HierarchyReferences> checkboxUIItems = new List<HierarchyReferences>();
	}
}
