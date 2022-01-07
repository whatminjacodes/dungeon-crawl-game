using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void GenerateNewDungeonDelegate();
    public static event GenerateNewDungeonDelegate GenerateNewDungeonEvent;


    public delegate void GameStartsDelegate();
    public static event GameStartsDelegate GameStartsEvent;

    private void Awake() {
        DungeonGenerator.DungeonGeneratedEvent += OnDungeonGenerated;
    }

    private void Start() {
        if(GenerateNewDungeonEvent != null) {
            Debug.Log("GameManager: sending event");
            GenerateNewDungeonEvent();
        }
    }

    private void OnDungeonGenerated() {
        if(GameStartsEvent != null) {
            GameStartsEvent();
        }
    }
}
