using System;

namespace Miriwork.Contracts
{
    /// <summary>
    /// Result of registration of dependencies of bounded context.
    /// </summary>
    public sealed class RegistrationResult
    {
        /// <summary>
        /// Module with dependencies to register.
        /// </summary>
        public T DependenciesModuleAs<T>() => (T)this.dependenciesModule;

        /// <summary>
        /// ResultType of registration.
        /// </summary>
        public RegistrationResultType ResultType => 
            this.dependenciesModule != null 
            ? RegistrationResultType.ReturnedDependenciesModuleToRegister 
            : RegistrationResultType.EverythingRegistered;

        private object dependenciesModule;

        private RegistrationResult() { }

        private RegistrationResult(object dependenciesModule)
        {
            this.dependenciesModule = dependenciesModule;
        }

        /// <summary>
        /// Instance to return if all dependencies are registered already.
        /// </summary>
        public static RegistrationResult EverythingRegistered() => new RegistrationResult();

        /// <summary>
        /// Instance to return if dependencies have to be registered outside.
        /// </summary>
        /// <param name="dependenciesModule">module with dependencies</param>
        public static RegistrationResult ReturnDependenciesModuleToRegister(object dependenciesModule)
            => new RegistrationResult(dependenciesModule);
    }

    /// <summary>
    /// ResultType of registration.
    /// </summary>
    public enum RegistrationResultType
    {
        /// <summary>
        /// All dependencies are registered already.
        /// </summary>
        EverythingRegistered,

        /// <summary>
        /// Dependencies are returned as module to register outside.
        /// </summary>
        ReturnedDependenciesModuleToRegister
    }
}