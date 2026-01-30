using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000008 RID: 8
[AddComponentMenu("KMonoBehaviour/scripts/UICurvePath")]
public class UICurvePath : KMonoBehaviour
{
	// Token: 0x06000022 RID: 34 RVA: 0x00002800 File Offset: 0x00000A00
	protected override void OnSpawn()
	{
		this.Init();
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		this.OnResize();
		this.startDelay = (float)UnityEngine.Random.Range(0, 8);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002850 File Offset: 0x00000A50
	private void OnResize()
	{
		this.A = this.startPoint.position;
		this.B = this.controlPointStart.position;
		this.C = this.controlPointEnd.position;
		this.D = this.endPoint.position;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000028A1 File Offset: 0x00000AA1
	protected override void OnCleanUp()
	{
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
		base.OnCleanUp();
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000028D0 File Offset: 0x00000AD0
	private void Update()
	{
		this.startDelay -= Time.unscaledDeltaTime;
		this.sprite.gameObject.SetActive(this.startDelay < 0f);
		if (this.startDelay > 0f)
		{
			return;
		}
		this.tick += Time.unscaledDeltaTime * this.moveSpeed;
		this.sprite.transform.position = this.DeCasteljausAlgorithm(this.tick);
		this.sprite.SetAlpha(Mathf.Min(this.sprite.color.a + this.tick / 2f, 1f));
		if (this.animateScale)
		{
			float num = Mathf.Min(this.sprite.transform.localScale.x + Time.unscaledDeltaTime * this.moveSpeed, 1f);
			this.sprite.transform.localScale = new Vector3(num, num, 1f);
		}
		if (this.loop && this.tick > 1f)
		{
			this.Init();
		}
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000029F0 File Offset: 0x00000BF0
	private void Init()
	{
		this.sprite.transform.position = this.startPoint.position;
		this.tick = 0f;
		if (this.animateScale)
		{
			this.sprite.transform.localScale = this.initialScale;
		}
		this.sprite.SetAlpha(this.initialAlpha);
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002A54 File Offset: 0x00000C54
	private void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			this.A = this.startPoint.position;
			this.B = this.controlPointStart.position;
			this.C = this.controlPointEnd.position;
			this.D = this.endPoint.position;
		}
		Gizmos.color = Color.white;
		Vector3 a = this.A;
		float num = 0.02f;
		int num2 = Mathf.FloorToInt(1f / num);
		for (int i = 1; i <= num2; i++)
		{
			float t = (float)i * num;
			this.DeCasteljausAlgorithm(t);
		}
		Gizmos.color = Color.green;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002AF4 File Offset: 0x00000CF4
	private Vector3 DeCasteljausAlgorithm(float t)
	{
		float d = 1f - t;
		Vector3 a = d * this.A + t * this.B;
		Vector3 a2 = d * this.B + t * this.C;
		Vector3 a3 = d * this.C + t * this.D;
		Vector3 a4 = d * a + t * a2;
		Vector3 a5 = d * a2 + t * a3;
		return d * a4 + t * a5;
	}

	// Token: 0x0400001E RID: 30
	public Transform startPoint;

	// Token: 0x0400001F RID: 31
	public Transform endPoint;

	// Token: 0x04000020 RID: 32
	public Transform controlPointStart;

	// Token: 0x04000021 RID: 33
	public Transform controlPointEnd;

	// Token: 0x04000022 RID: 34
	public Image sprite;

	// Token: 0x04000023 RID: 35
	public bool loop = true;

	// Token: 0x04000024 RID: 36
	public bool animateScale;

	// Token: 0x04000025 RID: 37
	public Vector3 initialScale;

	// Token: 0x04000026 RID: 38
	private float startDelay;

	// Token: 0x04000027 RID: 39
	public float initialAlpha = 0.5f;

	// Token: 0x04000028 RID: 40
	public float moveSpeed = 0.1f;

	// Token: 0x04000029 RID: 41
	private float tick;

	// Token: 0x0400002A RID: 42
	private Vector3 A;

	// Token: 0x0400002B RID: 43
	private Vector3 B;

	// Token: 0x0400002C RID: 44
	private Vector3 C;

	// Token: 0x0400002D RID: 45
	private Vector3 D;
}
