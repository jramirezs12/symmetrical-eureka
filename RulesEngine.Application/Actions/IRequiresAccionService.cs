using RulesEngine.Application.Actions.Common;

namespace RulesEngine.Application.Actions
{
    public interface IRequiresAccionService
    {
        void SetAccionService(IActionService service);
    }
}
