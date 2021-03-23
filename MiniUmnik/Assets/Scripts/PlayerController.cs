using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Скорость перемещения")]
    [Range(1, 9)]
    public float speed = 2;

    [Header("Чувствительность поворота мыши")]
    [Range(0.5f, 6)]
    public float sensitivity = 0.5f;

    [Header("Максимальный градус взгляда")]
    public float maxLook = 70;
    [Header("Минимальный градус взгляда")]
    public float minLook = -45;

    private float rotationX;
    private Transform eyes;
    private CharacterController controller;

    private GameMenu gameMenu;

    // Start is called before the first frame update
    void Start()
    {
        gameMenu = UIController.GetMenu<GameMenu>();
        eyes = Camera.main.transform;

        controller = gameObject.AddComponent<CharacterController>();

    }

    public void CameraLogic()
    {

        rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, minLook, maxLook);
        float delta = Input.GetAxis("Mouse X") * sensitivity;
        float rotationY = transform.localEulerAngles.y + delta;
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);

    }
    public void MoveLogic()
    {

        Vector3 frontBackMove = new Vector3(eyes.forward.x, 0, eyes.forward.z);
        Vector3 leftRightMove = new Vector3(frontBackMove.z, 0, -frontBackMove.x);
        controller.Move(frontBackMove * Input.GetAxis("Vertical") * Time.fixedDeltaTime * speed);
        controller.Move(leftRightMove * Input.GetAxis("Horizontal") * Time.fixedDeltaTime * speed);
    }

}
