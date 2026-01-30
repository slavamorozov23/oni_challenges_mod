using System;
using TemplateClasses;
using UnityEngine;

// Token: 0x020009C9 RID: 2505
public class StampToolPreview_Area : IStampToolPreviewPlugin
{
	// Token: 0x060048C3 RID: 18627 RVA: 0x001A3FF0 File Offset: 0x001A21F0
	public void Setup(StampToolPreviewContext context)
	{
		if (StampToolPreview_Area.material == null)
		{
			StampToolPreview_Area.material = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_background"));
			StampToolPreview_Area.material.name = "Area (" + StampToolPreview_Area.material.name + ")";
		}
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, new Action<string>(delegate(string error)
		{
			Color color = (error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK;
			color.a = 1f;
			StampToolPreview_Area.material.color = color;
		}));
		for (int i = 0; i < context.stampTemplate.cells.Count; i++)
		{
			Cell cell = context.stampTemplate.cells[i];
			MeshRenderer meshRenderer;
			GameObject gameObject;
			StampToolPreviewUtil.MakeQuad(out gameObject, out meshRenderer, 1f, null);
			gameObject.name = "AreaPlacer";
			gameObject.transform.SetParent(context.previewParent, false);
			gameObject.transform.localPosition = new Vector3((float)cell.location_x, (float)cell.location_y + Grid.HalfCellSizeInMeters);
			context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, new System.Action(delegate()
			{
				if (!gameObject.IsNullOrDestroyed())
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}));
			meshRenderer.sharedMaterial = StampToolPreview_Area.material;
		}
	}

	// Token: 0x0400306F RID: 12399
	public static Material material;
}
