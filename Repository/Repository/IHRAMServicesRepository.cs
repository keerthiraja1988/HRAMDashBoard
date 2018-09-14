﻿using DomainModel;
using Insight.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IHRAMServicesRepository
    {

      
        [Sql("P_UpdateWindowsServiceHeartBeat")]
        bool UpdateServerServiceStatusBatch(WindowsServiceStatus windowsServiceStatus);
    }
}
