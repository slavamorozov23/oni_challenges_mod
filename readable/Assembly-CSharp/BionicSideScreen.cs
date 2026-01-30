using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E1C RID: 3612
public class BionicSideScreen : SideScreenContent
{
	// Token: 0x06007285 RID: 29317 RVA: 0x002BC020 File Offset: 0x002BA220
	private void OnBionicUpgradeSlotClicked(BionicSideScreenUpgradeSlot slotClicked)
	{
		bool flag = slotClicked == null || this.lastSlotSelected == slotClicked.upgradeSlot.GetAssignableSlotInstance();
		bool flag2 = !flag && slotClicked.upgradeSlot.IsLocked;
		this.lastSlotSelected = (flag ? null : slotClicked.upgradeSlot.GetAssignableSlotInstance());
		this.RefreshSelectedStateInSlots();
		AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
		AssignableSlotInstance assignableSlotInstance = (flag || flag2) ? null : slotClicked.upgradeSlot.GetAssignableSlotInstance();
		if (this.ownableSidescreen != null)
		{
			this.ownableSidescreen.SetSelectedSlot(assignableSlotInstance);
			return;
		}
		if (flag || flag2)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
			return;
		}
		((OwnablesSecondSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.ownableSecondSideScreenPrefab, bionicUpgrade.Name)).SetSlot(assignableSlotInstance);
	}

	// Token: 0x06007286 RID: 29318 RVA: 0x002BC0EC File Offset: 0x002BA2EC
	private void RefreshSelectedStateInSlots()
	{
		for (int i = 0; i < this.bionicSlots.Count; i++)
		{
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			bionicSideScreenUpgradeSlot.SetSelected(bionicSideScreenUpgradeSlot.upgradeSlot.GetAssignableSlotInstance() == this.lastSlotSelected);
		}
	}

	// Token: 0x06007287 RID: 29319 RVA: 0x002BC134 File Offset: 0x002BA334
	public void RecreateBionicSlots()
	{
		int num = (this.upgradeMonitor != null) ? this.upgradeMonitor.upgradeComponentSlots.Length : 0;
		for (int i = 0; i < Mathf.Max(num, this.bionicSlots.Count); i++)
		{
			if (i >= this.bionicSlots.Count)
			{
				BionicSideScreenUpgradeSlot item = this.CreateBionicSlot();
				this.bionicSlots.Add(item);
			}
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			if (i < num)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot = this.upgradeMonitor.upgradeComponentSlots[i];
				bionicSideScreenUpgradeSlot.gameObject.SetActive(true);
				bionicSideScreenUpgradeSlot.Setup(upgradeSlot);
				bionicSideScreenUpgradeSlot.SetSelected(bionicSideScreenUpgradeSlot.upgradeSlot.GetAssignableSlotInstance() == this.lastSlotSelected);
			}
			else
			{
				bionicSideScreenUpgradeSlot.Setup(null);
				bionicSideScreenUpgradeSlot.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06007288 RID: 29320 RVA: 0x002BC200 File Offset: 0x002BA400
	private BionicSideScreenUpgradeSlot CreateBionicSlot()
	{
		BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = Util.KInstantiateUI<BionicSideScreenUpgradeSlot>(this.originalBionicSlot.gameObject, this.originalBionicSlot.transform.parent.gameObject, false);
		bionicSideScreenUpgradeSlot.OnClick = (Action<BionicSideScreenUpgradeSlot>)Delegate.Combine(bionicSideScreenUpgradeSlot.OnClick, new Action<BionicSideScreenUpgradeSlot>(this.OnBionicUpgradeSlotClicked));
		return bionicSideScreenUpgradeSlot;
	}

	// Token: 0x06007289 RID: 29321 RVA: 0x002BC255 File Offset: 0x002BA455
	private void OnBionicBecameOnline(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600728A RID: 29322 RVA: 0x002BC25D File Offset: 0x002BA45D
	private void OnBionicBecameOffline(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600728B RID: 29323 RVA: 0x002BC265 File Offset: 0x002BA465
	private void OnBionicWattageChanged(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600728C RID: 29324 RVA: 0x002BC26D File Offset: 0x002BA46D
	private void OnBionicBedTimeChoreStateChanged(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600728D RID: 29325 RVA: 0x002BC275 File Offset: 0x002BA475
	private void OnBionicUpgradeComponentSlotCountChanged(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600728E RID: 29326 RVA: 0x002BC27D File Offset: 0x002BA47D
	private void OnBionicUpgradeChanged(object o)
	{
		this.RecreateBionicSlots();
	}

	// Token: 0x0600728F RID: 29327 RVA: 0x002BC285 File Offset: 0x002BA485
	private void OnBionicTagsChanged(object o)
	{
		if (o == null)
		{
			return;
		}
		if (((Boxed<TagChangedEventData>)o).value.tag == GameTags.BionicBedTime)
		{
			this.OnBionicBedTimeChoreStateChanged(o);
		}
	}

	// Token: 0x06007290 RID: 29328 RVA: 0x002BC2B0 File Offset: 0x002BA4B0
	private void RefreshSlots()
	{
		for (int i = this.bionicSlots.Count - 1; i >= 0; i--)
		{
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			if (bionicSideScreenUpgradeSlot != null)
			{
				bionicSideScreenUpgradeSlot.Refresh();
				bionicSideScreenUpgradeSlot.gameObject.transform.SetAsFirstSibling();
			}
		}
	}

	// Token: 0x06007291 RID: 29329 RVA: 0x002BC304 File Offset: 0x002BA504
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.originalBionicSlot.gameObject.SetActive(false);
		this.ownableSidescreen = base.transform.parent.GetComponentInChildren<OwnablesSidescreen>();
		if (this.ownableSidescreen != null)
		{
			OwnablesSidescreen ownablesSidescreen = this.ownableSidescreen;
			ownablesSidescreen.OnSlotInstanceSelected = (Action<AssignableSlotInstance>)Delegate.Combine(ownablesSidescreen.OnSlotInstanceSelected, new Action<AssignableSlotInstance>(this.OnOwnableSidescreenRowSelected));
		}
	}

	// Token: 0x06007292 RID: 29330 RVA: 0x002BC373 File Offset: 0x002BA573
	private void OnOwnableSidescreenRowSelected(AssignableSlotInstance slot)
	{
		this.lastSlotSelected = slot;
		this.RefreshSelectedStateInSlots();
	}

	// Token: 0x06007293 RID: 29331 RVA: 0x002BC384 File Offset: 0x002BA584
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.lastSlotSelected = null;
		if (this.upgradeMonitor != null)
		{
			this.upgradeMonitor.Unsubscribe(ref this.onBionicBecameOnlineHandle);
			this.upgradeMonitor.Unsubscribe(ref this.onBionicBecameOfflineHandle);
			this.upgradeMonitor.Unsubscribe(ref this.onBionicUpgradeChangedHandle);
			this.upgradeMonitor.Unsubscribe(ref this.onBionicUpgradeComponentSlotCountChangedHandle);
		}
		if (this.batteryMonitor != null)
		{
			this.batteryMonitor.Unsubscribe(ref this.onBionicWattageChangedHandle);
		}
		if (this.bedTimeMonitor != null)
		{
			this.bedTimeMonitor.Unsubscribe(ref this.onBionicTagsChangedHandle);
		}
		this.batteryMonitor = target.GetSMI<BionicBatteryMonitor.Instance>();
		this.upgradeMonitor = target.GetSMI<BionicUpgradesMonitor.Instance>();
		this.bedTimeMonitor = target.GetSMI<BionicBedTimeMonitor.Instance>();
		this.onBionicBecameOnlineHandle = this.upgradeMonitor.Subscribe(160824499, new Action<object>(this.OnBionicBecameOnline));
		this.onBionicBecameOfflineHandle = this.upgradeMonitor.Subscribe(-1730800797, new Action<object>(this.OnBionicBecameOffline));
		this.onBionicUpgradeChangedHandle = this.upgradeMonitor.Subscribe(2000325176, new Action<object>(this.OnBionicUpgradeChanged));
		this.onBionicUpgradeComponentSlotCountChangedHandle = this.batteryMonitor.Subscribe(1095596132, new Action<object>(this.OnBionicUpgradeComponentSlotCountChanged));
		this.onBionicWattageChangedHandle = this.batteryMonitor.Subscribe(1361471071, new Action<object>(this.OnBionicWattageChanged));
		this.onBionicTagsChangedHandle = this.bedTimeMonitor.Subscribe(-1582839653, new Action<object>(this.OnBionicTagsChanged));
		this.RecreateBionicSlots();
		this.RefreshSlots();
	}

	// Token: 0x06007294 RID: 29332 RVA: 0x002BC519 File Offset: 0x002BA719
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshSlots();
		}
	}

	// Token: 0x06007295 RID: 29333 RVA: 0x002BC52B File Offset: 0x002BA72B
	public override void ClearTarget()
	{
		base.ClearTarget();
		if (this.upgradeMonitor != null)
		{
			this.upgradeMonitor.Unsubscribe(ref this.onBionicUpgradeChangedHandle);
		}
		this.bedTimeMonitor = null;
		this.upgradeMonitor = null;
		this.lastSlotSelected = null;
	}

	// Token: 0x06007296 RID: 29334 RVA: 0x002BC561 File Offset: 0x002BA761
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<BionicBatteryMonitor.Instance>() != null;
	}

	// Token: 0x06007297 RID: 29335 RVA: 0x002BC56C File Offset: 0x002BA76C
	public override int GetSideScreenSortOrder()
	{
		return 300;
	}

	// Token: 0x04004F24 RID: 20260
	public OwnablesSecondSideScreen ownableSecondSideScreenPrefab;

	// Token: 0x04004F25 RID: 20261
	public BionicSideScreenUpgradeSlot originalBionicSlot;

	// Token: 0x04004F26 RID: 20262
	private BionicUpgradesMonitor.Instance upgradeMonitor;

	// Token: 0x04004F27 RID: 20263
	private BionicBatteryMonitor.Instance batteryMonitor;

	// Token: 0x04004F28 RID: 20264
	private BionicBedTimeMonitor.Instance bedTimeMonitor;

	// Token: 0x04004F29 RID: 20265
	private List<BionicSideScreenUpgradeSlot> bionicSlots = new List<BionicSideScreenUpgradeSlot>();

	// Token: 0x04004F2A RID: 20266
	private OwnablesSidescreen ownableSidescreen;

	// Token: 0x04004F2B RID: 20267
	private AssignableSlotInstance lastSlotSelected;

	// Token: 0x04004F2C RID: 20268
	private int onBionicBecameOnlineHandle = -1;

	// Token: 0x04004F2D RID: 20269
	private int onBionicBecameOfflineHandle = -1;

	// Token: 0x04004F2E RID: 20270
	private int onBionicUpgradeChangedHandle = -1;

	// Token: 0x04004F2F RID: 20271
	private int onBionicUpgradeComponentSlotCountChangedHandle = -1;

	// Token: 0x04004F30 RID: 20272
	private int onBionicWattageChangedHandle = -1;

	// Token: 0x04004F31 RID: 20273
	private int onBionicTagsChangedHandle = -1;
}
