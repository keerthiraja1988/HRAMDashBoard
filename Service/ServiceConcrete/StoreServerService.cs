﻿using DomainModel;
using Insight.Database;
using Repository;
using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceConcrete
{
    public class StoreServerService : IStoreServerService
    {
        IStoreServerRepository _IStoreServerRepository;

        public StoreServerService()
        {

            SqlInsightDbProvider.RegisterProvider();
            //string sqlConnection = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;
            string sqlConnection = "Data Source=.;Initial Catalog=HRAMDashBoard;Integrated Security=True";
            DbConnection c = new SqlConnection(sqlConnection);
            _IStoreServerRepository = c.As<IStoreServerRepository>();
        }

        public List<StoreModel> GetStoresDetails()
        {
            return this._IStoreServerRepository.GetStoresDetails();
        }


        public List<StoreServerModel> GetStoresServerDetails()
        {
            return this._IStoreServerRepository.GetStoresServerDetails();
        }

        public Int64 GenerateServerServiceStatusBatch(StoreServerModel storeServerDetails)
        {
            return this._IStoreServerRepository.GenerateServerServiceStatusBatch(storeServerDetails);
        }

       public bool UpdateServerServiceStatusBatch(List<StoreServerModel> ServerStatusDetails)
        {
            return this._IStoreServerRepository.UpdateServerServiceStatusBatch(ServerStatusDetails);
        }



        public List<ServerServiceStatus> GetWindowsServiceDetails()
        {
            return this._IStoreServerRepository.GetWindowsServiceDetails();
        }

        public Int64 GenerateWindowsServiceStatusBatch(StoreServerModel storeServerDetails)
        {
            return this._IStoreServerRepository.GenerateWindowsServiceStatusBatch(storeServerDetails);
        }
    }
}
