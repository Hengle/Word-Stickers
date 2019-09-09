using UnityEngine;
using UnityEngine.UI;

namespace GaussianBlur_LiveBlur
{
	[RequireComponent(typeof(Image))]
	[ExecuteInEditMode]
	public class SyncCoordinates : MonoBehaviour
	{
		private Material thisMaterial;

		private Image thisImage;

		private Rect thisRect;

		public bool createNewInstance;

		private float _ScreenHeight;

		private float _ScreenWidth;

		private float _PanelY;

		private float _PanelX;

		private float _PanelHeight;

		private float _PanelWidth;

		public float ScreenHeight
		{
			get
			{
				return _ScreenHeight;
			}
			set
			{
				if (_ScreenHeight != value)
				{
					thisMaterial.SetFloat("_ScreenHeight", value);
					_ScreenHeight = value;
				}
			}
		}

		public float ScreenWidth
		{
			get
			{
				return _ScreenWidth;
			}
			set
			{
				if (_ScreenWidth != value)
				{
					thisMaterial.SetFloat("_ScreenWidth", value);
					_ScreenWidth = value;
				}
			}
		}

		public float PanelY
		{
			get
			{
				return _PanelY;
			}
			set
			{
				if (_PanelY != value)
				{
					thisMaterial.SetFloat("_PanelY", value);
					_PanelY = value;
				}
			}
		}

		public float PanelX
		{
			get
			{
				return _PanelX;
			}
			set
			{
				if (_PanelX != value)
				{
					thisMaterial.SetFloat("_PanelX", value);
					_PanelX = value;
				}
			}
		}

		public float PanelHeight
		{
			get
			{
				return _PanelHeight;
			}
			set
			{
				if (_PanelHeight != value)
				{
					thisMaterial.SetFloat("_PanelHeight", value);
					_PanelHeight = value;
				}
			}
		}

		public float PanelWidth
		{
			get
			{
				return _PanelWidth;
			}
			set
			{
				if (_PanelWidth != value)
				{
					thisMaterial.SetFloat("_PanelWidth", value);
					_PanelWidth = value;
				}
			}
		}

		private void Awake()
		{
			RectTransform rectTransform = (RectTransform)base.transform;
			thisRect = rectTransform.rect;
			thisImage = GetComponent<Image>();
			if (!createNewInstance)
			{
				thisMaterial = thisImage.material;
				return;
			}
			thisMaterial = UnityEngine.Object.Instantiate(thisImage.material);
			thisImage.material = thisMaterial;
		}

		private void Update()
		{
		}

		private void FixedUpdate()
		{
			RectTransform rectTransform = (RectTransform)base.transform;
			thisRect = rectTransform.rect;
			ScreenHeight = Screen.height;
			ScreenWidth = Screen.width;
			PanelY = base.transform.position.y;
			PanelX = base.transform.position.x;
			PanelHeight = thisRect.height * base.transform.lossyScale.y;
			PanelWidth = thisRect.width * base.transform.lossyScale.x;
		}
	}
}
