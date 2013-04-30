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
[assembly: global::System.Reflection.AssemblyProductAttribute("Wanderer")]
[assembly: global::System.Reflection.AssemblyTitleAttribute("Wanderer")]
[assembly: global::Microsoft.Dss.Core.Attributes.ServiceDeclarationAttribute(global::Microsoft.Dss.Core.Attributes.DssServiceDeclaration.Proxy, SourceAssemblyKey="POFerro.Robotics.Wanderer.Y2013.M02, Version=1.0.0.0, Culture=neutral, PublicKeyT" +
    "oken=e3a6d1d8ea7297f8")]
[assembly: global::System.Security.SecurityTransparentAttribute()]
[assembly: global::System.Security.SecurityRulesAttribute(global::System.Security.SecurityRuleSet.Level1)]

namespace POFerro.Robotics.Wanderer.Proxy {
    
    
    [global::Microsoft.Dss.Core.Attributes.DataContractAttribute(Namespace="http://schemas.pferro/2013/02/wanderer.html")]
    [global::System.Xml.Serialization.XmlRootAttribute(Namespace="http://schemas.pferro/2013/02/wanderer.html", ElementName="WandererState")]
    public class WandererState : global::Microsoft.Dss.Core.IDssSerializable, global::System.ICloneable {
        
        public WandererState() {
        }
        
        private double _IRSafeDistance;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public double IRSafeDistance {
            get {
                return this._IRSafeDistance;
            }
            set {
                this._IRSafeDistance = value;
            }
        }
        
        private double _IRDistanceDiferenceToAdjust;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public double IRDistanceDiferenceToAdjust {
            get {
                return this._IRDistanceDiferenceToAdjust;
            }
            set {
                this._IRDistanceDiferenceToAdjust = value;
            }
        }
        
        private global::POFerro.Robotics.Wanderer.Proxy.WandererLogicalState _CurrentState;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public global::POFerro.Robotics.Wanderer.Proxy.WandererLogicalState CurrentState {
            get {
                return this._CurrentState;
            }
            set {
                this._CurrentState = value;
            }
        }
        
        private global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState _LastLeftIRReading;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState LastLeftIRReading {
            get {
                return this._LastLeftIRReading;
            }
            set {
                this._LastLeftIRReading = value;
            }
        }
        
        private global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState _LastRightIRReading;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState LastRightIRReading {
            get {
                return this._LastRightIRReading;
            }
            set {
                this._LastRightIRReading = value;
            }
        }
        
        private global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState _LastSonarReading;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState LastSonarReading {
            get {
                return this._LastSonarReading;
            }
            set {
                this._LastSonarReading = value;
            }
        }
        
        private global::POFerro.Robotics.ArduinoSonarTurret.Proxy.ArduinoSonarTurretState _LastTurretReading;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public global::POFerro.Robotics.ArduinoSonarTurret.Proxy.ArduinoSonarTurretState LastTurretReading {
            get {
                return this._LastTurretReading;
            }
            set {
                this._LastTurretReading = value;
            }
        }
        
        private int _BestAngle;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public int BestAngle {
            get {
                return this._BestAngle;
            }
            set {
                this._BestAngle = value;
            }
        }
        
        private double _LeftWheelPower;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public double LeftWheelPower {
            get {
                return this._LeftWheelPower;
            }
            set {
                this._LeftWheelPower = value;
            }
        }
        
        private double _RightWheelPower;
        
        [global::Microsoft.Dss.Core.Attributes.DataMemberAttribute(Order=-1)]
        public double RightWheelPower {
            get {
                return this._RightWheelPower;
            }
            set {
                this._RightWheelPower = value;
            }
        }
        
