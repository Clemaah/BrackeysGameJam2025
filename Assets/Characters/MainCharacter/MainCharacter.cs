using System.Collections;
using UnityEngine;


public class MainCharacter : Character
{
    public ProjectileLauncher projectileLauncher;



    void Update()
    {
        Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputs = Quaternion.Euler(0, 0, -Camera.main.transform.rotation.eulerAngles.y) * inputs;

        if (inputs.magnitude > 0.1f && (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.LeftShift)))
        {
            if (TryDash(CharacterController.velocity.X0Z()))
            {
                return;
            }
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 intersection = ray.origin + ray.direction * Mathf.Abs(ray.origin.y / ray.direction.y);
        transform.forward = (intersection - transform.position).X0Z();

        if (IsDashing) return;

        CharacterController.Move(inputs.X0Y().normalized * (speed.Get() * Time.deltaTime));

        if (Input.GetMouseButton(0))
        {
            projectileLauncher.TryFire();
        }
    }

    
}