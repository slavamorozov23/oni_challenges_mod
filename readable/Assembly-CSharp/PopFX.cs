using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DDD RID: 3549
[AddComponentMenu("KMonoBehaviour/scripts/PopFX")]
public class PopFX : KMonoBehaviour
{
	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x06006F8B RID: 28555 RVA: 0x002A5CB6 File Offset: 0x002A3EB6
	public Vector3 StartPos
	{
		get
		{
			return this.startPos;
		}
	}

	// Token: 0x06006F8C RID: 28556 RVA: 0x002A5CC0 File Offset: 0x002A3EC0
	public void Recycle()
	{
		this.icon = null;
		this.mainIcon = null;
		this.text = "";
		this.targetTransform = null;
		this.lifeElapsed = 0f;
		this.trackTarget = false;
		this.startPos = Vector3.zero;
		this.positionToGroup = true;
		this.canvasPaddingMultiplier = Vector3.zero;
		this.IconDisplay.color = Color.white;
		this.TextDisplay.color = Color.white;
		this.MainIconDisplay.color = Color.white;
		PopFXManager.Instance.RecycleFX(this);
		this.canvasGroup.alpha = 0f;
		this.IconDisplay.gameObject.SetActive(false);
		base.gameObject.SetActive(false);
		this.isLive = false;
		this.isActiveWorld = false;
		Game.Instance.Unsubscribe(ref this.eventid);
	}

	// Token: 0x06006F8D RID: 28557 RVA: 0x002A5DA2 File Offset: 0x002A3FA2
	public void SetIconTint(Color color)
	{
		this.MainIconDisplay.color = color;
	}

	// Token: 0x06006F8E RID: 28558 RVA: 0x002A5DB0 File Offset: 0x002A3FB0
	public void Run(Vector3 groupSpawnPosition, Vector3 canvasPaddingMultiplier)
	{
		base.gameObject.SetActive(true);
		this.canvasPaddingMultiplier = canvasPaddingMultiplier;
		if (this.positionToGroup && groupSpawnPosition != PopFxGroup.INVALID_SPAWN_POSITION)
		{
			this.startPos = groupSpawnPosition;
		}
		if (this.trackTarget && this.targetTransform != null)
		{
			this.startPos = this.targetTransform.GetPosition();
			int num;
			int num2;
			Grid.PosToXY(this.startPos, out num, out num2);
			this.startPos.x = this.startPos.x - 0.5f;
		}
		this.TextDisplay.text = this.text;
		this.IconDisplay.sprite = this.icon;
		this.IconDisplay.Opacity(1f);
		this.MainIconDisplay.Opacity(1f);
		this.MainIconDisplay.sprite = this.mainIcon;
		this.IconDisplay.gameObject.SetActive(this.icon != null);
		this.canvasGroup.alpha = 1f;
		this.isLive = true;
		this.eventid = Game.Instance.Subscribe(1983128072, PopFX.OnActiveWorldChangedDispatcher, this);
		this.SetWorldActive(ClusterManager.Instance.activeWorldId);
		this.Update();
	}

	// Token: 0x06006F8F RID: 28559 RVA: 0x002A5EF0 File Offset: 0x002A40F0
	public void Setup(Sprite MainIcon, Sprite SecondaryIcon, string Text, Transform TargetTransform, Vector3 Offset, bool PositionToGroup, float LifeTime = 1.5f, bool TrackTarget = false)
	{
		this.mainIcon = MainIcon;
		this.icon = SecondaryIcon;
		this.text = Text;
		this.targetTransform = TargetTransform;
		this.trackTarget = TrackTarget;
		this.lifetime = LifeTime;
		this.offset = Offset;
		this.positionToGroup = PositionToGroup;
		if (this.targetTransform != null)
		{
			this.startPos = this.targetTransform.GetPosition();
		}
		int num;
		int num2;
		Grid.PosToXY(this.startPos, out num, out num2);
		this.startPos.x = this.startPos.x - 0.5f;
	}

	// Token: 0x06006F90 RID: 28560 RVA: 0x002A5F7C File Offset: 0x002A417C
	private void OnActiveWorldChanged(object data)
	{
		global::Tuple<int, int> tuple = (global::Tuple<int, int>)data;
		if (this.isLive)
		{
			this.SetWorldActive(tuple.first);
		}
	}

