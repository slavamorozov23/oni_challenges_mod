using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

// Token: 0x02000AB6 RID: 2742
[AddComponentMenu("KMonoBehaviour/scripts/PropertyTextures")]
public class PropertyTextures : KMonoBehaviour, ISim200ms
{
	// Token: 0x06004FAD RID: 20397 RVA: 0x001CE62C File Offset: 0x001CC82C
	public static void DestroyInstance()
	{
		ShaderReloader.Unregister(new System.Action(PropertyTextures.instance.OnShadersReloaded));
		PropertyTextures.externalFlowTex = IntPtr.Zero;
		PropertyTextures.externalLiquidTex = IntPtr.Zero;
		PropertyTextures.externalLiquidDataTex = IntPtr.Zero;
		PropertyTextures.externalExposedToSunlight = IntPtr.Zero;
		PropertyTextures.externalSolidDigAmountTex = IntPtr.Zero;
		PropertyTextures.instance = null;
	}

	// Token: 0x06004FAE RID: 20398 RVA: 0x001CE686 File Offset: 0x001CC886
	protected override void OnPrefabInit()
	{
		PropertyTextures.instance = this;
		base.OnPrefabInit();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
	}

	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x06004FAF RID: 20399 RVA: 0x001CE6A5 File Offset: 0x001CC8A5
	public static bool IsFogOfWarEnabled
	{
		get
		{
			return PropertyTextures.FogOfWarScale < 1f;
		}
	}

	// Token: 0x06004FB0 RID: 20400 RVA: 0x001CE6B3 File Offset: 0x001CC8B3
	public Texture GetTexture(PropertyTextures.Property property)
	{
		return this.textureBuffers[(int)property].texture;
	}

	// Token: 0x06004FB1 RID: 20401 RVA: 0x001CE6C2 File Offset: 0x001CC8C2
	private string GetShaderPropertyName(PropertyTextures.Property property)
	{
		return "_" + property.ToString() + "Tex";
	}

	// Token: 0x06004FB2 RID: 20402 RVA: 0x001CE6E0 File Offset: 0x001CC8E0
	protected override void OnSpawn()
	{
		if (GenericGameSettings.instance.disableFogOfWar)
		{
			PropertyTextures.FogOfWarScale = 1f;
		}
		this.WorldSizeID = Shader.PropertyToID("_WorldSizeInfo");
		this.ClusterWorldSizeID = Shader.PropertyToID("_ClusterWorldSizeInfo");
		this.FogOfWarScaleID = Shader.PropertyToID("_FogOfWarScale");
		this.PropTexWsToCsID = Shader.PropertyToID("_PropTexWsToCs");
		this.PropTexCsToWsID = Shader.PropertyToID("_PropTexCsToWs");
		this.TopBorderHeightID = Shader.PropertyToID("_TopBorderHeight");
		this.CameraZoomID = Shader.PropertyToID("_CameraZoomInfo");
	}

	// Token: 0x06004FB3 RID: 20403 RVA: 0x001CE774 File Offset: 0x001CC974
	public void OnReset(object data = null)
	{
		this.lerpers = new TextureLerper[16];
		this.texturePagePool = new TexturePagePool();
		this.textureBuffers = new TextureBuffer[16];
		this.externallyUpdatedTextures = new Texture2D[16];
		for (int i = 0; i < 16; i++)
		{
			PropertyTextures.TextureProperties textureProperties = new PropertyTextures.TextureProperties
			{
				textureFormat = TextureFormat.Alpha8,
				filterMode = FilterMode.Bilinear,
				blend = false,
				blendSpeed = 1f
			};
			for (int j = 0; j < this.textureProperties.Length; j++)
			{
				if (i == (int)this.textureProperties[j].simProperty)
				{
					textureProperties = this.textureProperties[j];
				}
			}
			PropertyTextures.Property property = (PropertyTextures.Property)i;
			textureProperties.name = property.ToString();
			if (this.externallyUpdatedTextures[i] != null)
			{
				UnityEngine.Object.Destroy(this.externallyUpdatedTextures[i]);
				this.externallyUpdatedTextures[i] = null;
			}
			Texture texture;
			if (textureProperties.updatedExternally)
			{
				this.externallyUpdatedTextures[i] = new Texture2D(Grid.WidthInCells, Grid.HeightInCells, TextureUtil.TextureFormatToGraphicsFormat(textureProperties.textureFormat), TextureCreationFlags.None);
				texture = this.externallyUpdatedTextures[i];
			}
			else
			{
				TextureBuffer[] array = this.textureBuffers;
				int num = i;
				property = (PropertyTextures.Property)i;
				array[num] = new TextureBuffer(property.ToString(), Grid.WidthInCells, Grid.HeightInCells, textureProperties.textureFormat, textureProperties.filterMode, this.texturePagePool);
				texture = this.textureBuffers[i].texture;
			}
			if (textureProperties.blend)
			{
				TextureLerper[] array2 = this.lerpers;
				int num2 = i;
				Texture target_texture = texture;
				property = (PropertyTextures.Property)i;
				array2[num2] = new TextureLerper(target_texture, property.ToString(), texture.filterMode, textureProperties.textureFormat);
				this.lerpers[i].Speed = textureProperties.blendSpeed;
			}
			string shaderPropertyName = this.GetShaderPropertyName((PropertyTextures.Property)i);
			texture.name = shaderPropertyName;
			textureProperties.texturePropertyName = shaderPropertyName;
			Shader.SetGlobalTexture(shaderPropertyName, texture);
			this.allTextureProperties.Add(textureProperties);
		}
	}

