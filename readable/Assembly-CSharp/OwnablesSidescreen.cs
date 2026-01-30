using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E5C RID: 3676
public class OwnablesSidescreen : SideScreenContent
{
	// Token: 0x06007493 RID: 29843 RVA: 0x002C7BB0 File Offset: 0x002C5DB0
	private void DefineCategories()
	{
		if (this.categories == null)
		{
			OwnablesSidescreen.Category[] array = new OwnablesSidescreen.Category[2];
			array[0] = new OwnablesSidescreen.Category((IAssignableIdentity assignableIdentity) => (assignableIdentity as MinionIdentity).GetEquipment(), new OwnablesSidescreenCategoryRow.Data(UI.UISIDESCREENS.OWNABLESSIDESCREEN.CATEGORIES.SUITS, new OwnablesSidescreenCategoryRow.AssignableSlotData[]
			{
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Suit, new Func<IAssignableIdentity, bool>(this.Always)),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Outfit, new Func<IAssignableIdentity, bool>(this.Always))
			}));
			array[1] = new OwnablesSidescreen.Category((IAssignableIdentity assignableIdentity) => assignableIdentity.GetSoleOwner(), new OwnablesSidescreenCategoryRow.Data(UI.UISIDESCREENS.OWNABLESSIDESCREEN.CATEGORIES.AMENITIES, new OwnablesSidescreenCategoryRow.AssignableSlotData[]
			{
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Bed, new Func<IAssignableIdentity, bool>(this.Always)),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Toilet, new Func<IAssignableIdentity, bool>(this.Always)),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.MessStation, new Func<IAssignableIdentity, bool>(MessStation.CanBeAssignedTo))
			}));
			this.categories = array;
		}
	}

	// Token: 0x06007494 RID: 29844 RVA: 0x002C7D17 File Offset: 0x002C5F17
	private bool Always(IAssignableIdentity identity)
	{
		return true;
	}

	// Token: 0x06007495 RID: 29845 RVA: 0x002C7D1A File Offset: 0x002C5F1A
	private Func<IAssignableIdentity, bool> HasAmount(string amountID)
	{
		return delegate(IAssignableIdentity identity)
		{
			if (identity == null)
			{
				return false;
			}
			GameObject targetGameObject = identity.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			return Db.Get().Amounts.Get(amountID).Lookup(targetGameObject) != null;
		};
	}

	// Token: 0x06007496 RID: 29846 RVA: 0x002C7D33 File Offset: 0x002C5F33
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06007497 RID: 29847 RVA: 0x002C7D3B File Offset: 0x002C5F3B
	private void ActivateSecondSidescreen(AssignableSlotInstance slot)
	{
		((OwnablesSecondSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.selectedSlotScreenPrefab, slot.slot.Name)).SetSlot(slot);
		if (slot != null && this.OnSlotInstanceSelected != null)
		{
			this.OnSlotInstanceSelected(slot);
		}
	}

	// Token: 0x06007498 RID: 29848 RVA: 0x002C7D7A File Offset: 0x002C5F7A
	private void DeactivateSecondScreen()
	{
		DetailsScreen.Instance.ClearSecondarySideScreen();
	}

	// Token: 0x06007499 RID: 29849 RVA: 0x002C7D88 File Offset: 0x002C5F88
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.UnsubscribeFromLastTarget();
		this.lastSelectedSlot = null;
		this.DefineCategories();
		this.CreateCategoryRows();
		this.DeactivateSecondScreen();
		this.RefreshSelectedStatusOnRows();
		IAssignableIdentity component = target.GetComponent<IAssignableIdentity>();
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			Assignables owner = this.categories[i].getAssignablesFn(component);
			this.categoryRows[i].SetOwner(owner);
		}
		this.titleSection.SetActive(target.GetComponent<MinionIdentity>().model == BionicMinionConfig.MODEL);
		MinionIdentity minionIdentity = component as MinionIdentity;
		if (minionIdentity != null)
		{
			this.lastTarget = minionIdentity;
			this.minionDestroyedCallbackIDX = minionIdentity.gameObject.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
		}
	}

	// Token: 0x0600749A RID: 29850 RVA: 0x002C7E5A File Offset: 0x002C605A
	private void OnTargetDestroyed(object o)
	{
		this.ClearTarget();
	}

	// Token: 0x0600749B RID: 29851 RVA: 0x002C7E64 File Offset: 0x002C6064
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.lastSelectedSlot = null;
		this.RefreshSelectedStatusOnRows();
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			this.categoryRows[i].SetOwner(null);
		}
		this.DeactivateSecondScreen();
		this.UnsubscribeFromLastTarget();
	}

	// Token: 0x0600749C RID: 29852 RVA: 0x002C7EB4 File Offset: 0x002C60B4
	private void CreateCategoryRows()
	{
		if (this.categoryRows == null)
		{
			this.originalCategoryRow.gameObject.SetActive(false);
			this.categoryRows = new OwnablesSidescreenCategoryRow[this.categories.Length];
			for (int i = 0; i < this.categories.Length; i++)
			{
				OwnablesSidescreenCategoryRow.Data data = this.categories[i].data;
				OwnablesSidescreenCategoryRow component = Util.KInstantiateUI(this.originalCategoryRow.gameObject, this.originalCategoryRow.transform.parent.gameObject, false).GetComponent<OwnablesSidescreenCategoryRow>();
				OwnablesSidescreenCategoryRow ownablesSidescreenCategoryRow = component;
				ownablesSidescreenCategoryRow.OnSlotRowClicked = (Action<OwnablesSidescreenItemRow>)Delegate.Combine(ownablesSidescreenCategoryRow.OnSlotRowClicked, new Action<OwnablesSidescreenItemRow>(this.OnSlotRowClicked));
				component.gameObject.SetActive(true);
				component.SetCategoryData(data);
				this.categoryRows[i] = component;
			}
			this.RefreshSelectedStatusOnRows();
		}
	}

	// Token: 0x0600749D RID: 29853 RVA: 0x002C7F8B File Offset: 0x002C618B
	private void OnSlotRowClicked(OwnablesSidescreenItemRow slotRow)
	{
		if (slotRow.IsLocked || slotRow.SlotInstance == this.lastSelectedSlot)
		{
			this.SetSelectedSlot(null);
			return;
		}
		this.SetSelectedSlot(slotRow.SlotInstance);
	}

	// Token: 0x0600749E RID: 29854 RVA: 0x002C7FB8 File Offset: 0x002C61B8
	public void RefreshSelectedStatusOnRows()
	{
		if (this.categoryRows == null)
		{
			return;
		}
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			this.categoryRows[i].SetSelectedRow_VisualsOnly(this.lastSelectedSlot);
		}
	}

	// Token: 0x0600749F RID: 29855 RVA: 0x002C7FF4 File Offset: 0x002C61F4
	public void SetSelectedSlot(AssignableSlotInstance slot)
	{
		this.lastSelectedSlot = slot;
		if (slot != null)
		{
			this.ActivateSecondSidescreen(slot);
		}
		else
		{
			this.DeactivateSecondScreen();
		}
		this.RefreshSelectedStatusOnRows();
	}

	// Token: 0x060074A0 RID: 29856 RVA: 0x002C8018 File Offset: 0x002C6218
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.categoryRows != null)
		{
			for (int i = 0; i < this.categoryRows.Length; i++)
			{
				if (this.categoryRows[i] != null)
				{
					this.categoryRows[i].SetOwner(null);
				}
			}
		}
		this.UnsubscribeFromLastTarget();
	}

	// Token: 0x060074A1 RID: 29857 RVA: 0x002C806A File Offset: 0x002C626A
	private void UnsubscribeFromLastTarget()
	{
		if (this.lastTarget != null && this.minionDestroyedCallbackIDX != -1)
		{
			this.lastTarget.Unsubscribe(this.minionDestroyedCallbackIDX);
		}
		this.minionDestroyedCallbackIDX = -1;
		this.lastTarget = null;
	}

	// Token: 0x060074A2 RID: 29858 RVA: 0x002C80A2 File Offset: 0x002C62A2
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IAssignableIdentity>() != null;
	}

	// Token: 0x060074A3 RID: 29859 RVA: 0x002C80AD File Offset: 0x002C62AD
	public void OnValidate()
	{
	}

	// Token: 0x060074A4 RID: 29860 RVA: 0x002C80AF File Offset: 0x002C62AF
	private void SetScrollBarVisibility(bool isVisible)
	{
		this.scrollbarSection.gameObject.SetActive(isVisible);
		this.mainLayoutGroup.padding.right = (isVisible ? 20 : 0);
		this.scrollRect.enabled = isVisible;
	}

	// Token: 0x04005098 RID: 20632
	public OwnablesSecondSideScreen selectedSlotScreenPrefab;

	// Token: 0x04005099 RID: 20633
	public OwnablesSidescreenCategoryRow originalCategoryRow;

	// Token: 0x0400509A RID: 20634
	[Header("Editor Settings")]
	public bool usingSlider = true;

	// Token: 0x0400509B RID: 20635
	public GameObject titleSection;

	// Token: 0x0400509C RID: 20636
	public GameObject scrollbarSection;

	// Token: 0x0400509D RID: 20637
	public VerticalLayoutGroup mainLayoutGroup;

	// Token: 0x0400509E RID: 20638
	public KScrollRect scrollRect;

	// Token: 0x0400509F RID: 20639
	private OwnablesSidescreenCategoryRow[] categoryRows;

	// Token: 0x040050A0 RID: 20640
	private AssignableSlotInstance lastSelectedSlot;

	// Token: 0x040050A1 RID: 20641
	private OwnablesSidescreen.Category[] categories;

	// Token: 0x040050A2 RID: 20642
	public Action<AssignableSlotInstance> OnSlotInstanceSelected;

	// Token: 0x040050A3 RID: 20643
	private MinionIdentity lastTarget;

	// Token: 0x040050A4 RID: 20644
	private int minionDestroyedCallbackIDX = -1;

	// Token: 0x020020CC RID: 8396
	public struct Category
	{
		// Token: 0x0600BA5C RID: 47708 RVA: 0x003FB043 File Offset: 0x003F9243
		public Category(Func<IAssignableIdentity, Assignables> getAssignablesFn, OwnablesSidescreenCategoryRow.Data categoryData)
		{
			this.getAssignablesFn = getAssignablesFn;
			this.data = categoryData;
		}

		// Token: 0x04009732 RID: 38706
		public Func<IAssignableIdentity, Assignables> getAssignablesFn;

		// Token: 0x04009733 RID: 38707
		public OwnablesSidescreenCategoryRow.Data data;
	}
}
