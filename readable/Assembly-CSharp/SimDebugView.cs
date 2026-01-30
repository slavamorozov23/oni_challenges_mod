using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

// Token: 0x02000B4B RID: 2891
[AddComponentMenu("KMonoBehaviour/scripts/SimDebugView")]
public class SimDebugView : KMonoBehaviour
{
	// Token: 0x0600551A RID: 21786 RVA: 0x001F0403 File Offset: 0x001EE603
	public static void DestroyInstance()
	{
		SimDebugView.Instance = null;
	}

	// Token: 0x0600551B RID: 21787 RVA: 0x001F040B File Offset: 0x001EE60B
	protected override void OnPrefabInit()
	{
		SimDebugView.Instance = this;
		this.material = UnityEngine.Object.Instantiate<Material>(this.material);
		this.diseaseMaterial = UnityEngine.Object.Instantiate<Material>(this.diseaseMaterial);
	}

	// Token: 0x0600551C RID: 21788 RVA: 0x001F0438 File Offset: 0x001EE638
	protected override void OnSpawn()
	{
		SimDebugViewCompositor.Instance.material.SetColor("_Color0", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[0].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color1", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[1].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color2", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[2].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color3", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[3].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color4", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[4].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color5", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[5].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color6", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[6].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color7", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[7].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color0", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[0].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color1", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[1].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color2", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[2].colorName));
		this.SetMode(global::OverlayModes.None.ID);
	}

	// Token: 0x0600551D RID: 21789 RVA: 0x001F06C4 File Offset: 0x001EE8C4
	public void OnReset()
	{
		this.plane = SimDebugView.CreatePlane("SimDebugView", base.transform);
		this.tex = SimDebugView.CreateTexture(out this.texBytes, Grid.WidthInCells, Grid.HeightInCells);
		this.plane.GetComponent<Renderer>().sharedMaterial = this.material;
		this.plane.GetComponent<Renderer>().sharedMaterial.mainTexture = this.tex;
		this.plane.transform.SetLocalPosition(new Vector3(0f, 0f, -6f));
		this.SetMode(global::OverlayModes.None.ID);
	}

	// Token: 0x0600551E RID: 21790 RVA: 0x001F0763 File Offset: 0x001EE963
	public static Texture2D CreateTexture(int width, int height)
	{
		return new Texture2D(width, height)
		{
			name = "SimDebugView",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x0600551F RID: 21791 RVA: 0x001F0785 File Offset: 0x001EE985
	public static Texture2D CreateTexture(out byte[] textureBytes, int width, int height)
	{
		textureBytes = new byte[width * height * 4];
		return new Texture2D(width, height, TextureUtil.TextureFormatToGraphicsFormat(TextureFormat.RGBA32), TextureCreationFlags.None)
		{
			name = "SimDebugView",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x06005520 RID: 21792 RVA: 0x001F07BC File Offset: 0x001EE9BC
	public static Texture2D CreateTexture(int width, int height, Color col)
	{
		Color[] array = new Color[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = col;
		}
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06005521 RID: 21793 RVA: 0x001F07FC File Offset: 0x001EE9FC
	public static GameObject CreatePlane(string layer, Transform parent)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "overlayViewDisplayPlane";
		gameObject.SetLayerRecursively(LayerMask.NameToLayer(layer));
		gameObject.transform.SetParent(parent);
		gameObject.transform.SetPosition(Vector3.zero);
		gameObject.AddComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		float y = 2f * (float)Grid.HeightInCells;
		vertices = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3((float)Grid.WidthInCells, 0f, 0f),
			new Vector3(0f, y, 0f),
			new Vector3(Grid.WidthInMeters, y, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 2f),
			new Vector2(1f, 2f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		Vector2 vector = new Vector2((float)Grid.WidthInCells, y);
		mesh.bounds = new Bounds(new Vector3(0.5f * vector.x, 0.5f * vector.y, 0f), new Vector3(vector.x, vector.y, 0f));
		return gameObject;
	}

	// Token: 0x06005522 RID: 21794 RVA: 0x001F09D4 File Offset: 0x001EEBD4
	private void Update()
	{
		if (this.plane == null)
		{
			return;
		}
		bool flag = this.mode != global::OverlayModes.None.ID;
		this.plane.SetActive(flag);
		SimDebugViewCompositor.Instance.Toggle(flag && !GameUtil.IsCapturingTimeLapse());
		SimDebugViewCompositor.Instance.material.SetVector("_Thresholds0", new Vector4(0.1f, 0.2f, 0.3f, 0.4f));
		SimDebugViewCompositor.Instance.material.SetVector("_Thresholds1", new Vector4(0.5f, 0.6f, 0.7f, 0.8f));
		float x = 0f;
		if (this.mode == global::OverlayModes.ThermalConductivity.ID || this.mode == global::OverlayModes.Temperature.ID)
		{
			x = 1f;
		}
		SimDebugViewCompositor.Instance.material.SetVector("_ThresholdParameters", new Vector4(x, this.thresholdRange, this.thresholdOpacity, 0f));
		if (flag)
		{
			this.UpdateData(this.tex, this.texBytes, this.mode, 192);
		}
	}

	// Token: 0x06005523 RID: 21795 RVA: 0x001F0AFA File Offset: 0x001EECFA
	private static void SetDefaultBilinear(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.material;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Bilinear;
	}

	// Token: 0x06005524 RID: 21796 RVA: 0x001F0B2A File Offset: 0x001EED2A
	private static void SetDefaultPoint(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.material;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Point;
	}

	// Token: 0x06005525 RID: 21797 RVA: 0x001F0B5A File Offset: 0x001EED5A
	private static void SetDisease(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.diseaseMaterial;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Bilinear;
	}

	// Token: 0x06005526 RID: 21798 RVA: 0x001F0B8C File Offset: 0x001EED8C
	public void UpdateData(Texture2D texture, byte[] textureBytes, HashedString viewMode, byte alpha)
	{
		Action<SimDebugView, Texture> action;
		if (!this.dataUpdateFuncs.TryGetValue(viewMode, out action))
		{
			action = new Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint);
		}
		action(this, texture);
		int x;
		int num;
		int x2;
		int num2;
		Grid.GetVisibleExtents(out x, out num, out x2, out num2);
		this.selectedNavigator = null;
		KSelectable selected = SelectTool.Instance.selected;
		if (selected != null)
		{
			this.selectedNavigator = selected.GetComponent<Navigator>();
		}
		this.updateSimViewWorkItems.Reset(new SimDebugView.UpdateSimViewSharedData(this, this.texBytes, viewMode, this));
		int num3 = 16;
		for (int i = num; i <= num2; i += num3)
		{
			int y = Math.Min(i + num3 - 1, num2);
			this.updateSimViewWorkItems.Add(new SimDebugView.UpdateSimViewWorkItem(x, i, x2, y));
		}
		this.currentFrame = Time.frameCount;
		this.selectedCell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		GlobalJobManager.Run(this.updateSimViewWorkItems);
		texture.LoadRawTextureData(textureBytes);
		texture.Apply();
	}

	// Token: 0x06005527 RID: 21799 RVA: 0x001F0C87 File Offset: 0x001EEE87
	public void SetGameGridMode(SimDebugView.GameGridMode mode)
	{
		this.gameGridMode = mode;
	}

	// Token: 0x06005528 RID: 21800 RVA: 0x001F0C90 File Offset: 0x001EEE90
	public SimDebugView.GameGridMode GetGameGridMode()
	{
		return this.gameGridMode;
	}

	// Token: 0x06005529 RID: 21801 RVA: 0x001F0C98 File Offset: 0x001EEE98
	public void SetMode(HashedString mode)
	{
		this.mode = mode;
		Game.Instance.gameObject.BoxingTrigger(1798162660, mode);
	}

	// Token: 0x0600552A RID: 21802 RVA: 0x001F0CB6 File Offset: 0x001EEEB6
	public HashedString GetMode()
	{
		return this.mode;
	}

	// Token: 0x0600552B RID: 21803 RVA: 0x001F0CC0 File Offset: 0x001EEEC0
	public static Color TemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0f, 1f);
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, 1f, 1f);
	}

	// Token: 0x0600552C RID: 21804 RVA: 0x001F0D0C File Offset: 0x001EEF0C
	public static Color LiquidTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float value = (temperature - minTempExpected) / (maxTempExpected - minTempExpected);
		float num = Mathf.Clamp(value, 0.5f, 1f);
		float s = Mathf.Clamp(value, 0f, 1f);
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x0600552D RID: 21805 RVA: 0x001F0D68 File Offset: 0x001EEF68
	public static Color SolidTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0.5f, 1f);
		float s = 1f;
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x0600552E RID: 21806 RVA: 0x001F0DB8 File Offset: 0x001EEFB8
	public static Color GasTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0f, 0.5f);
		float s = 1f;
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x0600552F RID: 21807 RVA: 0x001F0E08 File Offset: 0x001EF008
	public Color NormalizedTemperature(float actualTemperature)
	{
		float num = this.user_temperatureThresholds[0];
		float num2 = this.user_temperatureThresholds[1];
		float num3 = num2 - num;
		if (actualTemperature < num)
		{
			return GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[0].colorName);
		}
		if (actualTemperature > num2)
		{
			return GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[this.temperatureThresholds.Length - 1].colorName);
		}
		int num4 = 0;
		float t = 0f;
		Game.TemperatureOverlayModes temperatureOverlayMode = Game.Instance.temperatureOverlayMode;
		if (temperatureOverlayMode != Game.TemperatureOverlayModes.AbsoluteTemperature)
		{
			if (temperatureOverlayMode == Game.TemperatureOverlayModes.RelativeTemperature)
			{
				float num5 = num;
				for (int i = 0; i < SimDebugView.relativeTemperatureColorIntervals.Length; i++)
				{
					if (actualTemperature < num5 + SimDebugView.relativeTemperatureColorIntervals[i] * num3)
					{
						num4 = i;
						break;
					}
					num5 += SimDebugView.relativeTemperatureColorIntervals[i] * num3;
				}
				t = (actualTemperature - num5) / (SimDebugView.relativeTemperatureColorIntervals[num4] * num3);
			}
		}
		else
		{
			float num6 = num;
			for (int j = 0; j < SimDebugView.absoluteTemperatureColorIntervals.Length; j++)
			{
				if (actualTemperature < num6 + SimDebugView.absoluteTemperatureColorIntervals[j])
				{
					num4 = j;
					break;
				}
				num6 += SimDebugView.absoluteTemperatureColorIntervals[j];
			}
			t = (actualTemperature - num6) / SimDebugView.absoluteTemperatureColorIntervals[num4];
		}
		return Color.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[num4].colorName), GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[num4 + 1].colorName), t);
	}

	// Token: 0x06005530 RID: 21808 RVA: 0x001F0F9C File Offset: 0x001EF19C
	public Color NormalizedHeatFlow(int cell)
	{
		int num = 0;
		int num2 = 0;
		float thermalComfort = GameUtil.GetThermalComfort(GameTags.Minions.Models.Standard, cell, -DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_BASE_GENERATION_KILOWATTS);
		for (int i = 0; i < this.heatFlowThresholds.Length; i++)
		{
			if (thermalComfort <= this.heatFlowThresholds[i].value)
			{
				num2 = i;
				break;
			}
			num = i;
			num2 = i;
		}
		float num3 = 0f;
		if (num != num2)
		{
			num3 = (thermalComfort - this.heatFlowThresholds[num].value) / (this.heatFlowThresholds[num2].value - this.heatFlowThresholds[num].value);
		}
		num3 = Mathf.Max(num3, 0f);
		num3 = Mathf.Min(num3, 1f);
		Color result = Color.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[num].colorName), GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[num2].colorName), num3);
		if (Grid.Solid[cell])
		{
			result = Color.black;
		}
		return result;
	}

	// Token: 0x06005531 RID: 21809 RVA: 0x001F10C2 File Offset: 0x001EF2C2
	private static bool IsInsulated(int cell)
	{
		return (Grid.Element[cell].state & Element.State.TemperatureInsulated) > Element.State.Vacuum;
	}

	// Token: 0x06005532 RID: 21810 RVA: 0x001F10D6 File Offset: 0x001EF2D6
	private static Color GetMinionNavTableIsValid(SimDebugView instance, int cell)
	{
		if (Pathfinding.Instance.GetNavGrid("MinionNavGrid").NavTable.IsValid(cell, NavType.Floor))
		{
			return Color.green;
		}
		return Color.black;
	}

	// Token: 0x06005533 RID: 21811 RVA: 0x001F1100 File Offset: 0x001EF300
	private static Color GetMinionAsyncRenderDataColour(SimDebugView instance, int cell)
	{
		return Color.black;
	}

	// Token: 0x06005534 RID: 21812 RVA: 0x001F1108 File Offset: 0x001EF308
	private static Color GetDiseaseColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (Grid.DiseaseIdx[cell] != 255)
		{
			Disease disease = Db.Get().Diseases[(int)Grid.DiseaseIdx[cell]];
			result = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
			result.a = SimUtil.DiseaseCountToAlpha(Grid.DiseaseCount[cell]);
		}
		else
		{
			result.a = 0f;
		}
		return result;
	}

	// Token: 0x06005535 RID: 21813 RVA: 0x001F1189 File Offset: 0x001EF389
	private static Color GetHeatFlowColour(SimDebugView instance, int cell)
	{
		return instance.NormalizedHeatFlow(cell);
	}

	// Token: 0x06005536 RID: 21814 RVA: 0x001F1192 File Offset: 0x001EF392
	private static Color GetBlack(SimDebugView instance, int cell)
	{
		return Color.black;
	}

	// Token: 0x06005537 RID: 21815 RVA: 0x001F119C File Offset: 0x001EF39C
	public static Color GetLightColour(SimDebugView instance, int cell)
	{
		Color result = GlobalAssets.Instance.colorSet.lightOverlay;
		result.a = Mathf.Clamp(Mathf.Sqrt((float)(Grid.LightIntensity[cell] + LightGridManager.previewLux[cell])) / Mathf.Sqrt(80000f), 0f, 1f);
		if (Grid.LightIntensity[cell] > DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN)
		{
			float num = ((float)Grid.LightIntensity[cell] + (float)LightGridManager.previewLux[cell] - (float)DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN) / (float)(80000 - DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN);
			num /= 10f;
			result.r += Mathf.Min(0.1f, PerlinSimplexNoise.noise(Grid.CellToPos2D(cell).x / 8f, Grid.CellToPos2D(cell).y / 8f + (float)instance.currentFrame / 32f) * num);
		}
		return result;
	}

	// Token: 0x06005538 RID: 21816 RVA: 0x001F12AC File Offset: 0x001EF4AC
	public static Color GetRadiationColour(SimDebugView instance, int cell)
	{
		float a = Mathf.Clamp(Mathf.Sqrt(Grid.Radiation[cell]) / 30f, 0f, 1f);
		return new Color(0.2f, 0.9f, 0.3f, a);
	}

	// Token: 0x06005539 RID: 21817 RVA: 0x001F12F4 File Offset: 0x001EF4F4
	public static Color GetRoomsColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (Grid.IsValidCell(instance.selectedCell))
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (cavityForCell != null && cavityForCell.room != null)
			{
				Room room = cavityForCell.room;
				result = GlobalAssets.Instance.colorSet.GetColorByName(room.roomType.category.colorName);
				result.a = 0.45f;
				if (Game.Instance.roomProber.GetCavityForCell(instance.selectedCell) == cavityForCell)
				{
					result.a += 0.3f;
				}
			}
		}
		return result;
	}

	// Token: 0x0600553A RID: 21818 RVA: 0x001F1394 File Offset: 0x001EF594
	public static Color GetJoulesColour(SimDebugView instance, int cell)
	{
		float num = Grid.Element[cell].specificHeatCapacity * Grid.Temperature[cell] * (Grid.Mass[cell] * 1000f);
		float t = 0.5f * num / (ElementLoader.FindElementByHash(SimHashes.SandStone).specificHeatCapacity * 294f * 1000000f);
		return Color.Lerp(Color.black, Color.red, t);
	}

	// Token: 0x0600553B RID: 21819 RVA: 0x001F1400 File Offset: 0x001EF600
	public static Color GetNormalizedTemperatureColourMode(SimDebugView instance, int cell)
	{
		switch (Game.Instance.temperatureOverlayMode)
		{
		case Game.TemperatureOverlayModes.AbsoluteTemperature:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		case Game.TemperatureOverlayModes.AdaptiveTemperature:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		case Game.TemperatureOverlayModes.HeatFlow:
			return SimDebugView.GetHeatFlowColour(instance, cell);
		case Game.TemperatureOverlayModes.StateChange:
			return SimDebugView.GetStateChangeProximityColour(instance, cell);
		default:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		}
	}

	// Token: 0x0600553C RID: 21820 RVA: 0x001F1458 File Offset: 0x001EF658
	public static Color GetStateChangeProximityColour(SimDebugView instance, int cell)
	{
		float temperature = Grid.Temperature[cell];
		Element element = Grid.Element[cell];
		float num = element.lowTemp;
		float num2 = element.highTemp;
		if (element.IsGas)
		{
			num2 = Mathf.Min(num + 150f, num2);
			return SimDebugView.GasTemperatureToColor(temperature, num, num2);
		}
		if (element.IsSolid)
		{
			num = Mathf.Max(num2 - 150f, num);
			return SimDebugView.SolidTemperatureToColor(temperature, num, num2);
		}
		return SimDebugView.TemperatureToColor(temperature, num, num2);
	}

	// Token: 0x0600553D RID: 21821 RVA: 0x001F14D0 File Offset: 0x001EF6D0
	public static Color GetNormalizedTemperatureColour(SimDebugView instance, int cell)
	{
		float actualTemperature = Grid.Temperature[cell];
		return instance.NormalizedTemperature(actualTemperature);
	}

	// Token: 0x0600553E RID: 21822 RVA: 0x001F14F0 File Offset: 0x001EF6F0
	private static Color GetGameGridColour(SimDebugView instance, int cell)
	{
		Color result = new Color32(0, 0, 0, byte.MaxValue);
		switch (instance.gameGridMode)
		{
		case SimDebugView.GameGridMode.GameSolidMap:
			result = (Grid.Solid[cell] ? Color.white : Color.black);
			break;
		case SimDebugView.GameGridMode.Lighting:
			result = ((Grid.LightCount[cell] > 0 || LightGridManager.previewLux[cell] > 0) ? Color.white : Color.black);
			break;
		case SimDebugView.GameGridMode.DigAmount:
			if (Grid.Element[cell].IsSolid)
			{
				float num = Grid.Damage[cell] / 255f;
				result = Color.HSVToRGB(1f - num, 1f, 1f);
			}
			break;
		case SimDebugView.GameGridMode.DupePassable:
			result = (Grid.DupePassable[cell] ? Color.white : Color.black);
			break;
		}
		return result;
	}

	// Token: 0x0600553F RID: 21823 RVA: 0x001F15D1 File Offset: 0x001EF7D1
	public Color32 GetColourForID(int id)
	{
		return this.networkColours[id % this.networkColours.Length];
	}

	// Token: 0x06005540 RID: 21824 RVA: 0x001F15E8 File Offset: 0x001EF7E8
	private static Color GetThermalConductivityColour(SimDebugView instance, int cell)
	{
		bool flag = SimDebugView.IsInsulated(cell);
		Color black = Color.black;
		float num = instance.maxThermalConductivity - instance.minThermalConductivity;
		if (!flag && num != 0f)
		{
			float num2 = (Grid.Element[cell].thermalConductivity - instance.minThermalConductivity) / num;
			num2 = Mathf.Max(num2, 0f);
			num2 = Mathf.Min(num2, 1f);
			black = new Color(num2, num2, num2);
		}
		return black;
	}

	// Token: 0x06005541 RID: 21825 RVA: 0x001F1654 File Offset: 0x001EF854
	private static Color GetPressureMapColour(SimDebugView instance, int cell)
	{
		Color32 c = Color.black;
		if (Grid.Pressure[cell] > 0f)
		{
			float num = Mathf.Clamp((Grid.Pressure[cell] - instance.minPressureExpected) / (instance.maxPressureExpected - instance.minPressureExpected), 0f, 1f) * 0.9f;
			c = new Color(num, num, num, 1f);
		}
		return c;
	}

	// Token: 0x06005542 RID: 21826 RVA: 0x001F16CC File Offset: 0x001EF8CC
	private static Color GetOxygenMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!Grid.IsLiquid(cell) && !Grid.Solid[cell])
		{
			if (Grid.Mass[cell] > SimDebugView.minimumBreathable && (Grid.Element[cell].id == SimHashes.Oxygen || Grid.Element[cell].id == SimHashes.ContaminatedOxygen))
			{
				float time = Mathf.Clamp((Grid.Mass[cell] - SimDebugView.minimumBreathable) / SimDebugView.optimallyBreathable, 0f, 1f);
				result = instance.breathableGradient.Evaluate(time);
			}
			else
			{
				result = instance.unbreathableColour;
			}
		}
		return result;
	}

	// Token: 0x06005543 RID: 21827 RVA: 0x001F1774 File Offset: 0x001EF974
	private static Color GetTileColour(SimDebugView instance, int cell)
	{
		float num = 0.33f;
		Color result = new Color(num, num, num);
		Element element = Grid.Element[cell];
		bool flag = false;
		foreach (Tag search_tag in Game.Instance.tileOverlayFilters)
		{
			if (element.HasTag(search_tag))
			{
				flag = true;
			}
		}
		if (flag)
		{
			result = element.substance.uiColour;
		}
		return result;
	}

	// Token: 0x06005544 RID: 21828 RVA: 0x001F1804 File Offset: 0x001EFA04
	private static Color GetTileTypeColour(SimDebugView instance, int cell)
	{
		return Grid.Element[cell].substance.uiColour;
	}

	// Token: 0x06005545 RID: 21829 RVA: 0x001F181C File Offset: 0x001EFA1C
	private static Color GetStateMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		switch (Grid.Element[cell].state & Element.State.Solid)
		{
		case Element.State.Gas:
			result = Color.yellow;
			break;
		case Element.State.Liquid:
			result = Color.green;
			break;
		case Element.State.Solid:
			result = Color.blue;
			break;
		}
		return result;
	}

	// Token: 0x06005546 RID: 21830 RVA: 0x001F1870 File Offset: 0x001EFA70
	private static Color GetSolidLiquidMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		switch (Grid.Element[cell].state & Element.State.Solid)
		{
		case Element.State.Liquid:
			result = Color.green;
			break;
		case Element.State.Solid:
			result = Color.blue;
			break;
		}
		return result;
	}

	// Token: 0x06005547 RID: 21831 RVA: 0x001F18BC File Offset: 0x001EFABC
	private static Color GetStateChangeColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		Element element = Grid.Element[cell];
		if (!element.IsVacuum)
		{
			float num = Grid.Temperature[cell];
			float num2 = element.lowTemp * 0.05f;
			float a = Mathf.Abs(num - element.lowTemp) / num2;
			float num3 = element.highTemp * 0.05f;
			float b = Mathf.Abs(num - element.highTemp) / num3;
			float t = Mathf.Max(0f, 1f - Mathf.Min(a, b));
			result = Color.Lerp(Color.black, Color.red, t);
		}
		return result;
	}

	// Token: 0x06005548 RID: 21832 RVA: 0x001F1954 File Offset: 0x001EFB54
	private static Color GetDecorColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!Grid.Solid[cell])
		{
			float num = GameUtil.GetDecorAtCell(cell) / 100f;
			if (num > 0f)
			{
				result = Color.Lerp(GlobalAssets.Instance.colorSet.decorBaseline, GlobalAssets.Instance.colorSet.decorPositive, Mathf.Abs(num));
			}
			else
			{
				result = Color.Lerp(GlobalAssets.Instance.colorSet.decorBaseline, GlobalAssets.Instance.colorSet.decorNegative, Mathf.Abs(num));
			}
		}
		return result;
	}

	// Token: 0x06005549 RID: 21833 RVA: 0x001F19F4 File Offset: 0x001EFBF4
	private static Color GetDangerColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		SimDebugView.DangerAmount dangerAmount = SimDebugView.DangerAmount.None;
		if (!Grid.Element[cell].IsSolid)
		{
			float num = 0f;
			if (Grid.Temperature[cell] < SimDebugView.minMinionTemperature)
			{
				num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.minMinionTemperature);
			}
			if (Grid.Temperature[cell] > SimDebugView.maxMinionTemperature)
			{
				num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.maxMinionTemperature);
			}
			if (num > 0f)
			{
				if (num < 10f)
				{
					dangerAmount = SimDebugView.DangerAmount.VeryLow;
				}
				else if (num < 30f)
				{
					dangerAmount = SimDebugView.DangerAmount.Low;
				}
				else if (num < 100f)
				{
					dangerAmount = SimDebugView.DangerAmount.Moderate;
				}
				else if (num < 200f)
				{
					dangerAmount = SimDebugView.DangerAmount.High;
				}
				else if (num < 400f)
				{
					dangerAmount = SimDebugView.DangerAmount.VeryHigh;
				}
				else if (num > 800f)
				{
					dangerAmount = SimDebugView.DangerAmount.Extreme;
				}
			}
		}
		if (dangerAmount < SimDebugView.DangerAmount.VeryHigh && (Grid.Element[cell].IsVacuum || (Grid.Element[cell].IsGas && (Grid.Element[cell].id != SimHashes.Oxygen || Grid.Pressure[cell] < SimDebugView.minMinionPressure))))
		{
			dangerAmount++;
		}
		if (dangerAmount != SimDebugView.DangerAmount.None)
		{
			float num2 = (float)dangerAmount / 6f;
			result = Color.HSVToRGB((80f - num2 * 80f) / 360f, 1f, 1f);
		}
		return result;
	}

	// Token: 0x0600554A RID: 21834 RVA: 0x001F1B3C File Offset: 0x001EFD3C
	private static Color GetSimCheckErrorMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		Element element = Grid.Element[cell];
		float num = Grid.Mass[cell];
		float num2 = Grid.Temperature[cell];
		if (float.IsNaN(num) || float.IsNaN(num2) || num > 10000f || num2 > 10000f)
		{
			return Color.red;
		}
		if (element.IsVacuum)
		{
			if (num2 != 0f)
			{
				result = Color.yellow;
			}
			else if (num != 0f)
			{
				result = Color.blue;
			}
			else
			{
				result = Color.gray;
			}
		}
		else if (num2 < 10f)
		{
			result = Color.red;
		}
		else if (Grid.Mass[cell] < 1f && Grid.Pressure[cell] < 1f)
		{
			result = Color.green;
		}
		else if (num2 > element.highTemp + 3f && element.highTempTransition != null)
		{
			result = Color.magenta;
		}
		else if (num2 < element.lowTemp + 3f && element.lowTempTransition != null)
		{
			result = Color.cyan;
		}
		return result;
	}

	// Token: 0x0600554B RID: 21835 RVA: 0x001F1C44 File Offset: 0x001EFE44
	private static Color GetFakeFloorColour(SimDebugView instance, int cell)
	{
		if (!Grid.FakeFloor[cell])
		{
			return Color.black;
		}
		return Color.cyan;
	}

	// Token: 0x0600554C RID: 21836 RVA: 0x001F1C5E File Offset: 0x001EFE5E
	private static Color GetFoundationColour(SimDebugView instance, int cell)
	{
		if (!Grid.Foundation[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x0600554D RID: 21837 RVA: 0x001F1C78 File Offset: 0x001EFE78
	private static Color GetDupePassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.DupePassable[cell])
		{
			return Color.black;
		}
		return Color.green;
	}

	// Token: 0x0600554E RID: 21838 RVA: 0x001F1C92 File Offset: 0x001EFE92
	private static Color GetCritterImpassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.CritterImpassable[cell])
		{
			return Color.black;
		}
		return Color.yellow;
	}

	// Token: 0x0600554F RID: 21839 RVA: 0x001F1CAC File Offset: 0x001EFEAC
	private static Color GetDupeImpassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.DupeImpassable[cell])
		{
			return Color.black;
		}
		return Color.red;
	}

	// Token: 0x06005550 RID: 21840 RVA: 0x001F1CC6 File Offset: 0x001EFEC6
	private static Color GetMinionOccupiedColour(SimDebugView instance, int cell)
	{
		if (!(Grid.Objects[cell, 0] != null))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06005551 RID: 21841 RVA: 0x001F1CE7 File Offset: 0x001EFEE7
	private static Color GetMinionGroupProberColour(SimDebugView instance, int cell)
	{
		if (!MinionGroupProber.Get().IsReachable(cell))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06005552 RID: 21842 RVA: 0x001F1D01 File Offset: 0x001EFF01
	private static Color GetPathProberColour(SimDebugView instance, int cell)
	{
		if (!(instance.selectedNavigator != null) || instance.selectedNavigator.PathGrid.GetCost(cell) == -1)
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06005553 RID: 21843 RVA: 0x001F1D30 File Offset: 0x001EFF30
	private static Color GetReservedColour(SimDebugView instance, int cell)
	{
		if (!Grid.Reserved[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06005554 RID: 21844 RVA: 0x001F1D4A File Offset: 0x001EFF4A
	private static Color GetAllowPathFindingColour(SimDebugView instance, int cell)
	{
		if (!Grid.AllowPathfinding[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06005555 RID: 21845 RVA: 0x001F1D64 File Offset: 0x001EFF64
	private static Color GetMassColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!SimDebugView.IsInsulated(cell))
		{
			float num = Grid.Mass[cell];
			if (num > 0f)
			{
				float num2 = (num - SimDebugView.Instance.minMassExpected) / (SimDebugView.Instance.maxMassExpected - SimDebugView.Instance.minMassExpected);
				result = Color.HSVToRGB(1f - num2, 1f, 1f);
			}
		}
		return result;
	}

	// Token: 0x06005556 RID: 21846 RVA: 0x001F1DCE File Offset: 0x001EFFCE
	public static Color GetScenePartitionerColour(SimDebugView instance, int cell)
	{
		if (!GameScenePartitioner.Instance.DoDebugLayersContainItemsOnCell(cell))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x04003976 RID: 14710
	[SerializeField]
	public Material material;

	// Token: 0x04003977 RID: 14711
	public Material diseaseMaterial;

	// Token: 0x04003978 RID: 14712
	public bool hideFOW;

	// Token: 0x04003979 RID: 14713
	public const int colourSize = 4;

	// Token: 0x0400397A RID: 14714
	private byte[] texBytes;

	// Token: 0x0400397B RID: 14715
	private int currentFrame;

	// Token: 0x0400397C RID: 14716
	[SerializeField]
	private Texture2D tex;

	// Token: 0x0400397D RID: 14717
	[SerializeField]
	private GameObject plane;

	// Token: 0x0400397E RID: 14718
	private HashedString mode = global::OverlayModes.Power.ID;

	// Token: 0x0400397F RID: 14719
	private SimDebugView.GameGridMode gameGridMode = SimDebugView.GameGridMode.DigAmount;

	// Token: 0x04003980 RID: 14720
	private Navigator selectedNavigator;

	// Token: 0x04003981 RID: 14721
	public float minTempExpected = 173.15f;

	// Token: 0x04003982 RID: 14722
	public float maxTempExpected = 423.15f;

	// Token: 0x04003983 RID: 14723
	public float minMassExpected = 1.0001f;

	// Token: 0x04003984 RID: 14724
	public float maxMassExpected = 10000f;

	// Token: 0x04003985 RID: 14725
	public float minPressureExpected = 1.300003f;

	// Token: 0x04003986 RID: 14726
	public float maxPressureExpected = 201.3f;

	// Token: 0x04003987 RID: 14727
	public float minThermalConductivity;

	// Token: 0x04003988 RID: 14728
	public float maxThermalConductivity = 30f;

	// Token: 0x04003989 RID: 14729
	public float thresholdRange = 0.001f;

	// Token: 0x0400398A RID: 14730
	public float thresholdOpacity = 0.8f;

	// Token: 0x0400398B RID: 14731
	public static float minimumBreathable = 0.05f;

	// Token: 0x0400398C RID: 14732
	public static float optimallyBreathable = 1f;

	// Token: 0x0400398D RID: 14733
	public SimDebugView.ColorThreshold[] temperatureThresholds;

	// Token: 0x0400398E RID: 14734
	public Vector2 user_temperatureThresholds = Vector2.zero;

	// Token: 0x0400398F RID: 14735
	public SimDebugView.ColorThreshold[] heatFlowThresholds;

	// Token: 0x04003990 RID: 14736
	public Color32[] networkColours;

	// Token: 0x04003991 RID: 14737
	public Gradient breathableGradient = new Gradient();

	// Token: 0x04003992 RID: 14738
	public Color32 unbreathableColour = new Color(0.5f, 0f, 0f);

	// Token: 0x04003993 RID: 14739
	public Color32[] toxicColour = new Color32[]
	{
		new Color(0.5f, 0f, 0.5f),
		new Color(1f, 0f, 1f)
	};

	// Token: 0x04003994 RID: 14740
	public static SimDebugView Instance;

	// Token: 0x04003995 RID: 14741
	private WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData> updateSimViewWorkItems = new WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData>();

	// Token: 0x04003996 RID: 14742
	private int selectedCell;

	// Token: 0x04003997 RID: 14743
	private Dictionary<HashedString, Action<SimDebugView, Texture>> dataUpdateFuncs = new Dictionary<HashedString, Action<SimDebugView, Texture>>
	{
		{
			global::OverlayModes.Temperature.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.Oxygen.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.Decor.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.TileMode.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint)
		},
		{
			global::OverlayModes.Disease.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDisease)
		}
	};

	// Token: 0x04003998 RID: 14744
	private static float[] relativeTemperatureColorIntervals = new float[]
	{
		0.4f,
		0.05f,
		0.05f,
		0.05f,
		0.05f,
		0.2f,
		0.2f
	};

	// Token: 0x04003999 RID: 14745
	private static float[] absoluteTemperatureColorIntervals = new float[]
	{
		273.15f,
		10f,
		10f,
		10f,
		7f,
		63f,
		1700f,
		10000f
	};

	// Token: 0x0400399A RID: 14746
	private Dictionary<HashedString, Func<SimDebugView, int, Color>> getColourFuncs = new Dictionary<HashedString, Func<SimDebugView, int, Color>>
	{
		{
			global::OverlayModes.ThermalConductivity.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetThermalConductivityColour)
		},
		{
			global::OverlayModes.Temperature.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetNormalizedTemperatureColourMode)
		},
		{
			global::OverlayModes.Disease.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDiseaseColour)
		},
		{
			global::OverlayModes.Decor.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDecorColour)
		},
		{
			global::OverlayModes.Oxygen.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetOxygenMapColour)
		},
		{
			global::OverlayModes.Light.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetLightColour)
		},
		{
			global::OverlayModes.Radiation.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetRadiationColour)
		},
		{
			global::OverlayModes.Rooms.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetRoomsColour)
		},
		{
			global::OverlayModes.TileMode.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetTileColour)
		},
		{
			global::OverlayModes.Suit.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Priorities.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Crop.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Harvest.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			SimDebugView.OverlayModes.GameGrid,
			new Func<SimDebugView, int, Color>(SimDebugView.GetGameGridColour)
		},
		{
			SimDebugView.OverlayModes.StateChange,
			new Func<SimDebugView, int, Color>(SimDebugView.GetStateChangeColour)
		},
		{
			SimDebugView.OverlayModes.SimCheckErrorMap,
			new Func<SimDebugView, int, Color>(SimDebugView.GetSimCheckErrorMapColour)
		},
		{
			SimDebugView.OverlayModes.Foundation,
			new Func<SimDebugView, int, Color>(SimDebugView.GetFoundationColour)
		},
		{
			SimDebugView.OverlayModes.FakeFloor,
			new Func<SimDebugView, int, Color>(SimDebugView.GetFakeFloorColour)
		},
		{
			SimDebugView.OverlayModes.DupePassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDupePassableColour)
		},
		{
			SimDebugView.OverlayModes.DupeImpassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDupeImpassableColour)
		},
		{
			SimDebugView.OverlayModes.CritterImpassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetCritterImpassableColour)
		},
		{
			SimDebugView.OverlayModes.MinionGroupProber,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionGroupProberColour)
		},
		{
			SimDebugView.OverlayModes.PathProber,
			new Func<SimDebugView, int, Color>(SimDebugView.GetPathProberColour)
		},
		{
			SimDebugView.OverlayModes.Reserved,
			new Func<SimDebugView, int, Color>(SimDebugView.GetReservedColour)
		},
		{
			SimDebugView.OverlayModes.AllowPathFinding,
			new Func<SimDebugView, int, Color>(SimDebugView.GetAllowPathFindingColour)
		},
		{
			SimDebugView.OverlayModes.Danger,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDangerColour)
		},
		{
			SimDebugView.OverlayModes.MinionOccupied,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionOccupiedColour)
		},
		{
			SimDebugView.OverlayModes.Pressure,
			new Func<SimDebugView, int, Color>(SimDebugView.GetPressureMapColour)
		},
		{
			SimDebugView.OverlayModes.TileType,
			new Func<SimDebugView, int, Color>(SimDebugView.GetTileTypeColour)
		},
		{
			SimDebugView.OverlayModes.State,
			new Func<SimDebugView, int, Color>(SimDebugView.GetStateMapColour)
		},
		{
			SimDebugView.OverlayModes.SolidLiquid,
			new Func<SimDebugView, int, Color>(SimDebugView.GetSolidLiquidMapColour)
		},
		{
			SimDebugView.OverlayModes.Mass,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMassColour)
		},
		{
			SimDebugView.OverlayModes.Joules,
			new Func<SimDebugView, int, Color>(SimDebugView.GetJoulesColour)
		},
		{
			SimDebugView.OverlayModes.MinionNavTableIsValid,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionNavTableIsValid)
		},
		{
			SimDebugView.OverlayModes.MinionAsyncRenderDelta,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionAsyncRenderDataColour)
		},
		{
			SimDebugView.OverlayModes.ScenePartitioner,
			new Func<SimDebugView, int, Color>(SimDebugView.GetScenePartitionerColour)
		}
	};

	// Token: 0x0400399B RID: 14747
	public static readonly Color[] dbColours = new Color[]
	{
		new Color(0f, 0f, 0f, 0f),
		new Color(1f, 1f, 1f, 0.3f),
		new Color(0.7058824f, 0.8235294f, 1f, 0.2f),
		new Color(0f, 0.3137255f, 1f, 0.3f),
		new Color(0.7058824f, 1f, 0.7058824f, 0.5f),
		new Color(0.078431375f, 1f, 0f, 0.7f),
		new Color(1f, 0.9019608f, 0.7058824f, 0.9f),
		new Color(1f, 0.8235294f, 0f, 0.9f),
		new Color(1f, 0.7176471f, 0.3019608f, 0.9f),
		new Color(1f, 0.41568628f, 0f, 0.9f),
		new Color(1f, 0.7058824f, 0.7058824f, 1f),
		new Color(1f, 0f, 0f, 1f),
		new Color(1f, 0f, 0f, 1f)
	};

	// Token: 0x0400399C RID: 14748
	private static float minMinionTemperature = 260f;

	// Token: 0x0400399D RID: 14749
	private static float maxMinionTemperature = 310f;

	// Token: 0x0400399E RID: 14750
	private static float minMinionPressure = 80f;

	// Token: 0x02001CAE RID: 7342
	public static class OverlayModes
	{
		// Token: 0x040088C5 RID: 35013
		public static readonly HashedString Mass = "Mass";

		// Token: 0x040088C6 RID: 35014
		public static readonly HashedString Pressure = "Pressure";

		// Token: 0x040088C7 RID: 35015
		public static readonly HashedString GameGrid = "GameGrid";

		// Token: 0x040088C8 RID: 35016
		public static readonly HashedString ScenePartitioner = "ScenePartitioner";

		// Token: 0x040088C9 RID: 35017
		public static readonly HashedString ConduitUpdates = "ConduitUpdates";

		// Token: 0x040088CA RID: 35018
		public static readonly HashedString Flow = "Flow";

		// Token: 0x040088CB RID: 35019
		public static readonly HashedString StateChange = "StateChange";

		// Token: 0x040088CC RID: 35020
		public static readonly HashedString SimCheckErrorMap = "SimCheckErrorMap";

		// Token: 0x040088CD RID: 35021
		public static readonly HashedString DupePassable = "DupePassable";

		// Token: 0x040088CE RID: 35022
		public static readonly HashedString Foundation = "Foundation";

		// Token: 0x040088CF RID: 35023
		public static readonly HashedString FakeFloor = "FakeFloor";

		// Token: 0x040088D0 RID: 35024
		public static readonly HashedString CritterImpassable = "CritterImpassable";

		// Token: 0x040088D1 RID: 35025
		public static readonly HashedString DupeImpassable = "DupeImpassable";

		// Token: 0x040088D2 RID: 35026
		public static readonly HashedString MinionGroupProber = "MinionGroupProber";

		// Token: 0x040088D3 RID: 35027
		public static readonly HashedString PathProber = "PathProber";

		// Token: 0x040088D4 RID: 35028
		public static readonly HashedString Reserved = "Reserved";

		// Token: 0x040088D5 RID: 35029
		public static readonly HashedString AllowPathFinding = "AllowPathFinding";

		// Token: 0x040088D6 RID: 35030
		public static readonly HashedString Danger = "Danger";

		// Token: 0x040088D7 RID: 35031
		public static readonly HashedString MinionOccupied = "MinionOccupied";

		// Token: 0x040088D8 RID: 35032
		public static readonly HashedString TileType = "TileType";

		// Token: 0x040088D9 RID: 35033
		public static readonly HashedString State = "State";

		// Token: 0x040088DA RID: 35034
		public static readonly HashedString SolidLiquid = "SolidLiquid";

		// Token: 0x040088DB RID: 35035
		public static readonly HashedString Joules = "Joules";

		// Token: 0x040088DC RID: 35036
		public static readonly HashedString MinionNavTableIsValid = "MinionNavTableIsValid";

		// Token: 0x040088DD RID: 35037
		public static readonly HashedString MinionAsyncRenderDelta = "MinionAsyncRenderDelta";
	}

	// Token: 0x02001CAF RID: 7343
	public enum GameGridMode
	{
		// Token: 0x040088DF RID: 35039
		GameSolidMap,
		// Token: 0x040088E0 RID: 35040
		Lighting,
		// Token: 0x040088E1 RID: 35041
		RoomMap,
		// Token: 0x040088E2 RID: 35042
		Style,
		// Token: 0x040088E3 RID: 35043
		PlantDensity,
		// Token: 0x040088E4 RID: 35044
		DigAmount,
		// Token: 0x040088E5 RID: 35045
		DupePassable
	}

	// Token: 0x02001CB0 RID: 7344
	[Serializable]
	public struct ColorThreshold
	{
		// Token: 0x040088E6 RID: 35046
		public string colorName;

		// Token: 0x040088E7 RID: 35047
		public float value;
	}

	// Token: 0x02001CB1 RID: 7345
	private struct UpdateSimViewSharedData
	{
		// Token: 0x0600AE4F RID: 44623 RVA: 0x003D3040 File Offset: 0x003D1240
		public UpdateSimViewSharedData(SimDebugView instance, byte[] texture_bytes, HashedString sim_view_mode, SimDebugView sim_debug_view)
		{
			this.instance = instance;
			this.textureBytes = texture_bytes;
			this.simViewMode = sim_view_mode;
			this.simDebugView = sim_debug_view;
		}

		// Token: 0x040088E8 RID: 35048
		public SimDebugView instance;

		// Token: 0x040088E9 RID: 35049
		public HashedString simViewMode;

		// Token: 0x040088EA RID: 35050
		public SimDebugView simDebugView;

		// Token: 0x040088EB RID: 35051
		public byte[] textureBytes;
	}

	// Token: 0x02001CB2 RID: 7346
	private struct UpdateSimViewWorkItem : IWorkItem<SimDebugView.UpdateSimViewSharedData>
	{
		// Token: 0x0600AE50 RID: 44624 RVA: 0x003D3060 File Offset: 0x003D1260
		public UpdateSimViewWorkItem(int x0, int y0, int x1, int y1)
		{
			this.x0 = Mathf.Clamp(x0, 0, Grid.WidthInCells - 1);
			this.x1 = Mathf.Clamp(x1, 0, Grid.WidthInCells - 1);
			this.y0 = Mathf.Clamp(y0, 0, Grid.HeightInCells - 1);
			this.y1 = Mathf.Clamp(y1, 0, Grid.HeightInCells - 1);
		}

		// Token: 0x0600AE51 RID: 44625 RVA: 0x003D30C0 File Offset: 0x003D12C0
		public void Run(SimDebugView.UpdateSimViewSharedData shared_data, int threadIndex)
		{
			Func<SimDebugView, int, Color> func;
			if (!shared_data.instance.getColourFuncs.TryGetValue(shared_data.simViewMode, out func))
			{
				func = new Func<SimDebugView, int, Color>(SimDebugView.GetBlack);
			}
			for (int i = this.y0; i <= this.y1; i++)
			{
				int num = Grid.XYToCell(this.x0, i);
				int num2 = Grid.XYToCell(this.x1, i);
				for (int j = num; j <= num2; j++)
				{
					int num3 = j * 4;
					if (Grid.IsActiveWorld(j))
					{
						Color color = func(shared_data.instance, j);
						shared_data.textureBytes[num3] = (byte)(Mathf.Min(color.r, 1f) * 255f);
						shared_data.textureBytes[num3 + 1] = (byte)(Mathf.Min(color.g, 1f) * 255f);
						shared_data.textureBytes[num3 + 2] = (byte)(Mathf.Min(color.b, 1f) * 255f);
						shared_data.textureBytes[num3 + 3] = (byte)(Mathf.Min(color.a, 1f) * 255f);
					}
					else
					{
						shared_data.textureBytes[num3] = 0;
						shared_data.textureBytes[num3 + 1] = 0;
						shared_data.textureBytes[num3 + 2] = 0;
						shared_data.textureBytes[num3 + 3] = 0;
					}
				}
			}
		}

		// Token: 0x040088EC RID: 35052
		private int x0;

		// Token: 0x040088ED RID: 35053
		private int y0;

		// Token: 0x040088EE RID: 35054
		private int x1;

		// Token: 0x040088EF RID: 35055
		private int y1;
	}

	// Token: 0x02001CB3 RID: 7347
	public enum DangerAmount
	{
		// Token: 0x040088F1 RID: 35057
		None,
		// Token: 0x040088F2 RID: 35058
		VeryLow,
		// Token: 0x040088F3 RID: 35059
		Low,
		// Token: 0x040088F4 RID: 35060
		Moderate,
		// Token: 0x040088F5 RID: 35061
		High,
		// Token: 0x040088F6 RID: 35062
		VeryHigh,
		// Token: 0x040088F7 RID: 35063
		Extreme,
		// Token: 0x040088F8 RID: 35064
		MAX_DANGERAMOUNT = 6
	}
}
