using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using UnityEngine.Pool;

// Token: 0x020004FE RID: 1278
public static class AsyncPathProber
{
	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06001BAE RID: 7086 RVA: 0x0009939C File Offset: 0x0009759C
	// (set) Token: 0x06001BAF RID: 7087 RVA: 0x000993A3 File Offset: 0x000975A3
	public static AsyncPathProber.Manager Instance { get; private set; }

	// Token: 0x06001BB0 RID: 7088 RVA: 0x000993AB File Offset: 0x000975AB
	public static void CreateInstance(int count)
	{
		DebugUtil.Assert(AsyncPathProber.Instance == null);
		AsyncPathProber.Instance = new AsyncPathProber.Manager();
		AsyncPathProber.Instance.Start(count);
	}

	// Token: 0x06001BB1 RID: 7089 RVA: 0x000993CF File Offset: 0x000975CF
	public static void DestroyInstance()
	{
		AsyncPathProber.Instance.Shutdown();
		AsyncPathProber.Instance = null;
	}

	// Token: 0x04001027 RID: 4135
	public const int kMaxProbersPerFrame = 4;

	// Token: 0x02001396 RID: 5014
	public struct WorkResult
	{
		// Token: 0x04006BB9 RID: 27577
		public Navigator navigator;

		// Token: 0x04006BBA RID: 27578
		public PathGrid pathGrid;

		// Token: 0x04006BBB RID: 27579
		public List<int> reachableCells;

		// Token: 0x04006BBC RID: 27580
		public List<int> newlyReachableCells;

		// Token: 0x04006BBD RID: 27581
		public List<int> noLongerReachableCells;
	}

	// Token: 0x02001397 RID: 5015
	public struct WorkOrder
	{
		// Token: 0x06008C64 RID: 35940 RVA: 0x003612E8 File Offset: 0x0035F4E8
		public void Cleanup()
		{
			this.abilities.RecycleClone();
		}

		// Token: 0x06008C65 RID: 35941 RVA: 0x003612F8 File Offset: 0x0035F4F8
		public void Execute(PathFinder.PotentialList potentials, PathFinder.PotentialScratchPad scratch, ref AsyncPathProber.WorkResult result)
		{
			if (result.pathGrid.SerialNo >= this.serialNo)
			{
				result.pathGrid.ResetProberCells();
			}
			PathProber.Run(this.originCell, this.abilities, this.navGrid, this.startingNavType, result.pathGrid, this.serialNo, scratch, potentials, this.startingFlags, result.reachableCells);
			if (this.computeReachables)
			{
				result.reachableCells.Sort();
				int i = 0;
				int j = 0;
				while (i < this.navigator.occupiedCells.Count)
				{
					if (j >= result.reachableCells.Count)
					{
						break;
					}
					if (this.navigator.occupiedCells[i] < result.reachableCells[j])
					{
						result.noLongerReachableCells.Add(this.navigator.occupiedCells[i]);
						i++;
					}
					else if (result.reachableCells[j] < this.navigator.occupiedCells[i])
					{
						result.newlyReachableCells.Add(result.reachableCells[j]);
						j++;
					}
					else
					{
						i++;
						j++;
					}
				}
				while (i < this.navigator.occupiedCells.Count)
				{
					result.noLongerReachableCells.Add(this.navigator.occupiedCells[i]);
					i++;
				}
				while (j < result.reachableCells.Count)
				{
					result.newlyReachableCells.Add(result.reachableCells[j]);
					j++;
				}
			}
			this.Cleanup();
		}

		// Token: 0x04006BBE RID: 27582
		public Navigator navigator;

		// Token: 0x04006BBF RID: 27583
		public NavGrid navGrid;

		// Token: 0x04006BC0 RID: 27584
		public ulong gridClassification;

		// Token: 0x04006BC1 RID: 27585
		public PathFinderAbilities abilities;

		// Token: 0x04006BC2 RID: 27586
		public int originCell;

		// Token: 0x04006BC3 RID: 27587
		public NavType startingNavType;

		// Token: 0x04006BC4 RID: 27588
		public PathFinder.PotentialPath.Flags startingFlags;

		// Token: 0x04006BC5 RID: 27589
		public ushort serialNo;

		// Token: 0x04006BC6 RID: 27590
		public bool computeReachables;
	}

	// Token: 0x02001398 RID: 5016
	private static class AsyncPathProbeWorker
	{
		// Token: 0x06008C66 RID: 35942 RVA: 0x00361488 File Offset: 0x0035F688
		public static void main(object _)
		{
			PathFinder.PotentialList potentials = new PathFinder.PotentialList();
			PathFinder.PotentialScratchPad scratch = new PathFinder.PotentialScratchPad(Pathfinding.Instance.MaxLinksPerCell());
			try
			{
				while (!AsyncPathProber.Instance.Halting())
				{
					AsyncPathProber.WorkOrder workOrder;
					AsyncPathProber.WorkResult result;
					if (AsyncPathProber.Instance.NextTask(out workOrder, out result))
					{
						workOrder.Execute(potentials, scratch, ref result);
						AsyncPathProber.Instance.WorkCompleted(result);
					}
					else
					{
						Thread.Sleep(1);
					}
				}
			}
			catch (Exception source)
			{
				AsyncPathProber.Instance.SetException(ExceptionDispatchInfo.Capture(source));
			}
		}
	}

