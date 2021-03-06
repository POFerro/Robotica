//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
[assembly: global::System.Reflection.AssemblyProductAttribute("ArduinoGenericDistanceSensor")]
[assembly: global::System.Reflection.AssemblyTitleAttribute("ArduinoGenericDistanceSensor")]
[assembly: global::Microsoft.Dss.Core.Attributes.ServiceDeclarationAttribute(global::Microsoft.Dss.Core.Attributes.DssServiceDeclaration.Proxy, SourceAssemblyKey="ArduinoGenericDistanceSensor.Y2012.M01, Version=1.0.0.0, Culture=neutral, PublicK" +
    "eyToken=e3a6d1d8ea7297f8")]
[assembly: global::System.Security.SecurityTransparentAttribute()]
[assembly: global::System.Security.SecurityRulesAttribute(global::System.Security.SecurityRuleSet.Level1)]

namespace ArduinoGenericDistanceSensor.Proxy {
    
    
    [global::Microsoft.Dss.Core.Attributes.DataContractAttribute(Namespace="http://mrdsarduino.codeplex.com/2012/01/arduinogenericdistancesensor.html")]
    [global::System.Xml.Serialization.XmlRootAttribute(Namespace="http://mrdsarduino.codeplex.com/2012/01/arduinogenericdistancesensor.html", ElementName="ArduinoGenericDistanceSensorState")]
    public class ArduinoGenericDistanceSensorState : global::Microsoft.Dss.Core.IDssSerializable, global::System.ICloneable {
        
        public ArduinoGenericDistanceSensorState() {
        }
        
        private global::ArduinoGenericDistanceSensor.Sensors.Proxy.SensorsModels _SensorModel;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public global::ArduinoGenericDistanceSensor.Sensors.Proxy.SensorsModels SensorModel {
            get {
                return this._SensorModel;
            }
            set {
                this._SensorModel = value;
            }
        }
        
        private int _HardwareIdentifier;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Identifies the hardware port for the sensor.")]
        public int HardwareIdentifier {
            get {
                return this._HardwareIdentifier;
            }
            set {
                this._HardwareIdentifier = value;
            }
        }
        
        private global::Microsoft.Robotics.PhysicalModel.Proxy.Pose _Pose;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        [global::System.ComponentModel.DescriptionAttribute("Specifies the position and orientation of the sensor.")]
        public global::Microsoft.Robotics.PhysicalModel.Proxy.Pose Pose {
            get {
                return this._Pose;
            }
            set {
                this._Pose = value;
            }
        }
        
        private global::Arduino.Firmata.Types.Proxy.Pins _TriggerPin;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public global::Arduino.Firmata.Types.Proxy.Pins TriggerPin {
            get {
                return this._TriggerPin;
            }
            set {
                this._TriggerPin = value;
            }
        }
        
        /// <summary>
        ///Copies the data member values of the current ArduinoGenericDistanceSensorState to the specified target object
        ///</summary>
        ///<param name="target">target object (must be an instance of)</param>
        public virtual void CopyTo(Microsoft.Dss.Core.IDssSerializable target) {
            global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState typedTarget = ((global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState)(target));
            typedTarget._SensorModel = this._SensorModel;
            typedTarget._HardwareIdentifier = this._HardwareIdentifier;
            typedTarget._Pose = this._Pose;
            typedTarget._TriggerPin = this._TriggerPin;
        }
        
        /// <summary>
        ///Clones ArduinoGenericDistanceSensorState
        ///</summary>
        ///<returns>cloned value</returns>
        public virtual object Clone() {
            global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState target0 = new global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState();
            this.CopyTo(target0);
            return target0;
        }
        
        /// <summary>
        ///Serializes the data member values of the current ArduinoGenericDistanceSensorState to the specified writer
        ///</summary>
        ///<param name="writer">the writer to which to serialize</param>
        public virtual void Serialize(System.IO.BinaryWriter writer) {
            writer.Write(((int)(this._SensorModel)));
            writer.Write(this._HardwareIdentifier);
            ((Microsoft.Dss.Core.IDssSerializable)(this._Pose)).Serialize(writer);
            writer.Write(((int)(this._TriggerPin)));
        }
        
