using System;
using UnityEngine;

// Token: 0x02000594 RID: 1428
[AddComponentMenu("KMonoBehaviour/scripts/AnimEventHandler")]
public class AnimEventHandler : KMonoBehaviour
{
	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06001FFF RID: 8191 RVA: 0x000B8FE8 File Offset: 0x000B71E8
	// (remove) Token: 0x06002000 RID: 8192 RVA: 0x000B9020 File Offset: 0x000B7220
	private event AnimEventHandler.SetPos onWorkTargetSet;

	// Token: 0x06002001 RID: 8193 RVA: 0x000B9055 File Offset: 0x000B7255
	public int GetCachedCell()
	{
		return this.pickupable.cachedCell;
	}

	// Token: 0x06002002 RID: 8194 RVA: 0x000B9064 File Offset: 0x000B7264
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cachedTransform = base.transform;
		this.pickupable = base.GetComponent<Pickupable>();
		foreach (KBatchedAnimTracker kbatchedAnimTracker in base.GetComponentsInChildren<KBatchedAnimTracker>(true))
		{
			if (kbatchedAnimTracker.useTargetPoint)
			{
				this.onWorkTargetSet += kbatchedAnimTracker.SetTarget;
			}
		}
		this.baseOffset = this.animCollider.offset;
		AnimEventHandlerManager.Instance.Add(this);
	}

	// Token: 0x06002003 RID: 8195 RVA: 0x000B90DF File Offset: 0x000B72DF
	protected override void OnCleanUp()
	{
		AnimEventHandlerManager.Instance.Remove(this);
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x000B90EC File Offset: 0x000B72EC
	protected override void OnForcedCleanUp()
	{
		this.navigator = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06002005 RID: 8197 RVA: 0x000B90FB File Offset: 0x000B72FB
	public HashedString GetContext()
	{
		return this.context;
	}

	// Token: 0x06002006 RID: 8198 RVA: 0x000B9103 File Offset: 0x000B7303
	public void UpdateWorkTarget(Vector3 pos)
	{
		if (this.onWorkTargetSet != null)
		{
			this.onWorkTargetSet(pos);
		}
	}

	// Token: 0x06002007 RID: 8199 RVA: 0x000B9119 File Offset: 0x000B7319
	public void SetContext(HashedString context)
	{
		this.context = context;
	}

	// Token: 0x06002008 RID: 8200 RVA: 0x000B9122 File Offset: 0x000B7322
	public void SetTargetPos(Vector3 target_pos)
	{
		this.targetPos = target_pos;
	}

	// Token: 0x06002009 RID: 8201 RVA: 0x000B912B File Offset: 0x000B732B
	public Vector3 GetTargetPos()
	{
		return this.targetPos;
	}

	// Token: 0x0600200A RID: 8202 RVA: 0x000B9133 File Offset: 0x000B7333
	public void ClearContext()
	{
		this.context = default(HashedString);
	}

	// Token: 0x0600200B RID: 8203 RVA: 0x000B9144 File Offset: 0x000B7344
	public void UpdateOffset()
	{
		Vector3 pivotSymbolPosition = this.controller.GetPivotSymbolPosition();
		Vector3 vector = this.navigator.NavGrid.GetNavTypeData(this.navigator.CurrentNavType).animControllerOffset;
		Vector3 position = this.cachedTransform.position;
		Vector2 vector2 = new Vector2(this.baseOffset.x + pivotSymbolPosition.x - position.x - vector.x, this.baseOffset.y + pivotSymbolPosition.y - position.y + vector.y);
		if (this.animCollider.offset != vector2)
		{
			this.animCollider.offset = vector2;
		}
	}

	// Token: 0x040012A0 RID: 4768
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x040012A1 RID: 4769
	[MyCmpGet]
	private KBoxCollider2D animCollider;

	// Token: 0x040012A2 RID: 4770
	[MyCmpGet]
	private Navigator navigator;

	// Token: 0x040012A3 RID: 4771
	private Pickupable pickupable;

	// Token: 0x040012A4 RID: 4772
	private Vector3 targetPos;

	// Token: 0x040012A5 RID: 4773
	public Transform cachedTransform;

	// Token: 0x040012A7 RID: 4775
	public Vector2 baseOffset;

	// Token: 0x040012A8 RID: 4776
	private HashedString context;

	// Token: 0x02001415 RID: 5141
	// (Invoke) Token: 0x06008E84 RID: 36484
	private delegate void SetPos(Vector3 pos);
}
