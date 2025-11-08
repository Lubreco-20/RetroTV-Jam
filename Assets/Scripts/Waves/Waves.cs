using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class Waves : ScriptableObject
{
    public List<Wave> waves;

    [System.Serializable]
    public class Wave
    {
        public List<EnemyData> enemyList; //Lista de enemigos que se pueden haber en la oleada
        public int waveQuota; // Cuota de los enemigos que pueden haber
        public float enemySpawnInterval; // Intervalo de tiempo en el que aparecerán los enemigos
    }
}