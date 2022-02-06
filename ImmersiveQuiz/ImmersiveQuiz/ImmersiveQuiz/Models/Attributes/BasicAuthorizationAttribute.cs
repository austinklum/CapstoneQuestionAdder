using Microsoft.AspNetCore.Authorization;

namespace ImmersiveQuiz.Models.Attributes
{
    /// <summary>
    /// AuthorizeAttribute used for Basic Authentication
    /// </summary>
    public class BasicAuthorizationAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Constructs a <see cref="BasicAuthorizationAttribute"/> with a "BasicAuthentication" policy
        /// </summary>
        public BasicAuthorizationAttribute()
        {
            Policy = "BasicAuthentication";
        }
    }
}
