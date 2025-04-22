using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour
{
    public const int sMaxHeight = 100;

    GameObject[] iCharacters;
    InputAction iMoveAction;
    InputAction iMouseMoveAction;
    Tilemap iTilemap;

    void Start() {
        iMoveAction = InputSystem.actions.FindAction("Move");
        iMouseMoveAction = InputSystem.actions.FindAction("MouseMove");

        iTilemap = GetComponentInChildren<Tilemap>();
        iCharacters = GameObject.FindGameObjectsWithTag("character");

        // Place characters onto their nearest cell, to make sure they are centered
        foreach (GameObject character in iCharacters) {
            Vector3Int characterCell = iTilemap.WorldToCell(character.transform.position);
            MoveCharacterToCell(character, characterCell);
        }
    }

    void MoveCharacterToCell(GameObject character, Vector3Int toCell) {
        toCell.z = GetTopCellZ(toCell.x, toCell.y);
        Sprite newPosSprite = iTilemap.GetSprite(toCell);
        // Somehow move the player to the center of the sprite
        // bounds.center gives us the center of the sprite in local space?? need to convert to world space.
        // Vector3 newPosition = newPosSprite.bounds.center;
        // Vector3 newPosition = iTilemap.GetCellCenterWorld(toCell);
        character.transform.Translate(newPosition - character.transform.position);
    }

    int GetTopCellZ(int x, int y) {
        int Z = 0;
        for (int z = 0; z < sMaxHeight; z++) {
            TileBase tile = iTilemap.GetTile(new Vector3Int(x, y, z));
            if (tile == null) {
                break;
            }
            Z = z;
        }
        return Z;
    }

    void Update() {
        Vector2 playerMove = iMoveAction.ReadValue<Vector2>();

        foreach (GameObject character in iCharacters) {
            // WASD move
            if (iMoveAction.WasPressedThisFrame()) {
                Vector3Int playerCell = iTilemap.WorldToCell(character.transform.position);
                playerCell.x = (int)(playerCell.x + playerMove.x);
                playerCell.y = (int)(playerCell.y + playerMove.y);
                MoveCharacterToCell(character, playerCell);
            // Mouse Click move
            } else if (iMouseMoveAction.WasPressedThisFrame()) {
                Vector2 mousePosition = Pointer.current.position.ReadValue();
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
                Vector3Int mouseSelectedCell = iTilemap.WorldToCell(mouseWorldPos);
                MoveCharacterToCell(character, mouseSelectedCell);
            }
        }
    }
}
