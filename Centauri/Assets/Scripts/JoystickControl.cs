using UnityEngine;

public class JoystickControl : MonoBehaviour
{
    public GameObject player;
    [Header("Speed of rotation")]
    public float rotationalSpeed;

    private bool isOutOfBound = false;
    private InputDetectionUtils inputDetectionUtils;
    private Vector3 rotationVector;

    private void Start()
    {
        inputDetectionUtils = new InputDetectionUtils();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        //inputDetectionUtils.ProcessTouchState();
        rotationVector = inputDetectionUtils.MoveDirection.normalized;
        Debug.Log(rotationVector);

        Quaternion lookRotation = Quaternion.LookRotation(rotationVector - player.transform.parent.transform.position);
        Transform velocityVector = player.transform.Find("Player_VelocityVector").transform;
        velocityVector.rotation = Quaternion.RotateTowards(velocityVector.rotation, lookRotation, rotationalSpeed * Time.deltaTime);
    }

    public Vector2 GetBoundingFromObject(GameObject boundingObject)
    {
        return boundingObject.GetComponent<BoxCollider>().size;
    }

    private void OnTouchStay()
    {
        //TODO Process event here
        isOutOfBound = false;
    }

    private void OnTouchUp()
    {
        isOutOfBound = true;
    }

    private void OnTouchExit()
    {
        isOutOfBound = true;
    }

    private void ProcessControl()
    {
        // inputDetectionUtils.ProcessTouchState();
    }

}
