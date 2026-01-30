using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007A6 RID: 1958
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonWriter")]
public class LogicRibbonWriter : KMonoBehaviour, ILogicRibbonBitSelector, IRender200ms
{
	// Token: 0x06003340 RID: 13120 RVA: 0x00125073 File Offset: 0x00123273
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRibbonWriter>(-905833192, LogicRibbonWriter.OnCopySettingsDelegate);
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x0012508C File Offset: 0x0012328C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicRibbonWriter>(-801688580, LogicRibbonWriter.OnLogicValueChangedDelegate);
		this.ports = base.GetComponent<LogicPorts>();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.kbac.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x001250E8 File Offset: 0x001232E8
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicRibbonWriter.INPUT_PORT_ID)
		{
			return;
		}
		this.currentValue = logicValueChanged.newValue;
		this.UpdateLogicCircuit();
		this.UpdateVisuals();
	}

	// Token: 0x06003343 RID: 13123 RVA: 0x00125128 File Offset: 0x00123328
	private void OnCopySettings(object data)
	{
		LogicRibbonWriter component = ((GameObject)data).GetComponent<LogicRibbonWriter>();
		if (component != null)
		{
			this.SetBitSelection(component.selectedBit);
		}
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x00125158 File Offset: 0x00123358
	private void UpdateLogicCircuit()
	{
		int new_value = this.currentValue << this.selectedBit;
		base.GetComponent<LogicPorts>().SendSignal(LogicRibbonWriter.OUTPUT_PORT_ID, new_value);
	}

	// Token: 0x06003345 RID: 13125 RVA: 0x00125187 File Offset: 0x00123387
	public void Render200ms(float dt)
	{
		this.UpdateVisuals();
	}

	// Token: 0x06003346 RID: 13126 RVA: 0x00125190 File Offset: 0x00123390
	private LogicCircuitNetwork GetInputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.INPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x06003347 RID: 13127 RVA: 0x001251D0 File Offset: 0x001233D0
	private LogicCircuitNetwork GetOutputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x00125210 File Offset: 0x00123410
	public void SetBitSelection(int bit)
	{
		this.selectedBit = bit;
		this.UpdateLogicCircuit();
	}

	// Token: 0x06003349 RID: 13129 RVA: 0x0012521F File Offset: 0x0012341F
	public int GetBitSelection()
	{
		return this.selectedBit;
	}

	// Token: 0x0600334A RID: 13130 RVA: 0x00125227 File Offset: 0x00123427
	public int GetBitDepth()
	{
		return this.bitDepth;
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x0600334B RID: 13131 RVA: 0x0012522F File Offset: 0x0012342F
	public string SideScreenTitle
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_TITLE";
		}
	}

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x0600334C RID: 13132 RVA: 0x00125236 File Offset: 0x00123436
	public string SideScreenDescription
	{
		get
		{
			return UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_DESCRIPTION;
		}
	}

	// Token: 0x0600334D RID: 13133 RVA: 0x00125242 File Offset: 0x00123442
	public bool SideScreenDisplayWriterDescription()
	{
		return true;
	}

	// Token: 0x0600334E RID: 13134 RVA: 0x00125245 File Offset: 0x00123445
	public bool SideScreenDisplayReaderDescription()
	{
		return false;
	}

	// Token: 0x0600334F RID: 13135 RVA: 0x00125248 File Offset: 0x00123448
	public bool IsBitActive(int bit)
	{
		LogicCircuitNetwork logicCircuitNetwork = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID);
			logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return logicCircuitNetwork != null && logicCircuitNetwork.IsBitActive(bit);
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x00125294 File Offset: 0x00123494
	public int GetInputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetInputValue(LogicRibbonWriter.INPUT_PORT_ID);
	}

	// Token: 0x06003351 RID: 13137 RVA: 0x001252C0 File Offset: 0x001234C0
	public int GetOutputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetOutputValue(LogicRibbonWriter.OUTPUT_PORT_ID);
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x001252EC File Offset: 0x001234EC
	public void UpdateVisuals()
	{
		bool inputNetwork = this.GetInputNetwork() != null;
		LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
		int num = 0;
		if (inputNetwork)
		{
			num++;
			this.kbac.SetSymbolTint(LogicRibbonWriter.INPUT_SYMBOL, LogicCircuitNetwork.IsBitActive(0, this.GetInputValue()) ? this.colorOn : this.colorOff);
		}
		if (outputNetwork != null)
		{
			num += 4;
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_ONE_SYMBOL, this.IsBitActive(0) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_TWO_SYMBOL, this.IsBitActive(1) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_THREE_SYMBOL, this.IsBitActive(2) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_FOUR_SYMBOL, this.IsBitActive(3) ? this.colorOn : this.colorOff);
		}
		this.kbac.Play(num.ToString() + "_" + (this.GetBitSelection() + 1).ToString(), KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001F07 RID: 7943
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicRibbonWriterInput");

	// Token: 0x04001F08 RID: 7944
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicRibbonWriterOutput");

	// Token: 0x04001F09 RID: 7945
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001F0A RID: 7946
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>(delegate(LogicRibbonWriter component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001F0B RID: 7947
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>(delegate(LogicRibbonWriter component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001F0C RID: 7948
	private LogicPorts ports;

	// Token: 0x04001F0D RID: 7949
	public int bitDepth = 4;

	// Token: 0x04001F0E RID: 7950
	[Serialize]
	public int selectedBit;

	// Token: 0x04001F0F RID: 7951
	[Serialize]
	private int currentValue;

	// Token: 0x04001F10 RID: 7952
	private KBatchedAnimController kbac;

	// Token: 0x04001F11 RID: 7953
	private Color colorOn = new Color(0.34117648f, 0.7254902f, 0.36862746f);

	// Token: 0x04001F12 RID: 7954
	private Color colorOff = new Color(0.9529412f, 0.2901961f, 0.2784314f);

	// Token: 0x04001F13 RID: 7955
	private static KAnimHashedString BIT_ONE_SYMBOL = "bit1_bloom";

	// Token: 0x04001F14 RID: 7956
	private static KAnimHashedString BIT_TWO_SYMBOL = "bit2_bloom";

	// Token: 0x04001F15 RID: 7957
	private static KAnimHashedString BIT_THREE_SYMBOL = "bit3_bloom";

	// Token: 0x04001F16 RID: 7958
	private static KAnimHashedString BIT_FOUR_SYMBOL = "bit4_bloom";

	// Token: 0x04001F17 RID: 7959
	private static KAnimHashedString INPUT_SYMBOL = "input_light_bloom";
}
