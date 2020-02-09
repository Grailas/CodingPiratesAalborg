using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New EnemyStats", menuName = "EnemyStats", order = 52)]
public class EnemyStats : ScriptableObject
{
	public float maxHealth;
	public float damage;
    public float speed;
    public Sprite appearance;
	[SerializeField]
	private float fixedUpdatePositionChange;

	// Start is called before the first frame update
	void Start()
    {
		fixedUpdatePositionChange = speed * Time.fixedDeltaTime;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnValidate()
	{
		fixedUpdatePositionChange = speed * Time.fixedDeltaTime;
	}

	public float GetFixedUpdatePositionChange()
	{
		return fixedUpdatePositionChange;
	}

}
