using Microsoft.AspNetCore.Mvc;
using SanaAssessment.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SanaAssessment.Web.ViewComponents
{
    [ViewComponent(Name = "StorageConfiguration")]
    public class StorageConfiguration : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string storageSelected)
        {
            if (string.IsNullOrEmpty(storageSelected))
            {
                storageSelected = HttpContext.Session.Get<string>(SessionExtensions.SessionKeyStorageType);
            }
            List<SelectListItem> opciones = new List<SelectListItem>()
            {
                new SelectListItem{ Value="SQL",Text="Sql Server",Selected=storageSelected=="SQL"?true:false},
                new SelectListItem{ Value="InMem",Text="In Memory",Selected=storageSelected=="InMem"?true:false},
            };            
            ViewData["ActualStorage"] = storageSelected; 
            return View("Default", opciones);
        }
    }
}
