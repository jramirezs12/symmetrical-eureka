using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using NRules.Fluent.Dsl;
using RulesEngine.Application.Abstractions.Services;
using RulesEngine.Application.Actions;
using RulesEngine.Application.Builders;
using RulesEngine.Application.Common.Rule;
using RulesEngine.Domain.Invoices.Repositories;
using RulesEngine.Domain.Primitives;
using RulesEngine.Domain.RulesEngine.Repositories;
using RulesEngine.Domain.RulesEntities.Mundial.Entities;
using RulesEngineContracts.Mundial;
using System.Reflection;

namespace RulesEngine.Application.Invoices.UpdateInvoice;

public sealed class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, ErrorOr<InvoiceToCheckResponse>>
{
    private readonly IInvoiceRepository _IInvoiceRepository;
    private readonly IRuleEngineRepository _ruleEngineRepository;
    private readonly IDomainModelProvider _domainModelProvider;
    private readonly IActionServiceFactory _accionServiceFactory;
    private readonly IRuleSessionBuilder _sessionBuilder;
    private readonly IInvoiceToCheckBuilder _invoiceToCheckBuilder;
    private readonly IInvoiceToCheckBuilderFactory _builderFactory;

    public UpdateInvoiceCommandHandler(IInvoiceRepository invoiceRepository,
                                       IRuleEngineRepository ruleEngineRepository,
                                       IMemoryCache cache,
                                       IDomainModelProvider domainModelProvider,
                                       IActionServiceFactory accionServiceFactory,
                                       IRuleSessionBuilder sessionBuilder,
                                       IInvoiceToCheckBuilder invoiceToCheckBuilder,
                                       IInvoiceToCheckBuilderFactory builderFactory,
                                       IInvoiceRepository iInvoiceRepository)
    {
        _IInvoiceRepository = invoiceRepository;
        _ruleEngineRepository = ruleEngineRepository;
        _domainModelProvider = domainModelProvider;
        _accionServiceFactory = accionServiceFactory;
        _sessionBuilder = sessionBuilder;
        _invoiceToCheckBuilder = invoiceToCheckBuilder;
        _builderFactory = builderFactory;
        this._IInvoiceRepository = iInvoiceRepository;
    }

    public async Task<ErrorOr<InvoiceToCheckResponse>> Handle(UpdateInvoiceCommand command, CancellationToken cancellationToken)
    {
        // Paso 1 se crea el contexto base
        var baseContext = await _domainModelProvider.GetModelAsync(command.TenantName, command.RadNumber);

        //
        var builder = _builderFactory.ForTenant(command.TenantName);

        // Se llena con la información trida por cliente
        var domainModel = await builder.BuildAsync(command.RadNumber, command.ModuleName,command.Stage, command.TenantName);

        var ruleEngine = await _ruleEngineRepository.GetRulesCollectionByStage(command.Stage, command.TenantName);

        var accionService = _accionServiceFactory.CreateForTenant(command.TenantName);

        var reglasActivas = ruleEngine.Rules.Where(r => r.Active).ToList();

        // Enviar información especifica a cada regla
        var typificationMap = new Dictionary<string, string>(StringComparer.Ordinal);
        var hasPriorityMap = new Dictionary<string, bool>(StringComparer.Ordinal);
        
        var ruleTypes = ruleEngine.Rules!
        .Where(r => r.Active)
        .Select(r =>
        {
            var type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == r.ClassName
            && t.Namespace != null && t.Namespace.Contains(command.TenantName));

            if (type != null)
            {
                typificationMap[type.Name] = r.Typification ?? "Sin typification";
                hasPriorityMap[type.Name] = r.HasPriority;
            }

            return type;

        }).Where(t => t != null).Cast<Type>().ToList();

        if (domainModel is IRuleMetadataAware aware)
        {
            aware.TypificationMap = typificationMap;
            aware.HasPriorityMap = hasPriorityMap;

        }

        var session = _sessionBuilder.BuildSession(ruleTypes, out var matched, accionService);

        session.Insert(domainModel);
        session.Fire();

        await new RuleActionsResolver(domainModel, _IInvoiceRepository).ExecuteActions();
        return new InvoiceToCheckResponse(StatusCodes.Status200OK, "Reglas ejecutadas exitosamente", matched.Count, matched);
    }
}