using System;
using UnityEngine;

// Token: 0x020009ED RID: 2541
internal class LogicEventSender : ILogicEventSender, ILogicNetworkConnection, ILogicUIElement, IUniformGridObject
{
	// Token: 0x06004A0B RID: 18955 RVA: 0x001AD4E2 File Offset: 0x001AB6E2
	public LogicEventSender(HashedString id, int cell, Action<int, int> on_value_changed, Action<int, bool> on_connection_changed, LogicPortSpriteType sprite_type)
	{
		this.id = id;
		this.cell = cell;
		this.onValueChanged = on_value_changed;
		this.onConnectionChanged = on_connection_changed;
		this.spriteType = sprite_type;
	}

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06004A0C RID: 18956 RVA: 0x001AD517 File Offset: 0x001AB717
	public HashedString ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x06004A0D RID: 18957 RVA: 0x001AD51F File Offset: 0x001AB71F
	public int GetLogicCell()
	{
		return this.cell;
	}

	// Token: 0x06004A0E RID: 18958 RVA: 0x001AD527 File Offset: 0x001AB727
	public int GetLogicValue()
	{
		return this.logicValue;
	}

	// Token: 0x06004A0F RID: 18959 RVA: 0x001AD52F File Offset: 0x001AB72F
	public int GetLogicUICell()
	{
		return this.GetLogicCell();
	}

	// Token: 0x06004A10 RID: 18960 RVA: 0x001AD537 File Offset: 0x001AB737
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x06004A11 RID: 18961 RVA: 0x001AD53F File Offset: 0x001AB73F
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06004A12 RID: 18962 RVA: 0x001AD551 File Offset: 0x001AB751
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06004A13 RID: 18963 RVA: 0x001AD564 File Offset: 0x001AB764
	public void SetValue(int value)
	{
		int arg = this.logicValue;
		this.logicValue = value;
		this.onValueChanged(value, arg);
	}

	// Token: 0x06004A14 RID: 18964 RVA: 0x001AD58C File Offset: 0x001AB78C
	public void LogicTick()
	{
	}

	// Token: 0x06004A15 RID: 18965 RVA: 0x001AD58E File Offset: 0x001AB78E
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
		if (this.onConnectionChanged != null)
		{
			this.onConnectionChanged(this.cell, connected);
		}
	}

	// Token: 0x0400312A RID: 12586
	private HashedString id;

	// Token: 0x0400312B RID: 12587
	private int cell;

	// Token: 0x0400312C RID: 12588
	private int logicValue = -16;

	// Token: 0x0400312D RID: 12589
	private Action<int, int> onValueChanged;

	// Token: 0x0400312E RID: 12590
	private Action<int, bool> onConnectionChanged;

	// Token: 0x0400312F RID: 12591
	private LogicPortSpriteType spriteType;
}
