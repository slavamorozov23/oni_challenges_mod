using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DB8 RID: 3512
public class NameDisplayScreen : KScreen
{
	// Token: 0x06006D96 RID: 28054 RVA: 0x00298443 File Offset: 0x00296643
	public static void DestroyInstance()
	{
		NameDisplayScreen.Instance = null;
	}

	// Token: 0x06006D97 RID: 28055 RVA: 0x0029844B File Offset: 0x0029664B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NameDisplayScreen.Instance = this;
	}

	// Token: 0x06006D98 RID: 28056 RVA: 0x00298459 File Offset: 0x00296659
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Health.Register(new Action<Health>(this.OnHealthAdded), null);
		Components.Equipment.Register(new Action<Equipment>(this.OnEquipmentAdded), null);
		this.BindOnOverlayChange();
	}

	// Token: 0x06006D99 RID: 28057 RVA: 0x00298498 File Offset: 0x00296698
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isOverlayChangeBound && OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.isOverlayChangeBound = false;
		}
	}

	// Token: 0x06006D9A RID: 28058 RVA: 0x002984F0 File Offset: 0x002966F0
	private void BindOnOverlayChange()
	{
		if (this.isOverlayChangeBound)
		{
			return;
		}
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.isOverlayChangeBound = true;
		}
	}

	// Token: 0x06006D9B RID: 28059 RVA: 0x00298540 File Offset: 0x00296740
	public void RemoveWorldEntries(int worldId)
	{
		this.entries.RemoveAll((NameDisplayScreen.Entry entry) => entry.world_go.IsNullOrDestroyed() || entry.world_go.GetMyWorldId() == worldId);
	}

	// Token: 0x06006D9C RID: 28060 RVA: 0x00298572 File Offset: 0x00296772
	private void OnOverlayChanged(HashedString new_mode)
	{
		HashedString hashedString = this.lastKnownOverlayID;
		this.lastKnownOverlayID = new_mode;
		this.nameDisplayCanvas.enabled = (this.lastKnownOverlayID == OverlayModes.None.ID);
	}

	// Token: 0x06006D9D RID: 28061 RVA: 0x0029859D File Offset: 0x0029679D
	private void OnHealthAdded(Health health)
	{
		this.RegisterComponent(health.gameObject, health, false);
	}

	// Token: 0x06006D9E RID: 28062 RVA: 0x002985B0 File Offset: 0x002967B0
	private void OnEquipmentAdded(Equipment equipment)
	{
		MinionAssignablesProxy component = equipment.GetComponent<MinionAssignablesProxy>();
		GameObject targetGameObject = component.GetTargetGameObject();
		if (targetGameObject)
		{
			this.RegisterComponent(targetGameObject, equipment, false);
			return;
		}
		global::Debug.LogWarningFormat("OnEquipmentAdded proxy target {0} was null.", new object[]
		{
			component.TargetInstanceID
		});
	}

	// Token: 0x06006D9F RID: 28063 RVA: 0x002985FC File Offset: 0x002967FC
	private bool ShouldShowName(GameObject representedObject)
	{
		CharacterOverlay component = representedObject.GetComponent<CharacterOverlay>();
		return component != null && component.shouldShowName;
	}

	// Token: 0x06006DA0 RID: 28064 RVA: 0x00298624 File Offset: 0x00296824
	public Guid AddAreaText(string initialText, GameObject prefab)
	{
		NameDisplayScreen.TextEntry textEntry = new NameDisplayScreen.TextEntry();
		textEntry.guid = Guid.NewGuid();
		textEntry.display_go = Util.KInstantiateUI(prefab, this.areaTextDisplayCanvas.gameObject, true);
		textEntry.display_go.GetComponentInChildren<LocText>().text = initialText;
		this.textEntries.Add(textEntry);
		return textEntry.guid;
	}

	// Token: 0x06006DA1 RID: 28065 RVA: 0x00298680 File Offset: 0x00296880
	public GameObject GetWorldText(Guid guid)
	{
		GameObject result = null;
		foreach (NameDisplayScreen.TextEntry textEntry in this.textEntries)
		{
			if (textEntry.guid == guid)
			{
				result = textEntry.display_go;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006DA2 RID: 28066 RVA: 0x002986E8 File Offset: 0x002968E8
	public void RemoveWorldText(Guid guid)
	{
		int num = -1;
		for (int i = 0; i < this.textEntries.Count; i++)
		{
			if (this.textEntries[i].guid == guid)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			UnityEngine.Object.Destroy(this.textEntries[num].display_go);
			this.textEntries.RemoveAt(num);
		}
	}

	// Token: 0x06006DA3 RID: 28067 RVA: 0x00298750 File Offset: 0x00296950
	public void AddNewEntry(GameObject representedObject)
	{
		NameDisplayScreen.Entry entry = new NameDisplayScreen.Entry();
		entry.world_go = representedObject;
		entry.world_go_anim_controller = representedObject.GetComponent<KAnimControllerBase>();
		GameObject original = this.ShouldShowName(representedObject) ? this.nameAndBarsPrefab : this.barsPrefab;
		entry.kprfabID = representedObject.GetComponent<KPrefabID>();
		entry.collider = representedObject.GetComponent<KBoxCollider2D>();
		GameObject gameObject = Util.KInstantiateUI(original, this.nameDisplayCanvas.gameObject, true);
		entry.display_go = gameObject;
		entry.display_go_rect = gameObject.GetComponent<RectTransform>();
		entry.nameLabel = entry.display_go.GetComponentInChildren<LocText>();
		entry.display_go.SetActive(false);
		if (this.worldSpace)
		{
			entry.display_go.transform.localScale = Vector3.one * 0.01f;
		}
		gameObject.name = representedObject.name + " character overlay";
		entry.Name = representedObject.name;
		entry.refs = gameObject.GetComponent<HierarchyReferences>();
		this.entries.Add(entry);
		UnityEngine.Object component = representedObject.GetComponent<KSelectable>();
		FactionAlignment component2 = representedObject.GetComponent<FactionAlignment>();
		if (component != null)
		{
			if (component2 != null)
			{
				if (component2.Alignment == FactionManager.FactionID.Friendly || component2.Alignment == FactionManager.FactionID.Duplicant)
				{
					this.UpdateName(representedObject);
					return;
				}
			}
			else
			{
				this.UpdateName(representedObject);
			}
		}
	}

	// Token: 0x06006DA4 RID: 28068 RVA: 0x00298888 File Offset: 0x00296A88
	public void RegisterComponent(GameObject representedObject, object component, bool force_new_entry = false)
	{
		NameDisplayScreen.Entry entry = force_new_entry ? null : this.GetEntry(representedObject);
		if (entry == null)
		{
			CharacterOverlay component2 = representedObject.GetComponent<CharacterOverlay>();
			if (component2 != null)
			{
				component2.Register();
				entry = this.GetEntry(representedObject);
			}
		}
		if (entry == null)
		{
			return;
		}
		Transform reference = entry.refs.GetReference<Transform>("Bars");
		entry.bars_go = reference.gameObject;
		if (component is Health)
		{
			if (!entry.healthBar)
			{
				Health health = (Health)component;
				GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.healthBarPrefab, reference.gameObject, false);
				gameObject.name = "Health Bar";
				health.healthBar = gameObject.GetComponent<HealthBar>();
				health.healthBar.GetComponent<KSelectable>().entityName = UI.METERS.HEALTH.TOOLTIP;
				health.healthBar.GetComponent<KSelectableHealthBar>().IsSelectable = (representedObject.GetComponent<MinionBrain>() != null);
				entry.healthBar = health.healthBar;
				entry.healthBar.autoHide = false;
				gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
				return;
			}
			global::Debug.LogWarningFormat("Health added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is OxygenBreather)
		{
			if (!entry.breathBar)
			{
				GameObject gameObject2 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.breathBar = gameObject2.GetComponent<ProgressBar>();
				entry.breathBar.autoHide = false;
				gameObject2.gameObject.GetComponent<ToolTip>().AddMultiStringTooltip("Breath", this.ToolTipStyle_Property);
				gameObject2.name = "Breath Bar";
				gameObject2.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BreathBar");
				gameObject2.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("OxygenBreather added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is BionicOxygenTankMonitor.Instance)
		{
			if (!entry.bionicOxygenTankBar)
			{
				GameObject gameObject3 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.bionicOxygenTankBar = gameObject3.GetComponent<ProgressBar>();
				entry.bionicOxygenTankBar.autoHide = false;
				gameObject3.name = "Bionic Oxygen Tank Bar";
				gameObject3.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");
				gameObject3.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("BionicOxygenTankBar added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is Equipment)
		{
			if (!entry.suitBar)
			{
				GameObject gameObject4 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitBar = gameObject4.GetComponent<ProgressBar>();
				entry.suitBar.autoHide = false;
				gameObject4.name = "Suit Tank Bar";
				gameObject4.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");
				gameObject4.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
			}
			else
			{
				global::Debug.LogWarningFormat("SuitBar added twice {0}", new object[]
				{
					component
				});
			}
			if (!entry.suitFuelBar)
			{
				GameObject gameObject5 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitFuelBar = gameObject5.GetComponent<ProgressBar>();
				entry.suitFuelBar.autoHide = false;
				gameObject5.name = "Suit Fuel Bar";
				gameObject5.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("FuelTankBar");
				gameObject5.GetComponent<KSelectable>().entityName = UI.METERS.FUEL.TOOLTIP;
			}
			else
			{
				global::Debug.LogWarningFormat("FuelBar added twice {0}", new object[]
				{
					component
				});
			}
			if (!entry.suitBatteryBar)
			{
				GameObject gameObject6 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitBatteryBar = gameObject6.GetComponent<ProgressBar>();
				entry.suitBatteryBar.autoHide = false;
				gameObject6.name = "Suit Battery Bar";
				gameObject6.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BatteryBar");
				gameObject6.GetComponent<KSelectable>().entityName = UI.METERS.BATTERY.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("CoolantBar added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is ThoughtGraph.Instance || component is CreatureThoughtGraph.Instance)
		{
			if (!entry.thoughtBubble)
			{
				GameObject gameObject7 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubble, entry.display_go, false);
				entry.thoughtBubble = gameObject7.GetComponent<HierarchyReferences>();
				gameObject7.name = ((component is CreatureThoughtGraph.Instance) ? "Creature " : "") + "Thought Bubble";
				GameObject gameObject8 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubbleConvo, entry.display_go, false);
				entry.thoughtBubbleConvo = gameObject8.GetComponent<HierarchyReferences>();
				gameObject8.name = ((component is CreatureThoughtGraph.Instance) ? "Creature " : "") + "Thought Bubble Convo";
				return;
			}
			global::Debug.LogWarningFormat("ThoughtGraph added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else
		{
			if (!(component is GameplayEventMonitor.Instance))
			{
				if (component is Dreamer.Instance && !entry.dreamBubble)
				{
					GameObject gameObject9 = Util.KInstantiateUI(EffectPrefabs.Instance.DreamBubble, entry.display_go, false);
					gameObject9.name = "Dream Bubble";
					entry.dreamBubble = gameObject9.GetComponent<DreamBubble>();
				}
				return;
			}
			if (!entry.gameplayEventDisplay)
			{
				GameObject gameObject10 = Util.KInstantiateUI(EffectPrefabs.Instance.GameplayEventDisplay, entry.display_go, false);
				entry.gameplayEventDisplay = gameObject10.GetComponent<HierarchyReferences>();
				gameObject10.name = "Gameplay Event Display";
				return;
			}
			global::Debug.LogWarningFormat("GameplayEventDisplay added twice {0}", new object[]
			{
				component
			});
			return;
		}
	}

	// Token: 0x06006DA5 RID: 28069 RVA: 0x00298E95 File Offset: 0x00297095
	public bool IsVisibleToZoom()
	{
		return !(Game.MainCamera == null) && Game.MainCamera.orthographicSize < this.HideDistance;
	}

	// Token: 0x06006DA6 RID: 28070 RVA: 0x00298EB8 File Offset: 0x002970B8
	private void LateUpdate()
	{
		if (App.isLoading || App.IsExiting)
		{
			return;
		}
		this.BindOnOverlayChange();
		if (Game.MainCamera == null)
		{
			return;
		}
		if (this.lastKnownOverlayID != OverlayModes.None.ID)
		{
			return;
		}
		int count = this.entries.Count;
		bool flag = this.IsVisibleToZoom();
		bool flag2 = flag && this.lastKnownOverlayID == OverlayModes.None.ID;
		if (this.nameDisplayCanvas.enabled != flag2)
		{
			this.nameDisplayCanvas.enabled = flag2;
		}
		if (flag)
		{
			this.RemoveDestroyedEntries();
			this.Culling();
			this.UpdatePos();
			this.HideDeadProgressBars();
		}
	}

	// Token: 0x06006DA7 RID: 28071 RVA: 0x00298F58 File Offset: 0x00297158
	private void Culling()
	{
		if (this.entries.Count == 0)
		{
			return;
		}
		Vector2I vector2I;
		Vector2I vector2I2;
		Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2, 4, 1.5f);
		int num = Mathf.Min(500, this.entries.Count);
		for (int i = 0; i < num; i++)
		{
			int index = (this.currentUpdateIndex + i) % this.entries.Count;
			NameDisplayScreen.Entry entry = this.entries[index];
			Vector3 position = entry.world_go.transform.GetPosition();
			bool flag = position.x >= (float)vector2I.x && position.y >= (float)vector2I.y && position.x < (float)vector2I2.x && position.y < (float)vector2I2.y;
			if (entry.visible != flag)
			{
				entry.display_go.SetActive(flag);
			}
			entry.visible = flag;
		}
		this.currentUpdateIndex = (this.currentUpdateIndex + num) % this.entries.Count;
	}

	// Token: 0x06006DA8 RID: 28072 RVA: 0x00299064 File Offset: 0x00297264
	private void UpdatePos()
	{
		CameraController instance = CameraController.Instance;
		Transform followTarget = instance.followTarget;
		int count = this.entries.Count;
		for (int i = 0; i < count; i++)
		{
			NameDisplayScreen.Entry entry = this.entries[i];
			if (entry.visible)
			{
				GameObject world_go = entry.world_go;
				if (!(world_go == null))
				{
					Vector3 vector = world_go.transform.GetPosition();
					if (followTarget == world_go.transform)
					{
						vector = instance.followTargetPos;
					}
					else if (entry.world_go_anim_controller != null && entry.collider != null)
					{
						vector.x += entry.collider.offset.x;
						vector.y += entry.collider.offset.y - entry.collider.size.y / 2f;
					}
					entry.display_go_rect.anchoredPosition = (this.worldSpace ? vector : base.WorldToScreen(vector));
				}
			}
		}
	}

	// Token: 0x06006DA9 RID: 28073 RVA: 0x00299188 File Offset: 0x00297388
	private void RemoveDestroyedEntries()
	{
		int num = this.entries.Count;
		int i = 0;
		while (i < num)
		{
			if (this.entries[i].world_go == null)
			{
				UnityEngine.Object.Destroy(this.entries[i].display_go);
				num--;
				this.entries[i] = this.entries[num];
			}
			else
			{
				i++;
			}
		}
		this.entries.RemoveRange(num, this.entries.Count - num);
	}

	// Token: 0x06006DAA RID: 28074 RVA: 0x00299214 File Offset: 0x00297414
	private void HideDeadProgressBars()
	{
		int count = this.entries.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.entries[i].visible && !(this.entries[i].world_go == null) && this.entries[i].kprfabID.HasTag(GameTags.Dead) && this.entries[i].bars_go.activeSelf)
			{
				this.entries[i].bars_go.SetActive(false);
			}
		}
	}

	// Token: 0x06006DAB RID: 28075 RVA: 0x002992B4 File Offset: 0x002974B4
	public void UpdateName(GameObject representedObject)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		KSelectable component = representedObject.GetComponent<KSelectable>();
		entry.display_go.name = component.GetProperName() + " character overlay";
		if (entry.nameLabel != null)
		{
			entry.nameLabel.text = component.GetProperName();
			if (representedObject.GetComponent<RocketModule>() != null)
			{
				entry.nameLabel.text = representedObject.GetComponent<RocketModule>().GetParentRocketName();
			}
		}
	}

	// Token: 0x06006DAC RID: 28076 RVA: 0x00299334 File Offset: 0x00297534
	public void SetDream(GameObject minion_go, Dream dream)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.SetDream(dream);
		entry.dreamBubble.GetComponent<KSelectable>().entityName = "Dreaming";
		entry.dreamBubble.gameObject.SetActive(true);
		entry.dreamBubble.SetVisibility(true);
	}

	// Token: 0x06006DAD RID: 28077 RVA: 0x0029939C File Offset: 0x0029759C
	public void StopDreaming(GameObject minion_go)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.StopDreaming();
		entry.dreamBubble.gameObject.SetActive(false);
	}

	// Token: 0x06006DAE RID: 28078 RVA: 0x002993E0 File Offset: 0x002975E0
	public void DreamTick(GameObject minion_go, float dt)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.Tick(dt);
	}

	// Token: 0x06006DAF RID: 28079 RVA: 0x00299414 File Offset: 0x00297614
	public void SetThoughtBubbleDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.thoughtBubble == null)
		{
			return;
		}
		this.ApplyThoughtSprite(entry.thoughtBubble, bubble_sprite, "bubble_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubble, topic_sprite, "icon_sprite");
		entry.thoughtBubble.GetComponent<KSelectable>().entityName = hover_text;
		entry.thoughtBubble.gameObject.SetActive(bVisible);
	}

	// Token: 0x06006DB0 RID: 28080 RVA: 0x00299484 File Offset: 0x00297684
	public void SetThoughtBubbleConvoDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite, Sprite mode_sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.thoughtBubble == null)
		{
			return;
		}
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, bubble_sprite, "bubble_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, topic_sprite, "icon_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, mode_sprite, "icon_sprite_mode");
		entry.thoughtBubbleConvo.GetComponent<KSelectable>().entityName = hover_text;
		entry.thoughtBubbleConvo.gameObject.SetActive(bVisible);
	}

	// Token: 0x06006DB1 RID: 28081 RVA: 0x00299506 File Offset: 0x00297706
	private void ApplyThoughtSprite(HierarchyReferences active_bubble, Sprite sprite, string target)
	{
		active_bubble.GetReference<Image>(target).sprite = sprite;
	}

	// Token: 0x06006DB2 RID: 28082 RVA: 0x00299518 File Offset: 0x00297718
	public void SetGameplayEventDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.gameplayEventDisplay == null)
		{
			return;
		}
		entry.gameplayEventDisplay.GetReference<Image>("icon_sprite").sprite = sprite;
		entry.gameplayEventDisplay.GetComponent<KSelectable>().entityName = hover_text;
		entry.gameplayEventDisplay.gameObject.SetActive(bVisible);
	}

	// Token: 0x06006DB3 RID: 28083 RVA: 0x00299578 File Offset: 0x00297778
	public void SetBreathDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.breathBar == null)
		{
			return;
		}
		entry.breathBar.SetUpdateFunc(updatePercentFull);
		entry.breathBar.SetVisibility(bVisible);
	}

	// Token: 0x06006DB4 RID: 28084 RVA: 0x002995B8 File Offset: 0x002977B8
	public void SetHealthDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.healthBar == null)
		{
			return;
		}
		entry.healthBar.OnChange();
		entry.healthBar.SetUpdateFunc(updatePercentFull);
		if (entry.healthBar.gameObject.activeSelf != bVisible)
		{
			entry.healthBar.SetVisibility(bVisible);
		}
	}

	// Token: 0x06006DB5 RID: 28085 RVA: 0x00299618 File Offset: 0x00297818
	public void SetSuitTankDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitBar == null)
		{
			return;
		}
		entry.suitBar.SetUpdateFunc(updatePercentFull);
		entry.suitBar.SetVisibility(bVisible);
	}

	// Token: 0x06006DB6 RID: 28086 RVA: 0x00299658 File Offset: 0x00297858
	public void SetBionicOxygenTankDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.bionicOxygenTankBar == null)
		{
			return;
		}
		entry.bionicOxygenTankBar.SetUpdateFunc(updatePercentFull);
		entry.bionicOxygenTankBar.SetVisibility(bVisible);
	}

	// Token: 0x06006DB7 RID: 28087 RVA: 0x00299698 File Offset: 0x00297898
	public void SetSuitFuelDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitFuelBar == null)
		{
			return;
		}
		entry.suitFuelBar.SetUpdateFunc(updatePercentFull);
		entry.suitFuelBar.SetVisibility(bVisible);
	}

	// Token: 0x06006DB8 RID: 28088 RVA: 0x002996D8 File Offset: 0x002978D8
	public void SetSuitBatteryDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitBatteryBar == null)
		{
			return;
		}
		entry.suitBatteryBar.SetUpdateFunc(updatePercentFull);
		entry.suitBatteryBar.SetVisibility(bVisible);
	}

	// Token: 0x06006DB9 RID: 28089 RVA: 0x00299718 File Offset: 0x00297918
	private NameDisplayScreen.Entry GetEntry(GameObject worldObject)
	{
		return this.entries.Find((NameDisplayScreen.Entry entry) => entry.world_go == worldObject);
	}

	// Token: 0x04004ADF RID: 19167
	[SerializeField]
	private float HideDistance;

	// Token: 0x04004AE0 RID: 19168
	public static NameDisplayScreen Instance;

	// Token: 0x04004AE1 RID: 19169
	[SerializeField]
	private Canvas nameDisplayCanvas;

	// Token: 0x04004AE2 RID: 19170
	[SerializeField]
	private Canvas areaTextDisplayCanvas;

	// Token: 0x04004AE3 RID: 19171
	public GameObject nameAndBarsPrefab;

	// Token: 0x04004AE4 RID: 19172
	public GameObject barsPrefab;

	// Token: 0x04004AE5 RID: 19173
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04004AE6 RID: 19174
	[SerializeField]
	private Color selectedColor;

	// Token: 0x04004AE7 RID: 19175
	[SerializeField]
	private Color defaultColor;

	// Token: 0x04004AE8 RID: 19176
	public int fontsize_min = 14;

	// Token: 0x04004AE9 RID: 19177
	public int fontsize_max = 32;

	// Token: 0x04004AEA RID: 19178
	public float cameraDistance_fontsize_min = 6f;

	// Token: 0x04004AEB RID: 19179
	public float cameraDistance_fontsize_max = 4f;

	// Token: 0x04004AEC RID: 19180
	public List<NameDisplayScreen.Entry> entries = new List<NameDisplayScreen.Entry>();

	// Token: 0x04004AED RID: 19181
	public List<NameDisplayScreen.TextEntry> textEntries = new List<NameDisplayScreen.TextEntry>();

	// Token: 0x04004AEE RID: 19182
	public bool worldSpace = true;

	// Token: 0x04004AEF RID: 19183
	private bool isOverlayChangeBound;

	// Token: 0x04004AF0 RID: 19184
	private HashedString lastKnownOverlayID = OverlayModes.None.ID;

	// Token: 0x04004AF1 RID: 19185
	private int currentUpdateIndex;

	// Token: 0x0200200A RID: 8202
	[Serializable]
	public class Entry
	{
		// Token: 0x0400949B RID: 38043
		public string Name;

		// Token: 0x0400949C RID: 38044
		public bool visible;

		// Token: 0x0400949D RID: 38045
		public GameObject world_go;

		// Token: 0x0400949E RID: 38046
		public GameObject display_go;

		// Token: 0x0400949F RID: 38047
		public GameObject bars_go;

		// Token: 0x040094A0 RID: 38048
		public KPrefabID kprfabID;

		// Token: 0x040094A1 RID: 38049
		public KBoxCollider2D collider;

		// Token: 0x040094A2 RID: 38050
		public KAnimControllerBase world_go_anim_controller;

		// Token: 0x040094A3 RID: 38051
		public RectTransform display_go_rect;

		// Token: 0x040094A4 RID: 38052
		public LocText nameLabel;

		// Token: 0x040094A5 RID: 38053
		public HealthBar healthBar;

		// Token: 0x040094A6 RID: 38054
		public ProgressBar breathBar;

		// Token: 0x040094A7 RID: 38055
		public ProgressBar suitBar;

		// Token: 0x040094A8 RID: 38056
		public ProgressBar bionicOxygenTankBar;

		// Token: 0x040094A9 RID: 38057
		public ProgressBar suitFuelBar;

		// Token: 0x040094AA RID: 38058
		public ProgressBar suitBatteryBar;

		// Token: 0x040094AB RID: 38059
		public DreamBubble dreamBubble;

		// Token: 0x040094AC RID: 38060
		public HierarchyReferences thoughtBubble;

		// Token: 0x040094AD RID: 38061
		public HierarchyReferences thoughtBubbleConvo;

		// Token: 0x040094AE RID: 38062
		public HierarchyReferences gameplayEventDisplay;

		// Token: 0x040094AF RID: 38063
		public HierarchyReferences refs;
	}

	// Token: 0x0200200B RID: 8203
	public class TextEntry
	{
		// Token: 0x040094B0 RID: 38064
		public Guid guid;

		// Token: 0x040094B1 RID: 38065
		public GameObject display_go;
	}
}
