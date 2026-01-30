using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E67 RID: 3687
public class ReceptacleSideScreen : SideScreenContent, IRender1000ms
{
	// Token: 0x0600750F RID: 29967 RVA: 0x002CA864 File Offset: 0x002C8A64
	public override string GetTitle()
	{
		if (this.targetReceptacle == null)
		{
			return Strings.Get(this.titleKey).ToString().Replace("{0}", "");
		}
		return string.Format(Strings.Get(this.titleKey), this.targetReceptacle.GetProperName());
	}

	// Token: 0x06007510 RID: 29968 RVA: 0x002CA8BF File Offset: 0x002C8ABF
	private void RecycleToggle(GameObject toggle)
	{
		toggle.SetActive(false);
		this.recycledEntityToggles.Add(toggle);
	}

	// Token: 0x06007511 RID: 29969 RVA: 0x002CA8D4 File Offset: 0x002C8AD4
	private GameObject SpawnToggle(GameObject parent)
	{
		if (this.recycledEntityToggles.Count > 0)
		{
			GameObject gameObject = this.recycledEntityToggles[this.recycledEntityToggles.Count - 1];
			this.recycledEntityToggles.RemoveAt(this.recycledEntityToggles.Count - 1);
			gameObject.transform.SetParent(parent.transform);
			gameObject.SetActive(true);
			return gameObject;
		}
		return Util.KInstantiateUI(this.entityToggle, parent, true);
	}

	// Token: 0x06007512 RID: 29970 RVA: 0x002CA945 File Offset: 0x002C8B45
	private void RefreshCategoryOpen(GameObject categoryHeader, GameObject categoryGrid, Tag tag)
	{
		categoryHeader.GetComponent<MultiToggle>().ChangeState(this.categoryExpandedStatus[tag] ? 0 : 1);
		categoryGrid.gameObject.SetActive(this.categoryExpandedStatus[tag]);
	}

