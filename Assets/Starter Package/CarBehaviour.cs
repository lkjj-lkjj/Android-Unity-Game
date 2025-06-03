/*
 * Copyright 2021 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using UnityEngine;

/**
 * Our car will track a reticle and collide with a <see cref="PackageBehaviour"/>.
 */
public class CarBehaviour : MonoBehaviour
{
    public ReticleBehaviour Reticle;
    public float Speed = 1.2f;
    public float AvoidDistance = 0.5f; // 避障检测距离

    public int MaxHealth = 5;
    public int CurrentHealth;
    public int Score = 0;
    public int Level = 1;
    public int ScoreToLevelUp = 10;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        var trackingPosition = Reticle.transform.position;
        if (Vector3.Distance(trackingPosition, transform.position) < 0.1)
        {
            return;
        }

        // --- 障碍物避障逻辑开始 ---
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, AvoidDistance))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                // 随机向左或右转90度，尝试避开障碍物
                float turn = Random.value > 0.5f ? 1 : -1;
                transform.Rotate(0, 90 * turn, 0);
                return; // 本帧不前进
            }
        }
        // --- 障碍物避障逻辑结束 ---

        var lookRotation = Quaternion.LookRotation(trackingPosition - transform.position);
        transform.rotation =
            Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        transform.position =
            Vector3.MoveTowards(transform.position, trackingPosition, Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var Package = other.GetComponent<PackageBehaviour>();
        if (Package != null)
        {
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            // 小车死亡逻辑
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;
        if (Score >= ScoreToLevelUp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        Level++;
        Score = 0;
        // 可在这里弹出三选一升级界面
    }
}
