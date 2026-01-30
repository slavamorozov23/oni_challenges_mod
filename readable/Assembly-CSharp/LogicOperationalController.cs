using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020007A1 RID: 1953
[AddComponentMenu("KMonoBehaviour/scripts/LogicOperationalController")]
public class LogicOperationalController : KMonoBehaviour
{
	// Token: 0x060032E4 RID: 13028 RVA: 0x001240C4 File Offset: 0x001222C4
	public static List<LogicPorts.Port> CreateSingleInputPortList(CellOffset offset)
	{
		return new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, offset, UI.LOGIC_PORTS.CONTROL_OPERATIONAL, UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE, false, false)
		};
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x00124108 File Offset: 0x00122308
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicOperationalController>(-801688580, LogicOperationalController.OnLogicValueChangedDelegate);
		if (LogicOperationalController.infoStatusItem == null)
		{
			LogicOperationalController.infoStatusItem = new StatusItem("LogicOperationalInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			LogicOperationalController.infoStatusItem.resolveStringCallback = new Func<string, object, string>(LogicOperationalController.ResolveInfoStatusItemString);
		}
		this.CheckWireState();
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x00124178 File Offset: 0x00122378
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(LogicOperationalController.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x060032E7 RID: 13031 RVA: 0x001241A8 File Offset: 0x001223A8
	private LogicCircuitNetwork CheckWireState()
	{
		LogicCircuitNetwork network = this.GetNetwork();
		int value = (network != null) ? network.OutputValue : this.unNetworkedValue;
		this.operational.SetFlag(LogicOperationalController.LogicOperationalFlag, LogicCircuitNetwork.IsBitActive(0, value));
		return network;
	}

	// Token: 0x060032E8 RID: 13032 RVA: 0x001241E6 File Offset: 0x001223E6
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		return ((LogicOperationalController)data).operational.GetFlag(LogicOperationalController.LogicOperationalFlag) ? BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_ENABLED : BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_DISABLED;
	}

	// Token: 0x060032E9 RID: 13033 RVA: 0x00124210 File Offset: 0x00122410
	private void OnLogicValueChanged(object data)
	{
		if (((LogicValueChanged)data).portID == LogicOperationalController.PORT_ID)
		{
			LogicCircuitNetwork logicCircuitNetwork = this.CheckWireState();
			base.GetComponent<KSelectable>().ToggleStatusItem(LogicOperationalController.infoStatusItem, logicCircuitNetwork != null, this);
		}
	}

	// Token: 0x04001ED9 RID: 7897
	public static readonly HashedString PORT_ID = "LogicOperational";

	// Token: 0x04001EDA RID: 7898
	public int unNetworkedValue = 1;

	// Token: 0x04001EDB RID: 7899
	public static readonly Operational.Flag LogicOperationalFlag = new Operational.Flag("LogicOperational", Operational.Flag.Type.Requirement);

	// Token: 0x04001EDC RID: 7900
	private static StatusItem infoStatusItem;

	// Token: 0x04001EDD RID: 7901
	[MyCmpGet]
	public Operational operational;

	// Token: 0x04001EDE RID: 7902
	private static readonly EventSystem.IntraObjectHandler<LogicOperationalController> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicOperationalController>(delegate(LogicOperationalController component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}
