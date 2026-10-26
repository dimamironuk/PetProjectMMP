using System.Collections;
using UnityEngine;
using Zenject;

public class CharacterSlot : MonoBehaviour
{
    [SerializeField] private CharacterUnit unitPrefab;
    [SerializeField] private float deathDespawnDelay = 0.35f;

    [Inject] private DiContainer container;

    private Coroutine despawnRoutine;

    public CharacterUnit Unit { get; private set; }
    public bool IsEmpty => Unit == null || !Unit.IsAlive;

    public CharacterUnit Place(CharacterCardData data)
    {
        if (!IsEmpty) return null;
        if (unitPrefab == null || data == null) return null;

        ClearUnit(true);

        Unit = container.InstantiatePrefabForComponent<CharacterUnit>(unitPrefab, transform);
        Unit.Initialize(data);
        Unit.Died += OnUnitDied;
        return Unit;
    }

    private void OnUnitDied(CharacterUnit unit)
    {
        unit.Died -= OnUnitDied;
        if (despawnRoutine != null) StopCoroutine(despawnRoutine);
        despawnRoutine = StartCoroutine(DespawnAfterDelay(unit));
    }

    private IEnumerator DespawnAfterDelay(CharacterUnit unit)
    {
        yield return new WaitForSeconds(deathDespawnDelay);
        if (Unit == unit) Unit = null;
        DestroyUnit(unit);
        despawnRoutine = null;
    }

    private void OnDestroy()
    {
        ClearUnit(true);
    }

    private void ClearUnit(bool immediate)
    {
        if (despawnRoutine != null)
        {
            StopCoroutine(despawnRoutine);
            despawnRoutine = null;
        }

        if (Unit == null) return;

        Unit.Died -= OnUnitDied;
        if (immediate) DestroyUnit(Unit);
        Unit = null;
    }

    private static void DestroyUnit(CharacterUnit unit)
    {
        if (unit != null) Destroy(unit.gameObject);
    }
}
