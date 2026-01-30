using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000628 RID: 1576
[AddComponentMenu("KMonoBehaviour/scripts/ReportManager")]
public class ReportManager : KMonoBehaviour
{
	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x0600258E RID: 9614 RVA: 0x000D77B5 File Offset: 0x000D59B5
	public List<ReportManager.DailyReport> reports
	{
		get
		{
			return this.dailyReports;
		}
	}

	// Token: 0x0600258F RID: 9615 RVA: 0x000D77BD File Offset: 0x000D59BD
	public static void DestroyInstance()
	{
		ReportManager.Instance = null;
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06002590 RID: 9616 RVA: 0x000D77C5 File Offset: 0x000D59C5
	// (set) Token: 0x06002591 RID: 9617 RVA: 0x000D77CC File Offset: 0x000D59CC
	public static ReportManager Instance { get; private set; }

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06002592 RID: 9618 RVA: 0x000D77D4 File Offset: 0x000D59D4
	public ReportManager.DailyReport TodaysReport
	{
		get
		{
			return this.todaysReport;
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06002593 RID: 9619 RVA: 0x000D77DC File Offset: 0x000D59DC
	public ReportManager.DailyReport YesterdaysReport
	{
		get
		{
			if (this.dailyReports.Count <= 1)
			{
				return null;
			}
			return this.dailyReports[this.dailyReports.Count - 1];
		}
	}

	// Token: 0x06002594 RID: 9620 RVA: 0x000D7806 File Offset: 0x000D5A06
	protected override void OnPrefabInit()
	{
		ReportManager.Instance = this;
		base.Subscribe(Game.Instance.gameObject, -1917495436, new Action<object>(this.OnSaveGameReady));
		this.noteStorage = new ReportManager.NoteStorage();
	}

	// Token: 0x06002595 RID: 9621 RVA: 0x000D783B File Offset: 0x000D5A3B
	protected override void OnCleanUp()
	{
		ReportManager.Instance = null;
	}

	// Token: 0x06002596 RID: 9622 RVA: 0x000D7843 File Offset: 0x000D5A43
	[CustomSerialize]
	private void CustomSerialize(BinaryWriter writer)
	{
		writer.Write(0);
		this.noteStorage.Serialize(writer);
	}

	// Token: 0x06002597 RID: 9623 RVA: 0x000D7858 File Offset: 0x000D5A58
	[CustomDeserialize]
	private void CustomDeserialize(IReader reader)
	{
		if (this.noteStorageBytes == null)
		{
			global::Debug.Assert(reader.ReadInt32() == 0);
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(reader.RawBytes()));
			binaryReader.BaseStream.Position = (long)reader.Position;
			this.noteStorage.Deserialize(binaryReader);
			reader.SkipBytes((int)binaryReader.BaseStream.Position - reader.Position);
		}
	}

	// Token: 0x06002598 RID: 9624 RVA: 0x000D78C3 File Offset: 0x000D5AC3
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.noteStorageBytes != null)
		{
			this.noteStorage.Deserialize(new BinaryReader(new MemoryStream(this.noteStorageBytes)));
			this.noteStorageBytes = null;
		}
	}

	// Token: 0x06002599 RID: 9625 RVA: 0x000D78F0 File Offset: 0x000D5AF0
	private void OnSaveGameReady(object data)
	{
		base.Subscribe(GameClock.Instance.gameObject, -722330267, new Action<object>(this.OnNightTime));
		if (this.todaysReport == null)
		{
			this.todaysReport = new ReportManager.DailyReport(this);
			this.todaysReport.day = GameUtil.GetCurrentCycle();
		}
	}

	// Token: 0x0600259A RID: 9626 RVA: 0x000D7943 File Offset: 0x000D5B43
	public void ReportValue(ReportManager.ReportType reportType, float value, string note = null, string context = null)
	{
		this.TodaysReport.AddData(reportType, value, note, context);
	}

