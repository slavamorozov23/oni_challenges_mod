using System;
using System.Collections;
using UnityEngine;

// Token: 0x020009C7 RID: 2503
public class StampToolPreview
{
	// Token: 0x060048BA RID: 18618 RVA: 0x001A3BFF File Offset: 0x001A1DFF
	public StampToolPreview(InterfaceTool tool, params IStampToolPreviewPlugin[] plugins)
	{
		this.context = new StampToolPreviewContext();
		this.context.previewParent = new GameObject("StampToolPreview::Preview").transform;
		this.context.tool = tool;
		this.plugins = plugins;
	}

	// Token: 0x060048BB RID: 18619 RVA: 0x001A3C3F File Offset: 0x001A1E3F
	public IEnumerator Setup(TemplateContainer stampTemplate)
	{
		this.Cleanup();
		this.context.stampTemplate = stampTemplate;
		if (this.plugins != null)
		{
			IStampToolPreviewPlugin[] array = this.plugins;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Setup(this.context);
			}
		}
		yield return null;
		if (this.context.frameAfterSetupFn != null)
		{
			this.context.frameAfterSetupFn();
		}
		yield break;
	}

	// Token: 0x060048BC RID: 18620 RVA: 0x001A3C58 File Offset: 0x001A1E58
	public void Refresh(int originCell)
	{
		if (this.context.stampTemplate == null)
		{
			return;
		}
		if (originCell == this.prevOriginCell)
		{
			return;
		}
		this.prevOriginCell = originCell;
		if (!Grid.IsValidCell(originCell))
		{
			return;
		}
		if (this.context.refreshFn != null)
		{
			this.context.refreshFn(originCell);
		}
		this.context.previewParent.transform.SetPosition(Grid.CellToPosCBC(originCell, this.context.tool.visualizerLayer));
		this.context.previewParent.gameObject.SetActive(true);
	}

	// Token: 0x060048BD RID: 18621 RVA: 0x001A3CED File Offset: 0x001A1EED
	public void OnErrorChange(string error)
	{
		if (this.context.onErrorChangeFn != null)
		{
			this.context.onErrorChangeFn(error);
		}
	}

	// Token: 0x060048BE RID: 18622 RVA: 0x001A3D0D File Offset: 0x001A1F0D
	public void OnPlace()
	{
		if (this.context.onPlaceFn != null)
		{
			this.context.onPlaceFn();
		}
	}

	// Token: 0x060048BF RID: 18623 RVA: 0x001A3D2C File Offset: 0x001A1F2C
	public void Cleanup()
	{
		if (this.context.cleanupFn != null)
		{
			this.context.cleanupFn();
		}
		this.prevOriginCell = Grid.InvalidCell;
		this.context.stampTemplate = null;
		this.context.frameAfterSetupFn = null;
		this.context.refreshFn = null;
		this.context.onPlaceFn = null;
		this.context.cleanupFn = null;
	}

	// Token: 0x04003066 RID: 12390
	private IStampToolPreviewPlugin[] plugins;

	// Token: 0x04003067 RID: 12391
	private StampToolPreviewContext context;

	// Token: 0x04003068 RID: 12392
	private int prevOriginCell;
}
