using System;
using System.Collections.Generic;
using System.Threading;
using Klei.CustomSettings;
using ProcGenGame;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000ED9 RID: 3801
[AddComponentMenu("KMonoBehaviour/scripts/OfflineWorldGen")]
public class OfflineWorldGen : KMonoBehaviour
{
	// Token: 0x060079A3 RID: 31139 RVA: 0x002EC9CC File Offset: 0x002EABCC
	private void TrackProgress(string text)
	{
		if (this.trackProgress)
		{
			global::Debug.Log(text);
		}
	}

	// Token: 0x060079A4 RID: 31140 RVA: 0x002EC9DC File Offset: 0x002EABDC
	public static bool CanLoadSave()
	{
		bool flag = WorldGen.CanLoad(SaveLoader.GetActiveSaveFilePath());
		if (!flag)
		{
			SaveLoader.SetActiveSaveFilePath(null);
			flag = WorldGen.CanLoad(WorldGen.WORLDGEN_SAVE_FILENAME);
		}
		return flag;
	}

	// Token: 0x060079A5 RID: 31141 RVA: 0x002ECA0C File Offset: 0x002EAC0C
	public void Generate()
	{
		this.doWorldGen = !OfflineWorldGen.CanLoadSave();
		this.updateText.gameObject.SetActive(false);
		this.percentText.gameObject.SetActive(false);
		this.doWorldGen |= this.debug;
		if (this.doWorldGen)
		{
			this.seedText.text = string.Format(UI.WORLDGEN.USING_PLAYER_SEED, this.seed);
			this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.TITLE.ToString();
			this.mainText.text = UI.WORLDGEN.CHOOSEWORLDSIZE.ToString();
			for (int i = 0; i < this.validDimensions.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
				gameObject.SetActive(true);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.SetParent(this.buttonRoot);
				component.localScale = Vector3.one;
				TMP_Text componentInChildren = gameObject.GetComponentInChildren<LocText>();
				OfflineWorldGen.ValidDimensions validDimensions = this.validDimensions[i];
				componentInChildren.text = validDimensions.name.ToString();
				int idx = i;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.DoWorldGen(idx);
					this.ToggleGenerationUI();
				};
			}
			if (this.validDimensions.Length == 1)
			{
				this.DoWorldGen(0);
				this.ToggleGenerationUI();
			}
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
			this.OnResize();
		}
		else
		{
			this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.LOADINGGAME.ToString();
			this.mainText.gameObject.SetActive(false);
			this.currentConvertedCurrentStage = UI.WORLDGEN.COMPLETE.key;
			this.currentPercent = 1f;
			this.updateText.gameObject.SetActive(false);
			this.percentText.gameObject.SetActive(false);
			this.RemoveButtons();
		}
		this.buttonPrefab.SetActive(false);
	}

	// Token: 0x060079A6 RID: 31142 RVA: 0x002ECC0C File Offset: 0x002EAE0C
	private void OnResize()
	{
		float canvasScale = base.GetComponentInParent<KCanvasScaler>().GetCanvasScale();
		if (this.asteriodAnim != null)
		{
			this.asteriodAnim.animScale = 0.005f * (1f / canvasScale);
		}
	}

	// Token: 0x060079A7 RID: 31143 RVA: 0x002ECC54 File Offset: 0x002EAE54
	private void ToggleGenerationUI()
	{
		this.percentText.gameObject.SetActive(false);
		this.updateText.gameObject.SetActive(true);
		this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.GENERATINGWORLD.ToString();
		if (this.titleText != null && this.titleText.gameObject != null)
		{
			this.titleText.gameObject.SetActive(false);
		}
		if (this.buttonRoot != null && this.buttonRoot.gameObject != null)
		{
			this.buttonRoot.gameObject.SetActive(false);
		}
	}

	// Token: 0x060079A8 RID: 31144 RVA: 0x002ECCFC File Offset: 0x002EAEFC
	private bool UpdateProgress(StringKey stringKeyRoot, float completePercent, WorldGenProgressStages.Stages stage)
	{
		if (this.currentStage != stage)
		{
			this.currentStage = stage;
		}
		if (this.currentStringKeyRoot.Hash != stringKeyRoot.Hash)
		{
			this.currentConvertedCurrentStage = stringKeyRoot;
			this.currentStringKeyRoot = stringKeyRoot;
		}
		else
		{
			int num = (int)completePercent * 10;
			LocString locString = this.convertList.Find((LocString s) => s.key.Hash == stringKeyRoot.Hash);
			if (num != 0 && locString != null)
			{
				this.currentConvertedCurrentStage = new StringKey(locString.key.String + num.ToString());
			}
		}
		float num2 = 0f;
		float num3 = 0f;
		float num4 = WorldGenProgressStages.StageWeights[(int)stage].Value * completePercent;
		for (int i = 0; i < WorldGenProgressStages.StageWeights.Length; i++)
		{
			num3 += WorldGenProgressStages.StageWeights[i].Value;
			if (i < (int)this.currentStage)
			{
				num2 += WorldGenProgressStages.StageWeights[i].Value;
			}
		}
		float num5 = (num2 + num4) / num3;
		this.currentPercent = num5;
		return !this.shouldStop;
	}

	// Token: 0x060079A9 RID: 31145 RVA: 0x002ECE24 File Offset: 0x002EB024
	private void Update()
	{
		if (this.loadTriggered)
		{
			return;
		}
		if (this.currentConvertedCurrentStage.String == null)
		{
			return;
		}
		this.errorMutex.WaitOne();
		int count = this.errors.Count;
		this.errorMutex.ReleaseMutex();
		if (count > 0)
		{
			this.DoExitFlow();
			return;
		}
		this.updateText.text = Strings.Get(this.currentConvertedCurrentStage.String);
		if (!this.debug && this.currentConvertedCurrentStage.Hash == UI.WORLDGEN.COMPLETE.key.Hash && this.currentPercent >= 1f && this.cluster.IsGenerationComplete)
		{
			if (KCrashReporter.terminateOnError && KCrashReporter.hasCrash)
			{
				return;
			}
			this.percentText.text = "";
			this.loadTriggered = true;
			App.LoadScene(this.mainGameLevel);
			return;
		}
		else
		{
			if (this.currentPercent < 0f)
			{
				this.DoExitFlow();
				return;
			}
			if (this.currentPercent > 0f && !this.percentText.gameObject.activeSelf)
			{
				this.percentText.gameObject.SetActive(false);
			}
			this.percentText.text = GameUtil.GetFormattedPercent(this.currentPercent * 100f, GameUtil.TimeSlice.None);
			this.meterAnim.SetPositionPercent(this.currentPercent);
			return;
		}
	}

	// Token: 0x060079AA RID: 31146 RVA: 0x002ECF78 File Offset: 0x002EB178
	private void DisplayErrors()
	{
		this.errorMutex.WaitOne();
		if (this.errors.Count > 0)
		{
			foreach (OfflineWorldGen.ErrorInfo errorInfo in this.errors)
			{
				Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, FrontEndManager.Instance.gameObject, true).PopupConfirmDialog(errorInfo.errorDesc, new System.Action(this.OnConfirmExit), null, null, null, null, null, null, null);
			}
		}
		this.errorMutex.ReleaseMutex();
	}

	// Token: 0x060079AB RID: 31147 RVA: 0x002ED028 File Offset: 0x002EB228
	private void DoExitFlow()
	{
		if (this.startedExitFlow)
		{
			return;
		}
		this.startedExitFlow = true;
		this.percentText.text = UI.WORLDGEN.RESTARTING.ToString();
		this.loadTriggered = true;
		Sim.Shutdown();
		this.DisplayErrors();
	}

	// Token: 0x060079AC RID: 31148 RVA: 0x002ED061 File Offset: 0x002EB261
	private void OnConfirmExit()
	{
		App.LoadScene(this.frontendGameLevel);
	}

	// Token: 0x060079AD RID: 31149 RVA: 0x002ED070 File Offset: 0x002EB270
	private void RemoveButtons()
	{
		for (int i = this.buttonRoot.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.buttonRoot.GetChild(i).gameObject);
		}
	}

	// Token: 0x060079AE RID: 31150 RVA: 0x002ED0AB File Offset: 0x002EB2AB
	private void DoWorldGen(int selectedDimension)
	{
		this.RemoveButtons();
		this.DoWorldGenInitialize();
	}

	// Token: 0x060079AF RID: 31151 RVA: 0x002ED0BC File Offset: 0x002EB2BC
	private void DoWorldGenInitialize()
	{
		string clusterName = "";
		Func<int, WorldGen, bool> shouldSkipWorldCallback = null;
		this.seed = CustomGameSettings.Instance.GetCurrentWorldgenSeed();
		clusterName = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;
		List<string> list = new List<string>();
		foreach (string id in CustomGameSettings.Instance.GetCurrentStories())
		{
			list.Add(Db.Get().Stories.Get(id).worldgenStoryTraitKey);
		}
		this.cluster = new Cluster(clusterName, this.seed, list, true, false, false);
		this.cluster.ShouldSkipWorldCallback = shouldSkipWorldCallback;
		this.cluster.Generate(new WorldGen.OfflineCallbackFunction(this.UpdateProgress), new Action<OfflineWorldGen.ErrorInfo>(this.OnError), this.seed, this.seed, this.seed, this.seed, true, false, false);
	}

	// Token: 0x060079B0 RID: 31152 RVA: 0x002ED1BC File Offset: 0x002EB3BC
	private void OnError(OfflineWorldGen.ErrorInfo error)
	{
		this.errorMutex.WaitOne();
		this.errors.Add(error);
		this.errorMutex.ReleaseMutex();
	}

	// Token: 0x0400550C RID: 21772
	[SerializeField]
	private RectTransform buttonRoot;

	// Token: 0x0400550D RID: 21773
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x0400550E RID: 21774
	[SerializeField]
	private RectTransform chooseLocationPanel;

	// Token: 0x0400550F RID: 21775
	[SerializeField]
	private GameObject locationButtonPrefab;

	// Token: 0x04005510 RID: 21776
	private const float baseScale = 0.005f;

	// Token: 0x04005511 RID: 21777
	private Mutex errorMutex = new Mutex();

	// Token: 0x04005512 RID: 21778
	private List<OfflineWorldGen.ErrorInfo> errors = new List<OfflineWorldGen.ErrorInfo>();

	// Token: 0x04005513 RID: 21779
	private OfflineWorldGen.ValidDimensions[] validDimensions = new OfflineWorldGen.ValidDimensions[]
	{
		new OfflineWorldGen.ValidDimensions
		{
			width = 256,
			height = 384,
			name = UI.FRONTEND.WORLDGENSCREEN.SIZES.STANDARD.key
		}
	};

	// Token: 0x04005514 RID: 21780
	public string frontendGameLevel = "frontend";

	// Token: 0x04005515 RID: 21781
	public string mainGameLevel = "backend";

	// Token: 0x04005516 RID: 21782
	private bool shouldStop;

	// Token: 0x04005517 RID: 21783
	private StringKey currentConvertedCurrentStage;

	// Token: 0x04005518 RID: 21784
	private float currentPercent;

	// Token: 0x04005519 RID: 21785
	public bool debug;

	// Token: 0x0400551A RID: 21786
	private bool trackProgress = true;

	// Token: 0x0400551B RID: 21787
	private bool doWorldGen;

	// Token: 0x0400551C RID: 21788
	[SerializeField]
	private LocText titleText;

	// Token: 0x0400551D RID: 21789
	[SerializeField]
	private LocText mainText;

	// Token: 0x0400551E RID: 21790
	[SerializeField]
	private LocText updateText;

	// Token: 0x0400551F RID: 21791
	[SerializeField]
	private LocText percentText;

	// Token: 0x04005520 RID: 21792
	[SerializeField]
	private LocText seedText;

	// Token: 0x04005521 RID: 21793
	[SerializeField]
	private KBatchedAnimController meterAnim;

	// Token: 0x04005522 RID: 21794
	[SerializeField]
	private KBatchedAnimController asteriodAnim;

	// Token: 0x04005523 RID: 21795
	private Cluster cluster;

	// Token: 0x04005524 RID: 21796
	private StringKey currentStringKeyRoot;

	// Token: 0x04005525 RID: 21797
	private List<LocString> convertList = new List<LocString>
	{
		UI.WORLDGEN.SETTLESIM,
		UI.WORLDGEN.BORDERS,
		UI.WORLDGEN.PROCESSING,
		UI.WORLDGEN.COMPLETELAYOUT,
		UI.WORLDGEN.WORLDLAYOUT,
		UI.WORLDGEN.GENERATENOISE,
		UI.WORLDGEN.BUILDNOISESOURCE,
		UI.WORLDGEN.GENERATESOLARSYSTEM
	};

	// Token: 0x04005526 RID: 21798
	private WorldGenProgressStages.Stages currentStage;

	// Token: 0x04005527 RID: 21799
	private bool loadTriggered;

	// Token: 0x04005528 RID: 21800
	private bool startedExitFlow;

	// Token: 0x04005529 RID: 21801
	private int seed;

	// Token: 0x0200213A RID: 8506
	public struct ErrorInfo
	{
		// Token: 0x040098AD RID: 39085
		public string errorDesc;

		// Token: 0x040098AE RID: 39086
		public Exception exception;
	}

	// Token: 0x0200213B RID: 8507
	[Serializable]
	private struct ValidDimensions
	{
		// Token: 0x040098AF RID: 39087
		public int width;

		// Token: 0x040098B0 RID: 39088
		public int height;

		// Token: 0x040098B1 RID: 39089
		public StringKey name;
	}
}
