using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindRoommate.Infrastructure;

namespace FindRoommate.Models.AdvertImage
{
    public class EFAdvertImageRepository : IAdvertImageRepository
    {
        private readonly ApplicationDbContext context;

        public EFAdvertImageRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<AdvertImage> AdvertImages => context.AdvertImages;
    }
}