	// Token: 0x06004FB4 RID: 20404 RVA: 0x001CE958 File Offset: 0x001CCB58
	private void OnShadersReloaded()
	{
		for (int i = 0; i < 16; i++)
		{
			TextureLerper textureLerper = this.lerpers[i];
			if (textureLerper != null)
			{
				Shader.SetGlobalTexture(this.allTextureProperties[i].texturePropertyName, textureLerper.Update());
			}
		}
	}

	// Token: 0x06004FB5 RID: 20405 RVA: 0x001CE99C File Offset: 0x001CCB9C
	public void Sim200ms(float dt)
	{
		if (this.lerpers == null || this.lerpers.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.lerpers.Length; i++)
		{
			TextureLerper textureLerper = this.lerpers[i];
			if (textureLerper != null)
			{
				textureLerper.LongUpdate(dt);
			}
		}
	}

	// Token: 0x06004FB6 RID: 20406 RVA: 0x001CE9E4 File Offset: 0x001CCBE4
	private void UpdateTextureThreaded(TextureRegion texture_region, int x0, int y0, int x1, int y1, PropertyTextures.WorkItem.Callback update_texture_cb)
	{
		this.workItems.Reset(null);
		int num = 16;
		for (int i = y0; i <= y1; i += num)
		{
			int y2 = Math.Min(i + num - 1, y1);
			this.workItems.Add(new PropertyTextures.WorkItem(texture_region, x0, i, x1, y2, update_texture_cb));
		}
		GlobalJobManager.Run(this.workItems);
	}

