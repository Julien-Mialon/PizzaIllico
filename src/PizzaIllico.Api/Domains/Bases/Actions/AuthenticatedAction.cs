using System;
using PizzaIllico.Api.Domains.Accounts.Models;
using Storm.Api.Core.CQRS;
using Storm.Api.Core.Logs;

namespace PizzaIllico.Api.Domains.Bases.Actions
{
	public abstract class AuthenticatedAction<TParameter, TOutput> : BaseAuthenticatedAction<TParameter, TOutput, User, object>
	{
		public ILogService LogService => Resolve<ILogService>();

		protected AuthenticatedAction(IServiceProvider services) : base(services, null)
		{
		}
	}
}