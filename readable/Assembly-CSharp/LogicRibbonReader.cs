using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007A5 RID: 1957
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonReader")]
public class LogicRibbonReader : KMonoBehaviour, ILogicRibbonBitSelector, IRender200ms
{
	// Token: 0x0600332B RID: 13099 RVA: 0x00124B40 File Offset: 0x00122D40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicRibbonReader>(-801688580, LogicRibbonReader.OnLogicValueChangedDelegate);
		this.ports = base.GetComponent<LogicPorts>();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.kbac.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600332C RID: 13100 RVA: 0x00124B9C File Offset: 0x00122D9C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRibbonReader>(-905833192, LogicRibbonReader.OnCopySettingsDelegate);
	}

	// Token: 0x0600332D RID: 13101 RVA: 0x00124BB8 File Offset: 0x00122DB8
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicRibbonReader.INPUT_PORT_ID)
		{
			return;
		}
		this.currentValue = logicValueChanged.newValue;
		this.UpdateLogicCircuit();
		this.UpdateVisuals();
	}

	// Token: 0x0600332E RID: 13102 RVA: 0x00124BF8 File Offset: 0x00122DF8
	private void OnCopySettings(object data)
	{
		LogicRibbonReader component = ((GameObject)data).GetComponent<LogicRibbonReader>();
		if (component != null)
		{
			this.SetBitSelection(component.selectedBit);
		}
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x00124C28 File Offset: 0x00122E28
	private void UpdateLogicCircuit()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		LogicWire.BitDepth bitDepth = LogicWire.BitDepth.NumRatings;
		int portCell = component.GetPortCell(LogicRibbonReader.OUTPUT_PORT_ID);
		GameObject gameObject = Grid.Objects[portCell, 31];
		if (gameObject != null)
		{
			LogicWire component2 = gameObject.GetComponent<LogicWire>();
			if (component2 != null)
			{
				bitDepth = component2.MaxBitDepth;
			}
		}
		if (bitDepth != LogicWire.BitDepth.OneBit && bitDepth == LogicWire.BitDepth.FourBit)
		{
			int num = this.currentValue >> this.selectedBit;
			component.SendSignal(LogicRibbonReader.OUTPUT_PORT_ID, num);
		}
		else
		{
			int num = this.currentValue & 1 << this.selectedBit;
			component.SendSignal(LogicRibbonReader.OUTPUT_PORT_ID, (num > 0) ? 1 : 0);
		}
		this.UpdateVisuals();
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x00124CD1 File Offset: 0x00122ED1
	public void Render200ms(float dt)
	{
		this.UpdateVisuals();
	}

	// Token: 0x06003331 RID: 13105 RVA: 0x00124CD9 File Offset: 0x00122ED9
	public void SetBitSelection(int bit)
	{
		this.selectedBit = bit;
		this.UpdateLogicCircuit();
	}

	// Token: 0x06003332 RID: 13106 RVA: 0x00124CE8 File Offset: 0x00122EE8
	public int GetBitSelection()
	{
		return this.selectedBit;
	}

	// Token: 0x06003333 RID: 13107 RVA: 0x00124CF0 File Offset: 0x00122EF0
	public int GetBitDepth()
	{
		return this.bitDepth;
	}

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06003334 RID: 13108 RVA: 0x00124CF8 File Offset: 0x00122EF8
	public string SideScreenTitle
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_READER_TITLE";
		}
	}

	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06003335 RID: 13109 RVA: 0x00124CFF File Offset: 0x00122EFF
	public string SideScreenDescription
	{
		get
		{
			return UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_READER_DESCRIPTION;
		}
	}

	// Token: 0x06003336 RID: 13110 RVA: 0x00124D0B File Offset: 0x00122F0B
	public bool SideScreenDisplayWriterDescription()
	{
		return false;
	}

	// Token: 0x06003337 RID: 13111 RVA: 0x00124D0E File Offset: 0x00122F0E
	public bool SideScreenDisplayReaderDescription()
	{
		return true;
	}

	// Token: 0x06003338 RID: 13112 RVA: 0x00124D14 File Offset: 0x00122F14
	public bool IsBitActive(int bit)
	{
		LogicCircuitNetwork logicCircuitNetwork = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.INPUT_PORT_ID);
			logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return logicCircuitNetwork != null && logicCircuitNetwork.IsBitActive(bit);
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x00124D60 File Offset: 0x00122F60
	public int GetInputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetInputValue(LogicRibbonReader.INPUT_PORT_ID);
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x00124D8C File Offset: 0x00122F8C
	public int GetOutputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetOutputValue(LogicRibbonReader.OUTPUT_PORT_ID);
	}

	// Token: 0x0600333B RID: 13115 RVA: 0x00124DB8 File Offset: 0x00122FB8
	private LogicCircuitNetwork GetInputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.INPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x0600333C RID: 13116 RVA: 0x00124DF8 File Offset: 0x00122FF8
	private LogicCircuitNetwork GetOutputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.OUTPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x0600333D RID: 13117 RVA: 0x00124E38 File Offset: 0x00123038
	public void UpdateVisuals()
	{
		bool inputNetwork = this.GetInputNetwork() != null;
		LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
		this.GetInputValue();
		int num = 0;
		if (inputNetwork)
		{
			num += 4;
			this.kbac.SetSymbolTint(this.BIT_ONE_SYMBOL, this.IsBitActive(0) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_TWO_SYMBOL, this.IsBitActive(1) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_THREE_SYMBOL, this.IsBitActive(2) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_FOUR_SYMBOL, this.IsBitActive(3) ? this.colorOn : this.colorOff);
		}
		if (outputNetwork != null)
		{
			num++;
			this.kbac.SetSymbolTint(this.OUTPUT_SYMBOL, LogicCircuitNetwork.IsBitActive(0, this.GetOutputValue()) ? this.colorOn : this.colorOff);
		}
		this.kbac.Play(num.ToString() + "_" + (this.GetBitSelection() + 1).ToString(), KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001EF6 RID: 7926
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicRibbonReaderInput");

	// Token: 0x04001EF7 RID: 7927
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicRibbonReaderOutput");

	// Token: 0x04001EF8 RID: 7928
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001EF9 RID: 7929
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonReader> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicRibbonReader>(delegate(LogicRibbonReader component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001EFA RID: 7930
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonReader> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRibbonReader>(delegate(LogicRibbonReader component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001EFB RID: 7931
	private KAnimHashedString BIT_ONE_SYMBOL = "bit1_bloom";

	// Token: 0x04001EFC RID: 7932
	private KAnimHashedString BIT_TWO_SYMBOL = "bit2_bloom";

	// Token: 0x04001EFD RID: 7933
	private KAnimHashedString BIT_THREE_SYMBOL = "bit3_bloom";

	// Token: 0x04001EFE RID: 7934
	private KAnimHashedString BIT_FOUR_SYMBOL = "bit4_bloom";

	// Token: 0x04001EFF RID: 7935
	private KAnimHashedString OUTPUT_SYMBOL = "output_light_bloom";

	// Token: 0x04001F00 RID: 7936
	private KBatchedAnimController kbac;

	// Token: 0x04001F01 RID: 7937
	private Color colorOn = new Color(0.34117648f, 0.7254902f, 0.36862746f);

	// Token: 0x04001F02 RID: 7938
	private Color colorOff = new Color(0.9529412f, 0.2901961f, 0.2784314f);

	// Token: 0x04001F03 RID: 7939
	private LogicPorts ports;

	// Token: 0x04001F04 RID: 7940
	public int bitDepth = 4;

	// Token: 0x04001F05 RID: 7941
	[Serialize]
	public int selectedBit;

	// Token: 0x04001F06 RID: 7942
	[Serialize]
	private int currentValue;
}
