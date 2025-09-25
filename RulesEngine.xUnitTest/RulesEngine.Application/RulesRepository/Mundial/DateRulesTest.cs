namespace RulesEngine.xUnitTest.RulesRepositoryTest.Mundial
{
    public class RulesTest
    {
        //    private InvoiceToCheck _Entitiy = new(
        //        radNumber: "CMVIQ034000000001930",
        //        ipsNit: "811017810",
        //        moduleName: "Prestadores",
        //        soatNumber: string.Empty,
        //        licensePlate: string.Empty,
        //        victimId: string.Empty,
        //        eventDate: Date.Create("2028-02-25T17:21:51Z"),
        //        deathDate: Date.Create("2025-02-25T17:21:51Z"),
        //        claimDate: Date.Create("2022-02-25T17:21:51Z"),
        //        invoiceDate: Date.Create("2024-02-25T17:21:51Z"),
        //        incomeDate: Date.Create("2027-02-25T17:21:51Z"),
        //        egressDate: Date.Create("2026-02-25T17:21:51Z"),
        //        primaryTransportationDate: Date.Create("2025-02-25T17:21:51Z"),
        //        invoiceMAOSDate: Date.Create("2024-02-25T17:21:51Z"),
        //        documentType: "CC",
        //        samePlateDifferentEventNumber: 1,
        //        samePlateForMotorcycleDifferentEventNumber: 0,
        //        sameVictimIdDifferentEventNumber: 0,
        //        ipsNitFurips: "123",
        //        invoiceNumberF1: "123"
        //    );

        //    private async Task<int> ExecuteRule(Type rule)
        //    {
        //        // Load rules
        //        var ruleRepository = new RuleRepository();
        //        ruleRepository.Load(r => r.From(rule));

        //        // Compile rules
        //        var factory = ruleRepository.Compile();

        //        //Create a working session
        //        var session = factory.CreateSession();

        //        //Insert facts into rules engine's memory
        //        session.Insert(_Entitiy);

        //        //Fire  rule
        //        var qRules = session.Fire();

        //        return await Task.FromResult(qRules);
        //    }

        //    [Fact]
        //    public async Task EventDateGreaterClaimDateRule_3_Test()
        //    {
        //        // Arrange
        //        _Entitiy.EventDate = Date.Create("2026-02-25T17:21:51Z");
        //        _Entitiy.ClaimDate = Date.Create("2024-01-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(EventDateGreaterClaimDateRule_3));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la fecha de ocurrencia del evento es posterior a la fecha de aviso de la reclamación, lo que conlleva a la devolución de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task InvoiceDateGreaterClaimDate_4_Test()
        //    {
        //        // Arrange
        //        _Entitiy.InvoiceDate = Date.Create("2026-01-25T17:21:51Z");
        //        _Entitiy.ClaimDate = Date.Create("2024-02-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(InvoiceDateGreaterClaimDate_4));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la fecha de factura es posterior a la fecha de aviso de la reclamación, lo que conlleva a la devolución de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task IncomeDateGreaterEgressDate_5_Test()
        //    {
        //        // Arrange
        //        _Entitiy.IncomeDate = Date.Create("2026-01-25T17:21:51Z");
        //        _Entitiy.EgressDate = Date.Create("2024-02-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(IncomeDateGreaterEgressDate_5));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la fecha de ingreso es posterior a la fecha de egreso de la reclamación, lo que conlleva a la devolución de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task EgressDateGreaterInvoiceDate_6_Test()
        //    {
        //        // Arrange
        //        _Entitiy.EgressDate = Date.Create("2026-01-25T17:21:51Z");
        //        _Entitiy.InvoiceDate = Date.Create("2024-02-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(EgressDateGreaterInvoiceDate_6));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la fecha de egreso es posterior a la fecha de factura, lo que conlleva a la devolución de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task EventDateGreaterIncomeDate_7_Test()
        //    {
        //        // Arrange
        //        _Entitiy.EventDate = Date.Create("2026-01-25T17:21:51Z");
        //        _Entitiy.IncomeDate = Date.Create("2024-02-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(EventDateGreaterIncomeDate_7));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la fecha de ocurrencia del evento es posterior a la fecha de ingreso, lo que conlleva a la devolución de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task EgressDateGreaterInvoiceDate_8_Test()
        //    {
        //        // Arrange
        //        _Entitiy.EgressDate = Date.Create("2026-01-25T17:21:51Z");
        //        _Entitiy.InvoiceDate = Date.Create("2024-02-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(EgressDateGreaterInvoiceDate_8));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la fecha de egreso es posterior a la fecha de factura, lo que conlleva a la devolución de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task PrimaryTransportationDateGreaterInvoiceDate_9_Test()
        //    {
        //        // Arrange
        //        _Entitiy.PrimaryTransportationDate = Date.Create("2026-01-25T17:21:51Z");
        //        _Entitiy.InvoiceDate = Date.Create("2024-02-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(PrimaryTransportationDateGreaterInvoiceDate_9));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la Fecha de trasporte primario es posterior a la fecha de factura, lo que conlleva a la devolución de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task EventDateGreaterPrimaryTransportationDate_10_Test()
        //    {
        //        // Arrange
        //        _Entitiy.EventDate = Date.Create("2026-02-25T17:21:51Z");
        //        _Entitiy.PrimaryTransportationDate = Date.Create("2024-01-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(EventDateGreaterPrimaryTransportationDate_10));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la fecha de ocurrencia del evento es posterior a la fecha de trasporte primario, lo que conlleva a la devolución de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task InvoiceMAOSDateGreaterClaimDate_43_Test()
        //    {
        //        // Arrange
        //        _Entitiy.InvoiceMAOSDate = Date.Create("2026-02-25T17:21:51Z");
        //        _Entitiy.ClaimDate = Date.Create("2024-01-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(InvoiceMAOSDateGreaterClaimDate_43));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la fecha de factura proveedor MAOS es posterior a la fecha de aviso de la reclamación, lo que conlleva a la devolución de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task EventDateGreaterDeathDateRule_48_Test()
        //    {
        //        // Arrange
        //        _Entitiy.EventDate = Date.Create("2026-02-25T17:21:51Z");
        //        _Entitiy.DeathDate = Date.Create("2024-01-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(EventDateGreaterDeathDateRule_48));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la fecha evento/accidente es posterior a la fecha en caso de muerte, lo que conlleva a la devolución de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task DeathDateGreaterClaimDateRule_49_Test()
        //    {
        //        // Arrange
        //        _Entitiy.DeathDate = Date.Create("2026-01-25T17:21:51Z");
        //        _Entitiy.ClaimDate = Date.Create("2025-02-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(DeathDateGreaterClaimDateRule_49));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la fecha en caso de muerte es posterior a la fecha de aviso de la reclamación, lo que conlleva a la devolución de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task DeathDateYearAgoRule_50_Test()
        //    {
        //        // Arrange
        //        _Entitiy.DeathDate = Date.Create("2026-01-25T17:21:51Z");
        //        _Entitiy.EventDate = Date.Create("2024-02-25T17:21:51Z");

        //        // Act
        //        var qRules = await ExecuteRule(typeof(DeathDateYearAgoRule_50));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("DenyClaim", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("Permite validar si la fecha en caso de muerte ocurrió un año después de la fecha evento/accidente, lo que conlleva a la objeción de la reclamación", _Entitiy.Alerts.First().AlertDescription);
        //    }
    }
}