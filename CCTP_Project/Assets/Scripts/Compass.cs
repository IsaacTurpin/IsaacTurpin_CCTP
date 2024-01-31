using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] RawImage compassImage;
    [SerializeField] GameObject iconPrefab;
    List<CompassMarker> compassMarkers = new List<CompassMarker>();

    float compassUnit;
    public float maxDistance = 150f;

    public CompassMarker one;

    void Start()
    {
        compassUnit = compassImage.rectTransform.rect.width / 360f;

        AddCompassMarker(one);
    }

    void Update()
    {
        UpdateCompass();
    }

    public void AddCompassMarker(CompassMarker marker)
    {
        GameObject newMarker = Instantiate(iconPrefab, compassImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;

        compassMarkers.Add(marker);
        
    }

    Vector2 GetPositionOnCompass(CompassMarker marker)
    {
        Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 playerForward = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.SignedAngle(marker.position - playerPosition, playerForward);

        return new Vector2(compassUnit * angle, 0f);
    }

    void UpdateCompass()
    {
        compassImage.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);

        foreach (CompassMarker marker in compassMarkers)
        {
            marker.image.rectTransform.anchoredPosition = GetPositionOnCompass(marker);

            float distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), marker.position);
            float scale = 0f;

            if (distance < maxDistance)
            {
                scale = 1f - (distance / maxDistance);
            }

            marker.image.rectTransform.localScale = Vector3.one * scale;
        }
    }
}
