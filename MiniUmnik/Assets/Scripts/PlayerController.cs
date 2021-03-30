using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Пушка")]
    public Gun gun;

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
    private Camera mainCam;
    private CharacterController controller;

    private GameMenu gameMenu;
    public event Action<bool> OnRunStateChange;
    public bool IsRun { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        gameMenu = UIController.GetMenu<GameMenu>();
        mainCam = Camera.main;
        eyes = mainCam.transform.parent;
        controller = GetComponent<CharacterController>();
    }

    public void CameraLogic()
    {

        rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, minLook, maxLook);
        float delta = Input.GetAxis("Mouse X") * sensitivity;
        float rotationY = eyes.localEulerAngles.y + delta;
        eyes.localEulerAngles = new Vector3(rotationX, rotationY, 0);

    }

    internal void ShootLogic()
    {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);
        Vector3 target = hits != null ? hits[0].point : ray.direction * 100;
        gun.Shoot(target);
    }

    public void MoveLogic()
    {

        Vector3 frontBackMove = new Vector3(eyes.forward.x, 0, eyes.forward.z);
        Vector3 leftRightMove = new Vector3(frontBackMove.z, 0, -frontBackMove.x);

        Vector2 axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        IsRun = axis.magnitude > 0.0001f;
        gun.RunLogic(IsRun);
        var deltaTime = Time.fixedDeltaTime;
        controller.Move(frontBackMove * axis.x * deltaTime * speed);
        controller.Move(leftRightMove * axis.y * deltaTime * speed);
    }


}
