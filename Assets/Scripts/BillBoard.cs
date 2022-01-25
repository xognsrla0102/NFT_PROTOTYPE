using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform player;

    private void Start() => player = GameObject.Find("Player").transform;

    void Update() => transform.LookAt(player);
}
