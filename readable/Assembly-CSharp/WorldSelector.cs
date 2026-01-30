using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ED6 RID: 3798
public class WorldSelector : KScreen, ISim4000ms
{
	// Token: 0x06007984 RID: 31108 RVA: 0x002EB4FC File Offset: 0x002E96FC
	public static void DestroyInstance()
	{
		WorldSelector.Instance = null;
	}

	// Token: 0x06007985 RID: 31109 RVA: 0x002EB504 File Offset: 0x002E9704
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		WorldSelector.Instance = this;
	}

	// Token: 0x06007986 RID: 31110 RVA: 0x002EB514 File Offset: 0x002E9714
	protected override void OnSpawn()
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.Deactivate();
			return;
		}
		base.OnSpawn();
		this.worldRows = new Dictionary<int, MultiToggle>();
		this.SpawnToggles();
		this.RefreshToggles();
		Game.Instance.Subscribe(1983128072, delegate(object data)
		{
			this.RefreshToggles();
		});
		Game.Instance.Subscribe(-521212405, delegate(object data)
		{
			this.RefreshToggles();
		});
		Game.Instance.Subscribe(880851192, delegate(object data)
		{
			this.SortRows();
		});
		ClusterManager.Instance.Subscribe(-1280433810, delegate(object data)
		{
			this.AddWorld(data);
		});
		ClusterManager.Instance.Subscribe(-1078710002, delegate(object data)
		{
			this.RemoveWorld(data);
		});
		ClusterManager.Instance.Subscribe(1943181844, delegate(object data)
		{
			this.RefreshToggles();
		});
	}

	// Token: 0x06007987 RID: 31111 RVA: 0x002EB5F4 File Offset: 0x002E97F4
	private void SpawnToggles()
	{
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.worldRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.worldRows.Clear();
		foreach (int num in ClusterManager.Instance.GetWorldIDsSorted())
		{
			MultiToggle component = Util.KInstantiateUI(this.worldRowPrefab, this.worldRowContainer, false).GetComponent<MultiToggle>();
			this.worldRows.Add(num, component);
			this.previousWorldDiagnosticStatus.Add(num, ColonyDiagnostic.DiagnosticResult.Opinion.Normal);
			int id = num;
			MultiToggle multiToggle = component;
			multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
			{
				this.OnWorldRowClicked(id);
			}));
			component.GetComponentInChildren<AlertVignette>().worldID = num;
		}
	}

	// Token: 0x06007988 RID: 31112 RVA: 0x002EB718 File Offset: 0x002E9918
	private void AddWorld(object data)
	{
		int value = ((Boxed<int>)data).value;
		MultiToggle component = Util.KInstantiateUI(this.worldRowPrefab, this.worldRowContainer, false).GetComponent<MultiToggle>();
		this.worldRows.Add(value, component);
		this.previousWorldDiagnosticStatus.Add(value, ColonyDiagnostic.DiagnosticResult.Opinion.Normal);
		int id = value;
		MultiToggle multiToggle = component;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.OnWorldRowClicked(id);
		}));
		component.GetComponentInChildren<AlertVignette>().worldID = value;
		this.RefreshToggles();
	}

	// Token: 0x06007989 RID: 31113 RVA: 0x002EB7AC File Offset: 0x002E99AC
	private void RemoveWorld(object data)
	{
		int value = ((Boxed<int>)data).value;
		MultiToggle cmp;
		if (this.worldRows.TryGetValue(value, out cmp))
		{
			cmp.DeleteObject();
		}
		this.worldRows.Remove(value);
		this.previousWorldDiagnosticStatus.Remove(value);
		this.RefreshToggles();
	}

	// Token: 0x0600798A RID: 31114 RVA: 0x002EB7FC File Offset: 0x002E99FC
	public void OnWorldRowClicked(int id)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(id);
		if (world != null && world.IsDiscovered)
		{
			CameraController.Instance.ActiveWorldStarWipe(id, null);
		}
	}

	// Token: 0x0600798B RID: 31115 RVA: 0x002EB834 File Offset: 0x002E9A34
	private void RefreshToggles()
	{
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.worldRows)
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(keyValuePair.Key);
			ClusterGridEntity component = world.GetComponent<ClusterGridEntity>();
			HierarchyReferences component2 = keyValuePair.Value.GetComponent<HierarchyReferences>();
			if (world != null)
			{
				component2.GetReference<Image>("Icon").sprite = component.GetUISprite();
				component2.GetReference<LocText>("Label").SetText(world.GetComponent<ClusterGridEntity>().Name);
			}
			else
			{
				component2.GetReference<Image>("Icon").sprite = Assets.GetSprite("unknown_far");
			}
			if (keyValuePair.Key == CameraController.Instance.cameraActiveCluster)
			{
				keyValuePair.Value.ChangeState(1);
				keyValuePair.Value.gameObject.SetActive(true);
			}
			else if (world != null && world.IsDiscovered)
			{
				keyValuePair.Value.ChangeState(0);
				keyValuePair.Value.gameObject.SetActive(true);
			}
			else
			{
				keyValuePair.Value.ChangeState(0);
				keyValuePair.Value.gameObject.SetActive(false);
			}
			this.RefreshToggleTooltips();
			keyValuePair.Value.GetComponentInChildren<AlertVignette>().worldID = keyValuePair.Key;
		}
		this.RefreshWorldStatus();
		this.SortRows();
	}

	// Token: 0x0600798C RID: 31116 RVA: 0x002EB9C8 File Offset: 0x002E9BC8
	private void RefreshWorldStatus()
	{
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.worldRows)
		{
			if (!this.worldStatusIcons.ContainsKey(keyValuePair.Key))
			{
				this.worldStatusIcons.Add(keyValuePair.Key, new List<GameObject>());
			}
			foreach (GameObject original in this.worldStatusIcons[keyValuePair.Key])
			{
				Util.KDestroyGameObject(original);
			}
			LocText reference = keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("StatusLabel");
			reference.SetText(ClusterManager.Instance.GetWorld(keyValuePair.Key).GetStatus());
			reference.color = ColonyDiagnosticScreen.GetDiagnosticIndicationColor(ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResult(keyValuePair.Key));
		}
	}

	// Token: 0x0600798D RID: 31117 RVA: 0x002EBAE0 File Offset: 0x002E9CE0
	private void RefreshToggleTooltips()
	{
		int num = 0;
		List<int> discoveredAsteroidIDsSorted = ClusterManager.Instance.GetDiscoveredAsteroidIDsSorted();
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.worldRows)
		{
			ClusterGridEntity component = ClusterManager.Instance.GetWorld(keyValuePair.Key).GetComponent<ClusterGridEntity>();
			ToolTip component2 = keyValuePair.Value.GetComponent<ToolTip>();
			component2.ClearMultiStringTooltip();
			WorldContainer world = ClusterManager.Instance.GetWorld(keyValuePair.Key);
			if (world != null)
			{
				component2.AddMultiStringTooltip(component.Name, this.titleTextSetting);
				if (!world.IsModuleInterior)
				{
					int num2 = discoveredAsteroidIDsSorted.IndexOf(world.id);
					if (num2 != -1 && num2 <= 9)
					{
						component2.AddMultiStringTooltip(" ", this.bodyTextSetting);
						if (KInputManager.currentControllerIsGamepad)
						{
							component2.AddMultiStringTooltip(UI.FormatAsHotkey(GameUtil.GetActionString(this.IdxToHotkeyAction(num2))), this.bodyTextSetting);
						}
						else
						{
							component2.AddMultiStringTooltip(UI.FormatAsHotkey("[" + GameUtil.GetActionString(this.IdxToHotkeyAction(num2)) + "]"), this.bodyTextSetting);
						}
					}
				}
			}
			else
			{
				component2.AddMultiStringTooltip(UI.CLUSTERMAP.UNKNOWN_DESTINATION, this.titleTextSetting);
			}
			if (ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResult(world.id) < ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
			{
				component2.AddMultiStringTooltip(ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResultTooltip(world.id), this.bodyTextSetting);
			}
			num++;
		}
	}

	// Token: 0x0600798E RID: 31118 RVA: 0x002EBC90 File Offset: 0x002E9E90
	private void SortRows()
	{
		List<KeyValuePair<int, MultiToggle>> list = this.worldRows.ToList<KeyValuePair<int, MultiToggle>>();
		list.Sort(delegate(KeyValuePair<int, MultiToggle> x, KeyValuePair<int, MultiToggle> y)
		{
			float num = ClusterManager.Instance.GetWorld(x.Key).IsModuleInterior ? float.PositiveInfinity : ClusterManager.Instance.GetWorld(x.Key).DiscoveryTimestamp;
			float value = ClusterManager.Instance.GetWorld(y.Key).IsModuleInterior ? float.PositiveInfinity : ClusterManager.Instance.GetWorld(y.Key).DiscoveryTimestamp;
			return num.CompareTo(value);
		});
		for (int i = 0; i < list.Count; i++)
		{
			list[i].Value.transform.SetSiblingIndex(i);
		}
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in list)
		{
			keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Indent").anchoredPosition = Vector2.zero;
			keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Status").anchoredPosition = Vector2.right * 24f;
			WorldContainer world = ClusterManager.Instance.GetWorld(keyValuePair.Key);
			if (world.ParentWorldId != world.id && world.ParentWorldId != 255)
			{
				foreach (KeyValuePair<int, MultiToggle> keyValuePair2 in list)
				{
					if (keyValuePair2.Key == world.ParentWorldId)
					{
						int siblingIndex = keyValuePair2.Value.gameObject.transform.GetSiblingIndex();
						keyValuePair.Value.gameObject.transform.SetSiblingIndex(siblingIndex + 1);
						keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Indent").anchoredPosition = Vector2.right * 32f;
						keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Status").anchoredPosition = Vector2.right * -8f;
						break;
					}
				}
			}
		}
	}

	// Token: 0x0600798F RID: 31119 RVA: 0x002EBEB0 File Offset: 0x002EA0B0
	private global::Action IdxToHotkeyAction(int idx)
	{
		global::Action result;
		switch (idx)
		{
		case 0:
			result = global::Action.SwitchActiveWorld1;
			break;
		case 1:
			result = global::Action.SwitchActiveWorld2;
			break;
		case 2:
			result = global::Action.SwitchActiveWorld3;
			break;
		case 3:
			result = global::Action.SwitchActiveWorld4;
			break;
		case 4:
			result = global::Action.SwitchActiveWorld5;
			break;
		case 5:
			result = global::Action.SwitchActiveWorld6;
			break;
		case 6:
			result = global::Action.SwitchActiveWorld7;
			break;
		case 7:
			result = global::Action.SwitchActiveWorld8;
			break;
		case 8:
			result = global::Action.SwitchActiveWorld9;
			break;
		case 9:
			result = global::Action.SwitchActiveWorld10;
			break;
		default:
			global::Debug.LogError("Action must be a SwitchActiveWorld Action");
			result = global::Action.SwitchActiveWorld1;
			break;
		}
		return result;
	}

	// Token: 0x06007990 RID: 31120 RVA: 0x002EBF50 File Offset: 0x002EA150
	public void Sim4000ms(float dt)
	{
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.worldRows)
		{
			ColonyDiagnostic.DiagnosticResult.Opinion worldDiagnosticResult = ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResult(keyValuePair.Key);
			ColonyDiagnosticScreen.SetIndication(worldDiagnosticResult, keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference("Indicator").gameObject);
			if (this.previousWorldDiagnosticStatus[keyValuePair.Key] > worldDiagnosticResult && ClusterManager.Instance.activeWorldId != keyValuePair.Key)
			{
				this.TriggerVisualNotification(keyValuePair.Key, worldDiagnosticResult);
			}
			this.previousWorldDiagnosticStatus[keyValuePair.Key] = worldDiagnosticResult;
		}
		this.RefreshWorldStatus();
		this.RefreshToggleTooltips();
	}

	// Token: 0x06007991 RID: 31121 RVA: 0x002EC02C File Offset: 0x002EA22C
	public void TriggerVisualNotification(int worldID, ColonyDiagnostic.DiagnosticResult.Opinion result)
	{
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.worldRows)
		{
			if (keyValuePair.Key == worldID)
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound(ColonyDiagnosticScreen.notificationSoundsInactive[result], false));
				if (keyValuePair.Value.gameObject.activeInHierarchy)
				{
					keyValuePair.Value.StartCoroutine(this.VisualNotificationRoutine(keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").gameObject, keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Indicator"), keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Spacer").gameObject));
				}
			}
		}
	}

	// Token: 0x06007992 RID: 31122 RVA: 0x002EC114 File Offset: 0x002EA314
	private IEnumerator VisualNotificationRoutine(GameObject contentGameObject, RectTransform indicator, GameObject spacer)
	{
		Vector2 defaultIndicatorSize = new Vector2(8f, 8f);
		RectTransform contentRect = contentGameObject.rectTransform();
		float bounceDuration = 1f;
		for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
		{
			float d = Mathf.Sin(i * 3.1415927f) * 50f;
			contentRect.anchoredPosition = Vector2.left * d;
			indicator.sizeDelta = defaultIndicatorSize + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
			yield return 0;
		}
		for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
		{
			float d2 = Mathf.Sin(i * 3.1415927f) * 25f;
			contentRect.anchoredPosition = Vector2.left * d2;
			indicator.sizeDelta = defaultIndicatorSize + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
			yield return 0;
		}
		for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
		{
			float d3 = Mathf.Sin(i * 3.1415927f) * 12f;
			contentRect.anchoredPosition = Vector2.left * d3;
			indicator.sizeDelta = defaultIndicatorSize + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
			yield return 0;
		}
		contentRect.anchoredPosition = Vector2.zero;
		defaultIndicatorSize = new Vector2(8f, 8f);
		indicator.sizeDelta = defaultIndicatorSize;
		contentGameObject.rectTransform().localPosition = Vector2.zero;
		yield break;
	}

	// Token: 0x04005500 RID: 21760
	public static WorldSelector Instance;

	// Token: 0x04005501 RID: 21761
	public Dictionary<int, MultiToggle> worldRows;

	// Token: 0x04005502 RID: 21762
	public TextStyleSetting titleTextSetting;

	// Token: 0x04005503 RID: 21763
	public TextStyleSetting bodyTextSetting;

	// Token: 0x04005504 RID: 21764
	public GameObject worldRowPrefab;

	// Token: 0x04005505 RID: 21765
	public GameObject worldRowContainer;

	// Token: 0x04005506 RID: 21766
	private Dictionary<int, ColonyDiagnostic.DiagnosticResult.Opinion> previousWorldDiagnosticStatus = new Dictionary<int, ColonyDiagnostic.DiagnosticResult.Opinion>();

	// Token: 0x04005507 RID: 21767
	private Dictionary<int, List<GameObject>> worldStatusIcons = new Dictionary<int, List<GameObject>>();
}
