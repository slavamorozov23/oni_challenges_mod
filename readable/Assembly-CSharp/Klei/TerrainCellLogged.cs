using System;
using System.Collections.Generic;
using ProcGen.Map;
using ProcGenGame;
using VoronoiTree;

namespace Klei
{
	// Token: 0x02001010 RID: 4112
	public class TerrainCellLogged : TerrainCell
	{
		// Token: 0x06007F9B RID: 32667 RVA: 0x0033576E File Offset: 0x0033396E
		public TerrainCellLogged()
		{
		}

		// Token: 0x06007F9C RID: 32668 RVA: 0x00335776 File Offset: 0x00333976
		public TerrainCellLogged(Cell node, Diagram.Site site, Dictionary<Tag, int> distancesToTags) : base(node, site, distancesToTags)
		{
		}

		// Token: 0x06007F9D RID: 32669 RVA: 0x00335781 File Offset: 0x00333981
		public override void LogInfo(string evt, string param, float value)
		{
		}
	}
}
