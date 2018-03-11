namespace telldusconf.Models
{
    public class DeviceParameter
    {
        [Key("code")]
        public string Code { get; set; }

        [Key("house")]
        public string House { get; set; }

        [Key("unit")]
        public string Unit { get; set; }
    }
}