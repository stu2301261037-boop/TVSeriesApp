using Microsoft.AspNetCore.Authorization;

namespace TVSeriesApp.Attributes
{
	public class AuthorizeRolesAttribute : AuthorizeAttribute
	{
		public AuthorizeRolesAttribute(params string[] roles) : base()
		{
			Roles = string.Join(",", roles);
		}
	}
}