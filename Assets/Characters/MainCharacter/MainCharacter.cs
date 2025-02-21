using System;
using System.Collections;
using UnityEngine;


public class MainCharacter : Character
{
    public static MainCharacter Instance { get; private set; }

    public FloatValue speedMultiplierWhileShooting;
    public FloatValue controller;
    
    public ProjectileLauncher projectileLauncher;

    private bool _isShooting;
    
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
        if (GameManager.Instance)
        {
            if (GameManager.UIManager.currentMenu != MenuType.WishesSelection && Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.UIManager.OpenMenu(GameManager.Instance.Paused ? MenuType.None : MenuType.Pause);
                return;
            }
            
            if (GameManager.Instance.Paused) return;
        }

        bool carController = controller.Get() > 0.5;
        _isShooting = Input.GetMouseButton(0);
        
        
        Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (!carController)
            inputs = Quaternion.Euler(0, 0, -Camera.main.transform.rotation.eulerAngles.y) * inputs;

        if (inputs.magnitude > 0.1f 
            && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) 
            && TryDash(characterController.velocity.X0Z())) 
            return;

        if (!carController)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 intersection = ray.origin + ray.direction * Mathf.Abs((ray.origin.y - projectileLauncher.transform.position.y) / ray.direction.y);
            transform.forward = (intersection - transform.position).X0Z();
        }
        else
        {
            transform.rotation *= Quaternion.Euler(0, inputs.x, 0);
        }

        if (IsDashing) return;
        
        if (carController)
            characterController.Move(transform.forward * (inputs.y * (_isShooting ? speedMultiplierWhileShooting.Get() : 1.0f) * speed.Get() * Time.deltaTime));
        else
            characterController.Move(inputs.X0Y().normalized * ((_isShooting ? speedMultiplierWhileShooting.Get() : 1.0f) * speed.Get() * Time.deltaTime));
        
        if (_isShooting)
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