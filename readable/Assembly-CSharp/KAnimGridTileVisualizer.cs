using System;
using Rendering;
using UnityEngine;

// Token: 0x020005F0 RID: 1520
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimGridTileVisualizer")]
public class KAnimGridTileVisualizer : KMonoBehaviour, IBlockTileInfo
{
	// Token: 0x0600232C RID: 9004 RVA: 0x000CC11B File Offset: 0x000CA31B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<KAnimGridTileVisualizer>(-1503271301, KAnimGridTileVisualizer.OnSelectionChangedDelegate);
		base.Subscribe<KAnimGridTileVisualizer>(-1201923725, KAnimGridTileVisualizer.OnHighlightChangedDelegate);
	}

	// Token: 0x0600232D RID: 9005 RVA: 0x000CC148 File Offset: 0x000CA348
	protected override void OnCleanUp()
	{
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			ObjectLayer tileLayer = component.Def.TileLayer;
			if (Grid.Objects[cell, (int)tileLayer] == base.gameObject)
			{
				Grid.Objects[cell, (int)tileLayer] = null;
			}
			TileVisualizer.RefreshCell(cell, tileLayer, component.Def.ReplacementLayer);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x000CC1C0 File Offset: 0x000CA3C0
	private void OnSelectionChanged(object data)
	{
		bool value = ((Boxed<bool>)data).value;
		World.Instance.blockTileRenderer.SelectCell(Grid.PosToCell(base.transform.GetPosition()), value);
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x000CC1FC File Offset: 0x000CA3FC
	private void OnHighlightChanged(object data)
	{
		bool value = ((Boxed<bool>)data).value;
		World.Instance.blockTileRenderer.HighlightCell(Grid.PosToCell(base.transform.GetPosition()), value);
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x000CC235 File Offset: 0x000CA435
	public int GetBlockTileConnectorID()
	{
		return this.blockTileConnectorID;
	}

	// Token: 0x04001495 RID: 5269
	[SerializeField]
	public int blockTileConnectorID;

	// Token: 0x04001496 RID: 5270
	private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnSelectionChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>(delegate(KAnimGridTileVisualizer component, object data)
	{
		component.OnSelectionChanged(data);
	});

	// Token: 0x04001497 RID: 5271
	private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnHighlightChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>(delegate(KAnimGridTileVisualizer component, object data)
	{
		component.OnHighlightChanged(data);
	});
}
