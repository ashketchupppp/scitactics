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
            Vector3 cellCenter = iTilemap.GetCellCenterWorld(character.transform.position.ConvertTo<Vector3Int>());
            character.transform.Translate(cellCenter - character.transform.position);

            // Now check if the character needs to move
            if (playerMove.x > 0 || playerMove.y > 0) {
                Vector3 newPosition = iTilemap.GetCellCenterWorld(new Vector3Int(
                    character.transform.position.x + Math.Ceiling(playerMove.x),
                    character.transform.position.y + Math.Ceiling(playerMove.y),
                    character.transform.position.z
                )
                );
                character.transform.Translate(newPosition);
            }
        }
    }
}
