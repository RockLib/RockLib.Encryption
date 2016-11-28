using System;
using System.Collections.Generic;
using System.Linq;

namespace Rock.Encryption
{
    internal static class CredentialInfoExtensions
    {
        public static bool TryGetCredential<TCredentialInfo>(
            this IReadOnlyCollection<TCredentialInfo> credentials, object keyIdentifier, out TCredentialInfo credential)
            where TCredentialInfo : class, ICredentialInfo
        {
            var credentialName = keyIdentifier as string;

            if (credentialName != null)
            {
                credential =
                    credentials.FirstOrDefault(candidate =>
                        candidate.Name == credentialName);

                return credential != null;
            }

            var targetType = keyIdentifier as Type;

            if (targetType != null)
            {
                credential =
                    credentials.FirstOrDefault(candidate =>
                        candidate.Types != null && candidate.Types.Any(candidateType =>
                            candidateType == targetType.FullName));

                if (credential != null)
                {
                    return true;
                }

                var targetNamespaces = GetTargetNamespaces(targetType);

                credential =
                    credentials.FirstOrDefault(candidate =>
                        candidate.Namespaces != null && targetNamespaces.Any(targetNamespace =>
                            candidate.Namespaces.Any(candidateNamespace =>
                                candidateNamespace == targetNamespace)));

                return credential != null;
            }

            credential = null;
            return false;
        }

        private static IEnumerable<string> GetTargetNamespaces(Type targetType)
        {
            var targetNamespaces = new List<string>();

            var targetNamespace = targetType.Namespace;

            while (!string.IsNullOrEmpty(targetNamespace))
            {
                targetNamespaces.Add(targetNamespace);

                var lastDot = targetNamespace.LastIndexOf('.');

                if (lastDot == -1)
                {
                    break;
                }

                targetNamespace = targetNamespace.Substring(0, lastDot);
            }

            return targetNamespaces;
        }
    }
}