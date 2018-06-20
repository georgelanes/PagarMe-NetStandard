namespace PagarMe.Enumeration
{
    public enum PostbackDeliveryStatus
    {
        [Base.EnumValue("success")] Success,
        [Base.EnumValue("processing")] Processing,
        [Base.EnumValue("failed")] Failed
    }
}
