using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007A0 RID: 1952
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicMemory")]
public class LogicMemory : KMonoBehaviour
{
	// Token: 0x060032DF RID: 13023 RVA: 0x00123ED8 File Offset: 0x001220D8
	protected override void OnSpawn()
	{
		if (LogicMemory.infoStatusItem == null)
		{
			LogicMemory.infoStatusItem = new StatusItem("StoredValue", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			LogicMemory.infoStatusItem.resolveStringCallback = new Func<string, object, string>(LogicMemory.ResolveInfoStatusItemString);
		}
		base.Subscribe<LogicMemory>(-801688580, LogicMemory.OnLogicValueChangedDelegate);
	}

	// Token: 0x060032E0 RID: 13024 RVA: 0x00123F3C File Offset: 0x0012213C
	public void OnLogicValueChanged(object data)
	{
		if (this.ports == null || base.gameObject == null || this == null)
		{
			return;
		}
		if (((LogicValueChanged)data).portID != LogicMemory.READ_PORT_ID)
		{
			int inputValue = this.ports.GetInputValue(LogicMemory.SET_PORT_ID);
			int inputValue2 = this.ports.GetInputValue(LogicMemory.RESET_PORT_ID);
			int num = this.value;
			if (LogicCircuitNetwork.IsBitActive(0, inputValue2))
			{
				num = 0;
			}
			else if (LogicCircuitNetwork.IsBitActive(0, inputValue))
			{
				num = 1;
			}
			if (num != this.value)
			{
				this.value = num;
				this.ports.SendSignal(LogicMemory.READ_PORT_ID, this.value);
				KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					component.Play(LogicCircuitNetwork.IsBitActive(0, this.value) ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
				}
			}
		}
	}

	// Token: 0x060032E1 RID: 13025 RVA: 0x00124030 File Offset: 0x00122230
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		int outputValue = ((LogicMemory)data).ports.GetOutputValue(LogicMemory.READ_PORT_ID);
		return string.Format(BUILDINGS.PREFABS.LOGICMEMORY.STATUS_ITEM_VALUE, outputValue);
	}

	// Token: 0x04001ED2 RID: 7890
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x04001ED3 RID: 7891
	[Serialize]
	private int value;

	// Token: 0x04001ED4 RID: 7892
	private static StatusItem infoStatusItem;

	// Token: 0x04001ED5 RID: 7893
	public static readonly HashedString READ_PORT_ID = new HashedString("LogicMemoryRead");

	// Token: 0x04001ED6 RID: 7894
	public static readonly HashedString SET_PORT_ID = new HashedString("LogicMemorySet");

	// Token: 0x04001ED7 RID: 7895
	public static readonly HashedString RESET_PORT_ID = new HashedString("LogicMemoryReset");

	// Token: 0x04001ED8 RID: 7896
	private static readonly EventSystem.IntraObjectHandler<LogicMemory> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicMemory>(delegate(LogicMemory component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}