	// Token: 0x02001399 RID: 5017
	public class Manager
	{
		// Token: 0x06008C67 RID: 35943 RVA: 0x00361510 File Offset: 0x0035F710
		public bool Halting()
		{
			return this.halting;
		}

		// Token: 0x06008C68 RID: 35944 RVA: 0x00361518 File Offset: 0x0035F718
		public Manager()
		{
			this.navigatorOrderer = ((Navigator lhs, Navigator rhs) => this.navigators.GetValueOrDefault(rhs, 0).CompareTo(this.navigators.GetValueOrDefault(lhs, 0)));
		}

		// Token: 0x06008C69 RID: 35945 RVA: 0x003615C8 File Offset: 0x0035F7C8
		public void SetException(ExceptionDispatchInfo ex)
		{
			lock (this)
			{
				this.agentException = ex;
			}
		}

		// Token: 0x06008C6A RID: 35946 RVA: 0x00361604 File Offset: 0x0035F804
		public void Register(Navigator nav)
		{
			lock (this)
			{
				if (this.navigators.ContainsKey(nav))
				{
					Debug.LogWarning("Double registration of navigator to AsyncManager: " + nav.ToString());
				}
				if (!this.gridPool.ContainsKey(nav.PathGrid.AllocatedClassification))
				{
					bool flag2 = false;
					try
					{
						Monitor.Enter(this, ref flag2);
						int width = nav.PathGrid.widthInCells;
						int height = nav.PathGrid.heightInCells;
						bool applyOffset = nav.PathGrid.applyOffset;
						NavType[] navTypes = new NavType[nav.PathGrid.ValidNavTypes.Length];
						nav.PathGrid.ValidNavTypes.CopyTo(navTypes, 0);
						this.gridPool[nav.PathGrid.AllocatedClassification] = new ObjectPool<PathGrid>(() => new PathGrid(width, height, applyOffset, navTypes), null, null, null, false, 4 + this.agents.Length, 4 + this.agents.Length);
					}
					finally
					{
						if (flag2)
						{
							Monitor.Exit(this);
						}
					}
				}
				this.navigators[nav] = 10000;
			}
		}

		// Token: 0x06008C6B RID: 35947 RVA: 0x00361774 File Offset: 0x0035F974
		public void Unregister(Navigator nav)
		{
			lock (this)
			{
				if (!this.navigators.Remove(nav))
				{
					Debug.LogWarning("Unregister of unknown navigator from AsyncManager: " + nav.ToString());
				}
			}
		}

		// Token: 0x06008C6C RID: 35948 RVA: 0x003617CC File Offset: 0x0035F9CC
		public void WorkCompleted(AsyncPathProber.WorkResult result)
		{
			lock (this)
			{
				this.finishedWork.Add(result);
			}
		}

		// Token: 0x06008C6D RID: 35949 RVA: 0x00361810 File Offset: 0x0035FA10
		public bool NextTask(out AsyncPathProber.WorkOrder order, out AsyncPathProber.WorkResult result)
		{
			lock (this)
			{
				if (this.workQueue.Count > 0)
				{
					order = this.workQueue[0];
					this.workQueue.RemoveAt(0);
					if (this.navigators.ContainsKey(order.navigator))
					{
						result = new AsyncPathProber.WorkResult
						{
							navigator = order.navigator,
							pathGrid = this.gridPool[order.gridClassification].Get(),
							newlyReachableCells = this.indexListPool.Get(),
							noLongerReachableCells = this.indexListPool.Get(),
							reachableCells = this.indexListPool.Get()
						};
						this.navigators[order.navigator] = -1;
						return true;
					}
				}
			}
			order = default(AsyncPathProber.WorkOrder);
			result = default(AsyncPathProber.WorkResult);
			return false;
		}

		// Token: 0x06008C6E RID: 35950 RVA: 0x00361920 File Offset: 0x0035FB20
		private AsyncPathProber.WorkOrder makeWorkOrder(Navigator nav)
		{
			PathFinderAbilities currentAbilities = nav.GetCurrentAbilities();
			return new AsyncPathProber.WorkOrder
			{
				navigator = nav,
				navGrid = nav.NavGrid,
				gridClassification = nav.PathGrid.AllocatedClassification,
				abilities = currentAbilities.Clone(),
				originCell = nav.cachedCell,
				startingNavType = nav.CurrentNavType,
				startingFlags = nav.flags,
				serialNo = this.activeSerialNo,
				computeReachables = nav.reportOccupation
			};
		}

