using System;
using System.Collections.Generic;
using System.Linq;
using ProcGen;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000CB5 RID: 3253
public class ClusterMapPath : MonoBehaviour
{
	// Token: 0x060063A8 RID: 25512 RVA: 0x002519F4 File Offset: 0x0024FBF4
	public void Init()
	{
		this.lineRenderer = base.gameObject.GetComponentInChildren<UILineRenderer>();
		base.gameObject.SetActive(true);
	}

	// Token: 0x060063A9 RID: 25513 RVA: 0x00251A13 File Offset: 0x0024FC13
	public void Init(List<Vector2> nodes, Color color)
	{
		this.m_nodes = nodes;
		this.m_color = color;
		this.lineRenderer = base.gameObject.GetComponentInChildren<UILineRenderer>();
		this.UpdateColor();
		this.UpdateRenderer();
		base.gameObject.SetActive(true);
	}

	// Token: 0x060063AA RID: 25514 RVA: 0x00251A4C File Offset: 0x0024FC4C
	public void SetColor(Color color)
	{
		this.m_color = color;
		this.UpdateColor();
	}

	// Token: 0x060063AB RID: 25515 RVA: 0x00251A5B File Offset: 0x0024FC5B
	private void UpdateColor()
	{
		this.lineRenderer.color = this.m_color;
		this.pathStart.color = this.m_color;
		this.pathEnd.color = this.m_color;
	}

	// Token: 0x060063AC RID: 25516 RVA: 0x00251A90 File Offset: 0x0024FC90
	public void SetPoints(List<Vector2> points)
	{
		this.m_nodes = points;
		this.UpdateRenderer();
	}

	// Token: 0x060063AD RID: 25517 RVA: 0x00251AA0 File Offset: 0x0024FCA0
	private void UpdateRenderer()
	{
		HashSet<Vector2> pointsOnCatmullRomSpline = ProcGen.Util.GetPointsOnCatmullRomSpline(this.m_nodes, 10);
		this.lineRenderer.Points = pointsOnCatmullRomSpline.ToArray<Vector2>();
		if (this.lineRenderer.Points.Length > 1)
		{
			this.pathStart.transform.localPosition = this.lineRenderer.Points[0];
			this.pathStart.gameObject.SetActive(true);
			Vector2 vector = this.lineRenderer.Points[this.lineRenderer.Points.Length - 1];
			Vector2 b = this.lineRenderer.Points[this.lineRenderer.Points.Length - 2];
			this.pathEnd.transform.localPosition = vector;
			Vector2 v = vector - b;
			this.pathEnd.transform.rotation = Quaternion.LookRotation(Vector3.forward, v);
			this.pathEnd.gameObject.SetActive(true);
			return;
		}
		this.pathStart.gameObject.SetActive(false);
		this.pathEnd.gameObject.SetActive(false);
	}

	// Token: 0x060063AE RID: 25518 RVA: 0x00251BC8 File Offset: 0x0024FDC8
	public float GetRotationForNextSegment()
	{
		if (this.m_nodes.Count > 1)
		{
			Vector2 b = this.m_nodes[0];
			Vector2 to = this.m_nodes[1] - b;
			return Vector2.SignedAngle(Vector2.up, to);
		}
		return 0f;
	}

	// Token: 0x040043BC RID: 17340
	private List<Vector2> m_nodes;

	// Token: 0x040043BD RID: 17341
	private Color m_color;

	// Token: 0x040043BE RID: 17342
	public UILineRenderer lineRenderer;

	// Token: 0x040043BF RID: 17343
	public Image pathStart;

	// Token: 0x040043C0 RID: 17344
	public Image pathEnd;
}
