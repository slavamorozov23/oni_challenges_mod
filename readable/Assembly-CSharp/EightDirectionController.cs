using System;
using UnityEngine;

// Token: 0x0200074E RID: 1870
public class EightDirectionController
{
	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06002F4A RID: 12106 RVA: 0x0011110E File Offset: 0x0010F30E
	// (set) Token: 0x06002F4B RID: 12107 RVA: 0x00111116 File Offset: 0x0010F316
	public KBatchedAnimController controller { get; private set; }

	// Token: 0x06002F4C RID: 12108 RVA: 0x0011111F File Offset: 0x0010F31F
	public EightDirectionController(KAnimControllerBase buildingController, string targetSymbol, string defaultAnim, EightDirectionController.Offset frontBank)
	{
		this.Initialize(buildingController, targetSymbol, defaultAnim, frontBank, Grid.SceneLayer.NoLayer);
	}

	// Token: 0x06002F4D RID: 12109 RVA: 0x00111134 File Offset: 0x0010F334
	private void Initialize(KAnimControllerBase buildingController, string targetSymbol, string defaultAnim, EightDirectionController.Offset frontBack, Grid.SceneLayer userSpecifiedRenderLayer)
	{
		string name = buildingController.name + ".eight_direction";
		this.gameObject = new GameObject(name);
		this.gameObject.SetActive(false);
		this.gameObject.transform.parent = buildingController.transform;
		this.gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.defaultAnim = defaultAnim;
		this.controller = this.gameObject.AddOrGet<KBatchedAnimController>();
		this.controller.AnimFiles = new KAnimFile[]
		{
			buildingController.AnimFiles[0]
		};
		this.controller.initialAnim = defaultAnim;
		this.controller.isMovable = true;
		this.controller.sceneLayer = Grid.SceneLayer.NoLayer;
		if (EightDirectionController.Offset.UserSpecified == frontBack)
		{
			this.controller.sceneLayer = userSpecifiedRenderLayer;
		}
		buildingController.SetSymbolVisiblity(targetSymbol, false);
		bool flag;
		Vector3 position = buildingController.GetSymbolTransform(new HashedString(targetSymbol), out flag).GetColumn(3);
		switch (frontBack)
		{
		case EightDirectionController.Offset.Infront:
			position.z = buildingController.transform.GetPosition().z - 0.1f;
			break;
		case EightDirectionController.Offset.Behind:
			position.z = buildingController.transform.GetPosition().z + 0.1f;
			break;
		case EightDirectionController.Offset.UserSpecified:
			position.z = Grid.GetLayerZ(userSpecifiedRenderLayer);
			break;
		}
		this.gameObject.transform.SetPosition(position);
		this.gameObject.SetActive(true);
		this.link = new KAnimLink(buildingController, this.controller);
	}

	// Token: 0x06002F4E RID: 12110 RVA: 0x001112BC File Offset: 0x0010F4BC
	public void SetPositionPercent(float percent_full)
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.SetPositionPercent(percent_full);
	}

	// Token: 0x06002F4F RID: 12111 RVA: 0x001112D9 File Offset: 0x0010F4D9
	public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
	{
		if (this.controller != null)
		{
			this.controller.SetSymbolTint(symbol, colour);
		}
	}

	// Token: 0x06002F50 RID: 12112 RVA: 0x001112FB File Offset: 0x0010F4FB
	public void SetRotation(float rot)
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.Rotation = rot;
	}

	// Token: 0x06002F51 RID: 12113 RVA: 0x00111318 File Offset: 0x0010F518
	public void PlayAnim(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
	{
		this.controller.Play(anim, mode, 1f, 0f);
	}

	// Token: 0x04001C0E RID: 7182
	public GameObject gameObject;

	// Token: 0x04001C0F RID: 7183
	private string defaultAnim;

	// Token: 0x04001C10 RID: 7184
	private KAnimLink link;

	// Token: 0x0200162F RID: 5679
	public enum Offset
	{
		// Token: 0x04007403 RID: 29699
		Infront,
		// Token: 0x04007404 RID: 29700
		Behind,
		// Token: 0x04007405 RID: 29701
		UserSpecified
	}
}
