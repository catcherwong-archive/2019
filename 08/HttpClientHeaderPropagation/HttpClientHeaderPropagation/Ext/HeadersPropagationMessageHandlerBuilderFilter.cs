namespace HttpClientHeaderPropagation.Ext
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Http;
    using System;

    public class HeadersPropagationMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;        
        
        public HeadersPropagationMessageHandlerBuilderFilter(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            return (builder) =>
            {
                next(builder);

                builder.AdditionalHandlers.Add(new HeadersPropagationDelegatingHandler(httpContextAccessor));
            };
        }

    }
}
