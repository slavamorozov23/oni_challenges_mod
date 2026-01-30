using System;
using UnityEngine;

// Token: 0x020009F1 RID: 2545
public class LogicPortVisualizer : ILogicUIElement, IUniformGridObject
{
	// Token: 0x06004A50 RID: 19024 RVA: 0x001AEE44 File Offset: 0x001AD044
	public LogicPortVisualizer(int cell, LogicPortSpriteType sprite_type)
	{
		this.cell = cell;
		this.spriteType = sprite_type;
	}

	// Token: 0x06004A51 RID: 19025 RVA: 0x001AEE5A File Offset: 0x001AD05A
	public int GetLogicUICell()
	{
		return this.cell;
	}

	// Token: 0x06004A52 RID: 19026 RVA: 0x001AEE62 File Offset: 0x001AD062
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06004A53 RID: 19027 RVA: 0x001AEE74 File Offset: 0x001AD074
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06004A54 RID: 19028 RVA: 0x001AEE86 File Offset: 0x001AD086
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x0400314E RID: 12622
	private int cell;

	// Token: 0x0400314F RID: 12623
	private LogicPortSpriteType spriteType;
}
