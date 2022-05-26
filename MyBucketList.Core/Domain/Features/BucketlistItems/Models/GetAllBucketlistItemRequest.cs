using MediatR;
using MyBucketList.Core.Domain.Entities;
using System.Collections.Generic;

namespace MyBucketList.Core.Domain.Features.BucketlistItems.Models
{
    public class GetAllBucketlistItemRequest : IRequest<IList<BucketlistItem>>
    {

    }
}
