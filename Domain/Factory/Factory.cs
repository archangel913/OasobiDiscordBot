using Microsoft.Extensions.DependencyInjection;

namespace Domain.Factory
{
    public static class Factory
    {

        private static IServiceProvider? ServiceProvider;

        public static void SetService(IServiceCollection service)
        {
            ServiceProvider = service.BuildServiceProvider();
        }

        public static T GetService<T>() where T : notnull
        {
            if(ServiceProvider is null) throw new NullReferenceException("ServiceProvider is Null");
            return (T)ServiceProvider.GetRequiredService<T>();
        }
    }
}
