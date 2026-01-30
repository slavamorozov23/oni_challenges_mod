using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000797 RID: 1943
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGate : LogicGateBase, ILogicEventSender, ILogicNetworkConnection
{
	// Token: 0x06003222 RID: 12834 RVA: 0x00120028 File Offset: 0x0011E228
	protected override void OnSpawn()
	{
		this.inputOne = new LogicEventHandler(base.InputCellOne, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
		if (base.RequiresTwoInputs)
		{
			this.inputTwo = new LogicEventHandler(base.InputCellTwo, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
		}
		else if (base.RequiresFourInputs)
		{
			this.inputTwo = new LogicEventHandler(base.InputCellTwo, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
			this.inputThree = new LogicEventHandler(base.InputCellThree, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
			this.inputFour = new LogicEventHandler(base.InputCellFour, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
		}
		if (base.RequiresControlInputs)
		{
			this.controlOne = new LogicEventHandler(base.ControlCellOne, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.ControlInput);
			this.controlTwo = new LogicEventHandler(base.ControlCellTwo, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.ControlInput);
		}
		if (base.RequiresFourOutputs)
		{
			this.outputTwo = new LogicPortVisualizer(base.OutputCellTwo, LogicPortSpriteType.Output);
			this.outputThree = new LogicPortVisualizer(base.OutputCellThree, LogicPortSpriteType.Output);
			this.outputFour = new LogicPortVisualizer(base.OutputCellFour, LogicPortSpriteType.Output);
			this.outputTwoSender = new LogicEventSender(LogicGateBase.OUTPUT_TWO_PORT_ID, base.OutputCellTwo, delegate(int new_value, int prev_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_TWO_PORT_ID, new_value, prev_value);
				}
			}, null, LogicPortSpriteType.Output);
			this.outputThreeSender = new LogicEventSender(LogicGateBase.OUTPUT_THREE_PORT_ID, base.OutputCellThree, delegate(int new_value, int prev_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_THREE_PORT_ID, new_value, prev_value);
				}
			}, null, LogicPortSpriteType.Output);
			this.outputFourSender = new LogicEventSender(LogicGateBase.OUTPUT_FOUR_PORT_ID, base.OutputCellFour, delegate(int new_value, int prev_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_FOUR_PORT_ID, new_value, prev_value);
				}
			}, null, LogicPortSpriteType.Output);
		}
		base.Subscribe<LogicGate>(774203113, LogicGate.OnBuildingBrokenDelegate);
		base.Subscribe<LogicGate>(-1735440190, LogicGate.OnBuildingFullyRepairedDelegate);
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || !component.IsBroken)
		{
			this.Connect();
		}
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x00120215 File Offset: 0x0011E415
	protected override void OnCleanUp()
	{
		this.cleaningUp = true;
		this.Disconnect();
		base.Unsubscribe<LogicGate>(774203113, LogicGate.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<LogicGate>(-1735440190, LogicGate.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x0012024C File Offset: 0x0011E44C
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x00120254 File Offset: 0x0011E454
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06003226 RID: 12838 RVA: 0x0012025C File Offset: 0x0011E45C
	private void Connect()
	{
		if (!this.connected)
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem = Game.Instance.logicCircuitSystem;
			this.connected = true;
			int outputCellOne = base.OutputCellOne;
			logicCircuitSystem.AddToNetworks(outputCellOne, this, true);
			this.outputOne = new LogicPortVisualizer(outputCellOne, LogicPortSpriteType.Output);
			logicCircuitManager.AddVisElem(this.outputOne);
			if (base.RequiresFourOutputs)
			{
				this.outputTwo = new LogicPortVisualizer(base.OutputCellTwo, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellTwo, this.outputTwoSender, true);
				logicCircuitManager.AddVisElem(this.outputTwo);
				this.outputThree = new LogicPortVisualizer(base.OutputCellThree, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellThree, this.outputThreeSender, true);
				logicCircuitManager.AddVisElem(this.outputThree);
				this.outputFour = new LogicPortVisualizer(base.OutputCellFour, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellFour, this.outputFourSender, true);
				logicCircuitManager.AddVisElem(this.outputFour);
			}
			int inputCellOne = base.InputCellOne;
			logicCircuitSystem.AddToNetworks(inputCellOne, this.inputOne, true);
			logicCircuitManager.AddVisElem(this.inputOne);
			if (base.RequiresTwoInputs)
			{
				int inputCellTwo = base.InputCellTwo;
				logicCircuitSystem.AddToNetworks(inputCellTwo, this.inputTwo, true);
				logicCircuitManager.AddVisElem(this.inputTwo);
			}
			else if (base.RequiresFourInputs)
			{
				logicCircuitSystem.AddToNetworks(base.InputCellTwo, this.inputTwo, true);
				logicCircuitManager.AddVisElem(this.inputTwo);
				logicCircuitSystem.AddToNetworks(base.InputCellThree, this.inputThree, true);
				logicCircuitManager.AddVisElem(this.inputThree);
				logicCircuitSystem.AddToNetworks(base.InputCellFour, this.inputFour, true);
				logicCircuitManager.AddVisElem(this.inputFour);
			}
			if (base.RequiresControlInputs)
			{
				logicCircuitSystem.AddToNetworks(base.ControlCellOne, this.controlOne, true);
				logicCircuitManager.AddVisElem(this.controlOne);
				logicCircuitSystem.AddToNetworks(base.ControlCellTwo, this.controlTwo, true);
				logicCircuitManager.AddVisElem(this.controlTwo);
			}
			this.RefreshAnimation();
		}
	}

	// Token: 0x06003227 RID: 12839 RVA: 0x00120458 File Offset: 0x0011E658
	private void Disconnect()
	{
		if (this.connected)
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem = Game.Instance.logicCircuitSystem;
			this.connected = false;
			int outputCellOne = base.OutputCellOne;
			logicCircuitSystem.RemoveFromNetworks(outputCellOne, this, true);
			logicCircuitManager.RemoveVisElem(this.outputOne);
			this.outputOne = null;
			if (base.RequiresFourOutputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellTwo, this.outputTwoSender, true);
				logicCircuitManager.RemoveVisElem(this.outputTwo);
				this.outputTwo = null;
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellThree, this.outputThreeSender, true);
				logicCircuitManager.RemoveVisElem(this.outputThree);
				this.outputThree = null;
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellFour, this.outputFourSender, true);
				logicCircuitManager.RemoveVisElem(this.outputFour);
				this.outputFour = null;
			}
			int inputCellOne = base.InputCellOne;
			logicCircuitSystem.RemoveFromNetworks(inputCellOne, this.inputOne, true);
			logicCircuitManager.RemoveVisElem(this.inputOne);
			this.inputOne = null;
			if (base.RequiresTwoInputs)
			{
				int inputCellTwo = base.InputCellTwo;
				logicCircuitSystem.RemoveFromNetworks(inputCellTwo, this.inputTwo, true);
				logicCircuitManager.RemoveVisElem(this.inputTwo);
				this.inputTwo = null;
			}
			else if (base.RequiresFourInputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.InputCellTwo, this.inputTwo, true);
				logicCircuitManager.RemoveVisElem(this.inputTwo);
				this.inputTwo = null;
				logicCircuitSystem.RemoveFromNetworks(base.InputCellThree, this.inputThree, true);
				logicCircuitManager.RemoveVisElem(this.inputThree);
				this.inputThree = null;
				logicCircuitSystem.RemoveFromNetworks(base.InputCellFour, this.inputFour, true);
				logicCircuitManager.RemoveVisElem(this.inputFour);
				this.inputFour = null;
			}
			if (base.RequiresControlInputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.ControlCellOne, this.controlOne, true);
				logicCircuitManager.RemoveVisElem(this.controlOne);
				this.controlOne = null;
				logicCircuitSystem.RemoveFromNetworks(base.ControlCellTwo, this.controlTwo, true);
				logicCircuitManager.RemoveVisElem(this.controlTwo);
				this.controlTwo = null;
			}
			this.RefreshAnimation();
		}
	}

	// Token: 0x06003228 RID: 12840 RVA: 0x0012065C File Offset: 0x0011E85C
	private void UpdateState(int new_value, int prev_value)
	{
		if (this.cleaningUp)
		{
			return;
		}
		int value = this.inputOne.Value;
		int num = (this.inputTwo != null) ? this.inputTwo.Value : 0;
		int num2 = (this.inputThree != null) ? this.inputThree.Value : 0;
		int num3 = (this.inputFour != null) ? this.inputFour.Value : 0;
		int value2 = (this.controlOne != null) ? this.controlOne.Value : 0;
		int value3 = (this.controlTwo != null) ? this.controlTwo.Value : 0;
		if (base.RequiresFourInputs && base.RequiresControlInputs)
		{
			this.outputValueOne = 0;
			if (this.op == LogicGateBase.Op.Multiplexer)
			{
				if (!LogicCircuitNetwork.IsBitActive(0, value3))
				{
					if (!LogicCircuitNetwork.IsBitActive(0, value2))
					{
						this.outputValueOne = value;
					}
					else
					{
						this.outputValueOne = num;
					}
				}
				else if (!LogicCircuitNetwork.IsBitActive(0, value2))
				{
					this.outputValueOne = num2;
				}
				else
				{
					this.outputValueOne = num3;
				}
			}
		}
		if (base.RequiresFourOutputs && base.RequiresControlInputs)
		{
			this.outputValueOne = 0;
			this.outputValueTwo = 0;
			this.outputTwoSender.SetValue(0);
			this.outputValueThree = 0;
			this.outputThreeSender.SetValue(0);
			this.outputValueFour = 0;
			this.outputFourSender.SetValue(0);
			if (this.op == LogicGateBase.Op.Demultiplexer)
			{
				if (!LogicCircuitNetwork.IsBitActive(0, value2))
				{
					if (!LogicCircuitNetwork.IsBitActive(0, value3))
					{
						this.outputValueOne = value;
					}
					else
					{
						this.outputValueTwo = value;
						this.outputTwoSender.SetValue(value);
					}
				}
				else if (!LogicCircuitNetwork.IsBitActive(0, value3))
				{
					this.outputValueThree = value;
					this.outputThreeSender.SetValue(value);
				}
				else
				{
					this.outputValueFour = value;
					this.outputFourSender.SetValue(value);
				}
			}
		}
		switch (this.op)
		{
		case LogicGateBase.Op.And:
			this.outputValueOne = (value & num);
			break;
		case LogicGateBase.Op.Or:
			this.outputValueOne = (value | num);
			break;
		case LogicGateBase.Op.Not:
		{
			LogicWire.BitDepth bitDepth = LogicWire.BitDepth.NumRatings;
			int inputCellOne = base.InputCellOne;
			GameObject gameObject = Grid.Objects[inputCellOne, 31];
			if (gameObject != null)
			{
				LogicWire component = gameObject.GetComponent<LogicWire>();
				if (component != null)
				{
					bitDepth = component.MaxBitDepth;
				}
			}
			if (bitDepth != LogicWire.BitDepth.OneBit && bitDepth == LogicWire.BitDepth.FourBit)
			{
				uint num4 = (uint)value;
				num4 = ~num4;
				num4 &= 15U;
				this.outputValueOne = (int)num4;
			}
			else
			{
				this.outputValueOne = ((value == 0) ? 1 : 0);
			}
			break;
		}
		case LogicGateBase.Op.Xor:
			this.outputValueOne = (value ^ num);
			break;
		case LogicGateBase.Op.CustomSingle:
			this.outputValueOne = this.GetCustomValue(value, num);
			break;
		}
		this.RefreshAnimation();
	}

	// Token: 0x06003229 RID: 12841 RVA: 0x001208F0 File Offset: 0x0011EAF0
	private void OnAdditionalOutputsLogicValueChanged(HashedString port_id, int new_value, int prev_value)
	{
		if (base.gameObject != null)
		{
			LogicValueChanged logicValueChanged = LogicValueChanged.Pool.Get();
			logicValueChanged.portID = port_id;
			logicValueChanged.newValue = new_value;
			logicValueChanged.prevValue = prev_value;
			base.gameObject.Trigger(-801688580, logicValueChanged);
			LogicValueChanged.Pool.Release(logicValueChanged);
		}
	}

	// Token: 0x0600322A RID: 12842 RVA: 0x00120947 File Offset: 0x0011EB47
	public virtual void LogicTick()
	{
	}

	// Token: 0x0600322B RID: 12843 RVA: 0x00120949 File Offset: 0x0011EB49
	protected virtual int GetCustomValue(int val1, int val2)
	{
		return val1;
	}

	// Token: 0x0600322C RID: 12844 RVA: 0x0012094C File Offset: 0x0011EB4C
	public int GetPortValue(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			return this.inputOne.Value;
		case LogicGateBase.PortId.InputTwo:
			if (base.RequiresTwoInputs || base.RequiresFourInputs)
			{
				return this.inputTwo.Value;
			}
			return 0;
		case LogicGateBase.PortId.InputThree:
			if (!base.RequiresFourInputs)
			{
				return 0;
			}
			return this.inputThree.Value;
		case LogicGateBase.PortId.InputFour:
			if (!base.RequiresFourInputs)
			{
				return 0;
			}
			return this.inputFour.Value;
		case LogicGateBase.PortId.OutputOne:
			return this.outputValueOne;
		case LogicGateBase.PortId.OutputTwo:
			return this.outputValueTwo;
		case LogicGateBase.PortId.OutputThree:
			return this.outputValueThree;
		case LogicGateBase.PortId.OutputFour:
			return this.outputValueFour;
		case LogicGateBase.PortId.ControlOne:
			return this.controlOne.Value;
		case LogicGateBase.PortId.ControlTwo:
			return this.controlTwo.Value;
		default:
			return this.outputValueOne;
		}
	}

	// Token: 0x0600322D RID: 12845 RVA: 0x00120A1C File Offset: 0x0011EC1C
	public bool GetPortConnected(LogicGateBase.PortId port)
	{
		if ((port == LogicGateBase.PortId.InputTwo && !base.RequiresTwoInputs && !base.RequiresFourInputs) || (port == LogicGateBase.PortId.InputThree && !base.RequiresFourInputs) || (port == LogicGateBase.PortId.InputFour && !base.RequiresFourInputs))
		{
			return false;
		}
		int cell = base.PortCell(port);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(cell) != null;
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x00120A72 File Offset: 0x0011EC72
	public void SetPortDescriptions(LogicGate.LogicGateDescriptions descriptions)
	{
		this.descriptions = descriptions;
	}

	// Token: 0x0600322F RID: 12847 RVA: 0x00120A7C File Offset: 0x0011EC7C
	public LogicGate.LogicGateDescriptions.Description GetPortDescription(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			if (this.descriptions.inputOne != null)
			{
				return this.descriptions.inputOne;
			}
			if (!base.RequiresTwoInputs && !base.RequiresFourInputs)
			{
				return LogicGate.INPUT_ONE_SINGLE_DESCRIPTION;
			}
			return LogicGate.INPUT_ONE_MULTI_DESCRIPTION;
		case LogicGateBase.PortId.InputTwo:
			if (this.descriptions.inputTwo == null)
			{
				return LogicGate.INPUT_TWO_DESCRIPTION;
			}
			return this.descriptions.inputTwo;
		case LogicGateBase.PortId.InputThree:
			if (this.descriptions.inputThree == null)
			{
				return LogicGate.INPUT_THREE_DESCRIPTION;
			}
			return this.descriptions.inputThree;
		case LogicGateBase.PortId.InputFour:
			if (this.descriptions.inputFour == null)
			{
				return LogicGate.INPUT_FOUR_DESCRIPTION;
			}
			return this.descriptions.inputFour;
		case LogicGateBase.PortId.OutputOne:
			if (this.descriptions.inputOne != null)
			{
				return this.descriptions.inputOne;
			}
			if (!base.RequiresFourOutputs)
			{
				return LogicGate.OUTPUT_ONE_SINGLE_DESCRIPTION;
			}
			return LogicGate.OUTPUT_ONE_MULTI_DESCRIPTION;
		case LogicGateBase.PortId.OutputTwo:
			if (this.descriptions.outputTwo == null)
			{
				return LogicGate.OUTPUT_TWO_DESCRIPTION;
			}
			return this.descriptions.outputTwo;
		case LogicGateBase.PortId.OutputThree:
			if (this.descriptions.outputThree == null)
			{
				return LogicGate.OUTPUT_THREE_DESCRIPTION;
			}
			return this.descriptions.outputThree;
		case LogicGateBase.PortId.OutputFour:
			if (this.descriptions.outputFour == null)
			{
				return LogicGate.OUTPUT_FOUR_DESCRIPTION;
			}
			return this.descriptions.outputFour;
		case LogicGateBase.PortId.ControlOne:
			if (this.descriptions.controlOne == null)
			{
				return LogicGate.CONTROL_ONE_DESCRIPTION;
			}
			return this.descriptions.controlOne;
		case LogicGateBase.PortId.ControlTwo:
			if (this.descriptions.controlTwo == null)
			{
				return LogicGate.CONTROL_TWO_DESCRIPTION;
			}
			return this.descriptions.controlTwo;
		default:
			return this.descriptions.outputOne;
		}
	}

	// Token: 0x06003230 RID: 12848 RVA: 0x00120C21 File Offset: 0x0011EE21
	public int GetLogicValue()
	{
		return this.outputValueOne;
	}

	// Token: 0x06003231 RID: 12849 RVA: 0x00120C29 File Offset: 0x0011EE29
	public int GetLogicCell()
	{
		return this.GetLogicUICell();
	}

	// Token: 0x06003232 RID: 12850 RVA: 0x00120C31 File Offset: 0x0011EE31
	public int GetLogicUICell()
	{
		return base.OutputCellOne;
	}

	// Token: 0x06003233 RID: 12851 RVA: 0x00120C39 File Offset: 0x0011EE39
	public bool IsLogicInput()
	{
		return false;
	}

	// Token: 0x06003234 RID: 12852 RVA: 0x00120C3C File Offset: 0x0011EE3C
	private LogicEventHandler GetInputFromControlValue(int val)
	{
		switch (val)
		{
		case 1:
			return this.inputTwo;
		case 2:
			return this.inputThree;
		case 3:
			return this.inputFour;
		}
		return this.inputOne;
	}

	// Token: 0x06003235 RID: 12853 RVA: 0x00120C71 File Offset: 0x0011EE71
	private void ShowSymbolConditionally(bool showAnything, bool active, KBatchedAnimController kbac, KAnimHashedString ifTrue, KAnimHashedString ifFalse)
	{
		if (!showAnything)
		{
			kbac.SetSymbolVisiblity(ifTrue, false);
			kbac.SetSymbolVisiblity(ifFalse, false);
			return;
		}
		kbac.SetSymbolVisiblity(ifTrue, active);
		kbac.SetSymbolVisiblity(ifFalse, !active);
	}

	// Token: 0x06003236 RID: 12854 RVA: 0x00120C9E File Offset: 0x0011EE9E
	private void TintSymbolConditionally(bool tintAnything, bool condition, KBatchedAnimController kbac, KAnimHashedString symbol, Color ifTrue, Color ifFalse)
	{
		if (tintAnything)
		{
			kbac.SetSymbolTint(symbol, condition ? ifTrue : ifFalse);
			return;
		}
		kbac.SetSymbolTint(symbol, Color.white);
	}

	// Token: 0x06003237 RID: 12855 RVA: 0x00120CC2 File Offset: 0x0011EEC2
	private void SetBloomSymbolShowing(bool showing, KBatchedAnimController kbac, KAnimHashedString symbol, KAnimHashedString bloomSymbol)
	{
		kbac.SetSymbolVisiblity(bloomSymbol, showing);
		kbac.SetSymbolVisiblity(symbol, !showing);
	}

	// Token: 0x06003238 RID: 12856 RVA: 0x00120CD8 File Offset: 0x0011EED8
	protected void RefreshAnimation()
	{
		if (this.cleaningUp)
		{
			return;
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.op == LogicGateBase.Op.Multiplexer)
		{
			int num = LogicCircuitNetwork.GetBitValue(0, this.controlOne.Value) + LogicCircuitNetwork.GetBitValue(0, this.controlTwo.Value) * 2;
			if (this.lastAnimState != num)
			{
				if (this.lastAnimState == -1)
				{
					component.Play(num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(this.lastAnimState.ToString() + "_" + num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			this.lastAnimState = num;
			LogicEventHandler inputFromControlValue = this.GetInputFromControlValue(num);
			KAnimHashedString[] array = LogicGate.multiplexerSymbolPaths[num];
			LogicCircuitNetwork logicCircuitNetwork = Game.Instance.logicCircuitSystem.GetNetworkForCell(inputFromControlValue.GetLogicCell()) as LogicCircuitNetwork;
			UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellOne);
			UtilityNetwork networkForCell2 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellTwo);
			UtilityNetwork networkForCell3 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellThree);
			UtilityNetwork networkForCell4 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellFour);
			this.ShowSymbolConditionally(networkForCell != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL_BLM_RED, LogicGate.INPUT1_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell2 != null, this.inputTwo.Value == 0, component, LogicGate.INPUT2_SYMBOL_BLM_RED, LogicGate.INPUT2_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell3 != null, this.inputThree.Value == 0, component, LogicGate.INPUT3_SYMBOL_BLM_RED, LogicGate.INPUT3_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell4 != null, this.inputFour.Value == 0, component, LogicGate.INPUT4_SYMBOL_BLM_RED, LogicGate.INPUT4_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(logicCircuitNetwork != null, inputFromControlValue.Value == 0, component, LogicGate.OUTPUT1_SYMBOL_BLM_RED, LogicGate.OUTPUT1_SYMBOL_BLM_GRN);
			this.TintSymbolConditionally(networkForCell != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell2 != null, this.inputTwo.Value == 0, component, LogicGate.INPUT2_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell3 != null, this.inputThree.Value == 0, component, LogicGate.INPUT3_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell4 != null, this.inputFour.Value == 0, component, LogicGate.INPUT4_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(Game.Instance.logicCircuitSystem.GetNetworkForCell(base.OutputCellOne) != null && logicCircuitNetwork != null, inputFromControlValue.Value == 0, component, LogicGate.OUTPUT1_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			for (int i = 0; i < LogicGate.multiplexerSymbols.Length; i++)
			{
				KAnimHashedString symbol = LogicGate.multiplexerSymbols[i];
				KAnimHashedString kanimHashedString = LogicGate.multiplexerBloomSymbols[i];
				bool flag = Array.IndexOf<KAnimHashedString>(array, kanimHashedString) != -1 && logicCircuitNetwork != null;
				this.SetBloomSymbolShowing(flag, component, symbol, kanimHashedString);
				if (flag)
				{
					component.SetSymbolTint(kanimHashedString, (inputFromControlValue.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
				}
			}
			return;
		}
		if (this.op == LogicGateBase.Op.Demultiplexer)
		{
			int num2 = LogicCircuitNetwork.GetBitValue(0, this.controlOne.Value) * 2 + LogicCircuitNetwork.GetBitValue(0, this.controlTwo.Value);
			if (this.lastAnimState != num2)
			{
				if (this.lastAnimState == -1)
				{
					component.Play(num2.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(this.lastAnimState.ToString() + "_" + num2.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			this.lastAnimState = num2;
			KAnimHashedString[] array2 = LogicGate.demultiplexerSymbolPaths[num2];
			LogicCircuitNetwork logicCircuitNetwork2 = Game.Instance.logicCircuitSystem.GetNetworkForCell(this.inputOne.GetLogicCell()) as LogicCircuitNetwork;
			for (int j = 0; j < LogicGate.demultiplexerSymbols.Length; j++)
			{
				KAnimHashedString symbol2 = LogicGate.demultiplexerSymbols[j];
				KAnimHashedString kanimHashedString2 = LogicGate.demultiplexerBloomSymbols[j];
				bool flag2 = Array.IndexOf<KAnimHashedString>(array2, kanimHashedString2) != -1 && logicCircuitNetwork2 != null;
				this.SetBloomSymbolShowing(flag2, component, symbol2, kanimHashedString2);
				if (flag2)
				{
					component.SetSymbolTint(kanimHashedString2, (this.inputOne.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
				}
			}
			this.ShowSymbolConditionally(logicCircuitNetwork2 != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL_BLM_RED, LogicGate.INPUT1_SYMBOL_BLM_GRN);
			if (logicCircuitNetwork2 != null)
			{
				component.SetSymbolTint(LogicGate.INPUT1_SYMBOL_BLOOM, (this.inputOne.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
			}
			int[] array3 = new int[]
			{
				base.OutputCellOne,
				base.OutputCellTwo,
				base.OutputCellThree,
				base.OutputCellFour
			};
			for (int k = 0; k < LogicGate.demultiplexerOutputSymbols.Length; k++)
			{
				KAnimHashedString kanimHashedString3 = LogicGate.demultiplexerOutputSymbols[k];
				bool flag3 = Array.IndexOf<KAnimHashedString>(array2, kanimHashedString3) == -1 || this.inputOne.Value == 0;
				UtilityNetwork networkForCell5 = Game.Instance.logicCircuitSystem.GetNetworkForCell(array3[k]);
				this.TintSymbolConditionally(logicCircuitNetwork2 != null && networkForCell5 != null, flag3, component, kanimHashedString3, this.inactiveTintColor, this.activeTintColor);
				this.ShowSymbolConditionally(logicCircuitNetwork2 != null && networkForCell5 != null, flag3, component, LogicGate.demultiplexerOutputRedSymbols[k], LogicGate.demultiplexerOutputGreenSymbols[k]);
			}
			return;
		}
		if (this.op == LogicGateBase.Op.And || this.op == LogicGateBase.Op.Xor || this.op == LogicGateBase.Op.Not || this.op == LogicGateBase.Op.Or)
		{
			int outputCellOne = base.OutputCellOne;
			if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
			{
				component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (base.RequiresTwoInputs)
			{
				int num3 = this.inputOne.Value * 2 + this.inputTwo.Value;
				if (this.lastAnimState != num3)
				{
					if (this.lastAnimState == -1)
					{
						component.Play(num3.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					else
					{
						component.Play(this.lastAnimState.ToString() + "_" + num3.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					this.lastAnimState = num3;
					return;
				}
			}
			else
			{
				int value = this.inputOne.Value;
				if (this.lastAnimState != value)
				{
					if (this.lastAnimState == -1)
					{
						component.Play(value.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					else
					{
						component.Play(this.lastAnimState.ToString() + "_" + value.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					this.lastAnimState = value;
					return;
				}
			}
		}
		else
		{
			int outputCellOne2 = base.OutputCellOne;
			if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne2) is LogicCircuitNetwork))
			{
				component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (base.RequiresTwoInputs)
			{
				component.Play("on_" + (this.inputOne.Value + this.inputTwo.Value * 2 + this.outputValueOne * 4).ToString(), KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			component.Play("on_" + (this.inputOne.Value + this.outputValueOne * 4).ToString(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x001214DD File Offset: 0x0011F6DD
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
	}

	// Token: 0x04001E22 RID: 7714
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_ONE_SINGLE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE
	};

	// Token: 0x04001E23 RID: 7715
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_ONE_MULTI_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_INACTIVE
	};

	// Token: 0x04001E24 RID: 7716
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_INACTIVE
	};

	// Token: 0x04001E25 RID: 7717
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_THREE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_INACTIVE
	};

	// Token: 0x04001E26 RID: 7718
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_FOUR_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_INACTIVE
	};

	// Token: 0x04001E27 RID: 7719
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_ONE_SINGLE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE
	};

	// Token: 0x04001E28 RID: 7720
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_ONE_MULTI_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_INACTIVE
	};

	// Token: 0x04001E29 RID: 7721
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_INACTIVE
	};

	// Token: 0x04001E2A RID: 7722
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_THREE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_INACTIVE
	};

	// Token: 0x04001E2B RID: 7723
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_FOUR_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_INACTIVE
	};

	// Token: 0x04001E2C RID: 7724
	private static readonly LogicGate.LogicGateDescriptions.Description CONTROL_ONE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_INACTIVE
	};

	// Token: 0x04001E2D RID: 7725
	private static readonly LogicGate.LogicGateDescriptions.Description CONTROL_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_INACTIVE
	};

	// Token: 0x04001E2E RID: 7726
	private LogicGate.LogicGateDescriptions descriptions;

	// Token: 0x04001E2F RID: 7727
	private LogicEventSender[] additionalOutputs;

	// Token: 0x04001E30 RID: 7728
	private const bool IS_CIRCUIT_ENDPOINT = true;

	// Token: 0x04001E31 RID: 7729
	private bool connected;

	// Token: 0x04001E32 RID: 7730
	protected bool cleaningUp;

	// Token: 0x04001E33 RID: 7731
	private int lastAnimState = -1;

	// Token: 0x04001E34 RID: 7732
	[Serialize]
	protected int outputValueOne;

	// Token: 0x04001E35 RID: 7733
	[Serialize]
	protected int outputValueTwo;

	// Token: 0x04001E36 RID: 7734
	[Serialize]
	protected int outputValueThree;

	// Token: 0x04001E37 RID: 7735
	[Serialize]
	protected int outputValueFour;

	// Token: 0x04001E38 RID: 7736
	private LogicEventHandler inputOne;

	// Token: 0x04001E39 RID: 7737
	private LogicEventHandler inputTwo;

	// Token: 0x04001E3A RID: 7738
	private LogicEventHandler inputThree;

	// Token: 0x04001E3B RID: 7739
	private LogicEventHandler inputFour;

	// Token: 0x04001E3C RID: 7740
	private LogicPortVisualizer outputOne;

	// Token: 0x04001E3D RID: 7741
	private LogicPortVisualizer outputTwo;

	// Token: 0x04001E3E RID: 7742
	private LogicPortVisualizer outputThree;

	// Token: 0x04001E3F RID: 7743
	private LogicPortVisualizer outputFour;

	// Token: 0x04001E40 RID: 7744
	private LogicEventSender outputTwoSender;

	// Token: 0x04001E41 RID: 7745
	private LogicEventSender outputThreeSender;

	// Token: 0x04001E42 RID: 7746
	private LogicEventSender outputFourSender;

	// Token: 0x04001E43 RID: 7747
	private LogicEventHandler controlOne;

	// Token: 0x04001E44 RID: 7748
	private LogicEventHandler controlTwo;

	// Token: 0x04001E45 RID: 7749
	private static readonly EventSystem.IntraObjectHandler<LogicGate> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicGate>(delegate(LogicGate component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04001E46 RID: 7750
	private static readonly EventSystem.IntraObjectHandler<LogicGate> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicGate>(delegate(LogicGate component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x04001E47 RID: 7751
	private static KAnimHashedString INPUT1_SYMBOL = "input1";

	// Token: 0x04001E48 RID: 7752
	private static KAnimHashedString INPUT2_SYMBOL = "input2";

	// Token: 0x04001E49 RID: 7753
	private static KAnimHashedString INPUT3_SYMBOL = "input3";

	// Token: 0x04001E4A RID: 7754
	private static KAnimHashedString INPUT4_SYMBOL = "input4";

	// Token: 0x04001E4B RID: 7755
	private static KAnimHashedString OUTPUT1_SYMBOL = "output1";

	// Token: 0x04001E4C RID: 7756
	private static KAnimHashedString OUTPUT2_SYMBOL = "output2";

	// Token: 0x04001E4D RID: 7757
	private static KAnimHashedString OUTPUT3_SYMBOL = "output3";

	// Token: 0x04001E4E RID: 7758
	private static KAnimHashedString OUTPUT4_SYMBOL = "output4";

	// Token: 0x04001E4F RID: 7759
	private static KAnimHashedString INPUT1_SYMBOL_BLM_RED = "input1_red_bloom";

	// Token: 0x04001E50 RID: 7760
	private static KAnimHashedString INPUT1_SYMBOL_BLM_GRN = "input1_green_bloom";

	// Token: 0x04001E51 RID: 7761
	private static KAnimHashedString INPUT2_SYMBOL_BLM_RED = "input2_red_bloom";

	// Token: 0x04001E52 RID: 7762
	private static KAnimHashedString INPUT2_SYMBOL_BLM_GRN = "input2_green_bloom";

	// Token: 0x04001E53 RID: 7763
	private static KAnimHashedString INPUT3_SYMBOL_BLM_RED = "input3_red_bloom";

	// Token: 0x04001E54 RID: 7764
	private static KAnimHashedString INPUT3_SYMBOL_BLM_GRN = "input3_green_bloom";

	// Token: 0x04001E55 RID: 7765
	private static KAnimHashedString INPUT4_SYMBOL_BLM_RED = "input4_red_bloom";

	// Token: 0x04001E56 RID: 7766
	private static KAnimHashedString INPUT4_SYMBOL_BLM_GRN = "input4_green_bloom";

	// Token: 0x04001E57 RID: 7767
	private static KAnimHashedString OUTPUT1_SYMBOL_BLM_RED = "output1_red_bloom";

	// Token: 0x04001E58 RID: 7768
	private static KAnimHashedString OUTPUT1_SYMBOL_BLM_GRN = "output1_green_bloom";

	// Token: 0x04001E59 RID: 7769
	private static KAnimHashedString OUTPUT2_SYMBOL_BLM_RED = "output2_red_bloom";

	// Token: 0x04001E5A RID: 7770
	private static KAnimHashedString OUTPUT2_SYMBOL_BLM_GRN = "output2_green_bloom";

	// Token: 0x04001E5B RID: 7771
	private static KAnimHashedString OUTPUT3_SYMBOL_BLM_RED = "output3_red_bloom";

	// Token: 0x04001E5C RID: 7772
	private static KAnimHashedString OUTPUT3_SYMBOL_BLM_GRN = "output3_green_bloom";

	// Token: 0x04001E5D RID: 7773
	private static KAnimHashedString OUTPUT4_SYMBOL_BLM_RED = "output4_red_bloom";

	// Token: 0x04001E5E RID: 7774
	private static KAnimHashedString OUTPUT4_SYMBOL_BLM_GRN = "output4_green_bloom";

	// Token: 0x04001E5F RID: 7775
	private static KAnimHashedString LINE_LEFT_1_SYMBOL = "line_left_1";

	// Token: 0x04001E60 RID: 7776
	private static KAnimHashedString LINE_LEFT_2_SYMBOL = "line_left_2";

	// Token: 0x04001E61 RID: 7777
	private static KAnimHashedString LINE_LEFT_3_SYMBOL = "line_left_3";

	// Token: 0x04001E62 RID: 7778
	private static KAnimHashedString LINE_LEFT_4_SYMBOL = "line_left_4";

	// Token: 0x04001E63 RID: 7779
	private static KAnimHashedString LINE_RIGHT_1_SYMBOL = "line_right_1";

	// Token: 0x04001E64 RID: 7780
	private static KAnimHashedString LINE_RIGHT_2_SYMBOL = "line_right_2";

	// Token: 0x04001E65 RID: 7781
	private static KAnimHashedString LINE_RIGHT_3_SYMBOL = "line_right_3";

	// Token: 0x04001E66 RID: 7782
	private static KAnimHashedString LINE_RIGHT_4_SYMBOL = "line_right_4";

	// Token: 0x04001E67 RID: 7783
	private static KAnimHashedString FLIPPER_1_SYMBOL = "flipper1";

	// Token: 0x04001E68 RID: 7784
	private static KAnimHashedString FLIPPER_2_SYMBOL = "flipper2";

	// Token: 0x04001E69 RID: 7785
	private static KAnimHashedString FLIPPER_3_SYMBOL = "flipper3";

	// Token: 0x04001E6A RID: 7786
	private static KAnimHashedString INPUT_SYMBOL = "input";

	// Token: 0x04001E6B RID: 7787
	private static KAnimHashedString OUTPUT_SYMBOL = "output";

	// Token: 0x04001E6C RID: 7788
	private static KAnimHashedString INPUT1_SYMBOL_BLOOM = "input1_bloom";

	// Token: 0x04001E6D RID: 7789
	private static KAnimHashedString INPUT2_SYMBOL_BLOOM = "input2_bloom";

	// Token: 0x04001E6E RID: 7790
	private static KAnimHashedString INPUT3_SYMBOL_BLOOM = "input3_bloom";

	// Token: 0x04001E6F RID: 7791
	private static KAnimHashedString INPUT4_SYMBOL_BLOOM = "input4_bloom";

	// Token: 0x04001E70 RID: 7792
	private static KAnimHashedString OUTPUT1_SYMBOL_BLOOM = "output1_bloom";

	// Token: 0x04001E71 RID: 7793
	private static KAnimHashedString OUTPUT2_SYMBOL_BLOOM = "output2_bloom";

	// Token: 0x04001E72 RID: 7794
	private static KAnimHashedString OUTPUT3_SYMBOL_BLOOM = "output3_bloom";

	// Token: 0x04001E73 RID: 7795
	private static KAnimHashedString OUTPUT4_SYMBOL_BLOOM = "output4_bloom";

	// Token: 0x04001E74 RID: 7796
	private static KAnimHashedString LINE_LEFT_1_SYMBOL_BLOOM = "line_left_1_bloom";

	// Token: 0x04001E75 RID: 7797
	private static KAnimHashedString LINE_LEFT_2_SYMBOL_BLOOM = "line_left_2_bloom";

	// Token: 0x04001E76 RID: 7798
	private static KAnimHashedString LINE_LEFT_3_SYMBOL_BLOOM = "line_left_3_bloom";

	// Token: 0x04001E77 RID: 7799
	private static KAnimHashedString LINE_LEFT_4_SYMBOL_BLOOM = "line_left_4_bloom";

	// Token: 0x04001E78 RID: 7800
	private static KAnimHashedString LINE_RIGHT_1_SYMBOL_BLOOM = "line_right_1_bloom";

	// Token: 0x04001E79 RID: 7801
	private static KAnimHashedString LINE_RIGHT_2_SYMBOL_BLOOM = "line_right_2_bloom";

	// Token: 0x04001E7A RID: 7802
	private static KAnimHashedString LINE_RIGHT_3_SYMBOL_BLOOM = "line_right_3_bloom";

	// Token: 0x04001E7B RID: 7803
	private static KAnimHashedString LINE_RIGHT_4_SYMBOL_BLOOM = "line_right_4_bloom";

	// Token: 0x04001E7C RID: 7804
	private static KAnimHashedString FLIPPER_1_SYMBOL_BLOOM = "flipper1_bloom";

	// Token: 0x04001E7D RID: 7805
	private static KAnimHashedString FLIPPER_2_SYMBOL_BLOOM = "flipper2_bloom";

	// Token: 0x04001E7E RID: 7806
	private static KAnimHashedString FLIPPER_3_SYMBOL_BLOOM = "flipper3_bloom";

	// Token: 0x04001E7F RID: 7807
	private static KAnimHashedString INPUT_SYMBOL_BLOOM = "input_bloom";

	// Token: 0x04001E80 RID: 7808
	private static KAnimHashedString OUTPUT_SYMBOL_BLOOM = "output_bloom";

	// Token: 0x04001E81 RID: 7809
	private static KAnimHashedString[][] multiplexerSymbolPaths = new KAnimHashedString[][]
	{
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_3_SYMBOL_BLOOM,
			LogicGate.FLIPPER_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_4_SYMBOL_BLOOM,
			LogicGate.FLIPPER_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		}
	};

	// Token: 0x04001E82 RID: 7810
	private static KAnimHashedString[] multiplexerSymbols = new KAnimHashedString[]
	{
		LogicGate.LINE_LEFT_1_SYMBOL,
		LogicGate.LINE_LEFT_2_SYMBOL,
		LogicGate.LINE_LEFT_3_SYMBOL,
		LogicGate.LINE_LEFT_4_SYMBOL,
		LogicGate.LINE_RIGHT_1_SYMBOL,
		LogicGate.LINE_RIGHT_2_SYMBOL,
		LogicGate.FLIPPER_1_SYMBOL,
		LogicGate.FLIPPER_2_SYMBOL,
		LogicGate.FLIPPER_3_SYMBOL,
		LogicGate.OUTPUT_SYMBOL
	};

	// Token: 0x04001E83 RID: 7811
	private static KAnimHashedString[] multiplexerBloomSymbols = new KAnimHashedString[]
	{
		LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_3_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_4_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
		LogicGate.FLIPPER_1_SYMBOL_BLOOM,
		LogicGate.FLIPPER_2_SYMBOL_BLOOM,
		LogicGate.FLIPPER_3_SYMBOL_BLOOM,
		LogicGate.OUTPUT_SYMBOL_BLOOM
	};

	// Token: 0x04001E84 RID: 7812
	private static KAnimHashedString[][] demultiplexerSymbolPaths = new KAnimHashedString[][]
	{
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.OUTPUT1_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.OUTPUT2_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT3_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_4_SYMBOL_BLOOM,
			LogicGate.OUTPUT4_SYMBOL
		}
	};

	// Token: 0x04001E85 RID: 7813
	private static KAnimHashedString[] demultiplexerSymbols = new KAnimHashedString[]
	{
		LogicGate.INPUT_SYMBOL,
		LogicGate.LINE_LEFT_1_SYMBOL,
		LogicGate.LINE_LEFT_2_SYMBOL,
		LogicGate.LINE_RIGHT_1_SYMBOL,
		LogicGate.LINE_RIGHT_2_SYMBOL,
		LogicGate.LINE_RIGHT_3_SYMBOL,
		LogicGate.LINE_RIGHT_4_SYMBOL
	};

	// Token: 0x04001E86 RID: 7814
	private static KAnimHashedString[] demultiplexerBloomSymbols = new KAnimHashedString[]
	{
		LogicGate.INPUT_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_3_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_4_SYMBOL_BLOOM
	};

	// Token: 0x04001E87 RID: 7815
	private static KAnimHashedString[] demultiplexerOutputSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL,
		LogicGate.OUTPUT2_SYMBOL,
		LogicGate.OUTPUT3_SYMBOL,
		LogicGate.OUTPUT4_SYMBOL
	};

	// Token: 0x04001E88 RID: 7816
	private static KAnimHashedString[] demultiplexerOutputRedSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL_BLM_RED,
		LogicGate.OUTPUT2_SYMBOL_BLM_RED,
		LogicGate.OUTPUT3_SYMBOL_BLM_RED,
		LogicGate.OUTPUT4_SYMBOL_BLM_RED
	};

	// Token: 0x04001E89 RID: 7817
	private static KAnimHashedString[] demultiplexerOutputGreenSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT2_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT3_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT4_SYMBOL_BLM_GRN
	};

	// Token: 0x04001E8A RID: 7818
	private Color activeTintColor = new Color(0.5411765f, 0.9882353f, 0.29803923f);

	// Token: 0x04001E8B RID: 7819
	private Color inactiveTintColor = Color.red;

	// Token: 0x020016BC RID: 5820
	public class LogicGateDescriptions
	{
		// Token: 0x040075C0 RID: 30144
		public LogicGate.LogicGateDescriptions.Description inputOne;

		// Token: 0x040075C1 RID: 30145
		public LogicGate.LogicGateDescriptions.Description inputTwo;

		// Token: 0x040075C2 RID: 30146
		public LogicGate.LogicGateDescriptions.Description inputThree;

		// Token: 0x040075C3 RID: 30147
		public LogicGate.LogicGateDescriptions.Description inputFour;

		// Token: 0x040075C4 RID: 30148
		public LogicGate.LogicGateDescriptions.Description outputOne;

		// Token: 0x040075C5 RID: 30149
		public LogicGate.LogicGateDescriptions.Description outputTwo;

		// Token: 0x040075C6 RID: 30150
		public LogicGate.LogicGateDescriptions.Description outputThree;

		// Token: 0x040075C7 RID: 30151
		public LogicGate.LogicGateDescriptions.Description outputFour;

		// Token: 0x040075C8 RID: 30152
		public LogicGate.LogicGateDescriptions.Description controlOne;

		// Token: 0x040075C9 RID: 30153
		public LogicGate.LogicGateDescriptions.Description controlTwo;

		// Token: 0x0200290B RID: 10507
		public class Description
		{
			// Token: 0x0400B56B RID: 46443
			public string name;

			// Token: 0x0400B56C RID: 46444
			public string active;

			// Token: 0x0400B56D RID: 46445
			public string inactive;
		}
	}
}
