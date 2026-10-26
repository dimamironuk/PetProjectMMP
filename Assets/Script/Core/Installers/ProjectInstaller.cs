using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IAuthService>().To<PlayFabAuthService>().AsSingle();
    }
}