using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    // What direction is the door
    public enum Direction { North, East, South, West, DEFAULT };     // North = 0, East = 1, South = 2, West = 3

    // How many and what size rooms to generate
    [SerializeField] private int _numberOfRooms = 1;
    [SerializeField] private int _heightOfRoom = 7;
    [SerializeField] private int _widthOfRoom = 14;

    // Graphics
    [SerializeField] private GameObject[] _groundTiles;
    [SerializeField] private GameObject[] _wallTiles;
    [SerializeField] private GameObject _doorTile;


    // What is the area camera sees
    int roomCameraViewWidth = 16;
    int roomCameraViewHeight = 9;

    // Used for naming the parent prefab
    int _numberOfRoomsGenerated = 0;

    // Where to build the room
    int startBuildingPosX = 0;
    int startBuildingPosY = 0;

    int nextRoomStartBuildingPosX = 0;
    int nextRoomStartBuildingPosY = 0;

    int _previousRoomDirection = -1;
    int _nextRoomDirection = -1;

    public delegate void DungeonGeneratedDelegate();
    public static event DungeonGeneratedDelegate DungeonGeneratedEvent;

    private void Awake() {
        Debug.Log("DungeonGenerator: awake");
        GameManager.GenerateNewDungeonEvent += OnGenerateNewDungeon;
    }

    private void Start() {
        //GenerateRoom();
    }

    public void OnGenerateNewDungeon() {
        Debug.Log("DungeonGenerator: OnGenerateNewDungeon");

        // Generate multiple rooms
        for(int i = 1; i <= _numberOfRooms; i++) {

            // TODO: Refactor so that we know next room before building the first room
            // This is needed so that door can be placed on correct location
            GenerateRoom(startBuildingPosX, startBuildingPosY);
            CalculateNextRoomPosition();
        }

        if(DungeonGeneratedEvent != null) {
            DungeonGeneratedEvent();
        }
    }

    // Generate one room to startX and startY location
    public void GenerateRoom(int startX, int startY) {
        
        GameObject roomParent = new GameObject("RoomParent" + _numberOfRoomsGenerated);
        roomParent.transform.position = new Vector3(0, 0, 0);

        for (int i = startX; i < startX + _widthOfRoom; i++) {
            for (int j = startY; j < startY + _heightOfRoom; j++) {
                if (i == startX) {                                  // Instantiate wall left
                    Instantiate(_wallTiles[0], new Vector3(i, j, 0), Quaternion.identity, roomParent.transform);
                } else if (i == startX + _widthOfRoom - 1) {        // Instantiate wall right
                    Instantiate(_wallTiles[0], new Vector3(i, j, 0), Quaternion.identity, roomParent.transform);
                } else if (j == startY) {                           // Instantiate wall bottom
                    Instantiate(_wallTiles[0], new Vector3(i, j, 0), Quaternion.identity, roomParent.transform);
                } else if (j == startY + _heightOfRoom - 1) {       // Instantiate wall top
                    Instantiate(_wallTiles[0], new Vector3(i, j, 0), Quaternion.identity, roomParent.transform);
                } else {                                            // Instantiate ground
                    Instantiate(_groundTiles[0], new Vector3(i, j, 0), Quaternion.identity, roomParent.transform);
                }
            }
        }

        _numberOfRoomsGenerated++;
    }

    // Calculate the position of the next room
    private void CalculateNextRoomPosition() {
        
        int nextDirection = RandomizeNextRoomDirection();
        while(nextDirection == _previousRoomDirection) {
            nextDirection = RandomizeNextRoomDirection();
        }

        Debug.Log("Next room direction: " + nextDirection + ", previous direction: " + _previousRoomDirection);
        if (nextDirection == (int)Direction.North) {
            startBuildingPosY += _heightOfRoom;

        } else if (nextDirection == (int)Direction.East) {
            startBuildingPosX += _widthOfRoom;

        } else if (nextDirection == (int)Direction.South) {
            startBuildingPosY -= _heightOfRoom;

        } else if (nextDirection == (int)Direction.West) {
            startBuildingPosX -= _widthOfRoom;
        }

        _previousRoomDirection = GetOppositeDirection(nextDirection);
    }

    // Randomize the direction where the next room will be
    private int RandomizeNextRoomDirection() {
        return Random.Range(0, 4);   // Returns random between 0...3
    }

    // Get the direction where we last were building a room
    private int GetOppositeDirection(int nextRoomDirection) {
        int previousDirection = -1;

        if (nextRoomDirection == (int)Direction.North) {
            previousDirection = (int)Direction.South;
        } else if (nextRoomDirection == (int)Direction.East) {
            previousDirection = (int)Direction.West;
        } else if (nextRoomDirection == (int)Direction.South) {
            previousDirection = (int)Direction.North;
        } else if (nextRoomDirection == (int)Direction.West) {
            previousDirection = (int)Direction.East;
        }

        return previousDirection;
    }

    private int GetDoorPosition(int lengthOfWall) {
        return Random.Range(1, lengthOfWall);     // Get random location between 1 and length of wall -1
    }
}
