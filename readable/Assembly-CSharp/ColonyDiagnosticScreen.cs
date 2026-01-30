using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CDD RID: 3293
public class ColonyDiagnosticScreen : KScreen, ISim1000ms
{
	// Token: 0x060065BB RID: 26043 RVA: 0x00264FBC File Offset: 0x002631BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ColonyDiagnosticScreen.Instance = this;
		this.RefreshSingleWorld(null);
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshSingleWorld));
		MultiToggle multiToggle = this.seeAllButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			AllDiagnosticsScreen.Instance.Show(!AllDiagnosticsScreen.Instance.IsScreenActive());
		}));
	}

	// Token: 0x060065BC RID: 26044 RVA: 0x00265032 File Offset: 0x00263232
	protected override void OnForcedCleanUp()
	{
		ColonyDiagnosticScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060065BD RID: 26045 RVA: 0x00265040 File Offset: 0x00263240
	private void RefreshSingleWorld(object data = null)
	{
		foreach (ColonyDiagnosticScreen.DiagnosticRow diagnosticRow in this.diagnosticRows)
		{
			diagnosticRow.OnCleanUp();
			Util.KDestroyGameObject(diagnosticRow.gameObject);
		}
		this.diagnosticRows.Clear();
		this.SpawnTrackerLines(ClusterManager.Instance.activeWorldId);
	}

	// Token: 0x060065BE RID: 26046 RVA: 0x002650B8 File Offset: 0x002632B8
	private void SpawnTrackerLines(int world)
	{
		this.AddDiagnostic<BreathabilityDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<FoodDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<StressDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RadiationDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<ReactorDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
		{
			if (Game.IsDlcActiveForCurrentSave("EXPANSION1_ID"))
			{
				this.AddDiagnostic<SelfChargingElectrobankDiagnostic>(world, this.contentContainer, this.diagnosticRows);
			}
			this.AddDiagnostic<BionicBatteryDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		}
		this.AddDiagnostic<FloatingRocketDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RocketFuelDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RocketOxidizerDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<FarmDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<ToiletDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<BedDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<IdleDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<TrappedDuplicantDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<EntombedDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<PowerUseDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<BatteryDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RocketsInOrbitDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<MeteorDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		List<ColonyDiagnosticScreen.DiagnosticRow> list = new List<ColonyDiagnosticScreen.DiagnosticRow>();
		foreach (ColonyDiagnosticScreen.DiagnosticRow item in this.diagnosticRows)
		{
			list.Add(item);
		}
		list.Sort((ColonyDiagnosticScreen.DiagnosticRow a, ColonyDiagnosticScreen.DiagnosticRow b) => a.diagnostic.name.CompareTo(b.diagnostic.name));
		foreach (ColonyDiagnosticScreen.DiagnosticRow diagnosticRow in list)
		{
			diagnosticRow.gameObject.transform.SetAsLastSibling();
		}
		list.Clear();
		this.seeAllButton.transform.SetAsLastSibling();
		this.RefreshAll();
	}

	// Token: 0x060065BF RID: 26047 RVA: 0x00265340 File Offset: 0x00263540
	private GameObject AddDiagnostic<T>(int worldID, GameObject parent, List<ColonyDiagnosticScreen.DiagnosticRow> parentCollection) where T : ColonyDiagnostic
	{
		T diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic<T>(worldID);
		if (diagnostic == null)
		{
			return null;
		}
		GameObject gameObject = Util.KInstantiateUI(this.linePrefab, parent, true);
		parentCollection.Add(new ColonyDiagnosticScreen.DiagnosticRow(worldID, gameObject, diagnostic));
		return gameObject;
	}

	// Token: 0x060065C0 RID: 26048 RVA: 0x00265385 File Offset: 0x00263585
	public static void SetIndication(ColonyDiagnostic.DiagnosticResult.Opinion opinion, GameObject indicatorGameObject)
	{
		indicatorGameObject.GetComponentInChildren<Image>().color = ColonyDiagnosticScreen.GetDiagnosticIndicationColor(opinion);
	}

	// Token: 0x060065C1 RID: 26049 RVA: 0x00265398 File Offset: 0x00263598
	public static Color GetDiagnosticIndicationColor(ColonyDiagnostic.DiagnosticResult.Opinion opinion)
	{
		switch (opinion)
		{
		case ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
			return Constants.NEGATIVE_COLOR;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
			return Constants.WARNING_COLOR;
		}
		return Color.white;
	}

	// Token: 0x060065C2 RID: 26050 RVA: 0x002653D5 File Offset: 0x002635D5
	public void Sim1000ms(float dt)
	{
		this.RefreshAll();
	}

	// Token: 0x060065C3 RID: 26051 RVA: 0x002653E0 File Offset: 0x002635E0
	public void RefreshAll()
	{
		foreach (ColonyDiagnosticScreen.DiagnosticRow diagnosticRow in this.diagnosticRows)
		{
			if (diagnosticRow.worldID == ClusterManager.Instance.activeWorldId)
			{
				this.UpdateDiagnosticRow(diagnosticRow);
			}
		}
		ColonyDiagnosticScreen.SetIndication(ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResult(ClusterManager.Instance.activeWorldId), this.rootIndicator);
		this.seeAllButton.GetComponentInChildren<LocText>().SetText(string.Format(UI.DIAGNOSTICS_SCREEN.SEE_ALL, AllDiagnosticsScreen.Instance.GetRowCount()));
	}

	// Token: 0x060065C4 RID: 26052 RVA: 0x00265494 File Offset: 0x00263694
	private ColonyDiagnostic.DiagnosticResult.Opinion UpdateDiagnosticRow(ColonyDiagnosticScreen.DiagnosticRow row)
	{
		ColonyDiagnostic.DiagnosticResult.Opinion currentDisplayedResult = row.currentDisplayedResult;
		bool activeInHierarchy = row.gameObject.activeInHierarchy;
		if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(row.diagnostic.id))
		{
			this.SetRowActive(row, false);
		}
		else
		{
			switch (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[row.worldID][row.diagnostic.id])
			{
			case ColonyDiagnosticUtility.DisplaySetting.Always:
				this.SetRowActive(row, true);
				break;
			case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
				this.SetRowActive(row, row.diagnostic.LatestResult.opinion < ColonyDiagnostic.DiagnosticResult.Opinion.Normal);
				break;
			case ColonyDiagnosticUtility.DisplaySetting.Never:
				this.SetRowActive(row, false);
				break;
			}
			if (row.gameObject.activeInHierarchy && (row.currentDisplayedResult < currentDisplayedResult || (row.currentDisplayedResult < ColonyDiagnostic.DiagnosticResult.Opinion.Normal && !activeInHierarchy)) && row.CheckAllowVisualNotification())
			{
				row.TriggerVisualNotification();
			}
		}
		return row.diagnostic.LatestResult.opinion;
	}

	// Token: 0x060065C5 RID: 26053 RVA: 0x00265580 File Offset: 0x00263780
	private void SetRowActive(ColonyDiagnosticScreen.DiagnosticRow row, bool active)
	{
		if (row.gameObject.activeSelf != active)
		{
			row.gameObject.SetActive(active);
			row.ResolveNotificationRoutine();
		}
	}

	// Token: 0x04004500 RID: 17664
	public GameObject linePrefab;

	// Token: 0x04004501 RID: 17665
	public static ColonyDiagnosticScreen Instance;

	// Token: 0x04004502 RID: 17666
	private List<ColonyDiagnosticScreen.DiagnosticRow> diagnosticRows = new List<ColonyDiagnosticScreen.DiagnosticRow>();

	// Token: 0x04004503 RID: 17667
	public GameObject header;

	// Token: 0x04004504 RID: 17668
	public GameObject contentContainer;

	// Token: 0x04004505 RID: 17669
	public GameObject rootIndicator;

	// Token: 0x04004506 RID: 17670
	public MultiToggle seeAllButton;

	// Token: 0x04004507 RID: 17671
	public static Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string> notificationSoundsActive = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string>
	{
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening,
			"Diagnostic_Active_DuplicantThreatening"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Bad,
			"Diagnostic_Active_Bad"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Warning,
			"Diagnostic_Active_Warning"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Concern,
			"Diagnostic_Active_Concern"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion,
			"Diagnostic_Active_Suggestion"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial,
			"Diagnostic_Active_Tutorial"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			""
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Good,
			""
		}
	};

	// Token: 0x04004508 RID: 17672
	public static Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string> notificationSoundsInactive = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string>
	{
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening,
			"Diagnostic_Inactive_DuplicantThreatening"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Bad,
			"Diagnostic_Inactive_Bad"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Warning,
			"Diagnostic_Inactive_Warning"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Concern,
			"Diagnostic_Inactive_Concern"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion,
			"Diagnostic_Inactive_Suggestion"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial,
			"Diagnostic_Inactive_Tutorial"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			""
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Good,
			""
		}
	};

	// Token: 0x02001F11 RID: 7953
	private class DiagnosticRow : ISim4000ms
	{
		// Token: 0x0600B54A RID: 46410 RVA: 0x003ED77C File Offset: 0x003EB97C
		public DiagnosticRow(int worldID, GameObject gameObject, ColonyDiagnostic diagnostic)
		{
			global::Debug.Assert(diagnostic != null);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			this.worldID = worldID;
			this.sparkLayer = component.GetReference<SparkLayer>("SparkLayer");
			this.diagnostic = diagnostic;
			this.titleLabel = component.GetReference<LocText>("TitleLabel");
			this.valueLabel = component.GetReference<LocText>("ValueLabel");
			this.indicator = component.GetReference<Image>("Indicator");
			this.image = component.GetReference<Image>("Image");
			this.tooltip = gameObject.GetComponent<ToolTip>();
			this.gameObject = gameObject;
			this.titleLabel.SetText(diagnostic.name);
			this.sparkLayer.colorRules.setOwnColor = false;
			if (diagnostic.tracker == null)
			{
				this.sparkLayer.transform.parent.gameObject.SetActive(false);
			}
			else
			{
				this.sparkLayer.ClearLines();
				global::Tuple<float, float>[] points = diagnostic.tracker.ChartableData(600f);
				this.sparkLayer.NewLine(points, diagnostic.name);
			}
			this.button = gameObject.GetComponent<MultiToggle>();
			MultiToggle multiToggle = this.button;
			multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
			{
				KSelectable kselectable = null;
				Vector3 pos = Vector3.zero;
				if (diagnostic.LatestResult.clickThroughTarget != null)
				{
					pos = diagnostic.LatestResult.clickThroughTarget.first;
					kselectable = ((diagnostic.LatestResult.clickThroughTarget.second == null) ? null : diagnostic.LatestResult.clickThroughTarget.second.GetComponent<KSelectable>());
				}
				else
				{
					GameObject nextClickThroughObject = diagnostic.GetNextClickThroughObject();
					if (nextClickThroughObject != null)
					{
						kselectable = nextClickThroughObject.GetComponent<KSelectable>();
						pos = nextClickThroughObject.transform.GetPosition();
					}
				}
				if (kselectable == null)
				{
					CameraController.Instance.ActiveWorldStarWipe(diagnostic.worldID, null);
					return;
				}
				SelectTool.Instance.SelectAndFocus(pos, kselectable);
			}));
			this.defaultIndicatorSizeDelta = Vector2.zero;
			this.Update(true);
			SimAndRenderScheduler.instance.Add(this, true);
		}

		// Token: 0x0600B54B RID: 46411 RVA: 0x003ED907 File Offset: 0x003EBB07
		public void OnCleanUp()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x0600B54C RID: 46412 RVA: 0x003ED914 File Offset: 0x003EBB14
		public void Sim4000ms(float dt)
		{
			this.Update(false);
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x0600B54D RID: 46413 RVA: 0x003ED91D File Offset: 0x003EBB1D
		// (set) Token: 0x0600B54E RID: 46414 RVA: 0x003ED925 File Offset: 0x003EBB25
		public GameObject gameObject { get; private set; }

		// Token: 0x0600B54F RID: 46415 RVA: 0x003ED930 File Offset: 0x003EBB30
		public void Update(bool force = false)
		{
			if (!force && ClusterManager.Instance.activeWorldId != this.worldID)
			{
				return;
			}
			Color color = Color.white;
			global::Debug.Assert(this.diagnostic.LatestResult.opinion > ColonyDiagnostic.DiagnosticResult.Opinion.Unset, string.Format("{0} criteria returned no opinion. Make sure the DiagnosticResult parameters are used or an opinion result is otherwise set in all of its criteria", this.diagnostic));
			this.currentDisplayedResult = this.diagnostic.LatestResult.opinion;
			color = this.diagnostic.colors[this.diagnostic.LatestResult.opinion];
			if (this.diagnostic.tracker != null)
			{
				global::Tuple<float, float>[] data = this.diagnostic.tracker.ChartableData(600f);
				this.sparkLayer.RefreshLine(data, this.diagnostic.name);
				this.sparkLayer.SetColor(color);
			}
			this.indicator.color = this.diagnostic.colors[this.diagnostic.LatestResult.opinion];
			this.tooltip.SetSimpleTooltip((this.diagnostic.LatestResult.Message.IsNullOrWhiteSpace() ? UI.COLONY_DIAGNOSTICS.GENERIC_STATUS_NORMAL.text : this.diagnostic.LatestResult.Message) + "\n\n" + UI.COLONY_DIAGNOSTICS.MUTE_TUTORIAL.text);
			ColonyDiagnostic.PresentationSetting presentationSetting = this.diagnostic.presentationSetting;
			if (presentationSetting == ColonyDiagnostic.PresentationSetting.AverageValue || presentationSetting != ColonyDiagnostic.PresentationSetting.CurrentValue)
			{
				this.valueLabel.SetText(this.diagnostic.GetAverageValueString());
			}
			else
			{
				this.valueLabel.SetText(this.diagnostic.GetCurrentValueString());
			}
			if (!string.IsNullOrEmpty(this.diagnostic.icon))
			{
				this.image.sprite = Assets.GetSprite(this.diagnostic.icon);
			}
			if (color == Constants.NEUTRAL_COLOR)
			{
				color = Color.white;
			}
			this.titleLabel.color = color;
		}

		// Token: 0x0600B550 RID: 46416 RVA: 0x003EDB13 File Offset: 0x003EBD13
		public bool CheckAllowVisualNotification()
		{
			return this.timeOfLastNotification == 0f || GameClock.Instance.GetTime() >= this.timeOfLastNotification + 300f;
		}

		// Token: 0x0600B551 RID: 46417 RVA: 0x003EDB40 File Offset: 0x003EBD40
		public void TriggerVisualNotification()
		{
			if (DebugHandler.NotificationsDisabled)
			{
				return;
			}
			if (this.activeRoutine == null)
			{
				this.timeOfLastNotification = GameClock.Instance.GetTime();
				KFMOD.PlayUISound(GlobalAssets.GetSound(ColonyDiagnosticScreen.notificationSoundsActive[this.currentDisplayedResult], false));
				this.activeRoutine = this.gameObject.GetComponent<KMonoBehaviour>().StartCoroutine(this.VisualNotificationRoutine());
			}
		}

		// Token: 0x0600B552 RID: 46418 RVA: 0x003EDBA4 File Offset: 0x003EBDA4
		private IEnumerator VisualNotificationRoutine()
		{
			RectTransform indicator = this.gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Indicator").rectTransform;
			this.defaultIndicatorSizeDelta = Vector2.zero;
			indicator.sizeDelta = this.defaultIndicatorSizeDelta;
			RectTransform contentRect = this.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content");
			float bounceDuration = 1f;
			for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
			{
				float d = Mathf.Sin(i * 3.1415927f) * 50f;
				contentRect.anchoredPosition = Vector2.left * d;
				indicator.sizeDelta = this.defaultIndicatorSizeDelta + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
				yield return 0;
			}
			contentRect.anchoredPosition = Vector2.zero;
			for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
			{
				float d2 = Mathf.Sin(i * 3.1415927f) * 25f;
				contentRect.anchoredPosition = Vector2.left * d2;
				indicator.sizeDelta = this.defaultIndicatorSizeDelta + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
				yield return 0;
			}
			contentRect.anchoredPosition = Vector2.zero;
			for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
			{
				float d3 = Mathf.Sin(i * 3.1415927f) * 12f;
				contentRect.anchoredPosition = Vector2.left * d3;
				indicator.sizeDelta = this.defaultIndicatorSizeDelta + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
				yield return 0;
			}
			contentRect.anchoredPosition = Vector2.zero;
			this.ResolveNotificationRoutine();
			yield break;
		}

		// Token: 0x0600B553 RID: 46419 RVA: 0x003EDBB3 File Offset: 0x003EBDB3
		public void ResolveNotificationRoutine()
		{
			this.gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Indicator").rectTransform.sizeDelta = Vector2.zero;
			this.activeRoutine = null;
		}

		// Token: 0x04009173 RID: 37235
		private const float displayHistoryPeriod = 600f;

		// Token: 0x04009174 RID: 37236
		public ColonyDiagnostic diagnostic;

		// Token: 0x04009175 RID: 37237
		public SparkLayer sparkLayer;

		// Token: 0x04009177 RID: 37239
		public int worldID;

		// Token: 0x04009178 RID: 37240
		private LocText titleLabel;

		// Token: 0x04009179 RID: 37241
		private LocText valueLabel;

		// Token: 0x0400917A RID: 37242
		private Image indicator;

		// Token: 0x0400917B RID: 37243
		private ToolTip tooltip;

		// Token: 0x0400917C RID: 37244
		private MultiToggle button;

		// Token: 0x0400917D RID: 37245
		private Image image;

		// Token: 0x0400917E RID: 37246
		public ColonyDiagnostic.DiagnosticResult.Opinion currentDisplayedResult;

		// Token: 0x0400917F RID: 37247
		private Vector2 defaultIndicatorSizeDelta;

		// Token: 0x04009180 RID: 37248
		private float timeOfLastNotification;

		// Token: 0x04009181 RID: 37249
		private const float MIN_TIME_BETWEEN_NOTIFICATIONS = 300f;

		// Token: 0x04009182 RID: 37250
		private Coroutine activeRoutine;
	}
}
