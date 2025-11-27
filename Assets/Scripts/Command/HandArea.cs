using UnityEngine;

public class HandArea : CardArea
{
    [SerializeField] private float arcHeight = 50f;
    [SerializeField] private float maxRotation = 15f;

    public override void UpdateLayout()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] != null)
            {
                Vector3 position = GetArcPosition(i, cards.Count);
                Quaternion rotation = GetArcRotation(i, cards.Count);

                cards[i].transform.localPosition = position;
                cards[i].transform.localRotation = rotation;
                cards[i].transform.localScale = Vector3.one * 0.8f;
            }
        }
    }

    private Vector3 GetArcPosition(int index, int total)
    {
        if (total <= 1) return Vector3.zero;

        float normalized = (float)index / (total - 1);
        float angle = Mathf.Lerp(-60f, 60f, normalized) * Mathf.Deg2Rad;

        return new Vector3(
            Mathf.Sin(angle) * 200f,
            Mathf.Cos(angle) * arcHeight - arcHeight,
            -index * 0.1f
        );
    }

    private Quaternion GetArcRotation(int index, int total)
    {
        if (total <= 1) return Quaternion.identity;

        float rotation = Mathf.Lerp(-maxRotation, maxRotation, (float)index / (total - 1));
        return Quaternion.Euler(0, 0, rotation);
    }
}