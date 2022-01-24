using UnityEngine;

public class BillBoard : MonoBehaviour
{
    [SerializeField] private Transform player;
    void Update()
    {
        transform.LookAt(player);
    }
}
