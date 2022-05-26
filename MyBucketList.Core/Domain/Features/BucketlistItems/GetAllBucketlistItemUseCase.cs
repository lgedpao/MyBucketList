using AutoMapper;
using MediatR;
using MyBucketList.Core.Domain.Entities;
using MyBucketList.Core.Domain.Features.BucketlistItems.Models;
using MyBucketList.Core.Interfaces.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyBucketList.Core.Domain.Features.BucketlistItems
{
    public class GetAllBucketlistItemUseCase : IRequestHandler<GetAllBucketlistItemRequest, IList<BucketlistItem>>
    {
        private readonly IBucketlistItemRepository _bucketListItemRepository;

        public GetAllBucketlistItemUseCase(IBucketlistItemRepository bucketListItemRepository)
        {
            _bucketListItemRepository = bucketListItemRepository;
        }

        public Task<IList<BucketlistItem>> Handle(GetAllBucketlistItemRequest request, CancellationToken cancellationToken)
        {
            return _bucketListItemRepository.Get();
        }
    }
}
