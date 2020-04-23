using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Common;
using MAVN.LykkeType.LykkeService.Domain.Repositories;

namespace MAVN.LykkeType.LykkeService.AzureRepositories
{
    public class LykkeServiceRepository : ILykkeServiceRepository
    {
        private readonly INoSQLTableStorage<LykkeServiceEntity> _tableStorage;

        public LykkeServiceRepository(INoSQLTableStorage<LykkeServiceEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
    }
}
