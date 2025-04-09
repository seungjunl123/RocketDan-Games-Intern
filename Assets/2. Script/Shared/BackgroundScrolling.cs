using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float interval;
    public float speed = 1f;

    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private int firstIndex = 1;

    private void Awake()
    {
        // 원본 이미지를 하나 더 복제한다.
        var newSpriteRenderer = Instantiate<SpriteRenderer>(spriteRenderer);
        newSpriteRenderer.transform.SetParent(this.transform);
        spriteRenderers.Add(spriteRenderer);
        spriteRenderers.Add(newSpriteRenderer);
        SortImage();
    }


    private void SortImage()
    {
        for (int i = spriteRenderers.Count - 1; i >= 0; i--)
        {
            var spriteRenderer = spriteRenderers[i];
            spriteRenderer.transform.localPosition = Vector3.right * interval * i;
        }
    }

    private void Update()
    {
        UpdateMoveImages();
    }

    private void UpdateMoveImages()
    {
        float move = Time.deltaTime * speed;

        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            var spriteRenderer = spriteRenderers[i];
            spriteRenderer.transform.localPosition += Vector3.left * move;

            // 이미지가 너무 왼쪽으로 가면 오른쪽으로 보내기
            if (spriteRenderer.transform.localPosition.x <= -interval)
            {
                spriteRenderer.transform.localPosition = new Vector3(spriteRenderers[firstIndex].transform.localPosition.x + interval, 0f, 0f);
                firstIndex = spriteRenderers.IndexOf(spriteRenderer);
            }
        }
    }
}
