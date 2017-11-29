namespace TrekkingForCharity.Site.Web.Integrations.Doorbell
{
    public class DoorbellMessage
    {
        public DoorbellMessage(string name, string email, string message)
        {
            this.Name = name;
            this.Email = email;
            this.Message = message;
        }
        public string Email { get;}
        public string Name { get;}
        public string Message { get; }
    }
}