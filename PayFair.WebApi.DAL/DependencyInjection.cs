using Microsoft.Extensions.DependencyInjection;
using PayFair.WebApi.DAL.Repositories;
using PayFair.WebApi.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayFair.WebApi.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUsersAuthRepository, UsersAuthRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IFriendshipsRepository, FriendshipsRepository>();
            services.AddTransient<IGroupsRepository, GroupsRepository>();
            services.AddTransient<IMembershipsRepository, MembershipRepository>();
            return services;
        }
    }
}
