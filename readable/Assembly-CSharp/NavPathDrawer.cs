using System;
using UnityEngine;

// Token: 0x0200060D RID: 1549
[AddComponentMenu("KMonoBehaviour/scripts/NavPathDrawer")]
public class NavPathDrawer : KMonoBehaviour
{
	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06002443 RID: 9283 RVA: 0x000D1A57 File Offset: 0x000CFC57
	// (set) Token: 0x06002444 RID: 9284 RVA: 0x000D1A5E File Offset: 0x000CFC5E
	public static NavPathDrawer Instance { get; private set; }

	// Token: 0x06002445 RID: 9285 RVA: 0x000D1A66 File Offset: 0x000CFC66
	public static void DestroyInstance()
	{
		NavPathDrawer.Instance = null;
	}

	// Token: 0x06002446 RID: 9286 RVA: 0x000D1A70 File Offset: 0x000CFC70
	protected override void OnPrefabInit()
	{
		Shader shader = Shader.Find("Lines/Colored Blended");
		this.material = new Material(shader);
		NavPathDrawer.Instance = this;
	}

	// Token: 0x06002447 RID: 9287 RVA: 0x000D1A9A File Offset: 0x000CFC9A
	protected override void OnCleanUp()
	{
		NavPathDrawer.Instance = null;
	}

	// Token: 0x06002448 RID: 9288 RVA: 0x000D1AA2 File Offset: 0x000CFCA2
	public void DrawPath(Vector3 navigator_pos, PathFinder.Path path)
	{
		this.navigatorPos = navigator_pos;
		this.navigatorPos.y = this.navigatorPos.y + 0.5f;
		this.path = path;
	}

	// Token: 0x06002449 RID: 9289 RVA: 0x000D1ACE File Offset: 0x000CFCCE
	public Navigator GetNavigator()
	{
		return this.navigator;
	}

	// Token: 0x0600244A RID: 9290 RVA: 0x000D1AD6 File Offset: 0x000CFCD6
	public void SetNavigator(Navigator navigator)
	{
		this.navigator = navigator;
	}

	// Token: 0x0600244B RID: 9291 RVA: 0x000D1ADF File Offset: 0x000CFCDF
	public void ClearNavigator()
	{
		this.navigator = null;
	}

	// Token: 0x0600244C RID: 9292 RVA: 0x000D1AE8 File Offset: 0x000CFCE8
	private void DrawPath(PathFinder.Path path, Vector3 navigator_pos, Color color)
	{
		if (path.nodes != null && path.nodes.Count > 1)
		{
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.Begin(1);
			GL.Color(color);
			GL.Vertex(navigator_pos);
			GL.Vertex(NavTypeHelper.GetNavPos(path.nodes[1].cell, path.nodes[1].navType));
			for (int i = 1; i < path.nodes.Count - 1; i++)
			{
				if ((int)Grid.WorldIdx[path.nodes[i].cell] == ClusterManager.Instance.activeWorldId && (int)Grid.WorldIdx[path.nodes[i + 1].cell] == ClusterManager.Instance.activeWorldId)
				{
					Vector3 navPos = NavTypeHelper.GetNavPos(path.nodes[i].cell, path.nodes[i].navType);
					Vector3 navPos2 = NavTypeHelper.GetNavPos(path.nodes[i + 1].cell, path.nodes[i + 1].navType);
					GL.Vertex(navPos);
					GL.Vertex(navPos2);
				}
			}
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x0600244D RID: 9293 RVA: 0x000D1C34 File Offset: 0x000CFE34
	private void OnPostRender()
	{
		this.DrawPath(this.path, this.navigatorPos, Color.white);
		this.path = default(PathFinder.Path);
		this.DebugDrawSelectedNavigator();
		if (this.navigator != null)
		{
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.Begin(1);
			PathFinderQuery query = PathFinderQueries.drawNavGridQuery.Reset(null);
			this.navigator.RunQuery(query);
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x0600244E RID: 9294 RVA: 0x000D1CB4 File Offset: 0x000CFEB4
	private void DebugDrawSelectedNavigator()
	{
		if (!DebugHandler.DebugPathFinding)
		{
			return;
		}
		if (SelectTool.Instance == null)
		{
			return;
		}
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
		if (component == null)
		{
			return;
		}
		int mouseCell = DebugHandler.GetMouseCell();
		if (Grid.IsValidCell(mouseCell))
		{
			PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(Grid.PosToCell(component), component.CurrentNavType, component.flags);
			PathFinder.Path path = default(PathFinder.Path);
			if (!component.PathGrid.BuildPath(component.cachedCell, mouseCell, component.CurrentNavType, ref path))
			{
				PathFinder.UpdatePath(component.NavGrid, component.GetCurrentAbilities(), potential_path, PathFinderQueries.cellQuery.Reset(mouseCell), ref path);
			}
			string text = "";
			text = text + "Source: " + Grid.PosToCell(component).ToString() + "\n";
			text = text + "Dest: " + mouseCell.ToString() + "\n";
			text = text + "Cost: " + path.cost.ToString();
			this.DrawPath(path, component.GetComponent<KAnimControllerBase>().GetPivotSymbolPosition(), Color.green);
			DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
		}
	}

	// Token: 0x04001522 RID: 5410
	private PathFinder.Path path;

	// Token: 0x04001523 RID: 5411
	public Material material;

	// Token: 0x04001524 RID: 5412
	private Vector3 navigatorPos;

	// Token: 0x04001525 RID: 5413
	private Navigator navigator;
}
