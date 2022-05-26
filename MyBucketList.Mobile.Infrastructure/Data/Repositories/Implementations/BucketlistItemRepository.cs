using AutoMapper;
using MyBucketList.Core.Domain.Entities;
using MyBucketList.Core.Interfaces.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyBucketList.Mobile.Infrastructure.Data.Repositories.Implementations
{
    public class BucketlistItemRepository : Repository<BucketlistItem>, IBucketlistItemRepository
    {
        public BucketlistItemRepository(Lazy<DatabaseConnection> lazyDBConnection) : base(lazyDBConnection)
        {
        }
    }
}
