using FindRoommate.Models.Advert;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindRoommate.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IAdvertRepository repository;

        public NavigationMenuViewComponent(IAdvertRepository repository)
        {
            this.repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Adverts
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x));
        }
    }
}
