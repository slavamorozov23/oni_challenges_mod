using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DF0 RID: 3568
public class ResearchScreen : KModalScreen
{
	// Token: 0x0600705A RID: 28762 RVA: 0x002AB45B File Offset: 0x002A965B
	public bool IsBeingResearched(Tech tech)
	{
		return Research.Instance.IsBeingResearched(tech);
	}

	// Token: 0x0600705B RID: 28763 RVA: 0x002AB468 File Offset: 0x002A9668
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x0600705C RID: 28764 RVA: 0x002AB47D File Offset: 0x002A967D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.m_Raycaster = base.GetComponent<GraphicRaycaster>();
		this.scrollContentRaycaster = this.scrollContent.GetComponent<GraphicRaycaster>();
	}

	// Token: 0x0600705D RID: 28765 RVA: 0x002AB4B5 File Offset: 0x002A96B5
	private void ZoomOut()
	{
		this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerButton, this.minZoom, this.maxZoom);
		this.zoomCenterLock = true;
	}

	// Token: 0x0600705E RID: 28766 RVA: 0x002AB4E2 File Offset: 0x002A96E2
	private void ZoomIn()
	{
		this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerButton, this.minZoom, this.maxZoom);
		this.zoomCenterLock = true;
	}

	// Token: 0x0600705F RID: 28767 RVA: 0x002AB510 File Offset: 0x002A9710
	public void ZoomToTech(string techID, bool highlight = false)
	{
		Vector2 a = this.entryMap[Db.Get().Techs.Get(techID)].rectTransform().GetLocalPosition() + new Vector2(-this.foreground.rectTransform().rect.size.x / 2f, this.foreground.rectTransform().rect.size.y / 2f);
		this.forceTargetPosition = -a;
		this.zoomingToTarget = true;
		this.targetZoom = this.maxZoom;
		if (highlight)
		{
			this.sideBar.SetSearch(Db.Get().Techs.Get(techID).Name);
		}
	}

	// Token: 0x06007060 RID: 28768 RVA: 0x002AB5DC File Offset: 0x002A97DC
	private void Update()
	{
		if (!this.IsScreenActive())
		{
			return;
		}
		RectTransform component = this.scrollContent.GetComponent<RectTransform>();
		if (this.isDragging && !KInputManager.isFocused)
		{
			this.AbortDragging();
		}
		Vector2 anchoredPosition = component.anchoredPosition;
		float t = Mathf.Min(this.effectiveZoomSpeed * Time.unscaledDeltaTime, 0.9f);
		this.currentZoom = Mathf.Lerp(this.currentZoom, this.targetZoom, t);
		Vector2 b = Vector2.zero;
		Vector2 v = KInputManager.GetMousePos();
		Vector2 b2 = this.zoomCenterLock ? (component.InverseTransformPoint(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2))) * this.currentZoom) : (component.InverseTransformPoint(v) * this.currentZoom);
		component.localScale = new Vector3(this.currentZoom, this.currentZoom, 1f);
		b = (this.zoomCenterLock ? (component.InverseTransformPoint(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2))) * this.currentZoom) : (component.InverseTransformPoint(v) * this.currentZoom)) - b2;
		float d = this.keyboardScrollSpeed;
		if (this.panUp)
		{
			this.keyPanDelta -= Vector2.up * Time.unscaledDeltaTime * d;
		}
		else if (this.panDown)
		{
			this.keyPanDelta += Vector2.up * Time.unscaledDeltaTime * d;
		}
		if (this.panLeft)
		{
			this.keyPanDelta += Vector2.right * Time.unscaledDeltaTime * d;
		}
		else if (this.panRight)
		{
			this.keyPanDelta -= Vector2.right * Time.unscaledDeltaTime * d;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			Vector2 a = KInputManager.steamInputInterpreter.GetSteamCameraMovement();
			a *= -1f;
			this.keyPanDelta = a * Time.unscaledDeltaTime * d * 2f;
		}
		Vector2 b3 = new Vector2(Mathf.Lerp(0f, this.keyPanDelta.x, Time.unscaledDeltaTime * this.keyPanEasing), Mathf.Lerp(0f, this.keyPanDelta.y, Time.unscaledDeltaTime * this.keyPanEasing));
		this.keyPanDelta -= b3;
		Vector2 vector = Vector2.zero;
		if (this.isDragging)
		{
			Vector2 b4 = KInputManager.GetMousePos() - this.dragLastPosition;
			vector += b4;
			this.dragLastPosition = KInputManager.GetMousePos();
			this.dragInteria = Vector2.ClampMagnitude(this.dragInteria + b4, 400f);
		}
		this.dragInteria *= Mathf.Max(0f, 1f - Time.unscaledDeltaTime * 4f);
		Vector2 vector2 = anchoredPosition + b + this.keyPanDelta + vector;
		if (!this.isDragging)
		{
			Vector2 size = base.GetComponent<RectTransform>().rect.size;
			Vector2 vector3 = new Vector2((-component.rect.size.x / 2f - 250f) * this.currentZoom, -250f * this.currentZoom);
			Vector2 vector4 = new Vector2(250f * this.currentZoom, (component.rect.size.y + 250f) * this.currentZoom - size.y);
			Vector2 a2 = new Vector2(Mathf.Clamp(vector2.x, vector3.x, vector4.x), Mathf.Clamp(vector2.y, vector3.y, vector4.y));
			this.forceTargetPosition = new Vector2(Mathf.Clamp(this.forceTargetPosition.x, vector3.x, vector4.x), Mathf.Clamp(this.forceTargetPosition.y, vector3.y, vector4.y));
			Vector2 vector5 = a2 + this.dragInteria - vector2;
			if (!this.panLeft && !this.panRight && !this.panUp && !this.panDown)
			{
				vector2 += vector5 * this.edgeClampFactor * Time.unscaledDeltaTime;
			}
			else
			{
				vector2 += vector5;
				if (vector5.x < 0f)
				{
					this.keyPanDelta.x = Mathf.Min(0f, this.keyPanDelta.x);
				}
				if (vector5.x > 0f)
				{
					this.keyPanDelta.x = Mathf.Max(0f, this.keyPanDelta.x);
				}
				if (vector5.y < 0f)
				{
					this.keyPanDelta.y = Mathf.Min(0f, this.keyPanDelta.y);
				}
				if (vector5.y > 0f)
				{
					this.keyPanDelta.y = Mathf.Max(0f, this.keyPanDelta.y);
				}
			}
		}
		if (this.zoomingToTarget)
		{
			vector2 = Vector2.Lerp(vector2, this.forceTargetPosition, Time.unscaledDeltaTime * 4f);
			if (Vector3.Distance(vector2, this.forceTargetPosition) < 1f || this.isDragging || this.panLeft || this.panRight || this.panUp || this.panDown)
			{
				this.zoomingToTarget = false;
			}
		}
		component.anchoredPosition = vector2;
	}

	// Token: 0x06007061 RID: 28769 RVA: 0x002ABBD4 File Offset: 0x002A9DD4
	protected override void OnSpawn()
	{
		base.Subscribe(Research.Instance.gameObject, -1914338957, new Action<object>(this.OnActiveResearchChanged));
		base.Subscribe(Game.Instance.gameObject, -107300940, new Action<object>(this.OnResearchComplete));
		this.deactivateResearchScreenHandle = base.Subscribe(Game.Instance.gameObject, -1974454597, delegate(object o)
		{
			this.Show(false);
		});
		this.pointDisplayMap = new Dictionary<string, LocText>();
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.pointDisplayMap[researchType.id] = Util.KInstantiateUI(this.pointDisplayCountPrefab, this.pointDisplayContainer, true).GetComponentInChildren<LocText>();
			this.pointDisplayMap[researchType.id].text = Research.Instance.globalPointInventory.PointsByTypeID[researchType.id].ToString();
			this.pointDisplayMap[researchType.id].transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(researchType.description);
			this.pointDisplayMap[researchType.id].transform.parent.GetComponentInChildren<Image>().sprite = researchType.sprite;
		}
		this.pointDisplayContainer.transform.parent.gameObject.SetActive(Research.Instance.UseGlobalPointInventory);
		this.entryMap = new Dictionary<Tech, ResearchEntry>();
		List<Tech> resources = Db.Get().Techs.resources;
		resources.Sort((Tech x, Tech y) => y.center.y.CompareTo(x.center.y));
		List<TechTreeTitle> resources2 = Db.Get().TechTreeTitles.resources;
		resources2.Sort((TechTreeTitle x, TechTreeTitle y) => y.center.y.CompareTo(x.center.y));
		float x3 = 0f;
		float y3 = 125f;
		Vector2 b = new Vector2(x3, y3);
		for (int i = 0; i < resources2.Count; i++)
		{
			ResearchTreeTitle researchTreeTitle = Util.KInstantiateUI<ResearchTreeTitle>(this.researchTreeTitlePrefab.gameObject, this.treeTitles, false);
			TechTreeTitle techTreeTitle = resources2[i];
			researchTreeTitle.name = techTreeTitle.Name + " Title";
			Vector3 vector = techTreeTitle.center + b;
			researchTreeTitle.transform.rectTransform().anchoredPosition = vector;
			float num = techTreeTitle.height;
			if (i + 1 < resources2.Count)
			{
				TechTreeTitle techTreeTitle2 = resources2[i + 1];
				Vector3 vector2 = techTreeTitle2.center + b;
				num += vector.y - (vector2.y + techTreeTitle2.height);
			}
			else
			{
				num += 600f;
			}
			researchTreeTitle.transform.rectTransform().sizeDelta = new Vector2(techTreeTitle.width, num);
			researchTreeTitle.SetLabel(techTreeTitle.Name);
			researchTreeTitle.SetColor(i);
		}
		List<Vector2> list = new List<Vector2>();
		float x2 = 0f;
		float y2 = 0f;
		Vector2 b2 = new Vector2(x2, y2);
		for (int j = 0; j < resources.Count; j++)
		{
			ResearchEntry researchEntry = Util.KInstantiateUI<ResearchEntry>(this.entryPrefab.gameObject, this.scrollContent, false);
			Tech tech = resources[j];
			researchEntry.name = tech.Name + " Panel";
			Vector3 v = tech.center + b2;
			researchEntry.researchScreenReference = this;
			researchEntry.transform.rectTransform().anchoredPosition = v;
			researchEntry.transform.rectTransform().sizeDelta = new Vector2(tech.width, tech.height);
			this.entryMap.Add(tech, researchEntry);
			if (tech.edges.Count > 0)
			{
				for (int k = 0; k < tech.edges.Count; k++)
				{
					ResourceTreeNode.Edge edge = tech.edges[k];
					if (edge.path == null)
					{
						list.AddRange(edge.SrcTarget);
					}
					else
					{
						ResourceTreeNode.Edge.EdgeType edgeType = edge.edgeType;
						if (edgeType <= ResourceTreeNode.Edge.EdgeType.QuadCurveEdge || edgeType - ResourceTreeNode.Edge.EdgeType.BezierEdge <= 1)
						{
							list.Add(edge.SrcTarget[0]);
							list.Add(edge.path[0]);
							for (int l = 1; l < edge.path.Count; l++)
							{
								list.Add(edge.path[l - 1]);
								list.Add(edge.path[l]);
							}
							list.Add(edge.path[edge.path.Count - 1]);
							list.Add(edge.SrcTarget[1]);
						}
						else
						{
							list.AddRange(edge.path);
						}
					}
				}
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			list[m] = new Vector2(list[m].x, list[m].y + this.foreground.transform.rectTransform().rect.height);
		}
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetTech(keyValuePair.Key);
		}
		this.CloseButton.soundPlayer.Enabled = false;
		this.CloseButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		base.StartCoroutine(this.WaitAndSetActiveResearch());
		base.OnSpawn();
		this.scrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(250f, -250f);
		this.zoomOutButton.onClick += delegate()
		{
			this.ZoomOut();
		};
		this.zoomInButton.onClick += delegate()
		{
			this.ZoomIn();
		};
		base.gameObject.SetActive(true);
		this.Show(false);
	}

	// Token: 0x06007062 RID: 28770 RVA: 0x002AC288 File Offset: 0x002AA488
	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		this.isDragging = true;
	}

	// Token: 0x06007063 RID: 28771 RVA: 0x002AC298 File Offset: 0x002AA498
	public override void OnEndDrag(PointerEventData eventData)
	{
		base.OnEndDrag(eventData);
		this.AbortDragging();
	}

	// Token: 0x06007064 RID: 28772 RVA: 0x002AC2A7 File Offset: 0x002AA4A7
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.Unsubscribe(Game.Instance.gameObject, ref this.deactivateResearchScreenHandle);
	}

	// Token: 0x06007065 RID: 28773 RVA: 0x002AC2C5 File Offset: 0x002AA4C5
	private IEnumerator WaitAndSetActiveResearch()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		TechInstance targetResearch = Research.Instance.GetTargetResearch();
		if (targetResearch != null)
		{
			this.SetActiveResearch(targetResearch.tech);
		}
		yield break;
	}

	// Token: 0x06007066 RID: 28774 RVA: 0x002AC2D4 File Offset: 0x002AA4D4
	public Vector3 GetEntryPosition(Tech tech)
	{
		if (!this.entryMap.ContainsKey(tech))
		{
			global::Debug.LogError("The Tech provided was not present in the dictionary");
			return Vector3.zero;
		}
		return this.entryMap[tech].transform.GetPosition();
	}

	// Token: 0x06007067 RID: 28775 RVA: 0x002AC30A File Offset: 0x002AA50A
	public ResearchEntry GetEntry(Tech tech)
	{
		if (this.entryMap == null)
		{
			return null;
		}
		if (!this.entryMap.ContainsKey(tech))
		{
			global::Debug.LogError("The Tech provided was not present in the dictionary");
			return null;
		}
		return this.entryMap[tech];
	}

	// Token: 0x06007068 RID: 28776 RVA: 0x002AC33C File Offset: 0x002AA53C
	public void SetEntryPercentage(Tech tech, float percent)
	{
		ResearchEntry entry = this.GetEntry(tech);
		if (entry != null)
		{
			entry.SetPercentage(percent);
		}
	}

	// Token: 0x06007069 RID: 28777 RVA: 0x002AC364 File Offset: 0x002AA564
	public void TurnEverythingOff()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetEverythingOff();
		}
	}

	// Token: 0x0600706A RID: 28778 RVA: 0x002AC3BC File Offset: 0x002AA5BC
	public void TurnEverythingOn()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetEverythingOn();
		}
	}

	// Token: 0x0600706B RID: 28779 RVA: 0x002AC414 File Offset: 0x002AA614
	private void SelectAllEntries(Tech tech, bool isSelected)
	{
		ResearchEntry entry = this.GetEntry(tech);
		if (entry != null)
		{
			entry.QueueStateChanged(isSelected);
		}
		foreach (Tech tech2 in tech.requiredTech)
		{
			this.SelectAllEntries(tech2, isSelected);
		}
	}

	// Token: 0x0600706C RID: 28780 RVA: 0x002AC480 File Offset: 0x002AA680
	private void OnResearchComplete(object data)
	{
		if (data is Tech)
		{
			Tech tech = (Tech)data;
			ResearchEntry entry = this.GetEntry(tech);
			if (entry != null)
			{
				entry.ResearchCompleted(true);
			}
			this.UpdateProgressBars();
			this.UpdatePointDisplay();
		}
	}

	// Token: 0x0600706D RID: 28781 RVA: 0x002AC4C0 File Offset: 0x002AA6C0
	private void UpdatePointDisplay()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.pointDisplayMap[researchType.id].text = string.Format("{0}: {1}", Research.Instance.researchTypes.GetResearchType(researchType.id).name, Research.Instance.globalPointInventory.PointsByTypeID[researchType.id].ToString());
		}
	}

	// Token: 0x0600706E RID: 28782 RVA: 0x002AC574 File Offset: 0x002AA774
	private void OnActiveResearchChanged(object data)
	{
		List<TechInstance> list = (List<TechInstance>)data;
		foreach (TechInstance techInstance in list)
		{
			ResearchEntry entry = this.GetEntry(techInstance.tech);
			if (entry != null)
			{
				entry.QueueStateChanged(true);
			}
		}
		this.UpdateProgressBars();
		this.UpdatePointDisplay();
		if (list.Count > 0)
		{
			this.currentResearch = list[list.Count - 1].tech;
		}
	}

	// Token: 0x0600706F RID: 28783 RVA: 0x002AC610 File Offset: 0x002AA810
	private void UpdateProgressBars()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.UpdateProgressBars();
		}
	}

	// Token: 0x06007070 RID: 28784 RVA: 0x002AC668 File Offset: 0x002AA868
	public void CancelResearch()
	{
		List<TechInstance> researchQueue = Research.Instance.GetResearchQueue();
		foreach (TechInstance techInstance in researchQueue)
		{
			ResearchEntry entry = this.GetEntry(techInstance.tech);
			if (entry != null)
			{
				entry.QueueStateChanged(false);
			}
		}
		researchQueue.Clear();
	}

	// Token: 0x06007071 RID: 28785 RVA: 0x002AC6E0 File Offset: 0x002AA8E0
	private void SetActiveResearch(Tech newResearch)
	{
		if (newResearch != this.currentResearch && this.currentResearch != null)
		{
			this.SelectAllEntries(this.currentResearch, false);
		}
		this.currentResearch = newResearch;
		if (this.currentResearch != null)
		{
			this.SelectAllEntries(this.currentResearch, true);
		}
	}

	// Token: 0x06007072 RID: 28786 RVA: 0x002AC71C File Offset: 0x002AA91C
	public override bool IsScreenActive()
	{
		return this.canvasGroup.alpha > 0f;
	}

	// Token: 0x06007073 RID: 28787 RVA: 0x002AC730 File Offset: 0x002AA930
	public override void Show(bool show = true)
	{
		this.mouseOver = false;
		this.scrollContentChildFitter.enabled = show;
		this.canvasGroup.alpha = (float)(show ? 1 : 0);
		this.m_Raycaster.enabled = show;
		this.scrollContentRaycaster.enabled = show;
		this.sideBar.Show(show);
		this.OnShow(show);
	}

	// Token: 0x06007074 RID: 28788 RVA: 0x002AC790 File Offset: 0x002AA990
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.sideBar.ResetFilter();
		}
		if (show)
		{
			CameraController.Instance.DisableUserCameraControl = true;
			if (DetailsScreen.Instance != null)
			{
				DetailsScreen.Instance.gameObject.SetActive(false);
			}
		}
		else
		{
			CameraController.Instance.DisableUserCameraControl = false;
			if (SelectTool.Instance.selected != null && !DetailsScreen.Instance.gameObject.activeSelf)
			{
				DetailsScreen.Instance.gameObject.SetActive(true);
				DetailsScreen.Instance.Refresh(SelectTool.Instance.selected.gameObject);
			}
		}
		this.UpdateProgressBars();
		this.UpdatePointDisplay();
	}

	// Token: 0x06007075 RID: 28789 RVA: 0x002AC842 File Offset: 0x002AAA42
	private void AbortDragging()
	{
		this.isDragging = false;
		this.draggingJustEnded = true;
	}

	// Token: 0x06007076 RID: 28790 RVA: 0x002AC852 File Offset: 0x002AAA52
	private void LateUpdate()
	{
		this.draggingJustEnded = false;
	}

	// Token: 0x06007077 RID: 28791 RVA: 0x002AC85C File Offset: 0x002AAA5C
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			if (e.IsAction(global::Action.MouseRight) && !this.isDragging && !this.draggingJustEnded)
			{
				ManagementMenu.Instance.CloseAll();
			}
			if (e.IsAction(global::Action.MouseRight) || e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.MouseMiddle))
			{
				this.AbortDragging();
			}
			if (this.panUp && e.TryConsume(global::Action.PanUp))
			{
				this.panUp = false;
				return;
			}
			if (this.panDown && e.TryConsume(global::Action.PanDown))
			{
				this.panDown = false;
				return;
			}
			if (this.panRight && e.TryConsume(global::Action.PanRight))
			{
				this.panRight = false;
				return;
			}
			if (this.panLeft && e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = false;
				return;
			}
		}
		base.OnKeyUp(e);
	}

	// Token: 0x06007078 RID: 28792 RVA: 0x002AC934 File Offset: 0x002AAB34
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			if (e.TryConsume(global::Action.MouseRight))
			{
				this.dragStartPosition = KInputManager.GetMousePos();
				this.dragLastPosition = KInputManager.GetMousePos();
				return;
			}
			if (e.TryConsume(global::Action.MouseLeft))
			{
				this.dragStartPosition = KInputManager.GetMousePos();
				this.dragLastPosition = KInputManager.GetMousePos();
				return;
			}
			if (KInputManager.GetMousePos().x > this.sideBar.rectTransform().sizeDelta.x && CameraController.IsMouseOverGameWindow)
			{
				if (e.TryConsume(global::Action.ZoomIn))
				{
					this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
					this.zoomCenterLock = false;
					return;
				}
				if (e.TryConsume(global::Action.ZoomOut))
				{
					this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
					this.zoomCenterLock = false;
					return;
				}
			}
			if (e.TryConsume(global::Action.Escape))
			{
				ManagementMenu.Instance.CloseAll();
				return;
			}
			if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = true;
				return;
			}
			if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = true;
				return;
			}
			if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = true;
				return;
			}
			if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = true;
				return;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04004D3C RID: 19772
	private const float SCROLL_BUFFER = 250f;

	// Token: 0x04004D3D RID: 19773
	[SerializeField]
	private Image BG;

	// Token: 0x04004D3E RID: 19774
	public ResearchEntry entryPrefab;

	// Token: 0x04004D3F RID: 19775
	public ResearchTreeTitle researchTreeTitlePrefab;

	// Token: 0x04004D40 RID: 19776
	public GameObject foreground;

	// Token: 0x04004D41 RID: 19777
	public GameObject scrollContent;

	// Token: 0x04004D42 RID: 19778
	public GameObject treeTitles;

	// Token: 0x04004D43 RID: 19779
	public GameObject pointDisplayCountPrefab;

	// Token: 0x04004D44 RID: 19780
	public GameObject pointDisplayContainer;

	// Token: 0x04004D45 RID: 19781
	private Dictionary<string, LocText> pointDisplayMap;

	// Token: 0x04004D46 RID: 19782
	private Dictionary<Tech, ResearchEntry> entryMap;

	// Token: 0x04004D47 RID: 19783
	[SerializeField]
	private KButton zoomOutButton;

	// Token: 0x04004D48 RID: 19784
	[SerializeField]
	private KButton zoomInButton;

	// Token: 0x04004D49 RID: 19785
	[SerializeField]
	private ResearchScreenSideBar sideBar;

	// Token: 0x04004D4A RID: 19786
	private Tech currentResearch;

	// Token: 0x04004D4B RID: 19787
	public KButton CloseButton;

	// Token: 0x04004D4C RID: 19788
	private GraphicRaycaster m_Raycaster;

	// Token: 0x04004D4D RID: 19789
	private GraphicRaycaster scrollContentRaycaster;

	// Token: 0x04004D4E RID: 19790
	private PointerEventData m_PointerEventData;

	// Token: 0x04004D4F RID: 19791
	private Vector3 currentScrollPosition;

	// Token: 0x04004D50 RID: 19792
	private bool panUp;

	// Token: 0x04004D51 RID: 19793
	private bool panDown;

	// Token: 0x04004D52 RID: 19794
	private bool panLeft;

	// Token: 0x04004D53 RID: 19795
	private bool panRight;

	// Token: 0x04004D54 RID: 19796
	[SerializeField]
	private KChildFitter scrollContentChildFitter;

	// Token: 0x04004D55 RID: 19797
	private CanvasGroup canvasGroup;

	// Token: 0x04004D56 RID: 19798
	private bool isDragging;

	// Token: 0x04004D57 RID: 19799
	private Vector3 dragStartPosition;

	// Token: 0x04004D58 RID: 19800
	private Vector3 dragLastPosition;

	// Token: 0x04004D59 RID: 19801
	private Vector2 dragInteria;

	// Token: 0x04004D5A RID: 19802
	private Vector2 forceTargetPosition;

	// Token: 0x04004D5B RID: 19803
	private bool zoomingToTarget;

	// Token: 0x04004D5C RID: 19804
	private bool draggingJustEnded;

	// Token: 0x04004D5D RID: 19805
	private float targetZoom = 0.6f;

	// Token: 0x04004D5E RID: 19806
	private float currentZoom = 1f;

	// Token: 0x04004D5F RID: 19807
	private bool zoomCenterLock;

	// Token: 0x04004D60 RID: 19808
	private Vector2 keyPanDelta = Vector3.zero;

	// Token: 0x04004D61 RID: 19809
	[SerializeField]
	private float effectiveZoomSpeed = 5f;

	// Token: 0x04004D62 RID: 19810
	[SerializeField]
	private float zoomAmountPerScroll = 0.05f;

	// Token: 0x04004D63 RID: 19811
	[SerializeField]
	private float zoomAmountPerButton = 0.5f;

	// Token: 0x04004D64 RID: 19812
	[SerializeField]
	private float minZoom = 0.15f;

	// Token: 0x04004D65 RID: 19813
	[SerializeField]
	private float maxZoom = 1f;

	// Token: 0x04004D66 RID: 19814
	[SerializeField]
	private float keyboardScrollSpeed = 200f;

	// Token: 0x04004D67 RID: 19815
	[SerializeField]
	private float keyPanEasing = 1f;

	// Token: 0x04004D68 RID: 19816
	[SerializeField]
	private float edgeClampFactor = 0.5f;

	// Token: 0x04004D69 RID: 19817
	private int deactivateResearchScreenHandle = -1;

	// Token: 0x0200205B RID: 8283
	public enum ResearchState
	{
		// Token: 0x040095CB RID: 38347
		Available,
		// Token: 0x040095CC RID: 38348
		ActiveResearch,
		// Token: 0x040095CD RID: 38349
		ResearchComplete,
		// Token: 0x040095CE RID: 38350
		MissingPrerequisites,
		// Token: 0x040095CF RID: 38351
		StateCount
	}
}
