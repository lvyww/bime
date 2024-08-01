using System.ServiceModel;

namespace bime
{
    [ServiceContract]
    internal interface IMMService
    {
        [OperationContract]
        void ReloadMB();
        [OperationContract]
        void ShowConfigWin();
        [OperationContract]
        void ChangeMB(string mb);

        [OperationContract]
        void AddCi();
    }
}