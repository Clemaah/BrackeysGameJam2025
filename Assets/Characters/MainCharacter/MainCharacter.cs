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
            && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) 
            && TryDash(characterController.velocity.X0Z())) 
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 intersection = ray.origin + ray.direction * Mathf.Abs((ray.origin.y - projectileLauncher.transform.position.y) / ray.direction.y);
        transform.forward = (intersection - transform.position).X0Z();

        if (IsDashing) return;

        characterController.Move(inputs.X0Y().normalized * (speed.Get() * Time.deltaTime));

        if (Input.GetMouseButton(0))
        {
            projectileLauncher.TryFire();
        }
    }

    public void TeleportTo(Vector3 playerPosition, Vector3 cameraPosition)
    {
        characterController.enabled = false;
        transform.position = playerPosition;
        Camera.main.transform.position = cameraPosition;
        characterController.enabled = true;
    }
}