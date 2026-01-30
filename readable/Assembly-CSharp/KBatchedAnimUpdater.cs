using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000555 RID: 1365
public class KBatchedAnimUpdater : Singleton<KBatchedAnimUpdater>
{
	// Token: 0x06001E57 RID: 7767 RVA: 0x000A4920 File Offset: 0x000A2B20
	public void InitializeGrid()
	{
		this.Clear();
		Vector2I visibleSize = this.GetVisibleSize();
		int num = (visibleSize.x + 32 - 1) / 32;
		int num2 = (visibleSize.y + 32 - 1) / 32;
		this.controllerGrid = new Dictionary<int, KBatchedAnimController>[num, num2];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				this.controllerGrid[j, i] = new Dictionary<int, KBatchedAnimController>();
			}
		}
		this.visibleChunks.Clear();
		this.previouslyVisibleChunks.Clear();
		this.previouslyVisibleChunkGrid = new bool[num, num2];
		this.visibleChunkGrid = new bool[num, num2];
		this.controllerChunkInfos.Clear();
		this.movingControllerInfos.Clear();
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x000A49D4 File Offset: 0x000A2BD4
	public Vector2I GetVisibleSize()
	{
		if (CameraController.Instance != null)
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
			return new Vector2I((int)((float)(vector2I2.x + vector2I.x) * KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.x), (int)((float)(vector2I2.y + vector2I.y) * KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.y));
		}
		return new Vector2I((int)((float)Grid.WidthInCells * KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.x), (int)((float)Grid.HeightInCells * KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.y));
	}

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06001E59 RID: 7769 RVA: 0x000A4A60 File Offset: 0x000A2C60
	// (remove) Token: 0x06001E5A RID: 7770 RVA: 0x000A4A98 File Offset: 0x000A2C98
	public event System.Action OnClear;

	// Token: 0x06001E5B RID: 7771 RVA: 0x000A4AD0 File Offset: 0x000A2CD0
	public void Clear()
	{
		foreach (KBatchedAnimController kbatchedAnimController in this.updateList)
		{
			if (kbatchedAnimController != null)
			{
				UnityEngine.Object.DestroyImmediate(kbatchedAnimController);
			}
		}
		this.updateList.Clear();
		foreach (KBatchedAnimController kbatchedAnimController2 in this.alwaysUpdateList)
		{
			if (kbatchedAnimController2 != null)
			{
				UnityEngine.Object.DestroyImmediate(kbatchedAnimController2);
			}
		}
		this.alwaysUpdateList.Clear();
		this.queuedRegistrations.Clear();
		this.visibleChunks.Clear();
		this.previouslyVisibleChunks.Clear();
		this.controllerGrid = null;
		this.previouslyVisibleChunkGrid = null;
		this.visibleChunkGrid = null;
		System.Action onClear = this.OnClear;
		if (onClear == null)
		{
			return;
		}
		onClear();
	}

	// Token: 0x06001E5C RID: 7772 RVA: 0x000A4BD4 File Offset: 0x000A2DD4
	public void UpdateRegister(KBatchedAnimController controller)
	{
		switch (controller.updateRegistrationState)
		{
		case KBatchedAnimUpdater.RegistrationState.Registered:
			break;
		case KBatchedAnimUpdater.RegistrationState.PendingRemoval:
			controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Registered;
			return;
		case KBatchedAnimUpdater.RegistrationState.Unregistered:
			((controller.visibilityType == KAnimControllerBase.VisibilityType.Always) ? this.alwaysUpdateList : this.updateList).Add(controller);
			controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Registered;
			break;
		default:
			return;
		}
	}

	// Token: 0x06001E5D RID: 7773 RVA: 0x000A4C28 File Offset: 0x000A2E28
	public void UpdateUnregister(KBatchedAnimController controller)
	{
		switch (controller.updateRegistrationState)
		{
		case KBatchedAnimUpdater.RegistrationState.Registered:
			controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.PendingRemoval;
			break;
		case KBatchedAnimUpdater.RegistrationState.PendingRemoval:
		case KBatchedAnimUpdater.RegistrationState.Unregistered:
			break;
		default:
			return;
		}
	}

	// Token: 0x06001E5E RID: 7774 RVA: 0x000A4C58 File Offset: 0x000A2E58
	public void VisibilityRegister(KBatchedAnimController controller)
	{
		this.queuedRegistrations.Add(new KBatchedAnimUpdater.RegistrationInfo
		{
			transformId = controller.transform.GetInstanceID(),
			controllerInstanceId = controller.GetInstanceID(),
			controller = controller,
			register = true
		});
	}

	// Token: 0x06001E5F RID: 7775 RVA: 0x000A4CA8 File Offset: 0x000A2EA8
	public void VisibilityUnregister(KBatchedAnimController controller)
	{
		if (App.IsExiting)
		{
			return;
		}
		this.queuedRegistrations.Add(new KBatchedAnimUpdater.RegistrationInfo
		{
			transformId = controller.transform.GetInstanceID(),
			controllerInstanceId = controller.GetInstanceID(),
			controller = controller,
			register = false
		});
	}

	// Token: 0x06001E60 RID: 7776 RVA: 0x000A4D00 File Offset: 0x000A2F00
	private Dictionary<int, KBatchedAnimController> GetControllerMap(Vector2I chunk_xy)
	{
		Dictionary<int, KBatchedAnimController> result = null;
		if (this.controllerGrid != null && 0 <= chunk_xy.x && chunk_xy.x < this.controllerGrid.GetLength(0) && 0 <= chunk_xy.y && chunk_xy.y < this.controllerGrid.GetLength(1))
		{
			result = this.controllerGrid[chunk_xy.x, chunk_xy.y];
		}
		return result;
	}

	// Token: 0x06001E61 RID: 7777 RVA: 0x000A4D6C File Offset: 0x000A2F6C
	public void LateUpdate()
	{
		this.ProcessMovingAnims();
		this.UpdateVisibility();
		this.ProcessRegistrations();
		this.CleanUp();
		float num = Time.unscaledDeltaTime;
		int count = this.alwaysUpdateList.Count;
		KBatchedAnimUpdater.UpdateRegisteredAnims(this.alwaysUpdateList, num);
		if (this.DoGridProcessing())
		{
			num = Time.deltaTime;
			if (num > 0f)
			{
				int count2 = this.updateList.Count;
				KBatchedAnimUpdater.UpdateRegisteredAnims(this.updateList, num);
			}
		}
	}

	// Token: 0x06001E62 RID: 7778 RVA: 0x000A4DE0 File Offset: 0x000A2FE0
	private static void UpdateRegisteredAnims(List<KBatchedAnimController> list, float dt)
	{
		for (int i = list.Count - 1; i >= 0; i--)
		{
			KBatchedAnimController kbatchedAnimController = list[i];
			if (kbatchedAnimController == null)
			{
				list[i] = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
			}
			else if (kbatchedAnimController.updateRegistrationState != KBatchedAnimUpdater.RegistrationState.Registered)
			{
				kbatchedAnimController.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Unregistered;
				list[i] = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
			}
			else if (kbatchedAnimController.forceUseGameTime)
			{
				kbatchedAnimController.UpdateAnim(Time.deltaTime);
			}
			else
			{
				kbatchedAnimController.UpdateAnim(dt);
			}
		}
	}

	// Token: 0x06001E63 RID: 7779 RVA: 0x000A4E8C File Offset: 0x000A308C
	public bool IsChunkVisible(Vector2I chunk_xy)
	{
		return this.visibleChunkGrid[chunk_xy.x, chunk_xy.y];
	}

	// Token: 0x06001E64 RID: 7780 RVA: 0x000A4EA5 File Offset: 0x000A30A5
	public void GetVisibleArea(out Vector2I vis_chunk_min, out Vector2I vis_chunk_max)
	{
		vis_chunk_min = this.vis_chunk_min;
		vis_chunk_max = this.vis_chunk_max;
	}

	// Token: 0x06001E65 RID: 7781 RVA: 0x000A4EBF File Offset: 0x000A30BF
	public static Vector2I PosToChunkXY(Vector3 pos)
	{
		return KAnimBatchManager.CellXYToChunkXY(Grid.PosToXY(pos));
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x000A4ECC File Offset: 0x000A30CC
	private void UpdateVisibility()
	{
		if (!this.DoGridProcessing())
		{
			return;
		}
		Vector2I vector2I;
		Vector2I vector2I2;
		Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2, 4, 1.5f);
		this.vis_chunk_min = new Vector2I(vector2I.x / 32, vector2I.y / 32);
		this.vis_chunk_max = new Vector2I(vector2I2.x / 32, vector2I2.y / 32);
		this.vis_chunk_max.x = Math.Min(this.vis_chunk_max.x, this.controllerGrid.GetLength(0) - 1);
		this.vis_chunk_max.y = Math.Min(this.vis_chunk_max.y, this.controllerGrid.GetLength(1) - 1);
		bool[,] array = this.previouslyVisibleChunkGrid;
		this.previouslyVisibleChunkGrid = this.visibleChunkGrid;
		this.visibleChunkGrid = array;
		Array.Clear(this.visibleChunkGrid, 0, this.visibleChunkGrid.Length);
		List<Vector2I> list = this.previouslyVisibleChunks;
		this.previouslyVisibleChunks = this.visibleChunks;
		this.visibleChunks = list;
		this.visibleChunks.Clear();
		for (int i = this.vis_chunk_min.y; i <= this.vis_chunk_max.y; i++)
		{
			for (int j = this.vis_chunk_min.x; j <= this.vis_chunk_max.x; j++)
			{
				this.visibleChunkGrid[j, i] = true;
				this.visibleChunks.Add(new Vector2I(j, i));
				if (!this.previouslyVisibleChunkGrid[j, i])
				{
					foreach (KeyValuePair<int, KBatchedAnimController> keyValuePair in this.controllerGrid[j, i])
					{
						KBatchedAnimController value = keyValuePair.Value;
						if (!(value == null))
						{
							value.SetVisiblity(true);
						}
					}
				}
			}
		}
		for (int k = 0; k < this.previouslyVisibleChunks.Count; k++)
		{
			Vector2I vector2I3 = this.previouslyVisibleChunks[k];
			if (!this.visibleChunkGrid[vector2I3.x, vector2I3.y])
			{
				foreach (KeyValuePair<int, KBatchedAnimController> keyValuePair2 in this.controllerGrid[vector2I3.x, vector2I3.y])
				{
					KBatchedAnimController value2 = keyValuePair2.Value;
					if (!(value2 == null))
					{
						value2.SetVisiblity(false);
					}
				}
			}
		}
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x000A5178 File Offset: 0x000A3378
	private void ProcessMovingAnims()
	{
		foreach (KBatchedAnimUpdater.MovingControllerInfo movingControllerInfo in this.movingControllerInfos.Values)
		{
			if (!(movingControllerInfo.controller == null))
			{
				Vector2I vector2I = KBatchedAnimUpdater.PosToChunkXY(movingControllerInfo.controller.PositionIncludingOffset);
				if (movingControllerInfo.chunkXY != vector2I)
				{
					KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = default(KBatchedAnimUpdater.ControllerChunkInfo);
					DebugUtil.Assert(this.controllerChunkInfos.TryGetValue(movingControllerInfo.controllerInstanceId, out controllerChunkInfo));
					DebugUtil.Assert(movingControllerInfo.controller == controllerChunkInfo.controller);
					DebugUtil.Assert(controllerChunkInfo.chunkXY == movingControllerInfo.chunkXY);
					Dictionary<int, KBatchedAnimController> controllerMap = this.GetControllerMap(controllerChunkInfo.chunkXY);
					if (controllerMap != null)
					{
						DebugUtil.Assert(controllerMap.ContainsKey(movingControllerInfo.controllerInstanceId));
						controllerMap.Remove(movingControllerInfo.controllerInstanceId);
					}
					controllerMap = this.GetControllerMap(vector2I);
					if (controllerMap != null)
					{
						DebugUtil.Assert(!controllerMap.ContainsKey(movingControllerInfo.controllerInstanceId));
						controllerMap[movingControllerInfo.controllerInstanceId] = controllerChunkInfo.controller;
					}
					movingControllerInfo.chunkXY = vector2I;
					controllerChunkInfo.chunkXY = vector2I;
					this.controllerChunkInfos[movingControllerInfo.controllerInstanceId] = controllerChunkInfo;
					if (controllerMap != null)
					{
						controllerChunkInfo.controller.SetVisiblity(this.visibleChunkGrid[vector2I.x, vector2I.y]);
					}
					else
					{
						controllerChunkInfo.controller.SetVisiblity(false);
					}
				}
			}
		}
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x000A5318 File Offset: 0x000A3518
	private void ProcessRegistrations()
	{
		for (int i = 0; i < this.queuedRegistrations.Count; i++)
		{
			KBatchedAnimUpdater.RegistrationInfo registrationInfo = this.queuedRegistrations[i];
			if (registrationInfo.register)
			{
				if (!(registrationInfo.controller == null))
				{
					int instanceID = registrationInfo.controller.GetInstanceID();
					DebugUtil.Assert(!this.controllerChunkInfos.ContainsKey(instanceID));
					KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = new KBatchedAnimUpdater.ControllerChunkInfo
					{
						controller = registrationInfo.controller,
						chunkXY = KBatchedAnimUpdater.PosToChunkXY(registrationInfo.controller.PositionIncludingOffset)
					};
					this.controllerChunkInfos[instanceID] = controllerChunkInfo;
					bool flag = false;
					if (Singleton<CellChangeMonitor>.Instance != null)
					{
						flag = Singleton<CellChangeMonitor>.Instance.IsMoving(registrationInfo.controller.transform);
						registrationInfo.controller.movementChangedHandlerId = Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(registrationInfo.controller.transform, KBatchedAnimUpdater.onMovementStateChangedAction, registrationInfo.controller);
					}
					Dictionary<int, KBatchedAnimController> controllerMap = this.GetControllerMap(controllerChunkInfo.chunkXY);
					if (controllerMap != null)
					{
						DebugUtil.Assert(!controllerMap.ContainsKey(instanceID));
						controllerMap.Add(instanceID, registrationInfo.controller);
					}
					if (flag)
					{
						DebugUtil.DevAssertArgs(!this.movingControllerInfos.ContainsKey(instanceID), new object[]
						{
							"Readding controller which is already moving",
							registrationInfo.controller.name,
							controllerChunkInfo.chunkXY,
							this.movingControllerInfos.ContainsKey(instanceID) ? this.movingControllerInfos[instanceID].chunkXY.ToString() : null
						});
						this.movingControllerInfos[instanceID] = new KBatchedAnimUpdater.MovingControllerInfo
						{
							controllerInstanceId = instanceID,
							controller = registrationInfo.controller,
							chunkXY = controllerChunkInfo.chunkXY
						};
					}
					if (controllerMap != null && this.visibleChunkGrid[controllerChunkInfo.chunkXY.x, controllerChunkInfo.chunkXY.y])
					{
						registrationInfo.controller.SetVisiblity(true);
					}
				}
			}
			else
			{
				KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo2 = default(KBatchedAnimUpdater.ControllerChunkInfo);
				if (this.controllerChunkInfos.TryGetValue(registrationInfo.controllerInstanceId, out controllerChunkInfo2))
				{
					if (registrationInfo.controller != null)
					{
						Dictionary<int, KBatchedAnimController> controllerMap2 = this.GetControllerMap(controllerChunkInfo2.chunkXY);
						if (controllerMap2 != null)
						{
							DebugUtil.Assert(controllerMap2.ContainsKey(registrationInfo.controllerInstanceId));
							controllerMap2.Remove(registrationInfo.controllerInstanceId);
						}
						registrationInfo.controller.SetVisiblity(false);
					}
					this.movingControllerInfos.Remove(registrationInfo.controllerInstanceId);
					Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(ref registrationInfo.controller.movementChangedHandlerId);
					this.controllerChunkInfos.Remove(registrationInfo.controllerInstanceId);
				}
			}
		}
		this.queuedRegistrations.Clear();
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x000A55D8 File Offset: 0x000A37D8
	public void OnMovementStateChanged(Transform transform, bool is_moving, KBatchedAnimController controller)
	{
		if (transform == null)
		{
			return;
		}
		if (controller == null)
		{
			return;
		}
		int instanceID = controller.GetInstanceID();
		KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = default(KBatchedAnimUpdater.ControllerChunkInfo);
		DebugUtil.Assert(this.controllerChunkInfos.TryGetValue(instanceID, out controllerChunkInfo));
		if (is_moving)
		{
			if (this.movingControllerInfos.ContainsKey(instanceID))
			{
				DebugUtil.DevAssertArgs(!this.movingControllerInfos.ContainsKey(instanceID), new object[]
				{
					"Readding controller which is already moving",
					controller.name,
					controllerChunkInfo.chunkXY,
					this.movingControllerInfos.ContainsKey(instanceID) ? this.movingControllerInfos[instanceID].chunkXY.ToString() : null
				});
			}
			this.movingControllerInfos[instanceID] = new KBatchedAnimUpdater.MovingControllerInfo
			{
				controllerInstanceId = instanceID,
				controller = controller,
				chunkXY = controllerChunkInfo.chunkXY
			};
			return;
		}
		this.movingControllerInfos.Remove(instanceID);
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x000A56D4 File Offset: 0x000A38D4
	private void CleanUp()
	{
		if (!this.DoGridProcessing())
		{
			return;
		}
		int length = this.controllerGrid.GetLength(0);
		for (int i = 0; i < 16; i++)
		{
			int num = (this.cleanUpChunkIndex + i) % this.controllerGrid.Length;
			int num2 = num % length;
			int num3 = num / length;
			Dictionary<int, KBatchedAnimController> dictionary = this.controllerGrid[num2, num3];
			ListPool<int, KBatchedAnimUpdater>.PooledList pooledList = ListPool<int, KBatchedAnimUpdater>.Allocate();
			foreach (KeyValuePair<int, KBatchedAnimController> keyValuePair in dictionary)
			{
				if (keyValuePair.Value == null)
				{
					pooledList.Add(keyValuePair.Key);
				}
			}
			foreach (int key in pooledList)
			{
				dictionary.Remove(key);
			}
			pooledList.Recycle();
		}
		this.cleanUpChunkIndex = (this.cleanUpChunkIndex + 16) % this.controllerGrid.Length;
	}

	// Token: 0x06001E6B RID: 7787 RVA: 0x000A57FC File Offset: 0x000A39FC
	private bool DoGridProcessing()
	{
		return this.controllerGrid != null && Camera.main != null;
	}

	// Token: 0x040011BA RID: 4538
	private const int VISIBLE_BORDER = 4;

	// Token: 0x040011BB RID: 4539
	public static readonly Vector2I INVALID_CHUNK_ID = Vector2I.minusone;

	// Token: 0x040011BC RID: 4540
	private Dictionary<int, KBatchedAnimController>[,] controllerGrid;

	// Token: 0x040011BD RID: 4541
	private List<KBatchedAnimController> updateList = new List<KBatchedAnimController>();

	// Token: 0x040011BE RID: 4542
	private List<KBatchedAnimController> alwaysUpdateList = new List<KBatchedAnimController>();

	// Token: 0x040011BF RID: 4543
	private bool[,] visibleChunkGrid;

	// Token: 0x040011C0 RID: 4544
	private bool[,] previouslyVisibleChunkGrid;

	// Token: 0x040011C1 RID: 4545
	private List<Vector2I> visibleChunks = new List<Vector2I>();

	// Token: 0x040011C2 RID: 4546
	private List<Vector2I> previouslyVisibleChunks = new List<Vector2I>();

	// Token: 0x040011C3 RID: 4547
	private Vector2I vis_chunk_min = Vector2I.zero;

	// Token: 0x040011C4 RID: 4548
	private Vector2I vis_chunk_max = Vector2I.zero;

	// Token: 0x040011C5 RID: 4549
	private List<KBatchedAnimUpdater.RegistrationInfo> queuedRegistrations = new List<KBatchedAnimUpdater.RegistrationInfo>();

	// Token: 0x040011C6 RID: 4550
	private Dictionary<int, KBatchedAnimUpdater.ControllerChunkInfo> controllerChunkInfos = new Dictionary<int, KBatchedAnimUpdater.ControllerChunkInfo>();

	// Token: 0x040011C7 RID: 4551
	private Dictionary<int, KBatchedAnimUpdater.MovingControllerInfo> movingControllerInfos = new Dictionary<int, KBatchedAnimUpdater.MovingControllerInfo>();

	// Token: 0x040011C8 RID: 4552
	private const int CHUNKS_TO_CLEAN_PER_TICK = 16;

	// Token: 0x040011C9 RID: 4553
	private int cleanUpChunkIndex;

	// Token: 0x040011CA RID: 4554
	private static readonly Vector2 VISIBLE_RANGE_SCALE = new Vector2(1.5f, 1.5f);

	// Token: 0x040011CC RID: 4556
	private static Action<Transform, bool, object> onMovementStateChangedAction = delegate(Transform transform, bool is_moving, object context)
	{
		Singleton<KBatchedAnimUpdater>.Instance.OnMovementStateChanged(transform, is_moving, Unsafe.As<KBatchedAnimController>(context));
	};

	// Token: 0x020013EF RID: 5103
	public enum RegistrationState
	{
		// Token: 0x04006CBA RID: 27834
		Registered,
		// Token: 0x04006CBB RID: 27835
		PendingRemoval,
		// Token: 0x04006CBC RID: 27836
		Unregistered
	}

	// Token: 0x020013F0 RID: 5104
	private struct RegistrationInfo
	{
		// Token: 0x04006CBD RID: 27837
		public bool register;

		// Token: 0x04006CBE RID: 27838
		public int transformId;

		// Token: 0x04006CBF RID: 27839
		public int controllerInstanceId;

		// Token: 0x04006CC0 RID: 27840
		public KBatchedAnimController controller;
	}

	// Token: 0x020013F1 RID: 5105
	private struct ControllerChunkInfo
	{
		// Token: 0x04006CC1 RID: 27841
		public KBatchedAnimController controller;

		// Token: 0x04006CC2 RID: 27842
		public Vector2I chunkXY;
	}

	// Token: 0x020013F2 RID: 5106
	private class MovingControllerInfo
	{
		// Token: 0x04006CC3 RID: 27843
		public int controllerInstanceId;

		// Token: 0x04006CC4 RID: 27844
		public KBatchedAnimController controller;

		// Token: 0x04006CC5 RID: 27845
		public Vector2I chunkXY;
	}
}
