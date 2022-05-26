using AutoMapper;
using MyBucketList.Core.Domain.Entities;
using MyBucketList.Core.Domain.Features.BucketlistItems.Models;

namespace MyBucketList.Mobile.Infrastructure.Mappings
{
    public class BucketlistItemProfile : Profile
    {
        public BucketlistItemProfile()
        {
            CreateMap<AddEditBucketlistItemRequest, BucketlistItem>().ReverseMap();
        }
    }
}
