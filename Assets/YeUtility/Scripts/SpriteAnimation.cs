using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    public Sprite[] sprites;  // 要切换的图片数组
    public float frameRate = 10f;  // 动画帧率

    [SerializeField] private SpriteRenderer spriteRenderer;
    private int currentIndex = 0;
    private float timer = 0f;
    private Color color;

    void Start()
    {
        spriteRenderer.sprite = sprites[currentIndex];
        color = spriteRenderer.color;
    }

    public void Tick(float dt)
    {
        timer += dt;
        if (!(timer >= 1f / frameRate)) return;
        currentIndex = (currentIndex + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[currentIndex];
        timer = 0f;
    }
}
