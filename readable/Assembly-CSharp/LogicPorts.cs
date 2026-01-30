using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009F0 RID: 2544
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicPorts")]
public class LogicPorts : KMonoBehaviour, IGameObjectEffectDescriptor, IRenderEveryTick
{
	// Token: 0x06004A35 RID: 18997 RVA: 0x001AE1C9 File Offset: 0x001AC3C9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoRegisterSimRender = false;
	}

	// Token: 0x06004A36 RID: 18998 RVA: 0x001AE1D8 File Offset: 0x001AC3D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.isPhysical = (component == null || component is BuildingComplete);
		if (!this.isPhysical && !(component is BuildingUnderConstruction))
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.OnOverlayChanged(OverlayScreen.Instance.mode);
			this.CreateVisualizers();
			SimAndRenderScheduler.instance.Add(this, false);
			return;
		}
		if (this.isPhysical)
		{
			this.UpdateMissingWireIcon();
			this.CreatePhysicalPorts(false);
			return;
		}
		this.CreateVisualizers();
	}

	// Token: 0x06004A37 RID: 18999 RVA: 0x001AE28C File Offset: 0x001AC48C
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.DestroyVisualizers();
		if (this.isPhysical)
		{
			this.DestroyPhysicalPorts();
		}
		base.OnCleanUp();
	}

	// Token: 0x06004A38 RID: 19000 RVA: 0x001AE2D9 File Offset: 0x001AC4D9
	public void RenderEveryTick(float dt)
	{
		this.CreateVisualizers();
	}

	// Token: 0x06004A39 RID: 19001 RVA: 0x001AE2E1 File Offset: 0x001AC4E1
	public void HackRefreshVisualizers()
	{
		this.CreateVisualizers();
	}

	// Token: 0x06004A3A RID: 19002 RVA: 0x001AE2EC File Offset: 0x001AC4EC
	private void CreateVisualizers()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		bool flag = num != this.cell;
		this.cell = num;
		if (!flag)
		{
			Rotatable component = base.GetComponent<Rotatable>();
			if (component != null)
			{
				Orientation orientation = component.GetOrientation();
				flag = (orientation != this.orientation);
				this.orientation = orientation;
			}
		}
		if (!flag)
		{
			return;
		}
		this.DestroyVisualizers();
		if (this.outputPortInfo != null)
		{
			this.outputPorts = new List<ILogicUIElement>();
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port port = this.outputPortInfo[i];
				LogicPortVisualizer logicPortVisualizer = new LogicPortVisualizer(this.GetActualCell(port.cellOffset), port.spriteType);
				this.outputPorts.Add(logicPortVisualizer);
				Game.Instance.logicCircuitManager.AddVisElem(logicPortVisualizer);
			}
		}
		if (this.inputPortInfo != null)
		{
			this.inputPorts = new List<ILogicUIElement>();
			for (int j = 0; j < this.inputPortInfo.Length; j++)
			{
				LogicPorts.Port port2 = this.inputPortInfo[j];
				LogicPortVisualizer logicPortVisualizer2 = new LogicPortVisualizer(this.GetActualCell(port2.cellOffset), port2.spriteType);
				this.inputPorts.Add(logicPortVisualizer2);
				Game.Instance.logicCircuitManager.AddVisElem(logicPortVisualizer2);
			}
		}
	}

	// Token: 0x06004A3B RID: 19003 RVA: 0x001AE43C File Offset: 0x001AC63C
	private void DestroyVisualizers()
	{
		if (this.outputPorts != null)
		{
			foreach (ILogicUIElement elem in this.outputPorts)
			{
				Game.Instance.logicCircuitManager.RemoveVisElem(elem);
			}
		}
		if (this.inputPorts != null)
		{
			foreach (ILogicUIElement elem2 in this.inputPorts)
			{
				Game.Instance.logicCircuitManager.RemoveVisElem(elem2);
			}
		}
	}

	// Token: 0x06004A3C RID: 19004 RVA: 0x001AE4F4 File Offset: 0x001AC6F4
	private void CreatePhysicalPorts(bool forceCreate = false)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num == this.cell && !forceCreate)
		{
			return;
		}
		this.cell = num;
		this.DestroyVisualizers();
		if (this.outputPortInfo != null)
		{
			this.outputPorts = new List<ILogicUIElement>();
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port info = this.outputPortInfo[i];
				LogicEventSender logicEventSender = new LogicEventSender(info.id, this.GetActualCell(info.cellOffset), delegate(int new_value, int prev_value)
				{
					if (this != null)
					{
						this.OnLogicValueChanged(info.id, new_value, prev_value);
					}
				}, new Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
				this.outputPorts.Add(logicEventSender);
				Game.Instance.logicCircuitManager.AddVisElem(logicEventSender);
				Game.Instance.logicCircuitSystem.AddToNetworks(logicEventSender.GetLogicUICell(), logicEventSender, true);
			}
			if (this.serializedOutputValues != null && this.serializedOutputValues.Length == this.outputPorts.Count)
			{
				for (int j = 0; j < this.outputPorts.Count; j++)
				{
					(this.outputPorts[j] as LogicEventSender).SetValue(this.serializedOutputValues[j]);
				}
			}
			else
			{
				for (int k = 0; k < this.outputPorts.Count; k++)
				{
					(this.outputPorts[k] as LogicEventSender).SetValue(0);
				}
			}
		}
		this.serializedOutputValues = null;
		if (this.inputPortInfo != null)
		{
			this.inputPorts = new List<ILogicUIElement>();
			for (int l = 0; l < this.inputPortInfo.Length; l++)
			{
				LogicPorts.Port info = this.inputPortInfo[l];
				LogicEventHandler logicEventHandler = new LogicEventHandler(this.GetActualCell(info.cellOffset), delegate(int new_value, int prev_value)
				{
					if (this != null)
					{
						this.OnLogicValueChanged(info.id, new_value, prev_value);
					}
				}, new Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
				this.inputPorts.Add(logicEventHandler);
				Game.Instance.logicCircuitManager.AddVisElem(logicEventHandler);
				Game.Instance.logicCircuitSystem.AddToNetworks(logicEventHandler.GetLogicUICell(), logicEventHandler, true);
			}
		}
	}

	// Token: 0x06004A3D RID: 19005 RVA: 0x001AE750 File Offset: 0x001AC950
	private bool ShowMissingWireIcon()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		if (this.outputPortInfo != null)
		{
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port port = this.outputPortInfo[i];
				if (port.requiresConnection)
				{
					int portCell = this.GetPortCell(port.id);
					if (logicCircuitManager.GetNetworkForCell(portCell) == null)
					{
						return true;
					}
				}
			}
		}
		if (this.inputPortInfo != null)
		{
			for (int j = 0; j < this.inputPortInfo.Length; j++)
			{
				LogicPorts.Port port2 = this.inputPortInfo[j];
				if (port2.requiresConnection)
				{
					int portCell2 = this.GetPortCell(port2.id);
					if (logicCircuitManager.GetNetworkForCell(portCell2) == null)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06004A3E RID: 19006 RVA: 0x001AE803 File Offset: 0x001ACA03
	public void OnMove()
	{
		this.DestroyPhysicalPorts();
		this.CreatePhysicalPorts(false);
	}

	// Token: 0x06004A3F RID: 19007 RVA: 0x001AE812 File Offset: 0x001ACA12
	private void OnLogicNetworkConnectionChanged(int cell, bool connected)
	{
		this.UpdateMissingWireIcon();
	}

	// Token: 0x06004A40 RID: 19008 RVA: 0x001AE81A File Offset: 0x001ACA1A
	private void UpdateMissingWireIcon()
	{
		LogicCircuitManager.ToggleNoWireConnected(this.ShowMissingWireIcon(), base.gameObject);
	}

	// Token: 0x06004A41 RID: 19009 RVA: 0x001AE830 File Offset: 0x001ACA30
	private void DestroyPhysicalPorts()
	{
		if (this.outputPorts != null)
		{
			foreach (ILogicUIElement logicUIElement in this.outputPorts)
			{
				ILogicEventSender logicEventSender = (ILogicEventSender)logicUIElement;
				Game.Instance.logicCircuitSystem.RemoveFromNetworks(logicEventSender.GetLogicCell(), logicEventSender, true);
			}
		}
		if (this.inputPorts != null)
		{
			for (int i = 0; i < this.inputPorts.Count; i++)
			{
				LogicEventHandler logicEventHandler = this.inputPorts[i] as LogicEventHandler;
				if (logicEventHandler != null)
				{
					Game.Instance.logicCircuitSystem.RemoveFromNetworks(logicEventHandler.GetLogicCell(), logicEventHandler, true);
				}
			}
		}
	}

	// Token: 0x06004A42 RID: 19010 RVA: 0x001AE8EC File Offset: 0x001ACAEC
	private void OnLogicValueChanged(HashedString port_id, int new_value, int prev_value)
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

	// Token: 0x06004A43 RID: 19011 RVA: 0x001AE944 File Offset: 0x001ACB44
	private int GetActualCell(CellOffset offset)
	{
		Rotatable component = base.GetComponent<Rotatable>();
		if (component != null)
		{
			offset = component.GetRotatedCellOffset(offset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), offset);
	}

	// Token: 0x06004A44 RID: 19012 RVA: 0x001AE980 File Offset: 0x001ACB80
	public bool TryGetPortAtCell(int cell, out LogicPorts.Port port, out bool isInput)
	{
		foreach (LogicPorts.Port port2 in this.inputPortInfo)
		{
			if (this.GetActualCell(port2.cellOffset) == cell)
			{
				port = port2;
				isInput = true;
				return true;
			}
		}
		foreach (LogicPorts.Port port3 in this.outputPortInfo)
		{
			if (this.GetActualCell(port3.cellOffset) == cell)
			{
				port = port3;
				isInput = false;
				return true;
			}
		}
		port = default(LogicPorts.Port);
		isInput = false;
		return false;
	}

	// Token: 0x06004A45 RID: 19013 RVA: 0x001AEA08 File Offset: 0x001ACC08
	public void SendSignal(HashedString port_id, int new_value)
	{
		if (this.outputPortInfo != null && this.outputPorts == null)
		{
			this.CreatePhysicalPorts(true);
		}
		foreach (ILogicUIElement logicUIElement in this.outputPorts)
		{
			LogicEventSender logicEventSender = (LogicEventSender)logicUIElement;
			if (logicEventSender.ID == port_id)
			{
				logicEventSender.SetValue(new_value);
				break;
			}
		}
	}

	// Token: 0x06004A46 RID: 19014 RVA: 0x001AEA88 File Offset: 0x001ACC88
	public int GetPortCell(HashedString port_id)
	{
		foreach (LogicPorts.Port port in this.inputPortInfo)
		{
			if (port.id == port_id)
			{
				return this.GetActualCell(port.cellOffset);
			}
		}
		foreach (LogicPorts.Port port2 in this.outputPortInfo)
		{
			if (port2.id == port_id)
			{
				return this.GetActualCell(port2.cellOffset);
			}
		}
		return -1;
	}

	// Token: 0x06004A47 RID: 19015 RVA: 0x001AEB08 File Offset: 0x001ACD08
	public int GetInputValue(HashedString port_id)
	{
		int num = 0;
		while (num < this.inputPortInfo.Length && this.inputPorts != null)
		{
			if (this.inputPortInfo[num].id == port_id)
			{
				LogicEventHandler logicEventHandler = this.inputPorts[num] as LogicEventHandler;
				if (logicEventHandler == null)
				{
					return 0;
				}
				return logicEventHandler.Value;
			}
			else
			{
				num++;
			}
		}
		return 0;
	}

	// Token: 0x06004A48 RID: 19016 RVA: 0x001AEB68 File Offset: 0x001ACD68
	public int GetOutputValue(HashedString port_id)
	{
		for (int i = 0; i < this.outputPorts.Count; i++)
		{
			LogicEventSender logicEventSender = this.outputPorts[i] as LogicEventSender;
			if (logicEventSender == null)
			{
				return 0;
			}
			if (logicEventSender.ID == port_id)
			{
				return logicEventSender.GetLogicValue();
			}
		}
		return 0;
	}

	// Token: 0x06004A49 RID: 19017 RVA: 0x001AEBB8 File Offset: 0x001ACDB8
	public bool IsPortConnected(HashedString port_id)
	{
		int portCell = this.GetPortCell(port_id);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell) != null;
	}

	// Token: 0x06004A4A RID: 19018 RVA: 0x001AEBE0 File Offset: 0x001ACDE0
	private void OnOverlayChanged(HashedString mode)
	{
		if (mode == OverlayModes.Logic.ID)
		{
			base.enabled = true;
			this.CreateVisualizers();
			return;
		}
		base.enabled = false;
		this.DestroyVisualizers();
	}

	// Token: 0x06004A4B RID: 19019 RVA: 0x001AEC0C File Offset: 0x001ACE0C
	public LogicWire.BitDepth GetConnectedWireBitDepth(HashedString port_id)
	{
		LogicWire.BitDepth result = LogicWire.BitDepth.NumRatings;
		int portCell = this.GetPortCell(port_id);
		GameObject gameObject = Grid.Objects[portCell, 31];
		if (gameObject != null)
		{
			LogicWire component = gameObject.GetComponent<LogicWire>();
			if (component != null)
			{
				result = component.MaxBitDepth;
			}
		}
		return result;
	}

	// Token: 0x06004A4C RID: 19020 RVA: 0x001AEC54 File Offset: 0x001ACE54
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		LogicPorts component = go.GetComponent<LogicPorts>();
		if (component != null)
		{
			if (component.inputPortInfo != null && component.inputPortInfo.Length != 0)
			{
				Descriptor item = new Descriptor(UI.LOGIC_PORTS.INPUT_PORTS, UI.LOGIC_PORTS.INPUT_PORTS_TOOLTIP, Descriptor.DescriptorType.Effect, false);
				list.Add(item);
				foreach (LogicPorts.Port port in component.inputPortInfo)
				{
					string tooltip = string.Format(UI.LOGIC_PORTS.INPUT_PORT_TOOLTIP, port.activeDescription, port.inactiveDescription);
					item = new Descriptor(port.description, tooltip, Descriptor.DescriptorType.Effect, false);
					item.IncreaseIndent();
					list.Add(item);
				}
			}
			if (component.outputPortInfo != null && component.outputPortInfo.Length != 0)
			{
				Descriptor item2 = new Descriptor(UI.LOGIC_PORTS.OUTPUT_PORTS, UI.LOGIC_PORTS.OUTPUT_PORTS_TOOLTIP, Descriptor.DescriptorType.Effect, false);
				list.Add(item2);
				foreach (LogicPorts.Port port2 in component.outputPortInfo)
				{
					string tooltip2 = string.Format(UI.LOGIC_PORTS.OUTPUT_PORT_TOOLTIP, port2.activeDescription, port2.inactiveDescription);
					item2 = new Descriptor(port2.description, tooltip2, Descriptor.DescriptorType.Effect, false);
					item2.IncreaseIndent();
					list.Add(item2);
				}
			}
		}
		return list;
	}

	// Token: 0x06004A4D RID: 19021 RVA: 0x001AEDBC File Offset: 0x001ACFBC
	[OnSerializing]
	private void OnSerializing()
	{
		if (this.isPhysical && this.outputPorts != null)
		{
			this.serializedOutputValues = new int[this.outputPorts.Count];
			for (int i = 0; i < this.outputPorts.Count; i++)
			{
				LogicEventSender logicEventSender = this.outputPorts[i] as LogicEventSender;
				this.serializedOutputValues[i] = logicEventSender.GetLogicValue();
			}
		}
	}

	// Token: 0x06004A4E RID: 19022 RVA: 0x001AEE25 File Offset: 0x001AD025
	[OnSerialized]
	private void OnSerialized()
	{
		this.serializedOutputValues = null;
	}

	// Token: 0x04003146 RID: 12614
	[SerializeField]
	public LogicPorts.Port[] outputPortInfo;

	// Token: 0x04003147 RID: 12615
	[SerializeField]
	public LogicPorts.Port[] inputPortInfo;

	// Token: 0x04003148 RID: 12616
	public List<ILogicUIElement> outputPorts;

	// Token: 0x04003149 RID: 12617
	public List<ILogicUIElement> inputPorts;

	// Token: 0x0400314A RID: 12618
	private int cell = -1;

	// Token: 0x0400314B RID: 12619
	private Orientation orientation = Orientation.NumRotations;

	// Token: 0x0400314C RID: 12620
	[Serialize]
	private int[] serializedOutputValues;

	// Token: 0x0400314D RID: 12621
	private bool isPhysical;

	// Token: 0x02001A52 RID: 6738
	[Serializable]
	public struct Port
	{
		// Token: 0x0600A535 RID: 42293 RVA: 0x003B606D File Offset: 0x003B426D
		public Port(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon, LogicPortSpriteType sprite_type, bool display_custom_name = false)
		{
			this.id = id;
			this.cellOffset = cell_offset;
			this.description = description;
			this.activeDescription = activeDescription;
			this.inactiveDescription = inactiveDescription;
			this.requiresConnection = show_wire_missing_icon;
			this.spriteType = sprite_type;
			this.displayCustomName = display_custom_name;
		}

		// Token: 0x0600A536 RID: 42294 RVA: 0x003B60AC File Offset: 0x003B42AC
		public static LogicPorts.Port InputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Input, display_custom_name);
		}

		// Token: 0x0600A537 RID: 42295 RVA: 0x003B60BE File Offset: 0x003B42BE
		public static LogicPorts.Port OutputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Output, display_custom_name);
		}

		// Token: 0x0600A538 RID: 42296 RVA: 0x003B60D0 File Offset: 0x003B42D0
		public static LogicPorts.Port RibbonInputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.RibbonInput, display_custom_name);
		}

		// Token: 0x0600A539 RID: 42297 RVA: 0x003B60E2 File Offset: 0x003B42E2
		public static LogicPorts.Port RibbonOutputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.RibbonOutput, display_custom_name);
		}

		// Token: 0x0400815A RID: 33114
		public HashedString id;

		// Token: 0x0400815B RID: 33115
		public CellOffset cellOffset;

		// Token: 0x0400815C RID: 33116
		public string description;

		// Token: 0x0400815D RID: 33117
		public string activeDescription;

		// Token: 0x0400815E RID: 33118
		public string inactiveDescription;

		// Token: 0x0400815F RID: 33119
		public bool requiresConnection;

		// Token: 0x04008160 RID: 33120
		public LogicPortSpriteType spriteType;

		// Token: 0x04008161 RID: 33121
		public bool displayCustomName;
	}
}
