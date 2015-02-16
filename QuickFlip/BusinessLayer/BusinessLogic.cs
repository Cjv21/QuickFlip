using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuickFlip.DataAccessLayer;
using QuickFlip.Models;

namespace QuickFlip.BusinessLayer
{
    public class BusinessLogic
    {
        public static Community GetCommunityByCommunityId(int id)
        {
            DataAccess da = new DataAccess();

            Community comm = da.GetCommmunityByCommunityId(id);

            da.Dispose();

            return comm;
        }
    }
}