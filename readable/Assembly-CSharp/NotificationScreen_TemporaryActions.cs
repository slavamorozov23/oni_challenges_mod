using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000DC0 RID: 3520
public class NotificationScreen_TemporaryActions : KMonoBehaviour
{
	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x06006DFC RID: 28156 RVA: 0x0029B075 File Offset: 0x00299275
	// (set) Token: 0x06006DFD RID: 28157 RVA: 0x0029B07C File Offset: 0x0029927C
	public static NotificationScreen_TemporaryActions Instance { get; private set; }

	// Token: 0x06006DFE RID: 28158 RVA: 0x0029B084 File Offset: 0x00299284
	public static void DestroyInstance()
	{
		NotificationScreen_TemporaryActions.Instance = null;
	}

	// Token: 0x06006DFF RID: 28159 RVA: 0x0029B08C File Offset: 0x0029928C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NotificationScreen_TemporaryActions.Instance = this;
		this.originalRow.gameObject.SetActive(false);
	}

	// Token: 0x06006E00 RID: 28160 RVA: 0x0029B0AC File Offset: 0x002992AC
	private TemporaryActionRow CreateActionRow()
	{
		TemporaryActionRow temporaryActionRow = Util.KInstantiateUI<TemporaryActionRow>(this.originalRow.gameObject, this.originalRow.transform.parent.gameObject, false);
		temporaryActionRow.gameObject.SetActive(true);
		temporaryActionRow.transform.SetAsLastSibling();
		this.rows.Add(temporaryActionRow);
		return temporaryActionRow;
	}

	// Token: 0x06006E01 RID: 28161 RVA: 0x0029B104 File Offset: 0x00299304
	private void RemoveRow(TemporaryActionRow row)
	{
		if (this.rows.Contains(row))
		{
			this.rows.Remove(row);
		}
		row.OnRowHidden = null;
		row.gameObject.DeleteObject();
	}

	// Token: 0x06006E02 RID: 28162 RVA: 0x0029B134 File Offset: 0x00299334
	protected override void OnCleanUp()
	{
		if (this.rows != null)
		{
			foreach (TemporaryActionRow temporaryActionRow in this.rows.ToArray())
			{
				if (temporaryActionRow != null)
				{
					this.RemoveRow(temporaryActionRow);
				}
			}
			this.rows.Clear();
		}
		base.OnCleanUp();
	}

	// Token: 0x06006E03 RID: 28163 RVA: 0x0029B188 File Offset: 0x00299388
	public void CreateCameraReturnActionButton(Vector3 positionToReturnTo)
	{
		if (this.cameraReturnRow == null)
		{
			this.cameraReturnRow = this.CreateActionRow();
			this.cameraReturnRow.Setup(UI.TEMPORARY_ACTIONS.CAMERA_RETURN.NAME, UI.TEMPORARY_ACTIONS.CAMERA_RETURN.TOOLTIP, Assets.GetSprite("action_follow_cam"));
			this.cameraReturnRow.gameObject.name = "TemporaryActionRow_CameraReturn";
			this.cameraPositionToReturnTo = positionToReturnTo;
			this.cameraReturnRow.OnRowHidden = new Action<TemporaryActionRow>(this.RemoveRow);
			this.cameraReturnRow.OnRowClicked = new Action<TemporaryActionRow>(this.OnCameraReturnActionButtonClicked);
		}
		this.cameraReturnRow.SetLifetime(10f);
	}

	// Token: 0x06006E04 RID: 28164 RVA: 0x0029B23A File Offset: 0x0029943A
	private void OnCameraReturnActionButtonClicked(TemporaryActionRow row)
	{
		if (this.cameraPositionToReturnTo != Vector3.zero)
		{
			GameUtil.FocusCamera(this.cameraPositionToReturnTo, 2f, true, false);
		}
	}

	// Token: 0x04004B30 RID: 19248
	public TemporaryActionRow originalRow;

	// Token: 0x04004B31 RID: 19249
	private List<TemporaryActionRow> rows = new List<TemporaryActionRow>();

	// Token: 0x04004B32 RID: 19250
	private TemporaryActionRow cameraReturnRow;

	// Token: 0x04004B33 RID: 19251
	private Vector3 cameraPositionToReturnTo = Vector3.zero;

	// Token: 0x04004B34 RID: 19252
	private const float CAMERA_RETURN_BUTTON_LIFETIME = 10f;
}