        /// <summary>
        ///Copies the data member values of the current WandererState to the specified target object
        ///</summary>
        ///<param name="target">target object (must be an instance of)</param>
        public virtual void CopyTo(Microsoft.Dss.Core.IDssSerializable target) {
            global::POFerro.Robotics.Wanderer.Proxy.WandererState typedTarget = ((global::POFerro.Robotics.Wanderer.Proxy.WandererState)(target));
            typedTarget._IRSafeDistance = this._IRSafeDistance;
            typedTarget._IRDistanceDiferenceToAdjust = this._IRDistanceDiferenceToAdjust;
            typedTarget._CurrentState = this._CurrentState;
            if ((this._LastLeftIRReading != null)) {
                global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState tmp = new global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState();
                ((Microsoft.Dss.Core.IDssSerializable)(this._LastLeftIRReading)).CopyTo(((Microsoft.Dss.Core.IDssSerializable)(tmp)));
                typedTarget._LastLeftIRReading = tmp;
            }
            if ((this._LastRightIRReading != null)) {
                global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState tmp0 = new global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState();
                ((Microsoft.Dss.Core.IDssSerializable)(this._LastRightIRReading)).CopyTo(((Microsoft.Dss.Core.IDssSerializable)(tmp0)));
                typedTarget._LastRightIRReading = tmp0;
            }
            if ((this._LastSonarReading != null)) {
                global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState tmp1 = new global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState();
                ((Microsoft.Dss.Core.IDssSerializable)(this._LastSonarReading)).CopyTo(((Microsoft.Dss.Core.IDssSerializable)(tmp1)));
                typedTarget._LastSonarReading = tmp1;
            }
            if ((this._LastTurretReading != null)) {
                global::POFerro.Robotics.ArduinoSonarTurret.Proxy.ArduinoSonarTurretState tmp2 = new global::POFerro.Robotics.ArduinoSonarTurret.Proxy.ArduinoSonarTurretState();
                ((Microsoft.Dss.Core.IDssSerializable)(this._LastTurretReading)).CopyTo(((Microsoft.Dss.Core.IDssSerializable)(tmp2)));
                typedTarget._LastTurretReading = tmp2;
            }
            typedTarget._BestAngle = this._BestAngle;
            typedTarget._LeftWheelPower = this._LeftWheelPower;
            typedTarget._RightWheelPower = this._RightWheelPower;
        }
        
        /// <summary>
        ///Clones WandererState
        ///</summary>
        ///<returns>cloned value</returns>
        public virtual object Clone() {
            global::POFerro.Robotics.Wanderer.Proxy.WandererState target0 = new global::POFerro.Robotics.Wanderer.Proxy.WandererState();
            this.CopyTo(target0);
            return target0;
        }
        
        /// <summary>
        ///Serializes the data member values of the current WandererState to the specified writer
        ///</summary>
        ///<param name="writer">the writer to which to serialize</param>
        public virtual void Serialize(System.IO.BinaryWriter writer) {
            writer.Write(this._IRSafeDistance);
            writer.Write(this._IRDistanceDiferenceToAdjust);
            writer.Write(((int)(this._CurrentState)));
            if ((this._LastLeftIRReading == null)) {
                writer.Write(((byte)(0)));
            }
            else {
                writer.Write(((byte)(1)));
                ((Microsoft.Dss.Core.IDssSerializable)(this._LastLeftIRReading)).Serialize(writer);
            }
            if ((this._LastRightIRReading == null)) {
                writer.Write(((byte)(0)));
            }
            else {
                writer.Write(((byte)(1)));
                ((Microsoft.Dss.Core.IDssSerializable)(this._LastRightIRReading)).Serialize(writer);
            }
            if ((this._LastSonarReading == null)) {
                writer.Write(((byte)(0)));
            }
            else {
                writer.Write(((byte)(1)));
                ((Microsoft.Dss.Core.IDssSerializable)(this._LastSonarReading)).Serialize(writer);
            }
            if ((this._LastTurretReading == null)) {
                writer.Write(((byte)(0)));
            }
            else {
                writer.Write(((byte)(1)));
                ((Microsoft.Dss.Core.IDssSerializable)(this._LastTurretReading)).Serialize(writer);
            }
            writer.Write(this._BestAngle);
            writer.Write(this._LeftWheelPower);
            writer.Write(this._RightWheelPower);
        }
        
        /// <summary>
        ///Deserializes WandererState
        ///</summary>
        ///<param name="reader">the reader from which to deserialize</param>
        ///<returns>deserialized WandererState</returns>
        public virtual object Deserialize(System.IO.BinaryReader reader) {
            this._IRSafeDistance = reader.ReadDouble();
            this._IRDistanceDiferenceToAdjust = reader.ReadDouble();
            this._CurrentState = ((global::POFerro.Robotics.Wanderer.Proxy.WandererLogicalState)(reader.ReadInt32()));
            if ((reader.ReadByte() != 0)) {
                this._LastLeftIRReading = ((global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState)(((Microsoft.Dss.Core.IDssSerializable)(new global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState())).Deserialize(reader)));
            }
            if ((reader.ReadByte() != 0)) {
                this._LastRightIRReading = ((global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState)(((Microsoft.Dss.Core.IDssSerializable)(new global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState())).Deserialize(reader)));
            }
            if ((reader.ReadByte() != 0)) {
                this._LastSonarReading = ((global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState)(((Microsoft.Dss.Core.IDssSerializable)(new global::Microsoft.Robotics.Services.AnalogSensor.Proxy.AnalogSensorState())).Deserialize(reader)));
            }
            if ((reader.ReadByte() != 0)) {
                this._LastTurretReading = ((global::POFerro.Robotics.ArduinoSonarTurret.Proxy.ArduinoSonarTurretState)(((Microsoft.Dss.Core.IDssSerializable)(new global::POFerro.Robotics.ArduinoSonarTurret.Proxy.ArduinoSonarTurretState())).Deserialize(reader)));
            }
            this._BestAngle = reader.ReadInt32();
            this._LeftWheelPower = reader.ReadDouble();
            this._RightWheelPower = reader.ReadDouble();
            return this;
        }
    }
    
