using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindRoommate.Models.AdvertImage
{
    public interface IAdvertImageRepository
    {
        IQueryable<AdvertImage> AdvertImages { get; }
    }
}
