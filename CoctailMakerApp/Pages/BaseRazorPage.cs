using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace CoctailMakerApp.Pages
{
    public abstract class BaseRazorPage<TModel> : RazorPage<TModel>
    {
        protected Exception LoadingException { get; set; }
    }
}
