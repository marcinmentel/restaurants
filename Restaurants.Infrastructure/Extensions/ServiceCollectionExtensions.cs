using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
    {
        var connectionString = configuration.GetConnectionString("RestaurantsDb");
        services.AddDbContext<RestaurantsDbContext>( options => 
            options.UseSqlServer(connectionString)
            .EnableSensitiveDataLogging());


        services.AddIdentityApiEndpoints<User>()
                .AddRoles<IdentityRole>()
                .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<RestaurantsDbContext>();

        services.AddScoped<IRestaurantsSeeder, RestaurantsSeeder>();
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository,DishesRepository>();
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasNationality,
                builder => builder.RequireClaim(AppClaimTypes.Nationality, "German", "Polish"));
            //.AddPolicy(PolicyNames.AtLeast20,
            //    builder => builder.AddRequirements(new MinimumAgeRequirement(20)))
            //.AddPolicy(PolicyNames.CreatedAtleast2Restaurants,
            //    builder => builder.AddRequirements(new CreatedMultipleRestaurantsRequirement(2)));
    }
}
