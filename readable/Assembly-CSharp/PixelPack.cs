using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020007DB RID: 2011
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PixelPack")]
public class PixelPack : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06003561 RID: 13665 RVA: 0x0012D7DD File Offset: 0x0012B9DD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<PixelPack>(-905833192, PixelPack.OnCopySettingsDelegate);
	}

	// Token: 0x06003562 RID: 13666 RVA: 0x0012D7F8 File Offset: 0x0012B9F8
	private void OnCopySettings(object data)
	{
		PixelPack component = ((GameObject)data).GetComponent<PixelPack>();
		if (component != null)
		{
			for (int i = 0; i < component.colorSettings.Count; i++)
			{
				this.colorSettings[i] = component.colorSettings[i];
			}
		}
		this.UpdateColors();
	}

	// Token: 0x06003563 RID: 13667 RVA: 0x0012D850 File Offset: 0x0012BA50
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.Subscribe<PixelPack>(-801688580, PixelPack.OnLogicValueChangedDelegate);
		base.Subscribe<PixelPack>(-592767678, PixelPack.OnOperationalChangedDelegate);
		if (this.colorSettings == null)
		{
			PixelPack.ColorPair item = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			PixelPack.ColorPair item2 = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			PixelPack.ColorPair item3 = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			PixelPack.ColorPair item4 = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			this.colorSettings = new List<PixelPack.ColorPair>();
			this.colorSettings.Add(item);
			this.colorSettings.Add(item2);
			this.colorSettings.Add(item3);
			this.colorSettings.Add(item4);
		}
	}

	// Token: 0x06003564 RID: 13668 RVA: 0x0012D96C File Offset: 0x0012BB6C
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == PixelPack.PORT_ID)
		{
			this.logicValue = logicValueChanged.newValue;
			this.UpdateColors();
		}
	}

	// Token: 0x06003565 RID: 13669 RVA: 0x0012D9A4 File Offset: 0x0012BBA4
	private void OnOperationalChanged(object _)
	{
		if (this.operational.IsOperational)
		{
			this.UpdateColors();
			this.animController.Play(PixelPack.ON_ANIMS, KAnim.PlayMode.Once);
		}
		else
		{
			this.animController.Play(PixelPack.OFF_ANIMS, KAnim.PlayMode.Once);
		}
		this.operational.SetActive(this.operational.IsOperational, false);
	}

	// Token: 0x06003566 RID: 13670 RVA: 0x0012DA00 File Offset: 0x0012BC00
	public void UpdateColors()
	{
		if (this.operational.IsOperational)
		{
			LogicPorts component = base.GetComponent<LogicPorts>();
			if (component != null)
			{
				LogicWire.BitDepth connectedWireBitDepth = component.GetConnectedWireBitDepth(PixelPack.PORT_ID);
				if (connectedWireBitDepth == LogicWire.BitDepth.FourBit)
				{
					this.animController.SetSymbolTint(PixelPack.SYMBOL_ONE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[0].activeColor : this.colorSettings[0].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_TWO_NAME, LogicCircuitNetwork.IsBitActive(1, this.logicValue) ? this.colorSettings[1].activeColor : this.colorSettings[1].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_THREE_NAME, LogicCircuitNetwork.IsBitActive(2, this.logicValue) ? this.colorSettings[2].activeColor : this.colorSettings[2].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_FOUR_NAME, LogicCircuitNetwork.IsBitActive(3, this.logicValue) ? this.colorSettings[3].activeColor : this.colorSettings[3].standbyColor);
					return;
				}
				if (connectedWireBitDepth == LogicWire.BitDepth.OneBit)
				{
					this.animController.SetSymbolTint(PixelPack.SYMBOL_ONE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[0].activeColor : this.colorSettings[0].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_TWO_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[1].activeColor : this.colorSettings[1].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_THREE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[2].activeColor : this.colorSettings[2].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_FOUR_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[3].activeColor : this.colorSettings[3].standbyColor);
				}
			}
		}
	}

	// Token: 0x0400204A RID: 8266
	protected KBatchedAnimController animController;

	// Token: 0x0400204B RID: 8267
	private static readonly EventSystem.IntraObjectHandler<PixelPack> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<PixelPack>(delegate(PixelPack component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x0400204C RID: 8268
	private static readonly EventSystem.IntraObjectHandler<PixelPack> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<PixelPack>(delegate(PixelPack component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x0400204D RID: 8269
	public static readonly HashedString PORT_ID = new HashedString("PixelPackInput");

	// Token: 0x0400204E RID: 8270
	public static readonly HashedString SYMBOL_ONE_NAME = "screen1";

	// Token: 0x0400204F RID: 8271
	public static readonly HashedString SYMBOL_TWO_NAME = "screen2";

	// Token: 0x04002050 RID: 8272
	public static readonly HashedString SYMBOL_THREE_NAME = "screen3";

	// Token: 0x04002051 RID: 8273
	public static readonly HashedString SYMBOL_FOUR_NAME = "screen4";

	// Token: 0x04002052 RID: 8274
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002053 RID: 8275
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002054 RID: 8276
	private static readonly EventSystem.IntraObjectHandler<PixelPack> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<PixelPack>(delegate(PixelPack component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002055 RID: 8277
	public int logicValue;

	// Token: 0x04002056 RID: 8278
	[Serialize]
	public List<PixelPack.ColorPair> colorSettings;

	// Token: 0x04002057 RID: 8279
	private Color defaultActive = new Color(0.34509805f, 0.84705883f, 0.32941177f);

	// Token: 0x04002058 RID: 8280
	private Color defaultStandby = new Color(0.972549f, 0.47058824f, 0.34509805f);

	// Token: 0x04002059 RID: 8281
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on"
	};

	// Token: 0x0400205A RID: 8282
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"off_pre",
		"off"
	};

	// Token: 0x02001733 RID: 5939
	public struct ColorPair
	{
		// Token: 0x04007712 RID: 30482
		public Color activeColor;

		// Token: 0x04007713 RID: 30483
		public Color standbyColor;
	}
}
