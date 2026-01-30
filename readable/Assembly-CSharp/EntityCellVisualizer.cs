using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005DB RID: 1499
public class EntityCellVisualizer : KMonoBehaviour
{
	// Token: 0x1700015E RID: 350
	// (get) Token: 0x060022AB RID: 8875 RVA: 0x000C9B61 File Offset: 0x000C7D61
	public BuildingCellVisualizerResources Resources
	{
		get
		{
			return BuildingCellVisualizerResources.Instance();
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x060022AC RID: 8876 RVA: 0x000C9B68 File Offset: 0x000C7D68
	protected int CenterCell
	{
		get
		{
			return Grid.PosToCell(this);
		}
	}

	// Token: 0x060022AD RID: 8877 RVA: 0x000C9B70 File Offset: 0x000C7D70
	protected virtual void DefinePorts()
	{
	}

	// Token: 0x060022AE RID: 8878 RVA: 0x000C9B72 File Offset: 0x000C7D72
	protected override void OnPrefabInit()
	{
		this.LoadDiseaseIcon();
		this.DefinePorts();
	}

	// Token: 0x060022AF RID: 8879 RVA: 0x000C9B80 File Offset: 0x000C7D80
	public void ConnectedEventWithDelay(float delay, int connectionCount, int cell, string soundName)
	{
		base.StartCoroutine(this.ConnectedDelay(delay, connectionCount, cell, soundName));
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x000C9B94 File Offset: 0x000C7D94
	private IEnumerator ConnectedDelay(float delay, int connectionCount, int cell, string soundName)
	{
		float startTime = Time.realtimeSinceStartup;
		float currentTime = startTime;
		while (currentTime < startTime + delay)
		{
			currentTime += Time.unscaledDeltaTime;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.ConnectedEvent(cell);
		string sound = GlobalAssets.GetSound(soundName, false);
		if (sound != null)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = 0f;
			EventInstance instance = SoundEvent.BeginOneShot(sound, position, 1f, false);
			instance.setParameterByName("connectedCount", (float)connectionCount, false);
			SoundEvent.EndOneShot(instance);
		}
		yield break;
	}

	// Token: 0x060022B1 RID: 8881 RVA: 0x000C9BC0 File Offset: 0x000C7DC0
	private int ComputeCell(CellOffset cellOffset)
	{
		CellOffset offset = cellOffset;
		if (this.rotatable != null)
		{
			offset = this.rotatable.GetRotatedCellOffset(cellOffset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset);
	}

	// Token: 0x060022B2 RID: 8882 RVA: 0x000C9BFC File Offset: 0x000C7DFC
	public void ConnectedEvent(int cell)
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (this.ComputeCell(portEntry.cellOffset) == cell && portEntry.visualizer != null)
			{
				SizePulse pulse = portEntry.visualizer.AddComponent<SizePulse>();
				pulse.speed = 20f;
				pulse.multiplier = 0.75f;
				pulse.updateWhenPaused = true;
				SizePulse pulse2 = pulse;
				pulse2.onComplete = (System.Action)Delegate.Combine(pulse2.onComplete, new System.Action(delegate()
				{
					UnityEngine.Object.Destroy(pulse);
				}));
			}
		}
	}

	// Token: 0x060022B3 RID: 8883 RVA: 0x000C9CD8 File Offset: 0x000C7ED8
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell)
	{
		this.AddPort(type, cell, Color.white);
	}

	// Token: 0x060022B4 RID: 8884 RVA: 0x000C9CE7 File Offset: 0x000C7EE7
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell, Color tint)
	{
		this.AddPort(type, cell, tint, tint, 1.5f, false);
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x000C9CF9 File Offset: 0x000C7EF9
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell, Color connectedTint, Color disconnectedTint, float scale = 1.5f, bool hideBG = false)
	{
		this.ports.Add(new EntityCellVisualizer.PortEntry(type, cell, connectedTint, disconnectedTint, scale, hideBG));
		this.addedPorts |= type;
	}

