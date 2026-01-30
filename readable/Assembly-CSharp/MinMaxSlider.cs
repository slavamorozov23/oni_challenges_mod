using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ECE RID: 3790
[AddComponentMenu("KMonoBehaviour/scripts/MinMaxSlider")]
public class MinMaxSlider : KMonoBehaviour
{
	// Token: 0x1700084D RID: 2125
	// (get) Token: 0x06007953 RID: 31059 RVA: 0x002E9EDD File Offset: 0x002E80DD
	// (set) Token: 0x06007954 RID: 31060 RVA: 0x002E9EE5 File Offset: 0x002E80E5
	public MinMaxSlider.Mode mode { get; private set; }

	// Token: 0x06007955 RID: 31061 RVA: 0x002E9EF0 File Offset: 0x002E80F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ToolTip component = base.transform.parent.gameObject.GetComponent<ToolTip>();
		if (component != null)
		{
			UnityEngine.Object.DestroyImmediate(this.toolTip);
			this.toolTip = component;
		}
		this.minSlider.value = this.currentMinValue;
		this.maxSlider.value = this.currentMaxValue;
		this.minSlider.interactable = this.interactable;
		this.maxSlider.interactable = this.interactable;
		this.minSlider.maxValue = this.maxLimit;
		this.maxSlider.maxValue = this.maxLimit;
		this.minSlider.minValue = this.minLimit;
		this.maxSlider.minValue = this.minLimit;
		this.minSlider.direction = (this.maxSlider.direction = this.direction);
		if (this.isOverPowered != null)
		{
			this.isOverPowered.enabled = false;
		}
		this.minSlider.gameObject.SetActive(false);
		if (this.mode != MinMaxSlider.Mode.Single)
		{
			this.minSlider.gameObject.SetActive(true);
		}
		if (this.extraSlider != null)
		{
			this.extraSlider.value = this.currentExtraValue;
			this.extraSlider.wholeNumbers = (this.minSlider.wholeNumbers = (this.maxSlider.wholeNumbers = this.wholeNumbers));
			this.extraSlider.direction = this.direction;
			this.extraSlider.interactable = this.interactable;
			this.extraSlider.maxValue = this.maxLimit;
			this.extraSlider.minValue = this.minLimit;
			this.extraSlider.gameObject.SetActive(false);
			if (this.mode == MinMaxSlider.Mode.Triple)
			{
				this.extraSlider.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06007956 RID: 31062 RVA: 0x002EA0E0 File Offset: 0x002E82E0
	public void SetIcon(Image newIcon)
	{
		this.icon = newIcon;
		this.icon.gameObject.transform.SetParent(base.transform);
		this.icon.gameObject.transform.SetAsFirstSibling();
		this.icon.rectTransform().anchoredPosition = Vector2.zero;
	}

	// Token: 0x06007957 RID: 31063 RVA: 0x002EA13C File Offset: 0x002E833C
	public void SetMode(MinMaxSlider.Mode mode)
	{
		this.mode = mode;
		if (mode == MinMaxSlider.Mode.Single && this.extraSlider != null)
		{
			this.extraSlider.gameObject.SetActive(false);
			this.extraSlider.handleRect.gameObject.SetActive(false);
		}
	}

	// Token: 0x06007958 RID: 31064 RVA: 0x002EA188 File Offset: 0x002E8388
	private void SetAnchor(RectTransform trans, Vector2 min, Vector2 max)
	{
		trans.anchorMin = min;
		trans.anchorMax = max;
	}

	// Token: 0x06007959 RID: 31065 RVA: 0x002EA198 File Offset: 0x002E8398
	public void SetMinMaxValue(float currentMin, float currentMax, float min, float max)
	{
		this.minSlider.value = currentMin;
		this.currentMinValue = currentMin;
		this.maxSlider.value = currentMax;
		this.currentMaxValue = currentMax;
		this.minLimit = min;
		this.maxLimit = max;
		this.minSlider.minValue = this.minLimit;
		this.maxSlider.minValue = this.minLimit;
		this.minSlider.maxValue = this.maxLimit;
		this.maxSlider.maxValue = this.maxLimit;
		if (this.extraSlider != null)
		{
			this.extraSlider.minValue = this.minLimit;
			this.extraSlider.maxValue = this.maxLimit;
		}
	}

	// Token: 0x0600795A RID: 31066 RVA: 0x002EA252 File Offset: 0x002E8452
	public void SetExtraValue(float current)
	{
		this.extraSlider.value = current;
		this.toolTip.toolTip = base.transform.parent.name + ": " + current.ToString("F2");
	}

	// Token: 0x0600795B RID: 31067 RVA: 0x002EA294 File Offset: 0x002E8494
	public void SetMaxValue(float current, float max)
	{
		float num = current / max * 100f;
		if (this.isOverPowered != null)
		{
			this.isOverPowered.enabled = (num > 100f);
		}
		this.maxSlider.value = Mathf.Min(100f, num);
		if (this.toolTip != null)
		{
			this.toolTip.toolTip = string.Concat(new string[]
			{
				base.transform.parent.name,
				": ",
				current.ToString("F2"),
				"/",
				max.ToString("F2")
			});
		}
	}

	// Token: 0x0600795C RID: 31068 RVA: 0x002EA348 File Offset: 0x002E8548
	private void Update()
	{
		if (!this.interactable)
		{
			return;
		}
		this.minSlider.value = Mathf.Clamp(this.currentMinValue, this.minLimit, this.currentMinValue);
		this.maxSlider.value = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.currentMaxValue, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
		if (this.direction == Slider.Direction.LeftToRight || this.direction == Slider.Direction.RightToLeft)
		{
			this.minRect.anchorMax = new Vector2(this.minSlider.value / this.maxLimit, this.minRect.anchorMax.y);
			this.maxRect.anchorMax = new Vector2(this.maxSlider.value / this.maxLimit, this.maxRect.anchorMax.y);
			this.maxRect.anchorMin = new Vector2(this.minSlider.value / this.maxLimit, this.maxRect.anchorMin.y);
			return;
		}
		this.minRect.anchorMax = new Vector2(this.minRect.anchorMin.x, this.minSlider.value / this.maxLimit);
		this.maxRect.anchorMin = new Vector2(this.maxRect.anchorMin.x, this.minSlider.value / this.maxLimit);
	}

	// Token: 0x0600795D RID: 31069 RVA: 0x002EA4D4 File Offset: 0x002E86D4
	public void OnMinValueChanged(float ignoreThis)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange)
		{
			this.currentMaxValue = Mathf.Min(Mathf.Max(this.minLimit, this.minSlider.value) + this.range, this.maxLimit);
			this.currentMinValue = Mathf.Max(this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue - this.range));
		}
		else
		{
			this.currentMinValue = Mathf.Clamp(this.minSlider.value, this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue));
		}
		if (this.onMinChange != null)
		{
			this.onMinChange(this);
		}
	}

	// Token: 0x0600795E RID: 31070 RVA: 0x002EA598 File Offset: 0x002E8798
	public void OnMaxValueChanged(float ignoreThis)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange)
		{
			this.currentMinValue = Mathf.Max(this.maxSlider.value - this.range, this.minLimit);
			this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.currentMinValue + this.range, this.minLimit), this.maxLimit));
		}
		else
		{
			this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
		}
		if (this.onMaxChange != null)
		{
			this.onMaxChange(this);
		}
	}

	// Token: 0x0600795F RID: 31071 RVA: 0x002EA678 File Offset: 0x002E8878
	public void Lock(bool shouldLock)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockType == MinMaxSlider.LockingType.Drag)
		{
			this.lockRange = shouldLock;
			this.range = this.maxSlider.value - this.minSlider.value;
			this.mousePos = KInputManager.GetMousePos();
		}
	}

	// Token: 0x06007960 RID: 31072 RVA: 0x002EA6C8 File Offset: 0x002E88C8
	public void ToggleLock()
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockType == MinMaxSlider.LockingType.Toggle)
		{
			this.lockRange = !this.lockRange;
			if (this.lockRange)
			{
				this.range = this.maxSlider.value - this.minSlider.value;
			}
		}
	}

	// Token: 0x06007961 RID: 31073 RVA: 0x002EA71C File Offset: 0x002E891C
	public void OnDrag()
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange && this.lockType == MinMaxSlider.LockingType.Drag)
		{
			float num = KInputManager.GetMousePos().x - this.mousePos.x;
			if (this.direction == Slider.Direction.TopToBottom || this.direction == Slider.Direction.BottomToTop)
			{
				num = KInputManager.GetMousePos().y - this.mousePos.y;
			}
			this.currentMinValue = Mathf.Max(this.currentMinValue + num, this.minLimit);
			this.mousePos = KInputManager.GetMousePos();
		}
	}

	// Token: 0x040054B2 RID: 21682
	public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;

	// Token: 0x040054B4 RID: 21684
	public bool lockRange;

	// Token: 0x040054B5 RID: 21685
	public bool interactable = true;

	// Token: 0x040054B6 RID: 21686
	public float minLimit;

	// Token: 0x040054B7 RID: 21687
	public float maxLimit = 100f;

	// Token: 0x040054B8 RID: 21688
	public float range = 50f;

	// Token: 0x040054B9 RID: 21689
	public float barWidth = 10f;

	// Token: 0x040054BA RID: 21690
	public float barHeight = 100f;

	// Token: 0x040054BB RID: 21691
	public float currentMinValue = 10f;

	// Token: 0x040054BC RID: 21692
	public float currentMaxValue = 90f;

	// Token: 0x040054BD RID: 21693
	public float currentExtraValue = 50f;

	// Token: 0x040054BE RID: 21694
	public Slider.Direction direction;

	// Token: 0x040054BF RID: 21695
	public bool wholeNumbers = true;

	// Token: 0x040054C0 RID: 21696
	public Action<MinMaxSlider> onMinChange;

	// Token: 0x040054C1 RID: 21697
	public Action<MinMaxSlider> onMaxChange;

	// Token: 0x040054C2 RID: 21698
	public Slider minSlider;

	// Token: 0x040054C3 RID: 21699
	public Slider maxSlider;

	// Token: 0x040054C4 RID: 21700
	public Slider extraSlider;

	// Token: 0x040054C5 RID: 21701
	public RectTransform minRect;

	// Token: 0x040054C6 RID: 21702
	public RectTransform maxRect;

	// Token: 0x040054C7 RID: 21703
	public RectTransform bgFill;

	// Token: 0x040054C8 RID: 21704
	public RectTransform mgFill;

	// Token: 0x040054C9 RID: 21705
	public RectTransform fgFill;

	// Token: 0x040054CA RID: 21706
	public Text title;

	// Token: 0x040054CB RID: 21707
	[MyCmpGet]
	public ToolTip toolTip;

	// Token: 0x040054CC RID: 21708
	public Image icon;

	// Token: 0x040054CD RID: 21709
	public Image isOverPowered;

	// Token: 0x040054CE RID: 21710
	private Vector3 mousePos;

	// Token: 0x02002131 RID: 8497
	public enum LockingType
	{
		// Token: 0x0400988E RID: 39054
		Toggle,
		// Token: 0x0400988F RID: 39055
		Drag
	}

	// Token: 0x02002132 RID: 8498
	public enum Mode
	{
		// Token: 0x04009891 RID: 39057
		Single,
		// Token: 0x04009892 RID: 39058
		Double,
		// Token: 0x04009893 RID: 39059
		Triple
	}
}
