namespace RulesEngine.Application.Actions.Common
{
    public interface IActionService
    {
        void InsertarEnMongo(string radNumber, string mensaje);
        void LlamarAlertaWS(string radNumber);
    }
}
