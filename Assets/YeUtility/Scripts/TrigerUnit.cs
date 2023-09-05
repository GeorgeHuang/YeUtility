using UnityEngine;

public class TrigerUnit : MonoBehaviour
{
    public string value;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(value);
    }
}