		// Token: 0x06008C6F RID: 35951 RVA: 0x003619B4 File Offset: 0x0035FBB4
		public void TickFrame()
		{
			lock (this)
			{
				if (this.agentException != null)
				{
					this.agentException.Throw();
					this.agentException = null;
				}
				this.activeSerialNo += 1;
				if (this.activeSerialNo == 0)
				{
					this.activeSerialNo += 1;
				}
				for (int i = 0; i < this.finishedWork.Count; i++)
				{
					AsyncPathProber.WorkResult workResult = this.finishedWork[i];
					PathGrid pathGrid = workResult.pathGrid;
					if (this.navigators.ContainsKey(workResult.navigator))
					{
						pathGrid = workResult.navigator.TakeResult(ref workResult);
						this.navigators[workResult.navigator] = 0;
					}
					if (pathGrid != null)
					{
						this.gridPool[pathGrid.AllocatedClassification].Release(pathGrid);
					}
					this.indexListPool.Release(workResult.reachableCells);
					this.indexListPool.Release(workResult.newlyReachableCells);
					this.indexListPool.Release(workResult.noLongerReachableCells);
				}
				this.finishedWork.Clear();
				foreach (KeyValuePair<Navigator, int> keyValuePair in this.navigators)
				{
					this.navigatorOrdering.Add(keyValuePair.Key);
				}
				for (int j = this.navigatorOrdering.Count - 1; j >= 0; j--)
				{
					Navigator key = this.navigatorOrdering[j];
					int num = this.navigators[key];
					if (num == -1)
					{
						this.navigatorOrdering.RemoveAtSwap(j);
					}
					else
					{
						this.navigators[key] = num + 1;
					}
				}
				this.navigatorOrdering.Sort(this.navigatorOrderer);
				for (int k = 0; k < this.workQueue.Count; k++)
				{
					this.workQueue[k].Cleanup();
				}
				this.workQueue.Clear();
				int num2 = 0;
				while (num2 < this.navigatorOrdering.Count && this.workQueue.Count < 4)
				{
					AsyncPathProber.WorkOrder workOrder = this.makeWorkOrder(this.navigatorOrdering[num2]);
					if (Grid.IsValidCell(workOrder.originCell))
					{
						this.workQueue.Add(workOrder);
					}
					else
					{
						workOrder.Cleanup();
					}
					num2++;
				}
				this.navigatorOrdering.Clear();
			}
		}

		// Token: 0x06008C70 RID: 35952 RVA: 0x00361C68 File Offset: 0x0035FE68
		public void ApplyNavigationFailedPenalty(Navigator nav)
		{
			AsyncPathProber.Manager.NavFailures++;
			lock (this)
			{
				int num;
				if (this.navigators.TryGetValue(nav, out num) && num >= 0)
				{
					this.navigators[nav] = num + 10;
				}
			}
		}

		// Token: 0x06008C71 RID: 35953 RVA: 0x00361CD0 File Offset: 0x0035FED0
		public void Start(int agentCount)
		{
			this.agents = new Thread[agentCount];
			this.halting = false;
			for (int i = 0; i < agentCount; i++)
			{
				this.agents[i] = new Thread(new ParameterizedThreadStart(AsyncPathProber.AsyncPathProbeWorker.main));
				this.agents[i].Start();
			}
		}

		// Token: 0x06008C72 RID: 35954 RVA: 0x00361D24 File Offset: 0x0035FF24
		public void Shutdown()
		{
			lock (this)
			{
				this.halting = true;
			}
			Thread[] array = this.agents;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Join();
			}
		}

		// Token: 0x04006BC7 RID: 27591
		private const int kNovelProberPenalty = 10000;

		// Token: 0x04006BC8 RID: 27592
		private const int kFailedNavigationPenalty = 10;

		// Token: 0x04006BC9 RID: 27593
		private const int kNavigatorInFlightValue = -1;

		// Token: 0x04006BCA RID: 27594
		private Dictionary<Navigator, int> navigators = new Dictionary<Navigator, int>();

		// Token: 0x04006BCB RID: 27595
		private List<Navigator> navigatorOrdering = new List<Navigator>();

		// Token: 0x04006BCC RID: 27596
		private Comparison<Navigator> navigatorOrderer;

		// Token: 0x04006BCD RID: 27597
		private ushort activeSerialNo;

		// Token: 0x04006BCE RID: 27598
		private Thread[] agents;

		// Token: 0x04006BCF RID: 27599
		private bool halting;

		// Token: 0x04006BD0 RID: 27600
		private ExceptionDispatchInfo agentException;

		// Token: 0x04006BD1 RID: 27601
		private List<AsyncPathProber.WorkOrder> workQueue = new List<AsyncPathProber.WorkOrder>();

		// Token: 0x04006BD2 RID: 27602
		private List<AsyncPathProber.WorkResult> finishedWork = new List<AsyncPathProber.WorkResult>();

		// Token: 0x04006BD3 RID: 27603
		private ConcurrentDictionary<ulong, ObjectPool<PathGrid>> gridPool = new ConcurrentDictionary<ulong, ObjectPool<PathGrid>>();

		// Token: 0x04006BD4 RID: 27604
		private ObjectPool<List<int>> indexListPool = new ObjectPool<List<int>>(() => new List<int>(Grid.CellCount / 8), null, delegate(List<int> list)
		{
			list.Clear();
		}, null, false, 12, 10000);

		// Token: 0x04006BD5 RID: 27605
		private static int NavFailures;
	}
}
