using MobileClaims.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MobileClaims.Core.Services
{
    public class MenuService : ApiClientHelper, IMenuService
    {
        public IList<MenuItem> MenuItems { get; private set; }

        public async Task<IEnumerable<MenuItem>> GetMenuAsync(string planMemberId)
        {
            if (string.IsNullOrWhiteSpace(planMemberId))
            {
                throw new ArgumentNullException(nameof(planMemberId));
            }

            var apiClient = new ApiClient<IEnumerable<MenuItem>>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL),
                HttpMethod.Get,
                $"{GSCHelper.GSC_SERVICE_BASE_URL_SUB}/api/planmember/{planMemberId}/Menu");

            MenuItems = (await ExecuteRequestWithRetry(apiClient)).ToList();

            return MenuItems;
        }

        public bool IsAnyClaimForAudit()
        {
            var myAlertsMenuLink = MenuItems.Select(x => x.Links).SelectMany(x => x)
                .FirstOrDefault(x => x.MenuItemRel == MenuItemRel.MyAlertsPage);

            var myAlertsMenu = MenuItems.FirstOrDefault(x => x.Links.Contains(myAlertsMenuLink));
            return myAlertsMenu?.Count > 0;
        }
    }
}