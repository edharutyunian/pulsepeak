namespace PulsePeak.API.Configurations
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection ConfigureDependecyInjection(this IServiceCollection services)
        {
            return services.AddRepositories().AddOperations().AddHandlers();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            foreach (var repo in Repositories) {
                // TODO [ED]: find a way to add the repo as a service 
            }
            return services;
        }

        private static IServiceCollection AddOperations(this IServiceCollection services)
        {
            foreach (var operation in Operations) {

            }
            return services;
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            foreach (var handler in Handlers) {

            }
            return services;
        }

        private static readonly Dictionary<Type, Type> Repositories = new() {
            // TODO: add all the repositories here, where type is the interface , and the object is the repo itself
            {typeof(Type), typeof(object)}
        };

        private static readonly Dictionary<Type, Type> Operations = new() {
            // TODO: add all the operations here, where type is the interface , and the object is the operation itself
            {typeof(Type), typeof(object)},
        };

        // maybe only this one is needed
        private static readonly Dictionary<Type, Type> Handlers = new() {
            // TODO: add all the handlers here, where type is the interface , and the object is the handler itself
            {typeof(Type), typeof(object)},
        };
    }
}
