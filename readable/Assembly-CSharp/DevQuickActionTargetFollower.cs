using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000687 RID: 1671
public class DevQuickActionTargetFollower : MonoBehaviour
{
	// Token: 0x170001FF RID: 511
	// (get) Token: 0x0600292B RID: 10539 RVA: 0x000EAA71 File Offset: 0x000E8C71
	public new RectTransform transform
	{
		get
		{
			return base.transform as RectTransform;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x0600292C RID: 10540 RVA: 0x000EAA7E File Offset: 0x000E8C7E
	public bool IsToggleOn
	{
		get
		{
			return this.toggle.isOn;
		}
	}

	// Token: 0x0600292D RID: 10541 RVA: 0x000EAA8C File Offset: 0x000E8C8C
	private void Awake()
	{
		this.toggleOffColorBlock = this.toggle.colors;
		this.toggleOnColorBlock = this.toggle.colors;
		this.toggleOnColorBlock.normalColor = this.toggleOffColorBlock.pressedColor;
		this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
		this.toggle.SetIsOnWithoutNotify(true);
		this.RefreshToggleVisuals();
	}

	// Token: 0x0600292E RID: 10542 RVA: 0x000EAAFF File Offset: 0x000E8CFF
	public void ManualToggle(bool val)
	{
		this.toggle.isOn = val;
	}

	// Token: 0x0600292F RID: 10543 RVA: 0x000EAB0D File Offset: 0x000E8D0D
	public void OnToggleValueChanged(bool newValue)
	{
		this.RefreshToggleVisuals();
		Action<bool> onToggleChanged = this.OnToggleChanged;
		if (onToggleChanged == null)
		{
			return;
		}
		onToggleChanged(newValue);
	}

	// Token: 0x06002930 RID: 10544 RVA: 0x000EAB26 File Offset: 0x000E8D26
	public void RefreshToggleVisuals()
	{
		this.toggle.colors = (this.toggle.isOn ? this.toggleOnColorBlock : this.toggleOffColorBlock);
	}

	// Token: 0x06002931 RID: 10545 RVA: 0x000EAB4E File Offset: 0x000E8D4E
	public void SetTarget(GameObject target)
	{
		this.Target = target;
	}

	// Token: 0x06002932 RID: 10546 RVA: 0x000EAB57 File Offset: 0x000E8D57
	private void Update()
	{
		this.Refresh();
	}

	// Token: 0x06002933 RID: 10547 RVA: 0x000EAB60 File Offset: 0x000E8D60
	public void Refresh()
	{
		if (this.Target != null)
		{
			Vector3 vector = CameraController.Instance.overlayCamera.WorldToScreenPoint(this.Target.transform.position);
			this.targetPivot.transform.SetPosition(vector);
			Vector3 localPosition = this.targetPivot.localPosition;
			localPosition.z = 0f;
			this.targetPivot.localPosition = localPosition;
			Vector3 vector2 = this.transform.position - vector;
			vector2.z = 0f;
			Vector3 upwards = Vector3.Cross(Vector3.forward, vector2.normalized);
			this.line.rotation = Quaternion.LookRotation(Vector3.forward, upwards);
			Vector2 sizeDelta = this.line.sizeDelta;
			sizeDelta.x = this.targetPivot.localPosition.magnitude;
			this.line.sizeDelta = sizeDelta;
		}
	}

	// Token: 0x06002934 RID: 10548 RVA: 0x000EAC4F File Offset: 0x000E8E4F
	public void SetVisibleState(bool visible)
	{
		base.gameObject.SetActive(visible);
	}

	// Token: 0x0400184E RID: 6222
	public Toggle toggle;

	// Token: 0x0400184F RID: 6223
	public RectTransform targetPivot;

	// Token: 0x04001850 RID: 6224
	public RectTransform line;

	// Token: 0x04001851 RID: 6225
	private ColorBlock toggleOnColorBlock;

	// Token: 0x04001852 RID: 6226
	private ColorBlock toggleOffColorBlock;

	// Token: 0x04001853 RID: 6227
	private GameObject Target;

	// Token: 0x04001854 RID: 6228
	public Action<bool> OnToggleChanged;
}
