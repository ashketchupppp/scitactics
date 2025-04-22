using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour
{
    GameObject[] iCharacters;

    InputAction iMoveAction;
    Tilemap iTilemap;

    void Start() {
        iMoveAction = InputSystem.actions.FindAction("Move");

        iTilemap = GetComponentInChildren<Tilemap>();
        iCharacters = GameObject.FindGameObjectsWithTag("character");
    }

    void Update() {
        Vector2 playerMove = iMoveAction.ReadValue<Vector2>();

        foreach (GameObject character in iCharacters) {
            // First make sure the character is on a cell
            // Vector3 cellCenter = iTilemap.GetCellCenterWorld(character.transform.position.ConvertTo<Vector3Int>());
            // character.transform.Translate(cellCenter - character.transform.position);

            // Now check if the character needs to move
            if (iMoveAction.WasPressedThisFrame()) {
                Vector3Int playerCell = iTilemap.WorldToCell(character.transform.position);
                playerCell.x = (int)(playerCell.x + playerMove.x);
                playerCell.y = (int)(playerCell.y + playerMove.y);
                Vector3 newPosition = iTilemap.GetCellCenterWorld(playerCell);
                character.transform.Translate(newPosition - character.transform.position);
            }
        }
    }
}
