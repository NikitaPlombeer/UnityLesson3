using UnityEngine;

public class CoinController : MonoBehaviour
{

    public float RotationSpeed;
  
    void Update()
    {
        transform.Rotate(0f, RotationSpeed * Time.deltaTime, 0f);
    }
    
}
