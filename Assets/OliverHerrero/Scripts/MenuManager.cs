using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(XRSimpleInteractable))]
    public sealed class MenuManager : MonoBehaviour
    {
        [SerializeField] private Color touchedColor = Color.green;

        [SerializeField] private bool restoreWhenHandLeaves = true;

        [SerializeField] private Renderer targetRenderer;

        private XRSimpleInteractable interactable;
        private MaterialPropertyBlock propertyBlock;
        private Color initialColor = Color.white;
        private int activeHoverCount;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");

        [SerializeField] private Canvas canvas;
        [SerializeField] private TextMeshProUGUI level1Text;
        [SerializeField] private TextMeshProUGUI level2Text;
        private void Awake()
        {
            interactable = GetComponent<XRSimpleInteractable>();

            if (targetRenderer == null)
            {
                targetRenderer = GetComponent<Renderer>();
            }

            propertyBlock = new MaterialPropertyBlock();
            initialColor = ReadInitialColor();

        }

        void OnEnable()
        {
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        void OnSceneUnloaded(Scene current)
        {
            if (current == SceneManager.GetActiveScene())
            {
                LoaderUtility.Deinitialize();
                LoaderUtility.Initialize();
            }
        }
        void OnDisable()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            activeHoverCount++;
            SetColor(touchedColor);
            canvas.gameObject.SetActive(true);
            if (this.CompareTag("Level1"))
            {
                level1Text.gameObject.SetActive(true);
                level2Text.gameObject.SetActive(false);
            }
            else if (this.CompareTag("Level2"))
            {
                level2Text.gameObject.SetActive(true);
                level1Text.gameObject.SetActive(false);
            }
        }

        public void ChangeLevel1()
        {
            SceneManager.LoadScene("Level1");
        }
        public void ChangeLevel2()
        {
            SceneManager.LoadScene("Level2");
        }

        private void OnHoverExited(HoverExitEventArgs args)
        {
            activeHoverCount = Mathf.Max(0, activeHoverCount - 1);

            if (restoreWhenHandLeaves && activeHoverCount == 0)
                SetColor(initialColor);
            canvas.gameObject.SetActive(false);
            level1Text.gameObject.SetActive(false);
            level2Text.gameObject.SetActive(false);

        }

        private Color ReadInitialColor()
        {
            if (targetRenderer == null || targetRenderer.sharedMaterial == null)
                return Color.white;

            Material material = targetRenderer.sharedMaterial;

            if (material.HasProperty(BaseColorId))
                return material.GetColor(BaseColorId);

            if (material.HasProperty(ColorId))
                return material.GetColor(ColorId);

            return Color.white;
        }

        private void SetColor(Color color)
        {
            if (targetRenderer == null)
                return;

            targetRenderer.GetPropertyBlock(propertyBlock);

            // URP/Lit suele usar _BaseColor.
            propertyBlock.SetColor(BaseColorId, color);

            // Shaders legacy suelen usar _Color.
            propertyBlock.SetColor(ColorId, color);

            targetRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}

