using UnityEngine;

public class RiderMover : Mover
{
    public override void Update()
    {
        transform.position += Vector3.left * GameManager.instance.CalculateGameSpeed() * moveSpeed * Time.deltaTime;
    }
}
