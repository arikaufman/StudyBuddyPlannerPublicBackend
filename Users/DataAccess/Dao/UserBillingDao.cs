using System.Collections.Generic;
using System.Data;
using System.Reflection;
using plannerBackEnd.Common.sqlTools;
using plannerBackEnd.Users.DataAccess.Entities;

namespace plannerBackEnd.Users.DataAccess.Dao
{
    public class UserBillingDao
    {
        private SqlConnection sqlConnection;
        private SqlTools sqlTools;

        //----------------------------------------------------------------------------------------------------
        public UserBillingDao(
            SqlConnection sqlConnection,
            SqlTools sqlTools
        )
        {
            this.sqlConnection = sqlConnection;
            this.sqlTools = sqlTools;
        }

        // --------------------------------------------------------------------------------------------

        public UserBillingEntity Get(int requestedId)
        {
            string query = "SELECT * FROM userbilling WHERE id = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);


            UserBillingEntity returnRow = new UserBillingEntity();

            PropertyInfo[] properties = typeof(UserBillingEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // --------------------------------------------------------------------------------------------

        public UserBillingEntity GetByUserId(int requestedId)
        {
            string query = "SELECT * FROM userbilling WHERE userid = " + requestedId;

            DataRow dataRow = sqlTools.GetDataRow(query);


            UserBillingEntity returnRow = new UserBillingEntity();

            PropertyInfo[] properties = typeof(UserBillingEntity).GetProperties();

            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        //----------------------------------------------------------------------------------------------------
        public UserBillingEntity Create(UserBillingEntity userBillingEntity)
        {
            string query = @"INSERT INTO userbilling (
                            userid,
                            stripecustomerid,
                            stripesubscriptionid,
                            stripepriceid,
                            stripecurrentperiodend,
                            stripestatus) 

                            VALUES(
                            @userid,
                            @stripecustomerid,
                            @stripesubscriptionid,
                            @stripepriceid,
                            @stripecurrentperiodend,
                            @stripestatus);  ";

            query += "SELECT * FROM userbilling WHERE id = LAST_INSERT_ID();";
            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@userid", userBillingEntity.UserId},
                {"@stripecustomerid", userBillingEntity.StripeCustomerId},
                {"@stripesubscriptionid", userBillingEntity.StripeSubscriptionId},
                {"@stripepriceid", userBillingEntity.StripePriceId},
                {"@stripecurrentperiodend", userBillingEntity.StripeCurrentPeriodEnd},
                {"@stripestatus", userBillingEntity.StripeStatus},
            });

            UserBillingEntity returnRow = new UserBillingEntity();
            PropertyInfo[] properties = typeof(UserBillingEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }

        // -----------------------------------------------------------------------------

        public UserBillingEntity Update(UserBillingEntity userBillingEntity)
        {
            UserBillingEntity oldUserBillingEntity = Get(userBillingEntity.Id);

            string query = @"UPDATE userbilling SET 
                            userid = @userid,
                            stripecustomerid = @stripecustomerid,
                            stripesubscriptionid = @stripesubscriptionid,
                            stripepriceid = @stripepriceid,
                            stripecurrentperiodend = @stripecurrentperiodend,
                            stripestatus = @stripestatus
                            WHERE id = @id; 

                SELECT * FROM userbilling WHERE id = @id;";

            DataRow dataRow = sqlTools.GetDataRow(query, new Dictionary<string, object>
            {
                {"@id", userBillingEntity.Id},
                {"@userid", userBillingEntity.UserId},
                {"@stripecustomerid", userBillingEntity.StripeCustomerId},
                {"@stripesubscriptionid", userBillingEntity.StripeSubscriptionId},
                {"@stripepriceid", userBillingEntity.StripePriceId},
                {"@stripecurrentperiodend", userBillingEntity.StripeCurrentPeriodEnd},
                {"@stripestatus", userBillingEntity.StripeStatus},

            });

            UserBillingEntity returnRow = new UserBillingEntity();
            PropertyInfo[] properties = typeof(UserBillingEntity).GetProperties();
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                property.SetValue(returnRow, dataRow.ItemArray[i]);
                i++;
            }

            return returnRow;
        }
    }
}