using System;
using UnityEngine;

// Token: 0x02000554 RID: 1364
public class KBatchedAnimTracker : MonoBehaviour
{
	// Token: 0x06001E4C RID: 7756 RVA: 0x000A4298 File Offset: 0x000A2498
	private void Start()
	{
		if (this.controller == null)
		{
			Transform parent = base.transform.parent;
			while (parent != null)
			{
				this.controller = parent.GetComponent<KBatchedAnimController>();
				if (this.controller != null)
				{
					break;
				}
				parent = parent.parent;
			}
		}
		if (this.controller == null)
		{
			global::Debug.Log("Controller Null for tracker on " + base.gameObject.name, base.gameObject);
			base.enabled = false;
			return;
		}
		this.controller.onAnimEnter += this.OnAnimStart;
		this.controller.onAnimComplete += this.OnAnimStop;
		this.controller.onLayerChanged += this.OnLayerChanged;
		this.forceUpdate = true;
		if (this.myAnim != null)
		{
			return;
		}
		this.myAnim = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController kbatchedAnimController = this.myAnim;
		kbatchedAnimController.getPositionDataFunctionInUse = (Func<Vector4>)Delegate.Combine(kbatchedAnimController.getPositionDataFunctionInUse, new Func<Vector4>(this.MyAnimGetPosition));
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x000A43B0 File Offset: 0x000A25B0
	private Vector4 MyAnimGetPosition()
	{
		if (this.myAnim != null && this.controller != null && this.controller.transform == this.myAnim.transform.parent)
		{
			Vector3 pivotSymbolPosition = this.myAnim.GetPivotSymbolPosition();
			return new Vector4(pivotSymbolPosition.x - this.controller.Offset.x, pivotSymbolPosition.y - this.controller.Offset.y, pivotSymbolPosition.x, pivotSymbolPosition.y);
		}
		return base.transform.GetPosition();
	}

	// Token: 0x06001E4E RID: 7758 RVA: 0x000A4458 File Offset: 0x000A2658
	private void OnDestroy()
	{
		if (this.controller != null)
		{
			this.controller.onAnimEnter -= this.OnAnimStart;
			this.controller.onAnimComplete -= this.OnAnimStop;
			this.controller.onLayerChanged -= this.OnLayerChanged;
			this.controller = null;
		}
		if (this.myAnim != null)
		{
			KBatchedAnimController kbatchedAnimController = this.myAnim;
			kbatchedAnimController.getPositionDataFunctionInUse = (Func<Vector4>)Delegate.Remove(kbatchedAnimController.getPositionDataFunctionInUse, new Func<Vector4>(this.MyAnimGetPosition));
		}
		this.myAnim = null;
	}

	// Token: 0x06001E4F RID: 7759 RVA: 0x000A44FC File Offset: 0x000A26FC
	private void LateUpdate()
	{
		if (this.controller != null && (this.controller.IsVisible() || this.forceAlwaysVisible || this.forceUpdate))
		{
			this.UpdateFrame();
		}
		if (!this.alive)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x000A4549 File Offset: 0x000A2749
	public void SetAnimControllers(KBatchedAnimController controller, KBatchedAnimController parentController)
	{
		this.myAnim = controller;
		this.controller = parentController;
	}

	// Token: 0x06001E51 RID: 7761 RVA: 0x000A455C File Offset: 0x000A275C
	private void UpdateFrame()
	{
		this.forceUpdate = false;
		bool flag = false;
		if (this.controller.CurrentAnim != null)
		{
			Matrix2x3 symbolLocalTransform = this.controller.GetSymbolLocalTransform(this.symbol, out flag);
			Vector3 position = this.controller.transform.GetPosition();
			if (flag && (this.previousMatrix != symbolLocalTransform || position != this.previousPosition || (this.useTargetPoint && this.targetPoint != this.previousTargetPoint) || (this.matchParentOffset && this.myAnim.Offset != this.controller.Offset)))
			{
				this.previousMatrix = symbolLocalTransform;
				this.previousPosition = position;
				Matrix2x3 overrideTransformMatrix = ((this.useTargetPoint || this.myAnim == null) ? this.controller.GetTransformMatrix() : this.controller.GetTransformMatrix(new Vector2(this.myAnim.animWidth * this.myAnim.animScale, -this.myAnim.animHeight * this.myAnim.animScale))) * symbolLocalTransform;
				float z = base.transform.GetPosition().z;
				base.transform.SetPosition(overrideTransformMatrix.MultiplyPoint(this.offset));
				if (this.useTargetPoint)
				{
					this.previousTargetPoint = this.targetPoint;
					Vector3 position2 = base.transform.GetPosition();
					position2.z = 0f;
					Vector3 vector = this.targetPoint - position2;
					float num = Vector3.Angle(vector, Vector3.right);
					if (vector.y < 0f)
					{
						num = 360f - num;
					}
					base.transform.localRotation = Quaternion.identity;
					base.transform.RotateAround(position2, new Vector3(0f, 0f, 1f), num);
					float sqrMagnitude = vector.sqrMagnitude;
					this.myAnim.GetBatchInstanceData().SetClipRadius(base.transform.GetPosition().x, base.transform.GetPosition().y, sqrMagnitude, true);
				}
				else
				{
					Vector3 v = this.controller.FlipX ? Vector3.left : Vector3.right;
					Vector3 v2 = this.controller.FlipY ? Vector3.down : Vector3.up;
					base.transform.up = overrideTransformMatrix.MultiplyVector(v2);
					base.transform.right = overrideTransformMatrix.MultiplyVector(v);
					if (this.myAnim != null)
					{
						KBatchedAnimInstanceData batchInstanceData = this.myAnim.GetBatchInstanceData();
						if (batchInstanceData != null)
						{
							batchInstanceData.SetOverrideTransformMatrix(overrideTransformMatrix);
						}
					}
				}
				base.transform.SetPosition(new Vector3(base.transform.GetPosition().x, base.transform.GetPosition().y, z));
				if (this.matchParentOffset)
				{
					this.myAnim.Offset = this.controller.Offset;
				}
				this.myAnim.SetDirty();
			}
		}
		if (this.myAnim != null && flag != this.myAnim.enabled && this.synchronizeEnabledState)
		{
			this.myAnim.enabled = flag;
		}
	}

	// Token: 0x06001E52 RID: 7762 RVA: 0x000A489E File Offset: 0x000A2A9E
	[ContextMenu("ForceAlive")]
	private void OnAnimStart(HashedString name)
	{
		this.alive = true;
		base.enabled = true;
		this.forceUpdate = true;
	}

	// Token: 0x06001E53 RID: 7763 RVA: 0x000A48B5 File Offset: 0x000A2AB5
	private void OnAnimStop(HashedString name)
	{
		if (!this.forceAlwaysAlive)
		{
			this.alive = false;
		}
	}

	// Token: 0x06001E54 RID: 7764 RVA: 0x000A48C6 File Offset: 0x000A2AC6
	private void OnLayerChanged(int layer)
	{
		this.myAnim.SetLayer(layer);
	}

	// Token: 0x06001E55 RID: 7765 RVA: 0x000A48D4 File Offset: 0x000A2AD4
	public void SetTarget(Vector3 target)
	{
		this.targetPoint = target;
		this.targetPoint.z = 0f;
	}

	// Token: 0x040011AA RID: 4522
	public KBatchedAnimController controller;

	// Token: 0x040011AB RID: 4523
	public Vector3 offset = Vector3.zero;

	// Token: 0x040011AC RID: 4524
	public HashedString symbol;

	// Token: 0x040011AD RID: 4525
	public Vector3 targetPoint = Vector3.zero;

	// Token: 0x040011AE RID: 4526
	public Vector3 previousTargetPoint;

	// Token: 0x040011AF RID: 4527
	public bool useTargetPoint;

	// Token: 0x040011B0 RID: 4528
	public bool fadeOut = true;

	// Token: 0x040011B1 RID: 4529
	public bool forceAlwaysVisible;

	// Token: 0x040011B2 RID: 4530
	public bool matchParentOffset;

	// Token: 0x040011B3 RID: 4531
	public bool forceAlwaysAlive;

	// Token: 0x040011B4 RID: 4532
	private bool alive = true;

	// Token: 0x040011B5 RID: 4533
	private bool forceUpdate;

	// Token: 0x040011B6 RID: 4534
	private Matrix2x3 previousMatrix;

	// Token: 0x040011B7 RID: 4535
	private Vector3 previousPosition;

	// Token: 0x040011B8 RID: 4536
	public bool synchronizeEnabledState = true;

	// Token: 0x040011B9 RID: 4537
	[SerializeField]
	private KBatchedAnimController myAnim;
}
