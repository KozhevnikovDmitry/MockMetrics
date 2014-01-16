namespace MockMetrics.Eating.MoqFake
{
    /// <summary>
    /// Meanings of Moq methods invocations
    /// </summary>
    public enum MoqInvoke
    {
        None,
        Mock,
        FakeWithoutOptions,
        FakeWithOptions,
        StubWithOptions,
        FakeProperty,
        FakeCallback,
        FakeException
    }
}