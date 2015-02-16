using QuickFlip.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.HtmlControls;

namespace QuickFlip.DataAccessLayer
{
    public class DataAccess : IDisposable
    {
        private SqlConnection Connection;

        #region Constructor/Dispose

        public DataAccess()
        {
            Connection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            try 
            {
                Connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            } 
        }

        public void Dispose()
        {
            try
            {
                Connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Community

        public Community GetCommmunityByCommunityId(int communityId)
        {
            try
            {
                // form query
                SqlCommand command = new SqlCommand(
                    "SELECT * FROM [Community] WHERE CommunityId = @CommunityId", Connection);

                // add parameters
                command.Parameters.AddWithValue("@CommunityId", communityId);

                // execute
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Community comm = new Community
                    {
                        CommunityId = Convert.ToInt32(reader["CommunityId"]),
                        CommunityName = Convert.ToString(reader["CommunityName"]),
                        City = Convert.ToString(reader["City"]),
                        State = Convert.ToString(reader["State"]),
                        DefaultMeetingLocation = Convert.ToString(reader["DefaultMeetingLocation"]),
                        Logo = new HtmlImage 
                        {
                            Src = "/Images/" + (CommunityAbbrev)communityId + ".png"
                        }
                    };

                    reader.Close();

                    return comm;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        #endregion
    }
}