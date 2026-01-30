using System;
using UnityEngine;

// Token: 0x02000AB2 RID: 2738
public class FogOfWarPostFX : MonoBehaviour
{
	// Token: 0x06004F79 RID: 20345 RVA: 0x001CD794 File Offset: 0x001CB994
	private void Awake()
	{
		if (this.shader != null)
		{
			this.material = new Material(this.shader);
		}
	}

	// Token: 0x06004F7A RID: 20346 RVA: 0x001CD7B5 File Offset: 0x001CB9B5
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.SetupUVs();
		Graphics.Blit(source, destination, this.material, 0);
	}

	// Token: 0x06004F7B RID: 20347 RVA: 0x001CD7CC File Offset: 0x001CB9CC
	private void SetupUVs()
	{
		if (this.myCamera == null)
		{
			this.myCamera = base.GetComponent<Camera>();
			if (this.myCamera == null)
			{
				return;
			}
		}
		Ray ray = this.myCamera.ViewportPointToRay(Vector3.zero);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		Vector3 point = ray.GetPoint(distance);
		Vector4 vector;
		vector.x = point.x / Grid.WidthInMeters;
		vector.y = point.y / Grid.HeightInMeters;
		ray = this.myCamera.ViewportPointToRay(Vector3.one);
		distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		point = ray.GetPoint(distance);
		vector.z = point.x / Grid.WidthInMeters - vector.x;
		vector.w = point.y / Grid.HeightInMeters - vector.y;
		this.material.SetVector("_UVOffsetScale", vector);
	}

	// Token: 0x0400351F RID: 13599
	[SerializeField]
	private Shader shader;

	// Token: 0x04003520 RID: 13600
	private Material material;

	// Token: 0x04003521 RID: 13601
	private Camera myCamera;
}
