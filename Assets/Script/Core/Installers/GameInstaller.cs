using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private ReactionDatabase reactionDatabase;
    [SerializeField] private TokenSettings tokenSettings;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private HandManager handManager;
    [SerializeField] private PlayerBoard playerBoard;

    public override void InstallBindings()
    {
        Container.Bind<ReactionDatabase>().FromInstance(reactionDatabase);
        Container.Bind<TokenSettings>().FromInstance(tokenSettings);

        Container.BindInterfacesAndSelfTo<DeckManager>().FromInstance(deckManager);
        Container.BindInterfacesAndSelfTo<HandManager>().FromInstance(handManager);
        Container.BindInterfacesAndSelfTo<PlayerBoard>().FromInstance(playerBoard);
        Container.BindInterfacesAndSelfTo<TokenPool>().AsSingle();

        Container.Bind<ICardPlayHandler>().To<CharacterCardHandler>().AsSingle();
        Container.Bind<ICardPlayHandler>().To<ActionCardHandler>().AsSingle();
        Container.Bind<ICardPlayHandler>().To<PassiveCardHandler>().AsSingle();

        Container.Bind<CardPlayService>().AsSingle();
        Container.Bind<AbilityService>().AsSingle();
    }
}