﻿using DomainModel;
using ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterface
{
    public interface IStoreServerService
    {
        List<StoreModel> GetStoresDetails();

        List<StoreServerModel> GetStoresServerDetails();

        Int64 GenerateServerServiceStatusBatch(StoreServerModel storeServerDetails);


        bool UpdateServerServiceStatusBatch(List<StoreServerModel> ServerStatusDetails);

        List<ServerServiceStatus> GetWindowsServiceDetails();

        Int64 GenerateWindowsServiceStatusBatch(StoreServerModel storeServerDetails);
    }
}
