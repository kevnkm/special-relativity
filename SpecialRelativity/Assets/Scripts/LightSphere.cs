using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(SphereCollider))]
public class LightSphere : MonoBehaviour
{
    [Header("Fresnel Color Settings")]
    public Color baseColor = Color.magenta;

    private Material _materialInstance;
    private Coroutine scaleRoutine;
    private SphereCollider sphereCollider;

    [SerializeField]
    private bool scaleOnStart = false;

    void Start()
    {
        var renderer = GetComponent<Renderer>();
        _materialInstance = renderer.material;
        UpdateBaseColor();

        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = 0.5f;

        transform.localScale = Vector3.one;
        if (scaleOnStart)
            TriggerScale(new Vector3(100, 100, 100), 10f);
    }

    public void SetBaseColor(Color color)
    {
        baseColor = color;
        UpdateBaseColor();
    }

    private void UpdateBaseColor()
    {
        if (_materialInstance != null)
        {
            _materialInstance.SetColor("_BaseColor", baseColor);
        }
    }

    public void TriggerScale(Vector3 targetScale, float duration)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScaleAndFade(targetScale, duration));
    }

    private IEnumerator ScaleAndFade(Vector3 targetScale, float duration)
    {
        Vector3 startScale = Vector3.one;
        Color startColor = baseColor;
        Color endColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);

        float elapsed = 0f;
        transform.localScale = startScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            SetBaseColor(Color.Lerp(startColor, endColor, t));

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        SetBaseColor(endColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SignalLightSphere"))
        {
            Debug.Log($"LightSphere collided with button: {gameObject.name}");
            LightSphere otherLightSphere = other.GetComponent<LightSphere>();
            if (otherLightSphere != null)
            {
                otherLightSphere.TriggerScale(new Vector3(100, 100, 100), 8f);
            }
            else
            {
                Debug.LogWarning($"No LightSphere component found on {other.name}");
            }
        }
    }

    public void Reset()
    {
        transform.localScale = Vector3.one;
        StopAllCoroutines();
        if (_materialInstance != null)
            _materialInstance.SetColor("_BaseColor", baseColor);
    }
}
