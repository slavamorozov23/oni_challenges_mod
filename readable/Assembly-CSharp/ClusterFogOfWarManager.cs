using System;
using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200084A RID: 2122
public class ClusterFogOfWarManager : GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>
{
	// Token: 0x06003A2A RID: 14890 RVA: 0x00144CDC File Offset: 0x00142EDC
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.root;
		this.root.Enter(delegate(ClusterFogOfWarManager.Instance smi)
		{
			smi.Initialize();
		}).EventHandler(GameHashes.DiscoveredWorldsChanged, (ClusterFogOfWarManager.Instance smi) => Game.Instance, delegate(ClusterFogOfWarManager.Instance smi)
		{
			smi.UpdateRevealedCellsFromDiscoveredWorlds();
		});
	}

	// Token: 0x04002376 RID: 9078
	public const int AUTOMATIC_PEEK_RADIUS = 2;

	// Token: 0x020017F9 RID: 6137
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020017FA RID: 6138
	public new class Instance : GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>.GameInstance
	{
		// Token: 0x06009D44 RID: 40260 RVA: 0x0039C848 File Offset: 0x0039AA48
		public Instance(IStateMachineTarget master, ClusterFogOfWarManager.Def def) : base(master, def)
		{
		}

		// Token: 0x06009D45 RID: 40261 RVA: 0x0039C85D File Offset: 0x0039AA5D
		public void Initialize()
		{
			this.UpdateRevealedCellsFromDiscoveredWorlds();
			this.EnsureRevealedTilesHavePeek();
		}

		// Token: 0x06009D46 RID: 40262 RVA: 0x0039C86B File Offset: 0x0039AA6B
		public ClusterRevealLevel GetCellRevealLevel(AxialI location)
		{
			if (this.GetRevealCompleteFraction(location) >= 1f)
			{
				return ClusterRevealLevel.Visible;
			}
			if (this.GetRevealCompleteFraction(location) > 0f)
			{
				return ClusterRevealLevel.Peeked;
			}
			return ClusterRevealLevel.Hidden;
		}

		// Token: 0x06009D47 RID: 40263 RVA: 0x0039C88E File Offset: 0x0039AA8E
		public void DEBUG_REVEAL_ENTIRE_MAP()
		{
			this.RevealLocation(AxialI.ZERO, 100, 2);
		}

		// Token: 0x06009D48 RID: 40264 RVA: 0x0039C89E File Offset: 0x0039AA9E
		public bool IsLocationRevealed(AxialI location)
		{
			return ClusterGrid.Instance.IsValidCell(location) && this.GetRevealCompleteFraction(location) >= 1f;
		}

		// Token: 0x06009D49 RID: 40265 RVA: 0x0039C8C0 File Offset: 0x0039AAC0
		private void EnsureRevealedTilesHavePeek()
		{
			foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
			{
				if (this.IsLocationRevealed(keyValuePair.Key))
				{
					this.PeekLocation(keyValuePair.Key, 2);
				}
			}
		}

		// Token: 0x06009D4A RID: 40266 RVA: 0x0039C930 File Offset: 0x0039AB30
		public void PeekLocation(AxialI location, int radius)
		{
			foreach (AxialI key in AxialUtil.GetAllPointsWithinRadius(location, radius))
			{
				if (this.m_revealPointsByCell.ContainsKey(key))
				{
					this.m_revealPointsByCell[key] = Mathf.Max(this.m_revealPointsByCell[key], 0.01f);
				}
				else
				{
					this.m_revealPointsByCell[key] = 0.01f;
				}
			}
		}

		// Token: 0x06009D4B RID: 40267 RVA: 0x0039C9C0 File Offset: 0x0039ABC0
		public void RevealLocation(AxialI location, int radius = 0, int peekRadius = 2)
		{
			if (ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(location, EntityLayer.Asteroid).Count > 0 || ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.Asteroid) != null)
			{
				radius = Mathf.Max(radius, 1);
			}
			bool flag = false;
			foreach (AxialI cell in AxialUtil.GetAllPointsWithinRadius(location, radius))
			{
				flag |= this.RevealCellIfValid(cell, peekRadius);
			}
			if (flag)
			{
				Game.Instance.BoxingTrigger<AxialI>(-1991583975, location);
			}
		}

		// Token: 0x06009D4C RID: 40268 RVA: 0x0039CA60 File Offset: 0x0039AC60
		public bool EarnRevealPointsForLocation(AxialI location, float points)
		{
			global::Debug.Assert(ClusterGrid.Instance.IsValidCell(location), string.Format("EarnRevealPointsForLocation called with invalid location: {0}", location));
			if (this.IsLocationRevealed(location))
			{
				return false;
			}
			if (this.m_revealPointsByCell.ContainsKey(location))
			{
				Dictionary<AxialI, float> revealPointsByCell = this.m_revealPointsByCell;
				revealPointsByCell[location] += points;
			}
			else
			{
				this.m_revealPointsByCell[location] = points;
				Game.Instance.BoxingTrigger<AxialI>(-1554423969, location);
			}
			if (this.IsLocationRevealed(location))
			{
				this.RevealLocation(location, 0, 2);
				this.PeekLocation(location, 2);
				Game.Instance.BoxingTrigger<AxialI>(-1991583975, location);
				return true;
			}
			return false;
		}

		// Token: 0x06009D4D RID: 40269 RVA: 0x0039CB0C File Offset: 0x0039AD0C
		public float GetRevealCompleteFraction(AxialI location)
		{
			if (!ClusterGrid.Instance.IsValidCell(location))
			{
				global::Debug.LogError(string.Format("GetRevealCompleteFraction called with invalid location: {0}, {1}", location.r, location.q));
			}
			if (DebugHandler.RevealFogOfWar)
			{
				return 1f;
			}
			float num;
			if (this.m_revealPointsByCell.TryGetValue(location, out num))
			{
				return Mathf.Min(num / ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL, 1f);
			}
			return 0f;
		}

		// Token: 0x06009D4E RID: 40270 RVA: 0x0039CB7F File Offset: 0x0039AD7F
		private bool RevealCellIfValid(AxialI cell, int peekRadius = 2)
		{
			if (!ClusterGrid.Instance.IsValidCell(cell))
			{
				return false;
			}
			if (this.IsLocationRevealed(cell))
			{
				return false;
			}
			this.m_revealPointsByCell[cell] = ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL;
			this.PeekLocation(cell, peekRadius);
			return true;
		}

		// Token: 0x06009D4F RID: 40271 RVA: 0x0039CBB8 File Offset: 0x0039ADB8
		public bool GetUnrevealedLocationWithinRadius(AxialI center, int radius, out AxialI result)
		{
			for (int i = 0; i <= radius; i++)
			{
				foreach (AxialI axialI in AxialUtil.GetRing(center, i))
				{
					if (ClusterGrid.Instance.IsValidCell(axialI) && !this.IsLocationRevealed(axialI))
					{
						result = axialI;
						return true;
					}
				}
			}
			result = AxialI.ZERO;
			return false;
		}

		// Token: 0x06009D50 RID: 40272 RVA: 0x0039CC40 File Offset: 0x0039AE40
		public void UpdateRevealedCellsFromDiscoveredWorlds()
		{
			int radius = DlcManager.IsExpansion1Active() ? 0 : 2;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (worldContainer.IsDiscovered && !DebugHandler.RevealFogOfWar)
				{
					this.RevealLocation(worldContainer.GetComponent<ClusterGridEntity>().Location, radius, 2);
				}
			}
		}

		// Token: 0x04007952 RID: 31058
		[Serialize]
		private Dictionary<AxialI, float> m_revealPointsByCell = new Dictionary<AxialI, float>();
	}
}
