using UnityEngine;


public class MainCharacter : Character
{
    
    public ProjectileLauncher projectileLauncher;

    void Update()
    {
        Vector2 inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        CharacterController.Move(inputs.X0Y().normalized * (speed.Get() * Time.deltaTime));
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 intersection = ray.origin + ray.direction * Mathf.Abs(ray.origin.y / ray.direction.y);
        transform.forward = (intersection - transform.position).X0Z();


        if (Input.GetMouseButton(0))
        {
            projectileLauncher.TryFire();
        }
    }
}
