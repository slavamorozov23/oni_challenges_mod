using System;
using System.Collections.Generic;
using TemplateClasses;
using UnityEngine;

// Token: 0x020009CA RID: 2506
public class StampToolPreview_Placers : IStampToolPreviewPlugin
{
	// Token: 0x060048C5 RID: 18629 RVA: 0x001A4150 File Offset: 0x001A2350
	public StampToolPreview_Placers(GameObject placerPrefab)
	{
		StampToolPreview_Placers <>4__this = this;
		this.pool = new GameObjectPool(delegate()
		{
			if (<>4__this.poolParent == null)
			{
				<>4__this.poolParent = new GameObject("StampToolPreview::PlacerPool").transform;
			}
			GameObject gameObject = Util.KInstantiate(placerPrefab, <>4__this.poolParent.gameObject, null);
			gameObject.SetActive(false);
			return gameObject;
		}, delegate(GameObject _)
		{
		}, 0);
	}

	// Token: 0x060048C6 RID: 18630 RVA: 0x001A41BC File Offset: 0x001A23BC
	public void Setup(StampToolPreviewContext context)
	{
		for (int i = 0; i < context.stampTemplate.cells.Count; i++)
		{
			Cell cell = context.stampTemplate.cells[i];
			GameObject instance = this.pool.GetInstance();
			instance.transform.SetParent(context.previewParent.transform, false);
			instance.transform.localPosition = new Vector3((float)cell.location_x, (float)cell.location_y);
			instance.SetActive(true);
			this.inUse.Add(instance);
		}
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, new Action<string>(delegate(string error)
		{
			foreach (GameObject gameObject in this.inUse)
			{
				if (!gameObject.IsNullOrDestroyed())
				{
					gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color = ((error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
				}
			}
		}));
		context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, new System.Action(delegate()
		{
			foreach (GameObject gameObject in this.inUse)
			{
				if (!gameObject.IsNullOrDestroyed())
				{
					gameObject.SetActive(false);
					gameObject.transform.SetParent(this.poolParent);
					this.pool.ReleaseInstance(gameObject);
				}
			}
			this.inUse.Clear();
		}));
	}

	// Token: 0x04003070 RID: 12400
	private List<GameObject> inUse = new List<GameObject>();

	// Token: 0x04003071 RID: 12401
	private GameObjectPool pool;

	// Token: 0x04003072 RID: 12402
	private Transform poolParent;
}
