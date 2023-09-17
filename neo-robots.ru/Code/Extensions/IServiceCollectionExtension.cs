using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SMI.Areas.Admin.Models;
using SMI.Data.Entities;
using SMI.Data.Mappings;
using SMI.Managers;
using SMI.Managers.Core;
using SMI.Services;

namespace SMI.Code.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void DI(this IServiceCollection services)
        {
            services.AddAuthentication().AddCookie();
            services.AddSingleton(Log.Logger);
            services.AddMediatR(typeof(Startup));
            services.AddScoped<IViewRender, ViewRender>();
            services.AddSingleton<ICronManager, CronManager>();
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddScoped<ICitiesManager, CitiesManager>();
            services.AddScoped<IAuthorsManager, AuthorsManager>();
            services.AddScoped<IHashTagsManager, HashTagsManager>();
            services.AddScoped<INewsManager, NewsManager>();
            services.AddScoped<INewspapersManager, NewspapersManager>();
            services.AddScoped<IPhotosManager, PhotosManager>();
            services.AddScoped<IRegionsManager, RegionsManager>();
            services.AddScoped<IThemesManager, ThemesManager>();
            services.AddScoped<IAggregatorManager, AggregatorManager>();
            services.AddScoped<IAggregatorSourcesManager, AggregatorSourcesManager>();
            services.AddScoped<IAggregatorListsManager, AggregatorListsManager>();
            services.AddScoped<IAggregatorPagesManager, AggregatorPagesManager>();
        }
    }
}
