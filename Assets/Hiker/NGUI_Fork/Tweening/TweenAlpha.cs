//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2019 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the object's alpha. Works with both UI widgets as well as renderers.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
	[Range(0f, 1f)] public float from = 1f;
	[Range(0f, 1f)] public float to = 1f;

	bool mCached = false;
    CanvasGroup mCanvasGrp;
	Graphic mGraphic;
	Material mMat;
	Light mLight;
	SpriteRenderer mSr;
	float mBaseIntensity = 1f;

	[System.Obsolete("Use 'value' instead")]
	public float alpha { get { return this.value; } set { this.value = value; } }

	void Cache ()
	{
		mCached = true;
        mGraphic = GetComponent<Graphic>();
		mSr = GetComponent<SpriteRenderer>();
        mCanvasGrp = GetComponent<CanvasGroup>();

        if (mGraphic == null && mSr == null)
		{
			mLight = GetComponent<Light>();

			if (mLight == null)
			{
				Renderer ren = GetComponent<Renderer>();
				if (ren != null) mMat = ren.material;
				if (mMat == null) mGraphic = GetComponentInChildren<Graphic>();
			}
			else mBaseIntensity = mLight.intensity;
		}
	}

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public float value
	{
		get
		{
			if (!mCached) Cache();
            if (mCanvasGrp != null) return mCanvasGrp.alpha;
			if (mGraphic != null) return mGraphic.color.a;
			if (mSr != null) return mSr.color.a;
			return mMat != null ? mMat.color.a : 1f;
		}
		set
		{
			if (!mCached) Cache();

            if (mCanvasGrp != null)
            {
                mCanvasGrp.alpha = value;
            }
            else
			if (mGraphic != null)
			{
                mGraphic.color = new Color(mGraphic.color.r, mGraphic.color.g, mGraphic.color.b, value);
			}
			else if (mSr != null)
			{
				Color c = mSr.color;
				c.a = value;
				mSr.color = c;
			}
			else if (mMat != null)
			{
				Color c = mMat.color;
				c.a = value;
				mMat.color = c;
			}
			else if (mLight != null)
			{
				mLight.intensity = mBaseIntensity * value;
			}
		}
	}

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenAlpha Begin (GameObject go, float duration, float alpha, float delay = 0f)
	{
		TweenAlpha comp = UITweener.Begin<TweenAlpha>(go, duration, delay);
		comp.from = comp.value;
		comp.to = alpha;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	public override void SetStartToCurrentValue () { from = value; }
	public override void SetEndToCurrentValue () { to = value; }
}
