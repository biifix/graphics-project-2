using UnityEngine;
using UnityEngine.InputSystem;

public class DissolveController : MonoBehaviour
{
    private Material dissolveMaterial;
    private float dissolveAmount;
    private bool isDissolving;

    private static readonly int DissolveAmount =
        Shader.PropertyToID("_DissolveAmount");

    private static readonly int DissolveSpeed =
        Shader.PropertyToID("_DissolveSpeed");

    private void Awake()
    {
        Renderer objectRenderer = GetComponent<Renderer>();
        dissolveMaterial = objectRenderer.material;

        dissolveAmount = 0f;
        dissolveMaterial.SetFloat(DissolveAmount, dissolveAmount);
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isDissolving = !isDissolving;
        }

        float targetAmount = isDissolving ? 1f : 0f;
        float speed = dissolveMaterial.GetFloat(DissolveSpeed);

        dissolveAmount = Mathf.MoveTowards(
            dissolveAmount,
            targetAmount,
            Time.deltaTime * speed
        );

        dissolveMaterial.SetFloat(
            DissolveAmount,
            dissolveAmount
        );
    }

    public void BeginDissolve()
    {
        isDissolving = true;
    }
}