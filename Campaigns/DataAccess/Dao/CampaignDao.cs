using plannerBackEnd.Campaigns.DataAccess.Entities;
using plannerBackEnd.Common.sqlTools;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using plannerBackEnd.Campaigns.Domain.DomainObjects;
using plannerBackEnd.Common.Filters.DomainObjects;

namespace plannerBackEnd.Tasks.DataAccess.Dao
{
    public class CampaignDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public CampaignDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // --------------------------------------------------------------------------------------------

        public CampaignEntity Get(int requestedId)
        {
            string query = "SELECT * FROM campaigns WHERE id = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);

            CampaignEntity returnRow = new CampaignEntity();

            PropertyInfo[] properties = typeof(CampaignEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // --------------------------------------------------------------------------------------------

        public CampaignEntity GetByUserId(int requestedUserId)
        {
            string query = "SELECT * FROM campaigns WHERE startdate <= CURDATE() AND CURDATE() <= enddate AND userid = " + requestedUserId;

            DataRow dataRow = sqlTools.GetDataRow(query);

            CampaignEntity returnRow = new CampaignEntity();

            PropertyInfo[] properties = typeof(CampaignEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        //----------------------------------------------------------------------------------------------------
        public ReferredUserEntity GetList(BaseFilterRequest filter)
        {
            ReferredUserEntity filterResponse = new ReferredUserEntity();
            List<ReferredUserItemEntity> filterResponseList = new List<ReferredUserItemEntity>();

            string query = @"SELECT UP.firstname, UP.lastname, 
                                (SELECT name from schools S WHERE S.id = schoolid) AS `school`, 
                                (SELECT name from faculties F WHERE F.id = facultyid) AS `faculty`,
                                    IF((SELECT SUM(minutes) FROM tasks T WHERE T.userid = UP.id) > 600, 'Active', 'NonActive') AS `active`,
                                    UP.major, UP.email FROM userprofile UP WHERE
                                    UP.referredbycode = (SELECT campaigncode FROM campaigns WHERE userid = @UserId);";

            DataTable dataTable = sqlTools.GetTable(query, new Dictionary<string, object> { { "@UserId", filter.UserId } });

            if (dataTable != null)
            {
                filterResponseList = sqlTools.ConvertDataTable<ReferredUserItemEntity>(dataTable);
            }

            filterResponse.ReferredUsers = filterResponseList;

            return filterResponse;
        }

        //----------------------------------------------------------------------------------------------------
        public List<CampaignEntity> CreateList(List<CampaignEntity> campaignEntities)
        {
            List<CampaignEntity> filterResponse = new List<CampaignEntity>();

            string query = @"INSERT INTO campaigns (
                            userid,
                            campaigncode,
                            startdate,
                            enddate) 

                            VALUES";

            foreach (CampaignEntity campaignEntity in campaignEntities)
            {
                query += "(" + campaignEntity.UserId + ",'" + campaignEntity.CampaignCode + "','" + campaignEntity.StartDate + "','" + campaignEntity.EndDate + "')";
                query += ",";
            }

            query = query.Remove(query.Length - 1);
            query += ";";

            DataTable dataTable = sqlTools.GetTable(query);

            if (dataTable != null)
            {
                filterResponse = sqlTools.ConvertDataTable<CampaignEntity>(dataTable);
            }

            return filterResponse;
        }

    }
}