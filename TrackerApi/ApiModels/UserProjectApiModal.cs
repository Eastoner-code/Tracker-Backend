namespace TrackerApi.ApiModels
{
    public class UserProjectApiModal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }

        public virtual UserApiModel User { get; set; }
        public virtual ProjectApiModel Project { get; set; }
    }
}