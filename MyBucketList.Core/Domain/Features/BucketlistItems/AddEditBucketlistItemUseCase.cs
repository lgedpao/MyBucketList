using AutoMapper;
using MediatR;
using MyBucketList.Core.Domain.Entities;
using MyBucketList.Core.Domain.Features.BucketlistItems.Models;
using MyBucketList.Core.Interfaces.Data.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyBucketList.Core.Domain.Features.BucketlistItems
{
    public class AddEditBucketlistItemUseCase : IRequestHandler<AddEditBucketlistItemRequest, BucketlistItem>
    {
        private readonly IBucketlistItemRepository _bucketListItemRepository;
        private readonly IMapper _mapper;

        public AddEditBucketlistItemUseCase(IBucketlistItemRepository bucketListItemRepository, IMapper mapper)
        {
            _bucketListItemRepository = bucketListItemRepository;
            _mapper = mapper;
        }

        public async Task<BucketlistItem> Handle(AddEditBucketlistItemRequest request, CancellationToken cancellationToken)
        {
            if (request?.Id == 0)
            {
                var bucketlistItem = _mapper.Map<BucketlistItem>(request);
                await _bucketListItemRepository.InsertItem(bucketlistItem);

                return bucketlistItem;
            }
            else
            {
                var bucketlistItem = _mapper.Map<BucketlistItem>(request);
                await _bucketListItemRepository.UpdateItem(bucketlistItem);

                return bucketlistItem;
            }
        }
    }
}
