using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimationText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleFactor = 1f;
    public float speed = 1f;

    private Vector3 initialScale;
    [SerializeField] private GameObject HighlightText1;
    [SerializeField] private GameObject HighlightText2;

    private void Start()
    {
        SetOneTextTrue(true, false);
        initialScale = transform.localScale;
    }

    private void Update()
    {
        float scale = Mathf.Sin(Time.time * speed) * scaleFactor;
        transform.localScale = initialScale + new Vector3(scale, scale, scale);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetOneTextTrue(false, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetOneTextTrue(true, false);
    }

    private void SetOneTextTrue(bool _one, bool _two)
    {
        HighlightText1.SetActive(_one);
        HighlightText2.SetActive(_two);
    }
}
