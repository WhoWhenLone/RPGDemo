//--------------------------------------------
//            NGUI: HUD Text
// Copyright © 2012 Tasharen Entertainment
//--------------------------------------------

using UnityEngine;

/// <summary>
/// Attaching this script to an object will make it visibly follow another object, even if the two are using different cameras to draw them.
/// </summary>

public class UIFollowTarget : MonoBehaviour
{
	public delegate void OnVisibilityChange (bool isVisible);

	/// <summary>
	/// Callback triggered every time the object becomes visible or invisible.
	/// </summary>

	public OnVisibilityChange onChange;

	/// <summary>
	/// 3D target that this object will be positioned above.
	/// </summary>

	public Transform target;

	/// <summary>
	/// Game camera to use.
	/// </summary>

	public Camera gameCamera;

	/// <summary>
	/// UI camera to use.
	/// </summary>

	public Camera uiCamera;

	/// <summary>
	/// Whether the children will be disabled when this object is no longer visible.
	/// </summary>

	public bool disableIfInvisible = true;

	/// <summary>
	/// Destroy the game object when target disappears.
	/// </summary>

	public bool destroyWithTarget = true;

	Transform mTrans;
	int mIsVisible = -1;

	/// <summary>
	/// Whether the target is currently visible or not.
	/// </summary>

	public bool isVisible { get { return mIsVisible == 1; } }

	/// <summary>
	/// Cache the transform;
	/// </summary>

	void Awake () { mTrans = transform; }

	/// <summary>
	/// Find both the UI camera and the game camera so they can be used for the position calculations
	/// </summary>

	void Start()
	{
		if (target)
		{
			if (gameCamera == null) gameCamera = NGUITools.FindCameraForLayer(target.gameObject.layer);
			if (uiCamera == null) uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
			Update();
		}
		else
		{
			if (destroyWithTarget) Destroy(gameObject);
			else enabled = false;
		}
	}

	/// <summary>
	/// Update the position of the HUD object every frame such that is position correctly over top of its real world object.
	/// </summary>

	void Update ()
	{
		if (target && uiCamera != null)
		{
			Vector3 pos = gameCamera.WorldToViewportPoint(target.position);

			// Determine the visibility and the target alpha
			int isVisible = (gameCamera.orthographic || pos.z > 0f) && (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f) ? 1 : 0;
			bool vis = (isVisible == 1);

			// If visible, update the position
			if (vis)
			{
				if(mTrans.parent != null)
				{
					pos = uiCamera.ViewportToWorldPoint(pos);
					pos = mTrans.parent.InverseTransformPoint(pos);
					//pos.x = Mathf.RoundToInt(pos.x);
					//pos.y = Mathf.RoundToInt(pos.y);
					pos.z = 0f;
					mTrans.localPosition = pos;
				}
				
			}

			// Update the visibility flag
			if (mIsVisible != isVisible)
			{
				mIsVisible = isVisible;

				if (disableIfInvisible)
				{
					for (int i = 0, imax = mTrans.childCount; i < imax; ++i)
						NGUITools.SetActive(mTrans.GetChild(i).gameObject, vis);
				}

				// Inform the listener
				if (onChange != null) onChange(vis);
			}
		}
		else Destroy(gameObject);
	}
}
