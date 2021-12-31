using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Auditing;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.Features;
using Volo.Abp.AspNetCore.Mvc.GlobalFeatures;
using Volo.Abp.AspNetCore.Mvc.Response;
using Volo.Abp.AspNetCore.Mvc.Uow;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;

[DependsOn(typeof(AbpAspNetCoreMvcModule))]
//[DependsOn(typeof(AbpAutofacModule))]
public class AppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<MvcOptions>(mvcOptions =>
        {
            var removeFiltes = mvcOptions.Filters.RemoveAll(f =>
            {
                if (f is ServiceFilterAttribute serviceFilter)
                {
                    return serviceFilter.IsReusable == false;
                }
                return false;
            });

            //mvcOptions.Filters.AddService(typeof(GlobalFeatureActionFilter));
            mvcOptions.Filters.AddService(typeof(AbpAuditActionFilter));
            mvcOptions.Filters.AddService(typeof(AbpNoContentActionFilter));
            //mvcOptions.Filters.AddService(typeof(AbpFeatureActionFilter));
            mvcOptions.Filters.AddService(typeof(AbpValidationActionFilter));
            //mvcOptions.Filters.AddService(typeof(AbpUowActionFilter));
            mvcOptions.Filters.AddService(typeof(AbpExceptionFilter));

            foreach (var filter in mvcOptions.Filters)
            {
                if (filter is ServiceFilterAttribute serviceFilter)
                {
                    if (serviceFilter.ServiceType == typeof(GlobalFeatureActionFilter))
                    {
                        var result = serviceFilter.IsReusable;
                    }
                }
            }

            //options.Filters.AddService(typeof(GlobalFeatureActionFilter));
            //options.Filters.AddService(typeof(AbpAuditActionFilter));
            //options.Filters.AddService(typeof(AbpNoContentActionFilter));
            //options.Filters.AddService(typeof(AbpFeatureActionFilter));
            //options.Filters.AddService(typeof(AbpValidationActionFilter));
            //options.Filters.AddService(typeof(AbpUowActionFilter));
            //options.Filters.AddService(typeof(AbpExceptionFilter));
        });

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.AutoModelValidation = false;
        });

        Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabledForGetRequests = false;
        });
    }

    public override void OnApplicationInitialization(
        ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseUnitOfWork();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseConfiguredEndpoints();
    }
}
