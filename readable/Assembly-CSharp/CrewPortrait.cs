using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D6F RID: 3439
[AddComponentMenu("KMonoBehaviour/scripts/CrewPortrait")]
[Serializable]
public class CrewPortrait : KMonoBehaviour
{
	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x06006AB5 RID: 27317 RVA: 0x0028630D File Offset: 0x0028450D
	// (set) Token: 0x06006AB6 RID: 27318 RVA: 0x00286315 File Offset: 0x00284515
	public IAssignableIdentity identityObject { get; private set; }

	// Token: 0x06006AB7 RID: 27319 RVA: 0x0028631E File Offset: 0x0028451E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.startTransparent)
		{
			base.StartCoroutine(this.AlphaIn());
		}
		this.requiresRefresh = true;
	}

	// Token: 0x06006AB8 RID: 27320 RVA: 0x00286342 File Offset: 0x00284542
	private IEnumerator AlphaIn()
	{
		this.SetAlpha(0f);
		for (float i = 0f; i < 1f; i += Time.unscaledDeltaTime * 4f)
		{
			this.SetAlpha(i);
			yield return 0;
		}
		this.SetAlpha(1f);
		yield break;
	}

	// Token: 0x06006AB9 RID: 27321 RVA: 0x00286351 File Offset: 0x00284551
	private void OnRoleChanged(object data)
	{
		if (this.controller == null)
		{
			return;
		}
		CrewPortrait.RefreshHat(this.identityObject, this.controller);
	}

	// Token: 0x06006ABA RID: 27322 RVA: 0x00286374 File Offset: 0x00284574
	private void RegisterEvents()
	{
		if (this.areEventsRegistered)
		{
			return;
		}
		KMonoBehaviour kmonoBehaviour = this.identityObject as KMonoBehaviour;
		if (kmonoBehaviour == null)
		{
			return;
		}
		this.assignedRoleChanged = kmonoBehaviour.Subscribe(540773776, new Action<object>(this.OnRoleChanged));
		this.areEventsRegistered = true;
	}

	// Token: 0x06006ABB RID: 27323 RVA: 0x002863C4 File Offset: 0x002845C4
	private void UnregisterEvents()
	{
		if (!this.areEventsRegistered)
		{
			return;
		}
		this.areEventsRegistered = false;
		KMonoBehaviour kmonoBehaviour = this.identityObject as KMonoBehaviour;
		if (kmonoBehaviour == null)
		{
			return;
		}
		kmonoBehaviour.Unsubscribe(ref this.assignedRoleChanged);
	}

	// Token: 0x06006ABC RID: 27324 RVA: 0x00286403 File Offset: 0x00284603
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RegisterEvents();
		this.ForceRefresh();
	}

	// Token: 0x06006ABD RID: 27325 RVA: 0x00286417 File Offset: 0x00284617
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.UnregisterEvents();
	}

	// Token: 0x06006ABE RID: 27326 RVA: 0x00286425 File Offset: 0x00284625
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.UnregisterEvents();
	}

	// Token: 0x06006ABF RID: 27327 RVA: 0x00286434 File Offset: 0x00284634
	public void SetIdentityObject(IAssignableIdentity identity, bool jobEnabled = true)
	{
		this.UnregisterEvents();
		this.identityObject = identity;
		this.RegisterEvents();
		this.targetImage.enabled = true;
		if (this.identityObject != null)
		{
			this.targetImage.enabled = false;
		}
		if (this.useLabels && (identity is MinionIdentity || identity is MinionAssignablesProxy))
		{
			this.SetDuplicantJobTitleActive(jobEnabled);
		}
		this.requiresRefresh = true;
	}

	// Token: 0x06006AC0 RID: 27328 RVA: 0x0028649C File Offset: 0x0028469C
	public void SetSubTitle(string newTitle)
	{
		if (this.subTitle != null)
		{
			if (string.IsNullOrEmpty(newTitle))
			{
				this.subTitle.gameObject.SetActive(false);
				return;
			}
			this.subTitle.gameObject.SetActive(true);
			this.subTitle.SetText(newTitle);
		}
	}

	// Token: 0x06006AC1 RID: 27329 RVA: 0x002864EE File Offset: 0x002846EE
	public void SetDuplicantJobTitleActive(bool state)
	{
		if (this.duplicantJob != null && this.duplicantJob.gameObject.activeInHierarchy != state)
		{
			this.duplicantJob.gameObject.SetActive(state);
		}
	}

	// Token: 0x06006AC2 RID: 27330 RVA: 0x00286522 File Offset: 0x00284722
	public void ForceRefresh()
	{
		this.requiresRefresh = true;
	}

	// Token: 0x06006AC3 RID: 27331 RVA: 0x0028652B File Offset: 0x0028472B
	public void Update()
	{
		if (this.requiresRefresh && (this.controller == null || this.controller.enabled))
		{
			this.requiresRefresh = false;
			this.Rebuild();
		}
	}

	// Token: 0x06006AC4 RID: 27332 RVA: 0x00286560 File Offset: 0x00284760
	private void Rebuild()
	{
		if (this.controller == null)
		{
			this.controller = base.GetComponentInChildren<KBatchedAnimController>();
			if (this.controller == null)
			{
				if (this.targetImage != null)
				{
					this.targetImage.enabled = true;
				}
				global::Debug.LogWarning("Controller for [" + base.name + "] null");
				return;
			}
		}
		CrewPortrait.SetPortraitData(this.identityObject, this.controller, this.useDefaultExpression);
		if (this.useLabels && this.duplicantName != null)
		{
			this.duplicantName.SetText((!this.identityObject.IsNullOrDestroyed()) ? this.identityObject.GetProperName() : "");
			if (this.identityObject is MinionIdentity && this.duplicantJob != null)
			{
				this.duplicantJob.SetText((this.identityObject != null) ? (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle() : "");
				this.duplicantJob.GetComponent<ToolTip>().toolTip = (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle();
			}
		}
	}

	// Token: 0x06006AC5 RID: 27333 RVA: 0x00286698 File Offset: 0x00284898
	private static void RefreshHat(IAssignableIdentity identityObject, KBatchedAnimController controller)
	{
		string hat_id = "";
		MinionIdentity minionIdentity = identityObject as MinionIdentity;
		if (minionIdentity != null)
		{
			hat_id = minionIdentity.GetComponent<MinionResume>().CurrentHat;
		}
		else if (identityObject as StoredMinionIdentity != null)
		{
			hat_id = (identityObject as StoredMinionIdentity).currentHat;
		}
		MinionResume.ApplyHat(hat_id, controller);
	}

	// Token: 0x06006AC6 RID: 27334 RVA: 0x002866EC File Offset: 0x002848EC
	public static void SetPortraitData(IAssignableIdentity identityObject, KBatchedAnimController controller, bool useDefaultExpression = true)
	{
		if (identityObject == null)
		{
			controller.gameObject.SetActive(false);
			return;
		}
		MinionIdentity minionIdentity = identityObject as MinionIdentity;
		if (minionIdentity == null)
		{
			MinionAssignablesProxy minionAssignablesProxy = identityObject as MinionAssignablesProxy;
			if (minionAssignablesProxy != null && minionAssignablesProxy.target != null)
			{
				minionIdentity = (minionAssignablesProxy.target as MinionIdentity);
			}
		}
		controller.gameObject.SetActive(true);
		controller.Play("ui_idle", KAnim.PlayMode.Once, 1f, 0f);
		SymbolOverrideController component = controller.GetComponent<SymbolOverrideController>();
		component.RemoveAllSymbolOverrides(0);
		if (minionIdentity != null)
		{
			HashSet<KAnimHashedString> hashSet = new HashSet<KAnimHashedString>();
			HashSet<KAnimHashedString> hashSet2 = new HashSet<KAnimHashedString>();
			Accessorizer component2 = minionIdentity.GetComponent<Accessorizer>();
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				Accessory accessory = component2.GetAccessory(accessorySlot);
				if (accessory != null)
				{
					component.AddSymbolOverride(accessorySlot.targetSymbolId, accessory.symbol, 0);
					hashSet.Add(accessorySlot.targetSymbolId);
				}
				else
				{
					hashSet2.Add(accessorySlot.targetSymbolId);
				}
			}
			controller.BatchSetSymbolsVisiblity(hashSet, true);
			controller.BatchSetSymbolsVisiblity(hashSet2, false);
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component2.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			CrewPortrait.RefreshHat(minionIdentity, controller);
		}
		else
		{
			HashSet<KAnimHashedString> hashSet3 = new HashSet<KAnimHashedString>();
			HashSet<KAnimHashedString> hashSet4 = new HashSet<KAnimHashedString>();
			StoredMinionIdentity storedMinionIdentity = identityObject as StoredMinionIdentity;
			if (storedMinionIdentity == null)
			{
				MinionAssignablesProxy minionAssignablesProxy2 = identityObject as MinionAssignablesProxy;
				if (minionAssignablesProxy2 != null && minionAssignablesProxy2.target != null)
				{
					storedMinionIdentity = (minionAssignablesProxy2.target as StoredMinionIdentity);
				}
			}
			if (!(storedMinionIdentity != null))
			{
				controller.gameObject.SetActive(false);
				return;
			}
			foreach (AccessorySlot accessorySlot2 in Db.Get().AccessorySlots.resources)
			{
				Accessory accessory2 = storedMinionIdentity.GetAccessory(accessorySlot2);
				if (accessory2 != null)
				{
					component.AddSymbolOverride(accessorySlot2.targetSymbolId, accessory2.symbol, 0);
					hashSet3.Add(accessorySlot2.targetSymbolId);
				}
				else
				{
					hashSet4.Add(accessorySlot2.targetSymbolId);
				}
			}
			controller.BatchSetSymbolsVisiblity(hashSet3, true);
			controller.BatchSetSymbolsVisiblity(hashSet4, false);
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			CrewPortrait.RefreshHat(storedMinionIdentity, controller);
		}
		float animScale = 0.25f;
		controller.animScale = animScale;
		string s = "ui_idle";
		controller.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		controller.SetSymbolVisiblity("snapTo_neck", false);
		controller.SetSymbolVisiblity("snapTo_goggles", false);
	}

	// Token: 0x06006AC7 RID: 27335 RVA: 0x00286A84 File Offset: 0x00284C84
	public void SetAlpha(float value)
	{
		if (this.controller == null)
		{
			return;
		}
		if ((float)this.controller.TintColour.a != value)
		{
			this.controller.TintColour = new Color(1f, 1f, 1f, value);
		}
	}

	// Token: 0x0400496C RID: 18796
	public Image targetImage;

	// Token: 0x0400496D RID: 18797
	public bool startTransparent;

	// Token: 0x0400496E RID: 18798
	public bool useLabels = true;

	// Token: 0x0400496F RID: 18799
	[SerializeField]
	public KBatchedAnimController controller;

	// Token: 0x04004970 RID: 18800
	public float animScaleBase = 0.2f;

	// Token: 0x04004971 RID: 18801
	public LocText duplicantName;

	// Token: 0x04004972 RID: 18802
	public LocText duplicantJob;

	// Token: 0x04004973 RID: 18803
	public LocText subTitle;

	// Token: 0x04004974 RID: 18804
	public bool useDefaultExpression = true;

	// Token: 0x04004975 RID: 18805
	private bool requiresRefresh;

	// Token: 0x04004976 RID: 18806
	private bool areEventsRegistered;

	// Token: 0x04004977 RID: 18807
	private int assignedRoleChanged = -1;
}
