using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000AF7 RID: 2807
[AddComponentMenu("KMonoBehaviour/scripts/Timelapser")]
public class Timelapser : KMonoBehaviour
{
	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x06005197 RID: 20887 RVA: 0x001D9730 File Offset: 0x001D7930
	public bool CapturingTimelapseScreenshot
	{
		get
		{
			return this.screenshotActive;
		}
	}

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x06005198 RID: 20888 RVA: 0x001D9738 File Offset: 0x001D7938
	// (set) Token: 0x06005199 RID: 20889 RVA: 0x001D9740 File Offset: 0x001D7940
	public Texture2D freezeTexture { get; private set; }

	// Token: 0x0600519A RID: 20890 RVA: 0x001D974C File Offset: 0x001D794C
	protected override void OnPrefabInit()
	{
		this.RefreshRenderTextureSize(null);
		Game.Instance.Subscribe(75424175, new Action<object>(this.RefreshRenderTextureSize));
		this.freezeCamera = CameraController.Instance.timelapseFreezeCamera;
		if (this.CycleTimeToScreenshot() > 0f)
		{
			this.OnNewDay(null);
		}
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
		this.OnResize();
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		base.StartCoroutine(this.Render());
	}

	// Token: 0x0600519B RID: 20891 RVA: 0x001D97F5 File Offset: 0x001D79F5
	private void OnResize()
	{
		if (this.freezeTexture != null)
		{
			UnityEngine.Object.Destroy(this.freezeTexture);
		}
		this.freezeTexture = new Texture2D(Camera.main.pixelWidth, Camera.main.pixelHeight, TextureFormat.ARGB32, false);
	}

	// Token: 0x0600519C RID: 20892 RVA: 0x001D9834 File Offset: 0x001D7A34
	private void RefreshRenderTextureSize(object data = null)
	{
		if (this.previewScreenshot)
		{
			if (this.bufferRenderTexture != null)
			{
				this.bufferRenderTexture.DestroyRenderTexture();
			}
			this.bufferRenderTexture = new RenderTexture(this.previewScreenshotResolution.x, this.previewScreenshotResolution.y, 32, RenderTextureFormat.ARGB32);
			this.bufferRenderTexture.name = "Timelapser.PreviewScreenshot";
			return;
		}
		if (this.timelapseUserEnabled)
		{
			if (this.bufferRenderTexture != null)
			{
				this.bufferRenderTexture.DestroyRenderTexture();
			}
			this.bufferRenderTexture = new RenderTexture(SaveGame.Instance.TimelapseResolution.x, SaveGame.Instance.TimelapseResolution.y, 32, RenderTextureFormat.ARGB32);
			this.bufferRenderTexture.name = "Timelapser.Timelapse";
		}
	}

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x0600519D RID: 20893 RVA: 0x001D98F4 File Offset: 0x001D7AF4
	private bool timelapseUserEnabled
	{
		get
		{
			return SaveGame.Instance.TimelapseResolution.x > 0;
		}
	}

