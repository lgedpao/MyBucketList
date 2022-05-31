using SQLite;

namespace MyBucketList.Core.Domain.Entities
{
    [Table("bucketListItem")]
    public class BucketlistItem
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
