using System.Security.Principal;

namespace ImmersiveQuiz.Models
{
    /// <summary>
    /// Identity for Basic Authentication
    /// </summary>
    public class BasicAuthenticationIdentity : IIdentity
    {
        /// <summary>
        /// Constructs a <see cref="BasicAuthenticationIdentity"/>
        /// </summary>
        /// <param name="authenticationType">Type of authentication used</param>
        /// <param name="isAuthenticated">Indicates whether the user has been authenticated</param>
        /// <param name="name">Name of user authenticating</param>
        public BasicAuthenticationIdentity(string authenticationType, bool isAuthenticated, string name)
        {
            AuthenticationType = authenticationType;
            IsAuthenticated = isAuthenticated;
            Name = name;
        }
        /// <inheritdoc/>
        public string AuthenticationType { get; }

        /// <inheritdoc/>
        public bool IsAuthenticated { get; }
        
        /// <inheritdoc/>
        public string Name { get; }
    }
}
