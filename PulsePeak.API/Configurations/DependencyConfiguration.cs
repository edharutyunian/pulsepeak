namespace PulsePeak.API.Configurations
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection ConfigureDependecyInjection(this IServiceCollection services)
        {
            // TODO [ED]: add all the repos and other services, like operations and so
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            foreach (var repo in Repositories) {
                // TODO [ED]: find a way to add the repo as a service 

            }
            return services;
        }

        private static readonly Dictionary<Type, Type> Repositories = new() {
            // TODO [ED]: add all the repositories here, where type is the interface , and the object is the repo itself
            {typeof(Type), typeof(object)}
        };
    }
}
