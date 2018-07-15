using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Miriwork.Contracts;

namespace Miriwork
{
    internal class RequestMetadataFactory
    {
        private readonly Type requestBaseType;
        private readonly Type responseBaseType;
        private readonly Type serviceBaseType;
        private string[] httpMethodNames = Enum.GetNames(typeof(HttpMethod));

        public RequestMetadataFactory(Type requestBaseType, Type responseBaseType, Type serviceBaseType)
        {
            this.requestBaseType = requestBaseType;
            this.responseBaseType = responseBaseType;
            this.serviceBaseType = serviceBaseType;
        }

        public IEnumerable<RequestMetadata> CreateRequestMetadata(Assembly servicesAssembly)
        {
            List<RequestMetadata> requestMetadataOfAssembly = new List<RequestMetadata>();

            var possibleServiceClassTypes = GetPossibleServiceClassTypes(servicesAssembly);
            foreach (Type serviceType in possibleServiceClassTypes)
            {
                var possibleRestMethodInfos = GetPossibleRestMethodInfos(serviceType);
                foreach (MethodInfo methodInfo in possibleRestMethodInfos)
                {
                    if (!TryGetPossibleRequestAndResponseType(methodInfo, out Type requestType, out Type reponseType))
                        continue;

                    if (!IsRequestAndResponse(requestType, reponseType))
                        continue;
                    
                    HttpMethod httpMethod = (HttpMethod)Enum.Parse(typeof(HttpMethod), methodInfo.Name);
                    requestMetadataOfAssembly.Add(new RequestMetadata(requestType, httpMethod, serviceType));
                }
            }

            return requestMetadataOfAssembly;
        }

        private IEnumerable<Type> GetPossibleServiceClassTypes(Assembly servicesAssembly)
        {
            if (this.serviceBaseType != null)
                return servicesAssembly.GetTypes().Where(t => this.serviceBaseType.IsAssignableFrom(t));

            return servicesAssembly.GetTypes().Where(t => t.IsClass && t.IsPublic && !t.IsAbstract);
        }

        private IEnumerable<MethodInfo> GetPossibleRestMethodInfos(Type serviceType)
        {
            return serviceType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => httpMethodNames.Contains(m.Name));
        }

        private bool TryGetPossibleRequestAndResponseType(MethodInfo methodInfo, out Type requestType, out Type responseType)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length != 1)
            {
                requestType = null;
                responseType = null;
                return false;
            }

            requestType = parameters[0].ParameterType;
            responseType = methodInfo.ReturnType;
            return true;
        }

        private bool IsRequestAndResponse(Type requestType, Type responseType)
        {
            if (IsSimpleType(requestType) || IsSimpleType(responseType))
                return false;

            if (!IsRequest(requestType))
                return false;

            if (!IsResponse(responseType))
                return false;
            
            return true;
        }

        private bool IsRequest(Type requestType)
        {
            if (this.requestBaseType != null)
                return this.requestBaseType.IsAssignableFrom(requestType);

            return requestType.Name.ToLower().EndsWith("request");
        }

        private bool IsResponse(Type responseType)
        {
            // if method is awaitable then use generic type argument            
            Type innerResponseType;
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Task<>))
                innerResponseType = responseType.GenericTypeArguments[0];
            else
                innerResponseType = responseType;
            
            if (this.responseBaseType != null)
                return this.responseBaseType.IsAssignableFrom(innerResponseType);

            return innerResponseType.Name.ToLower().EndsWith("response");
        }

        private bool IsSimpleType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return false;
            return type.IsPrimitive || type.IsEnum || type.Equals(typeof(string)) || type.Equals(typeof(decimal));
        }
    }
}