using System;

namespace Example.Service
{
    public interface IServices
    {
        T CreateService<T>() where T : class;

        /// <summary>
        /// AddScoped时单独开辟新生命周期
        /// </summary>
        /// <returns></returns>
        IServiceScope CreateScope();
    }
    public class Services : IServices
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public Services(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public T CreateService<T>() where T : class
        {
            //httpContextAccessor.HttpContext.RequestServices.GetRequiredService
            return httpContextAccessor.HttpContext.RequestServices.GetService<T>();
        }

        public IServiceScope CreateScope() 
        {
            return httpContextAccessor.HttpContext.RequestServices.CreateScope();
        }
    }
}
