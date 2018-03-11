namespace telldusconf.Models
{
    public class TelldusController
    {
        [Key("id")]
        public int Id { get; set; }

        [Key("type")]
        public int Type { get; set; }

        [Key("serial")]
        public string Serial { get; set; }
       
    }
}