	// Token: 0x0600519E RID: 20894 RVA: 0x001D9908 File Offset: 0x001D7B08
	private void OnNewDay(object data = null)
	{
		if (this.worldsToScreenshot.Count == 0)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Timelapse.OnNewDay but worldsToScreenshot is not empty"
			});
		}
		int cycle = GameClock.Instance.GetCycle();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.IsDiscovered && !worldContainer.IsModuleInterior)
			{
				if (worldContainer.DiscoveryTimestamp + (float)cycle > (float)this.timelapseScreenshotCycles[this.timelapseScreenshotCycles.Length - 1])
				{
					if (worldContainer.DiscoveryTimestamp + (float)(cycle % 10) == 0f)
					{
						this.screenshotToday = true;
						this.worldsToScreenshot.Add(worldContainer.id);
					}
				}
				else
				{
					for (int i = 0; i < this.timelapseScreenshotCycles.Length; i++)
					{
						if ((int)worldContainer.DiscoveryTimestamp + cycle == this.timelapseScreenshotCycles[i])
						{
							this.screenshotToday = true;
							this.worldsToScreenshot.Add(worldContainer.id);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600519F RID: 20895 RVA: 0x001D9A28 File Offset: 0x001D7C28
	private void Update()
	{
		if (this.screenshotToday)
		{
			if (this.CycleTimeToScreenshot() <= 0f || GameClock.Instance.GetCycle() == 0)
			{
				if (!this.timelapseUserEnabled)
				{
					this.screenshotToday = false;
					this.worldsToScreenshot.Clear();
					return;
				}
				if (!PlayerController.Instance.CanDrag())
				{
					CameraController.Instance.ForcePanningState(false);
					this.screenshotToday = false;
					this.SaveScreenshot();
					return;
				}
			}
		}
		else
		{
			this.screenshotToday = (!this.screenshotPending && this.worldsToScreenshot.Count > 0);
		}
	}

	// Token: 0x060051A0 RID: 20896 RVA: 0x001D9AB5 File Offset: 0x001D7CB5
	private float CycleTimeToScreenshot()
	{
		return 300f - GameClock.Instance.GetTime() % 600f;
	}

	// Token: 0x060051A1 RID: 20897 RVA: 0x001D9ACD File Offset: 0x001D7CCD
	private IEnumerator Render()
	{
		for (;;)
		{
			yield return SequenceUtil.WaitForEndOfFrame;
			if (this.screenshotPending)
			{
				int num = this.previewScreenshot ? ClusterManager.Instance.GetStartWorld().id : this.worldsToScreenshot[0];
				if (!this.freezeCamera.enabled)
				{
					this.freezeTexture.ReadPixels(new Rect(0f, 0f, (float)Camera.main.pixelWidth, (float)Camera.main.pixelHeight), 0, 0);
					this.freezeTexture.Apply();
					this.freezeCamera.gameObject.GetComponent<FillRenderTargetEffect>().SetFillTexture(this.freezeTexture);
					this.freezeCamera.enabled = true;
					this.screenshotActive = true;
					this.RefreshRenderTextureSize(null);
					DebugHandler.SetTimelapseMode(true, num);
					this.SetPostionAndOrtho(num);
					this.activeOverlay = OverlayScreen.Instance.mode;
					OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, false);
				}
				else
				{
					this.RenderAndPrint(num);
					if (!this.previewScreenshot)
					{
						this.worldsToScreenshot.Remove(num);
					}
					this.freezeCamera.enabled = false;
					DebugHandler.SetTimelapseMode(false, 0);
					this.screenshotPending = false;
					this.previewScreenshot = false;
					this.screenshotActive = false;
					this.debugScreenShot = false;
					this.previewSaveGamePath = "";
					OverlayScreen.Instance.ToggleOverlay(this.activeOverlay, false);
				}
			}
		}
		yield break;
	}

	// Token: 0x060051A2 RID: 20898 RVA: 0x001D9ADC File Offset: 0x001D7CDC
	public void InitialScreenshot()
	{
		this.worldsToScreenshot.Add(ClusterManager.Instance.GetStartWorld().id);
		this.SaveScreenshot();
	}

	// Token: 0x060051A3 RID: 20899 RVA: 0x001D9AFE File Offset: 0x001D7CFE
	private void SaveScreenshot()
	{
		this.screenshotPending = true;
	}

	// Token: 0x060051A4 RID: 20900 RVA: 0x001D9B07 File Offset: 0x001D7D07
	public void SaveColonyPreview(string saveFileName)
	{
		this.previewSaveGamePath = saveFileName;
		this.previewScreenshot = true;
		this.SaveScreenshot();
	}

	// Token: 0x060051A5 RID: 20901 RVA: 0x001D9B20 File Offset: 0x001D7D20
	private void SetPostionAndOrtho(int world_id)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(world_id);
		if (world == null)
		{
			return;
		}
		float num = 0f;
		Camera overlayCamera = CameraController.Instance.overlayCamera;
		this.camSize = overlayCamera.orthographicSize;
		this.camPosition = CameraController.Instance.transform.position;
		if (!world.IsStartWorld)
		{
			CameraController.Instance.OrthographicSize = (float)(world.WorldSize.y / 2);
			CameraController.Instance.SetPosition(new Vector3((float)(world.WorldOffset.x + world.WorldSize.x / 2), (float)(world.WorldOffset.y + world.WorldSize.y / 2), CameraController.Instance.transform.position.z));
			return;
		}
		GameObject telepad = GameUtil.GetTelepad(world_id);
		if (telepad == null)
		{
			return;
		}
		Vector3 position = telepad.transform.GetPosition();
		foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
		{
			Vector3 position2 = buildingComplete.transform.GetPosition();
			float num2 = (float)this.bufferRenderTexture.width / (float)this.bufferRenderTexture.height;
			Vector3 vector = position - position2;
			num = Mathf.Max(new float[]
			{
				num,
				vector.x / num2,
				vector.y
			});
		}
		num += 10f;
		num = Mathf.Max(num, 18f);
		CameraController.Instance.OrthographicSize = num;
		CameraController.Instance.SetPosition(new Vector3(telepad.transform.position.x, telepad.transform.position.y, CameraController.Instance.transform.position.z));
	}

	// Token: 0x060051A6 RID: 20902 RVA: 0x001D9D0C File Offset: 0x001D7F0C
	private void RenderAndPrint(int world_id)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(world_id);
		if (world == null)
		{
			return;
		}
		if (world.IsStartWorld)
		{
			GameObject telepad = GameUtil.GetTelepad(world.id);
			if (telepad == null)
			{
				global::Debug.Log("No telepad present, aborting screenshot.");
				return;
			}
			Vector3 position = telepad.transform.position;
			position.z = CameraController.Instance.transform.position.z;
			CameraController.Instance.SetPosition(position);
		}
		else
		{
			CameraController.Instance.SetPosition(new Vector3((float)(world.WorldOffset.x + world.WorldSize.x / 2), (float)(world.WorldOffset.y + world.WorldSize.y / 2), CameraController.Instance.transform.position.z));
		}
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = this.bufferRenderTexture;
		CameraController.Instance.RenderForTimelapser(ref this.bufferRenderTexture);
		this.WriteToPng(this.bufferRenderTexture, world_id);
		CameraController.Instance.OrthographicSize = this.camSize;
		CameraController.Instance.SetPosition(this.camPosition);
		RenderTexture.active = active;
	}

	// Token: 0x060051A7 RID: 20903 RVA: 0x001D9E34 File Offset: 0x001D8034
	public void WriteToPng(RenderTexture renderTex, int world_id = -1)
	{
		Texture2D texture2D = new Texture2D(renderTex.width, renderTex.height, TextureFormat.ARGB32, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTex.width, (float)renderTex.height), 0, 0);
		texture2D.Apply();
		byte[] bytes = texture2D.EncodeToPNG();
		UnityEngine.Object.Destroy(texture2D);
		if (!Directory.Exists(Util.RootFolder()))
		{
			Directory.CreateDirectory(Util.RootFolder());
		}
		string text = Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName());
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string path = RetireColonyUtility.StripInvalidCharacters(SaveGame.Instance.BaseName);
		if (!this.previewScreenshot)
		{
			string text2 = Path.Combine(text, path);
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			string text3 = text2;
			if (world_id >= 0)
			{
				string name = ClusterManager.Instance.GetWorld(world_id).GetComponent<ClusterGridEntity>().Name;
				text3 = Path.Combine(text3, world_id.ToString("D5"));
				if (!Directory.Exists(text3))
				{
					Directory.CreateDirectory(text3);
				}
				text3 = Path.Combine(text3, name);
			}
			else
			{
				text3 = Path.Combine(text3, path);
			}
			DebugUtil.LogArgs(new object[]
			{
				"Saving screenshot to",
				text3
			});
			string format = "0000.##";
			text3 = text3 + "_cycle_" + GameClock.Instance.GetCycle().ToString(format);
			if (this.debugScreenShot)
			{
				text3 = string.Concat(new string[]
				{
					text3,
					"_",
					System.DateTime.Now.Day.ToString(),
					"-",
					System.DateTime.Now.Month.ToString(),
					"_",
					System.DateTime.Now.Hour.ToString(),
					"-",
					System.DateTime.Now.Minute.ToString(),
					"-",
					System.DateTime.Now.Second.ToString()
				});
			}
			File.WriteAllBytes(text3 + ".png", bytes);
			return;
		}
		string text4 = this.previewSaveGamePath;
		text4 = Path.ChangeExtension(text4, ".png");
		DebugUtil.LogArgs(new object[]
		{
			"Saving screenshot to",
			text4
		});
		File.WriteAllBytes(text4, bytes);
	}

	// Token: 0x04003731 RID: 14129
	private bool screenshotActive;

	// Token: 0x04003732 RID: 14130
	private bool screenshotPending;

	// Token: 0x04003733 RID: 14131
	private bool previewScreenshot;

	// Token: 0x04003734 RID: 14132
	private string previewSaveGamePath = "";

	// Token: 0x04003735 RID: 14133
	private bool screenshotToday;

	// Token: 0x04003736 RID: 14134
	private List<int> worldsToScreenshot = new List<int>();

	// Token: 0x04003737 RID: 14135
	private HashedString activeOverlay;

	// Token: 0x04003738 RID: 14136
	private Camera freezeCamera;

	// Token: 0x04003739 RID: 14137
	private RenderTexture bufferRenderTexture;

	// Token: 0x0400373B RID: 14139
	private Vector3 camPosition;

	// Token: 0x0400373C RID: 14140
	private float camSize;

	// Token: 0x0400373D RID: 14141
	private bool debugScreenShot;

	// Token: 0x0400373E RID: 14142
	private Vector2Int previewScreenshotResolution = new Vector2Int(Grid.WidthInCells * 2, Grid.HeightInCells * 2);

	// Token: 0x0400373F RID: 14143
	private const int DEFAULT_SCREENSHOT_INTERVAL = 10;

	// Token: 0x04003740 RID: 14144
	private int[] timelapseScreenshotCycles = new int[]
	{
		1,
		2,
		3,
		4,
		5,
		6,
		7,
		8,
		9,
		10,
		11,
		12,
		13,
		14,
		15,
		16,
		17,
		18,
		19,
		20,
		21,
		22,
		23,
		24,
		25,
		26,
		27,
		28,
		29,
		30,
		31,
		32,
		33,
		34,
		35,
		36,
		37,
		38,
		39,
		40,
		41,
		42,
		43,
		44,
		45,
		46,
		47,
		48,
		49,
		50,
		55,
		60,
		65,
		70,
		75,
		80,
		85,
		90,
		95,
		100,
		110,
		120,
		130,
		140,
		150,
		160,
		170,
		180,
		190,
		200,
		210,
		220,
		230,
		240,
		250,
		260,
		270,
		280,
		290,
		200,
		310,
		320,
		330,
		340,
		350,
		360,
		370,
		380,
		390,
		400,
		410,
		420,
		430,
		440,
		450,
		460,
		470,
		480,
		490,
		500
	};
}