	// Token: 0x060022B6 RID: 8886 RVA: 0x000C9D24 File Offset: 0x000C7F24
	protected override void OnCleanUp()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.visualizer != null)
			{
				UnityEngine.Object.Destroy(portEntry.visualizer);
			}
		}
		GameObject[] array = new GameObject[]
		{
			this.switchVisualizer,
			this.wireVisualizerAlpha,
			this.wireVisualizerBeta
		};
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i]);
		}
		base.OnCleanUp();
	}

	// Token: 0x060022B7 RID: 8887 RVA: 0x000C9DC8 File Offset: 0x000C7FC8
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (this.icons == null)
		{
			this.icons = new Dictionary<GameObject, Image>();
		}
		Components.EntityCellVisualizers.Add(this);
	}

	// Token: 0x060022B8 RID: 8888 RVA: 0x000C9DEE File Offset: 0x000C7FEE
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		Components.EntityCellVisualizers.Remove(this);
	}

	// Token: 0x060022B9 RID: 8889 RVA: 0x000C9E04 File Offset: 0x000C8004
	public void DrawIcons(HashedString mode)
	{
		EntityCellVisualizer.Ports ports = (EntityCellVisualizer.Ports)0;
		if (base.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
		{
			ports = (EntityCellVisualizer.Ports)0;
		}
		else if (mode == OverlayModes.Power.ID)
		{
			ports = (EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut);
		}
		else if (mode == OverlayModes.GasConduits.ID)
		{
			ports = (EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut);
		}
		else if (mode == OverlayModes.LiquidConduits.ID)
		{
			ports = (EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut);
		}
		else if (mode == OverlayModes.SolidConveyor.ID)
		{
			ports = (EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut);
		}
		else if (mode == OverlayModes.Radiation.ID)
		{
			ports = (EntityCellVisualizer.Ports.HighEnergyParticleIn | EntityCellVisualizer.Ports.HighEnergyParticleOut);
		}
		else if (mode == OverlayModes.Disease.ID)
		{
			ports = (EntityCellVisualizer.Ports.DiseaseIn | EntityCellVisualizer.Ports.DiseaseOut);
		}
		else if (mode == OverlayModes.Temperature.ID || mode == OverlayModes.HeatFlow.ID)
		{
			ports = (EntityCellVisualizer.Ports.HeatSource | EntityCellVisualizer.Ports.HeatSink);
		}
		bool flag = false;
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if ((portEntry.type & ports) == portEntry.type)
			{
				this.DrawUtilityIcon(portEntry);
				flag = true;
			}
			else if (portEntry.visualizer != null && portEntry.visualizer.activeInHierarchy)
			{
				portEntry.visualizer.SetActive(false);
			}
		}
		if (mode == OverlayModes.Power.ID)
		{
			if (!flag)
			{
				Switch component = base.GetComponent<Switch>();
				if (component != null)
				{
					int cell = Grid.PosToCell(base.transform.GetPosition());
					Color32 c = component.IsHandlerOn() ? this.Resources.switchColor : this.Resources.switchOffColor;
					this.DrawUtilityIcon(cell, this.Resources.switchIcon, ref this.switchVisualizer, c, 1f, false);
					return;
				}
				WireUtilityNetworkLink component2 = base.GetComponent<WireUtilityNetworkLink>();
				if (component2 != null)
				{
					int cell2;
					int cell3;
					component2.GetCells(out cell2, out cell3);
					this.DrawUtilityIcon(cell2, (Game.Instance.circuitManager.GetCircuitID(cell2) == ushort.MaxValue) ? this.Resources.electricityBridgeIcon : this.Resources.electricityConnectedIcon, ref this.wireVisualizerAlpha, this.Resources.electricityInputColor, 1f, false);
					this.DrawUtilityIcon(cell3, (Game.Instance.circuitManager.GetCircuitID(cell3) == ushort.MaxValue) ? this.Resources.electricityBridgeIcon : this.Resources.electricityConnectedIcon, ref this.wireVisualizerBeta, this.Resources.electricityInputColor, 1f, false);
					return;
				}
			}
		}
		else
		{
			foreach (GameObject gameObject in new GameObject[]
			{
				this.switchVisualizer,
				this.wireVisualizerAlpha,
				this.wireVisualizerBeta
			})
			{
				if (gameObject != null && gameObject.activeInHierarchy)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x060022BA RID: 8890 RVA: 0x000CA0E8 File Offset: 0x000C82E8
	private Sprite GetSpriteForPortType(EntityCellVisualizer.Ports type, bool connected)
	{
		if (type <= EntityCellVisualizer.Ports.SolidOut)
		{
			if (type <= EntityCellVisualizer.Ports.LiquidIn)
			{
				switch (type)
				{
				case EntityCellVisualizer.Ports.PowerIn:
					if (!connected)
					{
						return this.Resources.electricityInputIcon;
					}
					return this.Resources.electricityBridgeConnectedIcon;
				case EntityCellVisualizer.Ports.PowerOut:
					if (!connected)
					{
						return this.Resources.electricityOutputIcon;
					}
					return this.Resources.electricityBridgeConnectedIcon;
				case EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut:
					break;
				case EntityCellVisualizer.Ports.GasIn:
					return this.Resources.gasInputIcon;
				default:
					if (type == EntityCellVisualizer.Ports.GasOut)
					{
						return this.Resources.gasOutputIcon;
					}
					if (type == EntityCellVisualizer.Ports.LiquidIn)
					{
						return this.Resources.liquidInputIcon;
					}
					break;
				}
			}
			else
			{
				if (type == EntityCellVisualizer.Ports.LiquidOut)
				{
					return this.Resources.liquidOutputIcon;
				}
				if (type == EntityCellVisualizer.Ports.SolidIn)
				{
					return this.Resources.liquidInputIcon;
				}
				if (type == EntityCellVisualizer.Ports.SolidOut)
				{
					return this.Resources.liquidOutputIcon;
				}
			}
		}
		else if (type <= EntityCellVisualizer.Ports.DiseaseIn)
		{
			if (type == EntityCellVisualizer.Ports.HighEnergyParticleIn)
			{
				return this.Resources.highEnergyParticleInputIcon;
			}
			if (type == EntityCellVisualizer.Ports.HighEnergyParticleOut)
			{
				return this.GetIconForHighEnergyOutput();
			}
			if (type == EntityCellVisualizer.Ports.DiseaseIn)
			{
				return this.diseaseSourceSprite;
			}
		}
		else
		{
			if (type == EntityCellVisualizer.Ports.DiseaseOut)
			{
				return this.diseaseSourceSprite;
			}
			if (type == EntityCellVisualizer.Ports.HeatSource)
			{
				return this.Resources.heatSourceIcon;
			}
			if (type == EntityCellVisualizer.Ports.HeatSink)
			{
				return this.Resources.heatSinkIcon;
			}
		}
		return null;
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000CA25C File Offset: 0x000C845C
	protected virtual void DrawUtilityIcon(EntityCellVisualizer.PortEntry port)
	{
		int cell = this.ComputeCell(port.cellOffset);
		bool flag = true;
		bool connected = true;
		EntityCellVisualizer.Ports type = port.type;
		if (type <= EntityCellVisualizer.Ports.GasOut)
		{
			if (type - EntityCellVisualizer.Ports.PowerIn > 1)
			{
				if (type == EntityCellVisualizer.Ports.GasIn || type == EntityCellVisualizer.Ports.GasOut)
				{
					flag = (null != Grid.Objects[cell, 12]);
				}
			}
			else
			{
				bool flag2 = base.GetComponent<Building>() as BuildingPreview != null;
				BuildingEnabledButton component = base.GetComponent<BuildingEnabledButton>();
				connected = (!flag2 && Game.Instance.circuitManager.GetCircuitID(cell) != ushort.MaxValue);
				flag = (flag2 || component == null || component.IsEnabled);
			}
		}
		else if (type <= EntityCellVisualizer.Ports.LiquidOut)
		{
			if (type == EntityCellVisualizer.Ports.LiquidIn || type == EntityCellVisualizer.Ports.LiquidOut)
			{
				flag = (null != Grid.Objects[cell, 16]);
			}
		}
		else if (type == EntityCellVisualizer.Ports.SolidIn || type == EntityCellVisualizer.Ports.SolidOut)
		{
			flag = (null != Grid.Objects[cell, 20]);
		}
		this.DrawUtilityIcon(cell, this.GetSpriteForPortType(port.type, connected), ref port.visualizer, flag ? port.connectedTint : port.disconnectedTint, port.scale, port.hideBG);
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x000CA398 File Offset: 0x000C8598
	protected virtual void LoadDiseaseIcon()
	{
		DiseaseVisualization.Info info = Assets.instance.DiseaseVisualization.GetInfo(this.DiseaseCellVisName);
		if (info.name != null)
		{
			this.diseaseSourceSprite = Assets.instance.DiseaseVisualization.overlaySprite;
			this.diseaseSourceColour = GlobalAssets.Instance.colorSet.GetColorByName(info.overlayColourName);
		}
	}

	// Token: 0x060022BD RID: 8893 RVA: 0x000CA3F8 File Offset: 0x000C85F8
	protected virtual Sprite GetIconForHighEnergyOutput()
	{
		IHighEnergyParticleDirection component = base.GetComponent<IHighEnergyParticleDirection>();
		Sprite result = this.Resources.highEnergyParticleOutputIcons[0];
		if (component != null)
		{
			int directionIndex = EightDirectionUtil.GetDirectionIndex(component.Direction);
			result = this.Resources.highEnergyParticleOutputIcons[directionIndex];
		}
		return result;
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x000CA438 File Offset: 0x000C8638
	private void DrawUtilityIcon(int cell, Sprite icon_img, ref GameObject visualizerObj, Color tint, float scaleMultiplier = 1.5f, bool hideBG = false)
	{
		Vector3 position = Grid.CellToPosCCC(cell, Grid.SceneLayer.Building);
		if (visualizerObj == null)
		{
			visualizerObj = global::Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas, null);
			visualizerObj.transform.SetAsFirstSibling();
			this.icons.Add(visualizerObj, visualizerObj.transform.GetChild(0).GetComponent<Image>());
		}
		if (!visualizerObj.gameObject.activeInHierarchy)
		{
			visualizerObj.gameObject.SetActive(true);
		}
		visualizerObj.GetComponent<Image>().enabled = !hideBG;
		this.icons[visualizerObj].raycastTarget = this.enableRaycast;
		this.icons[visualizerObj].sprite = icon_img;
		visualizerObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = tint;
		visualizerObj.transform.SetPosition(position);
		if (visualizerObj.GetComponent<SizePulse>() == null)
		{
			visualizerObj.transform.localScale = Vector3.one * scaleMultiplier;
		}
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x000CA54C File Offset: 0x000C874C
	public Image GetPowerOutputIcon()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.type == EntityCellVisualizer.Ports.PowerOut)
			{
				return (portEntry.visualizer != null) ? portEntry.visualizer.transform.GetChild(0).GetComponent<Image>() : null;
			}
		}
		return null;
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x000CA5D0 File Offset: 0x000C87D0
	public Image GetPowerInputIcon()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.type == EntityCellVisualizer.Ports.PowerIn)
			{
				return (portEntry.visualizer != null) ? portEntry.visualizer.transform.GetChild(0).GetComponent<Image>() : null;
			}
		}
		return null;
	}

	// Token: 0x04001443 RID: 5187
	protected List<EntityCellVisualizer.PortEntry> ports = new List<EntityCellVisualizer.PortEntry>();

	// Token: 0x04001444 RID: 5188
	public EntityCellVisualizer.Ports addedPorts;

	// Token: 0x04001445 RID: 5189
	private GameObject switchVisualizer;

	// Token: 0x04001446 RID: 5190
	private GameObject wireVisualizerAlpha;

	// Token: 0x04001447 RID: 5191
	private GameObject wireVisualizerBeta;

	// Token: 0x04001448 RID: 5192
	public const EntityCellVisualizer.Ports HEAT_PORTS = EntityCellVisualizer.Ports.HeatSource | EntityCellVisualizer.Ports.HeatSink;

	// Token: 0x04001449 RID: 5193
	public const EntityCellVisualizer.Ports POWER_PORTS = EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut;

	// Token: 0x0400144A RID: 5194
	public const EntityCellVisualizer.Ports GAS_PORTS = EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut;

	// Token: 0x0400144B RID: 5195
	public const EntityCellVisualizer.Ports LIQUID_PORTS = EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut;

	// Token: 0x0400144C RID: 5196
	public const EntityCellVisualizer.Ports SOLID_PORTS = EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut;

	// Token: 0x0400144D RID: 5197
	public const EntityCellVisualizer.Ports ENERGY_PARTICLES_PORTS = EntityCellVisualizer.Ports.HighEnergyParticleIn | EntityCellVisualizer.Ports.HighEnergyParticleOut;

	// Token: 0x0400144E RID: 5198
	public const EntityCellVisualizer.Ports DISEASE_PORTS = EntityCellVisualizer.Ports.DiseaseIn | EntityCellVisualizer.Ports.DiseaseOut;

	// Token: 0x0400144F RID: 5199
	public const EntityCellVisualizer.Ports MATTER_PORTS = EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut | EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut | EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut;

	// Token: 0x04001450 RID: 5200
	protected Sprite diseaseSourceSprite;

	// Token: 0x04001451 RID: 5201
	protected Color32 diseaseSourceColour;

	// Token: 0x04001452 RID: 5202
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001453 RID: 5203
	protected bool enableRaycast = true;

	// Token: 0x04001454 RID: 5204
	protected Dictionary<GameObject, Image> icons;

	// Token: 0x04001455 RID: 5205
	public string DiseaseCellVisName = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;

	// Token: 0x020014B5 RID: 5301
	[Flags]
	public enum Ports
	{
		// Token: 0x04006F41 RID: 28481
		PowerIn = 1,
		// Token: 0x04006F42 RID: 28482
		PowerOut = 2,
		// Token: 0x04006F43 RID: 28483
		GasIn = 4,
		// Token: 0x04006F44 RID: 28484
		GasOut = 8,
		// Token: 0x04006F45 RID: 28485
		LiquidIn = 16,
		// Token: 0x04006F46 RID: 28486
		LiquidOut = 32,
		// Token: 0x04006F47 RID: 28487
		SolidIn = 64,
		// Token: 0x04006F48 RID: 28488
		SolidOut = 128,
		// Token: 0x04006F49 RID: 28489
		HighEnergyParticleIn = 256,
		// Token: 0x04006F4A RID: 28490
		HighEnergyParticleOut = 512,
		// Token: 0x04006F4B RID: 28491
		DiseaseIn = 1024,
		// Token: 0x04006F4C RID: 28492
		DiseaseOut = 2048,
		// Token: 0x04006F4D RID: 28493
		HeatSource = 4096,
		// Token: 0x04006F4E RID: 28494
		HeatSink = 8192
	}

	// Token: 0x020014B6 RID: 5302
	protected class PortEntry
	{
		// Token: 0x060090B8 RID: 37048 RVA: 0x0036F0D7 File Offset: 0x0036D2D7
		public PortEntry(EntityCellVisualizer.Ports type, CellOffset cellOffset, Color connectedTint, Color disconnectedTint, float scale, bool hideBG)
		{
			this.type = type;
			this.cellOffset = cellOffset;
			this.visualizer = null;
			this.connectedTint = connectedTint;
			this.disconnectedTint = disconnectedTint;
			this.scale = scale;
			this.hideBG = hideBG;
		}

		// Token: 0x04006F4F RID: 28495
		public EntityCellVisualizer.Ports type;

		// Token: 0x04006F50 RID: 28496
		public CellOffset cellOffset;

		// Token: 0x04006F51 RID: 28497
		public GameObject visualizer;

		// Token: 0x04006F52 RID: 28498
		public Color connectedTint;

		// Token: 0x04006F53 RID: 28499
		public Color disconnectedTint;

		// Token: 0x04006F54 RID: 28500
		public float scale;

		// Token: 0x04006F55 RID: 28501
		public bool hideBG;
	}
}
