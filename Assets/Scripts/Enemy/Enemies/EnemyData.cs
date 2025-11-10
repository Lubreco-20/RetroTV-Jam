using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("General")]
    public int enemyId; //Id del tipo de enemigo
    public GameObject enemyPrefab; //Prefab del enemigo

    [Header("Health")]
    public int maxHealth; //Máxima vida del enemigo

    [Header("Movement")]
    public float minSpeed; //Velocidad mínima a la que se va a mover
    public float maxSpeed; //Velocidad máxima a la que se va a mover

    [Header("Attack")]
    public int dmg; //Daño del enemigo

    [Header("Attack")]
    public float attackRange; //Rango desde el que comenzará a disparar o atacar
    public float attackCoolDown; //Intervalos de ataque

    [Header("Spawn")]
    [Range (0f, 1f)] public float spawnProbability = 1f;
    public bool isBoss = false;
}