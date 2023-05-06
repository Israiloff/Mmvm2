namespace Mmvm.Container.Services.IocContainer
{
    public interface IIocContainer
    {
        TObject Resolve<TObject>();

        TObject Resolve<TObject>(string name);

        TObject Resolve<TObject>(object key);

        TObject ResolveOptional<TObject>() where TObject : class;

        TObject ResolveOptional<TObject>(string name) where TObject : class;

        TObject ResolveOptional<TObject>(object key) where TObject : class;
    }
}