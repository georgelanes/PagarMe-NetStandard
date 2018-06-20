namespace PagarMe.Enumeration
{
    public enum BalanceOperationType
    {
        [Base.EnumValue("payable")] Payable,
        [Base.EnumValue("anticipation")] Anticipation,
        [Base.EnumValue("transfer")] Transfer
    }
}
