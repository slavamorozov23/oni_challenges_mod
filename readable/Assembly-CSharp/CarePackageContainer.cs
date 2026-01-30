using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C4C RID: 3148
public class CarePackageContainer : KScreen, ITelepadDeliverableContainer
{
	// Token: 0x06005F4F RID: 24399 RVA: 0x0022E624 File Offset: 0x0022C824
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x170006FF RID: 1791
	// (get) Token: 0x06005F50 RID: 24400 RVA: 0x0022E62C File Offset: 0x0022C82C
	public CarePackageInfo Info
	{
		get
		{
			return this.info;
		}
	}

	// Token: 0x06005F51 RID: 24401 RVA: 0x0022E634 File Offset: 0x0022C834
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
		base.StartCoroutine(this.DelayedGeneration());
	}

	// Token: 0x06005F52 RID: 24402 RVA: 0x0022E64F File Offset: 0x0022C84F
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06005F53 RID: 24403 RVA: 0x0022E656 File Offset: 0x0022C856
	private IEnumerator DelayedGeneration()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		if (this.controller != null)
		{
			this.GenerateCharacter(this.controller.IsStarterMinion);
		}
		yield break;
	}

	// Token: 0x06005F54 RID: 24404 RVA: 0x0022E665 File Offset: 0x0022C865
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.animController != null)
		{
			this.animController.gameObject.DeleteObject();
			this.animController = null;
		}
	}

	// Token: 0x06005F55 RID: 24405 RVA: 0x0022E694 File Offset: 0x0022C894
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.controller != null)
		{
			CharacterSelectionController characterSelectionController = this.controller;
			characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Remove(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
			CharacterSelectionController characterSelectionController2 = this.controller;
			characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Remove(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
			CharacterSelectionController characterSelectionController3 = this.controller;
			characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Remove(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		}
	}

	// Token: 0x06005F56 RID: 24406 RVA: 0x0022E72A File Offset: 0x0022C92A
	private void Initialize()
	{
		this.professionIconMap = new Dictionary<string, Sprite>();
		this.professionIcons.ForEach(delegate(CarePackageContainer.ProfessionIcon ic)
		{
			this.professionIconMap.Add(ic.professionName, ic.iconImg);
		});
		if (CarePackageContainer.containers == null)
		{
			CarePackageContainer.containers = new List<ITelepadDeliverableContainer>();
		}
		CarePackageContainer.containers.Add(this);
	}

	// Token: 0x06005F57 RID: 24407 RVA: 0x0022E76C File Offset: 0x0022C96C
	private void GenerateCharacter(bool is_starter)
	{
		int num = 0;
		do
		{
			this.info = Immigration.Instance.RandomCarePackage();
			num++;
		}
		while (this.IsCharacterRedundant() && num < 20);
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.carePackageInstanceData = new CarePackageContainer.CarePackageInstanceData();
		this.carePackageInstanceData.info = this.info;
		if (this.info.facadeID == "SELECTRANDOM")
		{
			this.carePackageInstanceData.facadeID = Db.GetEquippableFacades().resources.FindAll((EquippableFacadeResource match) => match.DefID == this.info.id).GetRandom<EquippableFacadeResource>().Id;
		}
		else
		{
			this.carePackageInstanceData.facadeID = this.info.facadeID;
		}
		this.SetAnimator();
		this.SetInfoText();
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x06005F58 RID: 24408 RVA: 0x0022E878 File Offset: 0x0022CA78
	private void SetAnimator()
	{
		GameObject prefab = Assets.GetPrefab(this.info.id.ToTag());
		EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(this.info.id);
		int num;
		if (ElementLoader.FindElementByName(this.info.id) != null)
		{
			num = 1;
		}
		else if (foodInfo != null && foodInfo.CaloriesPerUnit > 0f)
		{
			num = (int)Mathf.Max(1f, this.info.quantity % foodInfo.CaloriesPerUnit);
		}
		else
		{
			num = (int)this.info.quantity;
		}
		if (prefab != null)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject, false);
				gameObject.SetActive(true);
				Image component = gameObject.GetComponent<Image>();
				global::Tuple<Sprite, Color> uisprite;
				if (!this.carePackageInstanceData.facadeID.IsNullOrWhiteSpace())
				{
					uisprite = Def.GetUISprite(prefab.PrefabID(), this.carePackageInstanceData.facadeID);
				}
				else
				{
					uisprite = Def.GetUISprite(prefab, "ui", false);
				}
				component.sprite = uisprite.first;
				component.color = uisprite.second;
				this.entryIcons.Add(gameObject);
				if (num > 1)
				{
					int num2;
					int num3;
					int num4;
					if (num % 2 == 1)
					{
						num2 = Mathf.CeilToInt((float)(num / 2));
						num3 = num2 - i;
						num4 = ((num3 > 0) ? 1 : -1);
						num3 = Mathf.Abs(num3);
					}
					else
					{
						num2 = num / 2 - 1;
						if (i <= num2)
						{
							num3 = Mathf.Abs(num2 - i);
							num4 = -1;
						}
						else
						{
							num3 = Mathf.Abs(num2 + 1 - i);
							num4 = 1;
						}
					}
					int num5 = 0;
					if (num % 2 == 0)
					{
						num5 = ((i <= num2) ? -6 : 6);
						gameObject.transform.SetPosition(gameObject.transform.position += new Vector3((float)num5, 0f, 0f));
					}
					gameObject.transform.localScale = new Vector3(1f - (float)num3 * 0.1f, 1f - (float)num3 * 0.1f, 1f);
					gameObject.transform.Rotate(0f, 0f, 3f * (float)num3 * (float)num4);
					gameObject.transform.SetPosition(gameObject.transform.position + new Vector3(25f * (float)num3 * (float)num4, 5f * (float)num3) + new Vector3((float)num5, 0f, 0f));
					gameObject.GetComponent<Canvas>().sortingOrder = num - num3;
				}
			}
			return;
		}
		GameObject gameObject2 = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject, false);
		gameObject2.SetActive(true);
		Image component2 = gameObject2.GetComponent<Image>();
		component2.sprite = Def.GetUISpriteFromMultiObjectAnim(ElementLoader.GetElement(this.info.id.ToTag()).substance.anim, "ui", false, "");
		component2.color = ElementLoader.GetElement(this.info.id.ToTag()).substance.uiColour;
		this.entryIcons.Add(gameObject2);
	}

	// Token: 0x06005F59 RID: 24409 RVA: 0x0022EBBC File Offset: 0x0022CDBC
	private string GetSpawnableName()
	{
		GameObject prefab = Assets.GetPrefab(this.info.id);
		if (prefab == null)
		{
			Element element = ElementLoader.FindElementByName(this.info.id);
			if (element != null)
			{
				return element.substance.name;
			}
			return "";
		}
		else
		{
			if (string.IsNullOrEmpty(this.carePackageInstanceData.facadeID))
			{
				return prefab.GetProperName();
			}
			return EquippableFacade.GetNameOverride(this.carePackageInstanceData.info.id, this.carePackageInstanceData.facadeID);
		}
	}

	// Token: 0x06005F5A RID: 24410 RVA: 0x0022EC48 File Offset: 0x0022CE48
	private string GetSpawnableQuantityOnly()
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, GameUtil.GetFormattedMass(this.info.quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, GameUtil.GetFormattedCaloriesForItem(this.info.id, this.info.quantity, GameUtil.TimeSlice.None, true));
		}
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, this.info.quantity.ToString());
	}

	// Token: 0x06005F5B RID: 24411 RVA: 0x0022ECFC File Offset: 0x0022CEFC
	private string GetCurrentQuantity(WorldInventory inventory)
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			float amount = inventory.GetAmount(this.info.id.ToTag(), false);
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			float calories = WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(this.info.id, inventory, true);
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true));
		}
		float amount2 = inventory.GetAmount(this.info.id.ToTag(), false);
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, amount2.ToString());
	}

	// Token: 0x06005F5C RID: 24412 RVA: 0x0022EDC8 File Offset: 0x0022CFC8
	private string GetSpawnableQuantity()
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, GameUtil.GetFormattedMass(this.info.quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), Assets.GetPrefab(this.info.id).GetProperName());
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, GameUtil.GetFormattedCaloriesForItem(this.info.id, this.info.quantity, GameUtil.TimeSlice.None, true), Assets.GetPrefab(this.info.id).GetProperName());
		}
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT, Assets.GetPrefab(this.info.id).GetProperName(), this.info.quantity.ToString());
	}

	// Token: 0x06005F5D RID: 24413 RVA: 0x0022EEC8 File Offset: 0x0022D0C8
	private string GetSpawnableDescription()
	{
		Element element = ElementLoader.GetElement(this.info.id.ToTag());
		if (element != null)
		{
			return element.Description();
		}
		GameObject prefab = Assets.GetPrefab(this.info.id);
		if (prefab == null)
		{
			return "";
		}
		InfoDescription component = prefab.GetComponent<InfoDescription>();
		if (component != null)
		{
			return component.description;
		}
		return prefab.GetProperName();
	}

	// Token: 0x06005F5E RID: 24414 RVA: 0x0022EF38 File Offset: 0x0022D138
	private string GetSpawnableEffects()
	{
		GameObject prefab = Assets.GetPrefab(this.info.id);
		if (prefab == null)
		{
			return "";
		}
		string text = "";
		IGameObjectEffectDescriptor[] components = prefab.GetComponents<IGameObjectEffectDescriptor>();
		if (components != null)
		{
			IGameObjectEffectDescriptor[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				List<Descriptor> descriptors = array[i].GetDescriptors(prefab);
				if (descriptors != null)
				{
					foreach (Descriptor descriptor in descriptors)
					{
						text = text + descriptor.text + "\n";
					}
				}
			}
		}
		return text;
	}

	// Token: 0x06005F5F RID: 24415 RVA: 0x0022EFF0 File Offset: 0x0022D1F0
	private void SetInfoText()
	{
		this.characterName.SetText(this.GetSpawnableName());
		this.effects.SetText(this.GetSpawnableEffects());
		this.description.SetText(this.GetSpawnableDescription());
		this.itemName.SetText(this.GetSpawnableName());
		this.quantity.SetText(this.GetSpawnableQuantityOnly());
		this.currentQuantity.SetText(this.GetCurrentQuantity(ClusterManager.Instance.activeWorld.worldInventory));
	}

	// Token: 0x06005F60 RID: 24416 RVA: 0x0022F074 File Offset: 0x0022D274
	public void SelectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.AddDeliverable(this.carePackageInstanceData);
		}
		if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
		{
			MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 1f, true);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetActive();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.DeselectDeliverable();
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 0f, true);
			}
		};
		this.selectedBorder.SetActive(true);
		this.titleBar.color = this.selectedTitleColor;
	}

	// Token: 0x06005F61 RID: 24417 RVA: 0x0022F11C File Offset: 0x0022D31C
	public void DeselectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveDeliverable(this.carePackageInstanceData);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetInactive();
		this.selectButton.Deselect();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
		this.selectedBorder.SetActive(false);
		this.titleBar.color = this.deselectedTitleColor;
	}

	// Token: 0x06005F62 RID: 24418 RVA: 0x0022F1A2 File Offset: 0x0022D3A2
	private void OnReplacedEvent(ITelepadDeliverable stats)
	{
		if (stats == this.carePackageInstanceData)
		{
			this.DeselectDeliverable();
		}
	}

	// Token: 0x06005F63 RID: 24419 RVA: 0x0022F1B4 File Offset: 0x0022D3B4
	private void OnCharacterSelectionLimitReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		if (this.controller.AllowsReplacing)
		{
			this.selectButton.onClick += this.ReplaceCharacterSelection;
			return;
		}
		this.selectButton.onClick += this.CantSelectCharacter;
	}

	// Token: 0x06005F64 RID: 24420 RVA: 0x0022F22A File Offset: 0x0022D42A
	private void CantSelectCharacter()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x06005F65 RID: 24421 RVA: 0x0022F23C File Offset: 0x0022D43C
	private void ReplaceCharacterSelection()
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.RemoveLast();
		this.SelectDeliverable();
	}

	// Token: 0x06005F66 RID: 24422 RVA: 0x0022F260 File Offset: 0x0022D460
	private void OnCharacterSelectionLimitUnReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
	}

	// Token: 0x06005F67 RID: 24423 RVA: 0x0022F2B1 File Offset: 0x0022D4B1
	public void SetReshufflingState(bool enable)
	{
		this.reshuffleButton.gameObject.SetActive(enable);
	}

	// Token: 0x06005F68 RID: 24424 RVA: 0x0022F2C4 File Offset: 0x0022D4C4
	private void Reshuffle(bool is_starter)
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
		{
			this.DeselectDeliverable();
		}
		this.ClearEntryIcons();
		this.GenerateCharacter(is_starter);
	}

	// Token: 0x06005F69 RID: 24425 RVA: 0x0022F2FC File Offset: 0x0022D4FC
	public void SetController(CharacterSelectionController csc)
	{
		if (csc == this.controller)
		{
			return;
		}
		this.controller = csc;
		CharacterSelectionController characterSelectionController = this.controller;
		characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Combine(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
		CharacterSelectionController characterSelectionController2 = this.controller;
		characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Combine(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
		CharacterSelectionController characterSelectionController3 = this.controller;
		characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Combine(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		CharacterSelectionController characterSelectionController4 = this.controller;
		characterSelectionController4.OnReplacedEvent = (Action<ITelepadDeliverable>)Delegate.Combine(characterSelectionController4.OnReplacedEvent, new Action<ITelepadDeliverable>(this.OnReplacedEvent));
	}

	// Token: 0x06005F6A RID: 24426 RVA: 0x0022F3BC File Offset: 0x0022D5BC
	public void DisableSelectButton()
	{
		this.selectButton.soundPlayer.AcceptClickCondition = (() => false);
		this.selectButton.GetComponent<ImageToggleState>().SetDisabled();
		this.selectButton.soundPlayer.Enabled = false;
	}

	// Token: 0x06005F6B RID: 24427 RVA: 0x0022F41C File Offset: 0x0022D61C
	private bool IsCharacterRedundant()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in CarePackageContainer.containers)
		{
			if (telepadDeliverableContainer != this)
			{
				CarePackageContainer carePackageContainer = telepadDeliverableContainer as CarePackageContainer;
				if (carePackageContainer != null && carePackageContainer.info == this.info)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06005F6C RID: 24428 RVA: 0x0022F490 File Offset: 0x0022D690
	public string GetValueColor(bool isPositive)
	{
		if (!isPositive)
		{
			return "<color=#ff2222ff>";
		}
		return "<color=green>";
	}

	// Token: 0x06005F6D RID: 24429 RVA: 0x0022F4A0 File Offset: 0x0022D6A0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape))
		{
			this.controller.OnPressBack();
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x06005F6E RID: 24430 RVA: 0x0022F4C4 File Offset: 0x0022D6C4
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x06005F6F RID: 24431 RVA: 0x0022F4D4 File Offset: 0x0022D6D4
	protected override void OnCmpEnable()
	{
		base.OnActivate();
		if (this.info == null)
		{
			return;
		}
		this.ClearEntryIcons();
		this.SetAnimator();
		this.SetInfoText();
	}

	// Token: 0x06005F70 RID: 24432 RVA: 0x0022F4F8 File Offset: 0x0022D6F8
	private void ClearEntryIcons()
	{
		for (int i = 0; i < this.entryIcons.Count; i++)
		{
			UnityEngine.Object.Destroy(this.entryIcons[i]);
		}
	}

	// Token: 0x04003F96 RID: 16278
	[Header("UI References")]
	[SerializeField]
	private GameObject contentBody;

	// Token: 0x04003F97 RID: 16279
	[SerializeField]
	private LocText characterName;

	// Token: 0x04003F98 RID: 16280
	public GameObject selectedBorder;

	// Token: 0x04003F99 RID: 16281
	[SerializeField]
	private Image titleBar;

	// Token: 0x04003F9A RID: 16282
	[SerializeField]
	private Color selectedTitleColor;

	// Token: 0x04003F9B RID: 16283
	[SerializeField]
	private Color deselectedTitleColor;

	// Token: 0x04003F9C RID: 16284
	[SerializeField]
	private KButton reshuffleButton;

	// Token: 0x04003F9D RID: 16285
	private KBatchedAnimController animController;

	// Token: 0x04003F9E RID: 16286
	[SerializeField]
	private LocText itemName;

	// Token: 0x04003F9F RID: 16287
	[SerializeField]
	private LocText quantity;

	// Token: 0x04003FA0 RID: 16288
	[SerializeField]
	private LocText currentQuantity;

	// Token: 0x04003FA1 RID: 16289
	[SerializeField]
	private LocText description;

	// Token: 0x04003FA2 RID: 16290
	[SerializeField]
	private LocText effects;

	// Token: 0x04003FA3 RID: 16291
	[SerializeField]
	private KToggle selectButton;

	// Token: 0x04003FA4 RID: 16292
	private CarePackageInfo info;

	// Token: 0x04003FA5 RID: 16293
	public CarePackageContainer.CarePackageInstanceData carePackageInstanceData;

	// Token: 0x04003FA6 RID: 16294
	private CharacterSelectionController controller;

	// Token: 0x04003FA7 RID: 16295
	private static List<ITelepadDeliverableContainer> containers;

	// Token: 0x04003FA8 RID: 16296
	[SerializeField]
	private Sprite enabledSpr;

	// Token: 0x04003FA9 RID: 16297
	[SerializeField]
	private List<CarePackageContainer.ProfessionIcon> professionIcons;

	// Token: 0x04003FAA RID: 16298
	private Dictionary<string, Sprite> professionIconMap;

	// Token: 0x04003FAB RID: 16299
	public float baseCharacterScale = 0.38f;

	// Token: 0x04003FAC RID: 16300
	private List<GameObject> entryIcons = new List<GameObject>();

	// Token: 0x02001DF0 RID: 7664
	[Serializable]
	public struct ProfessionIcon
	{
		// Token: 0x04008CB5 RID: 36021
		public string professionName;

		// Token: 0x04008CB6 RID: 36022
		public Sprite iconImg;
	}

	// Token: 0x02001DF1 RID: 7665
	public class CarePackageInstanceData : ITelepadDeliverable
	{
		// Token: 0x0600B294 RID: 45716 RVA: 0x003E085E File Offset: 0x003DEA5E
		public GameObject Deliver(Vector3 position)
		{
			GameObject gameObject = this.info.Deliver(position);
			gameObject.GetComponent<CarePackage>().SetFacade(this.facadeID);
			return gameObject;
		}

		// Token: 0x04008CB7 RID: 36023
		public CarePackageInfo info;

		// Token: 0x04008CB8 RID: 36024
		public string facadeID;
	}
}
