using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008E0 RID: 2272
public abstract class ColonyDiagnostic : ISim4000ms, IHasDlcRestrictions
{
	// Token: 0x06003F3C RID: 16188 RVA: 0x00163552 File Offset: 0x00161752
	public GameObject GetNextClickThroughObject()
	{
		if (this.aggregatedUniqueClickThroughObjects.Count == 0)
		{
			return null;
		}
		this.clickThroughIndex = (this.clickThroughIndex + 1) % this.aggregatedUniqueClickThroughObjects.Count;
		return this.aggregatedUniqueClickThroughObjects[this.clickThroughIndex];
	}

	// Token: 0x06003F3D RID: 16189 RVA: 0x00163590 File Offset: 0x00161790
	public ColonyDiagnostic(int worldID, string name)
	{
		this.worldID = worldID;
		this.name = name;
		this.id = base.GetType().Name;
		this.IsWorldModuleInterior = ClusterManager.Instance.GetWorld(worldID).IsModuleInterior;
		this.colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Bad, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Warning, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, Constants.WARNING_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Good, Constants.POSITIVE_COLOR);
		SimAndRenderScheduler.instance.Add(this, true);
	}

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06003F3E RID: 16190 RVA: 0x001636C5 File Offset: 0x001618C5
	// (set) Token: 0x06003F3F RID: 16191 RVA: 0x001636CD File Offset: 0x001618CD
	public int worldID { get; protected set; }

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06003F40 RID: 16192 RVA: 0x001636D6 File Offset: 0x001618D6
	// (set) Token: 0x06003F41 RID: 16193 RVA: 0x001636DE File Offset: 0x001618DE
	public bool IsWorldModuleInterior { get; private set; }

	// Token: 0x06003F42 RID: 16194 RVA: 0x001636E7 File Offset: 0x001618E7
	public void OnCleanUp()
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06003F43 RID: 16195 RVA: 0x001636F4 File Offset: 0x001618F4
	public void Sim4000ms(float dt)
	{
		this.SetResult(ColonyDiagnosticUtility.IgnoreFirstUpdate ? ColonyDiagnosticUtility.NoDataResult : this.Evaluate());
	}

	// Token: 0x06003F44 RID: 16196 RVA: 0x00163710 File Offset: 0x00161910
	public DiagnosticCriterion[] GetCriteria()
	{
		DiagnosticCriterion[] array = new DiagnosticCriterion[this.criteria.Values.Count];
		this.criteria.Values.CopyTo(array, 0);
		return array;
	}

	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x06003F45 RID: 16197 RVA: 0x00163746 File Offset: 0x00161946
	// (set) Token: 0x06003F46 RID: 16198 RVA: 0x0016374E File Offset: 0x0016194E
	public ColonyDiagnostic.DiagnosticResult LatestResult
	{
		get
		{
			return this.latestResult;
		}
		private set
		{
			this.latestResult = value;
		}
	}

	// Token: 0x06003F47 RID: 16199 RVA: 0x00163757 File Offset: 0x00161957
	public virtual string GetAverageValueString()
	{
		if (this.tracker != null)
		{
			return this.tracker.FormatValueString(Mathf.Round(this.tracker.GetAverageValue(this.trackerSampleCountSeconds)));
		}
		return "";
	}

	// Token: 0x06003F48 RID: 16200 RVA: 0x00163788 File Offset: 0x00161988
	public virtual string GetCurrentValueString()
	{
		return "";
	}

	// Token: 0x06003F49 RID: 16201 RVA: 0x0016378F File Offset: 0x0016198F
	protected void AddCriterion(string id, DiagnosticCriterion criterion)
	{
		if (!this.criteria.ContainsKey(id))
		{
			criterion.SetID(id);
			this.criteria.Add(id, criterion);
		}
	}

	// Token: 0x06003F4A RID: 16202 RVA: 0x001637B4 File Offset: 0x001619B4
	public virtual ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, "", null);
		bool flag = false;
		if (!ClusterManager.Instance.GetWorld(this.worldID).IsDiscovered)
		{
			return diagnosticResult;
		}
		this.aggregatedUniqueClickThroughObjects.Clear();
		foreach (KeyValuePair<string, DiagnosticCriterion> keyValuePair in this.criteria)
		{
			if (ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(this.worldID, this.id, keyValuePair.Key))
			{
				ColonyDiagnostic.DiagnosticResult diagnosticResult2 = keyValuePair.Value.Evaluate();
				if (diagnosticResult2.opinion < diagnosticResult.opinion || (!flag && diagnosticResult2.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal))
				{
					flag = true;
					diagnosticResult.opinion = diagnosticResult2.opinion;
					diagnosticResult.Message = diagnosticResult2.Message;
					diagnosticResult.clickThroughTarget = diagnosticResult2.clickThroughTarget;
					if (diagnosticResult2.clickThroughObjects != null)
					{
						foreach (GameObject item in diagnosticResult2.clickThroughObjects)
						{
							if (!this.aggregatedUniqueClickThroughObjects.Contains(item))
							{
								this.aggregatedUniqueClickThroughObjects.Add(item);
							}
						}
					}
				}
			}
		}
		return diagnosticResult;
	}

	// Token: 0x06003F4B RID: 16203 RVA: 0x0016391C File Offset: 0x00161B1C
	public void SetResult(ColonyDiagnostic.DiagnosticResult result)
	{
		this.LatestResult = result;
	}

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06003F4C RID: 16204 RVA: 0x00163925 File Offset: 0x00161B25
	protected string NO_MINIONS
	{
		get
		{
			return this.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.NO_MINIONS_PLANETOID;
		}
	}

	// Token: 0x06003F4D RID: 16205 RVA: 0x00163940 File Offset: 0x00161B40
	public virtual string[] GetRequiredDlcIds()
	{
		return null;
	}

	// Token: 0x06003F4E RID: 16206 RVA: 0x00163943 File Offset: 0x00161B43
	public virtual string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0400274D RID: 10061
	private int clickThroughIndex;

	// Token: 0x0400274E RID: 10062
	private List<GameObject> aggregatedUniqueClickThroughObjects = new List<GameObject>();

	// Token: 0x04002750 RID: 10064
	public string name;

	// Token: 0x04002751 RID: 10065
	public string id;

	// Token: 0x04002753 RID: 10067
	public string icon = "icon_errand_operate";

	// Token: 0x04002754 RID: 10068
	private Dictionary<string, DiagnosticCriterion> criteria = new Dictionary<string, DiagnosticCriterion>();

	// Token: 0x04002755 RID: 10069
	public ColonyDiagnostic.PresentationSetting presentationSetting;

	// Token: 0x04002756 RID: 10070
	private ColonyDiagnostic.DiagnosticResult latestResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.NO_DATA, null);

	// Token: 0x04002757 RID: 10071
	public Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color> colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();

	// Token: 0x04002758 RID: 10072
	public Tracker tracker;

	// Token: 0x04002759 RID: 10073
	protected float trackerSampleCountSeconds = 4f;

	// Token: 0x020018F1 RID: 6385
	public enum PresentationSetting
	{
		// Token: 0x04007C7F RID: 31871
		AverageValue,
		// Token: 0x04007C80 RID: 31872
		CurrentValue
	}

	// Token: 0x020018F2 RID: 6386
	public struct DiagnosticResult
	{
		// Token: 0x0600A0D0 RID: 41168 RVA: 0x003AA838 File Offset: 0x003A8A38
		public DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion opinion, string message, global::Tuple<Vector3, GameObject> clickThroughTarget = null)
		{
			this.message = message;
			this.opinion = opinion;
			this.clickThroughTarget = null;
			this.clickThroughObjects = null;
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x0600A0D2 RID: 41170 RVA: 0x003AA85F File Offset: 0x003A8A5F
		// (set) Token: 0x0600A0D1 RID: 41169 RVA: 0x003AA856 File Offset: 0x003A8A56
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		// Token: 0x0600A0D3 RID: 41171 RVA: 0x003AA868 File Offset: 0x003A8A68
		public string GetFormattedMessage()
		{
			switch (this.opinion)
			{
			case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.NEGATIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.NEGATIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.WARNING_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion:
			case ColonyDiagnostic.DiagnosticResult.Opinion.Normal:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.WHITE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Good:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.POSITIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			}
			return this.message;
		}

		// Token: 0x04007C81 RID: 31873
		public ColonyDiagnostic.DiagnosticResult.Opinion opinion;

		// Token: 0x04007C82 RID: 31874
		public global::Tuple<Vector3, GameObject> clickThroughTarget;

		// Token: 0x04007C83 RID: 31875
		public List<GameObject> clickThroughObjects;

		// Token: 0x04007C84 RID: 31876
		private string message;

		// Token: 0x02002999 RID: 10649
		public enum Opinion
		{
			// Token: 0x0400B7F7 RID: 47095
			Unset,
			// Token: 0x0400B7F8 RID: 47096
			DuplicantThreatening,
			// Token: 0x0400B7F9 RID: 47097
			Bad,
			// Token: 0x0400B7FA RID: 47098
			Warning,
			// Token: 0x0400B7FB RID: 47099
			Concern,
			// Token: 0x0400B7FC RID: 47100
			Suggestion,
			// Token: 0x0400B7FD RID: 47101
			Tutorial,
			// Token: 0x0400B7FE RID: 47102
			Normal,
			// Token: 0x0400B7FF RID: 47103
			Good
		}
	}
}