        /// <summary>
        ///Deserializes ArduinoGenericDistanceSensorState
        ///</summary>
        ///<param name="reader">the reader from which to deserialize</param>
        ///<returns>deserialized ArduinoGenericDistanceSensorState</returns>
        public virtual object Deserialize(System.IO.BinaryReader reader) {
            this._SensorModel = ((global::ArduinoGenericDistanceSensor.Sensors.Proxy.SensorsModels)(reader.ReadInt32()));
            this._HardwareIdentifier = reader.ReadInt32();
            this._Pose = ((global::Microsoft.Robotics.PhysicalModel.Proxy.Pose)(((Microsoft.Dss.Core.IDssSerializable)(new global::Microsoft.Robotics.PhysicalModel.Proxy.Pose())).Deserialize(reader)));
            this._TriggerPin = ((global::Arduino.Firmata.Types.Proxy.Pins)(reader.ReadInt32()));
            return this;
        }
    }
    
    [global::System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    public class Get : global::Microsoft.Dss.ServiceModel.Dssp.Get<global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType, global:: Microsoft.Ccr.Core.PortSet<global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState, global:: W3C.Soap.Fault>> {
        
        public Get() {
        }
        
        public Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body) : 
                base(body) {
        }
        
        public Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body, global::Microsoft.Ccr.Core.PortSet<global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState, global:: W3C.Soap.Fault> responsePort) : 
                base(body, responsePort) {
        }
    }
    
    [global::System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    public class Subscribe : global::Microsoft.Dss.ServiceModel.Dssp.Subscribe<global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType, global:: Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.SubscribeResponseType, global:: W3C.Soap.Fault>> {
        
        public Subscribe() {
        }
        
        public Subscribe(global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType body) : 
                base(body) {
        }
        
        public Subscribe(global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType body, global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.SubscribeResponseType, global:: W3C.Soap.Fault> responsePort) : 
                base(body, responsePort) {
        }
    }
    
    [global::System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    public class ArduinoGenericDistanceSensorOperations : global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup, global:: Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop, global:: ArduinoGenericDistanceSensor.Proxy.Get, global:: ArduinoGenericDistanceSensor.Proxy.Subscribe> {
        
        public ArduinoGenericDistanceSensorOperations() {
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.LookupResponse, global::W3C.Soap.Fault> DsspDefaultLookup() {
            global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType();
            global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice DsspDefaultLookup(out global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup operation) {
            global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType();
            operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.LookupResponse, global::W3C.Soap.Fault> DsspDefaultLookup(global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType body) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType();
            }
            global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice DsspDefaultLookup(global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType body, out global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup operation) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.LookupRequestType();
            }
            operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.DefaultDropResponseType, global::W3C.Soap.Fault> DsspDefaultDrop() {
            global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType();
            global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice DsspDefaultDrop(out global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop operation) {
            global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType();
            operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.DefaultDropResponseType, global::W3C.Soap.Fault> DsspDefaultDrop(global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType body) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType();
            }
            global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice DsspDefaultDrop(global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType body, out global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop operation) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.DropRequestType();
            }
            operation = new global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState, global:: W3C.Soap.Fault> Get() {
            global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            global::ArduinoGenericDistanceSensor.Proxy.Get operation = new global::ArduinoGenericDistanceSensor.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice Get(out global::ArduinoGenericDistanceSensor.Proxy.Get operation) {
            global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            operation = new global::ArduinoGenericDistanceSensor.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorState, global:: W3C.Soap.Fault> Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            }
            global::ArduinoGenericDistanceSensor.Proxy.Get operation = new global::ArduinoGenericDistanceSensor.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body, out global::ArduinoGenericDistanceSensor.Proxy.Get operation) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            }
            operation = new global::ArduinoGenericDistanceSensor.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.SubscribeResponseType, global:: W3C.Soap.Fault> Subscribe(global::Microsoft.Ccr.Core.IPort notificationPort, params System.Type[] types) {
            global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType();
            global::ArduinoGenericDistanceSensor.Proxy.Subscribe operation = new global::ArduinoGenericDistanceSensor.Proxy.Subscribe(body);
            operation.NotificationPort = notificationPort;
            if ((types != null)) {
                body.TypeFilter = new string[types.Length];
                for (int index = 0; (index < types.Length); index = (index + 1)) {
                    body.TypeFilter[index] = global::Microsoft.Dss.ServiceModel.DsspServiceBase.DsspServiceBase.GetTypeFilterDescription(types[index]);
                }
            }
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice Subscribe(global::Microsoft.Ccr.Core.IPort notificationPort, out global::ArduinoGenericDistanceSensor.Proxy.Subscribe operation, params System.Type[] types) {
            global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType();
            operation = new global::ArduinoGenericDistanceSensor.Proxy.Subscribe(body);
            operation.NotificationPort = notificationPort;
            if ((types != null)) {
                body.TypeFilter = new string[types.Length];
                for (int index = 0; (index < types.Length); index = (index + 1)) {
                    body.TypeFilter[index] = global::Microsoft.Dss.ServiceModel.DsspServiceBase.DsspServiceBase.GetTypeFilterDescription(types[index]);
                }
            }
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.SubscribeResponseType, global:: W3C.Soap.Fault> Subscribe(global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType body, global::Microsoft.Ccr.Core.IPort notificationPort, params System.Type[] types) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType();
            }
            global::ArduinoGenericDistanceSensor.Proxy.Subscribe operation = new global::ArduinoGenericDistanceSensor.Proxy.Subscribe(body);
            operation.NotificationPort = notificationPort;
            if ((types != null)) {
                body.TypeFilter = new string[types.Length];
                for (int index = 0; (index < types.Length); index = (index + 1)) {
                    body.TypeFilter[index] = global::Microsoft.Dss.ServiceModel.DsspServiceBase.DsspServiceBase.GetTypeFilterDescription(types[index]);
                }
            }
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice Subscribe(global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType body, global::Microsoft.Ccr.Core.IPort notificationPort, out global::ArduinoGenericDistanceSensor.Proxy.Subscribe operation, params System.Type[] types) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType();
            }
            operation = new global::ArduinoGenericDistanceSensor.Proxy.Subscribe(body);
            operation.NotificationPort = notificationPort;
            if ((types != null)) {
                body.TypeFilter = new string[types.Length];
                for (int index = 0; (index < types.Length); index = (index + 1)) {
                    body.TypeFilter[index] = global::Microsoft.Dss.ServiceModel.DsspServiceBase.DsspServiceBase.GetTypeFilterDescription(types[index]);
                }
            }
            this.Post(operation);
            return operation.ResponsePort;
        }
    }
    
    [global::System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    [global::System.ComponentModel.DisplayNameAttribute("Arduino Generic Distance Sensor")]
    public class Contract {
        
        public const string Identifier = "http://mrdsarduino.codeplex.com/2012/01/arduinogenericdistancesensor.html";
        
        /// <summary>Creates a new instance of the service.</summary>
        /// <param name="constructorServicePort">Service constructor port</param>
        /// <param name="service">service path</param>
        /// <param name="partners">the partners of the service instance</param>
        /// <returns>create service response port</returns>
        public static global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse> CreateService(global::Microsoft.Dss.Services.Constructor.ConstructorPort constructorServicePort, string service, params Microsoft.Dss.ServiceModel.Dssp.PartnerType[] partners) {
            global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse> result = new global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse>();
            global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType serviceInfo = new global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType("http://mrdsarduino.codeplex.com/2012/01/arduinogenericdistancesensor.html", service);
            if ((partners != null)) {
                serviceInfo.PartnerList = new System.Collections.Generic.List<Microsoft.Dss.ServiceModel.Dssp.PartnerType>(partners);
            }
            global::Microsoft.Dss.Services.Constructor.Create create = new global::Microsoft.Dss.Services.Constructor.Create(serviceInfo, result);
            constructorServicePort.Post(create);
            return result;
        }
        
        /// <summary>Creates a new instance of the service.</summary>
        /// <param name="constructorServicePort">Service constructor port</param>
        /// <param name="partners">the partners of the service instance</param>
        /// <returns>create service response port</returns>
        public static global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse> CreateService(global::Microsoft.Dss.Services.Constructor.ConstructorPort constructorServicePort, params Microsoft.Dss.ServiceModel.Dssp.PartnerType[] partners) {
            global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse> result = new global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse>();
            global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType serviceInfo = new global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType("http://mrdsarduino.codeplex.com/2012/01/arduinogenericdistancesensor.html", null);
            if ((partners != null)) {
                serviceInfo.PartnerList = new System.Collections.Generic.List<Microsoft.Dss.ServiceModel.Dssp.PartnerType>(partners);
            }
            global::Microsoft.Dss.Services.Constructor.Create create = new global::Microsoft.Dss.Services.Constructor.Create(serviceInfo, result);
            constructorServicePort.Post(create);
            return result;
        }
    }
    
    public class CombinedOperationsPort : global::Microsoft.Dss.Core.DssCombinedOperationsPort {
        
        public CombinedOperationsPort() {
            this.ArduinoGenericDistanceSensorOperations = new global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorOperations();
            this.AnalogSensorOperations = new global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorOperations();
            this.SonarOperations = new global::Microsoft.Robotics.Services.Sonar.Proxy.SonarOperations();
            this.InfraredOperations = new global::Microsoft.Robotics.Services.Infrared.Proxy.InfraredOperations();
            base.Initialize(new global::Microsoft.Dss.Core.DssOperationsPortMetadata(this.ArduinoGenericDistanceSensorOperations, "http://mrdsarduino.codeplex.com/2012/01/arduinogenericdistancesensor.html", "ArduinoGenericDistanceSensorOperations", ""), new global::Microsoft.Dss.Core.DssOperationsPortMetadata(this.AnalogSensorOperations, "http://schemas.microsoft.com/robotics/2006/06/analogsensor.html", "AnalogSensorOperations", null), new global::Microsoft.Dss.Core.DssOperationsPortMetadata(this.SonarOperations, "http://schemas.microsoft.com/robotics/2006/06/sonar.html", "SonarOperations", null), new global::Microsoft.Dss.Core.DssOperationsPortMetadata(this.InfraredOperations, "http://schemas.microsoft.com/robotics/2011/10/infrared.html", "InfraredOperations", null));
        }
        
        public global::ArduinoGenericDistanceSensor.Proxy.ArduinoGenericDistanceSensorOperations ArduinoGenericDistanceSensorOperations;
        
        public global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorOperations AnalogSensorOperations;
        
        public global::Microsoft.Robotics.Services.Sonar.Proxy.SonarOperations SonarOperations;
        
        public global::Microsoft.Robotics.Services.Infrared.Proxy.InfraredOperations InfraredOperations;
    }
}
namespace ArduinoGenericDistanceSensor.Sensors.Proxy {
    
    
    [global::Microsoft.Dss.Core.Attributes.DataContractAttribute(Namespace="ArduinoGenericDistanceSensor")]
    [global::System.Xml.Serialization.XmlRootAttribute(Namespace="ArduinoGenericDistanceSensor", ElementName="SensorsModels")]
    public enum SensorsModels : int {
        
        None = 0,
        
        IR_Sharp_GP2Y0A21YK0F = 1,
        
        Sonar_HC_SR = 2,
    }
    
    [global::System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    public class Contract {
        
        public const string Identifier = "ArduinoGenericDistanceSensor";
    }
}
