using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New ProjectileStats", menuName = "ProjectileStats", order = 51)]
public class ProjectileStats : ScriptableObject
{	
    public float speed;
    public int damage;
    public Sprite appearance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// Moves a Projectile by its Rigidbody2D along its local x-axis (to the right).
	/// </summary>
	/// <param name="rb2">A Rigidbody2D that belongs to a Projectile</param>
	public void MoveProjectile(Rigidbody2D rb2)
    {
		rb2.MovePosition(rb2.transform.position + (rb2.transform.right * speed * Time.fixedDeltaTime)); 
    }
}
