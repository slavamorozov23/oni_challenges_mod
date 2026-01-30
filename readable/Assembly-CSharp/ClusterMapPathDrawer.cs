using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000CB6 RID: 3254
public class ClusterMapPathDrawer : MonoBehaviour
{
	// Token: 0x060063B0 RID: 25520 RVA: 0x00251C1C File Offset: 0x0024FE1C
	public ClusterMapPath AddPath()
	{
		ClusterMapPath clusterMapPath = UnityEngine.Object.Instantiate<ClusterMapPath>(this.pathPrefab, this.pathContainer);
		clusterMapPath.Init();
		return clusterMapPath;
	}

	// Token: 0x060063B1 RID: 25521 RVA: 0x00251C35 File Offset: 0x0024FE35
	public static List<Vector2> GetDrawPathList(Vector2 startLocation, List<AxialI> pathPoints)
	{
		List<Vector2> list = new List<Vector2>();
		list.Add(startLocation);
		list.AddRange(from point in pathPoints
		select point.ToWorld2D());
		return list;
	}

	// Token: 0x040043C1 RID: 17345
	public ClusterMapPath pathPrefab;

	// Token: 0x040043C2 RID: 17346
	public Transform pathContainer;
}
