using System.ServiceModel;

namespace Sample.CoreWCF
{
    /// <summary>
    /// Defines the service contract. For the tests to work, we have to use the old
    /// System.ServiceModel attributes instead of the native CoreWCF attributes. CoreWCF supports
    /// both which is why this works.
    /// </summary>
    [System.ServiceModel.ServiceContract]
    public interface IService
    {
        [System.ServiceModel.OperationContract]
        string GetData(int value);

        [System.ServiceModel.OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    /// <summary>
    /// Service implementation
    /// </summary>
    public class Service : IService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }

    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
