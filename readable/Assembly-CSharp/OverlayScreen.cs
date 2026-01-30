using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000C6C RID: 3180
[AddComponentMenu("KMonoBehaviour/scripts/OverlayScreen")]
public class OverlayScreen : KMonoBehaviour
{
	// Token: 0x17000706 RID: 1798
	// (get) Token: 0x060060EB RID: 24811 RVA: 0x0023A73A File Offset: 0x0023893A
	public HashedString mode
	{
		get
		{
			return this.currentModeInfo.mode.ViewMode();
		}
	}

	// Token: 0x060060EC RID: 24812 RVA: 0x0023A74C File Offset: 0x0023894C
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(OverlayScreen.Instance == null);
		OverlayScreen.Instance = this;
		this.powerLabelParent = GameObject.Find("WorldSpaceCanvas").GetComponent<Canvas>();
	}

	// Token: 0x060060ED RID: 24813 RVA: 0x0023A779 File Offset: 0x00238979
	protected override void OnLoadLevel()
	{
		this.harvestableNotificationPrefab = null;
		this.powerLabelParent = null;
		OverlayScreen.Instance = null;
		OverlayModes.Mode.Clear();
		this.modeInfos = null;
		this.currentModeInfo = default(OverlayScreen.ModeInfo);
		base.OnLoadLevel();
	}

	// Token: 0x060060EE RID: 24814 RVA: 0x0023A7B0 File Offset: 0x002389B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.techViewSound = KFMOD.CreateInstance(this.techViewSoundPath);
		this.techViewSoundPlaying = false;
		Shader.SetGlobalVector("_OverlayParams", Vector4.zero);
		this.RegisterModes();
		this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
	}

	// Token: 0x060060EF RID: 24815 RVA: 0x0023A808 File Offset: 0x00238A08
	private void RegisterModes()
	{
		this.modeInfos.Clear();
		OverlayModes.None mode = new OverlayModes.None();
		this.RegisterMode(mode);
		this.RegisterMode(new OverlayModes.Oxygen());
		this.RegisterMode(new OverlayModes.Power(this.powerLabelParent, this.powerLabelPrefab, this.batUIPrefab, this.powerLabelOffset, this.batteryUIOffset, this.batteryUITransformerOffset, this.batteryUISmallTransformerOffset));
		this.RegisterMode(new OverlayModes.Temperature());
		this.RegisterMode(new OverlayModes.ThermalConductivity());
		this.RegisterMode(new OverlayModes.Light());
		this.RegisterMode(new OverlayModes.LiquidConduits());
		this.RegisterMode(new OverlayModes.GasConduits());
		this.RegisterMode(new OverlayModes.Decor());
		this.RegisterMode(new OverlayModes.Disease(this.powerLabelParent, this.diseaseOverlayPrefab));
		this.RegisterMode(new OverlayModes.Crop(this.powerLabelParent, this.harvestableNotificationPrefab));
		this.RegisterMode(new OverlayModes.Harvest());
		this.RegisterMode(new OverlayModes.Priorities());
		this.RegisterMode(new OverlayModes.HeatFlow());
		this.RegisterMode(new OverlayModes.Rooms());
		this.RegisterMode(new OverlayModes.Suit(this.powerLabelParent, this.suitOverlayPrefab));
		this.RegisterMode(new OverlayModes.Logic(this.logicModeUIPrefab));
		this.RegisterMode(new OverlayModes.SolidConveyor());
		this.RegisterMode(new OverlayModes.TileMode());
		this.RegisterMode(new OverlayModes.Radiation());
	}

	// Token: 0x060060F0 RID: 24816 RVA: 0x0023A954 File Offset: 0x00238B54
	private void RegisterMode(OverlayModes.Mode mode)
	{
		this.modeInfos[mode.ViewMode()] = new OverlayScreen.ModeInfo
		{
			mode = mode
		};
	}

	// Token: 0x060060F1 RID: 24817 RVA: 0x0023A983 File Offset: 0x00238B83
	private void LateUpdate()
	{
		this.currentModeInfo.mode.Update();
	}

	// Token: 0x060060F2 RID: 24818 RVA: 0x0023A998 File Offset: 0x00238B98
	public void ToggleOverlay(HashedString newMode, bool allowSound = true)
	{
		bool flag = allowSound && !(this.currentModeInfo.mode.ViewMode() == newMode);
		if (newMode != OverlayModes.None.ID)
		{
			ManagementMenu.Instance.CloseAll();
		}
		this.currentModeInfo.mode.Disable();
		if (newMode != this.currentModeInfo.mode.ViewMode() && newMode == OverlayModes.None.ID)
		{
			ManagementMenu.Instance.CloseAll();
		}
		SimDebugView.Instance.SetMode(newMode);
		if (!this.modeInfos.TryGetValue(newMode, out this.currentModeInfo))
		{
			this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
		}
		this.currentModeInfo.mode.Enable();
		if (flag)
		{
			this.UpdateOverlaySounds();
		}
		if (OverlayModes.None.ID == this.currentModeInfo.mode.ViewMode())
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterOnMigrated, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.SetDynamicMusicOverlayInactive();
			this.techViewSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this.techViewSoundPlaying = false;
		}
		else if (!this.techViewSoundPlaying)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterOnMigrated);
			MusicManager.instance.SetDynamicMusicOverlayActive();
			this.techViewSound.start();
			this.techViewSoundPlaying = true;
		}
		if (this.OnOverlayChanged != null)
		{
			this.OnOverlayChanged(this.currentModeInfo.mode.ViewMode());
		}
		this.ActivateLegend();
	}

	// Token: 0x060060F3 RID: 24819 RVA: 0x0023AB1F File Offset: 0x00238D1F
	private void ActivateLegend()
	{
		if (OverlayLegend.Instance == null)
		{
			return;
		}
		OverlayLegend.Instance.SetLegend(this.currentModeInfo.mode, false);
	}

	// Token: 0x060060F4 RID: 24820 RVA: 0x0023AB45 File Offset: 0x00238D45
	public void Refresh()
	{
		this.LateUpdate();
	}

	// Token: 0x060060F5 RID: 24821 RVA: 0x0023AB4D File Offset: 0x00238D4D
	public HashedString GetMode()
	{
		if (this.currentModeInfo.mode == null)
		{
			return OverlayModes.None.ID;
		}
		return this.currentModeInfo.mode.ViewMode();
	}

	// Token: 0x060060F6 RID: 24822 RVA: 0x0023AB74 File Offset: 0x00238D74
	private void UpdateOverlaySounds()
	{
		string text = this.currentModeInfo.mode.GetSoundName();
		if (text != "")
		{
			text = GlobalAssets.GetSound(text, false);
			KMonoBehaviour.PlaySound(text);
		}
	}

	// Token: 0x040040C5 RID: 16581
	public static HashSet<Tag> WireIDs = new HashSet<Tag>();

	// Token: 0x040040C6 RID: 16582
	public static HashSet<Tag> GasVentIDs = new HashSet<Tag>();

	// Token: 0x040040C7 RID: 16583
	public static HashSet<Tag> LiquidVentIDs = new HashSet<Tag>();

	// Token: 0x040040C8 RID: 16584
	public static HashSet<Tag> HarvestableIDs = new HashSet<Tag>();

	// Token: 0x040040C9 RID: 16585
	public static HashSet<Tag> DiseaseIDs = new HashSet<Tag>();

	// Token: 0x040040CA RID: 16586
	public static HashSet<Tag> SuitIDs = new HashSet<Tag>();

	// Token: 0x040040CB RID: 16587
	public static HashSet<Tag> SolidConveyorIDs = new HashSet<Tag>();

	// Token: 0x040040CC RID: 16588
	public static HashSet<Tag> RadiationIDs = new HashSet<Tag>();

	// Token: 0x040040CD RID: 16589
	[SerializeField]
	public EventReference techViewSoundPath;

	// Token: 0x040040CE RID: 16590
	private EventInstance techViewSound;

	// Token: 0x040040CF RID: 16591
	private bool techViewSoundPlaying;

	// Token: 0x040040D0 RID: 16592
	public static OverlayScreen Instance;

	// Token: 0x040040D1 RID: 16593
	[Header("Power")]
	[SerializeField]
	private Canvas powerLabelParent;

	// Token: 0x040040D2 RID: 16594
	[SerializeField]
	private LocText powerLabelPrefab;

	// Token: 0x040040D3 RID: 16595
	[SerializeField]
	private BatteryUI batUIPrefab;

	// Token: 0x040040D4 RID: 16596
	[SerializeField]
	private Vector3 powerLabelOffset;

	// Token: 0x040040D5 RID: 16597
	[SerializeField]
	private Vector3 batteryUIOffset;

	// Token: 0x040040D6 RID: 16598
	[SerializeField]
	private Vector3 batteryUITransformerOffset;

	// Token: 0x040040D7 RID: 16599
	[SerializeField]
	private Vector3 batteryUISmallTransformerOffset;

	// Token: 0x040040D8 RID: 16600
	[SerializeField]
	private Color consumerColour;

	// Token: 0x040040D9 RID: 16601
	[SerializeField]
	private Color generatorColour;

	// Token: 0x040040DA RID: 16602
	[SerializeField]
	private Color buildingDisabledColour = Color.gray;

	// Token: 0x040040DB RID: 16603
	[Header("Circuits")]
	[SerializeField]
	private Color32 circuitUnpoweredColour;

	// Token: 0x040040DC RID: 16604
	[SerializeField]
	private Color32 circuitSafeColour;

	// Token: 0x040040DD RID: 16605
	[SerializeField]
	private Color32 circuitStrainingColour;

	// Token: 0x040040DE RID: 16606
	[SerializeField]
	private Color32 circuitOverloadingColour;

	// Token: 0x040040DF RID: 16607
	[Header("Crops")]
	[SerializeField]
	private GameObject harvestableNotificationPrefab;

	// Token: 0x040040E0 RID: 16608
	[Header("Disease")]
	[SerializeField]
	private GameObject diseaseOverlayPrefab;

	// Token: 0x040040E1 RID: 16609
	[Header("Suit")]
	[SerializeField]
	private GameObject suitOverlayPrefab;

	// Token: 0x040040E2 RID: 16610
	[Header("ToolTip")]
	[SerializeField]
	private TextStyleSetting TooltipHeader;

	// Token: 0x040040E3 RID: 16611
	[SerializeField]
	private TextStyleSetting TooltipDescription;

	// Token: 0x040040E4 RID: 16612
	[Header("Logic")]
	[SerializeField]
	private LogicModeUI logicModeUIPrefab;

	// Token: 0x040040E5 RID: 16613
	public Action<HashedString> OnOverlayChanged;

	// Token: 0x040040E6 RID: 16614
	private OverlayScreen.ModeInfo currentModeInfo;

	// Token: 0x040040E7 RID: 16615
	private Dictionary<HashedString, OverlayScreen.ModeInfo> modeInfos = new Dictionary<HashedString, OverlayScreen.ModeInfo>();

	// Token: 0x02001E40 RID: 7744
	private struct ModeInfo
	{
		// Token: 0x04008E16 RID: 36374
		public OverlayModes.Mode mode;
	}
}
