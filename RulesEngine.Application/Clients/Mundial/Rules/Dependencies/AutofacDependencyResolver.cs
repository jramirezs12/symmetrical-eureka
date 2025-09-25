using Autofac;
using NRules.Extensibility;
using RulesEngine.Domain.Invoices.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Application.Clients.Mundial.Rules.Dependencies
{
    /// <summary>
    /// Dependency resolver that uses Autofac DI container.
    /// </summary>
    public class AutofacDependencyResolver : IDependencyResolver
    {
        private readonly IInvoiceRepository _InvoiceRepository;

        public AutofacDependencyResolver(IInvoiceRepository invoiceRepository)
        {
            _InvoiceRepository = invoiceRepository;
        }

        public object Resolve(IResolutionContext context, Type serviceType)
        {
            return _InvoiceRepository;
        }
    }
}
