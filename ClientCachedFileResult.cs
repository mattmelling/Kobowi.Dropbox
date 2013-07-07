using System;
using System.Web;
using System.Web.Mvc;
using Orchard;
using Orchard.Services;

namespace Kobowi.Dropbox {
    /// <summary>
    /// Sets cache header so that result is cached by client
    /// </summary>
    public class ClientCachedFileResult : FileContentResult {
        private readonly WorkContext _workContext;
        private readonly TimeSpan _span;
        public ClientCachedFileResult(WorkContext workContext, TimeSpan span, byte[] fileContents, string contentType)
            : base(fileContents, contentType) {
            _workContext = workContext;
            _span = span;
        }
        protected override void WriteFile(HttpResponseBase response) {
            var clock = _workContext.Resolve<IClock>();
            response.Cache.SetCacheability(HttpCacheability.Private);
            response.Cache.SetExpires(clock.UtcNow.Add(_span));
            base.WriteFile(response);
        }
    }
}