    [global::System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    public class Get : global::Microsoft.Dss.ServiceModel.Dssp.Get<global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType, global:: Microsoft.Ccr.Core.PortSet<global::POFerro.Robotics.Wanderer.Proxy.WandererState, global:: W3C.Soap.Fault>> {
        
        public Get() {
        }
        
        public Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body) : 
                base(body) {
        }
        
        public Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body, global::Microsoft.Ccr.Core.PortSet<global::POFerro.Robotics.Wanderer.Proxy.WandererState, global:: W3C.Soap.Fault> responsePort) : 
                base(body, responsePort) {
        }
    }
    
    [global::System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    public class StateChangeNotify : global::Microsoft.Dss.ServiceModel.Dssp.Update<global::POFerro.Robotics.Wanderer.Proxy.WandererState, global:: Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.DefaultUpdateResponseType, global:: W3C.Soap.Fault>> {
        
        public StateChangeNotify() {
        }
        
        public StateChangeNotify(global::POFerro.Robotics.Wanderer.Proxy.WandererState body) : 
                base(body) {
        }
        
        public StateChangeNotify(global::POFerro.Robotics.Wanderer.Proxy.WandererState body, global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.DefaultUpdateResponseType, global:: W3C.Soap.Fault> responsePort) : 
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
    public class WandererOperations : global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.DsspDefaultLookup, global:: Microsoft.Dss.ServiceModel.Dssp.DsspDefaultDrop, global:: POFerro.Robotics.Wanderer.Proxy.Get, global:: POFerro.Robotics.Wanderer.Proxy.StateChangeNotify, global:: POFerro.Robotics.Wanderer.Proxy.Subscribe> {
        
        public WandererOperations() {
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
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::POFerro.Robotics.Wanderer.Proxy.WandererState, global:: W3C.Soap.Fault> Get() {
            global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            global::POFerro.Robotics.Wanderer.Proxy.Get operation = new global::POFerro.Robotics.Wanderer.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice Get(out global::POFerro.Robotics.Wanderer.Proxy.Get operation) {
            global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            operation = new global::POFerro.Robotics.Wanderer.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::POFerro.Robotics.Wanderer.Proxy.WandererState, global:: W3C.Soap.Fault> Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            }
            global::POFerro.Robotics.Wanderer.Proxy.Get operation = new global::POFerro.Robotics.Wanderer.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice Get(global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType body, out global::POFerro.Robotics.Wanderer.Proxy.Get operation) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.GetRequestType();
            }
            operation = new global::POFerro.Robotics.Wanderer.Proxy.Get(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.DefaultUpdateResponseType, global:: W3C.Soap.Fault> StateChangeNotify() {
            global::POFerro.Robotics.Wanderer.Proxy.WandererState body = new global::POFerro.Robotics.Wanderer.Proxy.WandererState();
            global::POFerro.Robotics.Wanderer.Proxy.StateChangeNotify operation = new global::POFerro.Robotics.Wanderer.Proxy.StateChangeNotify(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice StateChangeNotify(out global::POFerro.Robotics.Wanderer.Proxy.StateChangeNotify operation) {
            global::POFerro.Robotics.Wanderer.Proxy.WandererState body = new global::POFerro.Robotics.Wanderer.Proxy.WandererState();
            operation = new global::POFerro.Robotics.Wanderer.Proxy.StateChangeNotify(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.DefaultUpdateResponseType, global:: W3C.Soap.Fault> StateChangeNotify(global::POFerro.Robotics.Wanderer.Proxy.WandererState body) {
            if ((body == null)) {
                body = new global::POFerro.Robotics.Wanderer.Proxy.WandererState();
            }
            global::POFerro.Robotics.Wanderer.Proxy.StateChangeNotify operation = new global::POFerro.Robotics.Wanderer.Proxy.StateChangeNotify(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.Choice StateChangeNotify(global::POFerro.Robotics.Wanderer.Proxy.WandererState body, out global::POFerro.Robotics.Wanderer.Proxy.StateChangeNotify operation) {
            if ((body == null)) {
                body = new global::POFerro.Robotics.Wanderer.Proxy.WandererState();
            }
            operation = new global::POFerro.Robotics.Wanderer.Proxy.StateChangeNotify(body);
            this.Post(operation);
            return operation.ResponsePort;
        }
        
        public virtual global::Microsoft.Ccr.Core.PortSet<global::Microsoft.Dss.ServiceModel.Dssp.SubscribeResponseType, global:: W3C.Soap.Fault> Subscribe(global::Microsoft.Ccr.Core.IPort notificationPort, params System.Type[] types) {
            global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType();
            global::POFerro.Robotics.Wanderer.Proxy.Subscribe operation = new global::POFerro.Robotics.Wanderer.Proxy.Subscribe(body);
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
        
        public virtual global::Microsoft.Ccr.Core.Choice Subscribe(global::Microsoft.Ccr.Core.IPort notificationPort, out global::POFerro.Robotics.Wanderer.Proxy.Subscribe operation, params System.Type[] types) {
            global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType body = new global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType();
            operation = new global::POFerro.Robotics.Wanderer.Proxy.Subscribe(body);
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
            global::POFerro.Robotics.Wanderer.Proxy.Subscribe operation = new global::POFerro.Robotics.Wanderer.Proxy.Subscribe(body);
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
        
        public virtual global::Microsoft.Ccr.Core.Choice Subscribe(global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType body, global::Microsoft.Ccr.Core.IPort notificationPort, out global::POFerro.Robotics.Wanderer.Proxy.Subscribe operation, params System.Type[] types) {
            if ((body == null)) {
                body = new global::Microsoft.Dss.ServiceModel.Dssp.SubscribeRequestType();
            }
            operation = new global::POFerro.Robotics.Wanderer.Proxy.Subscribe(body);
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
    
    [global::Microsoft.Dss.Core.Attributes.DataContractAttribute(Namespace="http://schemas.pferro/2013/02/wanderer.html")]
    [global::System.Xml.Serialization.XmlRootAttribute(Namespace="http://schemas.pferro/2013/02/wanderer.html", ElementName="WandererLogicalState")]
    public enum WandererLogicalState : int {
        
        Unknown = 0,
        
        Ranging = 1,
        
        TurnLeft = 2,
        
        TurnRight = 3,
        
        AdjustLeft = 4,
        
        AdjustRight = 5,
        
        MoveForward = 6,
        
        ReverseLeft = 7,
        
        ReverseRight = 8,
    }
    
    [global::System.Xml.Serialization.XmlTypeAttribute(IncludeInSchema=false)]
    [global::System.ComponentModel.DescriptionAttribute("Wanderer service (no description provided)")]
    [global::System.ComponentModel.DisplayNameAttribute("Wanderer")]
    public class Contract {
        
        public const string Identifier = "http://schemas.pferro/2013/02/wanderer.html";
        
        /// <summary>Creates a new instance of the service.</summary>
        /// <param name="constructorServicePort">Service constructor port</param>
        /// <param name="service">service path</param>
        /// <param name="partners">the partners of the service instance</param>
        /// <returns>create service response port</returns>
        public static global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse> CreateService(global::Microsoft.Dss.Services.Constructor.ConstructorPort constructorServicePort, string service, params Microsoft.Dss.ServiceModel.Dssp.PartnerType[] partners) {
            global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse> result = new global::Microsoft.Dss.ServiceModel.Dssp.DsspResponsePort<Microsoft.Dss.ServiceModel.Dssp.CreateResponse>();
            global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType serviceInfo = new global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType("http://schemas.pferro/2013/02/wanderer.html", service);
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
            global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType serviceInfo = new global::Microsoft.Dss.ServiceModel.Dssp.ServiceInfoType("http://schemas.pferro/2013/02/wanderer.html", null);
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
            this.WandererOperations = new global::POFerro.Robotics.Wanderer.Proxy.WandererOperations();
            base.Initialize(new global::Microsoft.Dss.Core.DssOperationsPortMetadata(this.WandererOperations, "http://schemas.pferro/2013/02/wanderer.html", "WandererOperations", ""));
        }
        
        public global::POFerro.Robotics.Wanderer.Proxy.WandererOperations WandererOperations;
    }
}
