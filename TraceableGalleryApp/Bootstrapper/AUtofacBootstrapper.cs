using Autofac;
using TraceableGalleryApp.Interfaces;
using XLabs.Ioc;
using XLabs.Ioc.Autofac;

namespace TraceableGalleryApp
{
    public abstract class AutofacBootstrapper
    {
        public void Run()
        {
            var builder = new ContainerBuilder();
            RegisterModules (builder);

            var container = builder.Build();

            if (!Resolver.IsSet)
            {
                var autofacResolver = new AutofacResolver (container);
                Resolver.SetResolver (autofacResolver);
            }

            var viewFactory = container.Resolve<IViewFactory>();
            RegisterViews(viewFactory);

            ConfigureApplication (container);
        }

        protected virtual void RegisterModules(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacModule>();
        }

        protected abstract void RegisterViews(IViewFactory viewFactory);

        protected abstract void ConfigureApplication(IContainer container);
    }
}

