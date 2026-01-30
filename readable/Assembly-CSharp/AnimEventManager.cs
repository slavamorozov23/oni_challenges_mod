using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000542 RID: 1346
public class AnimEventManager : Singleton<AnimEventManager>
{
	// Token: 0x06001D22 RID: 7458 RVA: 0x0009E670 File Offset: 0x0009C870
	public void FreeResources()
	{
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x0009E674 File Offset: 0x0009C874
	public HandleVector<int>.Handle PlayAnim(KAnimControllerBase controller, KAnim.Anim anim, KAnim.PlayMode mode, float time, bool use_unscaled_time)
	{
		AnimEventManager.AnimData animData = default(AnimEventManager.AnimData);
		animData.frameRate = anim.frameRate;
		animData.totalTime = anim.totalTime;
		animData.numFrames = anim.numFrames;
		animData.useUnscaledTime = use_unscaled_time;
		AnimEventManager.EventPlayerData eventPlayerData = default(AnimEventManager.EventPlayerData);
		eventPlayerData.elapsedTime = time;
		eventPlayerData.mode = mode;
		eventPlayerData.controller = (controller as KBatchedAnimController);
		eventPlayerData.currentFrame = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
		eventPlayerData.previousFrame = -1;
		eventPlayerData.events = null;
		eventPlayerData.updatingEvents = null;
		eventPlayerData.events = GameAudioSheets.Get().GetEvents(anim.id);
		if (eventPlayerData.events == null)
		{
			eventPlayerData.events = AnimEventManager.emptyEventList;
		}
		HandleVector<int>.Handle result;
		if (animData.useUnscaledTime)
		{
			HandleVector<int>.Handle anim_data_handle = this.uiAnimData.Allocate(animData);
			HandleVector<int>.Handle event_data_handle = this.uiEventData.Allocate(eventPlayerData);
			result = this.indirectionData.Allocate(new AnimEventManager.IndirectionData(anim_data_handle, event_data_handle, true));
		}
		else
		{
			HandleVector<int>.Handle anim_data_handle2 = this.animData.Allocate(animData);
			HandleVector<int>.Handle event_data_handle2 = this.eventData.Allocate(eventPlayerData);
			result = this.indirectionData.Allocate(new AnimEventManager.IndirectionData(anim_data_handle2, event_data_handle2, false));
		}
		return result;
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x0009E7A8 File Offset: 0x0009C9A8
	public void SetMode(HandleVector<int>.Handle handle, KAnim.PlayMode mode)
	{
		if (!handle.IsValid())
		{
			return;
		}
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data.eventDataHandle);
		data2.mode = mode;
		kcompactedVector.SetData(data.eventDataHandle, data2);
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x0009E804 File Offset: 0x0009CA04
	public void StopAnim(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.AnimData> kcompactedVector = data.isUIData ? this.uiAnimData : this.animData;
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector2 = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector2.GetData(data.eventDataHandle);
		this.StopEvents(data2);
		kcompactedVector.Free(data.animDataHandle);
		kcompactedVector2.Free(data.eventDataHandle);
		this.indirectionData.Free(handle);
	}

	// Token: 0x06001D26 RID: 7462 RVA: 0x0009E890 File Offset: 0x0009CA90
	public float GetElapsedTime(HandleVector<int>.Handle handle)
	{
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		return (data.isUIData ? this.uiEventData : this.eventData).GetData(data.eventDataHandle).elapsedTime;
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x0009E8D0 File Offset: 0x0009CAD0
	public void SetElapsedTime(HandleVector<int>.Handle handle, float elapsed_time)
	{
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data.eventDataHandle);
		data2.elapsedTime = elapsed_time;
		kcompactedVector.SetData(data.eventDataHandle, data2);
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x0009E924 File Offset: 0x0009CB24
	public void SwapAnim(HandleVector<int>.Handle handle, KAnim.Anim anim)
	{
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.AnimData> kcompactedVector = data.isUIData ? this.uiAnimData : this.animData;
		AnimEventManager.AnimData data2 = kcompactedVector.GetData(data.eventDataHandle);
		data2.frameRate = anim.frameRate;
		data2.totalTime = anim.totalTime;
		data2.numFrames = anim.numFrames;
		kcompactedVector.SetData(data.animDataHandle, data2);
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector2 = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data3 = kcompactedVector2.GetData(data.eventDataHandle);
		data3.events = GameAudioSheets.Get().GetEvents(anim.id);
		if (data3.events == null)
		{
			data3.events = AnimEventManager.emptyEventList;
		}
		kcompactedVector2.SetData(data.eventDataHandle, data3);
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x0009E9F0 File Offset: 0x0009CBF0
	public void Update()
	{
		float deltaTime = Time.deltaTime;
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		this.Update(deltaTime, this.animData.GetDataList(), this.eventData.GetDataList());
		this.Update(unscaledDeltaTime, this.uiAnimData.GetDataList(), this.uiEventData.GetDataList());
		for (int i = 0; i < this.finishedCalls.Count; i++)
		{
			this.finishedCalls[i].TriggerStop();
		}
		this.finishedCalls.Clear();
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x0009EA78 File Offset: 0x0009CC78
	private void Update(float dt, List<AnimEventManager.AnimData> anim_data, List<AnimEventManager.EventPlayerData> event_data)
	{
		if (dt <= 0f)
		{
			return;
		}
		for (int i = 0; i < event_data.Count; i++)
		{
			AnimEventManager.EventPlayerData eventPlayerData = event_data[i];
			if (!(eventPlayerData.controller == null) && eventPlayerData.mode != KAnim.PlayMode.Paused)
			{
				eventPlayerData.currentFrame = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
				event_data[i] = eventPlayerData;
				this.PlayEvents(eventPlayerData);
				eventPlayerData.previousFrame = eventPlayerData.currentFrame;
				eventPlayerData.elapsedTime += dt * eventPlayerData.controller.GetPlaySpeed();
				event_data[i] = eventPlayerData;
				if (eventPlayerData.updatingEvents != null)
				{
					for (int j = 0; j < eventPlayerData.updatingEvents.Count; j++)
					{
						eventPlayerData.updatingEvents[j].OnUpdate(eventPlayerData);
					}
				}
				event_data[i] = eventPlayerData;
				if (eventPlayerData.mode != KAnim.PlayMode.Loop && eventPlayerData.currentFrame >= anim_data[i].numFrames - 1)
				{
					this.StopEvents(eventPlayerData);
					this.finishedCalls.Add(eventPlayerData.controller);
				}
			}
		}
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x0009EB90 File Offset: 0x0009CD90
	private void PlayEvents(AnimEventManager.EventPlayerData data)
	{
		for (int i = 0; i < data.events.Count; i++)
		{
			data.events[i].Play(data);
		}
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x0009EBC8 File Offset: 0x0009CDC8
	private void StopEvents(AnimEventManager.EventPlayerData data)
	{
		for (int i = 0; i < data.events.Count; i++)
		{
			data.events[i].Stop(data);
		}
		if (data.updatingEvents != null)
		{
			data.updatingEvents.Clear();
		}
	}

	// Token: 0x06001D2D RID: 7469 RVA: 0x0009EC10 File Offset: 0x0009CE10
	public AnimEventManager.DevTools_DebugInfo DevTools_GetDebugInfo()
	{
		return new AnimEventManager.DevTools_DebugInfo(this, this.animData, this.eventData, this.uiAnimData, this.uiEventData);
	}

	// Token: 0x04001118 RID: 4376
	private static readonly List<AnimEvent> emptyEventList = new List<AnimEvent>();

	// Token: 0x04001119 RID: 4377
	private const int INITIAL_VECTOR_SIZE = 256;

	// Token: 0x0400111A RID: 4378
	private KCompactedVector<AnimEventManager.AnimData> animData = new KCompactedVector<AnimEventManager.AnimData>(256);

	// Token: 0x0400111B RID: 4379
	private KCompactedVector<AnimEventManager.EventPlayerData> eventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);

	// Token: 0x0400111C RID: 4380
	private KCompactedVector<AnimEventManager.AnimData> uiAnimData = new KCompactedVector<AnimEventManager.AnimData>(256);

	// Token: 0x0400111D RID: 4381
	private KCompactedVector<AnimEventManager.EventPlayerData> uiEventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);

	// Token: 0x0400111E RID: 4382
	private KCompactedVector<AnimEventManager.IndirectionData> indirectionData = new KCompactedVector<AnimEventManager.IndirectionData>(0);

	// Token: 0x0400111F RID: 4383
	private List<KBatchedAnimController> finishedCalls = new List<KBatchedAnimController>();

	// Token: 0x020013E2 RID: 5090
	public struct AnimData
	{
		// Token: 0x04006C92 RID: 27794
		public float frameRate;

		// Token: 0x04006C93 RID: 27795
		public float totalTime;

		// Token: 0x04006C94 RID: 27796
		public int numFrames;

		// Token: 0x04006C95 RID: 27797
		public bool useUnscaledTime;
	}

	// Token: 0x020013E3 RID: 5091
	[DebuggerDisplay("{controller.name}, Anim={currentAnim}, Frame={currentFrame}, Mode={mode}")]
	public struct EventPlayerData
	{
		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06008E32 RID: 36402 RVA: 0x0036826B File Offset: 0x0036646B
		// (set) Token: 0x06008E33 RID: 36403 RVA: 0x00368273 File Offset: 0x00366473
		public int currentFrame { readonly get; set; }

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06008E34 RID: 36404 RVA: 0x0036827C File Offset: 0x0036647C
		// (set) Token: 0x06008E35 RID: 36405 RVA: 0x00368284 File Offset: 0x00366484
		public int previousFrame { readonly get; set; }

		// Token: 0x06008E36 RID: 36406 RVA: 0x0036828D File Offset: 0x0036648D
		public ComponentType GetComponent<ComponentType>()
		{
			return this.controller.GetComponent<ComponentType>();
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06008E37 RID: 36407 RVA: 0x0036829A File Offset: 0x0036649A
		public string name
		{
			get
			{
				return this.controller.name;
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06008E38 RID: 36408 RVA: 0x003682A7 File Offset: 0x003664A7
		public float normalizedTime
		{
			get
			{
				return this.elapsedTime / this.controller.CurrentAnim.totalTime;
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06008E39 RID: 36409 RVA: 0x003682C0 File Offset: 0x003664C0
		public Vector3 position
		{
			get
			{
				return this.controller.transform.GetPosition();
			}
		}

		// Token: 0x06008E3A RID: 36410 RVA: 0x003682D2 File Offset: 0x003664D2
		public void AddUpdatingEvent(AnimEvent ev)
		{
			if (this.updatingEvents == null)
			{
				this.updatingEvents = new List<AnimEvent>();
			}
			this.updatingEvents.Add(ev);
		}

		// Token: 0x06008E3B RID: 36411 RVA: 0x003682F3 File Offset: 0x003664F3
		public void SetElapsedTime(float elapsedTime)
		{
			this.elapsedTime = elapsedTime;
		}

		// Token: 0x06008E3C RID: 36412 RVA: 0x003682FC File Offset: 0x003664FC
		public void FreeResources()
		{
			this.elapsedTime = 0f;
			this.mode = KAnim.PlayMode.Once;
			this.currentFrame = 0;
			this.previousFrame = 0;
			this.events = null;
			this.updatingEvents = null;
			this.controller = null;
		}

		// Token: 0x04006C96 RID: 27798
		public float elapsedTime;

		// Token: 0x04006C97 RID: 27799
		public KAnim.PlayMode mode;

		// Token: 0x04006C9A RID: 27802
		public List<AnimEvent> events;

		// Token: 0x04006C9B RID: 27803
		public List<AnimEvent> updatingEvents;

		// Token: 0x04006C9C RID: 27804
		public KBatchedAnimController controller;
	}

	// Token: 0x020013E4 RID: 5092
	private struct IndirectionData
	{
		// Token: 0x06008E3D RID: 36413 RVA: 0x00368333 File Offset: 0x00366533
		public IndirectionData(HandleVector<int>.Handle anim_data_handle, HandleVector<int>.Handle event_data_handle, bool is_ui_data)
		{
			this.isUIData = is_ui_data;
			this.animDataHandle = anim_data_handle;
			this.eventDataHandle = event_data_handle;
		}

		// Token: 0x04006C9D RID: 27805
		public bool isUIData;

		// Token: 0x04006C9E RID: 27806
		public HandleVector<int>.Handle animDataHandle;

		// Token: 0x04006C9F RID: 27807
		public HandleVector<int>.Handle eventDataHandle;
	}

	// Token: 0x020013E5 RID: 5093
	public readonly struct DevTools_DebugInfo
	{
		// Token: 0x06008E3E RID: 36414 RVA: 0x0036834A File Offset: 0x0036654A
		public DevTools_DebugInfo(AnimEventManager eventManager, KCompactedVector<AnimEventManager.AnimData> animData, KCompactedVector<AnimEventManager.EventPlayerData> eventData, KCompactedVector<AnimEventManager.AnimData> uiAnimData, KCompactedVector<AnimEventManager.EventPlayerData> uiEventData)
		{
			this.eventManager = eventManager;
			this.animData = animData;
			this.eventData = eventData;
			this.uiAnimData = uiAnimData;
			this.uiEventData = uiEventData;
		}

		// Token: 0x04006CA0 RID: 27808
		public readonly AnimEventManager eventManager;

		// Token: 0x04006CA1 RID: 27809
		public readonly KCompactedVector<AnimEventManager.AnimData> animData;

		// Token: 0x04006CA2 RID: 27810
		public readonly KCompactedVector<AnimEventManager.EventPlayerData> eventData;

		// Token: 0x04006CA3 RID: 27811
		public readonly KCompactedVector<AnimEventManager.AnimData> uiAnimData;

		// Token: 0x04006CA4 RID: 27812
		public readonly KCompactedVector<AnimEventManager.EventPlayerData> uiEventData;
	}
}
