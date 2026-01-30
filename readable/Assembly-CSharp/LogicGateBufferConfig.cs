using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002B7 RID: 695
public class LogicGateBufferConfig : LogicGateBaseConfig
{
	// Token: 0x06000E16 RID: 3606 RVA: 0x00052E61 File Offset: 0x00051061
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.CustomSingle;
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000E17 RID: 3607 RVA: 0x00052E64 File Offset: 0x00051064
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				CellOffset.none
			};
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000E18 RID: 3608 RVA: 0x00052E78 File Offset: 0x00051078
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000E19 RID: 3609 RVA: 0x00052E8E File Offset: 0x0005108E
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x00052E94 File Offset: 0x00051094
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x00052EE1 File Offset: 0x000510E1
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateBUFFER", "logic_buffer_kanim", 2, 1);
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x00052EF8 File Offset: 0x000510F8
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGateBuffer logicGateBuffer = go.AddComponent<LogicGateBuffer>();
		logicGateBuffer.op = this.GetLogicOp();
		logicGateBuffer.inputPortOffsets = this.InputPortOffsets;
		logicGateBuffer.outputPortOffsets = this.OutputPortOffsets;
		logicGateBuffer.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGateBuffer>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000959 RID: 2393
	public const string ID = "LogicGateBUFFER";
}
