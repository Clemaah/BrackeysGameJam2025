using System;
using System.Collections;
using UnityEngine;


public class MainCharacter : Character
{
    public static MainCharacter Instance { get; private set; }
    
    public ProjectileLauncher projectileLauncher;
    
    protected new void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (GameManager.Instance && GameManager.Instance.Paused) return;
        
        Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputs = Quaternion.Euler(0, 0, -Camera.main.transform.rotation.eulerAngles.y) * inputs;

        if (inputs.magnitude > 0.1f 
            && (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.LeftShift)) 
            && TryDash(CharacterController.velocity.X0Z())) 
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 intersection = ray.origin + ray.direction * Mathf.Abs(ray.origin.y / ray.direction.y);
        transform.forward = (intersection - transform.position).X0Z();

        if (IsDashing) return;

        CharacterController.transform.localScale = new Vector3(scale.Get(), scale.Get(), scale.Get());
        CharacterController.Move(inputs.X0Y().normalized * (speed.Get() * Time.deltaTime));

        if (Input.GetMouseButton(0))
        {
            projectileLauncher.TryFire();
        }
    }

    public void TeleportTo(Vector3 playerPosition, Vector3 cameraPosition)
    {
        CharacterController.enabled = false;
        transform.position = playerPosition;
        Camera.main.transform.position = cameraPosition;
        CharacterController.enabled = true;
    }
}