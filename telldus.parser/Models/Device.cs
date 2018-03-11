namespace telldusconf.Models
{
    public class Device
    {
        [ObjectKey("parameters")]
        public DeviceParams Parameters { get; set; }
    }
}