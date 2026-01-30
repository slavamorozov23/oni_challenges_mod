using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000C4E RID: 3150
public class CharacterSelectionController : KModalScreen
{
	// Token: 0x17000701 RID: 1793
	// (get) Token: 0x06005FAF RID: 24495 RVA: 0x002319FD File Offset: 0x0022FBFD
	// (set) Token: 0x06005FB0 RID: 24496 RVA: 0x00231A05 File Offset: 0x0022FC05
	public bool IsStarterMinion { get; set; }

	// Token: 0x17000702 RID: 1794
	// (get) Token: 0x06005FB1 RID: 24497 RVA: 0x00231A0E File Offset: 0x0022FC0E
	public bool AllowsReplacing
	{
		get
		{
			return this.allowsReplacing;
		}
	}

	// Token: 0x06005FB2 RID: 24498 RVA: 0x00231A16 File Offset: 0x0022FC16
	protected virtual void OnProceed()
	{
	}

	// Token: 0x06005FB3 RID: 24499 RVA: 0x00231A18 File Offset: 0x0022FC18
	protected virtual void OnDeliverableAdded()
	{
	}

	// Token: 0x06005FB4 RID: 24500 RVA: 0x00231A1A File Offset: 0x0022FC1A
	protected virtual void OnDeliverableRemoved()
	{
	}

	// Token: 0x06005FB5 RID: 24501 RVA: 0x00231A1C File Offset: 0x0022FC1C
	protected virtual void OnLimitReached()
	{
	}

	// Token: 0x06005FB6 RID: 24502 RVA: 0x00231A1E File Offset: 0x0022FC1E
	protected virtual void OnLimitUnreached()
	{
	}

