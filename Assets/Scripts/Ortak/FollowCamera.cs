using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float minimumX = 4f;
    public float maximumX = 55f;
    public float smoothness = 6f;

    void LateUpdate()
    {
        float x = Mathf.Clamp(target.position.x + 3f, minimumX, maximumX);
        Vector3 destination = new Vector3(x, 0f, -10f);
        transform.position = Vector3.Lerp(transform.position, destination, smoothness * Time.deltaTime);
    }
}
