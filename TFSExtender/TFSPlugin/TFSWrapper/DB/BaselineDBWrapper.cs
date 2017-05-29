using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TFSWrapper.Model;

namespace TFSWrapper.DB
{
    public class BaselineDBWrapper
    {
        string connString = "Data Source=IE3BLT2X8SRC2\\SQLEXPRESS;Initial Catalog=TFS_Baseline;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public void InsertRecords(BaselineInfo baselineInfo, List<WorkItemInfo> lstWorkItemInfos)
        {
            
            CreateBaseline(baselineInfo);

            LinkWorkItemsToBaseLine(baselineInfo.BaselineId, lstWorkItemInfos, connString);


        }

        public List<BaselineInfo> GetAllBaselineInfos()
        {
            string query = $"Select * from Baseline";
            var baselineInfos = new List<BaselineInfo>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand(query))
                {

                    comm.Connection = conn;

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                var baseline = new BaselineInfo();
                                baseline.BaselineId = (int) reader["Id"];
                                baseline.BaselineName = (string) reader["Name"];
                                baseline.CreatedBy = (string) reader["CreatedBy"];
                                baseline.CreatedDate =  reader.GetDateTime(reader.GetOrdinal("CreatedDate"));

                                baselineInfos.Add(baseline);
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        // do something with the exception
                        // don't hide it
                    }
                }
            }

            return baselineInfos;
        }

        public List<WorkItemInfo> GetWorkItemsPerBaselineInfos(int baselineInfoId)
        {
            string query = $"Select * from BaselinedWorkItems where BaselineId = {baselineInfoId}";
            var workItemInfos = new List<WorkItemInfo>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand(query))
                {

                    comm.Connection = conn;

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                var workItemInfo = new WorkItemInfo();
                                workItemInfo.Id = int.Parse(((string) reader["Id"]).Split('_')[0]);
                                workItemInfo.Revision = (int) reader["RevisionId"];

                                workItemInfos.Add(workItemInfo);
                            }
                        }
                    }
                    catch (SqlException e)
                    {
                        // do something with the exception
                        // don't hide it
                    }
                }
            }

            return workItemInfos;
        }

        private void CreateBaseline(BaselineInfo baselineInfo)
        {
            string cmdString =
                $"INSERT INTO baseline (id,Name,CreatedBy, CreatedDate) VALUES ({baselineInfo.BaselineId}, '{baselineInfo.BaselineName}', '{baselineInfo.CreatedBy}','{baselineInfo.CreatedDate}')";
            InsertCommand(cmdString);
        }

        private void LinkWorkItemsToBaseLine(int baseLineId, List<WorkItemInfo> lstWorkItemInfos, string connString)
        {
            foreach (var workItem in lstWorkItemInfos)
            {
                var cmdString =
                    $"INSERT INTO baselinedWorkItems (Id,baselineId,RevisionId) VALUES ('{workItem.Id +"_"+ baseLineId}', {baseLineId}, {workItem.Revision})";
                InsertCommand(cmdString);
            }
        }

        private void InsertCommand(string cmdString)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = cmdString;
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        // do something with the exception
                        // don't hide it
                    }
                }
            }
        }
    }
}