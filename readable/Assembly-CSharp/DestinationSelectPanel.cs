using System;
using System.Collections.Generic;
using System.Linq;
using Klei.CustomSettings;
using ProcGen;
using UnityEngine;

// Token: 0x02000CF8 RID: 3320
[AddComponentMenu("KMonoBehaviour/scripts/DestinationSelectPanel")]
public class DestinationSelectPanel : KMonoBehaviour
{
	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x06006689 RID: 26249 RVA: 0x002699E3 File Offset: 0x00267BE3
	// (set) Token: 0x0600668A RID: 26250 RVA: 0x002699EA File Offset: 0x00267BEA
	public static int ChosenClusterCategorySetting
	{
		get
		{
			return DestinationSelectPanel.chosenClusterCategorySetting;
		}
		set
		{
			DestinationSelectPanel.chosenClusterCategorySetting = value;
		}
	}

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x0600668B RID: 26251 RVA: 0x002699F4 File Offset: 0x00267BF4
	// (remove) Token: 0x0600668C RID: 26252 RVA: 0x00269A2C File Offset: 0x00267C2C
	public event Action<ColonyDestinationAsteroidBeltData> OnAsteroidClicked;

	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x0600668D RID: 26253 RVA: 0x00269A64 File Offset: 0x00267C64
	private float min
	{
		get
		{
			return this.asteroidContainer.rect.x + this.offset;
		}
	}

	// Token: 0x1700077D RID: 1917
	// (get) Token: 0x0600668E RID: 26254 RVA: 0x00269A8C File Offset: 0x00267C8C
	private float max
	{
		get
		{
			return this.min + this.asteroidContainer.rect.width;
		}
	}

