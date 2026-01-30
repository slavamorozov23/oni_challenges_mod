using System;
using UnityEngine;

// Token: 0x02000543 RID: 1347
public class BatchAnimCamera : MonoBehaviour
{
	// Token: 0x06001D30 RID: 7472 RVA: 0x0009ECA6 File Offset: 0x0009CEA6
	private void Awake()
	{
		this.cam = base.GetComponent<Camera>();
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x0009ECB4 File Offset: 0x0009CEB4
	private void Update()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.right * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.left * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.up * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.down * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		this.ClampToBounds();
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.do_pan = true;
				this.last_pan = KInputManager.GetMousePos();
			}
			else if (Input.GetMouseButton(0) && this.do_pan)
			{
				Vector3 vector = this.cam.ScreenToViewportPoint(this.last_pan - KInputManager.GetMousePos());
				Vector3 translation = new Vector3(vector.x * BatchAnimCamera.pan_speed, vector.y * BatchAnimCamera.pan_speed, 0f);
				base.transform.Translate(translation, Space.World);
				this.ClampToBounds();
				this.last_pan = KInputManager.GetMousePos();
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.do_pan = false;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis != 0f)
		{
			this.cam.fieldOfView = Mathf.Clamp(this.cam.fieldOfView - axis * BatchAnimCamera.zoom_speed, this.zoom_min, this.zoom_max);
		}
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x0009EEB8 File Offset: 0x0009D0B8
	private void ClampToBounds()
	{
		Vector3 position = base.transform.GetPosition();
		position.x = Mathf.Clamp(base.transform.GetPosition().x, BatchAnimCamera.bounds.min.x, BatchAnimCamera.bounds.max.x);
		position.y = Mathf.Clamp(base.transform.GetPosition().y, BatchAnimCamera.bounds.min.y, BatchAnimCamera.bounds.max.y);
		position.z = Mathf.Clamp(base.transform.GetPosition().z, BatchAnimCamera.bounds.min.z, BatchAnimCamera.bounds.max.z);
		base.transform.SetPosition(position);
	}

	// Token: 0x06001D33 RID: 7475 RVA: 0x0009EF8C File Offset: 0x0009D18C
	private void OnDrawGizmosSelected()
	{
		DebugExtension.DebugBounds(BatchAnimCamera.bounds, Color.red, 0f, true);
	}

	// Token: 0x04001120 RID: 4384
	private static readonly float pan_speed = 5f;

	// Token: 0x04001121 RID: 4385
	private static readonly float zoom_speed = 5f;

	// Token: 0x04001122 RID: 4386
	public static Bounds bounds = new Bounds(new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f));

	// Token: 0x04001123 RID: 4387
	private float zoom_min = 1f;

	// Token: 0x04001124 RID: 4388
	private float zoom_max = 100f;

	// Token: 0x04001125 RID: 4389
	private Camera cam;

	// Token: 0x04001126 RID: 4390
	private bool do_pan;

	// Token: 0x04001127 RID: 4391
	private Vector3 last_pan;
}
