using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int MaxHealth = 3;
    private int currentHealth;
    public float Speed = 2.0f;
    public Transform Target; // 小车目标

    void Start()
    {
        currentHealth = MaxHealth;
        // 如果没指定目标，自动寻找小车
        if (Target == null)
        {
            var car = GameObject.FindWithTag("Player");
            if (car != null) Target = car.transform;
        }
    }

    void Update()
    {
        if (Target != null)
        {
            Vector3 dir = (Target.position - transform.position).normalized;
            transform.position += dir * Speed * Time.deltaTime;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            // 找到小车并加分
            var car = GameObject.FindWithTag("Player");
            if (car != null)
            {
                var carBehaviour = car.GetComponent<CarBehaviour>();
                if (carBehaviour != null)
                {
                    carBehaviour.AddScore(1);
                }
            }
            Destroy(gameObject);
        }
    }
}