	// Token: 0x0600668F RID: 26255 RVA: 0x00269AB4 File Offset: 0x00267CB4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.dragTarget.onBeginDrag += this.BeginDrag;
		this.dragTarget.onDrag += this.Drag;
		this.dragTarget.onEndDrag += this.EndDrag;
		MultiToggle multiToggle = this.leftArrowButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.ClickLeft));
		MultiToggle multiToggle2 = this.rightArrowButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.ClickRight));
	}

	// Token: 0x06006690 RID: 26256 RVA: 0x00269B5A File Offset: 0x00267D5A
	private void BeginDrag()
	{
		this.dragStartPos = KInputManager.GetMousePos();
		this.dragLastPos = this.dragStartPos;
		this.isDragging = true;
		KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Start", false));
	}

	// Token: 0x06006691 RID: 26257 RVA: 0x00269B90 File Offset: 0x00267D90
	private void Drag()
	{
		Vector2 vector = KInputManager.GetMousePos();
		float num = vector.x - this.dragLastPos.x;
		this.dragLastPos = vector;
		this.offset += num;
		int num2 = this.selectedIndex;
		this.selectedIndex = Mathf.RoundToInt(-this.offset / this.asteroidXSeparation);
		this.selectedIndex = Mathf.Clamp(this.selectedIndex, 0, this.clusterStartWorlds.Count - 1);
		if (num2 != this.selectedIndex)
		{
			this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
			KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll", false));
		}
	}

	// Token: 0x06006692 RID: 26258 RVA: 0x00269C4D File Offset: 0x00267E4D
	private void EndDrag()
	{
		this.Drag();
		this.isDragging = false;
		KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Stop", false));
	}

	// Token: 0x06006693 RID: 26259 RVA: 0x00269C6C File Offset: 0x00267E6C
	private void ClickLeft()
	{
		this.selectedIndex = Mathf.Clamp(this.selectedIndex - 1, 0, this.clusterKeys.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
	}

	// Token: 0x06006694 RID: 26260 RVA: 0x00269CC4 File Offset: 0x00267EC4
	private void ClickRight()
	{
		this.selectedIndex = Mathf.Clamp(this.selectedIndex + 1, 0, this.clusterKeys.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
	}

	// Token: 0x06006695 RID: 26261 RVA: 0x00269D19 File Offset: 0x00267F19
	public void Init()
	{
		this.clusterKeys = new List<string>();
		this.clusterStartWorlds = new Dictionary<string, string>();
		this.UpdateDisplayedClusters();
	}

	// Token: 0x06006696 RID: 26262 RVA: 0x00269D37 File Offset: 0x00267F37
	public void Uninit()
	{
	}

	// Token: 0x06006697 RID: 26263 RVA: 0x00269D3C File Offset: 0x00267F3C
	private void Update()
	{
		if (!this.isDragging)
		{
			float num = this.offset + (float)this.selectedIndex * this.asteroidXSeparation;
			float num2 = 0f;
			if (num != 0f)
			{
				num2 = -num;
			}
			num2 = Mathf.Clamp(num2, -this.asteroidXSeparation * 2f, this.asteroidXSeparation * 2f);
			if (num2 != 0f)
			{
				float num3 = this.centeringSpeed * Time.unscaledDeltaTime;
				float num4 = num2 * this.centeringSpeed * Time.unscaledDeltaTime;
				if (num4 > 0f && num4 < num3)
				{
					num4 = Mathf.Min(num3, num2);
				}
				else if (num4 < 0f && num4 > -num3)
				{
					num4 = Mathf.Max(-num3, num2);
				}
				this.offset += num4;
			}
		}
		float x = this.asteroidContainer.rect.min.x;
		float x2 = this.asteroidContainer.rect.max.x;
		this.offset = Mathf.Clamp(this.offset, (float)(-(float)(this.clusterStartWorlds.Count - 1)) * this.asteroidXSeparation + x, x2);
		this.RePlaceAsteroids();
		for (int i = 0; i < this.moonContainer.transform.childCount; i++)
		{
			this.moonContainer.transform.GetChild(i).GetChild(0).SetLocalPosition(new Vector3(0f, 1.5f + 3f * Mathf.Sin(((float)i + Time.realtimeSinceStartup) * 1.25f), 0f));
		}
	}

	// Token: 0x06006698 RID: 26264 RVA: 0x00269ED8 File Offset: 0x002680D8
	public void UpdateDisplayedClusters()
	{
		this.clusterKeys.Clear();
		this.clusterStartWorlds.Clear();
		this.asteroidData.Clear();
		foreach (KeyValuePair<string, ClusterLayout> keyValuePair in SettingsCache.clusterLayouts.clusterCache)
		{
			if ((!DlcManager.FeatureClusterSpaceEnabled() || !(keyValuePair.Key == "clusters/SandstoneDefault")) && keyValuePair.Value.clusterCategory == (ClusterLayout.ClusterCategory)DestinationSelectPanel.ChosenClusterCategorySetting)
			{
				this.clusterKeys.Add(keyValuePair.Key);
				ColonyDestinationAsteroidBeltData value = new ColonyDestinationAsteroidBeltData(keyValuePair.Value.GetStartWorld(), 0, keyValuePair.Key);
				this.asteroidData[keyValuePair.Key] = value;
				this.clusterStartWorlds.Add(keyValuePair.Key, keyValuePair.Value.GetStartWorld());
			}
		}
		this.clusterKeys.Sort((string a, string b) => SettingsCache.clusterLayouts.clusterCache[a].menuOrder.CompareTo(SettingsCache.clusterLayouts.clusterCache[b].menuOrder));
	}

	// Token: 0x06006699 RID: 26265 RVA: 0x0026A004 File Offset: 0x00268204
	[ContextMenu("RePlaceAsteroids")]
	public void RePlaceAsteroids()
	{
		this.BeginAsteroidDrawing();
		for (int i = 0; i < this.clusterKeys.Count; i++)
		{
			float x = this.offset + (float)i * this.asteroidXSeparation;
			string text = this.clusterKeys[i];
			float iconScale = this.asteroidData[text].GetStartWorld.iconScale;
			this.GetAsteroid(text, (i == this.selectedIndex) ? (this.asteroidFocusScale * iconScale) : iconScale).transform.SetLocalPosition(new Vector3(x, (i == this.selectedIndex) ? (5f + 10f * Mathf.Sin(Time.realtimeSinceStartup * 1f)) : 0f, 0f));
		}
		this.EndAsteroidDrawing();
	}

	// Token: 0x0600669A RID: 26266 RVA: 0x0026A0CB File Offset: 0x002682CB
	private void BeginAsteroidDrawing()
	{
		this.numAsteroids = 0;
	}

	// Token: 0x0600669B RID: 26267 RVA: 0x0026A0D4 File Offset: 0x002682D4
	private void ShowMoons(ColonyDestinationAsteroidBeltData asteroid)
	{
		if (asteroid.worlds.Count > 0)
		{
			while (this.moonContainer.transform.childCount < asteroid.worlds.Count)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.moonPrefab, this.moonContainer.transform);
			}
			for (int i = 0; i < asteroid.worlds.Count; i++)
			{
				KBatchedAnimController componentInChildren = this.moonContainer.transform.GetChild(i).GetComponentInChildren<KBatchedAnimController>();
				int index = (-1 + i + asteroid.worlds.Count / 2) % asteroid.worlds.Count;
				ProcGen.World world = asteroid.worlds[index];
				KAnimFile anim = Assets.GetAnim(world.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : world.asteroidIcon);
				if (anim != null)
				{
					componentInChildren.SetVisiblity(true);
					componentInChildren.SwapAnims(new KAnimFile[]
					{
						anim
					});
					componentInChildren.initialMode = KAnim.PlayMode.Loop;
					componentInChildren.initialAnim = "idle_loop";
					componentInChildren.gameObject.SetActive(true);
					if (componentInChildren.HasAnimation(componentInChildren.initialAnim))
					{
						componentInChildren.Play(componentInChildren.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
					}
					componentInChildren.transform.parent.gameObject.SetActive(true);
				}
			}
			for (int j = asteroid.worlds.Count; j < this.moonContainer.transform.childCount; j++)
			{
				KBatchedAnimController componentInChildren2 = this.moonContainer.transform.GetChild(j).GetComponentInChildren<KBatchedAnimController>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.SetVisiblity(false);
				}
				this.moonContainer.transform.GetChild(j).gameObject.SetActive(false);
			}
			return;
		}
		KBatchedAnimController[] componentsInChildren = this.moonContainer.GetComponentsInChildren<KBatchedAnimController>();
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			componentsInChildren[k].SetVisiblity(false);
		}
	}

	// Token: 0x0600669C RID: 26268 RVA: 0x0026A2D0 File Offset: 0x002684D0
	private DestinationAsteroid2 GetAsteroid(string name, float scale)
	{
		DestinationAsteroid2 destinationAsteroid;
		if (this.numAsteroids < this.asteroids.Count)
		{
			destinationAsteroid = this.asteroids[this.numAsteroids];
		}
		else
		{
			destinationAsteroid = global::Util.KInstantiateUI<DestinationAsteroid2>(this.asteroidPrefab, this.asteroidContainer.gameObject, false);
			destinationAsteroid.OnClicked += this.OnAsteroidClicked;
			this.asteroids.Add(destinationAsteroid);
		}
		destinationAsteroid.SetAsteroid(this.asteroidData[name]);
		this.asteroidData[name].TargetScale = scale;
		this.asteroidData[name].Scale += (this.asteroidData[name].TargetScale - this.asteroidData[name].Scale) * this.focusScaleSpeed * Time.unscaledDeltaTime;
		destinationAsteroid.transform.localScale = Vector3.one * this.asteroidData[name].Scale;
		this.numAsteroids++;
		return destinationAsteroid;
	}

	// Token: 0x0600669D RID: 26269 RVA: 0x0026A3D8 File Offset: 0x002685D8
	private void EndAsteroidDrawing()
	{
		for (int i = 0; i < this.asteroids.Count; i++)
		{
			this.asteroids[i].gameObject.SetActive(i < this.numAsteroids);
		}
	}

	// Token: 0x0600669E RID: 26270 RVA: 0x0026A41A File Offset: 0x0026861A
	public ColonyDestinationAsteroidBeltData SelectCluster(string name, int seed)
	{
		this.selectedIndex = this.clusterKeys.IndexOf(name);
		this.asteroidData[name].ReInitialize(seed);
		return this.asteroidData[name];
	}

	// Token: 0x0600669F RID: 26271 RVA: 0x0026A44C File Offset: 0x0026864C
	public string GetDefaultAsteroid()
	{
		foreach (string text in this.clusterKeys)
		{
			if (this.asteroidData[text].Layout.menuOrder == 0)
			{
				return text;
			}
		}
		return this.clusterKeys.First<string>();
	}

	// Token: 0x060066A0 RID: 26272 RVA: 0x0026A4C4 File Offset: 0x002686C4
	public ColonyDestinationAsteroidBeltData SelectDefaultAsteroid(int seed)
	{
		this.selectedIndex = 0;
		string key = this.asteroidData.Keys.First<string>();
		this.asteroidData[key].ReInitialize(seed);
		return this.asteroidData[key];
	}

	// Token: 0x060066A1 RID: 26273 RVA: 0x0026A508 File Offset: 0x00268708
	public void ScrollLeft()
	{
		int index = Mathf.Max(this.selectedIndex - 1, 0);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[index]]);
	}

	// Token: 0x060066A2 RID: 26274 RVA: 0x0026A548 File Offset: 0x00268748
	public void ScrollRight()
	{
		int index = Mathf.Min(this.selectedIndex + 1, this.clusterStartWorlds.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[index]]);
	}

	// Token: 0x060066A3 RID: 26275 RVA: 0x0026A594 File Offset: 0x00268794
	private void DebugCurrentSetting()
	{
		ColonyDestinationAsteroidBeltData colonyDestinationAsteroidBeltData = this.asteroidData[this.clusterKeys[this.selectedIndex]];
		string text = "{world}: {seed} [{traits}] {{settings}}";
		string startWorldName = colonyDestinationAsteroidBeltData.startWorldName;
		string newValue = colonyDestinationAsteroidBeltData.seed.ToString();
		text = text.Replace("{world}", startWorldName);
		text = text.Replace("{seed}", newValue);
		List<AsteroidDescriptor> traitDescriptors = colonyDestinationAsteroidBeltData.GetTraitDescriptors();
		string[] array = new string[traitDescriptors.Count];
		for (int i = 0; i < traitDescriptors.Count; i++)
		{
			array[i] = traitDescriptors[i].text;
		}
		string newValue2 = string.Join(", ", array);
		text = text.Replace("{traits}", newValue2);
		CustomGameSettings.CustomGameMode customGameMode = CustomGameSettings.Instance.customGameMode;
		if (customGameMode != CustomGameSettings.CustomGameMode.Survival)
		{
			if (customGameMode != CustomGameSettings.CustomGameMode.Nosweat)
			{
				if (customGameMode == CustomGameSettings.CustomGameMode.Custom)
				{
					List<string> list = new List<string>();
					foreach (KeyValuePair<string, SettingConfig> keyValuePair in CustomGameSettings.Instance.QualitySettings)
					{
						if (keyValuePair.Value.coordinate_range >= 0L)
						{
							SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(keyValuePair.Key);
							if (currentQualitySetting.id != keyValuePair.Value.GetDefaultLevelId())
							{
								list.Add(string.Format("{0}={1}", keyValuePair.Value.label, currentQualitySetting.label));
							}
						}
					}
					text = text.Replace("{settings}", string.Join(", ", list.ToArray()));
				}
			}
			else
			{
				text = text.Replace("{settings}", "Nosweat");
			}
		}
		else
		{
			text = text.Replace("{settings}", "Survival");
		}
		global::Debug.Log(text);
	}

	// Token: 0x0400460B RID: 17931
	[SerializeField]
	private GameObject asteroidPrefab;

	// Token: 0x0400460C RID: 17932
	[SerializeField]
	private KButtonDrag dragTarget;

	// Token: 0x0400460D RID: 17933
	[SerializeField]
	private MultiToggle leftArrowButton;

	// Token: 0x0400460E RID: 17934
	[SerializeField]
	private MultiToggle rightArrowButton;

	// Token: 0x0400460F RID: 17935
	[SerializeField]
	private RectTransform asteroidContainer;

	// Token: 0x04004610 RID: 17936
	[SerializeField]
	private float asteroidFocusScale = 2f;

	// Token: 0x04004611 RID: 17937
	[SerializeField]
	private float asteroidXSeparation = 240f;

	// Token: 0x04004612 RID: 17938
	[SerializeField]
	private float focusScaleSpeed = 0.5f;

	// Token: 0x04004613 RID: 17939
	[SerializeField]
	private float centeringSpeed = 0.5f;

	// Token: 0x04004614 RID: 17940
	[SerializeField]
	private GameObject moonContainer;

	// Token: 0x04004615 RID: 17941
	[SerializeField]
	private GameObject moonPrefab;

	// Token: 0x04004616 RID: 17942
	private static int chosenClusterCategorySetting;

	// Token: 0x04004618 RID: 17944
	private float offset;

	// Token: 0x04004619 RID: 17945
	private int selectedIndex = -1;

	// Token: 0x0400461A RID: 17946
	private List<DestinationAsteroid2> asteroids = new List<DestinationAsteroid2>();

	// Token: 0x0400461B RID: 17947
	private int numAsteroids;

	// Token: 0x0400461C RID: 17948
	private List<string> clusterKeys;

	// Token: 0x0400461D RID: 17949
	private Dictionary<string, string> clusterStartWorlds;

	// Token: 0x0400461E RID: 17950
	private Dictionary<string, ColonyDestinationAsteroidBeltData> asteroidData = new Dictionary<string, ColonyDestinationAsteroidBeltData>();

	// Token: 0x0400461F RID: 17951
	private Vector2 dragStartPos;

	// Token: 0x04004620 RID: 17952
	private Vector2 dragLastPos;

	// Token: 0x04004621 RID: 17953
	private bool isDragging;

	// Token: 0x04004622 RID: 17954
	private const string debugFmt = "{world}: {seed} [{traits}] {{settings}}";
}
