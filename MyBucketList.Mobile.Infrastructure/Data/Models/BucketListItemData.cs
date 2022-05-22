using SQLite;

namespace MyBucketList.Mobile.Infrastructure.Data.Models
{
    [Table("bucketListItem")]
    public class BucketListItemData
    {
        [PrimaryKey]
        [AutoIncrement]
        public string Id { get; set; }

        public string Title { get; set; }
    }
}