	// Token: 0x06007513 RID: 29971 RVA: 0x002CA97C File Offset: 0x002C8B7C
	public void Initialize(SingleEntityReceptacle target)
	{
		if (target == null)
		{
			global::Debug.LogError("SingleObjectReceptacle provided was null.");
			return;
		}
		this.targetReceptacle = target;
		base.gameObject.SetActive(true);
		this.depositObjectMap = new Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity>();
		this.entityToggles.ForEach(delegate(ReceptacleToggle rbi)
		{
			this.RecycleToggle(rbi.gameObject);
		});
		this.entityToggles.Clear();
		List<GameObject> list = new List<GameObject>();
		if (this.targetReceptacle.possibleDepositObjectTags.Count == 1)
		{
			this.categoryStartExpanded = true;
		}
		using (IEnumerator<Tag> enumerator = this.targetReceptacle.possibleDepositObjectTags.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Tag tag = enumerator.Current;
				List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(tag);
				int num = prefabsWithTag.Count;
				if (this.categoryExpandedStatus.ContainsKey(tag))
				{
					this.categoryExpandedStatus[tag] = this.categoryStartExpanded;
				}
				if (!this.contentContainers.ContainsKey(tag))
				{
					GameObject gameObject = Util.KInstantiateUI(this.categoryContainerPrefab, this.requestObjectListContainerContent, true);
					this.contentContainers.Add(tag, gameObject);
					HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
					component.GetReference<LocText>("HeaderLabel").SetText(tag.ProperName());
					this.categoryExpandedStatus.Add(tag, this.categoryStartExpanded);
					MultiToggle toggle = gameObject.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("HeaderToggle");
					GridLayoutGroup grid = component.GetReference<GridLayoutGroup>("GridLayout");
					MultiToggle toggle3 = toggle;
					toggle3.onClick = (System.Action)Delegate.Combine(toggle3.onClick, new System.Action(delegate()
					{
						this.categoryExpandedStatus[tag] = !this.categoryExpandedStatus[tag];
						this.RefreshCategoryOpen(toggle.gameObject, grid.gameObject, tag);
					}));
					this.RefreshCategoryOpen(toggle.gameObject, grid.gameObject, tag);
				}
				this.RefreshCategoryOpen(this.contentContainers[tag].GetComponent<HierarchyReferences>().GetReference<MultiToggle>("HeaderToggle").gameObject, this.contentContainers[tag].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("GridLayout").gameObject, tag);
				List<IHasSortOrder> list2 = new List<IHasSortOrder>();
				foreach (GameObject gameObject2 in prefabsWithTag)
				{
					if (!this.targetReceptacle.IsValidEntity(gameObject2) || list.Contains(gameObject2))
					{
						num--;
					}
					else
					{
						IHasSortOrder component2 = gameObject2.GetComponent<IHasSortOrder>();
						if (component2 != null)
						{
							list.Add(gameObject2);
							list2.Add(component2);
						}
					}
				}
				global::Debug.Assert(list2.Count == num, "Not all entities in this receptacle implement IHasSortOrder!");
				list2.Sort((IHasSortOrder a, IHasSortOrder b) => a.sortOrder - b.sortOrder);
				foreach (IHasSortOrder hasSortOrder in list2)
				{
					GameObject gameObject3 = (hasSortOrder as MonoBehaviour).gameObject;
					GameObject gameObject4 = this.SpawnToggle(this.contentContainers[tag].GetComponent<HierarchyReferences>().GetReference("GridLayout").gameObject);
					gameObject4.transform.SetAsLastSibling();
					gameObject4.SetActive(true);
					ReceptacleToggle newToggle = gameObject4.GetComponent<ReceptacleToggle>();
					IReceptacleDirection component3 = gameObject3.GetComponent<IReceptacleDirection>();
					string entityName = this.GetEntityName(gameObject3.PrefabID());
					newToggle.title.text = entityName;
					Sprite entityIcon = this.GetEntityIcon(gameObject3.PrefabID());
					if (entityIcon == null)
					{
						entityIcon = this.elementPlaceholderSpr;
					}
					newToggle.image.sprite = entityIcon;
					if (newToggle.toggle == null)
					{
						newToggle.toggle = newToggle.GetComponentInChildren<MultiToggle>();
					}
					MultiToggle toggle2 = newToggle.toggle;
					toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
					{
						this.ToggleClicked(newToggle);
					}));
					ToolTip component4 = newToggle.GetComponent<ToolTip>();
					if (component4 != null)
					{
						component4.SetSimpleTooltip(this.GetEntityTooltip(gameObject3.PrefabID()));
					}
					this.depositObjectMap.Add(newToggle, new ReceptacleSideScreen.SelectableEntity
					{
						tag = gameObject3.PrefabID(),
						direction = ((component3 != null) ? component3.Direction : SingleEntityReceptacle.ReceptacleDirection.Top),
						asset = gameObject3
					});
					this.entityToggles.Add(newToggle);
				}
			}
		}
		this.RestoreSelectionFromOccupant();
		this.selectedEntityToggle = null;
		if (this.entityToggles.Count > 0)
		{
			if (this.entityPreviousSelectionMap.ContainsKey(this.targetReceptacle))
			{
				int index = this.entityPreviousSelectionMap[this.targetReceptacle];
				this.ToggleClicked(this.entityToggles[index]);
			}
			else
			{
				this.subtitleLabel.SetText(Strings.Get(this.subtitleStringSelect).ToString());
				this.requestSelectedEntityBtn.isInteractable = false;
				this.descriptionLabel.SetText(Strings.Get(this.subtitleStringSelectDescription).ToString());
				this.HideAllDescriptorPanels();
			}
		}
		this.onStorageChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1697596308, new Action<object>(this.CheckAmountsAndUpdate));
		this.onOccupantValidChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1820564715, new Action<object>(this.OnOccupantValidChanged));
		this.UpdateState(null);
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x06007514 RID: 29972 RVA: 0x002CAFC0 File Offset: 0x002C91C0
	protected virtual void UpdateState(object data)
	{
		this.requestSelectedEntityBtn.ClearOnClick();
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (this.CheckReceptacleOccupied())
		{
			Uprootable uprootable = this.targetReceptacle.Occupant.GetComponent<Uprootable>();
			if (uprootable != null && uprootable.IsMarkedForUproot)
			{
				this.requestSelectedEntityBtn.onClick += delegate()
				{
					uprootable.ForceCancelUproot(null);
					this.UpdateState(null);
				};
				this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelRemove).ToString();
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingRemoval).ToString(), this.targetReceptacle.Occupant.GetProperName()));
			}
			else
			{
				this.requestSelectedEntityBtn.onClick += delegate()
				{
					this.targetReceptacle.OrderRemoveOccupant();
					this.UpdateState(null);
				};
				this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringRemove).ToString();
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringEntityDeposited).ToString(), this.targetReceptacle.Occupant.GetProperName()));
			}
			this.requestSelectedEntityBtn.isInteractable = true;
			this.ToggleObjectPicker(false);
			Tag tag = this.targetReceptacle.Occupant.GetComponent<KSelectable>().PrefabID();
			this.ConfigureActiveEntity(tag);
			this.SetResultDescriptions(this.targetReceptacle.Occupant);
		}
		else if (this.targetReceptacle.GetActiveRequest != null)
		{
			this.requestSelectedEntityBtn.onClick += delegate()
			{
				this.targetReceptacle.CancelActiveRequest();
				this.ClearSelection();
				this.UpdateAvailableAmounts(null);
				this.UpdateState(null);
			};
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelDeposit).ToString();
			this.requestSelectedEntityBtn.isInteractable = true;
			this.ToggleObjectPicker(false);
			this.ConfigureActiveEntity(this.targetReceptacle.GetActiveRequest.tagsFirst);
			GameObject prefab = Assets.GetPrefab(this.targetReceptacle.GetActiveRequest.tagsFirst);
			if (prefab != null)
			{
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingDelivery).ToString(), prefab.GetProperName()));
				this.SetResultDescriptions(prefab);
			}
		}
		else if (this.selectedEntityToggle != null)
		{
			this.requestSelectedEntityBtn.onClick += delegate()
			{
				this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
				this.UpdateAvailableAmounts(null);
				this.UpdateState(null);
			};
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
			this.targetReceptacle.SetPreview(this.depositObjectMap[this.selectedEntityToggle].tag, false);
			bool isInteractable = this.CanDepositEntity(this.depositObjectMap[this.selectedEntityToggle], true);
			this.requestSelectedEntityBtn.isInteractable = isInteractable;
			this.ToggleObjectPicker(true);
			GameObject prefab2 = Assets.GetPrefab(this.selectedDepositObjectTag);
			if (prefab2 != null)
			{
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingSelection).ToString(), prefab2.GetProperName()));
				this.SetResultDescriptions(prefab2);
			}
		}
		else
		{
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
			this.requestSelectedEntityBtn.isInteractable = false;
			this.ToggleObjectPicker(true);
		}
		this.UpdateAvailableAmounts(null);
		this.RefreshToggleStates();
		this.UpdateListeners();
	}

	// Token: 0x06007515 RID: 29973 RVA: 0x002CB330 File Offset: 0x002C9530
	private void UpdateListeners()
	{
		if (this.CheckReceptacleOccupied())
		{
			if (this.onObjectDestroyedHandle == -1)
			{
				this.onObjectDestroyedHandle = this.targetReceptacle.Occupant.gameObject.Subscribe(1969584890, delegate(object d)
				{
					this.UpdateState(null);
				});
				return;
			}
		}
		else if (this.onObjectDestroyedHandle != -1)
		{
			this.onObjectDestroyedHandle = -1;
		}
	}

	// Token: 0x06007516 RID: 29974 RVA: 0x002CB38C File Offset: 0x002C958C
	private void OnOccupantValidChanged(object _)
	{
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (!this.CheckReceptacleOccupied() && this.targetReceptacle.GetActiveRequest != null)
		{
			bool flag = false;
			ReceptacleSideScreen.SelectableEntity entity;
			if (this.depositObjectMap.TryGetValue(this.selectedEntityToggle, out entity))
			{
				flag = this.CanDepositEntity(entity, true);
			}
			if (!flag)
			{
				this.targetReceptacle.CancelActiveRequest();
				this.ClearSelection();
				this.UpdateState(null);
				this.UpdateAvailableAmounts(null);
			}
		}
	}

	// Token: 0x06007517 RID: 29975 RVA: 0x002CB400 File Offset: 0x002C9600
	protected bool CanDepositEntity(ReceptacleSideScreen.SelectableEntity entity, bool runAdditionalCanDepositTest = false)
	{
		return this.ValidRotationForDeposit(entity.direction) && (!this.RequiresAvailableAmountToDeposit() || this.GetAvailableAmount(entity.tag) > 0f) && (!runAdditionalCanDepositTest || this.AdditionalCanDepositTest());
	}

	// Token: 0x06007518 RID: 29976 RVA: 0x002CB438 File Offset: 0x002C9638
	protected virtual bool AdditionalCanDepositTest()
	{
		return true;
	}

	// Token: 0x06007519 RID: 29977 RVA: 0x002CB43B File Offset: 0x002C963B
	protected virtual bool RequiresAvailableAmountToDeposit()
	{
		return true;
	}

	// Token: 0x0600751A RID: 29978 RVA: 0x002CB43E File Offset: 0x002C963E
	private void ClearSelection()
	{
		this.selectedEntityToggle = null;
		this.RefreshToggleStates();
	}

	// Token: 0x0600751B RID: 29979 RVA: 0x002CB450 File Offset: 0x002C9650
	private void ToggleObjectPicker(bool Show)
	{
		this.requestObjectListContainer.SetActive(Show);
		if (this.scrollBarContainer != null)
		{
			this.scrollBarContainer.SetActive(Show);
		}
		this.requestObjectListContainer.SetActive(Show);
		this.activeEntityContainer.SetActive(!Show);
	}

	// Token: 0x0600751C RID: 29980 RVA: 0x002CB4A0 File Offset: 0x002C96A0
	private void ConfigureActiveEntity(Tag tag)
	{
		string properName = Assets.GetPrefab(tag).GetProperName();
		HierarchyReferences component = this.activeEntityContainer.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("Label").text = properName;
		component.GetReference<Image>("Icon").sprite = this.GetEntityIcon(tag);
	}

	// Token: 0x0600751D RID: 29981 RVA: 0x002CB4EB File Offset: 0x002C96EB
	protected virtual string GetEntityName(Tag prefabTag)
	{
		return Assets.GetPrefab(prefabTag).GetProperName();
	}

	// Token: 0x0600751E RID: 29982 RVA: 0x002CB4F8 File Offset: 0x002C96F8
	protected virtual string GetEntityTooltip(Tag prefabTag)
	{
		InfoDescription component = Assets.GetPrefab(prefabTag).GetComponent<InfoDescription>();
		string text = this.GetEntityName(prefabTag);
		if (component != null)
		{
			text = text + "\n\n" + component.description;
		}
		return text;
	}

	// Token: 0x0600751F RID: 29983 RVA: 0x002CB535 File Offset: 0x002C9735
	protected virtual Sprite GetEntityIcon(Tag prefabTag)
	{
		return Def.GetUISprite(Assets.GetPrefab(prefabTag), "ui", false).first;
	}

	// Token: 0x06007520 RID: 29984 RVA: 0x002CB550 File Offset: 0x002C9750
	public override bool IsValidForTarget(GameObject target)
	{
		SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
		return component != null && component.enabled && target.GetComponent<PlantablePlot>() == null && target.GetComponent<EggIncubator>() == null && target.GetComponent<SpecialCargoBayClusterReceptacle>() == null;
	}

	// Token: 0x06007521 RID: 29985 RVA: 0x002CB5A0 File Offset: 0x002C97A0
	public override void SetTarget(GameObject target)
	{
		SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
		if (component == null)
		{
			global::Debug.LogError("The object selected doesn't have a SingleObjectReceptacle!");
			return;
		}
		this.Initialize(component);
		this.UpdateState(null);
	}

	// Token: 0x06007522 RID: 29986 RVA: 0x002CB5D6 File Offset: 0x002C97D6
	protected virtual void RestoreSelectionFromOccupant()
	{
	}

	// Token: 0x06007523 RID: 29987 RVA: 0x002CB5D8 File Offset: 0x002C97D8
	public override void ClearTarget()
	{
		if (this.targetReceptacle != null)
		{
			if (this.CheckReceptacleOccupied())
			{
				this.targetReceptacle.Occupant.gameObject.Unsubscribe(this.onObjectDestroyedHandle);
				this.onObjectDestroyedHandle = -1;
			}
			this.targetReceptacle.Unsubscribe(this.onStorageChangedHandle);
			this.onStorageChangedHandle = -1;
			this.targetReceptacle.Unsubscribe(this.onOccupantValidChangedHandle);
			this.onOccupantValidChangedHandle = -1;
			if (this.targetReceptacle.GetActiveRequest == null)
			{
				this.targetReceptacle.SetPreview(Tag.Invalid, false);
			}
			SimAndRenderScheduler.instance.Remove(this);
			this.targetReceptacle = null;
		}
	}

	// Token: 0x06007524 RID: 29988 RVA: 0x002CB680 File Offset: 0x002C9880
	protected void RefreshToggleStates()
	{
		foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
		{
			if (this.selectedEntityToggle != keyValuePair.Key)
			{
				if (this.CanDepositEntity(keyValuePair.Value, false))
				{
					this.SetToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Inactive);
				}
				else
				{
					this.SetToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Disabled);
				}
			}
			else if (this.CanDepositEntity(keyValuePair.Value, false))
			{
				this.SetToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Active);
			}
			else
			{
				this.SetToggleState(keyValuePair.Key.toggle, ImageToggleState.State.DisabledActive);
			}
		}
	}

	// Token: 0x06007525 RID: 29989 RVA: 0x002CB758 File Offset: 0x002C9958
	protected void SetToggleState(MultiToggle toggle, ImageToggleState.State state)
	{
		switch (state)
		{
		case ImageToggleState.State.Disabled:
			toggle.ChangeState(2);
			toggle.gameObject.GetComponentsInChildrenOnly<Image>()[1].material = this.desaturatedMaterial;
			return;
		case ImageToggleState.State.Inactive:
			toggle.ChangeState(0);
			toggle.gameObject.GetComponentsInChildrenOnly<Image>()[1].material = this.defaultMaterial;
			return;
		case ImageToggleState.State.Active:
			toggle.ChangeState(1);
			toggle.gameObject.GetComponentsInChildrenOnly<Image>()[1].material = this.defaultMaterial;
			return;
		case ImageToggleState.State.DisabledActive:
			toggle.ChangeState(3);
			toggle.gameObject.GetComponentsInChildrenOnly<Image>()[1].material = this.desaturatedMaterial;
			return;
		default:
			return;
		}
	}

	// Token: 0x06007526 RID: 29990 RVA: 0x002CB7FB File Offset: 0x002C99FB
	public void Render1000ms(float dt)
	{
		this.CheckAmountsAndUpdate(null);
	}

	// Token: 0x06007527 RID: 29991 RVA: 0x002CB804 File Offset: 0x002C9A04
	private void CheckAmountsAndUpdate(object data)
	{
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (this.UpdateAvailableAmounts(null))
		{
			this.UpdateState(null);
		}
	}

	// Token: 0x06007528 RID: 29992 RVA: 0x002CB828 File Offset: 0x002C9A28
	private bool UpdateAvailableAmounts(object data)
	{
		bool result = false;
		foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
		{
			if (!DebugHandler.InstantBuildMode && this.hideUndiscoveredEntities && !DiscoveredResources.Instance.IsDiscovered(keyValuePair.Value.tag))
			{
				keyValuePair.Key.gameObject.SetActive(false);
			}
			else if (!keyValuePair.Key.gameObject.activeSelf)
			{
				keyValuePair.Key.gameObject.SetActive(true);
			}
			float availableAmount = this.GetAvailableAmount(keyValuePair.Value.tag);
			if (keyValuePair.Value.lastAmount != availableAmount)
			{
				result = true;
				keyValuePair.Value.lastAmount = availableAmount;
				keyValuePair.Key.amount.text = availableAmount.ToString();
			}
			if (!this.ValidRotationForDeposit(keyValuePair.Value.direction) || availableAmount <= 0f)
			{
				if (this.selectedEntityToggle != keyValuePair.Key)
				{
					keyValuePair.Key.toggle.ChangeState(2);
				}
				else
				{
					keyValuePair.Key.toggle.ChangeState(3);
				}
			}
			else if (this.selectedEntityToggle != keyValuePair.Key)
			{
				keyValuePair.Key.toggle.ChangeState(0);
			}
			else
			{
				keyValuePair.Key.toggle.ChangeState(1);
			}
		}
		foreach (KeyValuePair<Tag, GameObject> keyValuePair2 in this.contentContainers)
		{
			Transform transform = keyValuePair2.Value.GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("GridLayout").transform;
			bool flag = false;
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).gameObject.activeSelf)
				{
					flag = true;
					break;
				}
			}
			if (keyValuePair2.Value.activeSelf != flag)
			{
				keyValuePair2.Value.SetActive(flag);
			}
		}
		return result;
	}

	// Token: 0x06007529 RID: 29993 RVA: 0x002CBA7C File Offset: 0x002C9C7C
	protected float GetAvailableAmount(Tag tag)
	{
		if (this.ALLOW_ORDER_IGNORING_WOLRD_NEED)
		{
			IEnumerable<Pickupable> pickupables = this.targetReceptacle.GetMyWorld().worldInventory.GetPickupables(tag, true);
			float num = 0f;
			foreach (Pickupable pickupable in pickupables)
			{
				num += (float)Mathf.CeilToInt(pickupable.TotalAmount);
			}
			return num;
		}
		return this.targetReceptacle.GetMyWorld().worldInventory.GetAmount(tag, true);
	}

	// Token: 0x0600752A RID: 29994 RVA: 0x002CBB0C File Offset: 0x002C9D0C
	private bool ValidRotationForDeposit(SingleEntityReceptacle.ReceptacleDirection depositDir)
	{
		return this.targetReceptacle.rotatable == null || depositDir == this.targetReceptacle.Direction;
	}

	// Token: 0x0600752B RID: 29995 RVA: 0x002CBB34 File Offset: 0x002C9D34
	protected virtual void ToggleClicked(ReceptacleToggle toggle)
	{
		if (!this.depositObjectMap.ContainsKey(toggle))
		{
			global::Debug.LogError("Recipe not found on recipe list.");
			return;
		}
		this.selectedEntityToggle = toggle;
		this.entityPreviousSelectionMap[this.targetReceptacle] = this.entityToggles.IndexOf(toggle);
		this.selectedDepositObjectTag = this.depositObjectMap[toggle].tag;
		MutantPlant component = this.depositObjectMap[toggle].asset.GetComponent<MutantPlant>();
		this.selectedDepositObjectAdditionalTag = (component ? component.SubSpeciesID : Tag.Invalid);
		this.RefreshToggleStates();
		this.UpdateAvailableAmounts(null);
		this.UpdateState(null);
	}

	// Token: 0x0600752C RID: 29996 RVA: 0x002CBBDC File Offset: 0x002C9DDC
	private void CreateOrder(bool isInfinite)
	{
		this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
	}

	// Token: 0x0600752D RID: 29997 RVA: 0x002CBBF5 File Offset: 0x002C9DF5
	protected bool CheckReceptacleOccupied()
	{
		return this.targetReceptacle != null && this.targetReceptacle.Occupant != null;
	}

	// Token: 0x0600752E RID: 29998 RVA: 0x002CBC1C File Offset: 0x002C9E1C
	protected virtual void SetResultDescriptions(GameObject go)
	{
		string text = "";
		InfoDescription component = go.GetComponent<InfoDescription>();
		if (component)
		{
			text = component.description;
		}
		else
		{
			KPrefabID component2 = go.GetComponent<KPrefabID>();
			if (component2 != null)
			{
				Element element = ElementLoader.GetElement(component2.PrefabID());
				if (element != null)
				{
					text = element.Description();
				}
			}
			else
			{
				text = go.GetProperName();
			}
		}
		this.descriptionLabel.SetText(text);
	}

	// Token: 0x0600752F RID: 29999 RVA: 0x002CBC84 File Offset: 0x002C9E84
	protected virtual void HideAllDescriptorPanels()
	{
		for (int i = 0; i < this.descriptorPanels.Count; i++)
		{
			this.descriptorPanels[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x040050EE RID: 20718
	protected bool ALLOW_ORDER_IGNORING_WOLRD_NEED = true;

	// Token: 0x040050EF RID: 20719
	[SerializeField]
	protected KButton requestSelectedEntityBtn;

	// Token: 0x040050F0 RID: 20720
	[SerializeField]
	private string requestStringDeposit;

	// Token: 0x040050F1 RID: 20721
	[SerializeField]
	private string requestStringCancelDeposit;

	// Token: 0x040050F2 RID: 20722
	[SerializeField]
	private string requestStringRemove;

	// Token: 0x040050F3 RID: 20723
	[SerializeField]
	private string requestStringCancelRemove;

	// Token: 0x040050F4 RID: 20724
	public GameObject activeEntityContainer;

	// Token: 0x040050F5 RID: 20725
	public GameObject nothingDiscoveredContainer;

	// Token: 0x040050F6 RID: 20726
	[SerializeField]
	private bool categoryStartExpanded;

	// Token: 0x040050F7 RID: 20727
	[SerializeField]
	private GameObject categoryContainerPrefab;

	// Token: 0x040050F8 RID: 20728
	private Dictionary<Tag, GameObject> contentContainers = new Dictionary<Tag, GameObject>();

	// Token: 0x040050F9 RID: 20729
	[SerializeField]
	protected LocText descriptionLabel;

	// Token: 0x040050FA RID: 20730
	protected Dictionary<SingleEntityReceptacle, int> entityPreviousSelectionMap = new Dictionary<SingleEntityReceptacle, int>();

	// Token: 0x040050FB RID: 20731
	[SerializeField]
	private string subtitleStringSelect;

	// Token: 0x040050FC RID: 20732
	[SerializeField]
	private string subtitleStringSelectDescription;

	// Token: 0x040050FD RID: 20733
	[SerializeField]
	private string subtitleStringAwaitingSelection;

	// Token: 0x040050FE RID: 20734
	[SerializeField]
	private string subtitleStringAwaitingDelivery;

	// Token: 0x040050FF RID: 20735
	[SerializeField]
	private string subtitleStringEntityDeposited;

	// Token: 0x04005100 RID: 20736
	[SerializeField]
	private string subtitleStringAwaitingRemoval;

	// Token: 0x04005101 RID: 20737
	[SerializeField]
	private LocText subtitleLabel;

	// Token: 0x04005102 RID: 20738
	[SerializeField]
	private List<DescriptorPanel> descriptorPanels;

	// Token: 0x04005103 RID: 20739
	public Material defaultMaterial;

	// Token: 0x04005104 RID: 20740
	public Material desaturatedMaterial;

	// Token: 0x04005105 RID: 20741
	[SerializeField]
	private GameObject requestObjectListContainer;

	// Token: 0x04005106 RID: 20742
	[SerializeField]
	private GameObject requestObjectListContainerContent;

	// Token: 0x04005107 RID: 20743
	[SerializeField]
	private GameObject scrollBarContainer;

	// Token: 0x04005108 RID: 20744
	[SerializeField]
	private GameObject entityToggle;

	// Token: 0x04005109 RID: 20745
	[SerializeField]
	private Sprite buttonSelectedBG;

	// Token: 0x0400510A RID: 20746
	[SerializeField]
	private Sprite buttonNormalBG;

	// Token: 0x0400510B RID: 20747
	[SerializeField]
	private Sprite elementPlaceholderSpr;

	// Token: 0x0400510C RID: 20748
	[SerializeField]
	private bool hideUndiscoveredEntities;

	// Token: 0x0400510D RID: 20749
	protected ReceptacleToggle selectedEntityToggle;

	// Token: 0x0400510E RID: 20750
	protected SingleEntityReceptacle targetReceptacle;

	// Token: 0x0400510F RID: 20751
	protected Tag selectedDepositObjectTag;

	// Token: 0x04005110 RID: 20752
	protected Tag selectedDepositObjectAdditionalTag;

	// Token: 0x04005111 RID: 20753
	protected Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> depositObjectMap;

	// Token: 0x04005112 RID: 20754
	protected List<ReceptacleToggle> entityToggles = new List<ReceptacleToggle>();

	// Token: 0x04005113 RID: 20755
	private List<GameObject> recycledEntityToggles = new List<GameObject>();

	// Token: 0x04005114 RID: 20756
	private Dictionary<Tag, bool> categoryExpandedStatus = new Dictionary<Tag, bool>();

	// Token: 0x04005115 RID: 20757
	private int onObjectDestroyedHandle = -1;

	// Token: 0x04005116 RID: 20758
	private int onOccupantValidChangedHandle = -1;

	// Token: 0x04005117 RID: 20759
	private int onStorageChangedHandle = -1;

	// Token: 0x020020D6 RID: 8406
	protected class SelectableEntity
	{
		// Token: 0x0400974E RID: 38734
		public Tag tag;

		// Token: 0x0400974F RID: 38735
		public SingleEntityReceptacle.ReceptacleDirection direction;

		// Token: 0x04009750 RID: 38736
		public GameObject asset;

		// Token: 0x04009751 RID: 38737
		public float lastAmount = -1f;
	}
}