	// Token: 0x06005FB7 RID: 24503 RVA: 0x00231A20 File Offset: 0x0022FC20
	protected virtual void InitializeContainers()
	{
		this.DisableProceedButton();
		if (this.containers != null && this.containers.Count > 0)
		{
			return;
		}
		this.OnReplacedEvent = null;
		this.containers = new List<ITelepadDeliverableContainer>();
		if (this.IsStarterMinion || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CarePackages).id != "Enabled")
		{
			this.numberOfDuplicantOptions = 3;
			this.numberOfCarePackageOptions = 0;
		}
		else
		{
			this.numberOfCarePackageOptions = ((UnityEngine.Random.Range(0, 101) > 70) ? 2 : 1);
			this.numberOfDuplicantOptions = 4 - this.numberOfCarePackageOptions;
		}
		for (int i = 0; i < this.numberOfDuplicantOptions; i++)
		{
			CharacterContainer characterContainer = Util.KInstantiateUI<CharacterContainer>(this.containerPrefab.gameObject, this.containerParent, false);
			characterContainer.SetController(this);
			characterContainer.SetReshufflingState(true);
			this.containers.Add(characterContainer);
		}
		for (int j = 0; j < this.numberOfCarePackageOptions; j++)
		{
			CarePackageContainer carePackageContainer = Util.KInstantiateUI<CarePackageContainer>(this.carePackageContainerPrefab.gameObject, this.containerParent, false);
			carePackageContainer.SetController(this);
			this.containers.Add(carePackageContainer);
			carePackageContainer.gameObject.transform.SetSiblingIndex(UnityEngine.Random.Range(0, carePackageContainer.transform.parent.childCount));
		}
		this.selectedDeliverables = new List<ITelepadDeliverable>();
	}

	// Token: 0x06005FB8 RID: 24504 RVA: 0x00231B68 File Offset: 0x0022FD68
	public virtual void OnPressBack()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.ForceStopEditingTitle();
			}
		}
		this.Show(false);
	}

	// Token: 0x06005FB9 RID: 24505 RVA: 0x00231BD0 File Offset: 0x0022FDD0
	public void RemoveLast()
	{
		if (this.selectedDeliverables == null || this.selectedDeliverables.Count == 0)
		{
			return;
		}
		ITelepadDeliverable obj = this.selectedDeliverables[this.selectedDeliverables.Count - 1];
		if (this.OnReplacedEvent != null)
		{
			this.OnReplacedEvent(obj);
		}
	}

	// Token: 0x06005FBA RID: 24506 RVA: 0x00231C20 File Offset: 0x0022FE20
	public void AddDeliverable(ITelepadDeliverable deliverable)
	{
		if (this.selectedDeliverables.Contains(deliverable))
		{
			global::Debug.Log("Tried to add the same minion twice.");
			return;
		}
		if (this.selectedDeliverables.Count >= this.selectableCount)
		{
			global::Debug.LogError("Tried to add minions beyond the allowed limit");
			return;
		}
		this.selectedDeliverables.Add(deliverable);
		this.OnDeliverableAdded();
		if (this.selectedDeliverables.Count == this.selectableCount)
		{
			this.EnableProceedButton();
			if (this.OnLimitReachedEvent != null)
			{
				this.OnLimitReachedEvent();
			}
			this.OnLimitReached();
		}
	}

	// Token: 0x06005FBB RID: 24507 RVA: 0x00231CA8 File Offset: 0x0022FEA8
	public void RemoveDeliverable(ITelepadDeliverable deliverable)
	{
		bool flag = this.selectedDeliverables.Count >= this.selectableCount;
		this.selectedDeliverables.Remove(deliverable);
		this.OnDeliverableRemoved();
		if (flag && this.selectedDeliverables.Count < this.selectableCount)
		{
			this.DisableProceedButton();
			if (this.OnLimitUnreachedEvent != null)
			{
				this.OnLimitUnreachedEvent();
			}
			this.OnLimitUnreached();
		}
	}

	// Token: 0x06005FBC RID: 24508 RVA: 0x00231D12 File Offset: 0x0022FF12
	public bool IsSelected(ITelepadDeliverable deliverable)
	{
		return this.selectedDeliverables.Contains(deliverable);
	}

	// Token: 0x06005FBD RID: 24509 RVA: 0x00231D20 File Offset: 0x0022FF20
	protected void EnableProceedButton()
	{
		this.proceedButton.isInteractable = true;
		this.proceedButton.ClearOnClick();
		this.proceedButton.onClick += delegate()
		{
			this.OnProceed();
		};
	}

	// Token: 0x06005FBE RID: 24510 RVA: 0x00231D50 File Offset: 0x0022FF50
	protected void DisableProceedButton()
	{
		this.proceedButton.ClearOnClick();
		this.proceedButton.isInteractable = false;
		this.proceedButton.onClick += delegate()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		};
	}

	// Token: 0x04003FE5 RID: 16357
	[SerializeField]
	private CharacterContainer containerPrefab;

	// Token: 0x04003FE6 RID: 16358
	[SerializeField]
	private CarePackageContainer carePackageContainerPrefab;

	// Token: 0x04003FE7 RID: 16359
	[SerializeField]
	private GameObject containerParent;

	// Token: 0x04003FE8 RID: 16360
	[SerializeField]
	protected KButton proceedButton;

	// Token: 0x04003FE9 RID: 16361
	protected int numberOfDuplicantOptions = 3;

	// Token: 0x04003FEA RID: 16362
	protected int numberOfCarePackageOptions;

	// Token: 0x04003FEB RID: 16363
	[SerializeField]
	protected int selectableCount;

	// Token: 0x04003FEC RID: 16364
	[SerializeField]
	private bool allowsReplacing;

	// Token: 0x04003FEE RID: 16366
	protected List<ITelepadDeliverable> selectedDeliverables;

	// Token: 0x04003FEF RID: 16367
	protected List<ITelepadDeliverableContainer> containers;

	// Token: 0x04003FF0 RID: 16368
	public System.Action OnLimitReachedEvent;

	// Token: 0x04003FF1 RID: 16369
	public System.Action OnLimitUnreachedEvent;

	// Token: 0x04003FF2 RID: 16370
	public Action<bool> OnReshuffleEvent;

	// Token: 0x04003FF3 RID: 16371
	public Action<ITelepadDeliverable> OnReplacedEvent;

	// Token: 0x04003FF4 RID: 16372
	public System.Action OnProceedEvent;
}
