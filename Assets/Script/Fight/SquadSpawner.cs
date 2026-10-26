using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SquadSpawner : MonoBehaviour
{
    [SerializeField] private List<CharacterCardData> squad = new();

    [Inject] private IBoard board;

    private void Start()
    {
        foreach (var character in squad)
        {
            if (!board.TryDeploy(character)) break;
        }
    }
}