	// Token: 0x06006F91 RID: 28561 RVA: 0x002A5FA4 File Offset: 0x002A41A4
	private void SetWorldActive(int worldId)
	{
		int num = Grid.PosToCell((this.trackTarget && this.targetTransform != null) ? this.targetTransform.position : (this.startPos + this.offset));
		this.isActiveWorld = (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] == worldId);
	}

	// Token: 0x06006F92 RID: 28562 RVA: 0x002A6008 File Offset: 0x002A4208
	private void Update()
	{
		if (!this.isLive)
		{
			return;
		}
		if (!PopFXManager.Instance.Ready())
		{
			return;
		}
		this.lifeElapsed += Time.unscaledDeltaTime;
		if (this.lifeElapsed >= this.lifetime)
		{
			this.Recycle();
		}
		if (this.trackTarget && this.targetTransform != null)
		{
			Vector3 v = PopFXManager.Instance.WorldToScreen(this.targetTransform.GetPosition() + this.offset + Vector3.up * this.lifeElapsed * (2f * this.lifeElapsed));
			v.z = 0f;
			base.gameObject.rectTransform().anchoredPosition = v;
		}
		else
		{
			Vector3 vector = PopFXManager.Instance.WorldToScreen(this.startPos + this.offset + Vector3.up * this.lifeElapsed * (2f * (this.lifeElapsed / 2f)));
			vector.z = 0f;
			Vector3 b = this.Pivot.rect.size;
			b.x *= this.canvasPaddingMultiplier.x;
			b.y *= this.canvasPaddingMultiplier.y;
			b.z *= this.canvasPaddingMultiplier.z;
			vector += b;
			base.gameObject.rectTransform().anchoredPosition = vector;
		}
		float num = CameraController.Instance.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : 20f;
		float t = (CameraController.Instance.OrthographicSize - CameraController.Instance.minOrthographicSize) / (num - CameraController.Instance.minOrthographicSize);
		base.gameObject.rectTransform().localScale = Vector3.one * Mathf.Lerp(1f, 0.7f, t);
		float num2 = Mathf.Clamp01((this.lifetime - this.lifeElapsed) / this.lifetime);
		float t2 = Mathf.Clamp01((1f - num2) / 0.1f);
		float num3 = Mathf.Clamp01(num2 / 0.2f);
		this.mask.fillAmount = Mathf.Lerp(0.16f * num3, 1f, t2);
		this.canvasGroup.alpha = (this.isActiveWorld ? num3 : 0f);
	}

	// Token: 0x04004C51 RID: 19537
	public const float Speed = 2f;

	// Token: 0x04004C52 RID: 19538
	private Sprite mainIcon;

	// Token: 0x04004C53 RID: 19539
	private Sprite icon;

	// Token: 0x04004C54 RID: 19540
	private string text;

	// Token: 0x04004C55 RID: 19541
	private Transform targetTransform;

	// Token: 0x04004C56 RID: 19542
	private Vector3 offset;

	// Token: 0x04004C57 RID: 19543
	private Vector3 canvasPaddingMultiplier;

	// Token: 0x04004C58 RID: 19544
	public RectTransform Pivot;

	// Token: 0x04004C59 RID: 19545
	public Image bg;

	// Token: 0x04004C5A RID: 19546
	public Image MainIconDisplay;

	// Token: 0x04004C5B RID: 19547
	public Image IconDisplay;

	// Token: 0x04004C5C RID: 19548
	public Image mask;

	// Token: 0x04004C5D RID: 19549
	public LocText TextDisplay;

	// Token: 0x04004C5E RID: 19550
	public CanvasGroup canvasGroup;

	// Token: 0x04004C5F RID: 19551
	private Camera uiCamera;

	// Token: 0x04004C60 RID: 19552
	private float lifetime;

	// Token: 0x04004C61 RID: 19553
	private float lifeElapsed;

	// Token: 0x04004C62 RID: 19554
	private bool trackTarget;

	// Token: 0x04004C63 RID: 19555
	private bool positionToGroup = true;

	// Token: 0x04004C64 RID: 19556
	private Vector3 startPos;

	// Token: 0x04004C65 RID: 19557
	private bool isLive;

	// Token: 0x04004C66 RID: 19558
	private bool isActiveWorld;

	// Token: 0x04004C67 RID: 19559
	private int eventid = -1;

	// Token: 0x04004C68 RID: 19560
	private static Action<object, object> OnActiveWorldChangedDispatcher = delegate(object context, object data)
	{
		Unsafe.As<PopFX>(context).OnActiveWorldChanged(data);
	};
}
