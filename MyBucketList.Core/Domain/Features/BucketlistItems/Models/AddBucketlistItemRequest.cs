using MediatR;
using MyBucketList.Core.Domain.Entities;

namespace MyBucketList.Core.Domain.Features.BucketlistItems.Models
{
    public class AddEditBucketlistItemRequest : IRequest<BucketlistItem>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