	// Token: 0x06004FB7 RID: 20407 RVA: 0x001CEA40 File Offset: 0x001CCC40
	private void UpdateProperty(ref PropertyTextures.TextureProperties p, int x0, int y0, int x1, int y1)
	{
		if (Game.Instance == null || Game.Instance.IsLoading())
		{
			return;
		}
		int simProperty = (int)p.simProperty;
		if (!p.updatedExternally)
		{
			TextureRegion texture_region = this.textureBuffers[simProperty].Lock(x0, y0, x1 - x0 + 1, y1 - y0 + 1);
			switch (p.simProperty)
			{
			case PropertyTextures.Property.StateChange:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateStateChange));
				break;
			case PropertyTextures.Property.GasPressure:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdatePressure));
				break;
			case PropertyTextures.Property.GasColour:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateGasColour));
				break;
			case PropertyTextures.Property.GasDanger:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateDanger));
				break;
			case PropertyTextures.Property.FogOfWar:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateFogOfWar));
				break;
			case PropertyTextures.Property.SolidDigAmount:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateSolidDigAmount));
				break;
			case PropertyTextures.Property.SolidLiquidGasMass:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateSolidLiquidGasMass));
				break;
			case PropertyTextures.Property.WorldLight:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateWorldLight));
				break;
			case PropertyTextures.Property.Temperature:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateTemperature));
				break;
			case PropertyTextures.Property.FallingSolid:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateFallingSolidChange));
				break;
			case PropertyTextures.Property.Radiation:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateRadiation));
				break;
			case PropertyTextures.Property.SolidLiquidGasMassForLight:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateSolidLiquidGasMassForLight));
				break;
			}
			texture_region.Unlock();
			return;
		}
		PropertyTextures.Property simProperty2 = p.simProperty;
		if (simProperty2 <= PropertyTextures.Property.Liquid)
		{
			if (simProperty2 != PropertyTextures.Property.Flow)
			{
				if (simProperty2 == PropertyTextures.Property.Liquid)
				{
					this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalLiquidTex, 4 * Grid.WidthInCells * Grid.HeightInCells);
				}
			}
			else
			{
				this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalFlowTex, 8 * Grid.WidthInCells * Grid.HeightInCells);
			}
		}
		else if (simProperty2 != PropertyTextures.Property.ExposedToSunlight)
		{
			if (simProperty2 == PropertyTextures.Property.LiquidData)
			{
				this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalLiquidDataTex, 4 * Grid.WidthInCells * Grid.HeightInCells);
			}
		}
		else
		{
			this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalExposedToSunlight, Grid.WidthInCells * Grid.HeightInCells);
		}
		this.externallyUpdatedTextures[simProperty].Apply();
	}

	// Token: 0x06004FB8 RID: 20408 RVA: 0x001CECFC File Offset: 0x001CCEFC
	public static Vector4 CalculateClusterWorldSize()
	{
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		Vector2I worldOffset = activeWorld.WorldOffset;
		Vector2I worldSize = activeWorld.WorldSize;
		Vector4 zero = Vector4.zero;
		if (DlcManager.IsPureVanilla() || (CameraController.Instance != null && CameraController.Instance.ignoreClusterFX))
		{
			zero = new Vector4((float)Grid.WidthInCells, (float)Grid.HeightInCells, 0f, 0f);
		}
		else
		{
			zero = new Vector4((float)worldSize.x, (float)(worldSize.y - activeWorld.HiddenYOffset), (float)worldOffset.x, (float)worldOffset.y);
		}
		return zero;
	}

	// Token: 0x06004FB9 RID: 20409 RVA: 0x001CED94 File Offset: 0x001CCF94
	private void LateUpdate()
	{
		if (!Grid.IsInitialized())
		{
			return;
		}
		Shader.SetGlobalVector(this.WorldSizeID, new Vector4((float)Grid.WidthInCells, (float)Grid.HeightInCells, 1f / (float)Grid.WidthInCells, 1f / (float)Grid.HeightInCells));
		Vector4 value = PropertyTextures.CalculateClusterWorldSize();
		float num = CameraController.Instance.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : 20f;
		Shader.SetGlobalVector(this.CameraZoomID, new Vector4(CameraController.Instance.OrthographicSize, CameraController.Instance.overlayCamera.aspect, num, (CameraController.Instance.OrthographicSize - CameraController.Instance.minOrthographicSize) / (num - CameraController.Instance.minOrthographicSize)));
		Shader.SetGlobalVector(this.ClusterWorldSizeID, value);
		Shader.SetGlobalVector(this.PropTexWsToCsID, new Vector4(0f, 0f, 1f, 1f));
		Shader.SetGlobalVector(this.PropTexCsToWsID, new Vector4(0f, 0f, 1f, 1f));
		Shader.SetGlobalFloat(this.TopBorderHeightID, ClusterManager.Instance.activeWorld.FullyEnclosedBorder ? 0f : ((float)Grid.TopBorderHeight));
		int x;
		int y;
		int x2;
		int y2;
		this.GetVisibleCellRange(out x, out y, out x2, out y2);
		Shader.SetGlobalFloat(this.FogOfWarScaleID, PropertyTextures.FogOfWarScale);
		int nextPropertyIdx = this.NextPropertyIdx;
		this.NextPropertyIdx = nextPropertyIdx + 1;
		int num2 = nextPropertyIdx % this.allTextureProperties.Count;
		PropertyTextures.TextureProperties textureProperties = this.allTextureProperties[num2];
		while (textureProperties.updateEveryFrame)
		{
			nextPropertyIdx = this.NextPropertyIdx;
			this.NextPropertyIdx = nextPropertyIdx + 1;
			num2 = nextPropertyIdx % this.allTextureProperties.Count;
			textureProperties = this.allTextureProperties[num2];
		}
		for (int i = 0; i < this.allTextureProperties.Count; i++)
		{
			PropertyTextures.TextureProperties textureProperties2 = this.allTextureProperties[i];
			if (num2 == i || textureProperties2.updateEveryFrame || GameUtil.IsCapturingTimeLapse())
			{
				this.UpdateProperty(ref textureProperties2, x, y, x2, y2);
			}
		}
		for (int j = 0; j < 16; j++)
		{
			TextureLerper textureLerper = this.lerpers[j];
			if (textureLerper != null)
			{
				if (Time.timeScale == 0f)
				{
					textureLerper.LongUpdate(Time.unscaledDeltaTime);
				}
				Shader.SetGlobalTexture(this.allTextureProperties[j].texturePropertyName, textureLerper.Update());
			}
		}
	}

	// Token: 0x06004FBA RID: 20410 RVA: 0x001CEFF8 File Offset: 0x001CD1F8
	private void GetVisibleCellRange(out int x0, out int y0, out int x1, out int y1)
	{
		int num = 16;
		Grid.GetVisibleExtents(out x0, out y0, out x1, out y1);
		int widthInCells = Grid.WidthInCells;
		int heightInCells = Grid.HeightInCells;
		int num2 = 0;
		int num3 = 0;
		x0 = Math.Max(num2, x0 - num);
		y0 = Math.Max(num3, y0 - num);
		x0 = Mathf.Min(x0, widthInCells - 1);
		y0 = Mathf.Min(y0, heightInCells - 1);
		x1 = Mathf.CeilToInt((float)(x1 + num));
		y1 = Mathf.CeilToInt((float)(y1 + num));
		x1 = Mathf.Max(x1, num2);
		y1 = Mathf.Max(y1, num3);
		x1 = Mathf.Min(x1, widthInCells - 1);
		y1 = Mathf.Min(y1, heightInCells - 1);
	}

	// Token: 0x06004FBB RID: 20411 RVA: 0x001CF0A0 File Offset: 0x001CD2A0
	private static void UpdateFogOfWar(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		byte[] visible = Grid.Visible;
		int y2 = Grid.HeightInCells;
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			y2 = activeWorld.WorldSize.y + activeWorld.WorldOffset.y - 1;
		}
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					int num2 = Grid.XYToCell(j, y2);
					if (Grid.IsValidCell(num2))
					{
						region.SetBytes(j, i, visible[num2]);
					}
					else
					{
						region.SetBytes(j, i, 0);
					}
				}
				else
				{
					region.SetBytes(j, i, visible[num]);
				}
			}
		}
	}

	// Token: 0x06004FBC RID: 20412 RVA: 0x001CF15C File Offset: 0x001CD35C
	private static void UpdatePressure(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		Vector2 pressureRange = PropertyTextures.instance.PressureRange;
		float minPressureVisibility = PropertyTextures.instance.MinPressureVisibility;
		float num = pressureRange.y - pressureRange.x;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num2 = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num2))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					float num3 = 0f;
					Element element = Grid.Element[num2];
					if (element.IsGas)
					{
						float num4 = Grid.Pressure[num2];
						float b = (num4 > 0f) ? minPressureVisibility : 0f;
						num3 = Mathf.Max(Mathf.Clamp01((num4 - pressureRange.x) / num), b);
					}
					else if (element.IsLiquid)
					{
						int num5 = Grid.CellAbove(num2);
						if (Grid.IsValidCell(num5) && Grid.Element[num5].IsGas)
						{
							float num6 = Grid.Pressure[num5];
							float b2 = (num6 > 0f) ? minPressureVisibility : 0f;
							num3 = Mathf.Max(Mathf.Clamp01((num6 - pressureRange.x) / num), b2);
						}
					}
					region.SetBytes(j, i, (byte)(num3 * 255f));
				}
			}
		}
	}

	// Token: 0x06004FBD RID: 20413 RVA: 0x001CF29C File Offset: 0x001CD49C
	private static void UpdateDanger(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					byte b = (Grid.Element[num].id == SimHashes.Oxygen) ? 0 : byte.MaxValue;
					region.SetBytes(j, i, b);
				}
			}
		}
	}

	// Token: 0x06004FBE RID: 20414 RVA: 0x001CF308 File Offset: 0x001CD508
	private static void UpdateStateChange(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		float temperatureStateChangeRange = PropertyTextures.instance.TemperatureStateChangeRange;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					float num2 = 0f;
					Element element = Grid.Element[num];
					if (!element.IsVacuum)
					{
						float num3 = Grid.Temperature[num];
						float num4 = element.lowTemp * temperatureStateChangeRange;
						float a = Mathf.Abs(num3 - element.lowTemp) / num4;
						float num5 = element.highTemp * temperatureStateChangeRange;
						float b = Mathf.Abs(num3 - element.highTemp) / num5;
						num2 = Mathf.Max(num2, 1f - Mathf.Min(a, b));
					}
					region.SetBytes(j, i, (byte)(num2 * 255f));
				}
			}
		}
	}

	// Token: 0x06004FBF RID: 20415 RVA: 0x001CF3F0 File Offset: 0x001CD5F0
	private static void UpdateFallingSolidChange(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					float num2 = 0f;
					Element element = Grid.Element[num];
					if (element.id == SimHashes.Mud || element.id == SimHashes.ToxicMud)
					{
						num2 = 0.65f;
					}
					region.SetBytes(j, i, (byte)(num2 * 255f));
				}
			}
		}
	}

	// Token: 0x06004FC0 RID: 20416 RVA: 0x001CF474 File Offset: 0x001CD674
	private static void UpdateGasColour(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0, 0);
				}
				else
				{
					Element element = Grid.Element[num];
					if (element.IsGas)
					{
						region.SetBytes(j, i, element.substance.colour.r, element.substance.colour.g, element.substance.colour.b, byte.MaxValue);
					}
					else if (element.IsLiquid)
					{
						if (Grid.IsValidCell(Grid.CellAbove(num)))
						{
							region.SetBytes(j, i, element.substance.colour.r, element.substance.colour.g, element.substance.colour.b, byte.MaxValue);
						}
						else
						{
							region.SetBytes(j, i, 0, 0, 0, 0);
						}
					}
					else
					{
						region.SetBytes(j, i, 0, 0, 0, 0);
					}
				}
			}
		}
	}

	// Token: 0x06004FC1 RID: 20417 RVA: 0x001CF58C File Offset: 0x001CD78C
	private static void UpdateLiquid(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = x0; i <= x1; i++)
		{
			int num = Grid.XYToCell(i, y1);
			Element element = Grid.Element[num];
			for (int j = y1; j >= y0; j--)
			{
				int num2 = Grid.XYToCell(i, j);
				if (!Grid.IsActiveWorld(num2))
				{
					region.SetBytes(i, j, 0, 0, 0, 0);
				}
				else
				{
					Element element2 = Grid.Element[num2];
					if (element2.IsLiquid)
					{
						Color32 colour = element2.substance.colour;
						float liquidMaxMass = Lighting.Instance.Settings.LiquidMaxMass;
						float liquidAmountOffset = Lighting.Instance.Settings.LiquidAmountOffset;
						float num3;
						if (element.IsLiquid || element.IsSolid)
						{
							num3 = 1f;
						}
						else
						{
							num3 = liquidAmountOffset + (1f - liquidAmountOffset) * Mathf.Min(Grid.Mass[num2] / liquidMaxMass, 1f);
							num3 = Mathf.Pow(Mathf.Min(Grid.Mass[num2] / liquidMaxMass, 1f), 0.45f);
						}
						region.SetBytes(i, j, (byte)(num3 * 255f), colour.r, colour.g, colour.b);
					}
					else
					{
						region.SetBytes(i, j, 0, 0, 0, 0);
					}
					element = element2;
				}
			}
		}
	}

	// Token: 0x06004FC2 RID: 20418 RVA: 0x001CF6D8 File Offset: 0x001CD8D8
	private static void UpdateSolidDigAmount(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		ushort elementIndex = ElementLoader.GetElementIndex(SimHashes.Void);
		for (int i = y0; i <= y1; i++)
		{
			int num = Grid.XYToCell(x0, i);
			int num2 = Grid.XYToCell(x1, i);
			int j = num;
			int num3 = x0;
			while (j <= num2)
			{
				byte b = 0;
				byte b2 = 0;
				byte b3 = 0;
				if (Grid.ElementIdx[j] != elementIndex)
				{
					b3 = byte.MaxValue;
				}
				if (Grid.Solid[j])
				{
					b = byte.MaxValue;
					b2 = (byte)(255f * Grid.Damage[j]);
				}
				region.SetBytes(num3, i, b, b2, b3);
				j++;
				num3++;
			}
		}
	}

	// Token: 0x06004FC3 RID: 20419 RVA: 0x001CF774 File Offset: 0x001CD974
	private static void UpdateSolidLiquidGasMassForLight(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		PropertyTextures.UpdateSolidLiquidGasMass(region, x0, y0, x1, y1, false);
	}

	// Token: 0x06004FC4 RID: 20420 RVA: 0x001CF782 File Offset: 0x001CD982
	private static void UpdateSolidLiquidGasMass(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		PropertyTextures.UpdateSolidLiquidGasMass(region, x0, y0, x1, y1, true);
	}

	// Token: 0x06004FC5 RID: 20421 RVA: 0x001CF790 File Offset: 0x001CD990
	private static void UpdateSolidLiquidGasMass(TextureRegion region, int x0, int y0, int x1, int y1, bool diferenciateImpermeable)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0, 0);
				}
				else
				{
					Element element = Grid.Element[num];
					byte b = 0;
					byte b2 = 0;
					byte b3 = 0;
					if (element.IsSolid || (diferenciateImpermeable && Grid.LiquidImpermeable[num]))
					{
						if (element.IsSolid)
						{
							b = byte.MaxValue;
						}
						else
						{
							b = 200;
						}
					}
					else if (element.IsLiquid)
					{
						b2 = byte.MaxValue;
					}
					else if (element.IsGas || element.IsVacuum)
					{
						b3 = byte.MaxValue;
					}
					float num2 = Grid.Mass[num];
					float num3 = Mathf.Min(1f, num2 / 2000f);
					if (num2 > 0f)
					{
						num3 = Mathf.Max(0.003921569f, num3);
					}
					region.SetBytes(j, i, b, b2, b3, (byte)(num3 * 255f));
				}
			}
		}
	}

	// Token: 0x06004FC6 RID: 20422 RVA: 0x001CF8A0 File Offset: 0x001CDAA0
	private static void GetTemperatureAlpha(float t, Vector2 cold_range, Vector2 hot_range, out byte cold_alpha, out byte hot_alpha)
	{
		cold_alpha = 0;
		hot_alpha = 0;
		if (t <= cold_range.y)
		{
			float num = Mathf.Clamp01((cold_range.y - t) / (cold_range.y - cold_range.x));
			cold_alpha = (byte)(num * 255f);
			return;
		}
		if (t >= hot_range.x)
		{
			float num2 = Mathf.Clamp01((t - hot_range.x) / (hot_range.y - hot_range.x));
			hot_alpha = (byte)(num2 * 255f);
		}
	}

	// Token: 0x06004FC7 RID: 20423 RVA: 0x001CF914 File Offset: 0x001CDB14
	private static void UpdateTemperature(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		Vector2 cold_range = PropertyTextures.instance.coldRange;
		Vector2 hot_range = PropertyTextures.instance.hotRange;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0);
				}
				else
				{
					float num2 = Grid.Temperature[num];
					byte b;
					byte b2;
					PropertyTextures.GetTemperatureAlpha(num2, cold_range, hot_range, out b, out b2);
					byte b3 = (byte)(255f * Mathf.Pow(Mathf.Clamp(num2 / 1000f, 0f, 1f), 0.45f));
					region.SetBytes(j, i, b, b2, b3);
				}
			}
		}
	}

	// Token: 0x06004FC8 RID: 20424 RVA: 0x001CF9CC File Offset: 0x001CDBCC
	private static void UpdateWorldLight(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		if (!PropertyTextures.instance.ForceLightEverywhere)
		{
			for (int i = y0; i <= y1; i++)
			{
				int num = Grid.XYToCell(x0, i);
				int num2 = Grid.XYToCell(x1, i);
				int j = num;
				int num3 = x0;
				while (j <= num2)
				{
					Color32 color = (Grid.LightCount[j] > 0) ? Lighting.Instance.Settings.LightColour : new Color32(0, 0, 0, byte.MaxValue);
					region.SetBytes(num3, i, color.r, color.g, color.b, (color.r + color.g + color.b > 0) ? byte.MaxValue : 0);
					j++;
					num3++;
				}
			}
			return;
		}
		for (int k = y0; k <= y1; k++)
		{
			for (int l = x0; l <= x1; l++)
			{
				region.SetBytes(l, k, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}
	}

	// Token: 0x06004FC9 RID: 20425 RVA: 0x001CFAC4 File Offset: 0x001CDCC4
	private static void UpdateRadiation(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		Vector2 vector = PropertyTextures.instance.coldRange;
		Vector2 vector2 = PropertyTextures.instance.hotRange;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0);
				}
				else
				{
					float v = Grid.Radiation[num];
					region.SetBytes(j, i, v);
				}
			}
		}
	}

	// Token: 0x0400353A RID: 13626
	[NonSerialized]
	public bool ForceLightEverywhere;

	// Token: 0x0400353B RID: 13627
	[SerializeField]
	private Vector2 PressureRange = new Vector2(15f, 200f);

	// Token: 0x0400353C RID: 13628
	[SerializeField]
	private float MinPressureVisibility = 0.1f;

	// Token: 0x0400353D RID: 13629
	[SerializeField]
	[Range(0f, 1f)]
	private float TemperatureStateChangeRange = 0.05f;

	// Token: 0x0400353E RID: 13630
	public static PropertyTextures instance;

	// Token: 0x0400353F RID: 13631
	public static IntPtr externalFlowTex;

	// Token: 0x04003540 RID: 13632
	public static IntPtr externalLiquidTex;

	// Token: 0x04003541 RID: 13633
	public static IntPtr externalLiquidDataTex;

	// Token: 0x04003542 RID: 13634
	public static IntPtr externalExposedToSunlight;

	// Token: 0x04003543 RID: 13635
	public static IntPtr externalSolidDigAmountTex;

	// Token: 0x04003544 RID: 13636
	[SerializeField]
	private Vector2 coldRange;

	// Token: 0x04003545 RID: 13637
	[SerializeField]
	private Vector2 hotRange;

	// Token: 0x04003546 RID: 13638
	public static float FogOfWarScale;

	// Token: 0x04003547 RID: 13639
	private int WorldSizeID;

	// Token: 0x04003548 RID: 13640
	private int ClusterWorldSizeID;

	// Token: 0x04003549 RID: 13641
	private int FogOfWarScaleID;

	// Token: 0x0400354A RID: 13642
	private int PropTexWsToCsID;

	// Token: 0x0400354B RID: 13643
	private int PropTexCsToWsID;

	// Token: 0x0400354C RID: 13644
	private int TopBorderHeightID;

	// Token: 0x0400354D RID: 13645
	private int CameraZoomID;

	// Token: 0x0400354E RID: 13646
	private int NextPropertyIdx;

	// Token: 0x0400354F RID: 13647
	public TextureBuffer[] textureBuffers;

	// Token: 0x04003550 RID: 13648
	public TextureLerper[] lerpers;

	// Token: 0x04003551 RID: 13649
	private TexturePagePool texturePagePool;

	// Token: 0x04003552 RID: 13650
	[SerializeField]
	private Texture2D[] externallyUpdatedTextures;

	// Token: 0x04003553 RID: 13651
	private PropertyTextures.TextureProperties[] textureProperties = new PropertyTextures.TextureProperties[]
	{
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Flow,
			textureFormat = TextureFormat.RGFloat,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = true,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Liquid,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Point,
			updateEveryFrame = true,
			updatedExternally = true,
			blend = true,
			blendSpeed = 1f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.LiquidData,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Point,
			updateEveryFrame = true,
			updatedExternally = true,
			blend = true,
			blendSpeed = 1f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.ExposedToSunlight,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = true,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.SolidDigAmount,
			textureFormat = TextureFormat.RGB24,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.GasColour,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.GasDanger,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.GasPressure,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.FogOfWar,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.WorldLight,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.StateChange,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.FallingSolid,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.SolidLiquidGasMass,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Point,
			updateEveryFrame = true,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.SolidLiquidGasMassForLight,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Point,
			updateEveryFrame = true,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Temperature,
			textureFormat = TextureFormat.RGB24,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Radiation,
			textureFormat = TextureFormat.RFloat,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		}
	};

	// Token: 0x04003554 RID: 13652
	private List<PropertyTextures.TextureProperties> allTextureProperties = new List<PropertyTextures.TextureProperties>();

	// Token: 0x04003555 RID: 13653
	private WorkItemCollection<PropertyTextures.WorkItem, object> workItems = new WorkItemCollection<PropertyTextures.WorkItem, object>();

	// Token: 0x02001BFD RID: 7165
	public enum Property
	{
		// Token: 0x04008689 RID: 34441
		StateChange,
		// Token: 0x0400868A RID: 34442
		GasPressure,
		// Token: 0x0400868B RID: 34443
		GasColour,
		// Token: 0x0400868C RID: 34444
		GasDanger,
		// Token: 0x0400868D RID: 34445
		FogOfWar,
		// Token: 0x0400868E RID: 34446
		Flow,
		// Token: 0x0400868F RID: 34447
		SolidDigAmount,
		// Token: 0x04008690 RID: 34448
		SolidLiquidGasMass,
		// Token: 0x04008691 RID: 34449
		WorldLight,
		// Token: 0x04008692 RID: 34450
		Liquid,
		// Token: 0x04008693 RID: 34451
		Temperature,
		// Token: 0x04008694 RID: 34452
		ExposedToSunlight,
		// Token: 0x04008695 RID: 34453
		FallingSolid,
		// Token: 0x04008696 RID: 34454
		Radiation,
		// Token: 0x04008697 RID: 34455
		LiquidData,
		// Token: 0x04008698 RID: 34456
		SolidLiquidGasMassForLight,
		// Token: 0x04008699 RID: 34457
		Num
	}

	// Token: 0x02001BFE RID: 7166
	private struct TextureProperties
	{
		// Token: 0x0400869A RID: 34458
		public string name;

		// Token: 0x0400869B RID: 34459
		public PropertyTextures.Property simProperty;

		// Token: 0x0400869C RID: 34460
		public TextureFormat textureFormat;

		// Token: 0x0400869D RID: 34461
		public FilterMode filterMode;

		// Token: 0x0400869E RID: 34462
		public bool updateEveryFrame;

		// Token: 0x0400869F RID: 34463
		public bool updatedExternally;

		// Token: 0x040086A0 RID: 34464
		public bool blend;

		// Token: 0x040086A1 RID: 34465
		public float blendSpeed;

		// Token: 0x040086A2 RID: 34466
		public string texturePropertyName;
	}

	// Token: 0x02001BFF RID: 7167
	private struct WorkItem : IWorkItem<object>
	{
		// Token: 0x0600AC37 RID: 44087 RVA: 0x003CB75F File Offset: 0x003C995F
		public WorkItem(TextureRegion texture_region, int x0, int y0, int x1, int y1, PropertyTextures.WorkItem.Callback update_texture_cb)
		{
			this.textureRegion = texture_region;
			this.x0 = x0;
			this.y0 = y0;
			this.x1 = x1;
			this.y1 = y1;
			this.updateTextureCb = update_texture_cb;
		}

		// Token: 0x0600AC38 RID: 44088 RVA: 0x003CB78E File Offset: 0x003C998E
		public void Run(object shared_data, int threadIndex)
		{
			this.updateTextureCb(this.textureRegion, this.x0, this.y0, this.x1, this.y1);
		}

		// Token: 0x040086A3 RID: 34467
		private int x0;

		// Token: 0x040086A4 RID: 34468
		private int y0;

		// Token: 0x040086A5 RID: 34469
		private int x1;

		// Token: 0x040086A6 RID: 34470
		private int y1;

		// Token: 0x040086A7 RID: 34471
		private TextureRegion textureRegion;

		// Token: 0x040086A8 RID: 34472
		private PropertyTextures.WorkItem.Callback updateTextureCb;

		// Token: 0x02002A09 RID: 10761
		// (Invoke) Token: 0x0600D350 RID: 54096
		public delegate void Callback(TextureRegion texture_region, int x0, int y0, int x1, int y1);
	}
}
