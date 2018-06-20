namespace PagarMe.Enumeration
{
    public enum PayableType
    {
        [Base.EnumValue("credit")] Credit,
        [Base.EnumValue("refund")] Refund,
        [Base.EnumValue("chargeback")] Chargeback
    }
}
