using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotoReconstructionController : MonoBehaviour, IPointerUpHandler
{
    [SerializeField]
    private Renderer photoDisplay;

    [SerializeField]
    private float scanDuration = 0.8f;

    private Slider reconstructionSlider;
    private Material photoMaterial;
    private float displayedValue;
    private Coroutine scanCoroutine;

    private void Awake()
    {
        reconstructionSlider = GetComponent<Slider>();
        photoMaterial = photoDisplay.material;
        displayedValue = reconstructionSlider.value;

        photoMaterial.SetFloat("_PreviousReconstruction", displayedValue);
        photoMaterial.SetFloat("_Reconstruction", displayedValue);
        photoMaterial.SetFloat("_ScanProgress", 1f);
        photoMaterial.SetFloat("_ScanActive", 0f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (scanCoroutine != null)
        {
            StopCoroutine(scanCoroutine);
        }

        scanCoroutine = StartCoroutine(
            ScanToValue(reconstructionSlider.value)
        );
    }

    private IEnumerator ScanToValue(float targetValue)
    {
        reconstructionSlider.interactable = false;

        photoMaterial.SetFloat(
            "_PreviousReconstruction",
            displayedValue
        );
        photoMaterial.SetFloat("_Reconstruction", targetValue);
        photoMaterial.SetFloat("_ScanActive", 1f);

        float elapsedTime = 0f;

        while (elapsedTime < scanDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / scanDuration
            );

            photoMaterial.SetFloat("_ScanProgress", progress);

            yield return null;
        }

        displayedValue = targetValue;

        photoMaterial.SetFloat(
            "_PreviousReconstruction",
            displayedValue
        );
        photoMaterial.SetFloat("_Reconstruction", displayedValue);
        photoMaterial.SetFloat("_ScanProgress", 1f);
        photoMaterial.SetFloat("_ScanActive", 0f);

        reconstructionSlider.interactable = true;
        scanCoroutine = null;
    }
}
