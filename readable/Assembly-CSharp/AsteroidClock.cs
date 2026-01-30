using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CA7 RID: 3239
public class AsteroidClock : MonoBehaviour
{
	// Token: 0x06006315 RID: 25365 RVA: 0x0024BA58 File Offset: 0x00249C58
	private void Awake()
	{
		this.UpdateOverlay();
	}

	// Token: 0x06006316 RID: 25366 RVA: 0x0024BA60 File Offset: 0x00249C60
	private void Start()
	{
	}

	// Token: 0x06006317 RID: 25367 RVA: 0x0024BA62 File Offset: 0x00249C62
	private void Update()
	{
		if (GameClock.Instance != null)
		{
			this.rotationTransform.rotation = Quaternion.Euler(0f, 0f, 360f * -GameClock.Instance.GetCurrentCycleAsPercentage());
		}
	}

	// Token: 0x06006318 RID: 25368 RVA: 0x0024BA9C File Offset: 0x00249C9C
	private void UpdateOverlay()
	{
		float fillAmount = 0.125f;
		this.NightOverlay.fillAmount = fillAmount;
	}

	// Token: 0x04004327 RID: 17191
	public Transform rotationTransform;

	// Token: 0x04004328 RID: 17192
	public Image NightOverlay;
}
