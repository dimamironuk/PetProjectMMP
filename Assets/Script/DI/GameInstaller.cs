using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private ReactionDatabase reactionDatabase;
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private HandManager handManager;

    public override void InstallBindings()
    {
        Container.Bind<ReactionDatabase>().FromInstance(reactionDatabase).AsSingle();
        Container.Bind<EnergyManager>().FromComponentInHierarchy(energyManager).AsSingle();
        Container.Bind<DeckManager>().FromComponentInHierarchy(deckManager).AsSingle();
        Container.Bind<HandManager>().FromComponentInHierarchy(handManager).AsSingle();
    }
}