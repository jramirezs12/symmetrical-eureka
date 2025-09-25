namespace RulesEngine.xUnitTest.RulesEngine.Application.RulesRepository.Mundial
{
    public class InvestigationRulesTest
    {
        //    private readonly InvoiceToCheck _Entitiy = new()
        //    {
        //        RadNumber = "CMVIQ034000000001930",
        //        IpsNit = "811017810",
        //        ModuleName = "Prestadores",
        //        SoatNumber = string.Empty,
        //        LicensePlate = string.Empty,
        //        VictimId = string.Empty,
        //        EventDate = Date.Create("2028-02-25T17:21:51Z"),
        //        DeathDate = Date.Create("2025-02-25T17:21:51Z"),
        //        ClaimDate = Date.Create("2022-02-25T17:21:51Z"),
        //        InvoiceDate = Date.Create("2024-02-25T17:21:51Z"),
        //        IncomeDate = Date.Create("2027-02-25T17:21:51Z"),
        //        EgressDate = Date.Create("2026-02-25T17:21:51Z"),
        //        PrimaryTransportationDate = Date.Create("2025-02-25T17:21:51Z"),
        //        InvoiceMAOSDate = Date.Create("2024-02-25T17:21:51Z"),
        //        DocumentType = "CC",
        //        SamePlateDifferentEventNumber = 0,
        //        SamePlateForMotorcycleDifferentEventNumber = 0,
        //        SameVictimIdDifferentEventNumber = 0,
        //        IpsNitFurips = "123",
        //        InvoiceNumberF1 = "123"
        //    };

        //    private async Task<int> ExecuteRule(Type rule)
        //    {
        //        //Register files
        //        await RegisterFilesInfo();

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

        //    private async Task RegisterFilesInfo()
        //    {
        //        /*
        //        var fraudulentIps = FraudulentIpsManage.Create();
        //        _Entitiy.FraudulentIps = fraudulentIps.FirstOrDefault(x => x.IpsNit == _Entitiy.IpsNit);

        //        //Register Ips investigation
        //        var ipsInvestigation = IpsInvestigationManage.Create();
        //        _Entitiy.IpsInvestigation = ipsInvestigation.FirstOrDefault(x => x.IpsNit == _Entitiy.IpsNit);

        //        //Atypical Events
        //        var atypicalEvent = AtypicalEventManage.Create();
        //        _Entitiy.AtypicalEvent = atypicalEvent.Where(x => x.VictimId == _Entitiy.VictimId
        //                                                                || x.Policy == _Entitiy.SoatNumber
        //                                                                || x.LicensePlate == _Entitiy.LicensePlate).ToList();

        //        //Register Catastrophic event 
        //        var catastrophicEvent = CatastrophicEventManage.Create();
        //        _Entitiy.CatastrophicEvent = catastrophicEvent.Where(x => x.Policy == _Entitiy.SoatNumber).ToList();

        //        await Task.CompletedTask;
        //        */

        //        await Task.CompletedTask;
        //    }

        //    [Fact]
        //    public async Task FraudulentIpsRule_11_Test()
        //    {
        //        // Arrange
        //        _Entitiy.IpsNit = "901266096";

        //        // Act
        //        var qRules = await ExecuteRule(typeof(FraudulentIpsRule_11));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("Alert", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("El NIT de la IPS en la tabla de origen es igual al NIT de la IPS en la tabla de consulta y registra como acción \"Enviar a Investigar\"", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task FraudulentIpsWithoutEventInvestigationRule_12_Test()
        //    {
        //        // Arrange
        //        _Entitiy.IpsNit = "901266096";

        //        // Act
        //        var qRules = await ExecuteRule(typeof(FraudulentIpsWithoutEventInvestigationRule_12));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("SendToInvestigation", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("El NIT de la IPS en la tabla de origen es igua al NIT de la IPS en la tabla de consulta y registra como acción \"Enviar a Investigar\" y el siniestro no tiene resultado de investigación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task FraudulentIpsToObjectionRule_13_Test()
        //    {
        //        // Arrange
        //        _Entitiy.IpsNit = "900428864";

        //        // Act
        //        var qRules = await ExecuteRule(typeof(FraudulentIpsToObjectionRule_13));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("ClaimObjection", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("El NIT de la IPS en la tabla de origen es igua al NIT de la IPS en la tabla de consulta y registra como acción \"Objetar todo\"", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task IpsInvestigationRule_14_Test()
        //    {
        //        // Arrange
        //        _Entitiy.IpsNit = "824001252";

        //        // Act
        //        var qRules = await ExecuteRule(typeof(IpsInvestigationRule_14));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("Alert", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("El NIT de la IPS en la tabla de origen es igua al NIT de la IPS en la tabla de consulta", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task IpsInvestigationWithoutEventInvestigationRule_15_Test()
        //    {
        //        // Arrange
        //        _Entitiy.IpsNit = "824001252";

        //        // Act
        //        var qRules = await ExecuteRule(typeof(IpsInvestigationWithoutEventInvestigationRule_15));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("SendToInvestigation", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("El NIT de la IPS en la tabla de origen es igua al NIT de la IPS en la tabla de consulta  y el siniestro no tiene resultado de investigación", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task LicensePlateRule_16_Test()
        //    {
        //        // Arrange
        //        _Entitiy.RadNumber = "CMVIQ034000000001927";
        //        _Entitiy.LicensePlate = "JXT111";

        //        // Act
        //        var qRules = await ExecuteRule(typeof(LicensePlateRule_16));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("Alert", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal($"La placa presenta la siguiente alerta: Esta es mi prueba", _Entitiy.Alerts.First().AlertMessage);
        //    }

        //    [Fact]
        //    public async Task LicensePlateWithoutEventInvestigationRule_17_Test()
        //    {
        //        // Arrange
        //        _Entitiy.RadNumber = "CMVIQ034000000001927";
        //        _Entitiy.LicensePlate = "JXT111";

        //        // Act
        //        var qRules = await ExecuteRule(typeof(LicensePlateWithoutEventInvestigationRule_17));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("SendToInvestigation", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("La placa en la tabla de origen es igual a la placa en la tabla de consulta y no tiene resultado de investigación asociado al siniestro", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task VictimIdRule_18_Test()
        //    {
        //        // Arrange
        //        _Entitiy.VictimId = "1140844111";
        //        _Entitiy.AtypicalEvent = [];
        //        _Entitiy.AtypicalEvent.Add(new()
        //        {
        //            VictimId = "1140844111",
        //            AlertDescription = "Prueba",
        //            LicensePlate = "BJK007",
        //            SoatNumber = "2341235"
        //        });

        //        // Act
        //        var qRules = await ExecuteRule(typeof(VictimIdRule_18));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("Alert", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("El número de documento de identidad de la víctima en la tabla de origen es igual a el número de documento de identidad de la víctima en la tabla de consulta", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task VictimIdWithoutEventInvestigationRule_19_Test()
        //    {
        //        // Arrange
        //        _Entitiy.VictimId = "1140844111";
        //        _Entitiy.AtypicalEvent = [];
        //        _Entitiy.AtypicalEvent.Add(new()
        //        {
        //            VictimId = "1140844111",
        //            AlertDescription = "Prueba",
        //            LicensePlate = "BJK007",
        //            SoatNumber = "2341235"
        //        });

        //        // Act
        //        var qRules = await ExecuteRule(typeof(VictimIdWithoutEventInvestigationRule_19));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("SendToInvestigation", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("El número de documento de identidad de la víctima en la tabla de origen es igual a el número de documento de identidad de la víctima en la tabla de consulta y no tiene resultado de investigación asociado al siniestro", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task SoatRule_20_Test()
        //    {
        //        // Arrange
        //        _Entitiy.SoatNumber = "83733715";

        //        // Act
        //        var qRules = await ExecuteRule(typeof(SoatRule_20));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("Alert", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("El número de póliza SOAT en la tabla de origen es igual a la placa en la tabla de consulta", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task SoatWithoutEventInvestigationRule_21_Test()
        //    {
        //        // Arrange
        //        _Entitiy.SoatNumber = "83733715";

        //        // Act
        //        var qRules = await ExecuteRule(typeof(SoatWithoutEventInvestigationRule_21));

        //        // Assert
        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("SendToInvestigation", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("El número de póliza SOAT en la tabla de origen es igual a la placa en la tabla de consulta y no tiene resultado de investigación asociado al siniestro", _Entitiy.Alerts.First().AlertDescription);
        //    }

        //    [Fact]
        //    public async Task IpsCatastrophicEventRule_22_Test()
        //    {
        //        // Arrange
        //        _Entitiy.SoatNumber = "85328241";

        //        // Act
        //        var qRules = await ExecuteRule(typeof(IpsCatastrophicEventRule_22));

        //        Assert.Equal(1, qRules);
        //        Assert.Single(_Entitiy!.Alerts);
        //        Assert.Equal("SendToInvestigation", _Entitiy.Alerts.First().AlertAction);
        //        Assert.Equal("La póliza en la tabla de origen es igual a la póliza en la tabla de consulta y no tiene resultado de investigación asociado al siniestro", _Entitiy.Alerts.First().AlertDescription);
        //    }
    }
}