	// Token: 0x0600259B RID: 9627 RVA: 0x000D7958 File Offset: 0x000D5B58
	private void OnNightTime(object data)
	{
		this.dailyReports.Add(this.todaysReport);
		int day = this.todaysReport.day;
		ManagementMenuNotification notification = new ManagementMenuNotification(global::Action.ManageReport, NotificationValence.Good, null, string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TITLE, day), NotificationType.Good, (List<Notification> n, object d) => string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TOOLTIP, day), null, true, 0f, delegate(object d)
		{
			ManagementMenu.Instance.OpenReports(day);
		}, null, null, true);
		if (this.notifier == null)
		{
			global::Debug.LogError("Cant notify, null notifier");
		}
		else
		{
			this.notifier.Add(notification, "");
		}
		this.todaysReport = new ReportManager.DailyReport(this);
		this.todaysReport.day = GameUtil.GetCurrentCycle() + 1;
	}

	// Token: 0x0600259C RID: 9628 RVA: 0x000D7A20 File Offset: 0x000D5C20
	public ReportManager.DailyReport FindReport(int day)
	{
		foreach (ReportManager.DailyReport dailyReport in this.dailyReports)
		{
			if (dailyReport.day == day)
			{
				return dailyReport;
			}
		}
		if (this.todaysReport.day == day)
		{
			return this.todaysReport;
		}
		return null;
	}

	// Token: 0x0600259D RID: 9629 RVA: 0x000D7A94 File Offset: 0x000D5C94
	public ReportManager()
	{
		Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> dictionary = new Dictionary<ReportManager.ReportType, ReportManager.ReportGroup>();
		dictionary.Add(ReportManager.ReportType.DuplicantHeader, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.DUPLICANT_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.CaloriesCreated, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedCalories(v, GameUtil.TimeSlice.None, true), true, 1, UI.ENDOFDAYREPORT.CALORIES_CREATED.NAME, UI.ENDOFDAYREPORT.CALORIES_CREATED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CALORIES_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.StressDelta, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v, GameUtil.TimeSlice.None), true, 1, UI.ENDOFDAYREPORT.STRESS_DELTA.NAME, UI.ENDOFDAYREPORT.STRESS_DELTA.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.STRESS_DELTA.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DiseaseAdded, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.DISEASE_ADDED.NAME, UI.ENDOFDAYREPORT.DISEASE_ADDED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.DISEASE_ADDED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DiseaseStatus, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedDiseaseAmount((int)v, GameUtil.TimeSlice.None), true, 1, UI.ENDOFDAYREPORT.DISEASE_STATUS.NAME, UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP, UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.LevelUp, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.LEVEL_UP.NAME, UI.ENDOFDAYREPORT.LEVEL_UP.TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ToiletIncident, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.TOILET_INCIDENT.NAME, UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP, UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ChoreStatus, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.CHORE_STATUS.NAME, UI.ENDOFDAYREPORT.CHORE_STATUS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CHORE_STATUS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DomesticatedCritters, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NAME, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.WildCritters, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NAME, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.RocketsInFlight, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NAME, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.TimeSpentHeader, new ReportManager.ReportGroup(null, true, 2, UI.ENDOFDAYREPORT.TIME_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.WorkTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.WORK_TIME.NAME, UI.ENDOFDAYREPORT.WORK_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.TravelTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.TRAVEL_TIME.NAME, UI.ENDOFDAYREPORT.TRAVEL_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.PersonalTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.PERSONAL_TIME.NAME, UI.ENDOFDAYREPORT.PERSONAL_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.IdleTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.IDLE_TIME.NAME, UI.ENDOFDAYREPORT.IDLE_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.BaseHeader, new ReportManager.ReportGroup(null, true, 3, UI.ENDOFDAYREPORT.BASE_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.OxygenCreated, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), true, 3, UI.ENDOFDAYREPORT.OXYGEN_CREATED.NAME, UI.ENDOFDAYREPORT.OXYGEN_CREATED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.OXYGEN_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.EnergyCreated, new ReportManager.ReportGroup(new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules), true, 3, UI.ENDOFDAYREPORT.ENERGY_USAGE.NAME, UI.ENDOFDAYREPORT.ENERGY_USAGE.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.ENERGY_USAGE.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.EnergyWasted, new ReportManager.ReportGroup(new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules), true, 3, UI.ENDOFDAYREPORT.ENERGY_WASTED.NAME, UI.ENDOFDAYREPORT.NONE, UI.ENDOFDAYREPORT.ENERGY_WASTED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ContaminatedOxygenToilet, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), false, 3, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NAME, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ContaminatedOxygenSublimation, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), false, 3, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NAME, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		this.ReportGroups = dictionary;
		this.dailyReports = new List<ReportManager.DailyReport>();
		base..ctor();
	}

	// Token: 0x04001602 RID: 5634
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04001603 RID: 5635
	private ReportManager.NoteStorage noteStorage;

	// Token: 0x04001604 RID: 5636
	public Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> ReportGroups;

	// Token: 0x04001605 RID: 5637
	[Serialize]
	private List<ReportManager.DailyReport> dailyReports;

	// Token: 0x04001606 RID: 5638
	[Serialize]
	private ReportManager.DailyReport todaysReport;

	// Token: 0x04001607 RID: 5639
	[Serialize]
	private byte[] noteStorageBytes;

	// Token: 0x020014FD RID: 5373
	// (Invoke) Token: 0x060091C3 RID: 37315
	public delegate string FormattingFn(float v);

	// Token: 0x020014FE RID: 5374
	// (Invoke) Token: 0x060091C7 RID: 37319
	public delegate string GroupFormattingFn(float v, float numEntries);

	// Token: 0x020014FF RID: 5375
	public enum ReportType
	{
		// Token: 0x04007029 RID: 28713
		DuplicantHeader,
		// Token: 0x0400702A RID: 28714
		CaloriesCreated,
		// Token: 0x0400702B RID: 28715
		StressDelta,
		// Token: 0x0400702C RID: 28716
		LevelUp,
		// Token: 0x0400702D RID: 28717
		DiseaseStatus,
		// Token: 0x0400702E RID: 28718
		DiseaseAdded,
		// Token: 0x0400702F RID: 28719
		ToiletIncident,
		// Token: 0x04007030 RID: 28720
		ChoreStatus,
		// Token: 0x04007031 RID: 28721
		TimeSpentHeader,
		// Token: 0x04007032 RID: 28722
		TimeSpent,
		// Token: 0x04007033 RID: 28723
		WorkTime,
		// Token: 0x04007034 RID: 28724
		TravelTime,
		// Token: 0x04007035 RID: 28725
		PersonalTime,
		// Token: 0x04007036 RID: 28726
		IdleTime,
		// Token: 0x04007037 RID: 28727
		BaseHeader,
		// Token: 0x04007038 RID: 28728
		ContaminatedOxygenFlatulence,
		// Token: 0x04007039 RID: 28729
		ContaminatedOxygenToilet,
		// Token: 0x0400703A RID: 28730
		ContaminatedOxygenSublimation,
		// Token: 0x0400703B RID: 28731
		OxygenCreated,
		// Token: 0x0400703C RID: 28732
		EnergyCreated,
		// Token: 0x0400703D RID: 28733
		EnergyWasted,
		// Token: 0x0400703E RID: 28734
		DomesticatedCritters,
		// Token: 0x0400703F RID: 28735
		WildCritters,
		// Token: 0x04007040 RID: 28736
		RocketsInFlight
	}

	// Token: 0x02001500 RID: 5376
	public struct ReportGroup
	{
		// Token: 0x060091CA RID: 37322 RVA: 0x00372100 File Offset: 0x00370300
		public ReportGroup(ReportManager.FormattingFn formatfn, bool reportIfZero, int group, string stringKey, string positiveTooltip, string negativeTooltip, ReportManager.ReportEntry.Order pos_note_order = ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order neg_note_order = ReportManager.ReportEntry.Order.Unordered, bool is_header = false, ReportManager.GroupFormattingFn group_format_fn = null)
		{
			ReportManager.FormattingFn formattingFn;
			if (formatfn == null)
			{
				formattingFn = ((float v) => v.ToString());
			}
			else
			{
				formattingFn = formatfn;
			}
			this.formatfn = formattingFn;
			this.groupFormatfn = group_format_fn;
			this.stringKey = stringKey;
			this.positiveTooltip = positiveTooltip;
			this.negativeTooltip = negativeTooltip;
			this.reportIfZero = reportIfZero;
			this.group = group;
			this.posNoteOrder = pos_note_order;
			this.negNoteOrder = neg_note_order;
			this.isHeader = is_header;
		}

		// Token: 0x04007041 RID: 28737
		public ReportManager.FormattingFn formatfn;

		// Token: 0x04007042 RID: 28738
		public ReportManager.GroupFormattingFn groupFormatfn;

		// Token: 0x04007043 RID: 28739
		public string stringKey;

		// Token: 0x04007044 RID: 28740
		public string positiveTooltip;

		// Token: 0x04007045 RID: 28741
		public string negativeTooltip;

		// Token: 0x04007046 RID: 28742
		public bool reportIfZero;

		// Token: 0x04007047 RID: 28743
		public int group;

		// Token: 0x04007048 RID: 28744
		public bool isHeader;

		// Token: 0x04007049 RID: 28745
		public ReportManager.ReportEntry.Order posNoteOrder;

		// Token: 0x0400704A RID: 28746
		public ReportManager.ReportEntry.Order negNoteOrder;
	}

	// Token: 0x02001501 RID: 5377
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ReportEntry
	{
		// Token: 0x060091CB RID: 37323 RVA: 0x00372180 File Offset: 0x00370380
		public ReportEntry(ReportManager.ReportType reportType, int note_storage_id, string context, bool is_child = false)
		{
			this.reportType = reportType;
			this.context = context;
			this.isChild = is_child;
			this.accumulate = 0f;
			this.accPositive = 0f;
			this.accNegative = 0f;
			this.noteStorageId = note_storage_id;
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x060091CC RID: 37324 RVA: 0x003721D8 File Offset: 0x003703D8
		public float Positive
		{
			get
			{
				return this.accPositive;
			}
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x060091CD RID: 37325 RVA: 0x003721E0 File Offset: 0x003703E0
		public float Negative
		{
			get
			{
				return this.accNegative;
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060091CE RID: 37326 RVA: 0x003721E8 File Offset: 0x003703E8
		public float Net
		{
			get
			{
				return this.accPositive + this.accNegative;
			}
		}

		// Token: 0x060091CF RID: 37327 RVA: 0x003721F7 File Offset: 0x003703F7
		[OnDeserializing]
		private void OnDeserialize()
		{
			this.contextEntries.Clear();
		}

		// Token: 0x060091D0 RID: 37328 RVA: 0x00372204 File Offset: 0x00370404
		public void IterateNotes(Action<ReportManager.ReportEntry.Note> callback)
		{
			ReportManager.Instance.noteStorage.IterateNotes(this.noteStorageId, callback);
		}

		// Token: 0x060091D1 RID: 37329 RVA: 0x0037221C File Offset: 0x0037041C
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (this.gameHash != -1)
			{
				this.reportType = (ReportManager.ReportType)this.gameHash;
				this.gameHash = -1;
			}
		}

		// Token: 0x060091D2 RID: 37330 RVA: 0x0037223C File Offset: 0x0037043C
		public void AddData(ReportManager.NoteStorage note_storage, float value, string note = null, string dataContext = null)
		{
			this.AddActualData(note_storage, value, note);
			if (dataContext != null)
			{
				ReportManager.ReportEntry reportEntry = null;
				for (int i = 0; i < this.contextEntries.Count; i++)
				{
					if (this.contextEntries[i].context == dataContext)
					{
						reportEntry = this.contextEntries[i];
						break;
					}
				}
				if (reportEntry == null)
				{
					reportEntry = new ReportManager.ReportEntry(this.reportType, note_storage.GetNewNoteId(), dataContext, true);
					this.contextEntries.Add(reportEntry);
				}
				reportEntry.AddActualData(note_storage, value, note);
			}
		}

		// Token: 0x060091D3 RID: 37331 RVA: 0x003722C8 File Offset: 0x003704C8
		private void AddActualData(ReportManager.NoteStorage note_storage, float value, string note = null)
		{
			this.accumulate += value;
			if (value > 0f)
			{
				this.accPositive += value;
			}
			else
			{
				this.accNegative += value;
			}
			if (note != null)
			{
				note_storage.Add(this.noteStorageId, value, note);
			}
		}

		// Token: 0x060091D4 RID: 37332 RVA: 0x0037231A File Offset: 0x0037051A
		public bool HasContextEntries()
		{
			return this.contextEntries.Count > 0;
		}

		// Token: 0x0400704B RID: 28747
		[Serialize]
		public int noteStorageId;

		// Token: 0x0400704C RID: 28748
		[Serialize]
		public int gameHash = -1;

		// Token: 0x0400704D RID: 28749
		[Serialize]
		public ReportManager.ReportType reportType;

		// Token: 0x0400704E RID: 28750
		[Serialize]
		public string context;

		// Token: 0x0400704F RID: 28751
		[Serialize]
		public float accumulate;

		// Token: 0x04007050 RID: 28752
		[Serialize]
		public float accPositive;

		// Token: 0x04007051 RID: 28753
		[Serialize]
		public float accNegative;

		// Token: 0x04007052 RID: 28754
		[Serialize]
		public ArrayRef<ReportManager.ReportEntry> contextEntries;

		// Token: 0x04007053 RID: 28755
		public bool isChild;

		// Token: 0x020028AC RID: 10412
		public struct Note
		{
			// Token: 0x0600CCDC RID: 52444 RVA: 0x00430573 File Offset: 0x0042E773
			public Note(float value, string note)
			{
				this.value = value;
				this.note = note;
			}

			// Token: 0x0400B32A RID: 45866
			public float value;

			// Token: 0x0400B32B RID: 45867
			public string note;
		}

		// Token: 0x020028AD RID: 10413
		public enum Order
		{
			// Token: 0x0400B32D RID: 45869
			Unordered,
			// Token: 0x0400B32E RID: 45870
			Ascending,
			// Token: 0x0400B32F RID: 45871
			Descending
		}
	}

	// Token: 0x02001502 RID: 5378
	public class DailyReport
	{
		// Token: 0x060091D5 RID: 37333 RVA: 0x0037232C File Offset: 0x0037052C
		public DailyReport(ReportManager manager)
		{
			foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> keyValuePair in manager.ReportGroups)
			{
				this.reportEntries.Add(new ReportManager.ReportEntry(keyValuePair.Key, this.noteStorage.GetNewNoteId(), null, false));
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060091D6 RID: 37334 RVA: 0x003723B0 File Offset: 0x003705B0
		private ReportManager.NoteStorage noteStorage
		{
			get
			{
				return ReportManager.Instance.noteStorage;
			}
		}

		// Token: 0x060091D7 RID: 37335 RVA: 0x003723BC File Offset: 0x003705BC
		public ReportManager.ReportEntry GetEntry(ReportManager.ReportType reportType)
		{
			for (int i = 0; i < this.reportEntries.Count; i++)
			{
				ReportManager.ReportEntry reportEntry = this.reportEntries[i];
				if (reportEntry.reportType == reportType)
				{
					return reportEntry;
				}
			}
			ReportManager.ReportEntry reportEntry2 = new ReportManager.ReportEntry(reportType, this.noteStorage.GetNewNoteId(), null, false);
			this.reportEntries.Add(reportEntry2);
			return reportEntry2;
		}

		// Token: 0x060091D8 RID: 37336 RVA: 0x00372418 File Offset: 0x00370618
		public void AddData(ReportManager.ReportType reportType, float value, string note = null, string context = null)
		{
			this.GetEntry(reportType).AddData(this.noteStorage, value, note, context);
		}

		// Token: 0x04007054 RID: 28756
		[Serialize]
		public int day;

		// Token: 0x04007055 RID: 28757
		[Serialize]
		public List<ReportManager.ReportEntry> reportEntries = new List<ReportManager.ReportEntry>();
	}

	// Token: 0x02001503 RID: 5379
	public class NoteStorage
	{
		// Token: 0x060091D9 RID: 37337 RVA: 0x00372430 File Offset: 0x00370630
		public NoteStorage()
		{
			this.noteEntries = new ReportManager.NoteStorage.NoteEntries();
			this.stringTable = new ReportManager.NoteStorage.StringTable();
		}

		// Token: 0x060091DA RID: 37338 RVA: 0x00372450 File Offset: 0x00370650
		public void Add(int report_entry_id, float value, string note)
		{
			int note_id = this.stringTable.AddString(note, 6);
			this.noteEntries.Add(report_entry_id, value, note_id);
		}

		// Token: 0x060091DB RID: 37339 RVA: 0x0037247C File Offset: 0x0037067C
		public int GetNewNoteId()
		{
			int result = this.nextNoteId + 1;
			this.nextNoteId = result;
			return result;
		}

		// Token: 0x060091DC RID: 37340 RVA: 0x0037249A File Offset: 0x0037069A
		public void IterateNotes(int report_entry_id, Action<ReportManager.ReportEntry.Note> callback)
		{
			this.noteEntries.IterateNotes(this.stringTable, report_entry_id, callback);
		}

		// Token: 0x060091DD RID: 37341 RVA: 0x003724AF File Offset: 0x003706AF
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(6);
			writer.Write(this.nextNoteId);
			this.stringTable.Serialize(writer);
			this.noteEntries.Serialize(writer);
		}

		// Token: 0x060091DE RID: 37342 RVA: 0x003724DC File Offset: 0x003706DC
		public void Deserialize(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			if (num < 5)
			{
				return;
			}
			this.nextNoteId = reader.ReadInt32();
			this.stringTable.Deserialize(reader, num);
			this.noteEntries.Deserialize(reader, num);
		}

		// Token: 0x04007056 RID: 28758
		public const int SERIALIZATION_VERSION = 6;

		// Token: 0x04007057 RID: 28759
		private int nextNoteId;

		// Token: 0x04007058 RID: 28760
		private ReportManager.NoteStorage.NoteEntries noteEntries;

		// Token: 0x04007059 RID: 28761
		private ReportManager.NoteStorage.StringTable stringTable;

		// Token: 0x020028AE RID: 10414
		private class StringTable
		{
			// Token: 0x0600CCDD RID: 52445 RVA: 0x00430584 File Offset: 0x0042E784
			public int AddString(string str, int version = 6)
			{
				int num = Hash.SDBMLower(str);
				this.strings[num] = str;
				return num;
			}

			// Token: 0x0600CCDE RID: 52446 RVA: 0x004305A8 File Offset: 0x0042E7A8
			public string GetStringByHash(int hash)
			{
				string result = "";
				this.strings.TryGetValue(hash, out result);
				return result;
			}

			// Token: 0x0600CCDF RID: 52447 RVA: 0x004305CC File Offset: 0x0042E7CC
			public void Serialize(BinaryWriter writer)
			{
				writer.Write(this.strings.Count);
				foreach (KeyValuePair<int, string> keyValuePair in this.strings)
				{
					writer.Write(keyValuePair.Value);
				}
			}

			// Token: 0x0600CCE0 RID: 52448 RVA: 0x00430638 File Offset: 0x0042E838
			public void Deserialize(BinaryReader reader, int version)
			{
				int num = reader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					string str = reader.ReadString();
					this.AddString(str, version);
				}
			}

			// Token: 0x0400B330 RID: 45872
			private Dictionary<int, string> strings = new Dictionary<int, string>();
		}

		// Token: 0x020028AF RID: 10415
		private class NoteEntries
		{
			// Token: 0x0600CCE2 RID: 52450 RVA: 0x0043067C File Offset: 0x0042E87C
			public void Add(int report_entry_id, float value, int note_id)
			{
				Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary;
				if (!this.entries.TryGetValue(report_entry_id, out dictionary))
				{
					dictionary = new Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>(ReportManager.NoteStorage.NoteEntries.sKeyComparer);
					this.entries[report_entry_id] = dictionary;
				}
				ReportManager.NoteStorage.NoteEntries.NoteEntryKey noteEntryKey = new ReportManager.NoteStorage.NoteEntries.NoteEntryKey
				{
					noteHash = note_id,
					isPositive = (value > 0f)
				};
				if (dictionary.ContainsKey(noteEntryKey))
				{
					Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary2 = dictionary;
					ReportManager.NoteStorage.NoteEntries.NoteEntryKey key = noteEntryKey;
					dictionary2[key] += value;
					return;
				}
				dictionary[noteEntryKey] = value;
			}

			// Token: 0x0600CCE3 RID: 52451 RVA: 0x004306F8 File Offset: 0x0042E8F8
			public void Serialize(BinaryWriter writer)
			{
				writer.Write(this.entries.Count);
				foreach (KeyValuePair<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>> keyValuePair in this.entries)
				{
					writer.Write(keyValuePair.Key);
					writer.Write(keyValuePair.Value.Count);
					foreach (KeyValuePair<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> keyValuePair2 in keyValuePair.Value)
					{
						writer.Write(keyValuePair2.Key.noteHash);
						writer.Write(keyValuePair2.Key.isPositive);
						writer.WriteSingleFast(keyValuePair2.Value);
					}
				}
			}

			// Token: 0x0600CCE4 RID: 52452 RVA: 0x004307E8 File Offset: 0x0042E9E8
			public void Deserialize(BinaryReader reader, int version)
			{
				if (version < 6)
				{
					OldNoteEntriesV5 oldNoteEntriesV = new OldNoteEntriesV5();
					oldNoteEntriesV.Deserialize(reader);
					foreach (OldNoteEntriesV5.NoteStorageBlock noteStorageBlock in oldNoteEntriesV.storageBlocks)
					{
						for (int i = 0; i < noteStorageBlock.entryCount; i++)
						{
							OldNoteEntriesV5.NoteEntry noteEntry = noteStorageBlock.entries.structs[i];
							this.Add(noteEntry.reportEntryId, noteEntry.value, noteEntry.noteHash);
						}
					}
					return;
				}
				int num = reader.ReadInt32();
				this.entries = new Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>>(num);
				for (int j = 0; j < num; j++)
				{
					int key = reader.ReadInt32();
					int num2 = reader.ReadInt32();
					Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary = new Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>(num2, ReportManager.NoteStorage.NoteEntries.sKeyComparer);
					this.entries[key] = dictionary;
					for (int k = 0; k < num2; k++)
					{
						ReportManager.NoteStorage.NoteEntries.NoteEntryKey key2 = new ReportManager.NoteStorage.NoteEntries.NoteEntryKey
						{
							noteHash = reader.ReadInt32(),
							isPositive = reader.ReadBoolean()
						};
						dictionary[key2] = reader.ReadSingle();
					}
				}
			}

			// Token: 0x0600CCE5 RID: 52453 RVA: 0x0043091C File Offset: 0x0042EB1C
			public void IterateNotes(ReportManager.NoteStorage.StringTable string_table, int report_entry_id, Action<ReportManager.ReportEntry.Note> callback)
			{
				Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary;
				if (this.entries.TryGetValue(report_entry_id, out dictionary))
				{
					foreach (KeyValuePair<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> keyValuePair in dictionary)
					{
						string stringByHash = string_table.GetStringByHash(keyValuePair.Key.noteHash);
						ReportManager.ReportEntry.Note obj = new ReportManager.ReportEntry.Note(keyValuePair.Value, stringByHash);
						callback(obj);
					}
				}
			}

			// Token: 0x0400B331 RID: 45873
			private static ReportManager.NoteStorage.NoteEntries.NoteEntryKeyComparer sKeyComparer = new ReportManager.NoteStorage.NoteEntries.NoteEntryKeyComparer();

			// Token: 0x0400B332 RID: 45874
			private Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>> entries = new Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>>();

			// Token: 0x02003A43 RID: 14915
			public struct NoteEntryKey
			{
				// Token: 0x0400EB72 RID: 60274
				public int noteHash;

				// Token: 0x0400EB73 RID: 60275
				public bool isPositive;
			}

			// Token: 0x02003A44 RID: 14916
			public class NoteEntryKeyComparer : IEqualityComparer<ReportManager.NoteStorage.NoteEntries.NoteEntryKey>
			{
				// Token: 0x0600F3DA RID: 62426 RVA: 0x004958A9 File Offset: 0x00493AA9
				public bool Equals(ReportManager.NoteStorage.NoteEntries.NoteEntryKey a, ReportManager.NoteStorage.NoteEntries.NoteEntryKey b)
				{
					return a.noteHash == b.noteHash && a.isPositive == b.isPositive;
				}

				// Token: 0x0600F3DB RID: 62427 RVA: 0x004958C9 File Offset: 0x00493AC9
				public int GetHashCode(ReportManager.NoteStorage.NoteEntries.NoteEntryKey a)
				{
					return a.noteHash * (a.isPositive ? 1 : -1);
				}
			}
		}
	}
}
